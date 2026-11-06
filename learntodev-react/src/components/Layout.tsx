import { NavLink, Outlet } from "react-router";
import "./Layout.css";

// Shared frame around every page: menu, player status and the page content
export default function Layout() {
  return (
    <div className="layout">
      <header className="layout-header">
        <nav className="layout-nav" aria-label="Hovedmeny">
          <NavLink to="/" end>
            Start
          </NavLink>
          <NavLink to="/schools">Skoler</NavLink>
          <NavLink to="/jobs">Jobber</NavLink>
          <NavLink to="/housing">Bolig</NavLink>
          <NavLink to="/admin">Admin</NavLink>
          <NavLink to="/login">Logg inn</NavLink>
        </nav>

        {/* Filled with money, hours and job when the player API exists */}
        <section className="player-status" aria-label="Spillerstatus">
          Spillerstatus
        </section>
      </header>

      <main className="layout-main">
        <Outlet />
      </main>
    </div>
  );
}
