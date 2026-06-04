import type { ParamsProjectTaskRequest, ProjectTask, ProjectTaskStatus } from '../types'

const STATUS_BY_NUMBER: Record<number, ProjectTaskStatus> = {
  1: 'ToDo',
  2: 'InProgress',
  3: 'Done',
}

const STATUS_TO_NUMBER: Record<ProjectTaskStatus, number> = {
  ToDo: 1,
  InProgress: 2,
  Done: 3,
}

export function normalizeTaskStatus(
  status: ProjectTaskStatus | number | string,
): ProjectTaskStatus {
  if (typeof status === 'number') return STATUS_BY_NUMBER[status] ?? 'ToDo'
  if (status === 'ToDo' || status === 'InProgress' || status === 'Done') return status
  const numeric = Number(status)
  if (!Number.isNaN(numeric)) return STATUS_BY_NUMBER[numeric] ?? 'ToDo'
  return 'ToDo'
}

export function normalizeProjectTask(task: ProjectTask): ProjectTask {
  return { ...task, status: normalizeTaskStatus(task.status) }
}

export function normalizeProjectTasks(tasks: ProjectTask[]): ProjectTask[] {
  return tasks.map(normalizeProjectTask)
}

/** Backend сериализует enum как число (1, 2, 3), не как строку. */
export function serializeTaskRequest(body: ParamsProjectTaskRequest) {
  const { status, ...rest } = body
  return {
    ...rest,
    ...(status !== undefined ? { status: STATUS_TO_NUMBER[status] } : {}),
  }
}

export function taskStatusToQueryValue(status: ProjectTaskStatus): string {
  return String(STATUS_TO_NUMBER[status])
}