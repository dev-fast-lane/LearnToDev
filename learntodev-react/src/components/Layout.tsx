import { NavLink, Outlet, useNavigate } from "react-router";
import { useAuth } from "../auth/useAuth";
import { useToast } from "./toast/useToast";
import "./Layout.css";

// Shared frame around every page: menu, player status and the page content
export default function Layout() {
  const { user, isAdmin, logout } = useAuth();
  const toast = useToast();
  const navigate = useNavigate();

  async function handleLogout() {
    try {
      await logout();
      navigate("/login");
    } catch {
      toast("Kunne ikke logge ut. Prøv igjen.", "error");
    }
  }

  return (
    <div className="layout">
      <header className="layout-header">
        <nav className="layout-nav" aria-label="Hovedmeny">
          {/* Different menu for guest, player and admin (R-04) */}
          {user ? (
            <>
              <NavLink to="/" end>
                Start
              </NavLink>
              <NavLink to="/schools">Skoler</NavLink>
              <NavLink to="/jobs">Jobber</NavLink>
              <NavLink to="/housing">Bolig</NavLink>
              {isAdmin && <NavLink to="/admin">Admin</NavLink>}
            </>
          ) : (
            <>
              <NavLink to="/login">Logg inn</NavLink>
              <NavLink to="/register">Registrer</NavLink>
            </>
          )}
        </nav>

        {/* Only logged in users have a player and can log out */}
        {user && (
          <section className="player-status" aria-label="Spillerstatus">
            {user.userName}
            <button className="btn btn-secondary" onClick={handleLogout}>
              Logg ut
            </button>
          </section>
        )}
      </header>

      <main className="layout-main">
        <Outlet />
      </main>
    </div>
  );
}
