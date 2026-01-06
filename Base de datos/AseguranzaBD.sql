CREATE DATABASE AseguranzaBD
USE AseguranzaBD
-------------------------------------------
CREATE TABLE Localidad
(
	Id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	Nombre VARCHAR(100) NOT NULL
)
ALTER PROCEDURE spConsultarLocalidades
@TextoBuscar VARCHAR(100)
AS
BEGIN
	SELECT * FROM Localidad WHERE Nombre LIKE '%'+@TextoBuscar+'%';
END
CREATE PROCEDURE spGuardarLocalidad
@Id INT,
@Nombre VARCHAR(100)
AS
BEGIN
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
ALTER PROCEDURE spBorrarLocalidad
@Id INT
AS
BEGIN
	DELETE FROM Localidad WHERE Id=@Id
	SELECT '1' AS [Id], 'Se ha borrado correctamente' AS [Nombre];
END
-------------------------------------------

-------------------------------------------
CREATE TABLE Turno
(
	Id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	Nombre VARCHAR(100) NOT NULL
)
ALTER PROCEDURE spConsultarTurnos
@TextoBuscar VARCHAR(100)
AS
BEGIN
	SELECT * FROM Turno WHERE Nombre LIKE '%'+@TextoBuscar+'%';
END
CREATE PROCEDURE spGuardarTurno
@Id INT,
@Nombre VARCHAR(100)
AS
BEGIN
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
CREATE PROCEDURE spBorrarTurno
@Id INT
AS
BEGIN
	DELETE FROM Turno WHERE Id=@Id
	SELECT '1' AS [Id], 'Se ha borrado correctamente' AS [Nombre];
END
-------------------------------------------

-------------------------------------------
CREATE TABLE Planta
(
	Id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	Nombre VARCHAR(100) NOT NULL
)
ALTER PROCEDURE spConsultarPlantas
@TextoBuscar VARCHAR(100)
AS
BEGIN
	SELECT * FROM Planta WHERE Nombre LIKE '%'+@TextoBuscar+'%';
END
CREATE PROCEDURE spGuardarPlanta
@Id INT,
@Nombre VARCHAR(100)
AS
BEGIN
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
CREATE PROCEDURE spBorrarPlanta
@Id INT
AS
BEGIN
	DELETE FROM Planta WHERE Id=@Id
	SELECT '1' AS [Id], 'Se ha borrado correctamente' AS [Nombre];
END
-------------------------------------------

-------------------------------------------
CREATE TABLE Linea
(
	Id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	Nombre VARCHAR(100) NOT NULL,
	IdPlanta INT NOT NULL FOREIGN KEY REFERENCES Planta(Id),
	CONSTRAINT UQ_Linea_Planta_Nombre
	UNIQUE (IdPlanta, Nombre)
)
ALTER PROCEDURE spConsultarLineas
@TextoBuscar VARCHAR(100)
AS
BEGIN
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
CREATE PROCEDURE spConsultarLineasPorPlanta
@IdPlanta INT
AS
BEGIN
	SELECT Id, Nombre FROM Linea WHERE IdPlanta=@IdPlanta
END
CREATE PROCEDURE spGuardarLinea
@Id INT,
@Nombre VARCHAR(100),
@IdPlanta INT
AS
BEGIN
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
CREATE PROCEDURE spBorrarLinea
@Id INT
AS
BEGIN
	DELETE FROM Linea WHERE Id=@Id
	SELECT '1' AS [Id], 'Se ha borrado correctamente' AS [Nombre];
END
-------------------------------------------

