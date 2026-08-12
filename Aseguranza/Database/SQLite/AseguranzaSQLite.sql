PRAGMA foreign_keys = ON;

CREATE TABLE IF NOT EXISTS SchemaVersion (
    Version INTEGER PRIMARY KEY,
    FechaAplicacion TEXT NOT NULL DEFAULT (datetime('now', 'localtime')),
    Descripcion TEXT NOT NULL
);

CREATE TABLE IF NOT EXISTS Localidad (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Nombre TEXT NOT NULL
);

CREATE TABLE IF NOT EXISTS Turno (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Nombre TEXT NOT NULL
);

CREATE TABLE IF NOT EXISTS Planta (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Nombre TEXT NOT NULL
);

CREATE TABLE IF NOT EXISTS Linea (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Nombre TEXT NOT NULL,
    IdPlanta INTEGER NOT NULL,
    CONSTRAINT UQ_Linea_Planta_Nombre UNIQUE (IdPlanta, Nombre),
    CONSTRAINT FK_Linea_Planta
        FOREIGN KEY (IdPlanta) REFERENCES Planta(Id)
);

CREATE TABLE IF NOT EXISTS Proceso (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Nombre TEXT NOT NULL,
    Descripcion TEXT NULL,
    VigenciaMeses INTEGER NOT NULL
        CHECK (VigenciaMeses > 0)
);

CREATE TABLE IF NOT EXISTS Trabajador (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    NoReloj TEXT NOT NULL UNIQUE,
    Nombre TEXT NOT NULL,
    RutaFoto TEXT NULL,
    IdLocalidad INTEGER NOT NULL,
    IdTurno INTEGER NOT NULL,
    IdLinea INTEGER NOT NULL,
    CONSTRAINT FK_Trabajador_Localidad
        FOREIGN KEY (IdLocalidad) REFERENCES Localidad(Id),
    CONSTRAINT FK_Trabajador_Turno
        FOREIGN KEY (IdTurno) REFERENCES Turno(Id),
    CONSTRAINT FK_Trabajador_Linea
        FOREIGN KEY (IdLinea) REFERENCES Linea(Id)
);

CREATE TABLE IF NOT EXISTS Certificador (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    IdTrabajador INTEGER NOT NULL,
    CONSTRAINT FK_Certificador_Trabajador
        FOREIGN KEY (IdTrabajador) REFERENCES Trabajador(Id)
);

CREATE TABLE IF NOT EXISTS Certificacion (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    IdTrabajador INTEGER NOT NULL,
    IdProceso INTEGER NOT NULL,
    FechaCertificacion TEXT NOT NULL,
    FechaVencimiento TEXT NOT NULL,
    IdCertificador INTEGER NOT NULL,
    Comentario TEXT NULL,
    CONSTRAINT UQ_Certificacion_Trabajador_Proceso
        UNIQUE (IdTrabajador, IdProceso),
    CONSTRAINT CK_Fechas_Certificacion
        CHECK (date(FechaVencimiento) > date(FechaCertificacion)),
    CONSTRAINT FK_Certificacion_Trabajador
        FOREIGN KEY (IdTrabajador) REFERENCES Trabajador(Id),
    CONSTRAINT FK_Certificacion_Proceso
        FOREIGN KEY (IdProceso) REFERENCES Proceso(Id),
    CONSTRAINT FK_Certificacion_Certificador
        FOREIGN KEY (IdCertificador) REFERENCES Certificador(Id)
);

CREATE TABLE IF NOT EXISTS CertificacionAnulacion (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    IdCertificacion INTEGER NOT NULL,
    TipoAnulacion TEXT NOT NULL,
    FechaInicio TEXT NOT NULL DEFAULT (date('now', 'localtime')),
    FechaFin TEXT NULL,
    EsPermanente INTEGER NOT NULL DEFAULT 0
        CHECK (EsPermanente IN (0, 1)),
    Comentario TEXT NOT NULL,
    Activa INTEGER NOT NULL DEFAULT 1
        CHECK (Activa IN (0, 1)),
    FechaRegistro TEXT NOT NULL DEFAULT (datetime('now', 'localtime')),
    FechaModificacion TEXT NULL,
    CONSTRAINT FK_CertificacionAnulacion_Certificacion
        FOREIGN KEY (IdCertificacion) REFERENCES Certificacion(Id)
);

