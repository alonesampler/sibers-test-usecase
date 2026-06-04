import type { UserAccount } from '../types'
import { AUTH_BASE, request, setToken } from './http'

export interface LoginPayload {
  email: string
  password: string
}

export interface RegisterPayload {
  employeeId: string
  email: string
  password: string
  role: string
}

export async function login(payload: LoginPayload): Promise<string> {
  const data = await request<{ token: string }>(
    AUTH_BASE,
    '/auth/login',
    { method: 'POST', body: JSON.stringify(payload) },
    false,
  )
  setToken(data.token)
  return data.token
}

export async function register(payload: RegisterPayload): Promise<void> {
  await request<void>(AUTH_BASE, '/auth/register', {
    method: 'POST',
    body: JSON.stringify(payload),
  })
}

export async function getUserAccounts(): Promise<UserAccount[]> {
  return request<UserAccount[]>(AUTH_BASE, '/auth/user-accounts')
}

export function logout(): void {
  setToken(null)
}