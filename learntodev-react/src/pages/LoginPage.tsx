import { useState, type SubmitEvent } from "react";
import { useNavigate } from "react-router";
import FormField from "../components/FormField";
import { useToast } from "../components/toast/useToast";
import { fieldErrorsFrom, type FieldErrors } from "../utils/problemDetails";

export default function LoginPage() {
  const [emailOrUserName, setEmailOrUserName] = useState("");
  const [password, setPassword] = useState("");
  const [errors, setErrors] = useState<FieldErrors>({});
  const toast = useToast();
  const navigate = useNavigate();

  async function handleSubmit(event: SubmitEvent<HTMLFormElement>) {
    event.preventDefault();

    //Client validation first, same error shape as the server uses
    const clientErrors: FieldErrors = {};
    if (!emailOrUserName) clientErrors.emailOrUserName = "Skriv inn e-post eller brukernavn";
    if (!password) clientErrors.password = "Skriv inn passord";
    setErrors(clientErrors);
    if (Object.keys(clientErrors).length > 0) return;

    const response = await fetch("/api/auth/login", {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({ emailOrUserName, password, rememberMe: false }),
    });

    if (response.ok) {
      toast("Innlogging vellykket");
      navigate("/");
    } else if (response.status === 400) {
      setErrors(fieldErrorsFrom(await response.json())); // Validation errors from ProblemDetails format
    } else {
      const body = await response.json().catch(() => null);
      toast(body?.message ?? "Innlogging feilet", "error"); //401: Unauthorized or other errors
    }
  }

  return (
    <form className="card" onSubmit={handleSubmit} noValidate>
      <h1>Logg inn</h1>
      <FormField label="E-post eller brukernavn" name="emailOrUserName"
        value={emailOrUserName} onChange={(e) => setEmailOrUserName(e.target.value)} 
        error={errors.emailOrUserName} />
      <FormField label="Passord" name="password" type="password" 
        value={password} onChange={(e) => setPassword(e.target.value)} 
        error={errors.password} />
      <button className="btn" type="submit">Logg inn</button>
    </form>
  );
}
