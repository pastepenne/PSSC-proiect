-- Create Orders Database
-- Run this script in SQL Server Management Studio or Azure Data Studio

-- Create database
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'OrdersDb')
BEGIN
    CREATE DATABASE OrdersDb;
END
GO

USE OrdersDb;
GO

-- Create Product table (prepopulated - catalog of available products)
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Product')
BEGIN
    CREATE TABLE Product (
        ProductId INT IDENTITY(1,1) PRIMARY KEY,
        Code NVARCHAR(20) NOT NULL UNIQUE,
        Name NVARCHAR(100) NOT NULL,
        Price DECIMAL(18,2) NOT NULL,
        Stock INT NOT NULL DEFAULT 0
    );

    -- Insert seed data (prepopulated products)
    INSERT INTO Product (Code, Name, Price, Stock) VALUES
        ('PRD-00001', 'Laptop Gaming', 4500.00, 10),
        ('PRD-00002', 'Mouse Wireless', 150.00, 50),
        ('PRD-00003', 'Tastatura Mecanica', 350.00, 30),
        ('PRD-00004', 'Monitor 27 inch', 1200.00, 15),
        ('PRD-00005', 'Casti Gaming', 450.00, 25);
END
GO

-- Create Order table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Order')
BEGIN
    CREATE TABLE [Order] (
        OrderId INT IDENTITY(1,1) PRIMARY KEY,
        OrderNumber NVARCHAR(50) NOT NULL UNIQUE,
        ClientEmail NVARCHAR(100) NOT NULL,
        ShippingAddress NVARCHAR(200) NOT NULL,
        TotalPrice DECIMAL(18,2) NOT NULL,
        PlacedDate DATETIME2 NOT NULL
    );
END
GO

-- Create OrderItem table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'OrderItem')
BEGIN
    CREATE TABLE OrderItem (
        OrderItemId INT IDENTITY(1,1) PRIMARY KEY,
        OrderId INT NOT NULL,
        ProductCode NVARCHAR(20) NOT NULL,
        Quantity INT NOT NULL,
        UnitPrice DECIMAL(18,2) NOT NULL,
        TotalPrice DECIMAL(18,2) NOT NULL,
        CONSTRAINT FK_OrderItem_Order FOREIGN KEY (OrderId) REFERENCES [Order](OrderId)
    );
END
GO

PRINT 'Orders database created successfully!';

-- Inserează produsele DOAR dacă tabela e goală
IF NOT EXISTS (SELECT 1 FROM Product)
BEGIN
    INSERT INTO Product (Code, Name, Price, Stock) VALUES
        ('PRD-00001', 'Laptop Gaming', 4500.00, 10),
        ('PRD-00002', 'Mouse Wireless', 150.00, 50),
        ('PRD-00003', 'Tastatura Mecanica', 350.00, 30),
        ('PRD-00004', 'Monitor 27 inch', 1200.00, 15),
        ('PRD-00005', 'Casti Gaming', 450.00, 25);
    
    PRINT 'Products were added successfully!';
END
ELSE
BEGIN
    PRINT 'Products already exist in the database..';
END
GO

-- Verifică produsele inserate
SELECT * FROM Product;
GO