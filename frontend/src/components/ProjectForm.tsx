import { useMemo, useState } from 'react'
import { ApiError } from '../api/http'
import type { Employee, ParamsProjectRequest, Project } from '../types'
import { EmployeeSelect } from './EmployeeSelect'
import { EmployeeTeamPicker } from './EmployeeTeamPicker'

export function ProjectForm({
  employees,
  initial,
  onSubmit,
  onCancel,
  submitLabel,
}: {
  employees: Employee[]
  initial?: Project
  onSubmit: (data: ParamsProjectRequest) => Promise<void>
  onCancel: () => void
  submitLabel: string
}) {
  const [name, setName] = useState(initial?.name ?? '')
  const [customer, setCustomer] = useState(initial?.customerCompanyName ?? '')
  const [executor, setExecutor] = useState(initial?.executorCompanyName ?? '')
  const [startDate, setStartDate] = useState(initial?.startDate ?? '')
  const [endDate, setEndDate] = useState(initial?.endDate ?? '')
  const [priority, setPriority] = useState(initial?.priority ?? 5)
  const [managerId, setManagerId] = useState(initial?.manager.id ?? '')
  const [employeeIds, setEmployeeIds] = useState<string[]>(
    initial?.employees.map((e) => e.id).filter((id) => id !== initial?.manager.id) ?? [],
  )
  const [error, setError] = useState<string | null>(null)
  const [loading, setLoading] = useState(false)

  const payload = useMemo(
    (): ParamsProjectRequest => ({
      name,
      customerCompanyName: customer,
      executorCompanyName: executor,
      startDate,
      endDate,
      priority,
      managerId,
      employeeIds: employeeIds.filter((id) => id !== managerId),
    }),
    [name, customer, executor, startDate, endDate, priority, managerId, employeeIds],
  )

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault()
    setError(null)
    if (!payload.managerId) {
      setError('Выберите менеджера')
      return
    }
    if (payload.employeeIds.length === 0) {
      setError('Добавьте хотя бы одного исполнителя')
      return
    }
    setLoading(true)
    try {
      await onSubmit(payload)
    } catch (err) {
      setError(err instanceof ApiError ? err.message : 'Ошибка сохранения')
    } finally {
      setLoading(false)
    }
  }

  return (
    <form className="form-grid form-grid-2" onSubmit={handleSubmit}>
      {error && <p className="form-error span-2">{error}</p>}
      <label className="span-2">
        Название
        <input value={name} onChange={(e) => setName(e.target.value)} required />
      </label>
      <label>
        Заказчик
        <input value={customer} onChange={(e) => setCustomer(e.target.value)} required />
      </label>
      <label>
        Исполнитель
        <input value={executor} onChange={(e) => setExecutor(e.target.value)} required />
      </label>
      <label>
        Дата начала
        <input type="date" value={startDate} onChange={(e) => setStartDate(e.target.value)} required />
      </label>
      <label>
        Дата окончания
        <input type="date" value={endDate} onChange={(e) => setEndDate(e.target.value)} required />
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
      <div className="span-2">
        <EmployeeSelect
          label="Менеджер"
          employees={employees}
          value={managerId}
          onChange={(id) => {
            setManagerId(id)
            setEmployeeIds((prev) => prev.filter((x) => x !== id))
          }}
        />
      </div>
      <fieldset className="span-2 employee-picker">
        <legend>Исполнители проекта</legend>
        <EmployeeTeamPicker
          employees={employees}
          managerId={managerId}
          selectedIds={employeeIds}
          onChange={setEmployeeIds}
        />
      </fieldset>
      <div className="form-actions span-2">
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