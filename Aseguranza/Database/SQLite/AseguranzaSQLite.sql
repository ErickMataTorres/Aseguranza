PRAGMA foreign_keys = OFF;

BEGIN TRANSACTION;

CREATE TABLE IF NOT EXISTS "Localidad"
(
    "Id" INTEGER PRIMARY KEY AUTOINCREMENT,
    "Nombre" TEXT NOT NULL
);

CREATE TABLE IF NOT EXISTS "Turno"
(
    "Id" INTEGER PRIMARY KEY AUTOINCREMENT,
    "Nombre" TEXT NOT NULL
);

CREATE TABLE IF NOT EXISTS "Planta"
(
    "Id" INTEGER PRIMARY KEY AUTOINCREMENT,
    "Nombre" TEXT NOT NULL
);

CREATE TABLE IF NOT EXISTS "Linea"
(
    "Id" INTEGER PRIMARY KEY AUTOINCREMENT,
    "Nombre" TEXT NOT NULL,
    "IdPlanta" INTEGER NOT NULL,

    CONSTRAINT "UQ_Linea_Planta_Nombre"
        UNIQUE("IdPlanta", "Nombre"),

    CONSTRAINT "FK_Linea_Planta"
        FOREIGN KEY("IdPlanta")
        REFERENCES "Planta"("Id")
);

CREATE TABLE IF NOT EXISTS "Proceso"
(
    "Id" INTEGER PRIMARY KEY AUTOINCREMENT,
    "Nombre" TEXT NOT NULL,
    "Descripcion" TEXT,
    "VigenciaMeses" INTEGER NOT NULL
        CHECK("VigenciaMeses" > 0)
);

CREATE TABLE IF NOT EXISTS "Trabajador"
(
    "Id" INTEGER PRIMARY KEY AUTOINCREMENT,
    "NoReloj" TEXT NOT NULL UNIQUE,
    "Nombre" TEXT NOT NULL,
    "RutaFoto" TEXT NULL,
    "IdLocalidad" INTEGER NULL,
    "IdTurno" INTEGER NOT NULL,
    "IdPlanta" INTEGER NOT NULL,
    "IdLinea" INTEGER NULL,

    CONSTRAINT "FK_Trabajador_Localidad"
        FOREIGN KEY("IdLocalidad")
        REFERENCES "Localidad"("Id"),

    CONSTRAINT "FK_Trabajador_Turno"
        FOREIGN KEY("IdTurno")
        REFERENCES "Turno"("Id"),

    CONSTRAINT "FK_Trabajador_Planta"
        FOREIGN KEY("IdPlanta")
        REFERENCES "Planta"("Id"),

    CONSTRAINT "FK_Trabajador_Linea"
        FOREIGN KEY("IdLinea")
        REFERENCES "Linea"("Id")
);

CREATE TABLE IF NOT EXISTS "Certificador"
(
    "Id" INTEGER PRIMARY KEY AUTOINCREMENT,
    "IdTrabajador" INTEGER NOT NULL,

    CONSTRAINT "FK_Certificador_Trabajador"
        FOREIGN KEY("IdTrabajador")
        REFERENCES "Trabajador"("Id")
);

CREATE TABLE IF NOT EXISTS "Certificacion"
(
    "Id" INTEGER PRIMARY KEY AUTOINCREMENT,
    "IdTrabajador" INTEGER NOT NULL,
    "IdProceso" INTEGER NOT NULL,
    "FechaCertificacion" TEXT NOT NULL,
    "FechaVencimiento" TEXT NOT NULL,
    "IdCertificador" INTEGER NOT NULL,
    "Comentario" TEXT,

    CONSTRAINT "UQ_Certificacion_Trabajador_Proceso"
        UNIQUE("IdTrabajador", "IdProceso"),

    CONSTRAINT "FK_Certificacion_Certificador"
        FOREIGN KEY("IdCertificador")
        REFERENCES "Certificador"("Id"),

    CONSTRAINT "FK_Certificacion_Proceso"
        FOREIGN KEY("IdProceso")
        REFERENCES "Proceso"("Id"),

    CONSTRAINT "FK_Certificacion_Trabajador"
        FOREIGN KEY("IdTrabajador")
        REFERENCES "Trabajador"("Id"),

    CONSTRAINT "CK_Fechas_Certificacion"
        CHECK(
            date("FechaVencimiento")
            >
            date("FechaCertificacion")
        )
);