-------------------------------------------
CREATE TABLE Trabajador
(
	Id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	NoReloj VARCHAR(10) UNIQUE NOT NULL,
	Nombre VARCHAR(200) NOT NULL,
	RutaFoto VARCHAR(300),
	IdLocalidad INT NOT NULL FOREIGN KEY REFERENCES Localidad(Id),
	IdTurno INT NOT NULL FOREIGN KEY REFERENCES Turno(Id),
	IdLinea INT NOT NULL FOREIGN KEY REFERENCES Linea(Id)
)
ALTER PROCEDURE spConsultarTrabajadores
@TextoBuscar VARCHAR(100)
AS
BEGIN
	SELECT Trabajador.Id, Trabajador.NoReloj, Trabajador.Nombre, Trabajador.RutaFoto, Trabajador.IdLocalidad, Localidad.Nombre AS [NombreLocalidad], Trabajador.IdTurno, 
	Turno.Nombre AS [NombreTurno], Linea.IdPlanta, Planta.Nombre AS [NombrePlanta], Trabajador.IdLinea, Linea.Nombre AS [NombreLinea] FROM Trabajador
	INNER JOIN Localidad ON Localidad.Id = Trabajador.IdLocalidad INNER JOIN Turno ON Turno.Id = Trabajador.IdTurno INNER JOIN Linea ON Linea.Id = Trabajador.IdLinea
	INNER JOIN Planta ON Planta.Id = Linea.IdPlanta WHERE Trabajador.NoReloj LIKE '%'+@TextoBuscar+'%' OR Trabajador.Nombre LIKE '%'+@TextoBuscar+'%'
	OR Localidad.Nombre LIKE '%'+@TextoBuscar+'%' OR Turno.Nombre LIKE '%'+@TextoBuscar+'%' OR Planta.Nombre LIKE '%'+@TextoBuscar+'%' OR Linea.Nombre LIKE '%'+@TextoBuscar+'%';
END
--ALTER PROCEDURE spConsultarTrabajador
--@NoReloj VARCHAR(10)
--AS
--BEGIN
--	SELECT Trabajador.Id, Trabajador.Nombre, Trabajador.RutaFoto, Localidad.Id AS [IdLocalidad],Localidad.Nombre as [NombreLocalidad], Turno.Id AS [IdTurno] ,Turno.Nombre AS [NombreTurno], 
--	Planta.Id AS [IdPlanta], Planta.Nombre AS [NombrePlanta], Linea.Id AS [IdLinea], Linea.Nombre AS [NombreLinea] FROM Trabajador INNER JOIN Localidad
--	ON Localidad.Id=Trabajador.IdLocalidad INNER JOIN Turno ON Turno.Id=Trabajador.IdTurno 
--	INNER JOIN Linea ON Linea.Id=Trabajador.IdLinea INNER JOIN Planta ON Planta.Id=Linea.IdPlanta WHERE Trabajador.NoReloj=@NoReloj;
--END

CREATE PROCEDURE spGuardarTrabajador
@Id INT,
@NoReloj VARCHAR(10),
@Nombre VARCHAR(100),
@RutaFoto VARCHAR(200),
@IdLocalidad INT,
@IdTurno INT,
@IdLinea INT
AS
BEGIN
	IF NOT EXISTS (SELECT Id FROM Trabajador WHERE Id=@Id)
	BEGIN
		INSERT INTO Trabajador (NoReloj, Nombre, RutaFoto, IdLocalidad, IdTurno, IdLinea) VALUES (@NoReloj, @Nombre, @RutaFoto, @IdLocalidad, @IdTurno, @IdLinea);
		SELECT '1' AS [Id], 'Se ha registrado correctamente' AS [Nombre];
	END ELSE
		BEGIN
			UPDATE Trabajador SET Nombre=@Nombre, RutaFoto=@RutaFoto, IdLocalidad=@IdLocalidad, IdTurno=@IdTurno, IdLinea=@IdLinea WHERE Id=@Id AND NoReloj=@NoReloj;
			SELECT '2' AS [Id], 'Se ha modificado correctamente' AS [Nombre];
		END
END
CREATE PROCEDURE spBorrarTrabajador
@Id INT
AS
BEGIN
	DELETE FROM Trabajador WHERE Id=@Id
	SELECT '1' AS [Id], 'Se ha borrado correctamente' AS [Nombre];
