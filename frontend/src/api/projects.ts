import type {
  ParamsProjectRequest,
  Project,
  ProjectFilter,
  ProjectSort,
} from '../types'
import { API_BASE, request } from './http'

function buildQuery(
  filter: ProjectFilter,
  sort: ProjectSort,
): string {
  const params = new URLSearchParams({ sort })
  const entries: [keyof ProjectFilter, string | number | undefined][] = [
    ['startDateFrom', filter.startDateFrom],
    ['startDateTo', filter.startDateTo],
    ['priorityMin', filter.priorityMin],
    ['priorityMax', filter.priorityMax],
    ['name', filter.name],
    ['customerCompanyName', filter.customerCompanyName],
    ['executorCompanyName', filter.executorCompanyName],
  ]
  for (const [key, value] of entries) {
    if (value !== undefined && value !== '') params.set(key, String(value))
  }
  return params.toString()
}

export function getProjects(filter: ProjectFilter, sort: ProjectSort) {
  return request<Project[]>(API_BASE, `/projects?${buildQuery(filter, sort)}`)
}

export function getProject(id: string) {
  return request<Project>(API_BASE, `/projects/${id}`)
}

function isSameProject(p: Project, body: ParamsProjectRequest): boolean {
  const team = new Set(p.employees.map((e) => e.id))
  const expected = new Set(body.employeeIds)
  if (team.size !== expected.size) return false
  for (const id of expected) {
    if (!team.has(id)) return false
  }
  return (
    p.name === body.name &&
    p.customerCompanyName === body.customerCompanyName &&
    p.executorCompanyName === body.executorCompanyName &&
    p.startDate === body.startDate &&
    p.endDate === body.endDate &&
    p.priority === body.priority &&
    p.manager.id === body.managerId
  )
}

/** POST не возвращает id — находим проект в списке по полям. */
export async function createProject(body: ParamsProjectRequest): Promise<string> {
  await request<void>(API_BASE, '/projects', {
    method: 'POST',
    body: JSON.stringify(body),
  })

  const list = await getProjects({ name: body.name }, 'StartDateDesc')
  const match = list.find((p) => isSameProject(p, body))
  if (!match) {
    throw new Error('Проект создан, но не найден в списке. Обновите страницу проектов.')
  }
  return match.id
}

export function updateProject(id: string, body: ParamsProjectRequest) {
  return request<void>(API_BASE, `/projects/${id}`, {
    method: 'PUT',
    body: JSON.stringify(body),
  })
}

export function deleteProject(id: string) {
  return request<void>(API_BASE, `/projects/${id}`, { method: 'DELETE' })
}