CREATE TABLE IF NOT EXISTS "TrabajadorEstadoHistorial"
(
    "Id" INTEGER PRIMARY KEY AUTOINCREMENT,
    "IdTrabajador" INTEGER NOT NULL,
    "ActivoAnterior" INTEGER NULL CHECK("ActivoAnterior" IN (0, 1)),
    "ActivoNuevo" INTEGER NOT NULL CHECK("ActivoNuevo" IN (0, 1)),
    "FechaCambioUtc" TEXT NOT NULL,
    "Origen" TEXT NOT NULL CHECK("Origen" IN ('HDC', 'MANUAL')),
    "IdImportacionHdc" INTEGER NULL,
    "Motivo" TEXT NULL,
    FOREIGN KEY("IdTrabajador") REFERENCES "Trabajador"("Id"),
    FOREIGN KEY("IdImportacionHdc") REFERENCES "ImportacionHdc"("Id")
);

CREATE INDEX IF NOT EXISTS "IX_TrabajadorEstadoHistorial_Trabajador_Fecha"
    ON "TrabajadorEstadoHistorial"("IdTrabajador", "FechaCambioUtc");

INSERT OR IGNORE INTO "SchemaVersion" ("Version", "Descripcion")
VALUES (10, 'Estado activo de trabajador e historial de cambios');
