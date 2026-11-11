export type FieldErrors = Record<string, string>;

type ProblemDetails = {
    title?: string;
    errors?: Record<string, string[]>;
};

// Converts ASP.NET Core validation errors into a field-to-message map for forms.
// Uses the first message for each field and lowercases the field name's first character.
// For example, it converts { "Email": ["The Email field is required."] } into { "email": "The Email field is required." }.
export function fieldErrorsFrom(problem: ProblemDetails): FieldErrors {
    const result: FieldErrors = {};
    for (const [field, messages] of Object.entries(problem.errors ?? {})) {
        const key = field.charAt(0).toLowerCase() + field.slice(1);
        result[key] = messages[0];
    }
    return result;
}