import type { AppRole, AuthSession } from '../types'
import { getToken } from '../api/http'

const ROLE_CLAIM =
  'http://schemas.microsoft.com/ws/2008/06/identity/claims/role'
const EMAIL_CLAIM =
  'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress'

interface JwtPayload {
  email?: string
  employeeId?: string
  role?: string | string[]
  [key: string]: unknown
}

function decodeJwtPayload(token: string): JwtPayload | null {
  const segment = token.split('.')[1]
  if (!segment) return null
  const base64 = segment.replace(/-/g, '+').replace(/_/g, '/')
  const padded = base64 + '='.repeat((4 - (base64.length % 4)) % 4)
  try {
    return JSON.parse(atob(padded)) as JwtPayload
  } catch {
    return null
  }
}

function parseRoles(payload: JwtPayload): AppRole[] {
  const raw =
    payload.role ??
    payload[ROLE_CLAIM] ??
    payload['roles']
  const list = Array.isArray(raw) ? raw : raw ? [String(raw)] : []
  return list.filter((r): r is AppRole =>
    ['Director', 'ProjectManager', 'Employee'].includes(r),
  )
}

export function parseSession(token: string): AuthSession | null {
  try {
    const payload = decodeJwtPayload(token)
    if (!payload) return null

    const email =
      (typeof payload.email === 'string' && payload.email) ||
      (typeof payload[EMAIL_CLAIM] === 'string' && payload[EMAIL_CLAIM]) ||
      undefined
    const employeeId =
      typeof payload.employeeId === 'string' ? payload.employeeId : undefined
    if (!email || !employeeId) return null
    return {
      token,
      email,
      employeeId,
      roles: parseRoles(payload),
    }
  } catch {
    return null
  }
}

export function loadSession(): AuthSession | null {
  const token = getToken()
  if (!token) return null
  return parseSession(token)
}

export function hasRole(session: AuthSession, ...roles: AppRole[]): boolean {
  return roles.some((r) => session.roles.includes(r))
}