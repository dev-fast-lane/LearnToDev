# ITPE3200 Project Documentation 
### Group: WebApp Best Group

Lenke til OneDrive Word dokument: [**Word**](https://hioa365-my.sharepoint.com/:w:/r/personal/inhau1960_oslomet_no/Documents/OsloMet%20H%C3%B8st%202026/ITPE3200%20WebApp/ITPE3200%20WebApp%20DeltMappe/ITPE3200%20Prosjekt%20Dokumentasjon%20WebApp%20Best%20Group.docx?d=waf0d7cbe03da4c96b1332c5cd0279fa7&csf=1&web=1&e=RdYDC0) og [**Generell mappe**](https://hioa365-my.sharepoint.com/:f:/r/personal/inhau1960_oslomet_no/Documents/OsloMet%20H%C3%B8st%202026/ITPE3200%20WebApp/ITPE3200%20WebApp%20DeltMappe?d=w21d76fa95b324ae192aba1b092fa3e9b&csf=1&web=1&e=hYte1W)


## Databasestruktur og tilgang - ansvarlig: navn (~400 ord)
Tilhørende issue: [#71](https://github.com/dev-fast-lane/LearnToDev/issues/71)

## Arkitektur og beslutningslogg - ansvarlig: navn (~400 ord)
Tilhørende issue: [#72](https://github.com/dev-fast-lane/LearnToDev/issues/72)

## Kravanalyse og sporbarhetsmatrise - ansvarlig: navn (~400 ord)
Tilhørende issue: [#73](https://github.com/dev-fast-lane/LearnToDev/issues/73)

## Sikkerhet, robusthet og testing - ansvarlig: navn (~400 ord)
Tilhørende issue: [#74](https://github.com/dev-fast-lane/LearnToDev/issues/74)

## KI-bruk og verifisering - ansvarlig: navn (~300 ord)
Tilhørende issue: [#75](https://github.com/dev-fast-lane/LearnToDev/issues/75)

## Brukeropplevelse, konklusjon og refleksjon - ansvarlig: navn (~500 ord)
Tilhørende issue: [#76](https://github.com/dev-fast-lane/LearnToDev/issues/76)

## Kjøreinstruksjoner, testbrukere og test på ren maskin - ansvarlig: navn (~300 ord)
Tilhørende issue: [#77](https://github.com/dev-fast-lane/LearnToDev/issues/77)






## Innholdsfortegnelse

- [1. Tittel og administrative opplysninger](#1-tittel-og-administrative-opplysninger)
- [2. Kravsanalyse og sporbarhetsmatrise](#2-kravsanalyse-og-sporbarhetsmatrise)
  - [2.1 Verdiforslag og målgruppe](#21-verdiforslag-og-målgruppe)
  - [2.2 Kravsporbarhetsmatrise](#22-kravsporbarhetsmatrise)
- [3. Systemarkitektur og beslutningslogger for tekniske valg](#3-systemarkitektur-og-beslutningslogger-for-tekniske-valg)
  - [3.1 Systemarkitekturdiagrammer](#31-systemarkitekturdiagrammer)
  - [3.2 Sammenligning av arkitekturene](#32-sammenligning-av-arkitekturene)
  - [3.3 Arkitektoniske beslutningslogger (ADR-er)](#33-arkitektoniske-beslutningslogger-adr-er)
- [4. Databasestruktur og datatilgang](#4-databasestruktur-og-datatilgang)
  - [4.1 Entitet-relasjonsdiagram (ER-diagram)](#41-entitet-relasjonsdiagram-er-diagram)
  - [4.2 Datatilgang, repository-mønster og API-tjenestelag](#42-datatilgang-repository-mønster-og-api-tjenestelag)
- [5. Funksjonsoversikt og sporbarhet](#5-funksjonsoversikt-og-sporbarhet)
  - [5.1 Grundig gjennomgang av CRUD-operasjoner](#51-grundig-gjennomgang-av-crud-operasjoner)
    - [5.1.1 Underapplikasjon 1: MVC](#511-underapplikasjon-1-mvc)
    - [5.1.2 Underapplikasjon 2: API og SPA](#512-underapplikasjon-2-api-og-spa)
  - [5.2 Arbeidsflytdiagram: Hvordan ulike brukerroller bruker applikasjonen](#52-arbeidsflytdiagram-hvordan-ulike-brukerroller-bruker-applikasjonen)
- [6. Sikkerhet, robusthet og testing](#6-sikkerhet-robusthet-og-testing)
  - [6.1 Flyt for autentisering og autorisasjon](#61-flyt-for-autentisering-og-autorisasjon)
  - [6.2 Strategi for feilhåndtering og validering av inndata](#62-strategi-for-feilhandtering-og-validering-av-inndata)
  - [6.3 Strategi for enhetstesting](#63-strategi-for-enhetstesting)
- [7. KI-bruk og verifisering](#7-ki-bruk-og-verifisering)
  - [7.1 Metodikk for bruk av KI](#71-metodikk-for-bruk-av-ki)
  - [7.2 Logg over verifisering og forbedringer](#72-logg-over-verifisering-og-forbedringer)
- [8. Brukeropplevelse, brukervennlighet og tilgjengelighet](#8-brukeropplevelse-brukervennlighet-og-tilgjengelighet)
- [9. Konklusjon og egenrefleksjon](#9-konklusjon-og-egenrefleksjon)

---

## 1. Tittel og administrative opplysninger

**Ansvarlig:** navn · **Omfang:** ca. 150–250 ord  
**Tilhørende issues:** [#77](https://github.com/dev-fast-lane/LearnToDev/issues/77), [#78](https://github.com/dev-fast-lane/LearnToDev/issues/78)

Oppgi prosjektnavn, valgt oppgave, anonymiserte kandidat-ID-er, teknologistakk og versjoner. Beskriv hvordan applikasjonen startes, hvilke testbrukere som finnes, og hvordan leveringspakken klargjøres. Ikke ta med ekte passord eller andre hemmeligheter i rapporten.

---

## 2. Kravsanalyse og sporbarhetsmatrise

**Ansvarlig:** navn · **Omfang:** ca. 300–500 ord  
**Tilhørende issue:** [#73](https://github.com/dev-fast-lane/LearnToDev/issues/73)

### 2.1 Verdiforslag og målgruppe

Beskriv hvilket problem LearnToDev løser, hvem målgruppen er, og hvilke behov applikasjonen dekker. Ta med eksempler på typiske brukssituasjoner.

### 2.2 Kravsporbarhetsmatrise

Lag en matrise over funksjonelle og ikke-funksjonelle krav. For hvert krav oppgir dere status — oppnådd, delvis oppnådd eller utelatt — og begrunner statusen. Knytt kravene til relevante funksjoner eller issues der det passer.

---

## 3. Systemarkitektur og beslutningslogger for tekniske valg

**Ansvarlig:** navn · **Omfang:** ca. 400–700 ord  
**Tilhørende issues:** [#11](https://github.com/dev-fast-lane/LearnToDev/issues/11), [#32](https://github.com/dev-fast-lane/LearnToDev/issues/32), [#55](https://github.com/dev-fast-lane/LearnToDev/issues/55), [#56](https://github.com/dev-fast-lane/LearnToDev/issues/56), [#63](https://github.com/dev-fast-lane/LearnToDev/issues/63), [#64](https://github.com/dev-fast-lane/LearnToDev/issues/64)

### 3.1 Systemarkitekturdiagrammer

Ta med arkitekturdiagrammer som viser de faktiske komponentene og grensene mellom dem. Forklar én konkret forespørsel gjennom systemet, fra brukerhandling til datatilgang og tilbake til brukergrensesnittet. Bruk faktiske fil- og klassenavn.

### 3.2 Sammenligning av arkitekturene

Hvis prosjektet faktisk inneholder både MVC- og API/SPA-underapplikasjoner, sammenlign dataflyt, tilstandshåndtering og hvordan sidene vises. Hvis én av arkitekturene ikke finnes i prosjektet, forklar dette i stedet for å beskrive en ikke-eksisterende løsning.

### 3.3 Arkitektoniske beslutningslogger (ADR-er)

Dokumenter minst to viktige tekniske beslutninger. For hver beslutning beskriver dere kontekst, alternativer, valgt løsning, begrunnelse og ulemper eller kompromisser.

**Tilhørende issues:** [#3](https://github.com/dev-fast-lane/LearnToDev/issues/3), [#5](https://github.com/dev-fast-lane/LearnToDev/issues/5), [#42](https://github.com/dev-fast-lane/LearnToDev/issues/42)

---

## 4. Databasestruktur og datatilgang

**Ansvarlig:** navn · **Omfang:** ca. 300–500 ord  
**Tilhørende issues:** [#9](https://github.com/dev-fast-lane/LearnToDev/issues/9), [#10](https://github.com/dev-fast-lane/LearnToDev/issues/10), [#12](https://github.com/dev-fast-lane/LearnToDev/issues/12), [#19](https://github.com/dev-fast-lane/LearnToDev/issues/19), [#20](https://github.com/dev-fast-lane/LearnToDev/issues/20)

### 4.1 Entitet-relasjonsdiagram (ER-diagram)

Legg inn et ER-diagram som viser databasens faktiske entiteter, primær- og fremmednøkler, datatyper og relasjoner. Diagrammet må samsvare med EF Core-modellene og `DbContext`-konfigurasjonen.

### 4.2 Datatilgang, repository-mønster og API-tjenestelag

Beskriv hvordan `DbContext` er konfigurert, hvordan data hentes og lagres, og hvordan repositories og eventuelle tjenestelag brukes. Ta med et kort kodeeksempel fra prosjektet som viser en databaseforespørsel og hvordan feil håndteres. Ikke beskriv et API-tjenestelag dersom prosjektet ikke har et slikt lag.

---

## 5. Funksjonsoversikt og sporbarhet

**Ansvarlig:** navn · **Omfang:** ca. 450–750 ord  
**Tilhørende issues:** [#15](https://github.com/dev-fast-lane/LearnToDev/issues/15), [#27](https://github.com/dev-fast-lane/LearnToDev/issues/27), [#29](https://github.com/dev-fast-lane/LearnToDev/issues/29), [#30](https://github.com/dev-fast-lane/LearnToDev/issues/30), [#31](https://github.com/dev-fast-lane/LearnToDev/issues/31), [#32](https://github.com/dev-fast-lane/LearnToDev/issues/32), [#33](https://github.com/dev-fast-lane/LearnToDev/issues/33), [#34](https://github.com/dev-fast-lane/LearnToDev/issues/34), [#36](https://github.com/dev-fast-lane/LearnToDev/issues/36), [#37](https://github.com/dev-fast-lane/LearnToDev/issues/37), [#38](https://github.com/dev-fast-lane/LearnToDev/issues/38), [#39](https://github.com/dev-fast-lane/LearnToDev/issues/39), [#40](https://github.com/dev-fast-lane/LearnToDev/issues/40), [#43](https://github.com/dev-fast-lane/LearnToDev/issues/43), [#44](https://github.com/dev-fast-lane/LearnToDev/issues/44)

Beskriv de viktigste funksjonene og hvordan de støtter kravene. Velg én sentral domenemodell for den grundige CRUD-gjennomgangen.

### 5.1 Grundig gjennomgang av CRUD-operasjoner

**Valgt domenemodell:** [for eksempel `StudyQuestion`, `Job` eller `Housing`]

#### 5.1.1 Underapplikasjon 1: MVC

Beskriv oppretting, lesing, oppdatering og sletting. Følg dataflyten gjennom skjema, controller, validering, datatilgang og resultatet som vises til brukeren.

**Mulige tilhørende issues:** [#22](https://github.com/dev-fast-lane/LearnToDev/issues/22), [#23](https://github.com/dev-fast-lane/LearnToDev/issues/23), [#24](https://github.com/dev-fast-lane/LearnToDev/issues/24), [#25](https://github.com/dev-fast-lane/LearnToDev/issues/25), [#26](https://github.com/dev-fast-lane/LearnToDev/issues/26)

#### 5.1.2 Underapplikasjon 2: API og SPA

Hvis denne underapplikasjonen finnes i prosjektet, beskriv hvordan frontend sender forespørsler til API-et, hvordan API-et validerer og behandler dem, og hvordan svaret oppdaterer brukergrensesnittet. Hvis prosjektet ikke har en API/SPA-underapplikasjon, si det tydelig og dokumenter den faktiske løsningen i stedet.

### 5.2 Arbeidsflytdiagram: Hvordan ulike brukerroller bruker applikasjonen

Ta med minst ett arbeidsflytdiagram for en sentral handling, for eksempel å øve, ta eksamen, arbeide en dag eller bytte bolig. Vis relevante roller og beslutningspunkter.

**Mulige tilhørende issues:** [#29](https://github.com/dev-fast-lane/LearnToDev/issues/29), [#31](https://github.com/dev-fast-lane/LearnToDev/issues/31), [#33](https://github.com/dev-fast-lane/LearnToDev/issues/33), [#39](https://github.com/dev-fast-lane/LearnToDev/issues/39), [#43](https://github.com/dev-fast-lane/LearnToDev/issues/43)

---

## 6. Sikkerhet, robusthet og testing

**Ansvarlig:** navn · **Omfang:** ca. 400–650 ord  
**Tilhørende issues:** [#14](https://github.com/dev-fast-lane/LearnToDev/issues/14), [#16](https://github.com/dev-fast-lane/LearnToDev/issues/16), [#17](https://github.com/dev-fast-lane/LearnToDev/issues/17), [#47](https://github.com/dev-fast-lane/LearnToDev/issues/47), [#48](https://github.com/dev-fast-lane/LearnToDev/issues/48), [#49](https://github.com/dev-fast-lane/LearnToDev/issues/49), [#50](https://github.com/dev-fast-lane/LearnToDev/issues/50), [#51](https://github.com/dev-fast-lane/LearnToDev/issues/51), [#52](https://github.com/dev-fast-lane/LearnToDev/issues/52)

### 6.1 Flyt for autentisering og autorisasjon

Beskriv hvordan brukere logger inn, hvordan identiteten opprettholdes, og hvordan roller eller tilgangskontroller begrenser handlinger. Skill mellom autentisering (hvem brukeren er) og autorisasjon (hva brukeren får gjøre).

### 6.2 Strategi for feilhåndtering og validering av inndata

Forklar hvilken validering som skjer på klienten og serveren, hvordan ugyldige data håndteres, og hvordan uventede feil logges og vises til brukeren. Oppgi relevante eksempler fra den faktiske løsningen.

### 6.3 Strategi for enhetstesting

Beskriv testverktøyene, hva slags logikk som testes, og hvordan positive og negative scenarioer dekkes. Ta med en tabell med test-ID, testet enhet/metode, testtype, scenario og forventet resultat. Oppgi faktisk antall tester; ikke hev at et minstekrav er oppfylt uten å kontrollere det.

**Tilhørende issues:** [#47](https://github.com/dev-fast-lane/LearnToDev/issues/47), [#48](https://github.com/dev-fast-lane/LearnToDev/issues/48), [#49](https://github.com/dev-fast-lane/LearnToDev/issues/49), [#50](https://github.com/dev-fast-lane/LearnToDev/issues/50), [#51](https://github.com/dev-fast-lane/LearnToDev/issues/51)

---

## 7. KI-bruk og verifisering

**Ansvarlig:** navn · **Omfang:** ca. 250–450 ord  
**Tilhørende issue:** [#75](https://github.com/dev-fast-lane/LearnToDev/issues/75)

### 7.1 Metodikk for bruk av KI

Beskriv hvordan gruppa brukte KI til idéutvikling, forståelse av konsepter, brukeropplevelse eller kode. Forklar hvordan dere holdt arbeidet avgrenset, gjennomgikk forslagene og unngikk å ta inn kode ukritisk.

### 7.2 Logg over verifisering og forbedringer

Ta med minst tre konkrete eksempler i tråd med oppgaveteksten, inkludert ett eksempel der KI foreslo kode. Dokumenter hva dere spurte om, hva dere brukte, hva som måtte endres, og hvordan dere kontrollerte sluttresultatet. Bruk også den fortløpende KI-loggen i `docs/KI-logg.md`.

---

## 8. Brukeropplevelse, brukervennlighet og tilgjengelighet

**Ansvarlig:** navn · **Omfang:** ca. 250–450 ord  
**Tilhørende issues:** [#15](https://github.com/dev-fast-lane/LearnToDev/issues/15), [#27](https://github.com/dev-fast-lane/LearnToDev/issues/27), [#29](https://github.com/dev-fast-lane/LearnToDev/issues/29), [#30](https://github.com/dev-fast-lane/LearnToDev/issues/30), [#31](https://github.com/dev-fast-lane/LearnToDev/issues/31), [#37](https://github.com/dev-fast-lane/LearnToDev/issues/37), [#45](https://github.com/dev-fast-lane/LearnToDev/issues/45), [#80](https://github.com/dev-fast-lane/LearnToDev/issues/80), [#81](https://github.com/dev-fast-lane/LearnToDev/issues/81), [#82](https://github.com/dev-fast-lane/LearnToDev/issues/82)

Beskriv utformingen av grensesnittet, hvordan det fungerer på ulike skjermstørrelser, og hvilke hensyn dere tok til brukervennlighet og tilgjengelighet. Ta med konkret evaluering eller tilbakemelding hvis dere gjennomførte det. Hvis dere ikke testet med brukere, forklar hvordan dere ville gjort det. Ta bare med #80–#82 dersom funksjonene er implementert og relevante for rapporten.

---

## 9. Konklusjon og egenrefleksjon

**Ansvarlig:** navn · **Omfang:** ca. 200–350 ord  
**Tilhørende issue:** [#76](https://github.com/dev-fast-lane/LearnToDev/issues/76)

Oppsummer hva prosjektet har oppnådd, hvilke tekniske utfordringer som var størst, og hva gruppa lærte. Vurder gjenstående teknisk gjeld og hva dere ville prioritert videre dersom prosjektet skulle utvikles mer.