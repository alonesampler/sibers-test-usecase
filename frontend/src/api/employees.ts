import type { Employee, ParamsEmployeeRequest } from '../types'
import { API_BASE, request } from './http'

export function getEmployees() {
  return request<Employee[]>(API_BASE, '/employees')
}

export function searchEmployees(query: string) {
  const params = new URLSearchParams({ query })
  return request<Employee[]>(API_BASE, `/employees/search?${params}`)
}

export function getEmployee(id: string) {
  return request<Employee>(API_BASE, `/employees/${id}`)
}

export function createEmployee(body: ParamsEmployeeRequest) {
  return request<void>(API_BASE, '/employees', {
    method: 'POST',
    body: JSON.stringify(body),
  })
}

export function updateEmployee(id: string, body: ParamsEmployeeRequest) {
  return request<void>(API_BASE, `/employees/${id}`, {
    method: 'PUT',
    body: JSON.stringify(body),
  })
}

export function deleteEmployee(id: string) {
  return request<void>(API_BASE, `/employees/${id}`, { method: 'DELETE' })
}