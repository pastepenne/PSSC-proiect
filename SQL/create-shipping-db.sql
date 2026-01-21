-- Create Shipping Database
-- Run this script in SQL Server Management Studio or Azure Data Studio

-- Create database
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'ShippingDb')
BEGIN
    CREATE DATABASE ShippingDb;
END
GO

USE ShippingDb;
GO

-- Create Shipment table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Shipment')
BEGIN
    CREATE TABLE Shipment (
        ShipmentId INT IDENTITY(1,1) PRIMARY KEY,
        TrackingNumber NVARCHAR(50) NOT NULL UNIQUE,
        OrderNumber NVARCHAR(50) NOT NULL,
        InvoiceNumber NVARCHAR(50) NOT NULL,
        PreparedDate DATETIME2 NOT NULL,
        ShippedDate DATETIME2 NOT NULL,
        CourierName NVARCHAR(50) NOT NULL,
        ClientEmail NVARCHAR(100) NOT NULL,
        ShippingAddress NVARCHAR(200) NOT NULL,
        TotalAmount DECIMAL(18,2) NOT NULL
    );
END
GO

-- Create ShipmentItem table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'ShipmentItem')
BEGIN
    CREATE TABLE ShipmentItem (
        ShipmentItemId INT IDENTITY(1,1) PRIMARY KEY,
        ShipmentId INT NOT NULL,
        ProductCode NVARCHAR(20) NOT NULL,
        Quantity INT NOT NULL,
        UnitPrice DECIMAL(18,2) NOT NULL,
        TotalPrice DECIMAL(18,2) NOT NULL,
        CONSTRAINT FK_ShipmentItem_Shipment FOREIGN KEY (ShipmentId) REFERENCES Shipment(ShipmentId)
    );
END
GO

PRINT 'Shipping database created successfully!';
