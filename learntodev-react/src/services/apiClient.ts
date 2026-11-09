// Alle kall går til /api. Vite-proxyen (vite.config.ts) sender dem videre til backend,
// så appen og API-et deler adresse under utvikling og cookien blir med
const BASE_URL = "/api";

// Felles funksjon for alle kall til API-et (R-08).
// Komponentene skal aldri bruke fetch direkte, bare tjenestene i src/services
async function request<T>(path: string, options: RequestInit = {}): Promise<T> {
  const response = await fetch(BASE_URL + path, {
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
  
  if (!response.ok) {
    // OBS OBS! denne er midlertidig !! skal byttes ut med ApiError i steg 2
    throw new Error(`Feil fra API-et: ${response.status}`);
  }

  // 204 No Content (f.eks. logout) har ingen body, så .json() ville feilet
  if (response.status === 204) {
    return undefined as T;
  }

  return (await response.json()) as T;
}

// Små hjelpere, så tjenestefilene blir korte
export function get<T>(path: string): Promise<T> {
  return request<T>(path);
}

export function post<T>(path: string, body?: unknown): Promise<T> {
  return request<T>(path, {
    method: "POST",
    body: body !== undefined ? JSON.stringify(body) : undefined,
  });
}