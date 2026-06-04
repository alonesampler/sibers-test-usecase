import { useEffect, useRef, type ReactNode } from 'react'

export function Modal({
  title,
  onClose,
  children,
  wide,
}: {
  title: string
  onClose: () => void
  children: ReactNode
  wide?: boolean
}) {
  const backdropPressed = useRef(false)

  useEffect(() => {
    const onKey = (e: KeyboardEvent) => {
      if (e.key === 'Escape') onClose()
    }
    window.addEventListener('keydown', onKey)
    return () => window.removeEventListener('keydown', onKey)
  }, [onClose])

  const handleBackdropPointerDown = (e: React.PointerEvent<HTMLDivElement>) => {
    backdropPressed.current = e.target === e.currentTarget
  }

  const handleBackdropPointerUp = (e: React.PointerEvent<HTMLDivElement>) => {
    if (backdropPressed.current && e.target === e.currentTarget) onClose()
    backdropPressed.current = false
  }

  const handleBackdropPointerCancel = () => {
    backdropPressed.current = false
  }

  return (
    <div
      className="modal-backdrop"
      role="presentation"
      onPointerDown={handleBackdropPointerDown}
      onPointerUp={handleBackdropPointerUp}
      onPointerCancel={handleBackdropPointerCancel}
    >
      <div
        className={`modal ${wide ? 'modal-wide' : ''}`}
        onPointerDown={(e) => e.stopPropagation()}
        onPointerUp={(e) => e.stopPropagation()}
        role="dialog"
        aria-modal="true"
      >
        <div className="modal-header">
          <h2>{title}</h2>
          <button type="button" className="btn btn-ghost btn-sm" onClick={onClose}>
            ✕
          </button>
        </div>
        <div className="modal-body">{children}</div>
      </div>
    </div>
  )
}