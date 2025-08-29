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



/*PROCEDIMEINTOS PRODUCTOS*/
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

CREATE TABLE Salidas (
    Id INT PRIMARY KEY IDENTITY,
    ProductoId INT FOREIGN KEY REFERENCES Productos(Id),
    Cantidad INT NOT NULL,
    FechaSalida DATETIME DEFAULT GETDATE()
);

-----/*PROCEDIMIENTOS SALIDAS*/------

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


-- Listar todas las salidas
CREATE PROCEDURE USP_ListarSalidas
AS
BEGIN
    SELECT 
        S.Id,
        S.ProductoId,
        S.Cantidad,
        S.FechaSalida
    FROM Salidas S
    ORDER BY S.FechaSalida DESC;
END
GO

-- Obtener salida por Id
CREATE PROCEDURE USP_ObtenerSalidaPorId
    @Id INT
AS
BEGIN
    SELECT 
        S.Id,
        S.ProductoId,
        S.Cantidad,
        S.FechaSalida
    FROM Salidas S
    WHERE S.Id = @Id;
END
GO

-- Actualizar salida
CREATE PROCEDURE USP_ActualizarSalida
    @Id INT,
    @ProductoId INT,
    @Cantidad INT,
    @FechaSalida DATETIME
AS
BEGIN
    UPDATE Salidas
    SET ProductoId = @ProductoId,
        Cantidad = @Cantidad,
        FechaSalida = @FechaSalida
    WHERE Id = @Id;
END
GO

-- Eliminar salida
CREATE PROCEDURE USP_EliminarSalida
    @Id INT
AS
BEGIN
    DELETE FROM Salidas
    WHERE Id = @Id;
END
GO


CREATE TABLE Ingresos (
    Id INT PRIMARY KEY IDENTITY,
    ProductoId INT FOREIGN KEY REFERENCES Productos(Id),
    Cantidad INT NOT NULL,
    FechaIngreso DATETIME DEFAULT GETDATE()
);
GO

CREATE PROCEDURE USP_RegistrarIngreso
    @ProductoId INT,
    @Cantidad INT
AS
BEGIN
    -- Insertar en tabla Ingresos
    INSERT INTO Ingresos (ProductoId, Cantidad)
    VALUES (@ProductoId, @Cantidad);

    -- Actualizar stock (sumar)
    UPDATE Productos
    SET Stock = Stock + @Cantidad
    WHERE Id = @ProductoId;
END
GO


CREATE PROCEDURE USP_ListarIngresos
AS
BEGIN
    SELECT 
        I.Id,
        I.ProductoId,
        P.Nombre AS Producto,
        I.Cantidad,
        I.FechaIngreso
    FROM Ingresos I
    INNER JOIN Productos P ON P.Id = I.ProductoId
    ORDER BY I.FechaIngreso DESC;
END
GO

CREATE PROCEDURE USP_ObtenerIngresoPorId
    @Id INT
AS
BEGIN
    SELECT 
        I.Id,
        I.ProductoId,
        P.Nombre AS Producto,
        I.Cantidad,
        I.FechaIngreso
    FROM Ingresos I
    INNER JOIN Productos P ON P.Id = I.ProductoId
    WHERE I.Id = @Id;
END
GO

CREATE PROCEDURE USP_ActualizarIngreso
    @Id INT,
    @ProductoId INT,
    @Cantidad INT,
    @FechaIngreso DATETIME
AS
BEGIN
    DECLARE @CantidadAnterior INT;

    -- Obtener cantidad anterior
    SELECT @CantidadAnterior = Cantidad FROM Ingresos WHERE Id = @Id;

    -- Ajustar stock (restar lo anterior, sumar lo nuevo)
    UPDATE Productos
    SET Stock = Stock - @CantidadAnterior + @Cantidad
    WHERE Id = @ProductoId;

    -- Actualizar ingreso
    UPDATE Ingresos
    SET ProductoId = @ProductoId,
        Cantidad = @Cantidad,
        FechaIngreso = @FechaIngreso
    WHERE Id = @Id;
END
GO

CREATE PROCEDURE USP_EliminarIngreso
    @Id INT
AS
BEGIN
    DECLARE @ProductoId INT, @Cantidad INT;

    -- Obtener datos del ingreso
    SELECT @ProductoId = ProductoId, @Cantidad = Cantidad
    FROM Ingresos
    WHERE Id = @Id;

    -- Restar del stock
    UPDATE Productos
    SET Stock = Stock - @Cantidad
    WHERE Id = @ProductoId;

    -- Eliminar ingreso
    DELETE FROM Ingresos
    WHERE Id = @Id;
END
GO