END
ALTER PROCEDURE spConsultarTrabajador
@NoReloj VARCHAR(10)
AS
BEGIN
	IF EXISTS (SELECT Id FROM Trabajador WHERE NoReloj=@NoReloj)
	BEGIN
		SELECT Trabajador.Nombre, Trabajador.RutaFoto, Localidad.Nombre, Turno.Nombre, Planta.Nombre, Linea.Nombre FROM Trabajador
		INNER JOIN Localidad ON Localidad.Id=Trabajador.IdLocalidad INNER JOIN Turno ON Turno.Id=Trabajador.IdTurno
		INNER JOIN Linea ON Linea.Id=Trabajador.IdLinea INNER JOIN Planta ON Planta.Id=LInea.IdPlanta WHERE Trabajador.NoReloj=@NoReloj;
	END ELSE
		BEGIN
			SELECT 1 AS [Id], 'No existe trabajador con ese número de reloj' AS [Nombre];
		END
END


CREATE OR ALTER PROCEDURE spConsultarTrabajadoresEstadoCertificacion
@MostrarPor   VARCHAR(20),
@TextoBuscar  VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    ;WITH CTE_Estado AS
    (
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
            Li.Nombre AS NombreLinea,

            CASE 
                WHEN COUNT(C.Id) = 0 THEN 'Sin certificar'

                WHEN SUM(CASE 
                            WHEN C.FechaVencimiento < CAST(GETDATE() AS DATE) 
                            THEN 1 ELSE 0 
                         END) > 0 
                    THEN 'Vencida'

                WHEN SUM(CASE 
                            WHEN C.FechaVencimiento 
                                 BETWEEN CAST(GETDATE() AS DATE) 
                                 AND DATEADD(DAY, 30, GETDATE()) 
                            THEN 1 ELSE 0 
                         END) > 0 
                    THEN 'Por vencer'

                ELSE 'Vigente'
            END AS EstadoCertificacion

        FROM Trabajador T
        INNER JOIN Localidad L ON L.Id = T.IdLocalidad
        INNER JOIN Turno Tu ON Tu.Id = T.IdTurno
        INNER JOIN Linea Li ON Li.Id = T.IdLinea
        INNER JOIN Planta P ON P.Id = Li.IdPlanta
        LEFT JOIN Certificacion C ON C.IdTrabajador = T.Id

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
            T.Id, T.NoReloj, T.Nombre, T.RutaFoto,
            T.IdLocalidad, L.Nombre,
            T.IdTurno, Tu.Nombre,
            Li.IdPlanta, P.Nombre,
            T.IdLinea, Li.Nombre
    )

    SELECT *
    FROM CTE_Estado
    WHERE
        @MostrarPor = 'Todas'
        OR EstadoCertificacion = @MostrarPor
END





-------------------------------------------

-------------------------------------------
CREATE TABLE Certificador
(
	Id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	IdTrabajador INT NOT NULL FOREIGN KEY REFERENCES Trabajador(Id)
)
ALTER PROCEDURE spConsultarCertificadores
@TextoBuscar VARCHAR(100)
AS
BEGIN
	SELECT Certificador.Id, Certificador.IdTrabajador, Trabajador.NoReloj, Trabajador.Nombre AS [NombreTrabajador], Trabajador.RutaFoto,
	Trabajador.IdTurno, Turno.Nombre AS [NombreTurno], Planta.Id AS [IdPlanta], Planta.Nombre AS [NombrePlanta], Linea.Id AS [IdLinea], 
	Linea.Nombre AS [NombreLinea] FROM Certificador INNER JOIN Trabajador ON Certificador.IdTrabajador = Trabajador.Id
	INNER JOIN Turno ON Turno.Id = Trabajador.IdTurno INNER JOIN Linea ON Linea.Id = Trabajador.IdLinea INNER JOIN Planta ON Planta.Id = Linea.IdPlanta
	WHERE Trabajador.NoReloj LIKE '%'+@TextoBuscar+'%' OR Trabajador.Nombre LIKE '%'+@TextoBuscar+'%' OR Turno.Nombre LIKE '%'+@TextoBuscar+'%'
	OR Planta.Nombre LIKE '%'+@TextoBuscar+'%' OR Linea.Nombre LIKE '%'+@TextoBuscar+'%'
END

ALTER PROCEDURE spGuardarCertificador
@NoReloj VARCHAR(10)
AS
BEGIN
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

ALTER PROCEDURE spBorrarCertificador
@Id INT
AS
BEGIN
	DELETE FROM Certificador WHERE Id=@Id;
	SELECT '1' AS [Id], 'Se ha borrado correctamente' AS [Nombre];
