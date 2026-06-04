import { useState } from 'react'
import { ApiError } from '../api/http'
import type { Employee, ParamsEmployeeRequest } from '../types'

export function EmployeeForm({
  initial,
  onSubmit,
  onCancel,
  submitLabel,
  loginEmail = null,
}: {
  initial?: Employee
  onSubmit: (data: ParamsEmployeeRequest) => Promise<void>
  onCancel: () => void
  submitLabel: string
  loginEmail?: string | null
}) {
  const [email, setEmail] = useState(initial?.email ?? '')
  const [name, setName] = useState(initial?.name ?? '')
  const [surname, setSurname] = useState(initial?.surname ?? '')
  const [patronymic, setPatronymic] = useState(initial?.patronymic ?? '')
  const [error, setError] = useState<string | null>(null)
  const [loading, setLoading] = useState(false)

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault()
    setError(null)
    setLoading(true)
    try {
      await onSubmit({
        email,
        name,
        surname,
        patronymic: patronymic || null,
      })
    } catch (err) {
      setError(
        err instanceof ApiError
          ? err.message
          : err instanceof Error
            ? err.message
            : 'Ошибка сохранения',
      )
    } finally {
      setLoading(false)
    }
  }

  return (
    <form className="form-grid" onSubmit={handleSubmit}>
      {error && <p className="form-error">{error}</p>}
      {loginEmail && loginEmail.toLowerCase() !== email.toLowerCase() && (
        <p className="field-hint field-hint-warn">
          Email для входа: <strong>{loginEmail}</strong>. Укажите тот же адрес здесь — он обновится и в
          учётной записи.
          <button
            type="button"
            className="btn btn-ghost btn-sm"
            style={{ marginTop: '0.35rem' }}
            onClick={() => setEmail(loginEmail)}
          >
            Подставить {loginEmail}
          </button>
        </p>
      )}
      <label>
        Email сотрудника
        <input value={email} onChange={(e) => setEmail(e.target.value)} required type="email" />
      </label>
      <label>
        Фамилия
        <input value={surname} onChange={(e) => setSurname(e.target.value)} required />
      </label>
      <label>
        Имя
        <input value={name} onChange={(e) => setName(e.target.value)} required />
      </label>
      <label>
        Отчество
        <input value={patronymic} onChange={(e) => setPatronymic(e.target.value)} />
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