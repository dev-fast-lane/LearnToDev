import Layout from "./components/Layout";
import { Routes, Route } from "react-router";
import StartPage from "./pages/StartPage";
import LoginPage from "./pages/LoginPage";
import SchoolsPage from "./pages/SchoolsPage";
import JobsPage from "./pages/JobsPage";
import HousingPage from "./pages/HousingPage";
import AdminPage from "./pages/AdminPage";
import NotFoundPage from "./pages/NotFoundPage";

// Which page to show for each URL
export default function App() {
  return (
    <Routes>
      <Route element={<Layout />}>
        <Route path="/" element={<StartPage />} />
        <Route path="/login" element={<LoginPage />} />
        <Route path="/schools" element={<SchoolsPage />} />
        <Route path="/jobs" element={<JobsPage />} />
        <Route path="/housing" element={<HousingPage />} />
        <Route path="/admin" element={<AdminPage />} />
        <Route path="*" element={<NotFoundPage />} />
      </Route>
    </Routes>
  );
}
