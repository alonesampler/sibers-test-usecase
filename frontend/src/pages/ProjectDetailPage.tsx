import { useCallback, useEffect, useState } from 'react'
import { Link, useParams } from 'react-router-dom'
import * as documentsApi from '../api/documents'
import * as employeesApi from '../api/employees'
import { ApiError } from '../api/http'
import * as projectsApi from '../api/projects'
import * as tasksApi from '../api/tasks'
import { useAuth } from '../auth/AuthContext'
import { Alert } from '../components/Alert'
import { DocumentDropZone } from '../components/DocumentDropZone'
import { Modal } from '../components/Modal'
import { ProjectForm } from '../components/ProjectForm'
import { TaskForm } from '../components/TaskForm'
import type {
  Document,
  Employee,
  Project,
  ProjectTask,
  ProjectTaskFilter,
  ProjectTaskStatus,
  TaskSort,
} from '../types'
import { formatDate, formatDateTime, fullName } from '../utils/format'

const TASK_SORT: TaskSort = 'PriorityDesc'
const STATUS_LABELS: Record<ProjectTaskStatus, string> = {
  ToDo: 'К выполнению',
  InProgress: 'В работе',
  Done: 'Готово',
}

export function ProjectDetailPage() {
  const { id } = useParams<{ id: string }>()
  const { session, isRole } = useAuth()
  const canEditProject = isRole('Director', 'ProjectManager')
  const canManageTasks = isRole('Director', 'ProjectManager')
  const canDeleteTask = isRole('Director')

  const [project, setProject] = useState<Project | null>(null)
  const [tasks, setTasks] = useState<ProjectTask[]>([])
  const [documents, setDocuments] = useState<Document[]>([])
  const [employees, setEmployees] = useState<Employee[]>([])
  const [taskFilter, setTaskFilter] = useState<ProjectTaskFilter>({})
  const [error, setError] = useState<string | null>(null)
  const [loading, setLoading] = useState(true)
  const [editProject, setEditProject] = useState(false)
  const [taskModal, setTaskModal] = useState<'create' | ProjectTask | null>(null)
  const [uploading, setUploading] = useState(false)

  const load = useCallback(async () => {
    if (!id) return
    setLoading(true)
    setError(null)
    try {
      const [proj, taskList, docs, emps] = await Promise.all([
        projectsApi.getProject(id),
        tasksApi.getProjectTasks(id, taskFilter, TASK_SORT),
        documentsApi.getDocuments(id),
        employeesApi.getEmployees(),
      ])
      setProject(proj)
      setTasks(taskList)
      setDocuments(docs)
      setEmployees(emps)
    } catch (err) {
      setError(err instanceof ApiError ? err.message : 'Ошибка загрузки')
    } finally {
      setLoading(false)
    }
  }, [id, taskFilter])

  useEffect(() => {
    void load()
  }, [load])

  const handleUploadFiles = async (files: File[]) => {
    if (!id || files.length === 0) return
    setUploading(true)
    try {
      for (const file of files) {
        await documentsApi.uploadDocument(id, file)
      }
      await load()
    } catch (err) {
      alert(err instanceof ApiError ? err.message : 'Ошибка загрузки файла')
    } finally {
      setUploading(false)
    }
  }

  const handleDownload = async (doc: Document) => {
    try {
      const response = await documentsApi.downloadDocument(doc.id)
      if (!response.ok) throw new Error('Download failed')
      const blob = await response.blob()
      const url = URL.createObjectURL(blob)
      const a = document.createElement('a')
      a.href = url
      a.download = doc.fileName
      a.click()
      URL.revokeObjectURL(url)
    } catch {
      alert('Не удалось скачать файл')
    }
  }

  const handleDeleteDoc = async (doc: Document) => {
    if (!confirm(`Удалить ${doc.fileName}?`)) return
    try {
      await documentsApi.deleteDocument(doc.id)
      await load()
    } catch (err) {
      alert(err instanceof ApiError ? err.message : 'Ошибка удаления')
    }
  }

  const handleDeleteTask = async (task: ProjectTask) => {
    if (!confirm(`Удалить задачу «${task.name}»?`)) return
    try {
      await tasksApi.deleteTask(task.id)
      await load()
    } catch (err) {
      alert(err instanceof ApiError ? err.message : 'Ошибка удаления')
    }
  }

  if (loading && !project) {
    return <p className="muted">Загрузка проекта…</p>
  }

  if (!project) {
    return (
      <>
        {error && <Alert>{error}</Alert>}
        <p className="muted">Проект не найден</p>
        <Link to="/projects">← К списку</Link>
      </>
    )
  }

  return (
    <>
      <div className="breadcrumb">
        <Link to="/projects">Проекты</Link>
        <span>/</span>
        <span>{project.name}</span>
      </div>

      {error && <Alert>{error}</Alert>}

      <section className="card project-hero">
        <div className="project-hero-top">
          <div>
            <h1>{project.name}</h1>
            <p className="muted">
              {project.customerCompanyName} → {project.executorCompanyName}
            </p>
          </div>
          <span className={`badge priority-${project.priority}`}>Приоритет {project.priority}</span>
        </div>
        <dl className="hero-meta">
          <div>
            <dt>Менеджер</dt>
            <dd>{fullName(project.manager)}</dd>
          </div>
          <div>
            <dt>Период</dt>
            <dd>
              {formatDate(project.startDate)} — {formatDate(project.endDate)}
            </dd>
          </div>
          <div>
            <dt>Команда</dt>
            <dd>{project.employees.map((e) => fullName(e)).join(', ')}</dd>
          </div>
        </dl>
        {canEditProject && (
          <button type="button" className="btn btn-ghost btn-sm" onClick={() => setEditProject(true)}>
            Редактировать проект
          </button>
        )}
      </section>

      <div className="detail-columns">
        <section className="card">
          <div className="section-head">
            <h2>Задачи</h2>
            {canManageTasks && (
              <button type="button" className="btn btn-primary btn-sm" onClick={() => setTaskModal('create')}>
                Добавить
              </button>
            )}
          </div>
          <div className="filters-inline">
            <select
              value={taskFilter.status ?? ''}
              onChange={(e) => {
                const value = e.target.value
                setTaskFilter((f) => ({
                  ...f,
                  status: value ? (value as ProjectTaskStatus) : undefined,
                }))
              }}
            >
              <option value="">Все статусы</option>
              {(Object.keys(STATUS_LABELS) as ProjectTaskStatus[]).map((s) => (
                <option key={s} value={s}>
                  {STATUS_LABELS[s]}
                </option>
              ))}
            </select>
          </div>
          {tasks.length === 0 ? (
            <p className="muted">Задач нет</p>
          ) : (
            <ul className="task-list">
              {tasks.map((task) => (
                <li key={task.id} className={`task-item status-${task.status}`}>
                  <div>
                    <strong>{task.name}</strong>
                    <span className="badge">{STATUS_LABELS[task.status]}</span>
                    <span className="badge">P{task.priority}</span>
                  </div>
                  <p className="muted small">
                    Автор: {fullName(task.author)}
                    {task.assignee && ` · Исполнитель: ${fullName(task.assignee)}`}
                  </p>
                  {task.comment && <p className="small">{task.comment}</p>}
                  <div className="row-actions">
                    {canManageTasks && (
                      <button type="button" className="btn btn-ghost btn-sm" onClick={() => setTaskModal(task)}>
                        Изменить
                      </button>
                    )}
                    {canDeleteTask && (
                      <button type="button" className="btn btn-danger btn-sm" onClick={() => void handleDeleteTask(task)}>
                        Удалить
                      </button>
                    )}
                  </div>
                </li>
              ))}
            </ul>
          )}
        </section>

        <section className="card">
          <div className="section-head">
            <h2>Документы</h2>
          </div>
          <DocumentDropZone uploading={uploading} onFiles={(files) => void handleUploadFiles(files)} />
          {documents.length === 0 ? (
            <p className="muted">Документов нет</p>
          ) : (
            <ul className="doc-list">
              {documents.map((doc) => (
                <li key={doc.id}>
                  <div>
                    <strong>{doc.fileName}</strong>
                    <span className="muted small">{formatDateTime(doc.uploadedAt)}</span>
                  </div>
                  <div className="row-actions">
                    <button type="button" className="btn btn-ghost btn-sm" onClick={() => void handleDownload(doc)}>
                      Скачать
                    </button>
                    <button type="button" className="btn btn-danger btn-sm" onClick={() => void handleDeleteDoc(doc)}>
                      Удалить
                    </button>
                  </div>
                </li>
              ))}
            </ul>
          )}
        </section>
      </div>

      {editProject && (
        <Modal title="Редактирование проекта" onClose={() => setEditProject(false)} wide>
          <ProjectForm
            employees={employees}
            initial={project}
            submitLabel="Сохранить"
            onCancel={() => setEditProject(false)}
            onSubmit={async (data) => {
              await projectsApi.updateProject(project.id, data)
              setEditProject(false)
              await load()
            }}
          />
        </Modal>
      )}

      {taskModal === 'create' && session && (
        <Modal title="Новая задача" onClose={() => setTaskModal(null)}>
          <TaskForm
            employees={employees}
            defaultAuthorId={session.employeeId}
            submitLabel="Создать"
            onCancel={() => setTaskModal(null)}
            onSubmit={async (data) => {
              await tasksApi.createTask(project.id, data)
              setTaskModal(null)
              await load()
            }}
          />
        </Modal>
      )}

      {taskModal && taskModal !== 'create' && session && (
        <Modal title="Редактирование задачи" onClose={() => setTaskModal(null)}>
          <TaskForm
            employees={employees}
            defaultAuthorId={session.employeeId}
            initial={taskModal}
            submitLabel="Сохранить"
            onCancel={() => setTaskModal(null)}
            onSubmit={async (data) => {
              await tasksApi.updateTask(taskModal.id, data)
              setTaskModal(null)
              await load()
            }}
          />
        </Modal>
      )}
    </>
  )
}