import { NavLink, Outlet } from 'react-router-dom'
import heroLogo from '../assets/hero.png'
import { useAuth } from '../auth/AuthContext'

export function Layout() {
  const { session, logout } = useAuth()

  return (
    <div className="app-shell">
      <aside className="sidebar">
        <div className="sidebar-brand">
          <img src={heroLogo} alt="" className="brand-logo" />
          <div>
            <strong>Project Office</strong>
            <span>Sibers</span>
          </div>
        </div>

        <nav className="sidebar-nav">
          <NavLink to="/employees" className={({ isActive }) => (isActive ? 'active' : '')}>
            Сотрудники
          </NavLink>
          <NavLink to="/projects" className={({ isActive }) => (isActive ? 'active' : '')}>
            Проекты
          </NavLink>
        </nav>

        <div className="sidebar-footer">
          <div className="user-meta">
            <span>{session?.email}</span>
            <small>{session?.roles.join(', ')}</small>
          </div>
          <button type="button" className="btn btn-ghost btn-sm btn-block" onClick={logout}>
            Выйти
          </button>
        </div>
      </aside>

      <main className="main-content">
        <Outlet />
      </main>
    </div>
  )
}

export function PageHeader({
  title,
  subtitle,
  actions,
}: {
  title: string
  subtitle?: string
  actions?: React.ReactNode
}) {
  return (
    <div className="page-header">
      <div>
        <h1>{title}</h1>
        {subtitle && <p>{subtitle}</p>}
      </div>
      {actions && <div className="page-actions">{actions}</div>}
    </div>
  )
}