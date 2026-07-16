---
name: exam-grounding
description: Ground Claude Certified Architect trainings in the official
  exam_guide.pdf and the Claude docs. Use when the user asks to create,
  expand, or review a training, lesson, or study guide for the exam.
---

# Exam Grounding

## Where the source material lives
- Exam guide: `exam_guide.pdf` in the project root (the authoritative scope).
- Official docs: https://platform.claude.com/docs/en/home (for explanations
  and code examples that go beyond the guide's outline).

## Procedure
1. Run `dotnet run skills/exam-grounding/scripts/read_guide.cs "<topic>"` to
   pull the relevant pages of the exam guide as text.
2. Choose a topic the guide actually covers. Do not invent scope.
3. Fill gaps in explanation from the official docs — never from memory alone.
4. Draft 3-5 sections, then call the `CreateTraining` tool to write the file.

## Guardrails
- Stay within the exam guide's stated domains and task statements.
- Every code example must be runnable and match the docs' current API.
- Target a 5-10 minute read.