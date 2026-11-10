import { ApiError, createApiError } from "./apiError";

// Alle kall går til /api. Vite-proxyen (vite.config.ts) sender dem videre til backend,
// så appen og API-et deler adresse under utvikling og cookien blir med
const BASE_URL = "/api";

// Felles funksjon for alle kall til API-et (R-08).
// Komponentene skal aldri bruke fetch direkte, bare tjenestene i src/services
async function request<T>(path: string, options: RequestInit = {}): Promise<T> {
  let response: Response;
  try {
    response = await fetch(BASE_URL + path, {
      ...options,
      // Sender med innloggingscookien
      credentials: "include",
      headers: {
        Accept: "application/json",
        // Sier bare at vi sender JSON når vi faktisk har en body
        ...(options.body ? { "Content-Type": "application/json" } : {}),
        ...options.headers,
      },
    });
  } catch (error) {
    // Kallet ble avbrutt med vilje (AbortController). Det er ikke en feil brukeren skal se
    if (error instanceof DOMException && error.name === "AbortError") {
      throw error;
    }
    // fetch kaster bare når svaret aldri kom frem, f.eks. når API-et ikke kjører
    throw new ApiError("Får ikke kontakt med serveren. Prøv igjen senere.", 0);
  }

  if (!response.ok) {
    throw await createApiError(response);
  }

  // 204 No Content (f.eks. logout) har ingen body, så .json() ville feilet
  if (response.status === 204) {
    return undefined as T;
  }

  return (await response.json()) as T;
}

// Små hjelpere, så tjenestefilene blir korte
// <T> er typen svaret skal ha, og den sendes videre til request
export function get<T>(path: string): Promise<T> {
  return request<T>(path);
}

// Sender data, body gjøres om til en JSON-tekst.
export function post<T>(path: string, body?: unknown): Promise<T> {
  return request<T>(path, {
    method: "POST",
    body: body !== undefined ? JSON.stringify(body) : undefined,
  });
}