export type AppRole = 'Director' | 'ProjectManager' | 'Employee'

export type ProjectTaskStatus = 'ToDo' | 'InProgress' | 'Done'

export type ProjectSort =
  | 'NameAsc'
  | 'NameDesc'
  | 'StartDateAsc'
  | 'StartDateDesc'
  | 'EndDateAsc'
  | 'EndDateDesc'
  | 'PriorityAsc'
  | 'PriorityDesc'

export type TaskSort =
  | 'NameAsc'
  | 'NameDesc'
  | 'PriorityAsc'
  | 'PriorityDesc'
  | 'StatusAsc'
  | 'StatusDesc'

export interface Employee {
  id: string
  email: string
  name: string
  surname: string
  patronymic?: string | null
}

export interface Project {
  id: string
  name: string
  customerCompanyName: string
  executorCompanyName: string
  startDate: string
  endDate: string
  priority: number
  manager: Employee
  employees: Employee[]
}

export interface ProjectTask {
  id: string
  projectId: string
  name: string
  comment?: string | null
  priority: number
  status: ProjectTaskStatus
  author: Employee
  assignee?: Employee | null
}

export interface Document {
  id: string
  projectId: string
  fileName: string
  contentType: string
  uploadedAt: string
}

export interface ParamsEmployeeRequest {
  email: string
  name: string
  surname: string
  patronymic?: string | null
}

export interface ParamsProjectRequest {
  name: string
  customerCompanyName: string
  executorCompanyName: string
  startDate: string
  endDate: string
  priority: number
  managerId: string
  employeeIds: string[]
}

export interface ParamsProjectTaskRequest {
  name: string
  comment?: string | null
  priority: number
  authorId: string
  assigneeId?: string | null
  status?: ProjectTaskStatus
}

export interface ProjectFilter {
  startDateFrom?: string
  startDateTo?: string
  priorityMin?: number
  priorityMax?: number
  name?: string
  customerCompanyName?: string
  executorCompanyName?: string
}

export interface ProjectTaskFilter {
  status?: ProjectTaskStatus
  priorityMin?: number
  priorityMax?: number
  assigneeId?: string
  authorId?: string
}

export interface AuthSession {
  token: string
  email: string
  employeeId: string
  roles: AppRole[]
}

export interface UserAccount {
  employeeId: string
  email: string
  roles: AppRole[]
}