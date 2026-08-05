/*
    AseguranzaBD - Esquema actual limpio
    Generado a partir del esquema local del 2026-08-04.
    Contiene solamente estructura: tablas, restricciones, índices
    y procedimientos almacenados. No contiene datos.

    Requisito: la base [AseguranzaBD] debe existir antes de ejecutar.
*/

USE [AseguranzaBD]
GO
/****** Objeto: Table [dbo].[Certificacion] Fecha de script: 04/08/2026 11:38:20 p. m. ******/
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
/****** Objeto: Table [dbo].[CertificacionAnulacion] Fecha de script: 04/08/2026 11:38:20 p. m. ******/
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
/****** Objeto: Table [dbo].[Certificador] Fecha de script: 04/08/2026 11:38:20 p. m. ******/
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
/****** Objeto: Table [dbo].[ExpedienteTrabajador] Fecha de script: 04/08/2026 11:38:20 p. m. ******/
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
/****** Objeto: Table [dbo].[Linea] Fecha de script: 04/08/2026 11:38:20 p. m. ******/
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
/****** Objeto: Table [dbo].[Localidad] Fecha de script: 04/08/2026 11:38:20 p. m. ******/
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
/****** Objeto: Table [dbo].[Planta] Fecha de script: 04/08/2026 11:38:20 p. m. ******/
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
/****** Objeto: Table [dbo].[Proceso] Fecha de script: 04/08/2026 11:38:20 p. m. ******/
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
/****** Objeto: Table [dbo].[Trabajador] Fecha de script: 04/08/2026 11:38:20 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Trabajador](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[NoReloj] [varchar](10) NOT NULL,
	[Nombre] [varchar](200) NOT NULL,
	[RutaFoto] [varchar](300) NULL,
	[IdLocalidad] [int] NOT NULL,
	[IdTurno] [int] NOT NULL,
	[IdLinea] [int] NOT NULL,
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
/****** Objeto: Table [dbo].[Turno] Fecha de script: 04/08/2026 11:38:20 p. m. ******/
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
/****** Objeto: Index [IX_Certificacion_FechaVencimiento] Fecha de script: 04/08/2026 11:38:20 p. m. ******/
CREATE NONCLUSTERED INDEX [IX_Certificacion_FechaVencimiento] ON [dbo].[Certificacion]
(
	[FechaVencimiento] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Objeto: Index [IX_Certificacion_IdProceso] Fecha de script: 04/08/2026 11:38:20 p. m. ******/
CREATE NONCLUSTERED INDEX [IX_Certificacion_IdProceso] ON [dbo].[Certificacion]
(
	[IdProceso] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Objeto: Index [IX_Certificacion_IdTrabajador] Fecha de script: 04/08/2026 11:38:20 p. m. ******/
CREATE NONCLUSTERED INDEX [IX_Certificacion_IdTrabajador] ON [dbo].[Certificacion]
(
	[IdTrabajador] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Objeto: Index [IX_CertificacionAnulacion_IdCertificacion] Fecha de script: 04/08/2026 11:38:20 p. m. ******/
CREATE NONCLUSTERED INDEX [IX_CertificacionAnulacion_IdCertificacion] ON [dbo].[CertificacionAnulacion]
(
	[IdCertificacion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Objeto: Index [IX_Certificador_IdTrabajador] Fecha de script: 04/08/2026 11:38:20 p. m. ******/
CREATE NONCLUSTERED INDEX [IX_Certificador_IdTrabajador] ON [dbo].[Certificador]
(
	[IdTrabajador] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Objeto: Index [IX_ExpedienteTrabajador_IdTrabajador] Fecha de script: 04/08/2026 11:38:20 p. m. ******/
CREATE NONCLUSTERED INDEX [IX_ExpedienteTrabajador_IdTrabajador] ON [dbo].[ExpedienteTrabajador]
(
	[IdTrabajador] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Objeto: Index [IX_Linea_IdPlanta] Fecha de script: 04/08/2026 11:38:20 p. m. ******/
CREATE NONCLUSTERED INDEX [IX_Linea_IdPlanta] ON [dbo].[Linea]
(
	[IdPlanta] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Objeto: Index [IX_Trabajador_IdLinea] Fecha de script: 04/08/2026 11:38:20 p. m. ******/
CREATE NONCLUSTERED INDEX [IX_Trabajador_IdLinea] ON [dbo].[Trabajador]
(
	[IdLinea] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Objeto: Index [IX_Trabajador_IdLocalidad] Fecha de script: 04/08/2026 11:38:20 p. m. ******/
CREATE NONCLUSTERED INDEX [IX_Trabajador_IdLocalidad] ON [dbo].[Trabajador]
(
	[IdLocalidad] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Objeto: Index [IX_Trabajador_IdTurno] Fecha de script: 04/08/2026 11:38:20 p. m. ******/
CREATE NONCLUSTERED INDEX [IX_Trabajador_IdTurno] ON [dbo].[Trabajador]
(
	[IdTurno] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[CertificacionAnulacion] ADD  DEFAULT (CONVERT([date],getdate())) FOR [FechaInicio]
GO
ALTER TABLE [dbo].[CertificacionAnulacion] ADD  DEFAULT ((0)) FOR [EsPermanente]
GO
ALTER TABLE [dbo].[CertificacionAnulacion] ADD  DEFAULT ((1)) FOR [Activa]
GO
ALTER TABLE [dbo].[CertificacionAnulacion] ADD  DEFAULT (getdate()) FOR [FechaRegistro]
GO
ALTER TABLE [dbo].[ExpedienteTrabajador] ADD  DEFAULT ((1)) FOR [Activo]
GO
ALTER TABLE [dbo].[ExpedienteTrabajador] ADD  DEFAULT (getdate()) FOR [FechaRegistro]
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
ALTER TABLE [dbo].[ExpedienteTrabajador]  WITH CHECK ADD  CONSTRAINT [FK_ExpedienteTrabajador_Trabajador] FOREIGN KEY([IdTrabajador])
REFERENCES [dbo].[Trabajador] ([Id])
GO
ALTER TABLE [dbo].[ExpedienteTrabajador] CHECK CONSTRAINT [FK_ExpedienteTrabajador_Trabajador]
GO
ALTER TABLE [dbo].[Linea]  WITH CHECK ADD FOREIGN KEY([IdPlanta])
REFERENCES [dbo].[Planta] ([Id])
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
ALTER TABLE [dbo].[Certificacion]  WITH CHECK ADD  CONSTRAINT [CK_Fechas_Certificacion] CHECK  (([FechaVencimiento]>[FechaCertificacion]))
GO
ALTER TABLE [dbo].[Certificacion] CHECK CONSTRAINT [CK_Fechas_Certificacion]
GO
/****** Objeto: StoredProcedure [dbo].[spBorrarCertificacion] Fecha de script: 04/08/2026 11:38:21 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[spBorrarCertificacion]
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        IF NOT EXISTS (SELECT 1 FROM Certificacion WHERE Id = @Id)
        BEGIN
            ROLLBACK TRANSACTION;

            SELECT 
                0 AS Id,
                'No existe la certificación que intenta borrar.' AS Nombre;

            RETURN;
        END

        -- Primero borrar historial de anulaciones relacionado
        DELETE FROM CertificacionAnulacion
        WHERE IdCertificacion = @Id;

        -- Después borrar la certificación
        DELETE FROM Certificacion
        WHERE Id = @Id;

        COMMIT TRANSACTION;

        SELECT 
            1 AS Id,
            'Certificación eliminada correctamente.' AS Nombre;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        SELECT 
            0 AS Id,
            'No se pudo eliminar la certificación. Detalle: ' + ERROR_MESSAGE() AS Nombre;
    END CATCH
END
GO
/****** Objeto: StoredProcedure [dbo].[spBorrarCertificador] Fecha de script: 04/08/2026 11:38:21 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[spBorrarCertificador]
@Id INT
AS
BEGIN
	SET NOCOUNT ON;
	IF NOT EXISTS (SELECT 1 FROM Certificacion WHERE IdCertificador=@Id)
	BEGIN
		DELETE FROM Certificador WHERE Id=@Id
		SELECT '1' AS [Id], 'Se ha borrado correctamente' AS [Nombre];	
	END ELSE
		BEGIN
			SELECT '2' AS [Id], 'No se puede borrar por que hay registros asociados a este certificador' AS [Nombre];
		END
END
GO
/****** Objeto: StoredProcedure [dbo].[spBorrarLinea] Fecha de script: 04/08/2026 11:38:21 p. m. ******/
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
/****** Objeto: StoredProcedure [dbo].[spBorrarLocalidad] Fecha de script: 04/08/2026 11:38:21 p. m. ******/
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
/****** Objeto: StoredProcedure [dbo].[spBorrarPlanta] Fecha de script: 04/08/2026 11:38:21 p. m. ******/
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
/****** Objeto: StoredProcedure [dbo].[spBorrarProceso] Fecha de script: 04/08/2026 11:38:21 p. m. ******/
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
/****** Objeto: StoredProcedure [dbo].[spBorrarTrabajador] Fecha de script: 04/08/2026 11:38:21 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[spBorrarTrabajador]
@Id INT
AS
BEGIN
	SET NOCOUNT ON;
	IF NOT EXISTS (SELECT 1 FROM Certificador WHERE IdTrabajador=@Id) AND NOT EXISTS (SELECT 1 FROM Certificacion WHERE IdTrabajador=@Id)
	BEGIN
		DELETE FROM Trabajador WHERE Id=@Id
		SELECT '1' AS [Id], 'Se ha borrado correctamente' AS [Nombre];	
	END ELSE
		BEGIN
			SELECT '2' AS [Id], 'No se puede borrar por que hay registros asociados a este trabajador' AS [Nombre];
		END
END
GO
/****** Objeto: StoredProcedure [dbo].[spBorrarTurno] Fecha de script: 04/08/2026 11:38:21 p. m. ******/
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
/****** Objeto: StoredProcedure [dbo].[spConsultarAnulacionPorCertificacion] Fecha de script: 04/08/2026 11:38:21 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[spConsultarAnulacionPorCertificacion]
    @IdCertificacion INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 1
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
      AND (
            EsPermanente = 1
            OR FechaFin IS NULL
            OR FechaFin >= CAST(GETDATE() AS DATE)
          )
    ORDER BY FechaRegistro DESC;
END
GO
/****** Objeto: StoredProcedure [dbo].[spConsultarCertificacionesPorTrabajador] Fecha de script: 04/08/2026 11:38:21 p. m. ******/
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
/****** Objeto: StoredProcedure [dbo].[spConsultarCertificadores] Fecha de script: 04/08/2026 11:38:21 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[spConsultarCertificadores]
@TextoBuscar VARCHAR(100)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT Certificador.Id, Certificador.IdTrabajador, Trabajador.NoReloj, Trabajador.Nombre AS [NombreTrabajador], Trabajador.RutaFoto,
	Trabajador.IdTurno, Turno.Nombre AS [NombreTurno], Planta.Id AS [IdPlanta], Planta.Nombre AS [NombrePlanta], Linea.Id AS [IdLinea], 
	Linea.Nombre AS [NombreLinea] FROM Certificador INNER JOIN Trabajador ON Certificador.IdTrabajador = Trabajador.Id
	INNER JOIN Turno ON Turno.Id = Trabajador.IdTurno INNER JOIN Linea ON Linea.Id = Trabajador.IdLinea INNER JOIN Planta ON Planta.Id = Linea.IdPlanta
	WHERE Trabajador.NoReloj LIKE '%'+@TextoBuscar+'%' OR Trabajador.Nombre LIKE '%'+@TextoBuscar+'%' OR Turno.Nombre LIKE '%'+@TextoBuscar+'%'
	OR Planta.Nombre LIKE '%'+@TextoBuscar+'%' OR Linea.Nombre LIKE '%'+@TextoBuscar+'%'
END
GO
/****** Objeto: StoredProcedure [dbo].[spConsultarExpedienteTrabajador] Fecha de script: 04/08/2026 11:38:21 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[spConsultarExpedienteTrabajador]
    @IdTrabajador INT
AS
BEGIN
    SET NOCOUNT ON;

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
END
GO
/****** Objeto: StoredProcedure [dbo].[spConsultarLineas] Fecha de script: 04/08/2026 11:38:21 p. m. ******/
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
/****** Objeto: StoredProcedure [dbo].[spConsultarLineasPorPlanta] Fecha de script: 04/08/2026 11:38:21 p. m. ******/
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
/****** Objeto: StoredProcedure [dbo].[spConsultarLocalidades] Fecha de script: 04/08/2026 11:38:21 p. m. ******/
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
/****** Objeto: StoredProcedure [dbo].[spConsultarPlantas] Fecha de script: 04/08/2026 11:38:21 p. m. ******/
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
/****** Objeto: StoredProcedure [dbo].[spConsultarProcesos] Fecha de script: 04/08/2026 11:38:21 p. m. ******/
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
/****** Objeto: StoredProcedure [dbo].[spConsultarTrabajador] Fecha de script: 04/08/2026 11:38:21 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[spConsultarTrabajador]
    @NoReloj VARCHAR(10)
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM Trabajador WHERE NoReloj = @NoReloj)
    BEGIN
        SELECT 
            Trabajador.Id,
            Trabajador.NoReloj,
            Trabajador.Nombre,
            Trabajador.RutaFoto,

            Localidad.Id AS IdLocalidad,
            Localidad.Nombre AS Localidad,

            Turno.Id AS IdTurno,
            Turno.Nombre AS Turno,

            Planta.Id AS IdPlanta,
            Planta.Nombre AS Planta,

            Linea.Id AS IdLinea,
            Linea.Nombre AS Linea

        FROM Trabajador
        INNER JOIN Localidad 
            ON Localidad.Id = Trabajador.IdLocalidad
        INNER JOIN Turno 
            ON Turno.Id = Trabajador.IdTurno
        INNER JOIN Linea 
            ON Linea.Id = Trabajador.IdLinea
        INNER JOIN Planta 
            ON Planta.Id = Linea.IdPlanta
        WHERE Trabajador.NoReloj = @NoReloj;
    END
    ELSE
    BEGIN
        SELECT 
            1 AS Id,
            'No existe trabajador con ese número de reloj' AS Nombre;
    END
END
GO
/****** Objeto: StoredProcedure [dbo].[spConsultarTrabajadores] Fecha de script: 04/08/2026 11:38:21 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[spConsultarTrabajadores]
@TextoBuscar VARCHAR(100)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT TOP(100) Trabajador.Id, Trabajador.NoReloj, Trabajador.Nombre, Trabajador.RutaFoto, Trabajador.IdLocalidad, Localidad.Nombre AS [NombreLocalidad], Trabajador.IdTurno, 
	Turno.Nombre AS [NombreTurno], Linea.IdPlanta, Planta.Nombre AS [NombrePlanta], Trabajador.IdLinea, Linea.Nombre AS [NombreLinea] FROM Trabajador
	INNER JOIN Localidad ON Localidad.Id = Trabajador.IdLocalidad INNER JOIN Turno ON Turno.Id = Trabajador.IdTurno INNER JOIN Linea ON Linea.Id = Trabajador.IdLinea
	INNER JOIN Planta ON Planta.Id = Linea.IdPlanta WHERE Trabajador.NoReloj LIKE '%'+@TextoBuscar+'%' OR Trabajador.Nombre LIKE '%'+@TextoBuscar+'%'
	OR Localidad.Nombre LIKE '%'+@TextoBuscar+'%' OR Turno.Nombre LIKE '%'+@TextoBuscar+'%' OR Planta.Nombre LIKE '%'+@TextoBuscar+'%' OR Linea.Nombre LIKE '%'+@TextoBuscar+'%';
END
GO
/****** Objeto: StoredProcedure [dbo].[spConsultarTrabajadoresCertificaciones] Fecha de script: 04/08/2026 11:38:21 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spConsultarTrabajadoresCertificaciones]
@MostrarPor VARCHAR(10),
@TextoBuscar VARCHAR(100)
AS
BEGIN
	IF(@MostrarPor = 'TODAS')
	BEGIN
		SELECT Trabajador.Id, Trabajador.NoReloj, Trabajador.Nombre, Trabajador.RutaFoto, Trabajador.IdLocalidad, Localidad.Nombre AS [NombreLocalidad], Trabajador.IdTurno, 
		Turno.Nombre AS [NombreTurno], Linea.IdPlanta, Planta.Nombre AS [NombrePlanta], Trabajador.IdLinea, Linea.Nombre AS [NombreLinea] FROM Trabajador
		INNER JOIN Localidad ON Localidad.Id = Trabajador.IdLocalidad INNER JOIN Turno ON Turno.Id = Trabajador.IdTurno INNER JOIN Linea ON Linea.Id = Trabajador.IdLinea
		INNER JOIN Planta ON Planta.Id = Linea.IdPlanta WHERE Trabajador.NoReloj LIKE '%'+@TextoBuscar+'%' OR Trabajador.Nombre LIKE '%'+@TextoBuscar+'%'
		OR Localidad.Nombre LIKE '%'+@TextoBuscar+'%' OR Turno.Nombre LIKE '%'+@TextoBuscar+'%' OR Planta.Nombre LIKE '%'+@TextoBuscar+'%' OR Linea.Nombre LIKE '%'+@TextoBuscar+'%';
	END
END
GO
/****** Objeto: StoredProcedure [dbo].[spConsultarTrabajadoresEstadoCertificacion] Fecha de script: 04/08/2026 11:38:21 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[spConsultarTrabajadoresEstadoCertificacion]
    @MostrarPor VARCHAR(20),
    @TextoBuscar VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    ;WITH CTE_Estado AS
    (
        SELECT TOP(300)
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
            Li.Nombre AS NombreLinea,

            CASE
                WHEN COUNT(C.Id) = 0 THEN 'Sin certificar'

                WHEN SUM(
                    CASE 
                        WHEN C.FechaVencimiento < CAST(GETDATE() AS DATE)
                        THEN 1 ELSE 0 
                    END
                ) > 0 THEN 'Vencida'

                WHEN SUM(
                    CASE 
                        WHEN C.FechaVencimiento BETWEEN CAST(GETDATE() AS DATE)
                             AND DATEADD(DAY, 30, CAST(GETDATE() AS DATE))
                        THEN 1 ELSE 0 
                    END
                ) > 0 THEN 'Por vencer'

                ELSE 'Vigente'
            END AS EstadoCertificacion

        FROM Trabajador T
        INNER JOIN Localidad L 
            ON L.Id = T.IdLocalidad
        INNER JOIN Turno Tu 
            ON Tu.Id = T.IdTurno
        INNER JOIN Linea Li 
            ON Li.Id = T.IdLinea
        INNER JOIN Planta P 
            ON P.Id = Li.IdPlanta

        LEFT JOIN Certificacion C 
            ON C.IdTrabajador = T.Id
           AND NOT EXISTS
           (
                SELECT 1
                FROM CertificacionAnulacion CA
                WHERE CA.IdCertificacion = C.Id
                  AND CA.Activa = 1
                  AND (
                        CA.EsPermanente = 1
                        OR CA.FechaFin IS NULL
                        OR CA.FechaFin >= CAST(GETDATE() AS DATE)
                      )
           )

        WHERE
        (
            T.NoReloj LIKE '%' + @TextoBuscar + '%'
            OR T.Nombre LIKE '%' + @TextoBuscar + '%'
            OR L.Nombre LIKE '%' + @TextoBuscar + '%'
            OR Tu.Nombre LIKE '%' + @TextoBuscar + '%'
            OR P.Nombre LIKE '%' + @TextoBuscar + '%'
            OR Li.Nombre LIKE '%' + @TextoBuscar + '%'
            OR @TextoBuscar = ''
        )

        GROUP BY
            T.Id,
            T.NoReloj,
            T.Nombre,
            T.RutaFoto,
            T.IdLocalidad,
            L.Nombre,
            T.IdTurno,
            Tu.Nombre,
            Li.IdPlanta,
            P.Nombre,
            T.IdLinea,
            Li.Nombre
    )

    SELECT *
    FROM CTE_Estado
    WHERE @MostrarPor = 'Todas'
       OR EstadoCertificacion = @MostrarPor;
END
GO
/****** Objeto: StoredProcedure [dbo].[spConsultarTurnos] Fecha de script: 04/08/2026 11:38:21 p. m. ******/
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
/****** Objeto: StoredProcedure [dbo].[spConsultarVerificacionNoReloj] Fecha de script: 04/08/2026 11:38:21 p. m. ******/
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
        Li.Nombre AS NombreLinea,

        C.Id AS IdCertificacion,
        Pr.Id AS IdProceso,
        Pr.Nombre AS Proceso,
        C.FechaCertificacion,
        C.FechaVencimiento,
        DATEDIFF(DAY, CAST(GETDATE() AS DATE), C.FechaVencimiento) AS DiasRestantes,
        C.Comentario,

        C.IdCertificador,
        TCert.Nombre AS NombreCertificador

    FROM Trabajador T
    INNER JOIN Localidad L
        ON L.Id = T.IdLocalidad
    INNER JOIN Turno Tu
        ON Tu.Id = T.IdTurno
    INNER JOIN Linea Li
        ON Li.Id = T.IdLinea
    INNER JOIN Planta Pta
        ON Pta.Id = Li.IdPlanta

    LEFT JOIN Certificacion C
        ON C.IdTrabajador = T.Id
    LEFT JOIN Proceso Pr
        ON Pr.Id = C.IdProceso
    LEFT JOIN Certificador Cert
        ON Cert.Id = C.IdCertificador
    LEFT JOIN Trabajador TCert
        ON TCert.Id = Cert.IdTrabajador

    WHERE T.NoReloj = @NoReloj
      AND (
            C.Id IS NULL
            OR NOT EXISTS
            (
                SELECT 1
                FROM CertificacionAnulacion CA
                WHERE CA.IdCertificacion = C.Id
                  AND CA.Activa = 1
                  AND (
                        CA.EsPermanente = 1
                        OR CA.FechaFin IS NULL
                        OR CA.FechaFin >= CAST(GETDATE() AS DATE)
                      )
            )
          )

    ORDER BY Pr.Nombre;
END
GO
/****** Objeto: StoredProcedure [dbo].[spEliminarCertificacionAnulacion] Fecha de script: 04/08/2026 11:38:21 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[spEliminarCertificacionAnulacion]
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM CertificacionAnulacion WHERE Id = @Id AND Activa = 1)
    BEGIN
        SELECT 0 AS Id, 'No se encontró una anulación activa para eliminar.' AS Nombre;
        RETURN;
    END

    UPDATE CertificacionAnulacion
    SET
        Activa = 0,
        FechaModificacion = GETDATE()
    WHERE Id = @Id;

    SELECT 1 AS Id, 'Anulación eliminada correctamente.' AS Nombre;
END
GO
/****** Objeto: StoredProcedure [dbo].[spEliminarExpedienteTrabajador] Fecha de script: 04/08/2026 11:38:21 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[spEliminarExpedienteTrabajador]
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE ExpedienteTrabajador
    SET
        Activo = 0,
        FechaModificacion = GETDATE()
    WHERE Id = @Id;

    SELECT
        1 AS Id,
        'Archivo eliminado correctamente del expediente.' AS Nombre;
END
GO
/****** Objeto: StoredProcedure [dbo].[spGuardarCertificacion] Fecha de script: 04/08/2026 11:38:21 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[spGuardarCertificacion]
(
    @IdTrabajador INT,
    @IdProceso INT,
    @FechaCertificacion DATE,
    @IdCertificador INT,
    @Comentario VARCHAR(300)
)
AS
BEGIN
    SET NOCOUNT ON;

        DECLARE @IdCertificacionExistente INT;

    SELECT @IdCertificacionExistente = Id
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
              AND (
                    EsPermanente = 1
                    OR FechaFin IS NULL
                    OR FechaFin >= CAST(GETDATE() AS DATE)
                  )
       )
    BEGIN
        SELECT
            0 AS Id,
            'Esta certificación se encuentra anulada. No se puede agregar, modificar ni renovar mientras tenga una anulación activa.' AS Nombre;
        RETURN;
    END

    DECLARE @VigenciaMeses INT;
    DECLARE @FechaVencimiento DATE;

    -- Obtener vigencia desde Proceso
    SELECT @VigenciaMeses = VigenciaMeses
    FROM Proceso
    WHERE Id = @IdProceso;

    SET @FechaVencimiento =
        DATEADD(MONTH, @VigenciaMeses, @FechaCertificacion);

    IF EXISTS (
        SELECT 1
        FROM Certificacion
        WHERE IdTrabajador = @IdTrabajador
          AND IdProceso = @IdProceso
    )
    BEGIN
        -- RENOVAR
        UPDATE Certificacion
        SET
            FechaCertificacion = @FechaCertificacion,
            FechaVencimiento = @FechaVencimiento,
            IdCertificador = @IdCertificador,
            Comentario = @Comentario
        WHERE
            IdTrabajador = @IdTrabajador
            AND IdProceso = @IdProceso;

        SELECT 2 AS [Id], 'Certificación renovada correctamente' AS [Nombre];
    END
    ELSE
    BEGIN
        -- INSERTAR
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

        SELECT 1 AS [Id], 'Certificación registrada correctamente' AS [Nombre];
    END
END
GO
/****** Objeto: StoredProcedure [dbo].[spGuardarCertificacionAnulacion] Fecha de script: 04/08/2026 11:38:21 p. m. ******/
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

    IF NOT EXISTS (SELECT 1 FROM Certificacion WHERE Id = @IdCertificacion)
    BEGIN
        SELECT 0 AS Id, 'No se encontró la certificación seleccionada.' AS Nombre;
        RETURN;
    END

    IF @EsPermanente = 0 AND @FechaFin IS NULL
    BEGIN
        SELECT 0 AS Id, 'Debe seleccionar una fecha fin para una anulación temporal.' AS Nombre;
        RETURN;
    END

    IF @EsPermanente = 0 AND @FechaFin < @FechaInicio
    BEGIN
        SELECT 0 AS Id, 'La fecha fin no puede ser menor que la fecha inicio.' AS Nombre;
        RETURN;
    END

    IF LTRIM(RTRIM(ISNULL(@Comentario, ''))) = ''
    BEGIN
        SELECT 0 AS Id, 'Debe escribir un comentario de la anulación.' AS Nombre;
        RETURN;
    END

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
        FechaRegistro
    )
    VALUES
    (
        @IdCertificacion,
        @TipoAnulacion,
        @FechaInicio,
        CASE WHEN @EsPermanente = 1 THEN NULL ELSE @FechaFin END,
        @EsPermanente,
        @Comentario,
        1,
        GETDATE()
    );

    SELECT 1 AS Id, 'Certificación anulada correctamente.' AS Nombre;
END
GO
/****** Objeto: StoredProcedure [dbo].[spGuardarCertificador] Fecha de script: 04/08/2026 11:38:21 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[spGuardarCertificador]
@NoReloj VARCHAR(10)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @IdTrabajador INT
	SELECT @IdTrabajador = Id FROM Trabajador WHERE NoReloj=@NoReloj;

	IF NOT EXISTS (SELECT 1 FROM Certificador WHERE IdTrabajador=@IdTrabajador)
	BEGIN
		INSERT INTO Certificador (IdTrabajador) VALUES (@IdTrabajador);
		SELECT '1' AS [Id], 'Se ha guardado correctamente' AS [Nombre];
	END ELSE
		BEGIN
			SELECT '2' AS [Id], 'El trabajador ya es certificador' AS [Nombre];	
		END
END
GO
/****** Objeto: StoredProcedure [dbo].[spGuardarExpedienteTrabajador] Fecha de script: 04/08/2026 11:38:21 p. m. ******/
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
        FechaRegistro
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
        GETDATE()
    );

    SELECT
        1 AS Id,
        'Archivo agregado correctamente al expediente.' AS Nombre;
END
GO
/****** Objeto: StoredProcedure [dbo].[spGuardarLinea] Fecha de script: 04/08/2026 11:38:21 p. m. ******/
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
	IF NOT EXISTS (SELECT Id FROM Linea WHERE Id=@Id)
	BEGIN
		INSERT INTO Linea (Nombre, IdPlanta) VALUES (@Nombre, @IdPlanta);
		SELECT '1' AS [Id], 'Se ha registrado correctamente' AS [Nombre];
	END ELSE
		BEGIN
			UPDATE Linea SET Nombre=@Nombre WHERE Id=@Id;
			SELECT '2' AS [Id], 'Se ha modificado correctamente' AS [Nombre];
		END
END
GO
/****** Objeto: StoredProcedure [dbo].[spGuardarLocalidad] Fecha de script: 04/08/2026 11:38:21 p. m. ******/
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
/****** Objeto: StoredProcedure [dbo].[spGuardarPlanta] Fecha de script: 04/08/2026 11:38:21 p. m. ******/
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
/****** Objeto: StoredProcedure [dbo].[spGuardarProceso] Fecha de script: 04/08/2026 11:38:21 p. m. ******/
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
/****** Objeto: StoredProcedure [dbo].[spGuardarTrabajador] Fecha de script: 04/08/2026 11:38:21 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[spGuardarTrabajador]
@Id INT,
@NoReloj VARCHAR(10),
@Nombre VARCHAR(100),
@RutaFoto VARCHAR(200),
@IdLocalidad INT,
@IdTurno INT,
@IdLinea INT
AS
BEGIN
	SET NOCOUNT ON;
	IF NOT EXISTS (SELECT Id FROM Trabajador WHERE Id=@Id)
	BEGIN
		IF NOT EXISTS (SELECT 1 FROM Trabajador WHERE NoReloj=@NoReloj)
		BEGIN
			INSERT INTO Trabajador (NoReloj, Nombre, RutaFoto, IdLocalidad, IdTurno, IdLinea) VALUES (@NoReloj, @Nombre, @RutaFoto, @IdLocalidad, @IdTurno, @IdLinea);
			SELECT '1' AS [Id], 'Se ha registrado correctamente' AS [Nombre];	
		END	ELSE
			BEGIN
				SELECT '2' AS [Id], 'Ya existe un trabajador registrado con ese numero de reloj' AS [Nombre];
			END
	END ELSE
		BEGIN
			UPDATE Trabajador SET Nombre=@Nombre, RutaFoto=@RutaFoto, IdLocalidad=@IdLocalidad, IdTurno=@IdTurno, IdLinea=@IdLinea WHERE Id=@Id AND NoReloj=@NoReloj;
			SELECT '3' AS [Id], 'Se ha modificado correctamente' AS [Nombre];
		END
END
GO
/****** Objeto: StoredProcedure [dbo].[spGuardarTurno] Fecha de script: 04/08/2026 11:38:21 p. m. ******/
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
/****** Objeto: StoredProcedure [dbo].[spModificarCertificacionAnulacion] Fecha de script: 04/08/2026 11:38:21 p. m. ******/
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

    IF NOT EXISTS (SELECT 1 FROM CertificacionAnulacion WHERE Id = @Id AND Activa = 1)
    BEGIN
        SELECT 0 AS Id, 'No se encontró una anulación activa para modificar.' AS Nombre;
        RETURN;
    END

    IF @EsPermanente = 0 AND @FechaFin IS NULL
    BEGIN
        SELECT 0 AS Id, 'Debe seleccionar una fecha fin para una anulación temporal.' AS Nombre;
        RETURN;
    END

    IF @EsPermanente = 0 AND @FechaFin < @FechaInicio
    BEGIN
        SELECT 0 AS Id, 'La fecha fin no puede ser menor que la fecha inicio.' AS Nombre;
        RETURN;
    END

    IF LTRIM(RTRIM(ISNULL(@Comentario, ''))) = ''
    BEGIN
        SELECT 0 AS Id, 'Debe escribir un comentario de la anulación.' AS Nombre;
        RETURN;
    END

    UPDATE CertificacionAnulacion
    SET
        TipoAnulacion = @TipoAnulacion,
        FechaInicio = @FechaInicio,
        FechaFin = CASE WHEN @EsPermanente = 1 THEN NULL ELSE @FechaFin END,
        EsPermanente = @EsPermanente,
        Comentario = @Comentario,
        FechaModificacion = GETDATE()
    WHERE Id = @Id
      AND Activa = 1;

    SELECT 1 AS Id, 'Anulación modificada correctamente.' AS Nombre;
END
GO
/****** Objeto: StoredProcedure [dbo].[spReemplazarExpedienteTrabajador] Fecha de script: 04/08/2026 11:38:21 p. m. ******/
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
END
GO
