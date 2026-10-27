/*
    Aseguranza - Esquema maestro SQL Server / Azure SQL
    ==================================================

    Origen:
    - Generado desde la base AseguranzaBD actualmente funcional.
    - Revisado para eliminar dependencias específicas del equipo/usuario.

    Uso:
    1. Cree o seleccione una base de datos VACÍA.
    2. Ejecute este archivo completo dentro de esa base.
    3. No ejecute este archivo sobre una base ya inicializada.

    Importante:
    - Este script NO crea la base de datos.
    - Este script NO crea usuarios ni logins.
    - No contiene datos de producción.
    - Conserva tablas, restricciones, índices, procedimientos almacenados
      y triggers del esquema actual.
    - Al no incluir USE [AseguranzaBD], puede utilizarse también sobre una
      base con otro nombre para pruebas y sobre Azure SQL seleccionando
      previamente la base de destino.
*/

/****** Objeto: Table [dbo].[Certificacion] Fecha de script: 20/09/2026 05:35:11 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Certificacion](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdTrabajador] [int] NOT NULL,
	[IdProceso] [int] NOT NULL,
	[FechaCertificacion] [date] NOT NULL,
	[FechaVencimiento] [date] NOT NULL,
	[IdCertificador] [int] NOT NULL,
	[Comentario] [varchar](300) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_Certificacion_Trabajador_Proceso] UNIQUE NONCLUSTERED 
(
	[IdTrabajador] ASC,
	[IdProceso] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Objeto: Table [dbo].[CertificacionAnulacion] Fecha de script: 20/09/2026 05:35:11 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CertificacionAnulacion](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdCertificacion] [int] NOT NULL,
	[TipoAnulacion] [varchar](50) NOT NULL,
	[FechaInicio] [date] NOT NULL,
	[FechaFin] [date] NULL,
	[EsPermanente] [bit] NOT NULL,
	[Comentario] [varchar](500) NOT NULL,
	[Activa] [bit] NOT NULL,
	[FechaRegistro] [datetime] NOT NULL,
	[FechaModificacion] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Objeto: Table [dbo].[Certificador] Fecha de script: 20/09/2026 05:35:11 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Certificador](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdTrabajador] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Objeto: Table [dbo].[EquivalenciaLineaHdc] Fecha de script: 20/09/2026 05:35:11 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EquivalenciaLineaHdc](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CodigoLocalidadHdc] [varchar](50) NOT NULL,
	[ValorLineaHdc] [varchar](100) NOT NULL,
	[Accion] [varchar](20) NOT NULL,
	[IdLinea] [int] NULL,
	[Activo] [bit] NOT NULL,
	[Comentario] [varchar](500) NULL,
 CONSTRAINT [PK_EquivalenciaLineaHdc] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_EquivalenciaLineaHdc_Origen] UNIQUE NONCLUSTERED 
(
	[CodigoLocalidadHdc] ASC,
	[ValorLineaHdc] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Objeto: Table [dbo].[EquivalenciaPlantaHdc] Fecha de script: 20/09/2026 05:35:11 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EquivalenciaPlantaHdc](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CodigoLocalidadHdc] [varchar](50) NOT NULL,
	[IdPlanta] [int] NULL,
	[Activo] [bit] NOT NULL,
	[Comentario] [varchar](500) NULL,
	[Accion] [varchar](20) NOT NULL,
 CONSTRAINT [PK_EquivalenciaPlantaHdc] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_EquivalenciaPlantaHdc_Codigo] UNIQUE NONCLUSTERED 
(
	[CodigoLocalidadHdc] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Objeto: Table [dbo].[EquivalenciaTurnoHdc] Fecha de script: 20/09/2026 05:35:11 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EquivalenciaTurnoHdc](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ValorTurnoHdc] [varchar](50) NOT NULL,
	[IdTurno] [int] NOT NULL,
	[Activo] [bit] NOT NULL,
	[Comentario] [varchar](500) NULL,
 CONSTRAINT [PK_EquivalenciaTurnoHdc] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_EquivalenciaTurnoHdc_Valor] UNIQUE NONCLUSTERED 
(
	[ValorTurnoHdc] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Objeto: Table [dbo].[ExpedienteTrabajador] Fecha de script: 20/09/2026 05:35:11 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ExpedienteTrabajador](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdTrabajador] [int] NOT NULL,
	[NombreOriginal] [varchar](255) NOT NULL,
	[NombreArchivo] [varchar](255) NOT NULL,
	[Extension] [varchar](20) NOT NULL,
	[RutaArchivo] [varchar](500) NOT NULL,
	[TipoArchivo] [varchar](50) NULL,
	[Comentario] [varchar](300) NULL,
	[Activo] [bit] NOT NULL,
	[FechaRegistro] [datetime] NOT NULL,
	[FechaModificacion] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Objeto: Table [dbo].[ImportacionHdc] Fecha de script: 20/09/2026 05:35:11 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ImportacionHdc](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[NombreArchivo] [varchar](260) NOT NULL,
	[NombreHoja] [varchar](100) NOT NULL,
	[FechaImportacion] [datetime2](0) NOT NULL,
	[TotalRegistros] [int] NOT NULL,
	[Nuevos] [int] NOT NULL,
	[Actualizados] [int] NOT NULL,
	[SinCambios] [int] NOT NULL,
	[ConAdvertencias] [int] NOT NULL,
	[ConciliarAusencias] [bit] NOT NULL CONSTRAINT [DF_ImportacionHdc_ConciliarAusencias] DEFAULT (0),

	[HuellaSha256] [varchar](64) NULL,

	[Observaciones] [varchar](1000) NULL,
 CONSTRAINT [PK_ImportacionHdc] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE TABLE [dbo].[ImportacionHdcAlcance](
    [IdImportacionHdc] [int] NOT NULL,
    [CodigoLocalidadHdc] [varchar](50) NOT NULL,
    CONSTRAINT [PK_ImportacionHdcAlcance] PRIMARY KEY
        ([IdImportacionHdc], [CodigoLocalidadHdc]),
    CONSTRAINT [FK_ImportacionHdcAlcance_Importacion]
        FOREIGN KEY ([IdImportacionHdc]) REFERENCES [dbo].[ImportacionHdc]([Id])
);
GO

/****** Objeto: Table [dbo].[Linea] Fecha de script: 20/09/2026 05:35:11 p. m. ******/

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Linea](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](100) NOT NULL,
	[IdPlanta] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_Linea_Planta_Nombre] UNIQUE NONCLUSTERED 
(
	[IdPlanta] ASC,
	[Nombre] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Objeto: Table [dbo].[Localidad] Fecha de script: 20/09/2026 05:35:11 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Localidad](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Objeto: Table [dbo].[PerfilHdcTrabajador] Fecha de script: 20/09/2026 05:35:11 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PerfilHdcTrabajador](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdTrabajador] [int] NOT NULL,
	[IdImportacionHdc] [int] NULL,
	[EmpleadoHdc] [varchar](50) NULL,
	[NombreHdc] [varchar](200) NULL,
	[LocalidadHdc] [varchar](50) NULL,
	[TurnoHdc] [varchar](50) NULL,
	[FechaServicio] [date] NULL,
	[DepartamentoHdc] [varchar](100) NULL,
	[LineaHdc] [varchar](100) NULL,
	[PuestoHdc] [varchar](150) NULL,
	[CategoriaHdc] [varchar](100) NULL,
	[PositionHdc] [varchar](100) NULL,
	[FunctionHdc] [varchar](100) NULL,
	[ProcesoHdc] [varchar](100) NULL,
	[DptoHdc] [varchar](100) NULL,
	[FechaUltimaImportacion] [datetime2](0) NOT NULL,
	[EncontradoUltimoHdc] [bit] NOT NULL,
 CONSTRAINT [PK_PerfilHdcTrabajador] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_PerfilHdcTrabajador_IdTrabajador] UNIQUE NONCLUSTERED 
