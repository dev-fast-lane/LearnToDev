import { defineConfig } from "vite";
import react from "@vitejs/plugin-react";

// https://vite.dev/config/
export default defineConfig({
  plugins: [react()],
  server: {
    // Forward API calls to the backend, so the app and the API share origin during development
    proxy: {
      "/api": {
        target: "https://localhost:7028",
        changeOrigin: true,
        secure: false,
      },
    },
  },
});