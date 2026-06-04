# Frontend — Project Office

React-приложение для тестового задания Sibers. Работает с двумя backend-сервисами:

| Сервис | URL (dev) | Прокси Vite |
|--------|-----------|-------------|
| SB.Auth | http://localhost:5182 | `/auth-api` |
| ProjectService | http://localhost:5123 | `/api` |

## Запуск

Vite проксирует запросы на backend. Если в консоли `ECONNREFUSED` — **backend не запущен**.

### 1. MS SQL Server

Для локальной разработки без Docker: `localhost,1433`, sa / пароль из `appsettings.json` (как в `docker-compose.yml`).

Либо весь стек одной командой: `.\scripts\docker-up.ps1` (см. `docker/README.md`).

### 2. Backend (обязательно до `npm run dev`)

Из корня репозитория в PowerShell:

```powershell
.\scripts\start-backend.ps1
```

Откроются два окна (или пропустятся, если порты уже заняты):

| Сервис | URL |
|--------|-----|
| SB.Auth | http://localhost:5182 |
| ProjectService | http://localhost:5123 |

Дождитесь `Now listening on: http://localhost:...` в обоих окнах.

Вручную:

```powershell
cd backend\SB.Auth\SB.Auth
dotnet run --launch-profile http

cd backend\SB.ProjectService\ProjectService
dotnet run --launch-profile http
```

### 3. Frontend

```bash
cd frontend
npm install
npm run dev
```

Приложение: http://localhost:5173

## Первый вход

При старте сервисов создаётся администратор (сид):

| | |
|---|---|
| Email | `admin@admin.com` |
| Пароль | `Admin123!` |
| Роль | Director |

Дополнительные учётные записи Director создаёт в разделе **Сотрудники** → кнопка **Создать login** у нужного сотрудника (ID подставляется автоматически).

## Возможности UI

- **Вход / регистрация** — JWT в `localStorage`
- **Сотрудники** — список, поиск, CRUD (только Director)
- **Проекты** — фильтры, сортировка, создание (Director), удаление (Director)
- **Карточка проекта** — редактирование (Director / ProjectManager), задачи, документы

Роли берутся из JWT (`Director`, `ProjectManager`, `Employee`).