END
-------------------------------------------

-------------------------------------------
CREATE TABLE Proceso
(
	Id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	Nombre VARCHAR(100) NOT NULL,
	Descripcion VARCHAR(300),
	VigenciaMeses INT NOT NULL
)

CREATE PROCEDURE spConsultarProcesos
@TextoBuscar VARCHAR(100)
AS
BEGIN
	SELECT * FROM Proceso WHERE Nombre LIKE '%'+@TextoBuscar+'%'
END

CREATE PROCEDURE spGuardarProceso
@Id INT,
@Nombre VARCHAR(100),
@Descripcion VARCHAR(300),
@VigenciaMeses INT
AS
BEGIN
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

CREATE PROCEDURE spBorrarProceso
@Id INT
AS
BEGIN
	DELETE FROM Proceso WHERE Id=@Id;
	SELECT 1 AS [Id], 'Se ha borrado correctamente' AS [Nombre];
END

-------------------------------------------

-------------------------------------------
CREATE TABLE Certificacion
(
	Id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	IdTrabajador INT NOT NULL FOREIGN KEY REFERENCES Trabajador(Id),
	IdProceso INT NOT NULL FOREIGN KEY REFERENCES Proceso(Id),
	FechaCertificacion DATE NOT NULL,
	FechaVencimiento DATE NOT NULL,
	IdCertificador INT NOT NULL FOREIGN KEY REFERENCES Certificador(Id),
	Comentario VARCHAR(300),
	    CONSTRAINT UQ_Certificacion_Trabajador_Proceso
        UNIQUE (IdTrabajador, IdProceso),

    --  Validación de fechas (opcional)
    CONSTRAINT CK_Fechas_Certificacion
        CHECK (FechaVencimiento > FechaCertificacion)
)

CREATE OR ALTER PROCEDURE spConsultarCertificacionesPorTrabajador
    @IdTrabajador INT,
    @TextoBuscar VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        C.Id,
		P.Id AS [IdProceso],
        P.Nombre AS Proceso,
        C.FechaCertificacion,
        C.FechaVencimiento,
        DATEDIFF(DAY, GETDATE(), C.FechaVencimiento) AS DiasRestantes,
        C.Comentario,
		C.IdCertificador, Trabajador.Nombre AS [NombreCertificador]
    FROM Certificacion C
    INNER JOIN Proceso P ON P.Id = C.IdProceso
	INNER JOIN Certificador ON Certificador.Id = C.IdCertificador
	INNER JOIN Trabajador ON Trabajador.Id = Certificador.IdTrabajador
    WHERE
        C.IdTrabajador = @IdTrabajador
        AND (
            P.Nombre LIKE '%' + @TextoBuscar + '%'
            OR C.Comentario LIKE '%' + @TextoBuscar + '%'
			OR Trabajador.Nombre LIKE '%' + @TextoBuscar + '%'
        )
    ORDER BY C.FechaVencimiento;
END


CREATE OR ALTER PROCEDURE spGuardarCertificacion
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

CREATE PROCEDURE spBorrarCertificacion
@Id INT
AS
BEGIN
	DELETE FROM Certificacion WHERE Id=@Id;
	SELECT 1 AS [Id], 'Certificación borrada correctamente' AS [Nombre];
END




-------------------------------------------


CREATE INDEX IX_Trabajador_IdLinea
ON Trabajador (IdLinea);

CREATE INDEX IX_Trabajador_IdTurno
ON Trabajador (IdTurno);

CREATE INDEX IX_Trabajador_IdLocalidad
ON Trabajador (IdLocalidad);


CREATE INDEX IX_Linea_IdPlanta
ON Linea (IdPlanta);

CREATE INDEX IX_Certificador_IdTrabajador
ON Certificador (IdTrabajador);

CREATE INDEX IX_Certificacion_IdTrabajador
ON Certificacion (IdTrabajador);

CREATE INDEX IX_Certificacion_IdProceso
ON Certificacion (IdProceso);

CREATE INDEX IX_Certificacion_FechaVencimiento
ON Certificacion (FechaVencimiento);
