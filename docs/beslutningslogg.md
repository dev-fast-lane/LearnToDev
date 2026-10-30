# Beslutningslogg: spillregler

Denne loggen forklarer hvorfor spillreglene i [spilltall](spilltall.md) er som de er. Hver beslutning følger malen Kontekst, Beslutning, Konsekvenser og Dato.

## ADR 1: Fullstack er bare eksamen

**Dato:** 28.10.2026

**Kontekst:** Fullstack skal vise at spilleren behersker frontend, backend og database samlet, ikke ett fag til.

**Beslutning:** Fullstack har ingen øvingsspørsmål eller kompendium, bare eksamen (`IsExamOnly = true`). For å ta Fullstack på et nivå må spilleren ha bestått samme nivå på alle de andre skolene. Eksamensspørsmålene trekkes fra de andre skolene.

**Konsekvenser:** Ingen egne spørsmål å skrive for Fullstack, og ingen kompendium (`Compendium = null`). Fullstack blir en siste "boss" før de låser opp neste nivå, og får tilgang til andre jobber.

## ADR 2: Jobbstigen

**Dato:** 28.10.2026

**Kontekst:** Spilleren trenger en tydelig vei fra start til mål, der hver jobb føles som et steg opp.

**Beslutning:** Jobbene kommer i fast rekkefølge, og hver jobb krever skolenivå, Fullstack og erfaring fra forrige jobb:

| Jobb      | Krav                                         | Lønn per dag | Skift |
| --------- | -------------------------------------------- | ------------ | ----- |
| IT Help   | Alltid åpen                                  | 20           | 4 t   |
| Stud.ass  | Bestått nivå 1 på en skole                   | 75           | 4 t   |
| Office    | Nivå 1 + Fullstack 1, og jobbet som Stud.ass | 150          | 6 t   |
| Startup   | Nivå 2 + Fullstack 2, og jobbet på Office    | 260          | 6 t   |
| Tech Corp | Nivå 3 + Fullstack 3, og jobbet på Startup   | 450          | 8 t   |

Kravene sikrer at spilleren går gjennom stigen i riktig rekkefølge: skolenivået viser
at hen kan faget, Fullstack viser at hen behersker alle fagene samlet, og kravet om
tidligere jobb gjør at man ikke kan hoppe over trinn.

Lønnen er satt slik at overskuddet per dag (lønn minus husleie for boligen som hører til jobben) stiger på hvert nivå: 20, 35, 70, 100 og 150. Lønnen blir utbetalt på starten av hver nye dag.

**Konsekvenser:** Spilleren må både lære (skole og eksamen) og jobbe for å komme videre. Kravene lagres i `JobRequirement` (skolenivå) og `JobPrerequisite` (tidligere jobb). Bedre jobber har lengre skift, så de krever en bolig med flere timer.

## ADR 3: Oppsigelse

**Dato:** 28.10.2026

**Kontekst:** Arbeid må ha en konsekvens hvis spilleren svarer dårlig, men én dårlig dag skal ikke ødelegge spillet.

**Beslutning:** Et skift har 5 oppgaver. 0–2 riktige gir en advarsel. Etter to advarsler får du sparken. Advarslene nullstilles etter 5 dager.

**Konsekvenser:** Det er en risiko for at spilleren får sparken, og progresjonen stopper litt opp. Spilleren må da finne seg en ny jobb, og kan heller ikke søke på den jobben den fikk sparken fra, før det har gått 5 dager.

## ADR 4: Husleie og Telt

**Dato:** 28.10.2026

**Kontekst:** Penger må bety noe gjennom hele spillet. Spilleren skal ikke kunne samle opp så mye at penger mister verdi, men heller ikke ende med negativ saldo.

**Beslutning:** Husleie trekkes per natt. Telt er gratis. Hver jobb har råd til sin egen bolig med litt til overs, men ikke til neste. Bedre bolig gir 2 timer mer per dag (Telt 12 t, Penthouse 20 t). Husleie blir betalt på starten av hver nye dag, så dersom spilleren ikke har råd til husleien, blir hen kastet ut til Telt i stedet for å få negative penger.

| Bolig     | Husleie per natt | Timer per dag |
| --------- | ---------------- | ------------- |
| Telt      | 0                | 12            |
| Dorm      | 40               | 14            |
| Hybel     | 80               | 16            |
| Leilighet | 160              | 18            |
| Penthouse | 300              | 20            |

**Konsekvenser:** Spilleren må få ny jobb for å flytte opp. Det oppstår et valg mellom penger (billig bolig) og tid (dyr bolig med flere timer). Saldoen kan aldri bli negativ.

## ADR 5: Eksamen

**Dato:** 28.10.2026

**Kontekst:** Eksamen skal vise at spilleren faktisk kan stoffet, ikke at hen har gjettet seg til det.

**Beslutning:** Hver eksamen har 5 spørsmål, og 4 riktige gir bestått. Pris og tid stiger med nivå:

| Nivå | Pris | Tid |
| ---- | ---- | --- |
| 1    | 50   | 2 t |
| 2    | 100  | 3 t |
| 3    | 200  | 4 t |

Øving koster 1 time per spørsmål. Med rundt 15 spørsmål per nivå tar øvingen omtrent like lang tid (ca. 2 dager) som å spare til eksamen.

**Konsekvenser:** Eksamen er en liten brems økonomisk, men et tydelig kunnskapskrav. Nytt forsøk koster samme pris og tid.
