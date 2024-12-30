-- Create Database
CREATE DATABASE ReservationManagementSystem
GO

USE ReservationManagementSystem
GO

-- Create Tables
CREATE TABLE Salas (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Capacidad INT NOT NULL,
    Disponibilidad BIT DEFAULT 1
)

CREATE TABLE Reservas (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    SalaID INT NOT NULL,
    FechaReserva DATETIME NOT NULL,
    Usuario NVARCHAR(100) NOT NULL,
    CONSTRAINT FK_Reservas_Salas FOREIGN KEY (SalaID) REFERENCES Salas(ID),
    CONSTRAINT UQ_Reserva_Sala_Fecha UNIQUE (SalaID, FechaReserva)
)

-- Stored Procedures for Salas
CREATE PROCEDURE sp_InsertarSala
    @Nombre NVARCHAR(100),
    @Capacidad INT,
    @Disponibilidad BIT
AS
BEGIN
    INSERT INTO Salas (Nombre, Capacidad, Disponibilidad)
    VALUES (@Nombre, @Capacidad, @Disponibilidad)
    
    SELECT SCOPE_IDENTITY() AS ID
END
GO

CREATE PROCEDURE sp_ActualizarSala
    @ID INT,
    @Nombre NVARCHAR(100),
    @Capacidad INT,
    @Disponibilidad BIT
AS
BEGIN
    UPDATE Salas
    SET Nombre = @Nombre,
        Capacidad = @Capacidad,
        Disponibilidad = @Disponibilidad
    WHERE ID = @ID
END
GO

CREATE PROCEDURE sp_EliminarSala
    @ID INT
AS
BEGIN
    IF NOT EXISTS (SELECT 1 FROM Reservas WHERE SalaID = @ID)
    BEGIN
        DELETE FROM Salas WHERE ID = @ID
        RETURN 0
    END
    RETURN 1
END
GO

CREATE PROCEDURE sp_ObtenerSalas
AS
BEGIN
    SELECT ID, Nombre, Capacidad, Disponibilidad
    FROM Salas
    ORDER BY Nombre
END
GO

-- Stored Procedures for Reservas
CREATE PROCEDURE sp_ValidarDisponibilidadSala
    @SalaID INT,
    @FechaReserva DATETIME
AS
BEGIN
    IF EXISTS (
        SELECT 1 
        FROM Reservas 
        WHERE SalaID = @SalaID 
        AND CAST(FechaReserva AS DATE) = CAST(@FechaReserva AS DATE)
    )
        RETURN 1
    RETURN 0
END
GO

CREATE PROCEDURE sp_InsertarReserva
    @SalaID INT,
    @FechaReserva DATETIME,
    @Usuario NVARCHAR(100)
AS
BEGIN
    BEGIN TRY
        INSERT INTO Reservas (SalaID, FechaReserva, Usuario)
        VALUES (@SalaID, @FechaReserva, @Usuario)
        
        SELECT SCOPE_IDENTITY() AS ID
    END TRY
    BEGIN CATCH
        RETURN -1
    END CATCH
END
GO

CREATE PROCEDURE sp_ActualizarReserva
    @ID INT,
    @SalaID INT,
    @FechaReserva DATETIME,
    @Usuario NVARCHAR(100)
AS
BEGIN
    BEGIN TRY
        UPDATE Reservas
        SET SalaID = @SalaID,
            FechaReserva = @FechaReserva,
            Usuario = @Usuario
        WHERE ID = @ID
        RETURN 0
    END TRY
    BEGIN CATCH
        RETURN 1
    END CATCH
END
GO

CREATE PROCEDURE sp_EliminarReserva
    @ID INT
AS
BEGIN
    DELETE FROM Reservas WHERE ID = @ID
END
GO

CREATE PROCEDURE sp_ConsultarReservas
    @FechaInicio DATETIME = NULL,
    @FechaFin DATETIME = NULL,
    @SalaID INT = NULL
AS
BEGIN
    SELECT 
        R.ID,
        R.FechaReserva,
        R.Usuario,
        S.ID AS SalaID,
        S.Nombre,
        S.Capacidad
    FROM Reservas R
    INNER JOIN Salas S ON R.SalaID = S.ID
    WHERE (@FechaInicio IS NULL OR R.FechaReserva >= @FechaInicio)
    AND (@FechaFin IS NULL OR R.FechaReserva <= @FechaFin)
    AND (@SalaID IS NULL OR R.SalaID = @SalaID)
    ORDER BY R.FechaReserva DESC
END
GO
