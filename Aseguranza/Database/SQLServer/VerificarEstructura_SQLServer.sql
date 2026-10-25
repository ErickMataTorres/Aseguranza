/*
    Verificación de estructura Aseguranza
    Solo lectura.
    Ejecutar DESPUÉS de AseguranzaBD_maestro_revisado.sql.
*/

SET NOCOUNT ON;

PRINT '=== RESUMEN DE OBJETOS ===';

SELECT
    (SELECT COUNT(*) FROM sys.tables WHERE is_ms_shipped = 0) AS Tablas,
    (
        SELECT COUNT(*)
        FROM sys.procedures
        WHERE is_ms_shipped = 0
    ) AS Procedimientos,
    (
        SELECT COUNT(*)
        FROM sys.triggers
        WHERE parent_class_desc = 'OBJECT_OR_COLUMN'
          AND is_ms_shipped = 0
    ) AS Triggers;

PRINT '';
PRINT '=== TABLAS HDC ===';

SELECT
    T.name AS Tabla
FROM sys.tables AS T
WHERE T.name IN
(
    'EquivalenciaLineaHdc',
    'EquivalenciaPlantaHdc',
    'EquivalenciaTurnoHdc',
    'ImportacionHdc',
    'PerfilHdcTrabajador',
    'SchemaVersion'
)
ORDER BY T.name;

PRINT '';
PRINT '=== COLUMNAS CRITICAS DE TRABAJADOR ===';

SELECT
    C.name AS Columna,
    TYPE_NAME(C.user_type_id) AS Tipo,
    C.max_length AS Longitud,
    C.is_nullable AS PermiteNull
FROM sys.columns AS C
WHERE C.object_id = OBJECT_ID(N'dbo.Trabajador')
  AND C.name IN
(
    'Id',
    'NoReloj',
    'Nombre',
    'RutaFoto',
    'IdLocalidad',
    'IdTurno',
    'IdLinea',
    'IdPlanta'
)
ORDER BY C.column_id;

PRINT '';
PRINT '=== EQUIVALENCIA DE PLANTA HDC ===';

SELECT
    C.name AS Columna,
    TYPE_NAME(C.user_type_id) AS Tipo,
    C.max_length AS Longitud,
    C.is_nullable AS PermiteNull
FROM sys.columns AS C
WHERE C.object_id = OBJECT_ID(N'dbo.EquivalenciaPlantaHdc')
ORDER BY C.column_id;

PRINT '';
PRINT '=== RESTRICCIONES HDC ===';

SELECT
    CC.name AS Restriccion,
    OBJECT_NAME(CC.parent_object_id) AS Tabla,
    CC.definition AS Definicion
FROM sys.check_constraints AS CC
WHERE OBJECT_NAME(CC.parent_object_id) IN
(
    'EquivalenciaLineaHdc',
    'EquivalenciaPlantaHdc'
)
ORDER BY Tabla, Restriccion;

PRINT '';
PRINT '=== TRIGGER PLANTA / LINEA ===';

SELECT
    TR.name AS TriggerNombre,
    OBJECT_NAME(TR.parent_id) AS Tabla,
    TR.is_disabled AS Deshabilitado
FROM sys.triggers AS TR
WHERE TR.name = 'TR_Trabajador_ValidarLineaPlanta';

PRINT '';
PRINT '=== PROCEDIMIENTOS ===';

SELECT
    P.name AS Procedimiento
FROM sys.procedures AS P
WHERE P.is_ms_shipped = 0
ORDER BY P.name;

PRINT '';
PRINT '=== INTEGRIDAD REFERENCIAL ===';

DBCC CHECKCONSTRAINTS WITH ALL_CONSTRAINTS;
