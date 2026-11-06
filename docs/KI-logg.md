# KI-logg
Her er en oversikt over hvilke AI verktøy som har blitt brukt til å hjelpe oss å lage dette, og hva som ble spurt.

### Eksempel / Ingvald
```
Dato: 29-10-2026
Verktøy brukt: Chat-GPT/Sikt-KI-chat
Hva ble spurt: Hvordan skrive en lenke til en GitHub-issue i Markdown.
Hva ble brukt: Forslaget om å bruke `#nummer` i rapportmalen.
Verifisering: La teksten inn i en Markdown-fil og sjekket at `#71` ble klikkbar på GitHub og åpnet riktig issue.

Copy/paste:
Dato:
Verktøy brukt: Claude/ Chat-GPT/ Sikt-KI-chat/ osv.
Hva ble spurt:  
Hva ble brukt:
Verifisering:
```
### Jørgen
```
Dato:
Verktøy brukt: Claude/ Chat-GPT/ Sikt-KI-chat/ osv.
Hva ble spurt:  
Hva ble brukt:
Verifisering:
```
### Bjørnar
```
Dato: 06-11-2026
Verktøy brukt: Claude
Hva ble spurt: Veiledning steg for steg til AuthController (#57): DTO-er, endepunktene for innlogging, registrering, utlogging og me, og cookie-innstillingene.
Hva ble brukt: DTO-ene, AuthController og cookie-innstillingene skrev jeg selv etter forslag og forklaring fra Claude. Claude rettet feil navn på et felt i DTO-ene, la inn kommentarene i AuthController, og oppdaterte README og docs/login.md.
Verifisering: Testet endepunktene i Swagger. Claude kjørte også alle endepunktene mot en midlertidig database, med feil input, feil passord og låst konto, og sjekket at cookien har HttpOnly og SameSite=Strict. dotnet build ga 0 advarsler, og dotnet test ga 22 av 22 bestått.
```
### Sebastian
```
Dato: 26-10-2026
Verktøy brukt: Claude
Hva ble spurt: Hjelp til å balansere spilltallene (#3): lønn, husleie, eksamenspris og tid.
Hva ble brukt: Regnemetoden «lønn = husleie + det som er igjen per dag» og sjekktabeller. Tallene og reglene valgte jeg selv.
Verifisering: Regnet gjennom netto per dag og antall dager til eksamen for hvert nivå, og sjekket at hver jobb har råd til sin bolig, men ikke neste.

Dato: 29-10-2026
Verktøy brukt: Claude
Hva ble spurt: Hvordan JSON fungerer, og hvordan formatet bør se ut for skoler og spørsmål (#11).
Hva ble brukt: Forklaring av JSON-syntaks og forslag til strukturen i malene.
Verifisering: Sjekket at feltnavnene matcher modellklassene, og at filene er gyldig JSON. Rettet etter review fra gruppa.

Dato: 30-10-2026
Verktøy brukt: Claude
Hva ble spurt: Hjelp til å skrive beslutningsloggen for spillreglene (#5).
Hva ble brukt: Utkast til ADR-ene, som jeg gikk gjennom og endret.
Verifisering: Sammenlignet hver ADR med docs/spilltall.md og kravene i saken.

Dato: 01-11-2026
Verktøy brukt: Claude
Hva ble spurt: Lære hvordan enhetstester og mocking fungerer med xUnit og Moq (#47).
Hva ble brukt: Forklaringer av interface, konstruktør, lambda, Setup, Verify og .Object, og eksempeltesten i Examples/ som jeg bygde steg for steg.
Verifisering: Kjørte dotnet test (2 av 2 bestått) og dotnet build (0 advarsler). Så at testen feilet når forventet og faktisk verdi var ulike.

Dato: 04-11-2026
Verktøy brukt: Claude
Hva ble spurt: Hjelp til DbInitializer (#12). Jeg gikk gjennom koden steg for steg og fikk forklart hver del: JSON-lesing med System.Text.Json, async/await, streams, output-mappen og CopyToOutputDirectory, og hvordan EF Core kobler jobber til skolenivåer og andre jobber.
Hva ble brukt: Lesingen av JSON-filene og feilhåndteringen skrev jeg selv etter forklaringen. Claude genererte innholdet i seed-filene (spørsmål og svar) og de repeterende delene av jobbseedingen (jobber, oppgaver og krav). Jeg fikk forklart hver blokk før jeg tok den inn. Valget om å ha jobbene i koden og skolene i JSON tok jeg selv etter å ha vurdert fordeler og ulemper.
Verifisering: Bygget med 0 feil og 0 advarsler. Slettet databasen, startet appen og sjekket at alle tabellene fikk riktig antall rader, og at krav og jobbstige stemmer med spilltall og beslutningsloggen. Startet på nytt og sjekket at seedingen hoppes over. Gikk gjennom spørsmålene og oppgavene for faglige feil.
```
### Ingvald
```
Dato: 29-10-2026
Verktøy brukt: Chat-GPT/ Sikt-KI-chat
Hva ble spurt: Hjelp meg å gjøre ferdig rapportmal
Hva ble brukt: Forsalg til markdown oppsett osv.
Verifisering: Så greit ut.

Dato:
Verktøy brukt: Claude/ Chat-GPT/ Sikt-KI-chat/ osv.
Hva ble spurt:  
Hva ble brukt:
Verifisering:
```