import { useEffect, useState } from "react";
import { EmptyState, ErrorMessage, Loading } from "../components/StateViews";
import QuestionForm from "../components/QuestionForm";
import { deleteQuestion, getQuestions } from "../services/questionService";
import type { StudyQuestionDto } from "../types/question";

export default function AdminPage() {
  const [questions, setQuestions] = useState<StudyQuestionDto[]>([]);
  const [editingQuestion, setEditingQuestion] =
    useState<StudyQuestionDto | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  async function loadQuestions() {
    try {
      setQuestions(await getQuestions());
    } catch (error) {
      setError(
        error instanceof Error
          ? error.message
          : "Could not load questions.",
      );
    } finally {
      setLoading(false);
    }
  }

  async function handleDelete(question: StudyQuestionDto) {
    if (!window.confirm("Are you sure you want to delete this question?")) {
      return;
    }

    try {
      await deleteQuestion(question.id);
      setQuestions((current) =>
        current.filter((item) => item.id !== question.id),
      );
    } catch (error) {
      setError(
        error instanceof Error
          ? error.message
          : "Could not delete question.",
      );
    }
  }

  useEffect(() => {
    let cancelled = false;

    getQuestions()
      .then((result) => {
        if (!cancelled) {
          setQuestions(result);
        }
      })
      .catch((error: unknown) => {
        if (!cancelled) {
          setError(
            error instanceof Error
              ? error.message
              : "Could not load questions.",
          );
        }
      })
      .finally(() => {
        if (!cancelled) {
          setLoading(false);
        }
      });

    return () => {
      cancelled = true;
    };
  }, []);

  if (loading) {
    return <Loading text="Loading questions..." />;
  }

  if (error) {
    return (
      <ErrorMessage
        message={error}
        onRetry={() => {
          setLoading(true);
          setError(null);
          void loadQuestions();
        }}
      />
    );
  }

  return (
    <main>
      <h1>Manage questions</h1>

      <QuestionForm
        key={editingQuestion?.id ?? "create"}
        question={editingQuestion ?? undefined}
        onSaved={(createdQuestion) => {
          setQuestions((current) => {
            const exists = current.some(
              (question) => question.id === createdQuestion.id,
            );
            return exists
              ? current.map((question) =>
                  question.id === createdQuestion.id
                    ? createdQuestion
                    : question,
                )
              : [createdQuestion, ...current];
          });
          setEditingQuestion(null);
        }}
        onCancel={
          editingQuestion ? () => setEditingQuestion(null) : undefined
        }
      />

      {questions.length === 0 ? (
        <EmptyState text="There are no questions yet." />
      ) : (
        <ul>
          {questions.map((question) => (
            <li key={question.id}>
              <h2>{question.text}</h2>
              <p>
                {question.schoolName}, level {question.level}
              </p>
              <p>{question.explanation}</p>

              <ol>
                {question.answerOptions.map((option, index) => (
                  <li key={`${question.id}-${index}`}>
                    {option.text}
                    {option.isCorrect && " (correct)"}
                  </li>
                ))}
              </ol>
              <button
                type="button"
                onClick={() => setEditingQuestion(question)}
              >
                Edit
              </button>
              <button type="button" onClick={() => void handleDelete(question)}>
                Delete
              </button>
            </li>
          ))}
        </ul>
      )}
    </main>
  );
}
