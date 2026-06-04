import type {
  ParamsProjectTaskRequest,
  ProjectTask,
  ProjectTaskFilter,
  TaskSort,
} from '../types'
import { API_BASE, request } from './http'
import {
  normalizeProjectTask,
  normalizeProjectTasks,
  serializeTaskRequest,
  taskStatusToQueryValue,
} from './normalize'

function buildTaskQuery(filter: ProjectTaskFilter, sort: TaskSort): string {
  const params = new URLSearchParams({ sort })
  const entries: [keyof ProjectTaskFilter, string | number | undefined][] = [
    ['status', filter.status ? taskStatusToQueryValue(filter.status) : undefined],
    ['priorityMin', filter.priorityMin],
    ['priorityMax', filter.priorityMax],
    ['assigneeId', filter.assigneeId],
    ['authorId', filter.authorId],
  ]
  for (const [key, value] of entries) {
    if (value !== undefined && value !== '') params.set(key, String(value))
  }
  return params.toString()
}

export async function getProjectTasks(
  projectId: string,
  filter: ProjectTaskFilter,
  sort: TaskSort,
) {
  const tasks = await request<ProjectTask[]>(
    API_BASE,
    `/projects/${projectId}/tasks?${buildTaskQuery(filter, sort)}`,
  )
  return normalizeProjectTasks(tasks)
}

export async function getTask(id: string) {
  const task = await request<ProjectTask>(API_BASE, `/tasks/${id}`)
  return normalizeProjectTask(task)
}

export function createTask(projectId: string, body: ParamsProjectTaskRequest) {
  return request<void>(API_BASE, `/projects/${projectId}/tasks`, {
    method: 'POST',
    body: JSON.stringify(serializeTaskRequest(body)),
  })
}

export function updateTask(id: string, body: ParamsProjectTaskRequest) {
  return request<void>(API_BASE, `/tasks/${id}`, {
    method: 'PUT',
    body: JSON.stringify(serializeTaskRequest(body)),
  })
}

export function deleteTask(id: string) {
  return request<void>(API_BASE, `/tasks/${id}`, { method: 'DELETE' })
}