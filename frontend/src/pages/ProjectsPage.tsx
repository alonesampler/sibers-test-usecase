import { useCallback, useEffect, useMemo, useState } from 'react'
import { Link } from 'react-router-dom'
import * as employeesApi from '../api/employees'
import { ApiError } from '../api/http'
import * as projectsApi from '../api/projects'
import { useAuth } from '../auth/AuthContext'
import { Alert } from '../components/Alert'
import { PageHeader } from '../components/Layout'
import { Modal } from '../components/Modal'
import { CreateProjectWizard } from '../components/CreateProjectWizard'
import { SearchField } from '../components/SearchField'
import { useDebouncedValue } from '../hooks/useDebouncedValue'
import type { Employee, Project, ProjectFilter, ProjectSort } from '../types'
import { formatDate, fullName } from '../utils/format'

const SORT_OPTIONS: { value: ProjectSort; label: string }[] = [
  { value: 'NameAsc', label: 'Название ↑' },
  { value: 'NameDesc', label: 'Название ↓' },
  { value: 'StartDateAsc', label: 'Старт ↑' },
  { value: 'StartDateDesc', label: 'Старт ↓' },
  { value: 'PriorityDesc', label: 'Приоритет ↓' },
]

export function ProjectsPage() {
  const { isRole } = useAuth()
  const canCreate = isRole('Director', 'ProjectManager')
  const canDelete = isRole('Director')
  const [projects, setProjects] = useState<Project[]>([])
  const [employees, setEmployees] = useState<Employee[]>([])
  const [search, setSearch] = useState('')
  const debouncedSearch = useDebouncedValue(search, 400)
  const [extraFilter, setExtraFilter] = useState<Omit<ProjectFilter, 'name'>>({})
  const [sort, setSort] = useState<ProjectSort>('PriorityDesc')
  const [error, setError] = useState<string | null>(null)
  const [loading, setLoading] = useState(true)
  const [showCreate, setShowCreate] = useState(false)

  const filter = useMemo<ProjectFilter>(
    () => ({
      ...extraFilter,
      name: debouncedSearch.trim() || undefined,
    }),
    [extraFilter, debouncedSearch],
  )

  const load = useCallback(async () => {
    setLoading(true)
    setError(null)
    try {
      const [proj, emps] = await Promise.all([
        projectsApi.getProjects(filter, sort),
        employeesApi.getEmployees(),
      ])
      setProjects(proj)
      setEmployees(emps)
    } catch (err) {
      setError(err instanceof ApiError ? err.message : 'Ошибка загрузки')
    } finally {
      setLoading(false)
    }
  }, [filter, sort])

  useEffect(() => {
    void load()
  }, [load])

  const handleDelete = async (project: Project) => {
    if (!confirm(`Удалить проект «${project.name}»?`)) return
    try {
      await projectsApi.deleteProject(project.id)
      await load()
    } catch (err) {
      alert(err instanceof ApiError ? err.message : 'Ошибка удаления')
    }
  }

  return (
    <>
      <PageHeader
        title="Проекты"
        actions={
          canCreate && (
            <button type="button" className="btn btn-primary" onClick={() => setShowCreate(true)}>
              Новый проект
            </button>
          )
        }
      />

      <SearchField
        value={search}
        onChange={setSearch}
        placeholder="Поиск по названию проекта — обновление после паузы"
      />

      <div className="card filters-compact">
        <div className="filters-grid">
          <label>
            Заказчик
            <input
              value={extraFilter.customerCompanyName ?? ''}
              onChange={(e) =>
                setExtraFilter((f) => ({
                  ...f,
                  customerCompanyName: e.target.value || undefined,
                }))
              }
            />
          </label>
          <label>
            Приоритет от
            <input
              type="number"
              min={0}
              max={10}
              value={extraFilter.priorityMin ?? ''}
              onChange={(e) =>
                setExtraFilter((f) => ({
                  ...f,
                  priorityMin: e.target.value ? Number(e.target.value) : undefined,
                }))
              }
            />
          </label>
          <label>
            Сортировка
            <select value={sort} onChange={(e) => setSort(e.target.value as ProjectSort)}>
              {SORT_OPTIONS.map((o) => (
                <option key={o.value} value={o.value}>
                  {o.label}
                </option>
              ))}
            </select>
          </label>
        </div>
      </div>

      {error && <Alert>{error}</Alert>}

      <div className="project-grid">
        {loading ? (
          <p className="muted">Загрузка…</p>
        ) : projects.length === 0 ? (
          <p className="muted">Проекты не найдены</p>
        ) : (
          projects.map((project) => (
            <article key={project.id} className="project-card card">
              <div className="project-card-head">
                <Link to={`/projects/${project.id}`} className="project-title">
                  {project.name}
                </Link>
                <span className={`badge priority-${project.priority}`}>P{project.priority}</span>
              </div>
              <dl className="meta-list">
                <div>
                  <dt>Заказчик</dt>
                  <dd>{project.customerCompanyName}</dd>
                </div>
                <div>
                  <dt>Менеджер</dt>
                  <dd>{fullName(project.manager)}</dd>
                </div>
                <div>
                  <dt>Сроки</dt>
                  <dd>
                    {formatDate(project.startDate)} — {formatDate(project.endDate)}
                  </dd>
                </div>
                <div>
                  <dt>Команда</dt>
                  <dd>{project.employees.length} чел.</dd>
                </div>
              </dl>
              <div className="project-card-actions">
                <Link to={`/projects/${project.id}`} className="btn btn-ghost btn-sm">
                  Открыть
                </Link>
                {canDelete && (
                  <button
                    type="button"
                    className="btn btn-danger btn-sm"
                    onClick={() => void handleDelete(project)}
                  >
                    Удалить
                  </button>
                )}
              </div>
            </article>
          ))
        )}
      </div>

      {showCreate && (
        <Modal title="Новый проект" onClose={() => setShowCreate(false)} wide>
          <CreateProjectWizard
            employees={employees}
            onCancel={() => setShowCreate(false)}
            onDone={async () => {
              setShowCreate(false)
              await load()
            }}
          />
        </Modal>
      )}
    </>
  )
}