import { useState, type SubmitEvent } from "react";
import { useNavigate } from "react-router";
import FormField from "../components/FormField";
import { useToast } from "../components/toast/useToast";
import { ApiError } from "../services/apiError";
import { fieldErrorsFrom, type FieldErrors } from "../utils/problemDetails";
import { useAuth } from "../auth/useAuth";

export default function RegisterPage() {
  const [userName, setUserName] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");
  const [errors, setErrors] = useState<FieldErrors>({});
  const toast = useToast();
  const navigate = useNavigate();
  const { register } = useAuth();
  const [submitting, setSubmitting] = useState(false);

  async function handleSubmit(event: SubmitEvent<HTMLFormElement>) {
    event.preventDefault();

    //Client validation first, same messages as RegisterDto in the API
    const clientErrors: FieldErrors = {};
    if (!userName) clientErrors.userName = "Skriv inn et brukernavn.";
    if (!email) clientErrors.email = "Skriv inn e-post.";
    else if (!email.includes("@"))
      clientErrors.email = "Ugyldig e-postadresse.";
    if (!password) clientErrors.password = "Skriv inn passord.";
    if (!confirmPassword) clientErrors.confirmPassword = "Gjenta passordet.";
    else if (confirmPassword !== password)
      clientErrors.confirmPassword = "Passordene er ikke like.";
    setErrors(clientErrors);
    if (Object.keys(clientErrors).length > 0) return;
    if (submitting) return; // guard against double submit
    setSubmitting(true);
    try {
      await register({ userName, email, password, confirmPassword });
      toast("Bruker opprettet. Velkommen!", "success");
      navigate("/");
    } catch (error) {
      if (!(error instanceof ApiError)) {
        toast("Noe gikk galt. Prøv igjen.", "error");
        return;
      }
      // Identity errors (weak password, taken user name) have no field, so show them in a toast
      const general = error.fieldErrors[""];
      if (general) {
        toast(general.join(" "), "error");
      } else if (Object.keys(error.fieldErrors).length > 0) {
        setErrors(fieldErrorsFrom(error.fieldErrors)); // errors at each field
      } else {
        toast(error.message, "error");
      }
    } finally {
      setSubmitting(false); // always re-enable, also after an error
    }
  }
  return (
    <form className="card" onSubmit={handleSubmit} noValidate>
      <h1>Registrer deg</h1>
      <FormField
        label="Brukernavn"
        name="userName"
        value={userName}
        onChange={(e) => setUserName(e.target.value)}
        error={errors.userName}
      />
      <FormField
        label="E-post"
        name="email"
        type="email"
        value={email}
        onChange={(e) => setEmail(e.target.value)}
        error={errors.email}
      />
      <FormField
        label="Passord"
        name="password"
        type="password"
        value={password}
        onChange={(e) => setPassword(e.target.value)}
        error={errors.password}
      />
      <FormField
        label="Gjenta passord"
        name="confirmPassword"
        type="password"
        value={confirmPassword}
        onChange={(e) => setConfirmPassword(e.target.value)}
        error={errors.confirmPassword}
      />
      <button className="btn" type="submit" disabled={submitting}>
        {submitting ? "Registrerer..." : "Registrer"}
      </button>
    </form>
  );
}
