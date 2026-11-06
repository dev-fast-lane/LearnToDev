# Innlogging

Innloggingen bruker ASP.NET Core Identity med en egen `AuthController` (API). React sender JSON, og API-et svarer med JSON og statuskoder. Vi bruker ikke de ferdige Identity UI-sidene (Razor Pages).

## Filer

| Fil | Innhold |
| --- | --- |
| `Data/ApplicationDbContext.cs` | Arver fra `IdentityDbContext` og gir oss Identity-tabellene |
| `Program.cs` | `AddIdentity<IdentityUser, IdentityRole>`, cookie-innstillinger og `UseAuthentication()` |
| `DTOs/LoginDto.cs`, `RegisterDto.cs` | Det React sender inn |
| `DTOs/UserDto.cs` | Det API-et svarer med |
| `Controllers/AuthController.cs` | `login`, `register`, `logout` og `me` |

## Flyt for POST `api/auth/login`

```
JSON sendt inn
  → Felter fylt ut?               nei → 400 med feilmeldinger
  → Finn bruker (e-post/navn)
  → Riktig passord?               ja  → cookie + 200 med brukeren
  → Låst konto?                   ja  → 401 «For mange feilforsøk»
  → Ellers                            → 401 «Ugyldig innlogging»
```

## Sikkerhetsvalg

- **`SameSite=Strict`** gjør at nettleseren ikke sender cookien med forespørsler fra andre nettsider. Derfor trenger vi ikke anti-forgery-token.
- **`HttpOnly`** gjør at JavaScript ikke kan lese cookien, så et XSS-angrep ikke kan stjele innloggingen.
- **DTO-er** i svarene gjør at passord-hashen aldri blir sendt til React.
- **Én felles feilmelding** («Ugyldig innlogging») avslører ikke om e-posten eller brukernavnet finnes.
- **Utestenging:** 5 feil passord låser kontoen i 5 minutter.
- Passord lagres aldri, bare som hash (håndteres av Identity).

## Database

Identity-tabellene (`AspNetUsers`, `AspNetRoles` osv.) kommer med den første migreringen (#10), sammen med modellene fra ER-diagrammet.
