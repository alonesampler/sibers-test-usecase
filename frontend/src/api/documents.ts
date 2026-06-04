import type { Document } from '../types'
import { API_BASE, ApiError, getToken, request } from './http'

export function getDocuments(projectId: string) {
  const params = new URLSearchParams({ projectId })
  return request<Document[]>(API_BASE, `/documents?${params}`, {}, false)
}

export async function uploadDocument(projectId: string, file: File) {
  const form = new FormData()
  form.append('file', file)
  const params = new URLSearchParams({ projectId })
  const headers = new Headers()
  const token = getToken()
  if (token) headers.set('Authorization', `Bearer ${token}`)

  const response = await fetch(`${API_BASE}/documents?${params}`, {
    method: 'POST',
    body: form,
    headers,
  })
  if (!response.ok) {
    const body = await response.json().catch(() => null)
    throw new ApiError(
      typeof body === 'string' ? body : 'Не удалось загрузить файл',
      response.status,
      body,
    )
  }
}

export function downloadDocument(id: string) {
  const token = getToken()
  const url = `${API_BASE}/documents/${id}/download`
  return fetch(url, {
    headers: token ? { Authorization: `Bearer ${token}` } : {},
  })
}

export function deleteDocument(id: string) {
  return request<void>(API_BASE, `/documents/${id}`, { method: 'DELETE' }, false)
}