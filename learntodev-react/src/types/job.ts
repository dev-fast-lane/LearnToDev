// Mirrors the Job model in LearnToDev.Api. API sends camelCase
export interface JobDto {
  id: number;
  name: string;
  wage: number;
}