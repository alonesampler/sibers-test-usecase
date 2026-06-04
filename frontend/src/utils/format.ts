import type { Employee } from '../types'

export function fullName(e: Pick<Employee, 'name' | 'surname' | 'patronymic'>): string {
  return [e.surname, e.name, e.patronymic].filter(Boolean).join(' ')
}

export function formatDate(value: string): string {
  return new Date(value).toLocaleDateString('ru-RU')
}

export function formatDateTime(value: string): string {
  return new Date(value).toLocaleString('ru-RU')
}