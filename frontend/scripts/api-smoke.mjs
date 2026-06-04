const AUTH = 'http://localhost:5182/api/v1/auth'
const API = 'http://localhost:5123/api/v1'

async function req(url, opts = {}) {
  const r = await fetch(url, opts)
  const text = await r.text()
  let body = text
  try {
    body = text ? JSON.parse(text) : null
  } catch {
    /* keep text */
  }
  return { ok: r.ok, status: r.status, body }
}

async function main() {
  const email = `test${Date.now()}@test.com`
  const password = 'Test1234'

  const reg = await req(`${AUTH}/register`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({
      employeeId: process.argv[2] || '00000000-0000-0000-0000-000000000001',
      email,
      password,
      role: 'Director',
    }),
  })
  console.log('register', reg.status, reg.body)

  const login = await req(`${AUTH}/login`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ email, password }),
  })
  console.log('login', login.status, login.body)

  if (!login.body?.token) return

  const token = login.body.token
  const payload = JSON.parse(
    Buffer.from(token.split('.')[1], 'base64url').toString(),
  )
  console.log('jwt payload', JSON.stringify(payload, null, 2))

  const projects = await req(`${API}/projects?sort=NameAsc`, {
    headers: { Authorization: `Bearer ${token}` },
  })
  console.log('projects', projects.status, Array.isArray(projects.body) ? projects.body.length : projects.body)

  const projectId = projects.body?.[0]?.id
  if (projectId) {
    const tasks = await req(`${API}/projects/${projectId}/tasks?sort=NameAsc`, {
      headers: { Authorization: `Bearer ${token}` },
    })
    console.log('task sample status', tasks.body?.[0]?.status, typeof tasks.body?.[0]?.status)
  }
}

main().catch(console.error)