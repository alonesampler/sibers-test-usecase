import { useMemo, useState } from 'react'
import type { Employee } from '../types'
import { employeeMatchesQuery } from '../utils/employeeSearch'
import { fullName } from '../utils/format'

export function EmployeeSelect({
  employees,
  value,
  onChange,
  excludeIds = [],
  placeholder = 'Выберите…',
  label = 'Сотрудник',
}: {
  employees: Employee[]
  value: string
  onChange: (id: string) => void
  excludeIds?: string[]
  placeholder?: string
  label?: string
}) {
  const [query, setQuery] = useState('')

  const pool = useMemo(
    () => employees.filter((e) => !excludeIds.includes(e.id)),
    [employees, excludeIds],
  )

  const options = useMemo(() => {
    const matched = pool.filter((e) => employeeMatchesQuery(e, query))
    const selected = pool.find((e) => e.id === value)
    if (selected && !matched.some((e) => e.id === value)) {
      return [selected, ...matched]
    }
    return matched
  }, [pool, query, value])

  return (
    <div className="employee-select">
      <label>
        {label}
        <input
          type="search"
          value={query}
          onChange={(e) => setQuery(e.target.value)}
          placeholder="Поиск по ФИО или email"
          autoComplete="off"
        />
      </label>
      <select value={value} onChange={(e) => onChange(e.target.value)} required>
        <option value="">{placeholder}</option>
        {options.map((emp) => (
          <option key={emp.id} value={emp.id}>
            {fullName(emp)} ({emp.email})
          </option>
        ))}
      </select>
      {query.trim() && options.length === 0 && (
        <p className="muted small">Никого не найдено — уточните запрос</p>
      )}
    </div>
  )
}