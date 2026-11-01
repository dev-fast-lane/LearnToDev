# Seed-format for skoler, nivåer og spørsmål

Spørsmålene i spillet skrives i JSON-filer som `DbInitializer` (#12) leser inn første gang appen starter.

## Maler

Kopier en av malene når du skal skrive spørsmål for en skole:

- [`sporsmal-mal.json`](sporsmal-mal.json): vanlig skole med kompendium og spørsmål
- [`sporsmal-mal-fullstack.json`](sporsmal-mal-fullstack.json): Fullstack, som bare har eksamen

Malene har plassholdere og skal ikke seedes. De ekte filene legges i `LearnToDev.Mvc/Data/Seed/` (se #12).

## Én fil per skole

Hver skole har sin egen fil, for eksempel `frontend.json` og `backend.json`. Da kan flere skrive spørsmål samtidig uten merge-konflikter.

## Struktur

```
School
└── schoolLevels[]        (nøyaktig 3)
    └── studyQuestions[]
        └── answerOptions[]
```

Strukturen følger modellene `School` → `SchoolLevel` → `StudyQuestion` → `AnswerOption`. Relasjonene kommer av nestingen, så filen har ingen `id`- eller `…Id`-felter. Databasen lager dem.

## Felter

Feltnavnene er de samme som i modellklassene, i camelCase.

| Felt | Type | Regel |
|---|---|---|
| `name` | tekst | Påkrevd, maks 100 tegn |
| `isExamOnly` | true/false | `true` bare for Fullstack |
| `schoolLevels[].level` | tall | 1, 2 eller 3 |
| `schoolLevels[].compendium` | tekst eller `null` | `null` bare for Fullstack |
| `studyQuestions[].text` | tekst | Påkrevd, maks 1000 tegn |
| `studyQuestions[].explanation` | tekst | Påkrevd, maks 2000 tegn. Vises etter at spilleren har svart |
| `answerOptions[].text` | tekst | Påkrevd, maks 1000 tegn |
| `answerOptions[].isCorrect` | true/false | Se reglene under |

## Regler

- Hver skole har nøyaktig 3 nivåer: 1, 2 og 3.
- Hvert spørsmål har 3–4 svaralternativer.
- Hvert spørsmål har nøyaktig ett riktig svar (`isCorrect: true`).
- Fullstack har `isExamOnly: true`, `compendium: null` og tom `studyQuestions` på alle nivåer. Eksamen i Fullstack trekker spørsmål fra de andre skolene (#34).

## Kompendium

Kompendiet ligger som tekst i `compendium` på hvert nivå, rett i JSON-filen. Det følger modellen, der `Compendium` er et felt på `SchoolLevel`. Det er altså ett kompendium per nivå og ikke én egen fil per skole.

Om teksten skal være ren tekst eller Markdown er ikke bestemt ennå.

## Innlesing (#12)

`System.Text.Json` skiller som standard mellom store og små bokstaver, så `schoolLevels` blir ikke koblet til `SchoolLevels`. Da forsvinner nivåene uten feilmelding. Les filene med `new JsonSerializerOptions(JsonSerializerDefaults.Web)`.
