# LearnToDev

Et læringsspill der spilleren øver på fag, tar eksamener, får jobb og styrer økonomi og bolig. Prosjektet er en gruppeoppgave i ITPE3200 ved OsloMet.

Spilleren øver på spørsmål i ulike skoler og nivåer, består eksamener for å låse opp jobber, tjener penger på arbeidsdager og betaler husleie for å bo. Se [docs/features_overview.md](docs/features_overview.md) for en full beskrivelse av funksjonene.

## Innhold

| Mappe | Beskrivelse |
| --- | --- |
| `LearnToDev.Mvc/` | ASP.NET Core MVC-appen (Identity, EF Core, SQLite) |
| `LearnToDev.Tests/` | Enhetstester (xUnit) |
| `docs/` | ER-diagram, funksjonsoversikt, møtereferater og prosjektplan |

## Kom i gang

Du trenger [.NET SDK 10](https://dotnet.microsoft.com/download).

```bash
git clone https://github.com/dev-fast-lane/LearnToDev.git
cd LearnToDev
dotnet build
dotnet watch --project LearnToDev.Mvc
```

`dotnet watch` starter appen og bygger på nytt når du lagrer. Adressen står i utskriften (`https://localhost:xxxx`). Hvis nettleseren ikke stoler på sertifikatet, kjør `dotnet dev-certs https --trust` én gang.

Kjør testene:

```bash
dotnet test
```

## Arbeidsmåte

- Oppgavene ligger på prosjektbrettet [Kanban](https://github.com/orgs/dev-fast-lane/projects/2) i GitHub Projects.
- Én sak = én PR, med `Closes #nummer` i beskrivelsen.
- Branch-navn følger saken, for eksempel `12-job-eligibility`.
- En PR skal gjennomgås av ett annet gruppemedlem før den merges.
- Koden bygger uten advarsler (`dotnet build`).

## Dokumentasjon

- [Funksjonsoversikt](docs/features_overview.md)
- [ER-diagram v4](docs/erd-gruppe-v4.mermaid)
- [Prosjektplan](docs/fast-lane-prosjektplan.html)
