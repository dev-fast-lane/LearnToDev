import Layout from "./components/Layout";
import { Routes, Route } from "react-router";
import StartPage from "./pages/StartPage";
import LoginPage from "./pages/LoginPage";
import SchoolsPage from "./pages/SchoolsPage";
import JobsPage from "./pages/JobsPage";
import HousingPage from "./pages/HousingPage";
import AdminPage from "./pages/AdminPage";
import NotFoundPage from "./pages/NotFoundPage";
import RegisterPage from "./pages/RegisterPage";
import RequireAuth from "./auth/RequireAuth";
import RequireAdmin from "./auth/RequireAdmin";

// Which page to show for each URL
export default function App() {
  return (
    <Routes>
      <Route element={<Layout />}>
        {/* Open to everyone */}
        <Route path="/login" element={<LoginPage />} />
        <Route path="/register" element={<RegisterPage />} />

        {/* Logged in users only */}
        <Route element={<RequireAuth />}>
          <Route path="/" element={<StartPage />} />
          <Route path="/schools" element={<SchoolsPage />} />
          <Route path="/jobs" element={<JobsPage />} />
          <Route path="/housing" element={<HousingPage />} />

          {/* Admins only */}
          <Route element={<RequireAdmin />}>
            <Route path="/admin" element={<AdminPage />} />
          </Route>
        </Route>

        <Route path="*" element={<NotFoundPage />} />
      </Route>
    </Routes>
  );
}
