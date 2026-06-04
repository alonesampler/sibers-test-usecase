import {
  createContext,
  useCallback,
  useContext,
  useMemo,
  useState,
  type ReactNode,
} from 'react'
import * as authApi from '../api/auth'
import { setToken } from '../api/http'
import type { AppRole, AuthSession } from '../types'
import { hasRole, loadSession, parseSession } from './session'

interface AuthContextValue {
  session: AuthSession | null
  login: (email: string, password: string) => Promise<void>
  logout: () => void
  refresh: () => void
  isRole: (...roles: AppRole[]) => boolean
}

const AuthContext = createContext<AuthContextValue | null>(null)

export function AuthProvider({ children }: { children: ReactNode }) {
  const [session, setSession] = useState<AuthSession | null>(() => loadSession())

  const refresh = useCallback(() => {
    setSession(loadSession())
  }, [])

  const login = useCallback(async (email: string, password: string) => {
    const token = await authApi.login({ email, password })
    const parsed = parseSession(token)
    if (!parsed) throw new Error('Invalid token received')
    setSession(parsed)
  }, [])

  const logout = useCallback(() => {
    authApi.logout()
    setToken(null)
    setSession(null)
  }, [])

  const isRole = useCallback(
    (...roles: AppRole[]) => (session ? hasRole(session, ...roles) : false),
    [session],
  )

  const value = useMemo(
    () => ({ session, login, logout, refresh, isRole }),
    [session, login, logout, refresh, isRole],
  )

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>
}

export function useAuth() {
  const ctx = useContext(AuthContext)
  if (!ctx) throw new Error('useAuth must be used within AuthProvider')
  return ctx
}