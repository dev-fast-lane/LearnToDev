import { useEffect, useState } from "react";
import { createQuestion, getQuestionOptions } from "../services/questionService";
import type {
  QuestionOptionsDto,
  SaveStudyQuestionDto,
  StudyQuestionDto,
} from "../types/question";

type QuestionFormProps = {
  onSaved: (question: StudyQuestionDto) => void;
};

const emptyQuestion: SaveStudyQuestionDto = {
  schoolLevelId: 0,
  text: "",
  explanation: "",
  answerOptions: [
    { text: "", isCorrect: true },
    { text: "", isCorrect: false },
    { text: "", isCorrect: false },
    { text: "", isCorrect: false },
  ],
};

export default function QuestionForm({ onSaved }: QuestionFormProps) {
  const [form, setForm] = useState(emptyQuestion);
  const [options, setOptions] = useState<QuestionOptionsDto | null>(null);
  const [selectedSchoolId, setSelectedSchoolId] = useState(0);
  const [loadingOptions, setLoadingOptions] = useState(true);
  const [submitting, setSubmitting] = useState(false);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    let cancelled = false;

    getQuestionOptions()
      .then((result) => {
        if (!cancelled) {
          setOptions(result);
        }
      })
      .catch(() => {
        if (!cancelled) {
          setError("Could not load schools and levels.");
        }
      })
      .finally(() => {
        if (!cancelled) {
          setLoadingOptions(false);
        }
      });

    return () => {
      cancelled = true;
    };
  }, []);

  const availableLevels =
    options?.schoolLevels.filter(
      (schoolLevel) => schoolLevel.schoolId === selectedSchoolId,
    ) ?? [];

  function updateAnswerText(index: number, text: string) {
    setForm((current) => ({
      ...current,
      answerOptions: current.answerOptions.map((option, optionIndex) =>
        optionIndex === index ? { ...option, text } : option,
      ),
    }));
  }

  function updateCorrectAnswer(index: number) {
    setForm((current) => ({
      ...current,
      answerOptions: current.answerOptions.map((option, optionIndex) => ({
        ...option,
        isCorrect: optionIndex === index,
      })),
    }));
  }

  async function handleSubmit(event: React.FormEvent<HTMLFormElement>) {
    event.preventDefault();
    setSubmitting(true);
    setError(null);

    try {
      const createdQuestion = await createQuestion(form);
      onSaved(createdQuestion);
      setForm(emptyQuestion);
      setSelectedSchoolId(0);
    } catch (error) {
      setError(
        error instanceof Error ? error.message : "Could not create question.",
      );
    } finally {
      setSubmitting(false);
    }
  }

  if (loadingOptions) {
    return <p>Loading schools and levels...</p>;
  }

  return (
    <form onSubmit={handleSubmit}>
      <h2>Create question</h2>

      {error && <p role="alert">{error}</p>}

      <div className="field">
        <label htmlFor="school">School</label>
        <select
          id="school"
          value={selectedSchoolId}
          onChange={(event) => {
            setSelectedSchoolId(Number(event.target.value));
            setForm((current) => ({
              ...current,
              schoolLevelId: 0,
            }));
          }}
          required
        >
          <option value={0}>Select a school</option>
          {options?.schools.map((school) => (
            <option key={school.id} value={school.id}>
              {school.name}
            </option>
          ))}
        </select>
      </div>

      <div className="field">
        <label htmlFor="schoolLevel">Level</label>
        <select
          id="schoolLevel"
          value={form.schoolLevelId}
          onChange={(event) => {
            setForm((current) => ({
              ...current,
              schoolLevelId: Number(event.target.value),
            }));
          }}
          disabled={selectedSchoolId === 0}
          required
        >
          <option value={0}>Select a level</option>
          {availableLevels.map((schoolLevel) => (
            <option key={schoolLevel.id} value={schoolLevel.id}>
              Level {schoolLevel.level}
            </option>
          ))}
        </select>
      </div>

      <div className="field">
        <label htmlFor="questionText">Question</label>
        <textarea
          id="questionText"
          value={form.text}
          onChange={(event) =>
            setForm((current) => ({ ...current, text: event.target.value }))
          }
          required
          maxLength={1000}
        />
      </div>

      <div className="field">
        <label htmlFor="explanation">Explanation</label>
        <textarea
          id="explanation"
          value={form.explanation}
          onChange={(event) =>
            setForm((current) => ({
              ...current,
              explanation: event.target.value,
            }))
          }
          required
          maxLength={2000}
        />
      </div>

      <fieldset>
        <legend>Answer options</legend>
        {form.answerOptions.map((option, index) => (
          <div className="field" key={index}>
            <label htmlFor={`answer-${index}`}>Option {index + 1}</label>
            <input
              id={`answer-${index}`}
              value={option.text}
              onChange={(event) => updateAnswerText(index, event.target.value)}
              maxLength={1000}
              required
            />
            <label>
              <input
                type="radio"
                name="correctAnswer"
                checked={option.isCorrect}
                onChange={() => updateCorrectAnswer(index)}
              />
              Correct answer
            </label>
          </div>
        ))}
      </fieldset>

      <button type="submit" disabled={submitting}>
        {submitting ? "Saving..." : "Create question"}
      </button>
    </form>
  );
}
