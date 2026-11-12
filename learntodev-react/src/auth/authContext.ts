import { createContext } from "react";
import type { LoginDto, RegisterDto, UserDto } from "../types/auth";

// What every component can read with useAuth()
export interface AuthState {
  user: UserDto | null; // null = not logged in
  loading: boolean; // true while we ask the API who is logged in at startup
  isAdmin: boolean;
  login: (dto: LoginDto) => Promise<void>;
  register: (dto: RegisterDto) => Promise<void>;
  logout: () => Promise<void>;
}

export const AuthContext = createContext<AuthState | null>(null);