CREATE TABLE IF NOT EXISTS "CertificacionAnulacion"
(
    "Id" INTEGER PRIMARY KEY AUTOINCREMENT,
    "IdCertificacion" INTEGER NOT NULL,
    "TipoAnulacion" TEXT NOT NULL,
    "FechaInicio" TEXT NOT NULL
        DEFAULT (date('now', 'localtime')),
    "FechaFin" TEXT,
    "EsPermanente" INTEGER NOT NULL
        DEFAULT 0
        CHECK("EsPermanente" IN (0, 1)),
    "Comentario" TEXT NOT NULL,
    "Activa" INTEGER NOT NULL
        DEFAULT 1
        CHECK("Activa" IN (0, 1)),
    "FechaRegistro" TEXT NOT NULL
        DEFAULT (datetime('now', 'localtime')),
    "FechaModificacion" TEXT,

    CONSTRAINT "FK_CertificacionAnulacion_Certificacion"
        FOREIGN KEY("IdCertificacion")
        REFERENCES "Certificacion"("Id")
);

CREATE TABLE IF NOT EXISTS "ExpedienteTrabajador"
(
    "Id" INTEGER PRIMARY KEY AUTOINCREMENT,
    "IdTrabajador" INTEGER NOT NULL,
    "NombreOriginal" TEXT NOT NULL,
    "NombreArchivo" TEXT NOT NULL,
    "Extension" TEXT NOT NULL,
    "RutaArchivo" TEXT NOT NULL,
    "TipoArchivo" TEXT,
    "Comentario" TEXT,
    "Activo" INTEGER NOT NULL
        DEFAULT 1
        CHECK("Activo" IN (0, 1)),
    "FechaRegistro" TEXT NOT NULL
        DEFAULT (datetime('now', 'localtime')),
    "FechaModificacion" TEXT,

    CONSTRAINT "FK_ExpedienteTrabajador_Trabajador"
        FOREIGN KEY("IdTrabajador")
        REFERENCES "Trabajador"("Id")
);

CREATE TABLE IF NOT EXISTS "ImportacionHdc"
(
    "Id" INTEGER PRIMARY KEY AUTOINCREMENT,
    "NombreArchivo" TEXT NOT NULL,
    "NombreHoja" TEXT NOT NULL DEFAULT 'HDC',
    "FechaImportacion" TEXT NOT NULL
        DEFAULT (datetime('now', 'localtime')),
    "TotalRegistros" INTEGER NOT NULL DEFAULT 0,
    "Nuevos" INTEGER NOT NULL DEFAULT 0,
    "Actualizados" INTEGER NOT NULL DEFAULT 0,
    "SinCambios" INTEGER NOT NULL DEFAULT 0,
    "ConAdvertencias" INTEGER NOT NULL DEFAULT 0,
    "Observaciones" TEXT NULL
);

CREATE TABLE IF NOT EXISTS "PerfilHdcTrabajador"
(
    "Id" INTEGER PRIMARY KEY AUTOINCREMENT,
    "IdTrabajador" INTEGER NOT NULL,
    "IdImportacionHdc" INTEGER NULL,

    "EmpleadoHdc" TEXT NULL,
    "NombreHdc" TEXT NULL,
    "LocalidadHdc" TEXT NULL,
    "TurnoHdc" TEXT NULL,
    "FechaServicio" TEXT NULL,
    "DepartamentoHdc" TEXT NULL,
    "LineaHdc" TEXT NULL,
    "PuestoHdc" TEXT NULL,
    "CategoriaHdc" TEXT NULL,
    "PositionHdc" TEXT NULL,
    "FunctionHdc" TEXT NULL,
    "ProcesoHdc" TEXT NULL,
    "DptoHdc" TEXT NULL,

    "FechaUltimaImportacion" TEXT NOT NULL
        DEFAULT (datetime('now', 'localtime')),

    "EncontradoUltimoHdc" INTEGER NOT NULL
        DEFAULT 1
        CHECK("EncontradoUltimoHdc" IN (0, 1)),

    CONSTRAINT "UQ_PerfilHdcTrabajador_IdTrabajador"
        UNIQUE("IdTrabajador"),

    CONSTRAINT "FK_PerfilHdcTrabajador_Trabajador"
        FOREIGN KEY("IdTrabajador")
        REFERENCES "Trabajador"("Id")
        ON DELETE CASCADE,

    CONSTRAINT "FK_PerfilHdcTrabajador_Importacion"
        FOREIGN KEY("IdImportacionHdc")
        REFERENCES "ImportacionHdc"("Id")
        ON DELETE SET NULL
);

