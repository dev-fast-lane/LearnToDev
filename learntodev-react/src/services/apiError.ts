// Slik ser feil fra API-et ut. ProblemDetails er standardformatet ASP.NET bruker,
// og errors er med når valideringen feiler. message kommer fra AuthController
interface ApiErrorBody {
  title?: string;
  status?: number;
  errors?: Record<string, string[]>;
  message?: string;
}

// Feilen tjenestene kaster. Komponentene viser message,
// og skjemaer kan vise fieldErrors under hvert felt
export class ApiError extends Error {
  status: number;
  fieldErrors: Record<string, string[]>;

  constructor(message: string, status: number, fieldErrors: Record<string, string[]> = {}) {
    super(message);
    this.name = "ApiError";
    this.status = status;
    this.fieldErrors = fieldErrors;
  }
}

// title fra API-et er på engelsk, så vi viser norske meldinger basert på statuskoden
const statusMessages: Record<number, string> = {
  400: "Noe er feil i det du sendte inn. Sjekk feltene og prøv igjen.",
  401: "Du må logge inn for å gjøre dette.",
  403: "Du har ikke tilgang til dette.",
  404: "Fant ikke det du lette etter.",
  500: "Noe gikk galt på serveren. Prøv igjen senere.",
  // egen bolk for 502-504
  502: "Får ikke kontakt med serveren. Prøv igjen senere.",
  503: "Får ikke kontakt med serveren. Prøv igjen senere.",
  504: "Får ikke kontakt med serveren. Prøv igjen senere.",
};

// Gjør om et feilsvar fra API-et til en ApiError komponentene kan vise
export async function createApiError(response: Response): Promise<ApiError> {
  let body: ApiErrorBody | null = null;
  try {
    body = (await response.json()) as ApiErrorBody;
  } catch {
    // Tom body eller ikke JSON. Da bruker vi bare statuskoden
  }

  const message =
    body?.message ?? statusMessages[response.status] ?? `Uventet feil (${response.status}).`;

  return new ApiError(message, response.status, body?.errors ?? {});
}