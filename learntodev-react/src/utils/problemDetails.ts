export type FieldErrors = Record<string, string>;

// ApiError.fieldErrors is { Email: ["..."] } from ASP.NET. Forms want one message per field, with a lowercase first letter
export function fieldErrorsFrom(errors: Record<string, string[]>): FieldErrors {
  const result: FieldErrors = {};
  for (const [field, messages] of Object.entries(errors)) {
    const key = field.charAt(0).toLowerCase() + field.slice(1);
    result[key] = messages[0];
  }
  return result;
}