const TOKEN_KEY = 'sb_token'

export function getToken(): string | null {
  return localStorage.getItem(TOKEN_KEY)
}

export function setToken(token: string | null): void {
  if (token) localStorage.setItem(TOKEN_KEY, token)
  else localStorage.removeItem(TOKEN_KEY)
}

export class ApiError extends Error {
  status: number
  details?: unknown

  constructor(message: string, status: number, details?: unknown) {
    super(message)
    this.name = 'ApiError'
    this.status = status
    this.details = details
  }
}

function formatFluentError(item: unknown): string | null {
  if (typeof item === 'string') return item
  if (!item || typeof item !== 'object') return null
  const record = item as Record<string, unknown>
  if (typeof record.message === 'string' && record.message) return record.message
  if (Array.isArray(record.reasons) && record.reasons.length > 0) {
    const nested = record.reasons
      .map(formatFluentError)
      .filter((x): x is string => Boolean(x))
    if (nested.length) return nested.join('; ')
  }
  return null
}

function formatErrorBody(body: unknown): string {
  if (!body) return 'Request failed'
  if (typeof body === 'string') return body
  if (Array.isArray(body)) {
    const messages = body.map(formatFluentError).filter((x): x is string => Boolean(x))
    return messages.length ? messages.join('; ') : JSON.stringify(body)
  }
  if (typeof body === 'object' && body !== null) {
    const formatted = formatFluentError(body)
    if (formatted) return formatted
    if ('title' in body) return String((body as { title: unknown }).title)
  }
  return JSON.stringify(body)
}

export async function request<T>(
  baseUrl: string,
  path: string,
  init: RequestInit = {},
  auth = true,
): Promise<T> {
  const headers = new Headers(init.headers)
  if (auth) {
    const token = getToken()
    if (token) headers.set('Authorization', `Bearer ${token}`)
  }
  if (init.body && !(init.body instanceof FormData) && !headers.has('Content-Type')) {
    headers.set('Content-Type', 'application/json')
  }

  let response: Response
  try {
    response = await fetch(`${baseUrl}${path}`, { ...init, headers })
  } catch {
    throw new ApiError(
      'Сервер недоступен. Запустите backend: из корня репозитория выполните .\\scripts\\start-backend.ps1 (нужны порты 5182 и 5123).',
      0,
    )
  }
  if (response.status === 204 || response.status === 201) {
    if (!response.headers.get('content-type')?.includes('application/json')) {
      return undefined as T
    }
  }

  const contentType = response.headers.get('content-type') ?? ''
  const isJson = contentType.includes('application/json')
  const body = isJson ? await response.json().catch(() => null) : await response.text()

  if (!response.ok) {
    throw new ApiError(formatErrorBody(body), response.status, body)
  }

  return body as T
}

export const AUTH_BASE = '/auth-api/api/v1'
export const API_BASE = '/api/v1'