(
	[IdTrabajador] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Objeto: Table [dbo].[Planta] Fecha de script: 20/09/2026 05:35:11 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Planta](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Objeto: Table [dbo].[Proceso] Fecha de script: 20/09/2026 05:35:11 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Proceso](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](100) NOT NULL,
	[Descripcion] [varchar](300) NULL,
	[VigenciaMeses] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Objeto: Table [dbo].[SchemaVersion] Fecha de script: 20/09/2026 05:35:11 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SchemaVersion](
	[Version] [int] NOT NULL,
	[FechaAplicacion] [datetime2](0) NOT NULL,
	[Descripcion] [varchar](300) NOT NULL,
 CONSTRAINT [PK_SchemaVersion] PRIMARY KEY CLUSTERED 
(
	[Version] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Objeto: Table [dbo].[Trabajador] Fecha de script: 20/09/2026 05:35:11 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Trabajador](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[NoReloj] [varchar](10) NOT NULL,
	[Nombre] [varchar](200) NOT NULL,
	[Activo] [bit] NOT NULL CONSTRAINT [DF_Trabajador_Activo] DEFAULT ((1)),
	[RutaFoto] [varchar](300) NULL,
	[IdLocalidad] [int] NULL,
	[IdTurno] [int] NOT NULL,
	[IdLinea] [int] NULL,
	[IdPlanta] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[NoReloj] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE TABLE [dbo].[TrabajadorEstadoHistorial](
    [Id] int IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [IdTrabajador] int NOT NULL,
    [ActivoAnterior] bit NULL,
    [ActivoNuevo] bit NOT NULL,
    [FechaCambioUtc] datetime2(3) NOT NULL,
    [Origen] varchar(20) NOT NULL,
    [IdImportacionHdc] int NULL,
    [Motivo] nvarchar(500) NULL,
    CONSTRAINT [CK_TrabajadorEstadoHistorial_Origen]
        CHECK ([Origen] IN ('HDC', 'MANUAL')),
    CONSTRAINT [FK_TrabajadorEstadoHistorial_Trabajador]
        FOREIGN KEY ([IdTrabajador]) REFERENCES [dbo].[Trabajador]([Id]),
    CONSTRAINT [FK_TrabajadorEstadoHistorial_Importacion]
        FOREIGN KEY ([IdImportacionHdc]) REFERENCES [dbo].[ImportacionHdc]([Id])
);
GO
CREATE INDEX [IX_TrabajadorEstadoHistorial_Trabajador_Fecha]
    ON [dbo].[TrabajadorEstadoHistorial]([IdTrabajador], [FechaCambioUtc]);
GO
INSERT INTO [dbo].[SchemaVersion]([Version], [FechaAplicacion], [Descripcion])
VALUES (10, SYSUTCDATETIME(), 'Estado activo de trabajador e historial de cambios');
GO
INSERT INTO [dbo].[SchemaVersion]([Version], [FechaAplicacion], [Descripcion])
VALUES (11, SYSUTCDATETIME(), 'Conciliacion de ausencias HDC con alcance y huella de origen');
GO
INSERT INTO [dbo].[SchemaVersion]([Version], [FechaAplicacion], [Descripcion])
VALUES (12, SYSUTCDATETIME(), 'Filtros de personal activo e inactivo en listas operativas');
GO
INSERT INTO [dbo].[SchemaVersion]([Version], [FechaAplicacion], [Descripcion])
VALUES (13, SYSUTCDATETIME(), 'Paginación de la lista de trabajadores');
GO
INSERT INTO [dbo].[SchemaVersion]([Version], [FechaAplicacion], [Descripcion])
VALUES (14, SYSUTCDATETIME(), 'Paginación de la lista de certificaciones');
GO
/****** Objeto: Table [dbo].[Turno] Fecha de script: 20/09/2026 05:35:11 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Turno](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Objeto: Index [IX_Certificacion_FechaVencimiento] Fecha de script: 20/09/2026 05:35:11 p. m. ******/
CREATE NONCLUSTERED INDEX [IX_Certificacion_FechaVencimiento] ON [dbo].[Certificacion]
(
	[FechaVencimiento] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Objeto: Index [IX_Certificacion_IdProceso] Fecha de script: 20/09/2026 05:35:11 p. m. ******/
CREATE NONCLUSTERED INDEX [IX_Certificacion_IdProceso] ON [dbo].[Certificacion]
(
	[IdProceso] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Objeto: Index [IX_Certificacion_IdTrabajador] Fecha de script: 20/09/2026 05:35:11 p. m. ******/
CREATE NONCLUSTERED INDEX [IX_Certificacion_IdTrabajador] ON [dbo].[Certificacion]
(
	[IdTrabajador] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Objeto: Index [IX_CertificacionAnulacion_IdCertificacion] Fecha de script: 20/09/2026 05:35:11 p. m. ******/
CREATE NONCLUSTERED INDEX [IX_CertificacionAnulacion_IdCertificacion] ON [dbo].[CertificacionAnulacion]
(
	[IdCertificacion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Objeto: Index [IX_Certificador_IdTrabajador] Fecha de script: 20/09/2026 05:35:11 p. m. ******/
CREATE NONCLUSTERED INDEX [IX_Certificador_IdTrabajador] ON [dbo].[Certificador]
(
	[IdTrabajador] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Objeto: Index [UX_Certificador_IdTrabajador] Fecha de script: 20/09/2026 05:35:11 p. m. ******/
CREATE UNIQUE NONCLUSTERED INDEX [UX_Certificador_IdTrabajador] ON [dbo].[Certificador]
(
	[IdTrabajador] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Objeto: Index [IX_ExpedienteTrabajador_IdTrabajador] Fecha de script: 20/09/2026 05:35:11 p. m. ******/
CREATE NONCLUSTERED INDEX [IX_ExpedienteTrabajador_IdTrabajador] ON [dbo].[ExpedienteTrabajador]
(
	[IdTrabajador] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Objeto: Index [IX_Linea_IdPlanta] Fecha de script: 20/09/2026 05:35:11 p. m. ******/
CREATE NONCLUSTERED INDEX [IX_Linea_IdPlanta] ON [dbo].[Linea]
(
	[IdPlanta] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Objeto: Index [UX_Localidad_Nombre] Fecha de script: 20/09/2026 05:35:11 p. m. ******/
CREATE UNIQUE NONCLUSTERED INDEX [UX_Localidad_Nombre] ON [dbo].[Localidad]
(
	[Nombre] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Objeto: Index [IX_PerfilHdcTrabajador_DepartamentoHdc] Fecha de script: 20/09/2026 05:35:11 p. m. ******/
CREATE NONCLUSTERED INDEX [IX_PerfilHdcTrabajador_DepartamentoHdc] ON [dbo].[PerfilHdcTrabajador]
(
	[DepartamentoHdc] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Objeto: Index [IX_PerfilHdcTrabajador_IdImportacionHdc] Fecha de script: 20/09/2026 05:35:11 p. m. ******/
CREATE NONCLUSTERED INDEX [IX_PerfilHdcTrabajador_IdImportacionHdc] ON [dbo].[PerfilHdcTrabajador]
(
	[IdImportacionHdc] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Objeto: Index [IX_PerfilHdcTrabajador_LineaHdc] Fecha de script: 20/09/2026 05:35:11 p. m. ******/
CREATE NONCLUSTERED INDEX [IX_PerfilHdcTrabajador_LineaHdc] ON [dbo].[PerfilHdcTrabajador]
(
	[LineaHdc] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Objeto: Index [IX_PerfilHdcTrabajador_LocalidadHdc] Fecha de script: 20/09/2026 05:35:11 p. m. ******/
CREATE NONCLUSTERED INDEX [IX_PerfilHdcTrabajador_LocalidadHdc] ON [dbo].[PerfilHdcTrabajador]
(
	[LocalidadHdc] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Objeto: Index [UX_Planta_Nombre] Fecha de script: 20/09/2026 05:35:11 p. m. ******/
CREATE UNIQUE NONCLUSTERED INDEX [UX_Planta_Nombre] ON [dbo].[Planta]
(
	[Nombre] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Objeto: Index [UX_Proceso_Nombre] Fecha de script: 20/09/2026 05:35:11 p. m. ******/
CREATE UNIQUE NONCLUSTERED INDEX [UX_Proceso_Nombre] ON [dbo].[Proceso]
(
	[Nombre] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Objeto: Index [IX_Trabajador_IdLinea] Fecha de script: 20/09/2026 05:35:11 p. m. ******/
CREATE NONCLUSTERED INDEX [IX_Trabajador_IdLinea] ON [dbo].[Trabajador]
(
	[IdLinea] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Objeto: Index [IX_Trabajador_IdLocalidad] Fecha de script: 20/09/2026 05:35:11 p. m. ******/
CREATE NONCLUSTERED INDEX [IX_Trabajador_IdLocalidad] ON [dbo].[Trabajador]
(
	[IdLocalidad] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Objeto: Index [IX_Trabajador_IdPlanta] Fecha de script: 20/09/2026 05:35:11 p. m. ******/
CREATE NONCLUSTERED INDEX [IX_Trabajador_IdPlanta] ON [dbo].[Trabajador]
(
	[IdPlanta] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Objeto: Index [IX_Trabajador_IdTurno] Fecha de script: 20/09/2026 05:35:11 p. m. ******/
CREATE NONCLUSTERED INDEX [IX_Trabajador_IdTurno] ON [dbo].[Trabajador]
(
	[IdTurno] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Objeto: Index [UX_Turno_Nombre] Fecha de script: 20/09/2026 05:35:11 p. m. ******/
CREATE UNIQUE NONCLUSTERED INDEX [UX_Turno_Nombre] ON [dbo].[Turno]
(
	[Nombre] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[CertificacionAnulacion] ADD  DEFAULT (CONVERT([date],getdate())) FOR [FechaInicio]
GO
ALTER TABLE [dbo].[CertificacionAnulacion] ADD  DEFAULT ((0)) FOR [EsPermanente]
GO
ALTER TABLE [dbo].[CertificacionAnulacion] ADD  DEFAULT ((1)) FOR [Activa]
GO
ALTER TABLE [dbo].[CertificacionAnulacion] ADD  DEFAULT (getdate()) FOR [FechaRegistro]
GO
ALTER TABLE [dbo].[EquivalenciaLineaHdc] ADD  CONSTRAINT [DF_EquivalenciaLineaHdc_Accion]  DEFAULT ('MAPEAR') FOR [Accion]
GO
ALTER TABLE [dbo].[EquivalenciaLineaHdc] ADD  CONSTRAINT [DF_EquivalenciaLineaHdc_Activo]  DEFAULT ((1)) FOR [Activo]
GO
ALTER TABLE [dbo].[EquivalenciaPlantaHdc] ADD  CONSTRAINT [DF_EquivalenciaPlantaHdc_Activo]  DEFAULT ((1)) FOR [Activo]
GO
ALTER TABLE [dbo].[EquivalenciaPlantaHdc] ADD  CONSTRAINT [DF_EquivalenciaPlantaHdc_Accion]  DEFAULT ('MAPEAR') FOR [Accion]
GO
ALTER TABLE [dbo].[EquivalenciaTurnoHdc] ADD  CONSTRAINT [DF_EquivalenciaTurnoHdc_Activo]  DEFAULT ((1)) FOR [Activo]
GO
ALTER TABLE [dbo].[ExpedienteTrabajador] ADD  DEFAULT ((1)) FOR [Activo]
GO
ALTER TABLE [dbo].[ExpedienteTrabajador] ADD  DEFAULT (getdate()) FOR [FechaRegistro]
GO
ALTER TABLE [dbo].[ImportacionHdc] ADD  CONSTRAINT [DF_ImportacionHdc_NombreHoja]  DEFAULT ('HDC') FOR [NombreHoja]
GO
ALTER TABLE [dbo].[ImportacionHdc] ADD  CONSTRAINT [DF_ImportacionHdc_Fecha]  DEFAULT (sysdatetime()) FOR [FechaImportacion]
GO
ALTER TABLE [dbo].[ImportacionHdc] ADD  CONSTRAINT [DF_ImportacionHdc_Total]  DEFAULT ((0)) FOR [TotalRegistros]
GO
ALTER TABLE [dbo].[ImportacionHdc] ADD  CONSTRAINT [DF_ImportacionHdc_Nuevos]  DEFAULT ((0)) FOR [Nuevos]
GO
ALTER TABLE [dbo].[ImportacionHdc] ADD  CONSTRAINT [DF_ImportacionHdc_Actualizados]  DEFAULT ((0)) FOR [Actualizados]
GO
ALTER TABLE [dbo].[ImportacionHdc] ADD  CONSTRAINT [DF_ImportacionHdc_SinCambios]  DEFAULT ((0)) FOR [SinCambios]
GO
ALTER TABLE [dbo].[ImportacionHdc] ADD  CONSTRAINT [DF_ImportacionHdc_Advertencias]  DEFAULT ((0)) FOR [ConAdvertencias]
GO
ALTER TABLE [dbo].[PerfilHdcTrabajador] ADD  CONSTRAINT [DF_PerfilHdcTrabajador_Fecha]  DEFAULT (sysdatetime()) FOR [FechaUltimaImportacion]
GO
ALTER TABLE [dbo].[PerfilHdcTrabajador] ADD  CONSTRAINT [DF_PerfilHdcTrabajador_Encontrado]  DEFAULT ((1)) FOR [EncontradoUltimoHdc]
GO
ALTER TABLE [dbo].[SchemaVersion] ADD  CONSTRAINT [DF_SchemaVersion_FechaAplicacion]  DEFAULT (sysdatetime()) FOR [FechaAplicacion]
GO
ALTER TABLE [dbo].[Certificacion]  WITH CHECK ADD FOREIGN KEY([IdCertificador])
REFERENCES [dbo].[Certificador] ([Id])
GO
ALTER TABLE [dbo].[Certificacion]  WITH CHECK ADD FOREIGN KEY([IdProceso])
REFERENCES [dbo].[Proceso] ([Id])
GO
ALTER TABLE [dbo].[Certificacion]  WITH CHECK ADD FOREIGN KEY([IdTrabajador])
REFERENCES [dbo].[Trabajador] ([Id])
GO
ALTER TABLE [dbo].[CertificacionAnulacion]  WITH CHECK ADD  CONSTRAINT [FK_CertificacionAnulacion_Certificacion] FOREIGN KEY([IdCertificacion])
REFERENCES [dbo].[Certificacion] ([Id])
GO
ALTER TABLE [dbo].[CertificacionAnulacion] CHECK CONSTRAINT [FK_CertificacionAnulacion_Certificacion]
GO
ALTER TABLE [dbo].[Certificador]  WITH CHECK ADD FOREIGN KEY([IdTrabajador])
REFERENCES [dbo].[Trabajador] ([Id])
GO
ALTER TABLE [dbo].[EquivalenciaLineaHdc]  WITH CHECK ADD  CONSTRAINT [FK_EquivalenciaLineaHdc_Linea] FOREIGN KEY([IdLinea])
REFERENCES [dbo].[Linea] ([Id])
GO
ALTER TABLE [dbo].[EquivalenciaLineaHdc] CHECK CONSTRAINT [FK_EquivalenciaLineaHdc_Linea]
GO
ALTER TABLE [dbo].[EquivalenciaPlantaHdc]  WITH CHECK ADD  CONSTRAINT [FK_EquivalenciaPlantaHdc_Planta] FOREIGN KEY([IdPlanta])
REFERENCES [dbo].[Planta] ([Id])
GO
ALTER TABLE [dbo].[EquivalenciaPlantaHdc] CHECK CONSTRAINT [FK_EquivalenciaPlantaHdc_Planta]
GO
ALTER TABLE [dbo].[EquivalenciaTurnoHdc]  WITH CHECK ADD  CONSTRAINT [FK_EquivalenciaTurnoHdc_Turno] FOREIGN KEY([IdTurno])
REFERENCES [dbo].[Turno] ([Id])
GO
ALTER TABLE [dbo].[EquivalenciaTurnoHdc] CHECK CONSTRAINT [FK_EquivalenciaTurnoHdc_Turno]
GO
ALTER TABLE [dbo].[ExpedienteTrabajador]  WITH CHECK ADD  CONSTRAINT [FK_ExpedienteTrabajador_Trabajador] FOREIGN KEY([IdTrabajador])
REFERENCES [dbo].[Trabajador] ([Id])
GO
ALTER TABLE [dbo].[ExpedienteTrabajador] CHECK CONSTRAINT [FK_ExpedienteTrabajador_Trabajador]
GO
ALTER TABLE [dbo].[Linea]  WITH CHECK ADD FOREIGN KEY([IdPlanta])
REFERENCES [dbo].[Planta] ([Id])
GO
ALTER TABLE [dbo].[PerfilHdcTrabajador]  WITH CHECK ADD  CONSTRAINT [FK_PerfilHdcTrabajador_Importacion] FOREIGN KEY([IdImportacionHdc])
REFERENCES [dbo].[ImportacionHdc] ([Id])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[PerfilHdcTrabajador] CHECK CONSTRAINT [FK_PerfilHdcTrabajador_Importacion]
GO
ALTER TABLE [dbo].[PerfilHdcTrabajador]  WITH CHECK ADD  CONSTRAINT [FK_PerfilHdcTrabajador_Trabajador] FOREIGN KEY([IdTrabajador])
REFERENCES [dbo].[Trabajador] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PerfilHdcTrabajador] CHECK CONSTRAINT [FK_PerfilHdcTrabajador_Trabajador]
GO
ALTER TABLE [dbo].[Trabajador]  WITH CHECK ADD FOREIGN KEY([IdLinea])
REFERENCES [dbo].[Linea] ([Id])
GO
ALTER TABLE [dbo].[Trabajador]  WITH CHECK ADD FOREIGN KEY([IdLocalidad])
REFERENCES [dbo].[Localidad] ([Id])
GO
ALTER TABLE [dbo].[Trabajador]  WITH CHECK ADD FOREIGN KEY([IdTurno])
REFERENCES [dbo].[Turno] ([Id])
GO
ALTER TABLE [dbo].[Trabajador]  WITH CHECK ADD  CONSTRAINT [FK_Trabajador_Planta] FOREIGN KEY([IdPlanta])
REFERENCES [dbo].[Planta] ([Id])
GO
ALTER TABLE [dbo].[Trabajador] CHECK CONSTRAINT [FK_Trabajador_Planta]
GO
ALTER TABLE [dbo].[Certificacion]  WITH CHECK ADD  CONSTRAINT [CK_Fechas_Certificacion] CHECK  (([FechaVencimiento]>[FechaCertificacion]))
GO
ALTER TABLE [dbo].[Certificacion] CHECK CONSTRAINT [CK_Fechas_Certificacion]
GO
ALTER TABLE [dbo].[EquivalenciaLineaHdc]  WITH CHECK ADD  CONSTRAINT [CK_EquivalenciaLineaHdc_Accion] CHECK  (([Accion]='MAPEAR' AND [IdLinea] IS NOT NULL OR ([Accion]='IGNORAR' OR [Accion]='SIN_ASIGNAR') AND [IdLinea] IS NULL))
GO
ALTER TABLE [dbo].[EquivalenciaLineaHdc] CHECK CONSTRAINT [CK_EquivalenciaLineaHdc_Accion]
GO
ALTER TABLE [dbo].[EquivalenciaPlantaHdc]  WITH CHECK ADD  CONSTRAINT [CK_EquivalenciaPlantaHdc_Accion] CHECK  (([Accion]='MAPEAR' AND [IdPlanta] IS NOT NULL OR ([Accion]='PENDIENTE' OR [Accion]='IGNORAR') AND [IdPlanta] IS NULL))
GO
ALTER TABLE [dbo].[EquivalenciaPlantaHdc] CHECK CONSTRAINT [CK_EquivalenciaPlantaHdc_Accion]
GO
/****** Objeto: StoredProcedure [dbo].[spBorrarCertificacion] Fecha de script: 20/09/2026 05:35:11 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[spBorrarCertificacion]
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    IF NOT EXISTS
    (
        SELECT 1
        FROM Certificacion
        WHERE Id = @Id
    )
    BEGIN
        SELECT
            0 AS Id,
            'No existe la certificación que intenta borrar.' AS Nombre;

        RETURN;
    END;

    BEGIN TRY
        BEGIN TRANSACTION;

        DELETE FROM CertificacionAnulacion
        WHERE IdCertificacion = @Id;

        DELETE FROM Certificacion
        WHERE Id = @Id;

        COMMIT TRANSACTION;

        SELECT
            1 AS Id,
            'Certificación eliminada correctamente.' AS Nombre;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
        BEGIN
            ROLLBACK TRANSACTION;
        END;

        SELECT
            0 AS Id,
            'No se pudo borrar la certificación. Detalle: '
                + ERROR_MESSAGE() AS Nombre;
    END CATCH;
END;
GO
/****** Objeto: StoredProcedure [dbo].[spBorrarCertificador] Fecha de script: 20/09/2026 05:35:11 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[spBorrarCertificador]
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    IF NOT EXISTS
    (
        SELECT 1
        FROM Certificador
        WHERE Id = @Id
    )
    BEGIN
        SELECT
            0 AS Id,
            'El certificador seleccionado no existe.' AS Nombre;

        RETURN;
    END;

    IF EXISTS
    (
        SELECT 1
        FROM Certificacion
        WHERE IdCertificador = @Id
    )
    BEGIN
        SELECT
            2 AS Id,
            'No se puede borrar porque hay certificaciones asociadas a este certificador.'
                AS Nombre;

        RETURN;
    END;

    BEGIN TRY
        DELETE FROM Certificador
        WHERE Id = @Id;

        SELECT
            1 AS Id,
            'Se ha borrado correctamente.' AS Nombre;
    END TRY
    BEGIN CATCH
        SELECT
            0 AS Id,
            'No se pudo borrar el certificador. Detalle: '
                + ERROR_MESSAGE() AS Nombre;
    END CATCH;
END;
GO
/****** Objeto: StoredProcedure [dbo].[spBorrarLinea] Fecha de script: 20/09/2026 05:35:11 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[spBorrarLinea]
@Id INT
AS
BEGIN
	SET NOCOUNT ON;
	IF NOT EXISTS (SELECT 1 FROM Trabajador WHERE IdLinea=@Id)
	BEGIN
		DELETE FROM Linea WHERE Id=@Id
		SELECT '1' AS [Id], 'Se ha borrado correctamente' AS [Nombre];	
	END ELSE
		BEGIN
			SELECT '2' AS [Id], 'No se puede borrar por que hay registros asociados a esta linea' AS [Nombre];
		END
END
GO
/****** Objeto: StoredProcedure [dbo].[spBorrarLocalidad] Fecha de script: 20/09/2026 05:35:11 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[spBorrarLocalidad]
@Id INT
AS
BEGIN
	SET NOCOUNT ON;
	IF NOT EXISTS (SELECT 1 FROM Trabajador WHERE IdLocalidad=@Id)
	BEGIN
		DELETE FROM Localidad WHERE Id=@Id
		SELECT '1' AS [Id], 'Se ha borrado correctamente' AS [Nombre];	
	END ELSE
		BEGIN
			SELECT '2' AS [Id], 'No se puede borrar por que hay registros asociados a esta localidad' AS [Nombre];
		END
END
GO
/****** Objeto: StoredProcedure [dbo].[spBorrarPlanta] Fecha de script: 20/09/2026 05:35:11 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[spBorrarPlanta]
@Id INT
AS
BEGIN
	SET NOCOUNT ON;
	IF NOT EXISTS (SELECT 1 FROM Linea WHERE IdPlanta=@Id)
	BEGIN
		DELETE FROM Planta WHERE Id=@Id
		SELECT '1' AS [Id], 'Se ha borrado correctamente' AS [Nombre];	
	END ELSE
		BEGIN
			SELECT '2' AS [Id], 'No se puede borrar por que hay registros asociados a esta planta' AS [Nombre];
		END
END
GO
/****** Objeto: StoredProcedure [dbo].[spBorrarProceso] Fecha de script: 20/09/2026 05:35:11 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[spBorrarProceso]
@Id INT
AS
BEGIN
	SET NOCOUNT ON;
	IF NOT EXISTS (SELECT 1 FROM Certificacion WHERE IdProceso=@Id)
	BEGIN
		DELETE FROM Proceso WHERE Id=@Id
		SELECT '1' AS [Id], 'Se ha borrado correctamente' AS [Nombre];	
	END ELSE
		BEGIN
			SELECT '2' AS [Id], 'No se puede borrar por que hay registros asociados a este proceso' AS [Nombre];
		END
END
GO
/****** Objeto: StoredProcedure [dbo].[spBorrarTrabajador] Fecha de script: 20/09/2026 05:35:11 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[spBorrarTrabajador]
@Id INT
AS
BEGIN
	SET NOCOUNT ON;
	IF NOT EXISTS (SELECT 1 FROM Certificador WHERE IdTrabajador=@Id) AND NOT EXISTS (SELECT 1 FROM Certificacion WHERE IdTrabajador=@Id) AND NOT EXISTS (SELECT 1 FROM TrabajadorEstadoHistorial WHERE IdTrabajador=@Id)
	BEGIN
		DELETE FROM Trabajador WHERE Id=@Id
		SELECT '1' AS [Id], 'Se ha borrado correctamente' AS [Nombre];	
	END ELSE
		BEGIN
			SELECT '2' AS [Id], 'No se puede borrar por que hay registros asociados a este trabajador' AS [Nombre];
		END
END
GO
/****** Objeto: StoredProcedure [dbo].[spBorrarTurno] Fecha de script: 20/09/2026 05:35:11 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[spBorrarTurno]
@Id INT
AS
BEGIN
	SET NOCOUNT ON;
	IF NOT EXISTS (SELECT 1 FROM Trabajador WHERE IdTurno=@Id)
	BEGIN
		DELETE FROM Turno WHERE Id=@Id
		SELECT '1' AS [Id], 'Se ha borrado correctamente' AS [Nombre];	
	END ELSE
		BEGIN
			SELECT '2' AS [Id], 'No se puede borrar por que hay registros asociados a este turno' AS [Nombre];
		END
END
GO
/****** Objeto: StoredProcedure [dbo].[spConsultarAnulacionPorCertificacion] Fecha de script: 20/09/2026 05:35:11 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[spConsultarAnulacionPorCertificacion]
    @IdCertificacion INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP (1)
        Id,
        IdCertificacion,
        TipoAnulacion,
        FechaInicio,
        FechaFin,
        EsPermanente,
        Comentario,
        Activa,
        FechaRegistro,
        FechaModificacion
    FROM CertificacionAnulacion
    WHERE IdCertificacion = @IdCertificacion
      AND Activa = 1
      AND
      (
          EsPermanente = 1
          OR FechaFin IS NULL
          OR FechaFin >= CONVERT(DATE, GETDATE())
      )
    ORDER BY
        FechaRegistro DESC,
        Id DESC;
END;
GO
/****** Objeto: StoredProcedure [dbo].[spConsultarCertificacionesPorTrabajador] Fecha de script: 20/09/2026 05:35:11 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[spConsultarCertificacionesPorTrabajador]
    @IdTrabajador INT,
    @TextoBuscar VARCHAR(100) = ''
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        C.Id,
        C.IdTrabajador,
        P.Id AS IdProceso,
        P.Nombre AS Proceso,
        C.FechaCertificacion,
        C.FechaVencimiento,
        DATEDIFF(DAY, CAST(GETDATE() AS DATE), C.FechaVencimiento) AS DiasRestantes,
        C.Comentario,
        C.IdCertificador,
        TCert.Nombre AS NombreCertificador,

        CASE 
            WHEN CA.Id IS NOT NULL THEN 1
            ELSE 0
        END AS EstaAnulada,

        CA.Id AS IdAnulacion,
        CA.TipoAnulacion,
        CA.FechaInicio AS FechaInicioAnulacion,
        CA.FechaFin AS FechaFinAnulacion,
        CA.EsPermanente,
        CA.Comentario AS ComentarioAnulacion

    FROM Certificacion C
    INNER JOIN Proceso P 
        ON P.Id = C.IdProceso
    INNER JOIN Certificador Cert 
        ON Cert.Id = C.IdCertificador
    INNER JOIN Trabajador TCert 
        ON TCert.Id = Cert.IdTrabajador
    OUTER APPLY
    (
        SELECT TOP 1
            A.Id,
            A.TipoAnulacion,
            A.FechaInicio,
            A.FechaFin,
            A.EsPermanente,
            A.Comentario
        FROM CertificacionAnulacion A
        WHERE A.IdCertificacion = C.Id
          AND A.Activa = 1
          AND (
                A.EsPermanente = 1
                OR A.FechaFin IS NULL
                OR A.FechaFin >= CAST(GETDATE() AS DATE)
              )
        ORDER BY A.FechaRegistro DESC
    ) CA
    WHERE C.IdTrabajador = @IdTrabajador
      AND (
            @TextoBuscar = ''
            OR P.Nombre LIKE '%' + @TextoBuscar + '%'
            OR C.Comentario LIKE '%' + @TextoBuscar + '%'
            OR TCert.Nombre LIKE '%' + @TextoBuscar + '%'
            OR CA.Comentario LIKE '%' + @TextoBuscar + '%'
          )
    ORDER BY C.FechaVencimiento;
END
GO
/****** Objeto: StoredProcedure [dbo].[spConsultarCertificadores] Fecha de script: 20/09/2026 05:35:11 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/* ============================================================
   8. CERTIFICADORES
   ============================================================ */

CREATE   PROCEDURE [dbo].[spConsultarCertificadores]
    @TextoBuscar VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        C.Id,
        C.IdTrabajador,

        T.NoReloj,
        T.Nombre AS NombreTrabajador,
        T.RutaFoto,

        T.IdTurno,
        Tu.Nombre AS NombreTurno,

        T.IdPlanta,
        P.Nombre AS NombrePlanta,

        T.IdLinea,
        COALESCE(Li.Nombre, 'SIN ASIGNAR') AS NombreLinea

    FROM dbo.Certificador AS C

    INNER JOIN dbo.Trabajador AS T
        ON T.Id = C.IdTrabajador

    INNER JOIN dbo.Turno AS Tu
        ON Tu.Id = T.IdTurno

    INNER JOIN dbo.Planta AS P
        ON P.Id = T.IdPlanta

    LEFT JOIN dbo.Linea AS Li
        ON Li.Id = T.IdLinea

    WHERE
        T.NoReloj LIKE '%' + ISNULL(@TextoBuscar, '') + '%'
        OR T.Nombre LIKE '%' + ISNULL(@TextoBuscar, '') + '%'
        OR Tu.Nombre LIKE '%' + ISNULL(@TextoBuscar, '') + '%'
        OR P.Nombre LIKE '%' + ISNULL(@TextoBuscar, '') + '%'
        OR COALESCE(Li.Nombre, 'SIN ASIGNAR')
            LIKE '%' + ISNULL(@TextoBuscar, '') + '%'

    ORDER BY T.Nombre;
END;

GO
/****** Objeto: StoredProcedure [dbo].[spConsultarExpedienteTrabajador] Fecha de script: 20/09/2026 05:35:11 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[spConsultarExpedienteTrabajador]
    @IdTrabajador INT
AS
BEGIN
    SET NOCOUNT ON;

    IF @IdTrabajador <= 0
    BEGIN
        RETURN;
    END;

    SELECT
        Id,
        IdTrabajador,
        NombreOriginal,
        NombreArchivo,
        Extension,
        RutaArchivo,
        TipoArchivo,
        Comentario,
        FechaRegistro,
        FechaModificacion
    FROM ExpedienteTrabajador
    WHERE IdTrabajador = @IdTrabajador
      AND Activo = 1
    ORDER BY FechaRegistro DESC;
END;
GO
/****** Objeto: StoredProcedure [dbo].[spConsultarLineas] Fecha de script: 20/09/2026 05:35:11 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[spConsultarLineas]
@TextoBuscar VARCHAR(100)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT 
        Linea.Id,
        Linea.Nombre,
        Linea.IdPlanta,
        Planta.Nombre AS [NombrePlanta]
    FROM Linea
    INNER JOIN Planta ON Linea.IdPlanta = Planta.Id
	WHERE Linea.Nombre LIKE '%' + @TextoBuscar + '%'
        OR Planta.Nombre LIKE '%' + @TextoBuscar + '%';
END
GO
/****** Objeto: StoredProcedure [dbo].[spConsultarLineasPorPlanta] Fecha de script: 20/09/2026 05:35:11 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[spConsultarLineasPorPlanta]
@IdPlanta INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT Id, Nombre FROM Linea WHERE IdPlanta=@IdPlanta
END
GO
/****** Objeto: StoredProcedure [dbo].[spConsultarLocalidades] Fecha de script: 20/09/2026 05:35:11 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[spConsultarLocalidades]
@TextoBuscar VARCHAR(100)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT * FROM Localidad WHERE Nombre LIKE '%'+@TextoBuscar+'%';
END
GO
/****** Objeto: StoredProcedure [dbo].[spConsultarPlantas] Fecha de script: 20/09/2026 05:35:11 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[spConsultarPlantas]
@TextoBuscar VARCHAR(100)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT * FROM Planta WHERE Nombre LIKE '%'+@TextoBuscar+'%';
END
GO
/****** Objeto: StoredProcedure [dbo].[spConsultarProcesos] Fecha de script: 20/09/2026 05:35:11 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[spConsultarProcesos]
@TextoBuscar VARCHAR(100)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT * FROM Proceso WHERE Nombre LIKE '%'+@TextoBuscar+'%'
END
GO
/****** Objeto: StoredProcedure [dbo].[spConsultarTrabajador] Fecha de script: 20/09/2026 05:35:11 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/* ============================================================
   4. CONSULTAR UN TRABAJADOR
   Mantiene columnas de Localidad temporalmente por compatibilidad.
   ============================================================ */

CREATE   PROCEDURE [dbo].[spConsultarTrabajador]
    @NoReloj VARCHAR(10)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP (1)
        T.Id,
        T.NoReloj,
        T.Nombre,
        T.Activo,
        T.RutaFoto,

        T.IdLocalidad,
        Lo.Nombre AS NombreLocalidad,

        T.IdTurno,
        Tu.Nombre AS NombreTurno,

        T.IdPlanta,
        P.Nombre AS NombrePlanta,

        T.IdLinea,
        COALESCE(Li.Nombre, 'SIN ASIGNAR') AS NombreLinea

    FROM dbo.Trabajador AS T

    LEFT JOIN dbo.Localidad AS Lo
        ON Lo.Id = T.IdLocalidad

    INNER JOIN dbo.Turno AS Tu
        ON Tu.Id = T.IdTurno

    INNER JOIN dbo.Planta AS P
        ON P.Id = T.IdPlanta

    LEFT JOIN dbo.Linea AS Li
        ON Li.Id = T.IdLinea

    WHERE T.NoReloj = @NoReloj;
END;

GO
/****** Objeto: StoredProcedure [dbo].[spConsultarTrabajadores] Fecha de script: 20/09/2026 05:35:11 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/* ============================================================
   5. LISTA DE TRABAJADORES
   ============================================================ */

CREATE   PROCEDURE [dbo].[spConsultarTrabajadores]
    @TextoBuscar VARCHAR(100) = '',
    @Activo BIT = NULL,
    @PageNumber INT = 1 OUTPUT,
    @PageSize INT = 100,
    @TotalCount INT = 0 OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    IF @PageSize IS NULL OR @PageSize <= 0 OR @PageSize > 100
        SET @PageSize = 100;
    IF @PageNumber IS NULL OR @PageNumber < 1
        SET @PageNumber = 1;

    SELECT
        T.Id,
        T.Nombre
    INTO #TrabajadoresFiltrados
    FROM dbo.Trabajador AS T
    LEFT JOIN dbo.Localidad AS Lo ON Lo.Id = T.IdLocalidad
    INNER JOIN dbo.Turno AS Tu ON Tu.Id = T.IdTurno
    INNER JOIN dbo.Planta AS P ON P.Id = T.IdPlanta
    LEFT JOIN dbo.Linea AS Li ON Li.Id = T.IdLinea
    WHERE (@Activo IS NULL OR T.Activo = @Activo)
      AND (
        T.NoReloj LIKE '%' + ISNULL(@TextoBuscar, '') + '%'
        OR T.Nombre LIKE '%' + ISNULL(@TextoBuscar, '') + '%'
        OR ISNULL(Lo.Nombre, '') LIKE '%' + ISNULL(@TextoBuscar, '') + '%'
        OR Tu.Nombre LIKE '%' + ISNULL(@TextoBuscar, '') + '%'
        OR P.Nombre LIKE '%' + ISNULL(@TextoBuscar, '') + '%'
        OR COALESCE(Li.Nombre, 'SIN ASIGNAR')
            LIKE '%' + ISNULL(@TextoBuscar, '') + '%')

    SELECT @TotalCount = COUNT(*) FROM #TrabajadoresFiltrados;

    DECLARE @TotalPages INT = CASE WHEN @TotalCount = 0 THEN 0
        ELSE (@TotalCount - 1) / @PageSize + 1 END;
    IF @PageNumber > @TotalPages
        SET @PageNumber = CASE WHEN @TotalPages = 0 THEN 1
            ELSE @TotalPages END;

    DECLARE @Offset BIGINT = (CONVERT(BIGINT, @PageNumber) - 1) * @PageSize;
    ;WITH Pagina AS
    (
        SELECT Id, Nombre FROM #TrabajadoresFiltrados
        ORDER BY Nombre, Id
        OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY
    )
    SELECT
        T.Id,
        T.NoReloj,
        T.Nombre,
        T.Activo,
        T.RutaFoto,
        T.IdLocalidad,
        Lo.Nombre AS NombreLocalidad,
        T.IdTurno,
        Tu.Nombre AS NombreTurno,
        T.IdPlanta,
        P.Nombre AS NombrePlanta,
        T.IdLinea,
        COALESCE(Li.Nombre, 'SIN ASIGNAR') AS NombreLinea
    FROM Pagina AS F
    INNER JOIN dbo.Trabajador AS T ON T.Id = F.Id
    LEFT JOIN dbo.Localidad AS Lo ON Lo.Id = T.IdLocalidad
    INNER JOIN dbo.Turno AS Tu ON Tu.Id = T.IdTurno
    INNER JOIN dbo.Planta AS P ON P.Id = T.IdPlanta
    LEFT JOIN dbo.Linea AS Li ON Li.Id = T.IdLinea
    ORDER BY F.Nombre, F.Id;
END;

GO
/****** Objeto: StoredProcedure [dbo].[spConsultarTrabajadoresCertificaciones] Fecha de script: 20/09/2026 05:35:11 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[spConsultarTrabajadoresCertificaciones]
    @MostrarPor VARCHAR(10),
    @TextoBuscar VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    IF UPPER(ISNULL(@MostrarPor, '')) = 'TODAS'
    BEGIN
        SELECT
            T.Id,
            T.NoReloj,
            T.Nombre,
            T.RutaFoto,
            T.IdLocalidad,
            L.Nombre AS NombreLocalidad,
            T.IdTurno,
            Tu.Nombre AS NombreTurno,
            Li.IdPlanta,
            P.Nombre AS NombrePlanta,
            T.IdLinea,
            Li.Nombre AS NombreLinea
        FROM dbo.Trabajador AS T
        LEFT JOIN dbo.Localidad AS L
            ON L.Id = T.IdLocalidad
        INNER JOIN dbo.Turno AS Tu
            ON Tu.Id = T.IdTurno
        INNER JOIN dbo.Linea AS Li
            ON Li.Id = T.IdLinea
        INNER JOIN dbo.Planta AS P
            ON P.Id = Li.IdPlanta
        WHERE
            ISNULL(@TextoBuscar, '') = ''
            OR T.NoReloj LIKE '%' + @TextoBuscar + '%'
            OR T.Nombre LIKE '%' + @TextoBuscar + '%'
            OR L.Nombre LIKE '%' + @TextoBuscar + '%'
            OR Tu.Nombre LIKE '%' + @TextoBuscar + '%'
            OR P.Nombre LIKE '%' + @TextoBuscar + '%'
            OR Li.Nombre LIKE '%' + @TextoBuscar + '%';
    END;
END;

GO
/****** Objeto: StoredProcedure [dbo].[spConsultarTrabajadoresEstadoCertificacion] Fecha de script: 20/09/2026 05:35:11 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/* ============================================================
   7. ESTADO DE CERTIFICACIONES
   Planta directa + Línea opcional.
   Conserva la lógica de exclusión de anulaciones activas.
   ============================================================ */

CREATE OR ALTER PROCEDURE [dbo].[spConsultarTrabajadoresEstadoCertificacion]
    @MostrarPor VARCHAR(20),
    @TextoBuscar VARCHAR(100),
    @Activo BIT = NULL,
    @AsOfDate DATE = NULL,
    @PageNumber INT = 1 OUTPUT,
    @PageSize INT = 100,
    @TotalCount INT = 0 OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    IF @AsOfDate IS NULL SET @AsOfDate = CAST(GETDATE() AS DATE);
    IF @PageSize IS NULL OR @PageSize <= 0 OR @PageSize > 100
        SET @PageSize = 100;
    IF @PageNumber IS NULL OR @PageNumber < 1
        SET @PageNumber = 1;

    SELECT
        T.Id, T.NoReloj, T.Nombre, T.Activo, T.RutaFoto,
        T.IdLocalidad, Lo.Nombre AS NombreLocalidad,
        T.IdTurno, Tu.Nombre AS NombreTurno,
        T.IdPlanta, P.Nombre AS NombrePlanta,
        T.IdLinea, COALESCE(Li.Nombre, 'SIN ASIGNAR') AS NombreLinea,
        CASE
            WHEN COUNT(C.Id) = 0 THEN 'Sin certificar'
            WHEN SUM(CASE WHEN C.FechaVencimiento < @AsOfDate
                          THEN 1 ELSE 0 END) > 0 THEN 'Vencida'
            WHEN SUM(CASE WHEN C.FechaVencimiento
                               BETWEEN @AsOfDate AND DATEADD(DAY, 30, @AsOfDate)
                          THEN 1 ELSE 0 END) > 0 THEN 'Por vencer'
            ELSE 'Vigente'
        END AS EstadoCertificacion
    INTO #TrabajadoresEstado
    FROM dbo.Trabajador AS T
    LEFT JOIN dbo.Localidad AS Lo ON Lo.Id = T.IdLocalidad
    INNER JOIN dbo.Turno AS Tu ON Tu.Id = T.IdTurno
    INNER JOIN dbo.Planta AS P ON P.Id = T.IdPlanta
    LEFT JOIN dbo.Linea AS Li ON Li.Id = T.IdLinea
    LEFT JOIN dbo.Certificacion AS C ON C.IdTrabajador = T.Id
        AND NOT EXISTS
        (
            SELECT 1 FROM dbo.CertificacionAnulacion AS CA
            WHERE CA.IdCertificacion = C.Id AND CA.Activa = 1
              AND (CA.EsPermanente = 1 OR CA.FechaFin IS NULL
                   OR CAST(CA.FechaFin AS DATE) >= @AsOfDate)
        )
    WHERE (@Activo IS NULL OR T.Activo = @Activo)
      AND (T.NoReloj LIKE '%' + ISNULL(@TextoBuscar, '') + '%'
           OR T.Nombre LIKE '%' + ISNULL(@TextoBuscar, '') + '%'
           OR ISNULL(Lo.Nombre, '') LIKE '%' + ISNULL(@TextoBuscar, '') + '%'
           OR Tu.Nombre LIKE '%' + ISNULL(@TextoBuscar, '') + '%'
           OR P.Nombre LIKE '%' + ISNULL(@TextoBuscar, '') + '%'
           OR COALESCE(Li.Nombre, 'SIN ASIGNAR')
               LIKE '%' + ISNULL(@TextoBuscar, '') + '%'
           OR ISNULL(@TextoBuscar, '') = '')
    GROUP BY T.Id, T.NoReloj, T.Nombre, T.Activo, T.RutaFoto,
             T.IdLocalidad, Lo.Nombre, T.IdTurno, Tu.Nombre,
             T.IdPlanta, P.Nombre, T.IdLinea, Li.Nombre;

    SELECT COUNT(*) AS Total,
           ISNULL(SUM(CASE WHEN EstadoCertificacion = 'Vigente'
                           THEN 1 ELSE 0 END), 0) AS Vigentes,
           ISNULL(SUM(CASE WHEN EstadoCertificacion = 'Por vencer'
                           THEN 1 ELSE 0 END), 0) AS PorVencer,
           ISNULL(SUM(CASE WHEN EstadoCertificacion = 'Vencida'
                           THEN 1 ELSE 0 END), 0) AS Vencidas,
           ISNULL(SUM(CASE WHEN EstadoCertificacion = 'Sin certificar'
                           THEN 1 ELSE 0 END), 0) AS SinCertificar
    FROM #TrabajadoresEstado;

    SELECT @TotalCount = COUNT(*) FROM #TrabajadoresEstado
    WHERE @MostrarPor = 'Todas' OR EstadoCertificacion = @MostrarPor;
    DECLARE @TotalPages INT = CASE WHEN @TotalCount = 0 THEN 0
        ELSE (@TotalCount - 1) / @PageSize + 1 END;
    IF @PageNumber > @TotalPages
        SET @PageNumber = CASE WHEN @TotalPages = 0 THEN 1
            ELSE @TotalPages END;
    DECLARE @Offset BIGINT = (CONVERT(BIGINT, @PageNumber) - 1) * @PageSize;

    SELECT Id, NoReloj, Nombre, Activo, RutaFoto,
           IdLocalidad, NombreLocalidad, IdTurno, NombreTurno,
           IdPlanta, NombrePlanta, IdLinea, NombreLinea,
           EstadoCertificacion
    FROM #TrabajadoresEstado
    WHERE @MostrarPor = 'Todas' OR EstadoCertificacion = @MostrarPor
    ORDER BY Nombre, Id
    OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
END;

GO
/****** Objeto: StoredProcedure [dbo].[spConsultarTurnos] Fecha de script: 20/09/2026 05:35:11 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[spConsultarTurnos]
@TextoBuscar VARCHAR(100)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT * FROM Turno WHERE Nombre LIKE '%'+@TextoBuscar+'%';
END
GO
/****** Objeto: StoredProcedure [dbo].[spConsultarVerificacionNoReloj] Fecha de script: 20/09/2026 05:35:11 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[spConsultarVerificacionNoReloj]
    @NoReloj VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        T.Id AS IdTrabajador,
        T.NoReloj,
        T.Nombre,
        T.RutaFoto,

        L.Nombre AS NombreLocalidad,
        Tu.Nombre AS NombreTurno,
        Pta.Nombre AS NombrePlanta,
        COALESCE(
            Li.Nombre,
            'SIN ASIGNAR'
        ) AS NombreLinea,

        C.Id AS IdCertificacion,
        Pr.Id AS IdProceso,
        Pr.Nombre AS Proceso,
        C.FechaCertificacion,
        C.FechaVencimiento,

        CASE
            WHEN C.FechaVencimiento IS NULL
                THEN NULL
            ELSE DATEDIFF(
                DAY,
                CAST(GETDATE() AS DATE),
                CAST(C.FechaVencimiento AS DATE)
            )
        END AS DiasRestantes,

        C.Comentario,
        C.IdCertificador,
        TCert.Nombre AS NombreCertificador

    FROM dbo.Trabajador AS T

    /*
     * Localidad se conserva temporalmente por compatibilidad,
     * pero ya no es obligatoria.
     */
    LEFT JOIN dbo.Localidad AS L
        ON L.Id = T.IdLocalidad

    INNER JOIN dbo.Turno AS Tu
        ON Tu.Id = T.IdTurno

    /*
     * Planta directa del trabajador.
     */
    INNER JOIN dbo.Planta AS Pta
        ON Pta.Id = T.IdPlanta

    /*
     * Línea opcional.
     */
    LEFT JOIN dbo.Linea AS Li
        ON Li.Id = T.IdLinea

    /*
     * LEFT JOIN es intencional:
     * aunque el trabajador no tenga certificaciones,
     * debe regresar una fila con los datos del trabajador
     * para que Verificaciones muestre "SIN CERTIFICACIONES".
     */
    LEFT JOIN dbo.Certificacion AS C
        ON C.IdTrabajador = T.Id
       AND NOT EXISTS
       (
            SELECT 1
            FROM dbo.CertificacionAnulacion AS CA
            WHERE CA.IdCertificacion = C.Id
              AND CA.Activa = 1
              AND
              (
                  CA.EsPermanente = 1
                  OR CA.FechaFin IS NULL
                  OR CAST(CA.FechaFin AS DATE)
                     >= CAST(GETDATE() AS DATE)
              )
       )

    LEFT JOIN dbo.Proceso AS Pr
        ON Pr.Id = C.IdProceso

    LEFT JOIN dbo.Certificador AS Cert
        ON Cert.Id = C.IdCertificador

    LEFT JOIN dbo.Trabajador AS TCert
        ON TCert.Id = Cert.IdTrabajador

    WHERE T.NoReloj = @NoReloj

    ORDER BY Pr.Nombre;
END;

GO
/****** Objeto: StoredProcedure [dbo].[spEliminarCertificacionAnulacion] Fecha de script: 20/09/2026 05:35:11 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[spEliminarCertificacionAnulacion]
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    IF NOT EXISTS
    (
        SELECT 1
        FROM CertificacionAnulacion
        WHERE Id = @Id
          AND Activa = 1
    )
    BEGIN
        SELECT
            0 AS Id,
            'No se encontró una anulación activa para eliminar.'
                AS Nombre;

        RETURN;
    END;

    BEGIN TRY
        UPDATE CertificacionAnulacion
        SET
            Activa = 0,
            FechaModificacion = GETDATE()
        WHERE Id = @Id
          AND Activa = 1;

        SELECT
            1 AS Id,
            'Anulación eliminada correctamente.'
                AS Nombre;
    END TRY
    BEGIN CATCH
        SELECT
            0 AS Id,
            'No se pudo eliminar la anulación. Detalle: '
                + ERROR_MESSAGE()
                AS Nombre;
    END CATCH;
END;
GO
/****** Objeto: StoredProcedure [dbo].[spEliminarExpedienteTrabajador] Fecha de script: 20/09/2026 05:35:11 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[spEliminarExpedienteTrabajador]
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    IF NOT EXISTS
    (
        SELECT 1
        FROM ExpedienteTrabajador
        WHERE Id = @Id
          AND Activo = 1
    )
    BEGIN
        SELECT
            0 AS Id,
            'No se encontró el archivo activo que intenta eliminar.'
                AS Nombre;

        RETURN;
    END;

    BEGIN TRY
        /*
         * Eliminación lógica para conservar el historial
         * del registro en la base de datos.
         */
        UPDATE ExpedienteTrabajador
        SET
            Activo = 0,
            FechaModificacion = GETDATE()
        WHERE Id = @Id
          AND Activo = 1;

        SELECT
            1 AS Id,
            'Archivo eliminado correctamente del expediente.' AS Nombre;
    END TRY
    BEGIN CATCH
        SELECT
            0 AS Id,
            'No se pudo eliminar el archivo. Detalle: '
                + ERROR_MESSAGE() AS Nombre;
    END CATCH;
END;
GO
/****** Objeto: StoredProcedure [dbo].[spGuardarCertificacion] Fecha de script: 20/09/2026 05:35:11 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[spGuardarCertificacion]
    @IdTrabajador INT,
    @IdProceso INT,
    @FechaCertificacion DATE,
    @IdCertificador INT,
    @Comentario VARCHAR(300) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @VigenciaMeses INT;
    DECLARE @FechaVencimiento DATE;
    DECLARE @IdCertificacionExistente INT;

    SET @Comentario =
        NULLIF(LTRIM(RTRIM(@Comentario)), '');

    IF NOT EXISTS
    (
        SELECT 1
        FROM Trabajador
        WHERE Id = @IdTrabajador
    )
    BEGIN
        SELECT
            0 AS Id,
            'El trabajador seleccionado no existe.' AS Nombre;

        RETURN;
    END;

    SELECT
        @VigenciaMeses = VigenciaMeses
    FROM Proceso
    WHERE Id = @IdProceso;

    IF @VigenciaMeses IS NULL
    BEGIN
        SELECT
            0 AS Id,
            'El proceso seleccionado no existe.' AS Nombre;

        RETURN;
    END;

    IF @VigenciaMeses <= 0
    BEGIN
        SELECT
            0 AS Id,
            'La vigencia del proceso debe ser mayor que cero.' AS Nombre;

        RETURN;
    END;

    IF NOT EXISTS
    (
        SELECT 1
        FROM Certificador
        WHERE Id = @IdCertificador
    )
    BEGIN
        SELECT
            0 AS Id,
            'El certificador seleccionado no existe.' AS Nombre;

        RETURN;
    END;

    IF @FechaCertificacion IS NULL
    BEGIN
        SELECT
            0 AS Id,
            'La fecha de certificación es obligatoria.' AS Nombre;

        RETURN;
    END;

    SELECT
        @IdCertificacionExistente = Id
    FROM Certificacion
    WHERE IdTrabajador = @IdTrabajador
      AND IdProceso = @IdProceso;

    IF @IdCertificacionExistente IS NOT NULL
       AND EXISTS
       (
            SELECT 1
            FROM CertificacionAnulacion
            WHERE IdCertificacion = @IdCertificacionExistente
              AND Activa = 1
              AND
              (
                  EsPermanente = 1
                  OR FechaFin IS NULL
                  OR FechaFin >= CAST(GETDATE() AS DATE)
              )
       )
    BEGIN
        SELECT
            0 AS Id,
            'Esta certificación se encuentra anulada. No se puede modificar ni renovar mientras tenga una anulación activa.'
                AS Nombre;

        RETURN;
    END;

    SET @FechaVencimiento =
        DATEADD(
            MONTH,
            @VigenciaMeses,
            @FechaCertificacion
        );

    BEGIN TRY
        BEGIN TRANSACTION;

        IF @IdCertificacionExistente IS NOT NULL
        BEGIN
            UPDATE Certificacion
            SET
                FechaCertificacion = @FechaCertificacion,
                FechaVencimiento = @FechaVencimiento,
                IdCertificador = @IdCertificador,
                Comentario = @Comentario
            WHERE Id = @IdCertificacionExistente;

            COMMIT TRANSACTION;

            SELECT
                2 AS Id,
                'Certificación renovada correctamente.' AS Nombre;

            RETURN;
        END;

        INSERT INTO Certificacion
        (
            IdTrabajador,
            IdProceso,
            FechaCertificacion,
            FechaVencimiento,
            IdCertificador,
            Comentario
        )
        VALUES
        (
            @IdTrabajador,
            @IdProceso,
            @FechaCertificacion,
            @FechaVencimiento,
            @IdCertificador,
            @Comentario
        );

        COMMIT TRANSACTION;

        SELECT
            1 AS Id,
            'Certificación registrada correctamente.' AS Nombre;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
        BEGIN
            ROLLBACK TRANSACTION;
        END;

        SELECT
            0 AS Id,
            'No se pudo guardar la certificación. Detalle: '
                + ERROR_MESSAGE() AS Nombre;
    END CATCH;
END;
GO
/****** Objeto: StoredProcedure [dbo].[spGuardarCertificacionAnulacion] Fecha de script: 20/09/2026 05:35:11 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[spGuardarCertificacionAnulacion]
    @IdCertificacion INT,
    @TipoAnulacion VARCHAR(50),
    @FechaInicio DATE,
    @FechaFin DATE = NULL,
    @EsPermanente BIT,
    @Comentario VARCHAR(500)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    SET @TipoAnulacion =
        LTRIM(RTRIM(ISNULL(@TipoAnulacion, '')));

    SET @Comentario =
        LTRIM(RTRIM(ISNULL(@Comentario, '')));

    IF NOT EXISTS
    (
        SELECT 1
        FROM Certificacion
        WHERE Id = @IdCertificacion
    )
    BEGIN
        SELECT
            0 AS Id,
            'No se encontró la certificación seleccionada.'
                AS Nombre;

        RETURN;
    END;

    IF @TipoAnulacion = ''
    BEGIN
        SELECT
            0 AS Id,
            'Debe seleccionar un tipo de anulación.'
                AS Nombre;

        RETURN;
    END;

    IF @FechaInicio IS NULL
    BEGIN
        SELECT
            0 AS Id,
            'La fecha de inicio es obligatoria.'
                AS Nombre;

        RETURN;
    END;

    IF @EsPermanente IS NULL
    BEGIN
        SELECT
            0 AS Id,
            'Debe indicar si la anulación es permanente.'
                AS Nombre;

        RETURN;
    END;

    IF @EsPermanente = 0
       AND @FechaFin IS NULL
    BEGIN
        SELECT
            0 AS Id,
            'Debe seleccionar una fecha fin para una anulación temporal.'
                AS Nombre;

        RETURN;
    END;

    IF @EsPermanente = 0
       AND @FechaFin < @FechaInicio
    BEGIN
        SELECT
            0 AS Id,
            'La fecha fin no puede ser menor que la fecha inicio.'
                AS Nombre;

        RETURN;
    END;

    IF @Comentario = ''
    BEGIN
        SELECT
            0 AS Id,
            'Debe escribir un comentario de la anulación.'
                AS Nombre;

        RETURN;
    END;

    BEGIN TRY
        BEGIN TRANSACTION;

        /*
         * Conserva el historial y evita que queden
         * dos anulaciones activas para la certificación.
         */
        UPDATE CertificacionAnulacion
        SET
            Activa = 0,
            FechaModificacion = GETDATE()
        WHERE IdCertificacion = @IdCertificacion
          AND Activa = 1;

        INSERT INTO CertificacionAnulacion
        (
            IdCertificacion,
            TipoAnulacion,
            FechaInicio,
            FechaFin,
            EsPermanente,
            Comentario,
            Activa,
            FechaRegistro,
            FechaModificacion
        )
        VALUES
        (
            @IdCertificacion,
            @TipoAnulacion,
            @FechaInicio,
            CASE
                WHEN @EsPermanente = 1
                    THEN NULL
                ELSE @FechaFin
            END,
            @EsPermanente,
            @Comentario,
            1,
            GETDATE(),
            NULL
        );

        COMMIT TRANSACTION;

        SELECT
            1 AS Id,
            'Certificación anulada correctamente.'
                AS Nombre;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
        BEGIN
            ROLLBACK TRANSACTION;
        END;

        SELECT
            0 AS Id,
            'No se pudo guardar la anulación. Detalle: '
                + ERROR_MESSAGE()
                AS Nombre;
    END CATCH;
END;
GO
/****** Objeto: StoredProcedure [dbo].[spGuardarCertificador] Fecha de script: 20/09/2026 05:35:11 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[spGuardarCertificador]
    @NoReloj VARCHAR(10)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @IdTrabajador INT;

    SET @NoReloj = LTRIM(RTRIM(@NoReloj));

    IF @NoReloj = ''
    BEGIN
        SELECT
            0 AS Id,
            'El número de reloj es obligatorio.' AS Nombre;

        RETURN;
    END;

    SELECT
        @IdTrabajador = Id
    FROM Trabajador
    WHERE NoReloj = @NoReloj;

    IF @IdTrabajador IS NULL
    BEGIN
        SELECT
            0 AS Id,
            'El trabajador no existe.' AS Nombre;

        RETURN;
    END;

    BEGIN TRY
        BEGIN TRANSACTION;

        IF EXISTS
        (
            SELECT 1
            FROM Certificador WITH (UPDLOCK, HOLDLOCK)
            WHERE IdTrabajador = @IdTrabajador
        )
        BEGIN
            COMMIT TRANSACTION;

            SELECT
                2 AS Id,
                'El trabajador ya es certificador.' AS Nombre;

            RETURN;
        END;

        INSERT INTO Certificador
        (
            IdTrabajador
        )
        VALUES
        (
            @IdTrabajador
        );

        COMMIT TRANSACTION;

        SELECT
            1 AS Id,
            'Se ha guardado correctamente.' AS Nombre;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        SELECT
            0 AS Id,
            'No se pudo guardar el certificador. Detalle: '
                + ERROR_MESSAGE() AS Nombre;
    END CATCH;
END;
GO
/****** Objeto: StoredProcedure [dbo].[spGuardarExpedienteTrabajador] Fecha de script: 20/09/2026 05:35:11 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[spGuardarExpedienteTrabajador]
    @IdTrabajador INT,
    @NombreOriginal VARCHAR(255),
    @NombreArchivo VARCHAR(255),
    @Extension VARCHAR(20),
    @RutaArchivo VARCHAR(500),
    @TipoArchivo VARCHAR(50) = NULL,
    @Comentario VARCHAR(300) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    SET @NombreOriginal =
        LTRIM(RTRIM(ISNULL(@NombreOriginal, '')));

    SET @NombreArchivo =
        LTRIM(RTRIM(ISNULL(@NombreArchivo, '')));

    SET @Extension =
        LTRIM(RTRIM(ISNULL(@Extension, '')));

    SET @RutaArchivo =
        LTRIM(RTRIM(ISNULL(@RutaArchivo, '')));

    SET @TipoArchivo =
        NULLIF(LTRIM(RTRIM(@TipoArchivo)), '');

    SET @Comentario =
        NULLIF(LTRIM(RTRIM(@Comentario)), '');

    IF NOT EXISTS
    (
        SELECT 1
        FROM Trabajador
        WHERE Id = @IdTrabajador
    )
    BEGIN
        SELECT
            0 AS Id,
            'No se encontró el trabajador seleccionado.' AS Nombre;

        RETURN;
    END;

    IF @NombreOriginal = ''
       OR @NombreArchivo = ''
       OR @Extension = ''
       OR @RutaArchivo = ''
    BEGIN
        SELECT
            0 AS Id,
            'Los datos principales del archivo están incompletos.' AS Nombre;

        RETURN;
    END;

    BEGIN TRY
        INSERT INTO ExpedienteTrabajador
        (
            IdTrabajador,
            NombreOriginal,
            NombreArchivo,
            Extension,
            RutaArchivo,
            TipoArchivo,
            Comentario,
            Activo,
            FechaRegistro,
            FechaModificacion
        )
        VALUES
        (
            @IdTrabajador,
            @NombreOriginal,
            @NombreArchivo,
            @Extension,
            @RutaArchivo,
            @TipoArchivo,
            @Comentario,
            1,
            GETDATE(),
            NULL
        );

        SELECT
            1 AS Id,
            'Archivo agregado correctamente al expediente.' AS Nombre;
    END TRY
    BEGIN CATCH
        SELECT
            0 AS Id,
            'No se pudo registrar el archivo. Detalle: '
                + ERROR_MESSAGE() AS Nombre;
    END CATCH;
END;
GO
/****** Objeto: StoredProcedure [dbo].[spGuardarLinea] Fecha de script: 20/09/2026 05:35:11 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[spGuardarLinea]
    @Id INT,
    @Nombre VARCHAR(100),
    @IdPlanta INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    SET @Nombre = UPPER(LTRIM(RTRIM(@Nombre)));

    IF @Nombre = ''
    BEGIN
        SELECT
            0 AS Id,
            'El nombre de la línea es obligatorio.' AS Nombre;

        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM Planta
        WHERE Id = @IdPlanta
    )
    BEGIN
        SELECT
            0 AS Id,
            'La planta seleccionada no existe.' AS Nombre;

        RETURN;
    END;

    IF EXISTS (
        SELECT 1
        FROM Linea
        WHERE IdPlanta = @IdPlanta
          AND Nombre = @Nombre
          AND Id <> @Id
    )
    BEGIN
        SELECT
            0 AS Id,
            'Ya existe una línea con ese nombre en la planta seleccionada.'
                AS Nombre;

        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM Linea
        WHERE Id = @Id
    )
    BEGIN
        INSERT INTO Linea (
            Nombre,
            IdPlanta
        )
        VALUES (
            @Nombre,
            @IdPlanta
        );

        SELECT
            1 AS Id,
            'Se ha registrado correctamente' AS Nombre;

        RETURN;
    END;

    UPDATE Linea
    SET
        Nombre = @Nombre,
        IdPlanta = @IdPlanta
    WHERE Id = @Id;

    SELECT
        2 AS Id,
        'Se ha modificado correctamente' AS Nombre;
END;
GO
/****** Objeto: StoredProcedure [dbo].[spGuardarLocalidad] Fecha de script: 20/09/2026 05:35:11 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[spGuardarLocalidad]
@Id INT,
@Nombre VARCHAR(100)
AS
BEGIN
	SET NOCOUNT ON;
	IF NOT EXISTS (SELECT Id FROM Localidad WHERE Id=@Id)
	BEGIN
		INSERT INTO Localidad (Nombre) VALUES (@Nombre);
		SELECT '1' AS [Id], 'Se ha registrado correctamente' AS [Nombre];
	END ELSE
		BEGIN
			UPDATE Localidad SET Nombre=@Nombre WHERE Id=@Id;
			SELECT '2' AS [Id], 'Se ha modificado correctamente' AS [Nombre];
		END
END
GO
/****** Objeto: StoredProcedure [dbo].[spGuardarPlanta] Fecha de script: 20/09/2026 05:35:11 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[spGuardarPlanta]
@Id INT,
@Nombre VARCHAR(100)
AS
BEGIN
	SET NOCOUNT ON;
	IF NOT EXISTS (SELECT Id FROM Planta WHERE Id=@Id)
	BEGIN
		INSERT INTO Planta (Nombre) VALUES (@Nombre);
		SELECT '1' AS [Id], 'Se ha registrado correctamente' AS [Nombre];
	END ELSE
		BEGIN
			UPDATE Planta SET Nombre=@Nombre WHERE Id=@Id;
			SELECT '2' AS [Id], 'Se ha modificado correctamente' AS [Nombre];
		END
END
GO
/****** Objeto: StoredProcedure [dbo].[spGuardarProceso] Fecha de script: 20/09/2026 05:35:11 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[spGuardarProceso]
@Id INT,
@Nombre VARCHAR(100),
@Descripcion VARCHAR(300),
@VigenciaMeses INT
AS
BEGIN
	SET NOCOUNT ON;
	IF NOT EXISTS (SELECT 1 FROM Proceso WHERE Id=@Id)
	BEGIN
		INSERT INTO Proceso (Nombre, Descripcion, VigenciaMeses) VALUES (@Nombre, @Descripcion, @VigenciaMeses);
		SELECT 1 AS [Id], 'Se ha guardado correctamente' AS [Nombre];
	END ELSE
		BEGIN
			UPDATE Proceso SET Nombre=@Nombre, Descripcion=@Descripcion, VigenciaMeses=@VigenciaMeses WHERE Id=@Id;
			SELECT 2 AS [Id], 'Se ha modificado correctamente' AS [Nombre];
		END
END
GO
/****** Objeto: StoredProcedure [dbo].[spGuardarTrabajador] Fecha de script: 20/09/2026 05:35:11 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/* ============================================================
   6. GUARDAR TRABAJADOR
   Compatible temporalmente con:
   - Código anterior: envía IdLocalidad + IdLinea.
   - Código nuevo: envía IdPlanta + IdLinea opcional.

   Como los parámetros se envían por nombre desde C#,
   el orden no afecta a los repositorios.
   ============================================================ */

CREATE   PROCEDURE [dbo].[spGuardarTrabajador]
    @Id INT,
    @NoReloj VARCHAR(10),
    @Nombre VARCHAR(200),
    @IdTurno INT,
    @RutaFoto VARCHAR(300) = NULL,
    @IdLocalidad INT = NULL,
    @IdLinea INT = NULL,
    @IdPlanta INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SET @NoReloj =
        LTRIM(RTRIM(@NoReloj));

    SET @Nombre =
        UPPER(LTRIM(RTRIM(@Nombre)));

    /*
        Compatibilidad con el código anterior:
        si no envía IdPlanta pero sí IdLinea,
        obtenemos la planta desde esa línea.
    */
    IF @IdPlanta IS NULL
       AND @IdLinea IS NOT NULL
    BEGIN
        SELECT
            @IdPlanta = IdPlanta
        FROM dbo.Linea
        WHERE Id = @IdLinea;
    END;

    IF NULLIF(@NoReloj, '') IS NULL
    BEGIN
        SELECT
            0 AS Id,
            'El número de reloj es obligatorio.' AS Nombre;
        RETURN;
    END;

    IF NULLIF(@Nombre, '') IS NULL
    BEGIN
        SELECT
            0 AS Id,
            'El nombre del trabajador es obligatorio.' AS Nombre;
        RETURN;
    END;

    IF NOT EXISTS
    (
        SELECT 1
        FROM dbo.Turno
        WHERE Id = @IdTurno
    )
    BEGIN
        SELECT
            0 AS Id,
            'El turno seleccionado no existe.' AS Nombre;
        RETURN;
    END;

    IF @IdPlanta IS NULL
       OR NOT EXISTS
       (
           SELECT 1
           FROM dbo.Planta
           WHERE Id = @IdPlanta
       )
    BEGIN
        SELECT
            0 AS Id,
            'La planta seleccionada no existe.' AS Nombre;
        RETURN;
    END;

    IF @IdLinea IS NOT NULL
       AND NOT EXISTS
       (
           SELECT 1
           FROM dbo.Linea
           WHERE Id = @IdLinea
             AND IdPlanta = @IdPlanta
       )
    BEGIN
        SELECT
            0 AS Id,
            'La línea seleccionada no pertenece a la planta.' AS Nombre;
        RETURN;
    END;

    IF EXISTS
    (
        SELECT 1
        FROM dbo.Trabajador
        WHERE NoReloj = @NoReloj
          AND Id <> ISNULL(@Id, 0)
    )
    BEGIN
        SELECT
            2 AS Id,
            'Ya existe un trabajador con ese número de reloj.' AS Nombre;
        RETURN;
    END;

    IF ISNULL(@Id, 0) > 0
       AND EXISTS
       (
           SELECT 1
           FROM dbo.Trabajador
           WHERE Id = @Id
       )
    BEGIN
        UPDATE dbo.Trabajador
        SET
            NoReloj = @NoReloj,
            Nombre = @Nombre,
            RutaFoto = @RutaFoto,
            IdLocalidad =
                CASE
                    WHEN @IdLocalidad IS NULL
                        THEN IdLocalidad
                    ELSE @IdLocalidad
                END,
            IdTurno = @IdTurno,
            IdPlanta = @IdPlanta,
            IdLinea = @IdLinea
        WHERE Id = @Id;

        SELECT
            3 AS Id,
            'Se ha modificado correctamente' AS Nombre;

        RETURN;
    END;

    INSERT INTO dbo.Trabajador
    (
        NoReloj,
        Nombre,
        RutaFoto,
        IdLocalidad,
        IdTurno,
        IdPlanta,
        IdLinea
    )
    VALUES
    (
        @NoReloj,
        @Nombre,
        @RutaFoto,
        @IdLocalidad,
        @IdTurno,
        @IdPlanta,
        @IdLinea
    );

    SELECT
        1 AS Id,
        'Se ha guardado correctamente' AS Nombre;
END;

GO
/****** Objeto: StoredProcedure [dbo].[spGuardarTurno] Fecha de script: 20/09/2026 05:35:11 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[spGuardarTurno]
@Id INT,
@Nombre VARCHAR(100)
AS
BEGIN
	SET NOCOUNT ON;
	IF NOT EXISTS (SELECT Id FROM Turno WHERE Id=@Id)
	BEGIN
		INSERT INTO Turno (Nombre) VALUES (@Nombre);
		SELECT '1' AS [Id], 'Se ha registrado correctamente' AS [Nombre];
	END ELSE
		BEGIN
			UPDATE Turno SET Nombre=@Nombre WHERE Id=@Id;
			SELECT '2' AS [Id], 'Se ha modificado correctamente' AS [Nombre];
		END
END
GO
/****** Objeto: StoredProcedure [dbo].[spModificarCertificacionAnulacion] Fecha de script: 20/09/2026 05:35:11 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[spModificarCertificacionAnulacion]
    @Id INT,
    @TipoAnulacion VARCHAR(50),
    @FechaInicio DATE,
    @FechaFin DATE = NULL,
    @EsPermanente BIT,
    @Comentario VARCHAR(500)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    SET @TipoAnulacion =
        LTRIM(RTRIM(ISNULL(@TipoAnulacion, '')));

    SET @Comentario =
        LTRIM(RTRIM(ISNULL(@Comentario, '')));

    IF NOT EXISTS
    (
        SELECT 1
        FROM CertificacionAnulacion
        WHERE Id = @Id
          AND Activa = 1
    )
    BEGIN
        SELECT
            0 AS Id,
            'No se encontró una anulación activa para modificar.'
                AS Nombre;

        RETURN;
    END;

    IF @TipoAnulacion = ''
    BEGIN
        SELECT
            0 AS Id,
            'Debe seleccionar un tipo de anulación.'
                AS Nombre;

        RETURN;
    END;

    IF @FechaInicio IS NULL
    BEGIN
        SELECT
            0 AS Id,
            'La fecha de inicio es obligatoria.'
                AS Nombre;

        RETURN;
    END;

    IF @EsPermanente = 0
       AND @FechaFin IS NULL
    BEGIN
        SELECT
            0 AS Id,
            'Debe seleccionar una fecha fin para una anulación temporal.'
                AS Nombre;

        RETURN;
    END;

    IF @EsPermanente = 0
       AND @FechaFin < @FechaInicio
    BEGIN
        SELECT
            0 AS Id,
            'La fecha fin no puede ser menor que la fecha inicio.'
                AS Nombre;

        RETURN;
    END;

    IF @Comentario = ''
    BEGIN
        SELECT
            0 AS Id,
            'Debe escribir un comentario de la anulación.'
                AS Nombre;

        RETURN;
    END;

    BEGIN TRY
        UPDATE CertificacionAnulacion
        SET
            TipoAnulacion = @TipoAnulacion,
            FechaInicio = @FechaInicio,
            FechaFin =
                CASE
                    WHEN @EsPermanente = 1
                        THEN NULL
                    ELSE @FechaFin
                END,
            EsPermanente = @EsPermanente,
            Comentario = @Comentario,
            FechaModificacion = GETDATE()
        WHERE Id = @Id
          AND Activa = 1;

        SELECT
            1 AS Id,
            'Anulación modificada correctamente.'
                AS Nombre;
    END TRY
    BEGIN CATCH
        SELECT
            0 AS Id,
            'No se pudo modificar la anulación. Detalle: '
                + ERROR_MESSAGE()
                AS Nombre;
    END CATCH;
END;
GO
/****** Objeto: StoredProcedure [dbo].[spReemplazarExpedienteTrabajador] Fecha de script: 20/09/2026 05:35:11 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[spReemplazarExpedienteTrabajador]
    @Id INT,
    @NombreOriginal VARCHAR(255),
    @NombreArchivo VARCHAR(255),
    @Extension VARCHAR(20),
    @RutaArchivo VARCHAR(500),
    @TipoArchivo VARCHAR(50) = NULL,
    @Comentario VARCHAR(300) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    SET @NombreOriginal =
        LTRIM(RTRIM(ISNULL(@NombreOriginal, '')));

    SET @NombreArchivo =
        LTRIM(RTRIM(ISNULL(@NombreArchivo, '')));

    SET @Extension =
        LTRIM(RTRIM(ISNULL(@Extension, '')));

    SET @RutaArchivo =
        LTRIM(RTRIM(ISNULL(@RutaArchivo, '')));

    SET @TipoArchivo =
        NULLIF(LTRIM(RTRIM(@TipoArchivo)), '');

    SET @Comentario =
        NULLIF(LTRIM(RTRIM(@Comentario)), '');

    IF NOT EXISTS
    (
        SELECT 1
        FROM ExpedienteTrabajador
        WHERE Id = @Id
          AND Activo = 1
    )
    BEGIN
        SELECT
            0 AS Id,
            'No se encontró el archivo activo que intenta reemplazar.'
                AS Nombre;

        RETURN;
    END;

    IF @NombreOriginal = ''
       OR @NombreArchivo = ''
       OR @Extension = ''
       OR @RutaArchivo = ''
    BEGIN
        SELECT
            0 AS Id,
            'Los datos principales del archivo están incompletos.' AS Nombre;

        RETURN;
    END;

    BEGIN TRY
        UPDATE ExpedienteTrabajador
        SET
            NombreOriginal = @NombreOriginal,
            NombreArchivo = @NombreArchivo,
            Extension = @Extension,
            RutaArchivo = @RutaArchivo,
            TipoArchivo = @TipoArchivo,
            Comentario = @Comentario,
            FechaModificacion = GETDATE()
        WHERE Id = @Id
          AND Activo = 1;

        SELECT
            1 AS Id,
            'Archivo reemplazado correctamente.' AS Nombre;
    END TRY
    BEGIN CATCH
        SELECT
            0 AS Id,
            'No se pudo reemplazar el archivo. Detalle: '
                + ERROR_MESSAGE() AS Nombre;
    END CATCH;
END;
GO
/****** Objeto: Trigger [dbo].[TR_Trabajador_ValidarLineaPlanta] Fecha de script: 20/09/2026 05:35:11 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/* ============================================================
   3. PROTECCIÓN PLANTA / LÍNEA
   ============================================================ */

CREATE   TRIGGER [dbo].[TR_Trabajador_ValidarLineaPlanta]
ON [dbo].[Trabajador]
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS
    (
        SELECT 1
        FROM inserted AS I
        LEFT JOIN dbo.Linea AS L
            ON L.Id = I.IdLinea
        WHERE I.IdLinea IS NOT NULL
          AND
          (
              L.Id IS NULL
              OR L.IdPlanta <> I.IdPlanta
          )
    )
    BEGIN
        THROW 51003,
            'La línea seleccionada no pertenece a la planta del trabajador.',
            1;
    END;
END;

GO
ALTER TABLE [dbo].[Trabajador] ENABLE TRIGGER [TR_Trabajador_ValidarLineaPlanta]
GO
