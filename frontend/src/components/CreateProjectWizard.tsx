import { useMemo, useState } from 'react'
import { useNavigate } from 'react-router-dom'
import * as documentsApi from '../api/documents'
import { ApiError } from '../api/http'
import * as projectsApi from '../api/projects'
import type { Employee, ParamsProjectRequest } from '../types'

import { DocumentDropZone } from './DocumentDropZone'
import { EmployeeSelect } from './EmployeeSelect'
import { EmployeeTeamPicker } from './EmployeeTeamPicker'
import { WizardSteps } from './WizardSteps'

const WIZARD_STEPS = [
  'Основное',
  'Сроки',
  'Менеджер',
  'Команда',
  'Документы',
] as const

const TOTAL_STEPS = WIZARD_STEPS.length

type Draft = ParamsProjectRequest

function emptyDraft(): Draft {
  const today = new Date().toISOString().slice(0, 10)
  const nextMonth = new Date(Date.now() + 30 * 86400000).toISOString().slice(0, 10)
  return {
    name: '',
    customerCompanyName: '',
    executorCompanyName: '',
    startDate: today,
    endDate: nextMonth,
    priority: 5,
    managerId: '',
    employeeIds: [],
  }
}

export function CreateProjectWizard({
  employees,
  onDone,
  onCancel,
}: {
  employees: Employee[]
  onDone: () => void | Promise<void>
  onCancel: () => void
}) {
  const navigate = useNavigate()
  const [step, setStep] = useState(1)
  const [draft, setDraft] = useState<Draft>(emptyDraft)
  const [projectId, setProjectId] = useState<string | null>(null)
  const [pendingFiles, setPendingFiles] = useState<File[]>([])
  const [uploading, setUploading] = useState(false)
  const [creating, setCreating] = useState(false)
  const [error, setError] = useState<string | null>(null)

  const patch = (part: Partial<Draft>) => setDraft((d) => ({ ...d, ...part }))

  const draftForSubmit = useMemo(
    (): Draft => ({
      ...draft,
      employeeIds: draft.employeeIds.filter((id) => id !== draft.managerId),
    }),
    [draft],
  )

  const validationError = useMemo((): string | null => {
    switch (step) {
      case 1:
        if (!draft.name.trim()) return 'Укажите название проекта'
        if (!draft.customerCompanyName.trim()) return 'Укажите заказчика'
        if (!draft.executorCompanyName.trim()) return 'Укажите исполнителя'
        return null
      case 2:
        if (!draft.startDate || !draft.endDate) return 'Укажите даты'
        if (draft.startDate >= draft.endDate) return 'Дата окончания должна быть позже начала'
        if (draft.priority < 1 || draft.priority > 10) return 'Приоритет от 1 до 10'
        return null
      case 3:
        if (!draft.managerId) return 'Выберите менеджера'
        return null
      case 4:
        if (draftForSubmit.employeeIds.length === 0) return 'Добавьте хотя бы одного исполнителя'
        return null
      default:
        return null
    }
  }, [draft, draftForSubmit, step])

  const goNext = () => {
    if (validationError) {
      setError(validationError)
      return
    }
    setError(null)
    setStep((s) => Math.min(s + 1, TOTAL_STEPS))
  }

  const goBack = () => {
    setError(null)
    if (step === TOTAL_STEPS && projectId) return
    setStep((s) => Math.max(s - 1, 1))
  }

  const createProject = async () => {
    if (validationError) {
      setError(validationError)
      return
    }
    setCreating(true)
    setError(null)
    try {
      const id = await projectsApi.createProject(draftForSubmit)
      setProjectId(id)
      setStep(5)
    } catch (err) {
      setError(err instanceof ApiError ? err.message : 'Не удалось создать проект')
    } finally {
      setCreating(false)
    }
  }

  const uploadFiles = async (files: File[]) => {
    if (!projectId) return
    setUploading(true)
    setError(null)
    try {
      for (const file of files) {
        await documentsApi.uploadDocument(projectId, file)
      }
      setPendingFiles((prev) => [...prev, ...files])
    } catch (err) {
      setError(err instanceof ApiError ? err.message : 'Ошибка загрузки файла')
    } finally {
      setUploading(false)
    }
  }

  const finish = async () => {
    await onDone()
    if (projectId) navigate(`/projects/${projectId}`)
  }

  return (
    <div className="project-wizard">
      <WizardSteps steps={WIZARD_STEPS} current={step} />

      {error && <p className="form-error">{error}</p>}

      {step === 1 && (
        <div className="form-grid">
          <label className="span-2">
            Название проекта
            <input
              value={draft.name}
              onChange={(e) => patch({ name: e.target.value })}
              required
              autoFocus
            />
          </label>
          <label>
            Заказчик
            <input
              value={draft.customerCompanyName}
              onChange={(e) => patch({ customerCompanyName: e.target.value })}
              required
            />
          </label>
          <label>
            Исполнитель
            <input
              value={draft.executorCompanyName}
              onChange={(e) => patch({ executorCompanyName: e.target.value })}
              required
            />
          </label>
        </div>
      )}

      {step === 2 && (
        <div className="form-grid">
          <label>
            Дата начала
            <input
              type="date"
              value={draft.startDate}
              onChange={(e) => patch({ startDate: e.target.value })}
              required
            />
          </label>
          <label>
            Дата окончания
            <input
              type="date"
              value={draft.endDate}
              onChange={(e) => patch({ endDate: e.target.value })}
              required
            />
          </label>
          <label>
            Приоритет (1–10)
            <input
              type="number"
              min={1}
              max={10}
              value={draft.priority}
              onChange={(e) => patch({ priority: Number(e.target.value) })}
              required
            />
          </label>
        </div>
      )}

      {step === 3 && (
        <div className="form-grid">
          <EmployeeSelect
            label="Менеджер проекта"
            employees={employees}
            value={draft.managerId}
            onChange={(managerId) =>
              setDraft((d) => ({
                ...d,
                managerId,
                employeeIds: d.employeeIds.filter((id) => id !== managerId),
              }))
            }
          />
          <p className="muted small">
            На следующем шаге выберите исполнителей (менеджер в команду не входит).
          </p>
        </div>
      )}

      {step === 4 && (
        <fieldset className="employee-picker">
          <legend>Исполнители проекта</legend>
          <EmployeeTeamPicker
            employees={employees}
            managerId={draft.managerId}
            selectedIds={draft.employeeIds}
            onChange={(employeeIds) => patch({ employeeIds })}
          />
          <p className="muted small picker-footnote">
            После «Создать проект» можно загрузить документы на следующем шаге.
          </p>
        </fieldset>
      )}

      {step === 5 && projectId && (
        <div>
          <p className="muted mt-0">
            Проект <strong>{draft.name}</strong> создан. Перетащите файлы или выберите с диска.
          </p>
          <DocumentDropZone uploading={uploading} onFiles={(files) => void uploadFiles(files)} />
          {pendingFiles.length > 0 && (
            <ul className="wizard-uploaded-list">
              {pendingFiles.map((f, i) => (
                <li key={`${f.name}-${i}`}>{f.name}</li>
              ))}
            </ul>
          )}
        </div>
      )}

      <div className="form-actions wizard-actions">
        <button type="button" className="btn btn-ghost" onClick={onCancel}>
          Отмена
        </button>
        <div className="wizard-actions-right">
          {step > 1 && step < 5 && (
            <button type="button" className="btn btn-ghost" onClick={goBack}>
              Назад
            </button>
          )}
          {step < 4 && (
            <button type="button" className="btn btn-primary" onClick={goNext}>
              Далее
            </button>
          )}
          {step === 4 && (
            <button
              type="button"
              className="btn btn-primary"
              disabled={creating}
              onClick={() => void createProject()}
            >
              {creating ? 'Создание…' : 'Создать проект'}
            </button>
          )}
          {step === 5 && (
            <>
              <button type="button" className="btn btn-ghost" onClick={() => void finish()}>
                Пропустить
              </button>
              <button type="button" className="btn btn-primary" onClick={() => void finish()}>
                Готово
              </button>
            </>
          )}
        </div>
      </div>
    </div>
  )
}