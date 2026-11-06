# Features L T D

Tall og regler er bestemt i [beslutningsloggen](beslutningslogg.md) og [spilltall](spilltall.md). Hvis noe her er uklart, er det de som gjelder.

## Main features

### Spiller

- male / female
- bolignivå / tid per dag / leie
- jobb / lønn
- studienivå / jobber tilgjengelige
- penger /damn son

### Quiz

- ett dataset per skole, unntatt Fullstack, som ikke har egne spørsmål og trekker eksamensspørsmål fra de andre skolene
- .json = questions
- multiple choice på learning
- insert field på jobber

### Jobber

når man jobber er det insert field

Rekkefølge på jobber er IT Help -> Stud.ass (Frontend, Backend, Database) -> Office -> Startup -> Tech Corp

- IT Help = alltid åpen
- Stud.ass Frontend / Backend / Database = bestått nivå 1 i sin skole & jobbet som IT Help
- Office = bestått Fullstack 1 & jobbet i alle tre Stud.ass-jobbene
- Startup = bestått Fullstack 2 & jobbet på Office
- Tech Corp = bestått Fullstack 3 & jobbet på Startup

### Skoler

- Kompendie tilgjengelig på hver enkelt skole, unntatt Fullstack
- egenstudie = multiple choice quiz med forklaring etter svar
- eksamen 5 spørsmål uten forklaring, 4 riktige = bestått
- Fullstack = bare eksamen. Krever bestått samme nivå på alle de andre skolene, og spørsmålene trekkes fra dem

### Boliger

- 5 ulike bolignivåer fra lav til høy - telt, dorm, hybel, leilighet, penthouse
- For hver oppgradering i bolig øker tiden per dag med 2 timer, fra 12-20 timer.
- Boligene har ulik husleie per natt, der telt er helt gratis. Husleien betales på starten av hver nye dag, etter at lønnen er utbetalt. Har man ikke råd, blir man kastet ut til telt.

### Achievements

- slacker = ikke åpne kompendiet før fullført eksamen
- fullført alle spørsmål på en skole
- klart eksamen på første forsøk

### Time Management

tid er direkte knyttet til hus  
rekkefølge på bolig: tent -> dorm -> hybel -> Apt. -> Penthouse  
man legger på 2 t per upgrade
endre klokke til at rødt er tid du IKKE har, at viseren beveger seg mot det røde.

- Tent = 12 timer til rådighet per dag, dorm = 14 t osv.

### Money

#### microtransaksjoner
