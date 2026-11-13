import { del, get, post, put } from "./apiClient";
import type {
  SaveStudyQuestionDto,
  StudyQuestionDto,
} from "../types/question";

export function getQuestions(): Promise<StudyQuestionDto[]> {
  return get<StudyQuestionDto[]>("/questions");
}

export function getQuestion(id: number): Promise<StudyQuestionDto> {
  return get<StudyQuestionDto>(`/questions/${id}`);
}

export function createQuestion(
  question: SaveStudyQuestionDto,
): Promise<StudyQuestionDto> {
  return post<StudyQuestionDto>("/questions", question);
}

export function updateQuestion(
  id: number,
  question: SaveStudyQuestionDto,
): Promise<StudyQuestionDto> {
  return put<StudyQuestionDto>(`/questions/${id}`, question);
}

export function deleteQuestion(id: number): Promise<void> {
  return del<void>(`/questions/${id}`);
}
