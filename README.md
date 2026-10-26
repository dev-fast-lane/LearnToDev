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

## Arbeidsprosess

Alt arbeid styres fra prosjektbrettet [Kanban](https://github.com/orgs/dev-fast-lane/projects/2). Hver oppgave er en sak (issue), og én sak = én branch = én PR.

### Statuskolonner

| Kolonne | Betydning |
| --- | --- |
| **Backlog** | Planlagt, men ikke klar. Kan være blokkert av andre saker |
| **Klar** | Tydelig omfang og ingen åpne blokkeringer. Trygg å plukke |
| **Pågår** | Noen jobber med den. Maks 2 per person |
| **Til review** | PR er åpen og trenger én reviewer |
| **Ferdig** | Merget og oppfyller Definition of Done |

### Slik jobber du, steg for steg

1. **Velg en sak.** Velg **bare fra Klar**, aldri direkte fra Backlog. Saksnummeret sier ingenting om rekkefølgen, så ikke velg etter det. Plukk en sak (helst i gjeldende uke, se feltet *Uke*) som ingen har tatt. Saker med «⛔ Kan ikke påbegynnes før…» i beskrivelsen eller et *Blocked*-merke venter på andre saker og skal ikke startes.
2. **Ta den.** Sett deg selv som *assignee* og flytt saken til **Pågår**.
3. **Lag en branch fra oppdatert `main`.** Navnet er saksnummeret og et kort navn, for eksempel `8-modellklasser`:
   ```bash
   git checkout main
   git pull
   git checkout -b 8-modellklasser
   ```
4. **Jobb og commit ofte.** Bruk korte meldinger i imperativ som sier hva som er gjort, for eksempel `Legg til Player-modellen`.
5. **Sjekk før du pusher.** `dotnet build` skal gi **0 advarsler**, og `dotnet test` skal være grønn:
   ```bash
   dotnet build
   dotnet test
   ```
6. **Push og åpne en PR mot `main`.** PR-malen fylles ut automatisk. Skriv hva som er endret, hvordan det testes, og `Closes #saksnummer` nederst. Sett deg selv som *assignee* og velg **én annen** på gruppen som *reviewer*. Saken flyttes automatisk til **Til review**, og resten av gruppen får et GitHub-varsel.
7. **Review.** Reviewer leser koden, tester den ved behov og godkjenner eller ber om endringer. Rett opp og push på samme branch. PR-en oppdateres av seg selv.
8. **Merge.** Når PR-en er godkjent og sjekken **Build og test** er grønn, merger du den. Saken lukkes av `Closes #…` og flyttes til **Ferdig**.
9. **Rydd opp.** Slett branchen og hent oppdatert `main` før neste sak:
   ```bash
   git checkout main
   git pull
   ```

### Avhengigheter

Mange saker bygger på hverandre (for eksempel modellklasser, deretter DbContext, deretter migrering). Derfor:

- Saker som venter på andre har en liste over hva som må være ferdig først, og et *Blocked*-merke.
- Når alt en sak venter på er merget, flyttes den til **Klar** (automatisk når en blokkerende sak lukkes, ellers flytter du den selv).
- ***Ikke push en PR for en sak som fortsatt er blokkert. Koden vil ikke bygge eller må skrives om.***

### Definition of Done

En sak er ferdig når:

- [ ] `dotnet build` / `npm run build` gir 0 advarsler
- [ ] Validering finnes på både klient og server (der det er relevant)
- [ ] `ILogger` og feilhåndtering er på plass i koden som er endret
- [ ] Det finnes en test hvis saken berører en domeneregel
- [ ] Ett annet gruppemedlem har gjennomgått PR-en

### Regler

- Maks **2 saker per person** i Pågår.
- Én sak = én PR. Ikke bland flere oppgaver i samme PR.
- Ingen direkte push til `main`. Alt går via PR.
- Fører du KI-bruk, skriv det i KI-loggen fortløpende (se `docs/`).
- Endrer du modellen, oppdater ER-diagrammet i `docs/`.
- Har du tvil om hva som skal gjøres, spør i saken eller på gruppen før du begynner.

## Dokumentasjon

- [Funksjonsoversikt](docs/features_overview.md)
- [ER-diagram v4](docs/erd-gruppe-v4.mermaid)
- [Prosjektplan](docs/prosjektplan.md) (peker til Kanban-brettet)
 