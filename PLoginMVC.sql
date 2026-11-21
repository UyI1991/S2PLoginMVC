--1. Crear la base de datos
CREATE DATABASE DB_LOGIN;
GO

--2. Usar la base de datos
USE DB_LOGIN;
GO

--3. Crear la tabla Productos (4 campos básicos)
CREATE TABLE Usuarios (
    IdUsuario INT IDENTITY(1,1) PRIMARY KEY,
    Nickname VARCHAR(50) NOT NULL,
    Contrasena VARCHAR(255) NOT NULL,
);
GO

-- 4. Insertar datos de prueba
INSERT INTO Usuarios (Nickname, Contrasena)
VALUES 
    ('alorenzo', 12345678),
GO

--5. Comprobar los datos
SELECT * FROM dbo.Usuarios;
GO