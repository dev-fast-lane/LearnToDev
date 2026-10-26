Closes #
<!-- Skriv saksnummeret, f.eks. Closes #12. Uten dette flyttes ikke saken til Til review, ingen får varsel, og saken lukkes ikke ved merge. -->

## Hva
<!-- Én–to setninger: hva gjør denne PR-en, og hvorfor? -->

## Endringer
<!-- De viktigste endringene, så reviewer vet hva hen skal se etter. -->
-
-

## Slik tester du
<!-- Hva må reviewer gjøre for å se at det virker? -->
1. `git checkout <branch>`
2.

## Skjermbilder
<!-- Valgfritt: før/etter-bilder når noe synlig er endret. Slett hvis tomt. -->

## Merk
<!-- Valgfritt: hva er bevisst utelatt, avhengigheter, eller noe reviewer bør vite. Slett hvis tomt. -->

## Sjekkliste (Definition of Done)
**Alltid**
- [ ] 0 advarsler på `dotnet build` / `npm run build`
- [ ] Gjennomgått av ett annet gruppemedlem

**Hvis koden i appen er endret** <!-- Slett denne delen hvis PR-en ikke rører appen (f.eks. docs, ERD, workflows). -->
- [ ] Validering på både klient og server (der det er relevant)
- [ ] `ILogger` og feilhåndtering i koden som er endret
- [ ] Test lagt til hvis saken berører en domeneregel
