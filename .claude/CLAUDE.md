# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project

DebateAnalyzer is a SaaS app that analyzes debate videos downloaded from YouTube: fallacy
detection, fact-checking, and argument explanation, with several features backed by AI.

The codebase is currently a fresh scaffold (Aspire AppHost + default .NET Web API + default
Angular CLI app) — no feature code exists yet. The target architectures below are the intended
direction to follow as features are built, not yet-realized structure to go looking for.

- **Backend** (`src/DebateAnalyzer.Api`): .NET 10 Web API, intended to grow into **Clean
  Architecture** (Domain / Application / Infrastructure / Api layers). No such layering exists
  yet — `Program.cs` still has the default weather-forecast template endpoint. When adding
  backend features, set up the layer separation (as separate projects/folders) rather than
  piling logic into the Api project.
- **Frontend** (`src/DebateAnalyzer.Web`): Angular 22 app, intended to adopt **Feature Slice
  Design** (features organized as self-contained vertical slices — e.g. by domain capability
  like `fallacy-analysis`, `fact-checking`, `argument-explanation` — rather than by technical
  layer). Currently just the default `ng new` scaffold under `src/app`.

## Orchestration (.NET Aspire)

The whole app (API + Angular dev server) is run together via **Aspire**, defined in
`apphost.cs` at the repo root:
- `api` — the `DebateAnalyzer.Api` project
- `web` — the Angular app run via Vite (`AddViteApp`, runs `npm start` i.e. `ng serve`), wired
  to reference and wait for `api`, with external HTTP endpoints exposed

Use the `aspire` skill for anything involving `aspire start/stop/logs/resource/dashboard/doctor`
etc. — it is the router for Aspire operations in this repo and has safety guardrails baked in.
Don't run `dotnet run` on the AppHost directly or shell out to raw `aspire` CLI commands without
consulting that skill first.

`App.ServiceDefaults` is the shared Aspire service-defaults project (referenced by `api`):
wires up OpenTelemetry, health checks (`/health`, `/alive`), and service discovery with
resilience-enabled `HttpClient`s. Any new backend service project should reference this too.

## Commands

### Backend (`src/DebateAnalyzer.Api`, .NET 10)
```bash
dotnet build                                   # from repo root or src/DebateAnalyzer.Api
dotnet test                                    # once test projects exist
dotnet test --filter FullyQualifiedName~TestName   # run a single test
```

### Frontend (`src/DebateAnalyzer.Web`, Angular 22 + Vitest)
```bash
npm start                  # ng serve — dev server at http://localhost:4200
npm run build              # ng build — output to dist/
npm run watch              # ng build --watch --configuration development
npm test                   # ng test — runs Vitest
ng generate component <name>   # scaffolding, run from src/DebateAnalyzer.Web
```
To run a single spec file with Vitest directly: `npx vitest run <path-to-spec>` from
`src/DebateAnalyzer.Web`.

Styling uses Tailwind CSS v4 (`@tailwindcss/postcss`, configured via `.postcssrc.json`).
Formatting uses Prettier (`.prettierrc`).

## Code style

- Readability is a priority. Extract short but semantically distinct blocks of
  logic (even just 2-3 lines) into their own well-named private method rather
  than leaving them inline in an event handler or larger method. Prefer a
  short orchestrating method that delegates to named steps over one method
  that inlines everything.

## Notes
- Target framework is `net10.0`; NuGet is restricted to `nuget.org` only (see `nuget.config`).
- No solution (`.sln`) file exists yet — projects are referenced directly by path/`apphost.cs`.
