import { useCallback, useEffect, useMemo, useState } from 'react'
import * as authApi from '../api/auth'
import * as employeesApi from '../api/employees'
import { ApiError } from '../api/http'
import { useAuth } from '../auth/AuthContext'
import { Alert } from '../components/Alert'
import { EmployeeForm } from '../components/EmployeeForm'
import { PageHeader } from '../components/Layout'
import { Modal } from '../components/Modal'
import { RegisterUserForm } from '../components/RegisterUserForm'
import { SearchField } from '../components/SearchField'
import { useDebouncedValue } from '../hooks/useDebouncedValue'
import type { Employee, UserAccount } from '../types'
import { fullName } from '../utils/format'

function accountByEmployeeId(accounts: UserAccount[]): Map<string, UserAccount> {
  return new Map(accounts.map((a) => [a.employeeId.toLowerCase(), a]))
}

export function EmployeesPage() {
  const { isRole } = useAuth()
  const canEdit = isRole('Director')
  const [employees, setEmployees] = useState<Employee[]>([])
  const [userAccounts, setUserAccounts] = useState<UserAccount[]>([])
  const [query, setQuery] = useState('')
  const debouncedQuery = useDebouncedValue(query, 400)
  const [error, setError] = useState<string | null>(null)
  const [loading, setLoading] = useState(true)
  const [createOpen, setCreateOpen] = useState(false)
  const [editEmployee, setEditEmployee] = useState<Employee | null>(null)
  const [registerAccount, setRegisterAccount] = useState<Employee | null>(null)

  const accountsMap = useMemo(() => accountByEmployeeId(userAccounts), [userAccounts])

  const loadAccounts = useCallback(async () => {
    if (!canEdit) return
    try {
      const accounts = await authApi.getUserAccounts()
      setUserAccounts(accounts)
    } catch {
      setUserAccounts([])
    }
  }, [canEdit])

  const load = useCallback(
    async (search: string) => {
      setLoading(true)
      setError(null)
      try {
        const data = search.trim()
          ? await employeesApi.searchEmployees(search)
          : await employeesApi.getEmployees()
        setEmployees(data)
        await loadAccounts()
      } catch (err) {
        setError(err instanceof ApiError ? err.message : 'Ошибка загрузки')
      } finally {
        setLoading(false)
      }
    },
    [loadAccounts],
  )

  useEffect(() => {
    void load(debouncedQuery)
  }, [debouncedQuery, load])

  const handleDelete = async (emp: Employee) => {
    if (!confirm(`Удалить ${fullName(emp)}?`)) return
    try {
      await employeesApi.deleteEmployee(emp.id)
      setEditEmployee(null)
      await load(debouncedQuery)
    } catch (err) {
      alert(err instanceof ApiError ? err.message : 'Ошибка удаления')
    }
  }

  return (
    <>
      <PageHeader
        title="Сотрудники"
        actions={
          canEdit && (
            <button type="button" className="btn btn-primary" onClick={() => setCreateOpen(true)}>
              Добавить
            </button>
          )
        }
      />

      <SearchField
        value={query}
        onChange={setQuery}
        placeholder="Поиск по ФИО или email — результаты обновятся после паузы"
      />

      {error && <Alert>{error}</Alert>}

      <div className="card table-wrap">
        {loading ? (
          <p className="muted">Загрузка…</p>
        ) : employees.length === 0 ? (
          <p className="muted">Сотрудники не найдены</p>
        ) : (
          <table className="employees-table">
            <thead>
              <tr>
                <th>ФИО</th>
                <th>Email сотрудника</th>
                {canEdit && <th>Вход в систему</th>}
                {canEdit && <th />}
              </tr>
            </thead>
            <tbody>
              {employees.map((emp) => {
                const account = accountsMap.get(emp.id.toLowerCase())
                const hasAuthAccount = Boolean(account)
                const emailsMismatch =
                  hasAuthAccount &&
                  emp.email.trim().toLowerCase() !== account!.email.trim().toLowerCase()
                return (
                  <tr key={emp.id} className={emailsMismatch ? 'row-email-mismatch' : undefined}>
                    <td className="cell-name">{fullName(emp)}</td>
                    <td className="cell-email">
                      <span>{emp.email}</span>
                      {emailsMismatch && (
                        <span className="email-mismatch-hint">не совпадает с входом</span>
                      )}
                    </td>
                    {canEdit && (
                      <td className="cell-account">
                        {hasAuthAccount ? (
                          <div className="account-cell">
                            <span className="badge badge-ok">{account!.email}</span>
                            {account!.roles.length > 0 && (
                              <span className="account-roles">{account!.roles.join(', ')}</span>
                            )}
                            {emailsMismatch && (
                              <button
                                type="button"
                                className="btn btn-ghost btn-sm"
                                onClick={async () => {
                                  try {
                                    await employeesApi.updateEmployee(emp.id, {
                                      email: account!.email,
                                      name: emp.name,
                                      surname: emp.surname,
                                      patronymic: emp.patronymic ?? null,
                                    })
                                    await load(debouncedQuery)
                                  } catch (err) {
                                    alert(
                                      err instanceof ApiError ? err.message : 'Не удалось обновить email',
                                    )
                                  }
                                }}
                              >
                                Сделать {account!.email}
                              </button>
                            )}
                          </div>
                        ) : (
                          <span className="muted">Нет входа</span>
                        )}
                      </td>
                    )}
                    {canEdit && (
                      <td className="row-actions">
                        {!hasAuthAccount && (
                          <button
                            type="button"
                            className="btn btn-primary btn-sm"
                            onClick={() => setRegisterAccount(emp)}
                          >
                            Создать login
                          </button>
                        )}
                        <button type="button" className="btn btn-ghost btn-sm" onClick={() => setEditEmployee(emp)}>
                          Изменить
                        </button>
                        <button type="button" className="btn btn-danger btn-sm" onClick={() => void handleDelete(emp)}>
                          Удалить
                        </button>
                      </td>
                    )}
                  </tr>
                )
              })}
            </tbody>
          </table>
        )}
      </div>

      {createOpen && (
        <Modal title="Новый сотрудник" onClose={() => setCreateOpen(false)}>
          <EmployeeForm
            submitLabel="Создать"
            onCancel={() => setCreateOpen(false)}
            onSubmit={async (data) => {
              await employeesApi.createEmployee(data)
              setCreateOpen(false)
              await load(debouncedQuery)
            }}
          />
        </Modal>
      )}

      {editEmployee && (
        <Modal title="Редактирование" onClose={() => setEditEmployee(null)}>
          <EmployeeForm
            initial={editEmployee}
            loginEmail={accountsMap.get(editEmployee.id.toLowerCase())?.email ?? null}
            submitLabel="Сохранить"
            onCancel={() => setEditEmployee(null)}
            onSubmit={async (data) => {
              await employeesApi.updateEmployee(editEmployee.id, data)
              setEditEmployee(null)
              await load(debouncedQuery)
            }}
          />
        </Modal>
      )}

      {registerAccount && (
        <Modal
          title={`Учётная запись — ${fullName(registerAccount)}`}
          onClose={() => setRegisterAccount(null)}
        >
          <RegisterUserForm
            employee={registerAccount}
            onCancel={() => setRegisterAccount(null)}
            onSuccess={async () => {
              setRegisterAccount(null)
              await load(debouncedQuery)
              alert('Учётная запись создана')
            }}
          />
        </Modal>
      )}
    </>
  )
}