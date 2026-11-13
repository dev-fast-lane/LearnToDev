export type AnswerOptionDto = {
  text: string;
  isCorrect: boolean;
};

export type StudyQuestionDto = {
  id: number;
  schoolLevelId: number;
  schoolName: string;
  level: number;
  text: string;
  explanation: string;
  answerOptions: AnswerOptionDto[];
};

export type SaveStudyQuestionDto = {
  schoolLevelId: number;
  text: string;
  explanation: string;
  answerOptions: AnswerOptionDto[];
};

export type SchoolOptionDto = {
  id: number;
  name: string;
};

export type SchoolLevelOptionDto = {
  id: number;
  schoolId: number;
  level: number;
};

export type QuestionOptionsDto = {
  schools: SchoolOptionDto[];
  schoolLevels: SchoolLevelOptionDto[];
}