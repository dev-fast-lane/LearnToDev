import { useEffect, useState, type ReactNode } from "react";
import { AuthContext } from "./authContext";
import * as authService from "../services/authService";
import type { LoginDto, RegisterDto, UserDto } from "../types/auth";

// Keeps track of the logged in user and shares it with the whole app
export default function AuthProvider({ children }: { children: ReactNode }) {
  const [user, setUser] = useState<UserDto | null>(null);
  const [loading, setLoading] = useState(true);

  // The cookie survives a page reload, so ask the API who we are once at startup
  useEffect(() => {
    authService
      .getCurrentUser()
      .then(setUser)
      .catch(() => setUser(null))
      .finally(() => setLoading(false));
  }, []);

  // Errors (ApiError) are not caught here, so the forms can show them
  async function login(dto: LoginDto) {
    setUser(await authService.login(dto));
  }

  async function register(dto: RegisterDto) {
    setUser(await authService.register(dto));
  }

  async function logout() {
    try {
      await authService.logout();
    } finally {
      setUser(null);
    }
  }

  // Must match Roles.Admin in the API
  const isAdmin = user?.roles.includes("Admin") ?? false;

  return (
    <AuthContext value={{ user, loading, isAdmin, login, register, logout }}>
      {children}
    </AuthContext>
  );
}
