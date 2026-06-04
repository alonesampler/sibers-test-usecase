import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

export default defineConfig({
  plugins: [react()],
  server: {
    port: 5173,
    proxy: {
      '/auth-api': {
        target: 'http://localhost:5182',
        changeOrigin: true,
        rewrite: (path) => path.replace(/^\/auth-api/, ''),
      },
      '/api': {
        target: 'http://localhost:5123',
        changeOrigin: true,
      },
    },
  },
})