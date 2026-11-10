import { get, post } from "./apiClient";
import { ApiError } from "./apiError";
import type { LoginDto, RegisterDto, UserDto } from "../types/auth";

// Alle kall til /api/auth (AuthController). Komponentene bruker disse funksjonene,
// ikke fetch, og får UserDto tilbake eller en ApiError ved feil

// Logger inn med e-post eller brukernavn. API-et setter innloggingscookien
export function login(dto: LoginDto): Promise<UserDto> {
  return post<UserDto>("/auth/login", dto);
}

// Oppretter bruker og logger den inn med en gang
export function register(dto: RegisterDto): Promise<UserDto> {
  return post<UserDto>("/auth/register", dto);
}

// Sletter innloggingscookien. API-et svarer 204 uten body, derfor void
export function logout(): Promise<void> {
  return post<void>("/auth/logout");
}

// Sjekker hvem som er innlogget når appen lastes.
// Å ikke være innlogget er helt vanlig her og ikke en feil, så 401 gir null i stedet for ApiError
export async function getCurrentUser(): Promise<UserDto | null> {
  try {
    return await get<UserDto>("/auth/me");
  } catch (error) {
    if (error instanceof ApiError && error.status === 401) {
      return null;
    }
    throw error;
  }
}