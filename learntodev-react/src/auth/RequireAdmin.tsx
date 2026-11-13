import { Navigate, Outlet } from "react-router";
import { useAuth } from "./useAuth";

// Only lets admins in. Placed inside RequireAuth, so the user is already logged in here
export default function RequireAdmin() {
  const { isAdmin } = useAuth();

  if (!isAdmin) return <Navigate to="/" replace />;
  return <Outlet />;
}
