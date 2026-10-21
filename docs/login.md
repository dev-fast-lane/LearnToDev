# Innlogging

Innloggingen bruker ASP.NET Core Identity med en egen `AccountController` (MVC). Vi bruker ikke de ferdige Identity UI-sidene (Razor Pages).

## Filer

| Fil | Innhold |
| --- | --- |
| `Data/ApplicationDbContext.cs` | Arver fra `IdentityDbContext` og gir oss Identity-tabellene |
| `Program.cs` | `AddIdentity<IdentityUser, IdentityRole>` og `UseAuthentication()` |
| `Models/LoginViewModel.cs` | Feltene i innloggingsskjemaet |
| `Controllers/AccountController.cs` | `Login` (GET viser skjemaet, POST logger inn) |

## Flyt for POST `Login`

```
Skjema sendt inn
  → Ekte skjema? (token)          nei → avvist
  → Felter fylt ut?               nei → vis skjema med feil
  → Finn bruker (e-post/navn)
  → Riktig passord?               ja  → cookie + send videre
  → Låst konto?                   ja  → «For mange feilforsøk»
  → Ellers                            → «Ugyldig innlogging»
```

## Sikkerhetsvalg

- **Anti-forgery-token** på skjemaet hindrer at andre nettsider sender det inn for brukeren.
- **Én felles feilmelding** («Ugyldig innlogging») avslører ikke om e-posten eller brukernavnet finnes.
- **Utestenging:** 5 feil passord låser kontoen i 5 minutter.
- **`Url.IsLocalUrl`** sørger for at brukeren bare sendes tilbake til sider på vårt eget nettsted etter innlogging.
- Passord lagres aldri, bare som hash (håndteres av Identity).

## Database

Identity-tabellene (`AspNetUsers`, `AspNetRoles` osv.) kommer med den første migreringen (#10), sammen med modellene fra ER-diagrammet.