CREATE TABLE IF NOT EXISTS "EquivalenciaPlantaHdc"
(
    "Id" INTEGER PRIMARY KEY AUTOINCREMENT,
    "CodigoLocalidadHdc" TEXT NOT NULL,
    "IdPlanta" INTEGER NOT NULL,
    "Activo" INTEGER NOT NULL
        DEFAULT 1
        CHECK("Activo" IN (0, 1)),
    "Comentario" TEXT NULL,

    CONSTRAINT "UQ_EquivalenciaPlantaHdc_Codigo"
        UNIQUE("CodigoLocalidadHdc"),

    CONSTRAINT "FK_EquivalenciaPlantaHdc_Planta"
        FOREIGN KEY("IdPlanta")
        REFERENCES "Planta"("Id")
);

CREATE TABLE IF NOT EXISTS "EquivalenciaTurnoHdc"
(
    "Id" INTEGER PRIMARY KEY AUTOINCREMENT,
    "ValorTurnoHdc" TEXT NOT NULL,
    "IdTurno" INTEGER NOT NULL,
    "Activo" INTEGER NOT NULL
        DEFAULT 1
        CHECK("Activo" IN (0, 1)),
    "Comentario" TEXT NULL,

    CONSTRAINT "UQ_EquivalenciaTurnoHdc_Valor"
        UNIQUE("ValorTurnoHdc"),

    CONSTRAINT "FK_EquivalenciaTurnoHdc_Turno"
        FOREIGN KEY("IdTurno")
        REFERENCES "Turno"("Id")
);

CREATE TABLE IF NOT EXISTS "EquivalenciaLineaHdc"
(
    "Id" INTEGER PRIMARY KEY AUTOINCREMENT,
    "CodigoLocalidadHdc" TEXT NOT NULL,
    "ValorLineaHdc" TEXT NOT NULL,
    "Accion" TEXT NOT NULL
        DEFAULT 'MAPEAR'
        CHECK(
            "Accion" IN
            (
                'MAPEAR',
                'SIN_ASIGNAR',
                'IGNORAR'
            )
        ),
    "IdLinea" INTEGER NULL,
    "Activo" INTEGER NOT NULL
        DEFAULT 1
        CHECK("Activo" IN (0, 1)),
    "Comentario" TEXT NULL,

    CONSTRAINT "UQ_EquivalenciaLineaHdc_Origen"
        UNIQUE(
            "CodigoLocalidadHdc",
            "ValorLineaHdc"
        ),

    CONSTRAINT "CK_EquivalenciaLineaHdc_Accion"
        CHECK
        (
            (
                "Accion" = 'MAPEAR'
                AND "IdLinea" IS NOT NULL
            )
            OR
            (
                "Accion" IN
                (
                    'SIN_ASIGNAR',
                    'IGNORAR'
                )
                AND "IdLinea" IS NULL
            )
        ),

    CONSTRAINT "FK_EquivalenciaLineaHdc_Linea"
        FOREIGN KEY("IdLinea")
        REFERENCES "Linea"("Id")
);

CREATE TABLE IF NOT EXISTS "SchemaVersion"
(
    "Version" INTEGER PRIMARY KEY,
    "FechaAplicacion" TEXT NOT NULL
        DEFAULT (datetime('now', 'localtime')),
    "Descripcion" TEXT NOT NULL
);

CREATE INDEX IF NOT EXISTS
    "IX_Linea_IdPlanta"
    ON "Linea"("IdPlanta");

CREATE INDEX IF NOT EXISTS
    "IX_Trabajador_IdLocalidad"
    ON "Trabajador"("IdLocalidad");

CREATE INDEX IF NOT EXISTS
    "IX_Trabajador_IdTurno"
    ON "Trabajador"("IdTurno");

CREATE INDEX IF NOT EXISTS
    "IX_Trabajador_IdPlanta"
    ON "Trabajador"("IdPlanta");

CREATE INDEX IF NOT EXISTS
    "IX_Trabajador_IdLinea"
    ON "Trabajador"("IdLinea");

CREATE INDEX IF NOT EXISTS
    "IX_Certificador_IdTrabajador"
    ON "Certificador"("IdTrabajador");

CREATE INDEX IF NOT EXISTS
    "IX_Certificacion_IdTrabajador"
    ON "Certificacion"("IdTrabajador");

CREATE INDEX IF NOT EXISTS
    "IX_Certificacion_IdProceso"
    ON "Certificacion"("IdProceso");

