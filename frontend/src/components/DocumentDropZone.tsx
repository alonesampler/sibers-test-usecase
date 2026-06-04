import { useCallback, useId, useRef, useState } from 'react'

export function DocumentDropZone({
  onFiles,
  uploading = false,
  disabled = false,
}: {
  onFiles: (files: File[]) => void | Promise<void>
  uploading?: boolean
  disabled?: boolean
}) {
  const inputId = useId()
  const inputRef = useRef<HTMLInputElement>(null)
  const dragDepth = useRef(0)
  const [dragActive, setDragActive] = useState(false)

  const pickFiles = useCallback(
    (list: FileList | null | undefined) => {
      if (!list?.length || disabled || uploading) return
      onFiles(Array.from(list))
    },
    [disabled, onFiles, uploading],
  )

  const onDragEnter = (e: React.DragEvent) => {
    e.preventDefault()
    e.stopPropagation()
    if (disabled || uploading) return
    dragDepth.current += 1
    if (e.dataTransfer.types.includes('Files')) setDragActive(true)
  }

  const onDragLeave = (e: React.DragEvent) => {
    e.preventDefault()
    e.stopPropagation()
    dragDepth.current -= 1
    if (dragDepth.current <= 0) {
      dragDepth.current = 0
      setDragActive(false)
    }
  }

  const onDragOver = (e: React.DragEvent) => {
    e.preventDefault()
    e.stopPropagation()
    if (disabled || uploading) return
    e.dataTransfer.dropEffect = 'copy'
  }

  const onDrop = (e: React.DragEvent) => {
    e.preventDefault()
    e.stopPropagation()
    dragDepth.current = 0
    setDragActive(false)
    pickFiles(e.dataTransfer.files)
  }

  const openPicker = () => {
    if (!disabled && !uploading) inputRef.current?.click()
  }

  return (
    <div
      className={[
        'doc-dropzone',
        dragActive && 'doc-dropzone-active',
        uploading && 'doc-dropzone-busy',
        disabled && 'doc-dropzone-disabled',
      ]
        .filter(Boolean)
        .join(' ')}
      onDragEnter={onDragEnter}
      onDragLeave={onDragLeave}
      onDragOver={onDragOver}
      onDrop={onDrop}
      onClick={openPicker}
      onKeyDown={(e) => {
        if (e.key === 'Enter' || e.key === ' ') {
          e.preventDefault()
          openPicker()
        }
      }}
      role="button"
      tabIndex={disabled || uploading ? -1 : 0}
      aria-disabled={disabled || uploading}
      aria-busy={uploading}
    >
      <input
        ref={inputRef}
        id={inputId}
        type="file"
        className="doc-dropzone-input"
        multiple
        disabled={disabled || uploading}
        onChange={(e) => {
          pickFiles(e.target.files)
          e.target.value = ''
        }}
        onClick={(e) => e.stopPropagation()}
      />

      <div className="doc-dropzone-icon" aria-hidden>
        <svg width="40" height="40" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="1.5">
          <path d="M12 16V4m0 0L8 8m4-4 4 4" strokeLinecap="round" strokeLinejoin="round" />
          <path d="M4 14v4a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2v-4" strokeLinecap="round" />
        </svg>
      </div>

      {uploading ? (
        <p className="doc-dropzone-title">Загрузка…</p>
      ) : dragActive ? (
        <p className="doc-dropzone-title">Отпустите файл</p>
      ) : (
        <>
          <p className="doc-dropzone-title">Перетащите файлы сюда</p>
          <p className="doc-dropzone-hint">или нажмите, чтобы выбрать на диске</p>
        </>
      )}
    </div>
  )
}