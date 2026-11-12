import { get } from "./apiClient";
import type { JobDto } from "../types/job";

// All calls to /api/jobs. Components use this, not fetch
export function getJobs(): Promise<JobDto[]> {
  return get<JobDto[]>("/jobs");
}