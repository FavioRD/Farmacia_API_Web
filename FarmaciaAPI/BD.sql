CREATE DATABASE FarmaciaDB;
GO

USE FarmaciaDB;
GO

CREATE TABLE Productos (
    Id INT PRIMARY KEY IDENTITY,
    Nombre NVARCHAR(100),
    Descripcion NVARCHAR(250),
    Precio DECIMAL(10,2),
    Stock INT,
    FechaExpiracion DATE
);

CREATE TABLE Salidas (
    Id INT PRIMARY KEY IDENTITY,
    ProductoId INT FOREIGN KEY REFERENCES Productos(Id),
    Cantidad INT NOT NULL,
    FechaSalida DATETIME DEFAULT GETDATE()
);

-- Insertar --
CREATE PROCEDURE USP_InsertarProducto
    @Nombre NVARCHAR(100),
    @Descripcion NVARCHAR(250),
    @Precio DECIMAL(10,2),
    @Stock INT,
    @FechaExpiracion DATE
AS
BEGIN
    INSERT INTO Productos (Nombre, Descripcion, Precio, Stock, FechaExpiracion)
    VALUES (@Nombre, @Descripcion, @Precio, @Stock, @FechaExpiracion);
END


-- Listar --
CREATE PROCEDURE USP_ListarProductos
AS
BEGIN
    SELECT * FROM Productos;
END

--
CREATE PROCEDURE USP_ObtenerProductoPorId
    @Id INT
AS
BEGIN
    SELECT * FROM Productos WHERE Id = @Id;
END


-- Actualizar --
CREATE PROCEDURE USP_ActualizarProducto
    @Id INT,
    @Nombre NVARCHAR(100),
    @Descripcion NVARCHAR(250),
    @Precio DECIMAL(10,2),
    @Stock INT,
    @FechaExpiracion DATE
AS
BEGIN
    UPDATE Productos
    SET Nombre = @Nombre,
        Descripcion = @Descripcion,
        Precio = @Precio,
        Stock = @Stock,
        FechaExpiracion = @FechaExpiracion
    WHERE Id = @Id;
END


-- Eliminar --
CREATE PROCEDURE USP_EliminarProducto
    @Id INT
AS
BEGIN
    DELETE FROM Productos WHERE Id = @Id;
END

---- datos ----
EXEC USP_InsertarProducto 'Paracetamol 500mg', 'Analgésico y antipirético', 2.50, 150, '2026-01-15';
EXEC USP_InsertarProducto 'Ibuprofeno 400mg', 'Antiinflamatorio no esteroideo', 3.00, 200, '2026-03-10';
EXEC USP_InsertarProducto 'Amoxicilina 500mg', 'Antibiótico de amplio espectro', 4.50, 120, '2025-12-01';
EXEC USP_InsertarProducto 'Omeprazol 20mg', 'Inhibidor de la bomba de protones', 2.80, 180, '2026-05-30';
EXEC USP_InsertarProducto 'Loratadina 10mg', 'Antihistamínico para alergias', 1.90, 300, '2026-02-28';
EXEC USP_InsertarProducto 'Salbutamol Inhalador', 'Broncodilatador para asma', 5.75, 80, '2025-11-15';
EXEC USP_InsertarProducto 'Metformina 850mg', 'Antidiabético oral', 3.20, 90, '2026-04-10';
EXEC USP_InsertarProducto 'Ácido Fólico 5mg', 'Suplemento vitamínico', 1.25, 250, '2027-01-01';
EXEC USP_InsertarProducto 'Diclofenaco 50mg', 'Analgésico y antiinflamatorio', 2.40, 160, '2025-10-20';
EXEC USP_InsertarProducto 'Ranitidina 150mg', 'Reductor de ácido estomacal', 2.10, 70, '2025-09-01';

----SALIDAS-----

--registrar
CREATE PROCEDURE USP_RegistrarSalida
    @ProductoId INT,
    @Cantidad INT
AS
BEGIN
    -- Verificar stock disponible
    DECLARE @StockActual INT;
    SELECT @StockActual = Stock FROM Productos WHERE Id = @ProductoId;

    IF @StockActual < @Cantidad
    BEGIN
        RAISERROR('Stock insuficiente para la salida.', 16, 1);
        RETURN;
    END

    -- Insertar en tabla Salidas
    INSERT INTO Salidas (ProductoId, Cantidad)
    VALUES (@ProductoId, @Cantidad);

    -- Actualizar stock
    UPDATE Productos
    SET Stock = Stock - @Cantidad
    WHERE Id = @ProductoId;
END


----reporte
CREATE PROCEDURE USP_SalidasDiarias
AS
BEGIN
    SELECT 
        P.Nombre,
        SUM(S.Cantidad) AS TotalSalidas,
        CONVERT(DATE, S.FechaSalida) AS Fecha
    FROM Salidas S
    INNER JOIN Productos P ON P.Id = S.ProductoId
    WHERE CONVERT(DATE, S.FechaSalida) = CONVERT(DATE, GETDATE())
    GROUP BY P.Nombre, CONVERT(DATE, S.FechaSalida);
END


--- reporte por fechas
CREATE PROCEDURE USP_SalidasPorRango
    @FechaInicio DATE,
    @FechaFin DATE
AS
BEGIN
    SELECT 
        P.Nombre,
        SUM(S.Cantidad) AS TotalSalidas,
        CONVERT(DATE, S.FechaSalida) AS Fecha
    FROM Salidas S
    INNER JOIN Productos P ON P.Id = S.ProductoId
    WHERE S.FechaSalida BETWEEN @FechaInicio AND @FechaFin
    GROUP BY P.Nombre, CONVERT(DATE, S.FechaSalida);
END
