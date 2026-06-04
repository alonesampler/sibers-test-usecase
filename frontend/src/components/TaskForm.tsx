import { useState } from 'react'
import type {
  Employee,
  ParamsProjectTaskRequest,
  ProjectTask,
  ProjectTaskStatus,
} from '../types'

const STATUSES: ProjectTaskStatus[] = ['ToDo', 'InProgress', 'Done']

export function TaskForm({
  employees,
  defaultAuthorId,
  initial,
  onSubmit,
  onCancel,
  submitLabel,
}: {
  employees: Employee[]
  defaultAuthorId: string
  initial?: ProjectTask
  onSubmit: (data: ParamsProjectTaskRequest) => Promise<void>
  onCancel: () => void
  submitLabel: string
}) {
  const [name, setName] = useState(initial?.name ?? '')
  const [comment, setComment] = useState(initial?.comment ?? '')
  const [priority, setPriority] = useState(initial?.priority ?? 5)
  const [authorId, setAuthorId] = useState(initial?.author.id ?? defaultAuthorId)
  const [assigneeId, setAssigneeId] = useState(initial?.assignee?.id ?? '')
  const [status, setStatus] = useState<ProjectTaskStatus>(initial?.status ?? 'ToDo')
  const [error, setError] = useState<string | null>(null)
  const [loading, setLoading] = useState(false)

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault()
    setError(null)
    setLoading(true)
    try {
      await onSubmit({
        name,
        comment: comment || null,
        priority,
        authorId,
        assigneeId: assigneeId || null,
        status,
      })
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Ошибка сохранения')
    } finally {
      setLoading(false)
    }
  }

  return (
    <form className="form-grid" onSubmit={handleSubmit}>
      {error && <p className="form-error">{error}</p>}
      <label>
        Название
        <input value={name} onChange={(e) => setName(e.target.value)} required />
      </label>
      <label>
        Комментарий
        <textarea value={comment} onChange={(e) => setComment(e.target.value)} rows={3} />
      </label>
      <label>
        Приоритет (1–10)
        <input
          type="number"
          min={1}
          max={10}
          value={priority}
          onChange={(e) => setPriority(Number(e.target.value))}
          required
        />
      </label>
      <label>
        Статус
        <select value={status} onChange={(e) => setStatus(e.target.value as ProjectTaskStatus)}>
          {STATUSES.map((s) => (
            <option key={s} value={s}>
              {s}
            </option>
          ))}
        </select>
      </label>
      <label>
        Автор
        <select value={authorId} onChange={(e) => setAuthorId(e.target.value)} required>
          {employees.map((emp) => (
            <option key={emp.id} value={emp.id}>
              {emp.surname} {emp.name}
            </option>
          ))}
        </select>
      </label>
      <label>
        Исполнитель
        <select value={assigneeId} onChange={(e) => setAssigneeId(e.target.value)}>
          <option value="">Не назначен</option>
          {employees.map((emp) => (
            <option key={emp.id} value={emp.id}>
              {emp.surname} {emp.name}
            </option>
          ))}
        </select>
      </label>
      <div className="form-actions">
        <button type="button" className="btn btn-ghost" onClick={onCancel}>
          Отмена
        </button>
        <button type="submit" className="btn btn-primary" disabled={loading}>
          {loading ? 'Сохранение…' : submitLabel}
        </button>
      </div>
    </form>
  )
}