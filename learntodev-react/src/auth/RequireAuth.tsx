import { Navigate, Outlet } from "react-router";
import { useAuth } from "./useAuth";
import { Loading } from "../components/StateViews";

// Only lets logged in users see the pages inside it
export default function RequireAuth() {
  const { user, loading } = useAuth();

  // Wait for /auth/me, or a logged in user is sent to /login on page reload
  if (loading) return <Loading />;
  if (!user) return <Navigate to="/login" replace />;
  return <Outlet />;
}
