import { useState, type SubmitEvent } from "react";
import { Link, useNavigate } from "react-router";
import FormField from "../components/FormField";
import { useToast } from "../components/toast/useToast";
import { ApiError } from "../services/apiError";
import { fieldErrorsFrom, type FieldErrors } from "../utils/problemDetails";
import { useAuth } from "../auth/useAuth";

export default function LoginPage() {
  const [emailOrUserName, setEmailOrUserName] = useState("");
  const [password, setPassword] = useState("");
  const [errors, setErrors] = useState<FieldErrors>({});
  const toast = useToast();
  const navigate = useNavigate();
  const { login } = useAuth();
  const [submitting, setSubmitting] = useState(false);

  async function handleSubmit(event: SubmitEvent<HTMLFormElement>) {
    event.preventDefault();

    //Client validation first, same error shape as the server uses
    const clientErrors: FieldErrors = {};
    if (!emailOrUserName)
      clientErrors.emailOrUserName = "Skriv inn e-post eller brukernavn";
    if (!password) clientErrors.password = "Skriv inn passord";
    setErrors(clientErrors);
    if (Object.keys(clientErrors).length > 0) return;
    if (submitting) return; // guard against double submit

    setSubmitting(true);
    try {
      await login({ emailOrUserName, password, rememberMe: false });
      toast("Innlogging vellykket", "success");
      navigate("/");
    } catch (error) {
      if (!(error instanceof ApiError)) {
        toast("Noe gikk galt. Prøv igjen.", "error");
        return;
      }
      const fieldErrors = fieldErrorsFrom(error.fieldErrors);
      if (Object.keys(fieldErrors).length > 0) {
        setErrors(fieldErrors); // validation errors at the fields
      } else {
        toast(error.message, "error"); // e.g. "Ugyldig innlogging."
      }
    } finally {
      setSubmitting(false); // NEW: always re-enable, also after an error
    }
  }
  return (
    <form className="card" onSubmit={handleSubmit} noValidate>
      <h1>Logg inn</h1>
      <FormField
        label="E-post eller brukernavn"
        name="emailOrUserName"
        value={emailOrUserName}
        onChange={(e) => setEmailOrUserName(e.target.value)}
        error={errors.emailOrUserName}
      />
      <FormField
        label="Passord"
        name="password"
        type="password"
        value={password}
        onChange={(e) => setPassword(e.target.value)}
        error={errors.password}
      />
      <button className="btn" type="submit" disabled={submitting}>
        {" "}
        {/*disabled while submitting */}
        {submitting ? "Logger inn..." : "Logg inn"}
      </button>
      <p>
        Har du ikke bruker? <Link to="/register">Registrer deg</Link>
      </p>
    </form>
  );
}
