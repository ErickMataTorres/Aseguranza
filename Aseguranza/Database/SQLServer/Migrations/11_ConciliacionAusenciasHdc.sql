SET XACT_ABORT ON;
BEGIN TRANSACTION;

IF OBJECT_ID(N'dbo.ImportacionHdc', N'U') IS NULL
    THROW 50020, 'Missing dbo.ImportacionHdc; apply the HDC schema first.', 1;
IF OBJECT_ID(N'dbo.SchemaVersion', N'U') IS NULL
    THROW 50021, 'Missing dbo.SchemaVersion; apply migration 10 first.', 1;
IF NOT EXISTS (SELECT 1 FROM dbo.SchemaVersion WHERE Version = 10)
    THROW 50022, 'Migration 10 must be deployed before migration 11.', 1;

IF COL_LENGTH(N'dbo.ImportacionHdc', N'ConciliarAusencias') IS NULL
    ALTER TABLE dbo.ImportacionHdc ADD ConciliarAusencias bit NOT NULL
        CONSTRAINT DF_ImportacionHdc_ConciliarAusencias DEFAULT (0) WITH VALUES;

IF COL_LENGTH(N'dbo.ImportacionHdc', N'HuellaSha256') IS NULL
    ALTER TABLE dbo.ImportacionHdc ADD HuellaSha256 varchar(64) NULL;

IF OBJECT_ID(N'dbo.ImportacionHdcAlcance', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.ImportacionHdcAlcance (
        IdImportacionHdc int NOT NULL,
        CodigoLocalidadHdc varchar(50) NOT NULL,
        CONSTRAINT PK_ImportacionHdcAlcance
            PRIMARY KEY (IdImportacionHdc, CodigoLocalidadHdc),
        CONSTRAINT FK_ImportacionHdcAlcance_Importacion
            FOREIGN KEY (IdImportacionHdc) REFERENCES dbo.ImportacionHdc(Id)
    );
END;

IF COL_LENGTH(N'dbo.ImportacionHdc', N'ConciliarAusencias') IS NULL
    OR COL_LENGTH(N'dbo.ImportacionHdc', N'HuellaSha256') IS NULL
    OR OBJECT_ID(N'dbo.ImportacionHdcAlcance', N'U') IS NULL
    THROW 50023, 'Migration 11 schema validation failed.', 1;

IF NOT EXISTS (
    SELECT 1 FROM sys.columns
    WHERE object_id = OBJECT_ID(N'dbo.ImportacionHdc')
      AND name = N'ConciliarAusencias'
      AND system_type_id = TYPE_ID(N'bit')
      AND is_nullable = 0
      AND default_object_id <> 0
)
    THROW 50024, 'ConciliarAusencias must be a non-null bit with default false.', 1;

IF NOT EXISTS (
    SELECT 1 FROM sys.key_constraints
    WHERE parent_object_id = OBJECT_ID(N'dbo.ImportacionHdcAlcance')
      AND type = 'PK'
)
    OR NOT EXISTS (
        SELECT 1 FROM sys.foreign_keys
        WHERE parent_object_id = OBJECT_ID(N'dbo.ImportacionHdcAlcance')
          AND referenced_object_id = OBJECT_ID(N'dbo.ImportacionHdc')
    )
    THROW 50025, 'Import scope key or foreign key is missing.', 1;

IF NOT EXISTS (SELECT 1 FROM dbo.SchemaVersion WHERE Version = 11)
    INSERT INTO dbo.SchemaVersion(Version, FechaAplicacion, Descripcion)
    VALUES (11, SYSUTCDATETIME(),
        'Conciliacion de ausencias HDC con alcance y huella de origen');

COMMIT TRANSACTION;
