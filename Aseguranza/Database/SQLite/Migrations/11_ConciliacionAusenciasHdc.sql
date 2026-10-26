ALTER TABLE ImportacionHdc ADD COLUMN ConciliarAusencias INTEGER NOT NULL
    DEFAULT 0 CHECK(ConciliarAusencias IN (0, 1));
ALTER TABLE ImportacionHdc ADD COLUMN HuellaSha256 TEXT NULL;
CREATE TABLE IF NOT EXISTS ImportacionHdcAlcance
(
    IdImportacionHdc INTEGER NOT NULL,
    CodigoLocalidadHdc TEXT NOT NULL,
    PRIMARY KEY (IdImportacionHdc, CodigoLocalidadHdc),
    FOREIGN KEY (IdImportacionHdc) REFERENCES ImportacionHdc(Id)
);
INSERT INTO SchemaVersion(Version, Descripcion)
VALUES (11, 'Conciliacion de ausencias HDC con alcance y huella de origen');
