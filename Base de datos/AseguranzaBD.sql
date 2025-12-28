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
CREATE PROCEDURE spBorrarLocalidad
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
        Planta.Nombre AS [Planta]
    FROM Linea
    INNER JOIN Planta ON Linea.IdPlanta = Planta.Id
	WHERE
        @TextoBuscar IS NULL
        OR @TextoBuscar = ''
        OR Linea.Nombre LIKE '%' + @TextoBuscar + '%'
        OR Planta.Nombre LIKE '%' + @TextoBuscar + '%';
END
CREATE PROCEDURE spConsultarLineasPorPlanta
@IdPlanta INT
AS
BEGIN
	SELECT Id, Nombre FROM Linea WHERE IdPlanta=@IdPlanta
END
ALTER PROCEDURE spGuardarLinea
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
	SELECT Trabajador.Id, Trabajador.NoReloj, Trabajador.Nombre, Trabajador.RutaFoto, Trabajador.IdLocalidad, Trabajador.IdTurno, Linea.IdPlanta, Trabajador.IdLinea FROM Trabajador
	INNER JOIN Linea ON Trabajador.IdLinea = Linea.Id  WHERE Trabajador.Nombre LIKE '%'+@TextoBuscar+'%';
END
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
-------------------------------------------

-------------------------------------------
CREATE TABLE Certificador
(
	Id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	IdTrabajador INT NOT NULL FOREIGN KEY REFERENCES Trabajador(Id)
)
ALTER PROCEDURE spConsultarCertificadores
AS
BEGIN
	SELECT Certificador.Id, Certificador.IdTrabajador, Trabajador.NoReloj, Trabajador.Nombre, Trabajador.RutaFoto,
	Trabajador.IdTurno, Turno.Nombre AS [NombreTurno], Planta.Id AS [IdPlanta], Planta.Nombre AS [NombrePlanta] FROM Certificador INNER JOIN Trabajador ON Certificador.IdTrabajador = Trabajador.Id
	INNER JOIN Turno ON Turno.Id = Trabajador.IdTurno INNER JOIN Linea ON Linea.Id = Trabajador.IdLinea INNER JOIN Planta ON Planta.Id = Linea.IdPlanta
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
-------------------------------------------

-------------------------------------------
CREATE TABLE Proceso
(
	Id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	Nombre VARCHAR(100) NOT NULL,
	Descripcion VARCHAR(300),
	VigenciaMeses INT NOT NULL
)
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
