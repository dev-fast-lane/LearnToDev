import { useEffect, useState } from "react";
import { Loading, ErrorMessage, EmptyState } from "../components/StateViews";

type Job = {
  id: number;
  name: string;
  wage: number;
};

export default function JobsPage() {
  const [jobs, setJobs] = useState<Job[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [attempt, setAttempt] = useState(0); // Used to trigger a re-fetch on retry

  useEffect(() => {
    let cancelled = false; // ignore the answer if the page is left before it arrives

    async function fetchJobs() {
      try {
        const response = await fetch("/api/jobs"); // Placeholder URL for the API endpoint
        if (!response.ok) throw new Error();
        const data = await response.json();
        if (!cancelled) setJobs(data);
      } catch {
        if (!cancelled) setError("Kunne ikke hente jobbene.");
      } finally {
        if (!cancelled) setLoading(false);
      }
    }

    fetchJobs();
    return () => {
      cancelled = true;
    };
  }, [attempt]);

  function retry() {
    setLoading(true);
    setError(null);
    setAttempt((prev) => prev + 1); // Trigger a re-fetch by changing the attempt state
  }

  //Only one of these states is shown at a time
  if (loading) return <Loading />;
  if (error) return <ErrorMessage message={error} onRetry={retry} />;
  if (jobs.length === 0) return <EmptyState text="Ingen jobber enda." />;

  return (
    <div className="table-wrap">
      <table className="table">
        <thead>
          <tr>
            <th>Jobb</th>
            <th>Lønn</th>
          </tr>
        </thead>
        <tbody>
          {jobs.map((job) => (
            <tr key={job.id}>
              <td>{job.name}</td>
              <td>{job.wage}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}
