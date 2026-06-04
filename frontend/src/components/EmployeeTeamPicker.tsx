import { useMemo, useState } from 'react'
import type { Employee } from '../types'
import { employeeMatchesQuery } from '../utils/employeeSearch'
import { fullName } from '../utils/format'

export function EmployeeTeamPicker({
  employees,
  managerId,
  selectedIds,
  onChange,
}: {
  employees: Employee[]
  managerId: string
  selectedIds: string[]
  onChange: (ids: string[]) => void
}) {
  const [query, setQuery] = useState('')

  const manager = useMemo(
    () => employees.find((e) => e.id === managerId),
    [employees, managerId],
  )

  const executors = useMemo(
    () => employees.filter((e) => e.id !== managerId),
    [employees, managerId],
  )

  const filtered = useMemo(
    () => executors.filter((e) => employeeMatchesQuery(e, query)),
    [executors, query],
  )

  const toggle = (id: string) => {
    onChange(
      selectedIds.includes(id)
        ? selectedIds.filter((x) => x !== id)
        : [...selectedIds, id],
    )
  }

  if (!managerId) {
    return <p className="muted small">Сначала выберите менеджера на предыдущем шаге.</p>
  }

  return (
    <div className="team-picker">
      {manager && (
        <p className="team-picker-manager muted small">
          Менеджер <strong>{fullName(manager)}</strong> не входит в список исполнителей.
        </p>
      )}

      <label className="team-picker-search">
        Поиск исполнителя
        <input
          type="search"
          value={query}
          onChange={(e) => setQuery(e.target.value)}
          placeholder="ФИО или email"
          autoComplete="off"
        />
      </label>

      <p className="team-picker-meta muted small">
        Выбрано: {selectedIds.length}
        {query.trim() && ` · показано ${filtered.length} из ${executors.length}`}
      </p>

      <div className="checkbox-list team-picker-list" role="list">
        {filtered.length === 0 ? (
          <p className="muted small team-picker-empty">Никого не найдено</p>
        ) : (
          filtered.map((emp) => (
            <label key={emp.id} className="checkbox-item">
              <input
                type="checkbox"
                checked={selectedIds.includes(emp.id)}
                onChange={() => toggle(emp.id)}
              />
              <span>
                {fullName(emp)}
                <span className="team-picker-email">{emp.email}</span>
              </span>
            </label>
          ))
        )}
      </div>
    </div>
  )
}