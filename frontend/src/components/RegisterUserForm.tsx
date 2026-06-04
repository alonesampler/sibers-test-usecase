import { useState } from 'react'
import { ApiError } from '../api/http'
import * as authApi from '../api/auth'
import * as employeesApi from '../api/employees'
import type { AppRole, Employee } from '../types'
import { fullName } from '../utils/format'

const ROLES: AppRole[] = ['ProjectManager', 'Employee', 'Director']

export function RegisterUserForm({
  employee,
  onSuccess,
  onCancel,
}: {
  employee: Employee
  onSuccess: () => void
  onCancel: () => void
}) {
  const [email, setEmail] = useState(employee.email)
  const [password, setPassword] = useState('')
  const [role, setRole] = useState<AppRole>('Employee')
  const [error, setError] = useState<string | null>(null)
  const [loading, setLoading] = useState(false)

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault()
    setError(null)
    setLoading(true)
    try {
      await authApi.register({
        employeeId: employee.id,
        email,
        password,
        role,
      })
      const loginEmail = email.trim()
      if (loginEmail.toLowerCase() !== employee.email.trim().toLowerCase()) {
        await employeesApi.updateEmployee(employee.id, {
          email: loginEmail,
          name: employee.name,
          surname: employee.surname,
          patronymic: employee.patronymic ?? null,
        })
      }
      onSuccess()
    } catch (err) {
      if (err instanceof ApiError && err.status === 401) {
        setError('Сессия истекла или нет прав. Войдите снова как Director (admin@admin.com).')
      } else if (err instanceof ApiError && err.status === 0) {
        setError('Сервис авторизации недоступен. Запустите SB.Auth на порту 5182.')
      } else {
        setError(err instanceof ApiError ? err.message : 'Ошибка создания учётной записи')
      }
    } finally {
      setLoading(false)
    }
  }

  return (
    <form className="form-grid" onSubmit={handleSubmit}>
      {error && <p className="form-error">{error}</p>}
      <p className="muted small">
        Сотрудник: <strong>{fullName(employee)}</strong>
      </p>
      <label>
        ID сотрудника
        <input value={employee.id} readOnly />
      </label>
      <label>
        Email для входа
        <input type="email" value={email} onChange={(e) => setEmail(e.target.value)} required />
        {email.trim().toLowerCase() !== employee.email.trim().toLowerCase() && (
          <span className="field-hint">
            После создания email в карточке сотрудника тоже станет {email || '…'}
          </span>
        )}
      </label>
      <label>
        Пароль
        <input
          type="password"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          required
          minLength={8}
          autoComplete="new-password"
        />
        <span className="field-hint">Минимум 8 символов, хотя бы одна цифра</span>
      </label>
      <label>
        Роль
        <select value={role} onChange={(e) => setRole(e.target.value as AppRole)}>
          {ROLES.map((r) => (
            <option key={r} value={r}>
              {r}
            </option>
          ))}
        </select>
      </label>
      <div className="form-actions">
        <button type="button" className="btn btn-ghost" onClick={onCancel}>
          Отмена
        </button>
        <button type="submit" className="btn btn-primary" disabled={loading}>
          {loading ? 'Создание…' : 'Создать login'}
        </button>
      </div>
    </form>
  )
}