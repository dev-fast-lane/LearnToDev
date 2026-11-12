import type { InputHTMLAttributes } from "react";

type Props = InputHTMLAttributes<HTMLInputElement> & {
  label: string;
  error?: string;
};

export default function FormField({ label, error, id, ...rest }: Props) {
    const inputId = id ?? rest.name ?? label;
    return (
        <div className="field">
            <label htmlFor={inputId}>{label}</label>
            <input id={inputId} aria-invalid={!!error} aria-describedby={error ? `${inputId}-error` : undefined} {...rest} />
            {error && <span id={`${inputId}-error`} className="field-error">{error}</span>}
        </div>
    );
}