import type { Employee } from '../types'
import { fullName } from './format'

export function employeeMatchesQuery(employee: Employee, query: string): boolean {
  const q = query.trim().toLowerCase()
  if (!q) return true
  const haystack = `${fullName(employee)} ${employee.email}`.toLowerCase()
  return haystack.includes(q)
}