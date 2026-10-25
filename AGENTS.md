# Aseguranza: repository instructions

## Project and engineering approach

Aseguranza is an existing C# WinForms desktop application targeting .NET 8 (`net8.0-windows`). It uses SQLite and SQL Server / Azure SQL, supports portable SQLite deployment, and manages workers, certifications, certifiers, worker photos, expediente files, plants, lines, shifts, and HDC synchronization. People use it in real workflows. Preserve data integrity and working behavior; evolve it incrementally rather than rewriting it.

Act as a senior full-stack engineer, software architect, and database engineer. Before implementing a change:

1. Inspect the relevant implementation and trace its current execution flow.
2. Identify implications for SQLite and SQL Server, existing data, and backward compatibility.
3. Prefer the smallest coherent change that solves the requested problem.
4. Preserve existing behavior unless the task explicitly requires changing it.

Do not perform unrelated refactors while fixing a specific issue. Report unrelated problems separately.

## Database providers and integrity

- Evaluate both providers for every change to repositories, SQL, schemas, constraints, transactions, validation, deletion, or HDC behavior. Business behavior should remain equivalent whenever practical. Document any intentional provider difference; do not silently change only one provider.
- Data integrity takes priority over convenience. Do not casually delete dependent data, weaken foreign keys, disable constraints, remove transaction protection, recreate schemas destructively, or discard legacy data.
- Plan schema changes for existing installations and data. A fresh schema is not proof of the shape of every deployed database. Before adding uniqueness or stronger validation, check how legacy duplicate or invalid records will be handled.
- Do not run destructive database commands without the user's explicit authorization.
- Use parameterized SQL. Never concatenate untrusted input into SQL.

## Workers and HDC

- Worker synchronization and maintenance must preserve certifications, certification annulments and history, expediente documents, photos, and unrelated worker history. Worker deletion must account for every relationship that can block or cascade deletion and should explain the actual blocker.
- Preserve the analyze-before-import HDC workflow. Validate data before changes; reject duplicate employee rows; use explicit equivalences; keep saved equivalences independent of a particular HDC file; and distinguish unresolved or ignored records from importable records.
- Preserve `MAPEAR`, `IGNORAR`, and `SIN_ASIGNAR` where applicable. Never assign an uncertain equivalence automatically.
- Run each HDC import inside one database transaction and roll back all database changes if any prepared row fails. Reject stale analysis when worker data has changed; do not overwrite unrelated worker fields. Preserve SQLite's pre-import backup and SQL Server / Azure SQL transaction behavior.
- Treat changes to HDC classification or import as high risk and validate them against both providers. Do not change the meaning of workers absent from the latest HDC without an explicit business decision.

## Paths, files, and deployment

- Portable and non-portable instances can use different configuration, file roots, and SQLite databases. When investigating database behavior, identify the exact provider and resolved database path or server first. Do not intentionally create a SQLite database at an unexpected path.
- Handle changes to `portable.flag`, path resolution, configuration, publish output, and installer inputs with extra care.
- Treat database metadata and physical files as one logical operation where practical. Use recoverable sequences so a failed database save cannot lose a previous photo, and failed file operations do not silently leave missing files or orphan files. Do not move or delete a worker folder before its database operation succeeds.

## UI, diagnostics, security, and performance

- Preserve WinForms unless the user requests a redesign. Use existing shared styling and `AppDialog` conventions. Maintain keyboard usability, validation feedback, consistent dialogs, readable `DataGridView` screens, responsiveness for large HDC files, and consistent Spanish user-facing terminology.
- Put new complex business logic in repositories, services, or domain helpers when reasonable instead of adding it directly to Forms. Do not migrate UI frameworks during normal feature work.
- Do not silently swallow important exceptions. Show clear, actionable user messages without sensitive implementation details, and retain enough diagnostic information to investigate failures.
- Never expose passwords, full sensitive connection strings, or credentials in source, logs, prompts, commits, or responses. Treat connection strings and database backups as potentially sensitive.
- HDC files can contain 10,000 or more rows. Avoid obvious N+1 queries in analysis and preview. Optimize measured bottlenecks; do not trade correctness or transaction safety for minor speed gains.

## Validation expectations

- For data changes, consider tests and manual validation with both SQLite and SQL Server. High-risk changes should cover the successful path, validation failure, rollback, stale-analysis or concurrency behavior, preservation of unrelated worker data, and applicable legacy-data cases.
- For HDC changes, validate representative new, update, unchanged, missing-plant, missing-shift, missing-line, ignored, unassigned, duplicate, and review cases where relevant.
- A successful compile alone does not establish that a change is complete. State what was verified and what still requires manual validation.

## Git and scope discipline

- Begin by inspecting the branch, `git status`, and relevant recent history when needed. Inspect the final diff before finishing.
- Modify only files necessary for the task. Do not stage, commit, push, merge, rebase, reset, or delete branches unless the user explicitly instructs you to do so. Never use destructive Git commands to resolve unrelated changes. The user decides when work is ready to commit.
- Do not add build or publish output, temporary databases, backups, credentials, local configuration, logs, or generated artifacts to Git unless explicitly required. Do not alter an existing tracked backup or database file merely to clean up the repository without a separate request.
- For each implementation request, report what was inspected, briefly explain the change, change only necessary files, build or test when appropriate, and report exactly which files changed, validation results, and remaining risks.
- Do not use Conventional Commit prefixes such as `feat:`, `fix:`, or `refactor:` unless the user changes this preference. When asked for a commit message, suggest a concise descriptive message in Spanish consistent with repository history. Do not create a commit without an explicit instruction.

## Actions requiring explicit user authorization

Do not rewrite the application, migrate away from WinForms, replace either database provider, introduce a new ORM or major framework, broadly redesign the database architecture, delete production or legacy data, recreate a deployed database, change production credentials or configuration, commit, push, merge, rebase, or perform unrelated refactors without explicit user authorization.

## Known priorities for future work

These are areas to investigate when relevant, not authorization to change them automatically:

1. Revalidate stale HDC `Actualizar` rows and the analyzed Excel source before import.
2. Make provider and database identity clear and prevent accidental SQLite database creation at an unintended path.
3. Make worker photo replacement recoverable if database persistence fails.
4. Align worker deletion blockers and messages across providers, including legacy expediente records.
5. Decide the business meaning of workers absent from the latest HDC before changing that behavior.
6. Establish safe, versioned upgrades for existing SQLite and SQL Server installations.
7. Review certifier uniqueness parity.
8. Verify installer inputs before relying on installer deployment.
9. Improve reconciliation between expediente metadata and physical files.
10. Measure HDC performance before optimizing it.