CREATE INDEX IF NOT EXISTS
    "IX_Certificacion_FechaVencimiento"
    ON "Certificacion"("FechaVencimiento");

CREATE INDEX IF NOT EXISTS
    "IX_CertificacionAnulacion_IdCertificacion"
    ON "CertificacionAnulacion"("IdCertificacion");

CREATE INDEX IF NOT EXISTS
    "IX_ExpedienteTrabajador_IdTrabajador"
    ON "ExpedienteTrabajador"("IdTrabajador");

CREATE INDEX IF NOT EXISTS
    "IX_PerfilHdcTrabajador_IdImportacionHdc"
    ON "PerfilHdcTrabajador"("IdImportacionHdc");

CREATE INDEX IF NOT EXISTS
    "IX_PerfilHdcTrabajador_LocalidadHdc"
    ON "PerfilHdcTrabajador"("LocalidadHdc");

CREATE INDEX IF NOT EXISTS
    "IX_PerfilHdcTrabajador_LineaHdc"
    ON "PerfilHdcTrabajador"("LineaHdc");

CREATE INDEX IF NOT EXISTS
    "IX_PerfilHdcTrabajador_DepartamentoHdc"
    ON "PerfilHdcTrabajador"("DepartamentoHdc");

CREATE INDEX IF NOT EXISTS
    "IX_EquivalenciaPlantaHdc_IdPlanta"
    ON "EquivalenciaPlantaHdc"("IdPlanta");

CREATE INDEX IF NOT EXISTS
    "IX_EquivalenciaTurnoHdc_IdTurno"
    ON "EquivalenciaTurnoHdc"("IdTurno");

CREATE INDEX IF NOT EXISTS
    "IX_EquivalenciaLineaHdc_IdLinea"
    ON "EquivalenciaLineaHdc"("IdLinea");

CREATE UNIQUE INDEX IF NOT EXISTS
    "UX_Linea_Planta_Nombre_Normalizado"
    ON "Linea"
    (
        "IdPlanta",
        UPPER(TRIM("Nombre"))
    );

CREATE UNIQUE INDEX IF NOT EXISTS
    "UX_Localidad_Nombre_Normalizado"
    ON "Localidad"
    (
        UPPER(TRIM("Nombre"))
    );

CREATE UNIQUE INDEX IF NOT EXISTS
    "UX_Planta_Nombre_Normalizado"
    ON "Planta"
    (
        UPPER(TRIM("Nombre"))
    );

CREATE UNIQUE INDEX IF NOT EXISTS
    "UX_Proceso_Nombre_Normalizado"
    ON "Proceso"
    (
        UPPER(TRIM("Nombre"))
    );

CREATE UNIQUE INDEX IF NOT EXISTS
    "UX_Turno_Nombre_Normalizado"
    ON "Turno"
    (
        UPPER(TRIM("Nombre"))
    );

CREATE TRIGGER IF NOT EXISTS
    "TR_Trabajador_ValidarLineaPlanta_Insert"
BEFORE INSERT ON "Trabajador"
WHEN NEW."IdLinea" IS NOT NULL
     AND NOT EXISTS
     (
         SELECT 1
         FROM "Linea"
         WHERE "Id" = NEW."IdLinea"
           AND "IdPlanta" = NEW."IdPlanta"
     )
BEGIN
    SELECT RAISE(
        ABORT,
        'La linea no pertenece a la planta.'
    );
END;

CREATE TRIGGER IF NOT EXISTS
    "TR_Trabajador_ValidarLineaPlanta_Update"
BEFORE UPDATE OF "IdPlanta", "IdLinea"
ON "Trabajador"
WHEN NEW."IdLinea" IS NOT NULL
     AND NOT EXISTS
     (
         SELECT 1
         FROM "Linea"
         WHERE "Id" = NEW."IdLinea"
           AND "IdPlanta" = NEW."IdPlanta"
     )
BEGIN
    SELECT RAISE(
        ABORT,
        'La linea no pertenece a la planta.'
    );
END;

INSERT OR IGNORE INTO "SchemaVersion"
(
    "Version",
    "Descripcion"
)
VALUES
(
    8,
    'Localidad opcional en Trabajador y estructura base para importacion HDC'
);

INSERT OR IGNORE INTO "SchemaVersion"
(
    "Version",
    "Descripcion"
)
VALUES
(
    9,
    'Trabajador con planta directa y linea opcional'
);

COMMIT;

PRAGMA foreign_keys = ON;