CREATE TABLE IF NOT EXISTS ExpedienteTrabajador (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    IdTrabajador INTEGER NOT NULL,
    NombreOriginal TEXT NOT NULL,
    NombreArchivo TEXT NOT NULL,
    Extension TEXT NOT NULL,
    RutaArchivo TEXT NOT NULL,
    TipoArchivo TEXT NULL,
    Comentario TEXT NULL,
    Activo INTEGER NOT NULL DEFAULT 1
        CHECK (Activo IN (0, 1)),
    FechaRegistro TEXT NOT NULL DEFAULT (datetime('now', 'localtime')),
    FechaModificacion TEXT NULL,
    CONSTRAINT FK_ExpedienteTrabajador_Trabajador
        FOREIGN KEY (IdTrabajador) REFERENCES Trabajador(Id)
);


CREATE UNIQUE INDEX IF NOT EXISTS UX_Turno_Nombre_Normalizado
    ON Turno (UPPER(TRIM(Nombre)));


CREATE UNIQUE INDEX IF NOT EXISTS UX_Localidad_Nombre_Normalizado
    ON Localidad (UPPER(TRIM(Nombre)));


CREATE UNIQUE INDEX IF NOT EXISTS UX_Planta_Nombre_Normalizado
    ON Planta (UPPER(TRIM(Nombre)));


CREATE UNIQUE INDEX IF NOT EXISTS UX_Proceso_Nombre_Normalizado
    ON Proceso (UPPER(TRIM(Nombre)));


CREATE UNIQUE INDEX IF NOT EXISTS UX_Linea_Planta_Nombre_Normalizado
    ON Linea (IdPlanta, UPPER(TRIM(Nombre)));

CREATE INDEX IF NOT EXISTS IX_Linea_IdPlanta
    ON Linea(IdPlanta);

CREATE INDEX IF NOT EXISTS IX_Trabajador_IdLocalidad
    ON Trabajador(IdLocalidad);

CREATE INDEX IF NOT EXISTS IX_Trabajador_IdTurno
    ON Trabajador(IdTurno);

CREATE INDEX IF NOT EXISTS IX_Trabajador_IdLinea
    ON Trabajador(IdLinea);

CREATE INDEX IF NOT EXISTS IX_Certificador_IdTrabajador
    ON Certificador(IdTrabajador);

CREATE INDEX IF NOT EXISTS IX_Certificacion_IdTrabajador
    ON Certificacion(IdTrabajador);

CREATE INDEX IF NOT EXISTS IX_Certificacion_IdProceso
    ON Certificacion(IdProceso);

CREATE INDEX IF NOT EXISTS IX_Certificacion_FechaVencimiento
    ON Certificacion(FechaVencimiento);

CREATE INDEX IF NOT EXISTS IX_CertificacionAnulacion_IdCertificacion
    ON CertificacionAnulacion(IdCertificacion);

CREATE INDEX IF NOT EXISTS IX_ExpedienteTrabajador_IdTrabajador
    ON ExpedienteTrabajador(IdTrabajador);

INSERT OR IGNORE INTO SchemaVersion (
    Version,
    Descripcion
)
VALUES (
    1,
    'Esquema SQLite inicial de Aseguranza'
);

INSERT OR IGNORE INTO SchemaVersion (
    Version,
    Descripcion
)
VALUES (
    2,
    'Turno protegido contra nombres duplicados normalizados'
);

INSERT OR IGNORE INTO SchemaVersion (
    Version,
    Descripcion
)
VALUES (
    3,
    'Planta protegida contra nombres duplicados normalizados'
);

INSERT OR IGNORE INTO SchemaVersion (
    Version,
    Descripcion
)
VALUES (
    4,
    'Linea protegida por planta contra nombres duplicados normalizados'
);

INSERT OR IGNORE INTO SchemaVersion (
    Version,
    Descripcion
)
VALUES (
    5,
    'Proceso protegido contra nombres duplicados normalizados'
);

INSERT OR IGNORE INTO SchemaVersion (
    Version,
    Descripcion
)
VALUES (
    6,
    'Localidad protegida contra nombres duplicados normalizados'
);

