export function Alert({
  variant = 'error',
  children,
}: {
  variant?: 'error' | 'success' | 'info'
  children: React.ReactNode
}) {
  return <div className={`alert alert-${variant}`}>{children}</div>
}