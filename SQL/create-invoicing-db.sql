-- Create Invoicing Database
-- Run this script in SQL Server Management Studio or Azure Data Studio

-- Create database
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'InvoicingDb')
BEGIN
    CREATE DATABASE InvoicingDb;
END
GO

USE InvoicingDb;
GO

-- Create Invoice table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Invoice')
BEGIN
    CREATE TABLE Invoice (
        InvoiceId INT IDENTITY(1,1) PRIMARY KEY,
        InvoiceNumber NVARCHAR(50) NOT NULL UNIQUE,
        OrderNumber NVARCHAR(50) NOT NULL,
        OrderDate DATETIME2 NOT NULL,
        InvoiceDate DATETIME2 NOT NULL,
        SentDate DATETIME2 NOT NULL,
        ClientEmail NVARCHAR(100) NOT NULL,
        ShippingAddress NVARCHAR(200) NOT NULL,
        TotalAmount DECIMAL(18,2) NOT NULL
    );
END
GO

-- Create InvoiceItem table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'InvoiceItem')
BEGIN
    CREATE TABLE InvoiceItem (
        InvoiceItemId INT IDENTITY(1,1) PRIMARY KEY,
        InvoiceId INT NOT NULL,
        ProductCode NVARCHAR(20) NOT NULL,
        Quantity INT NOT NULL,
        UnitPrice DECIMAL(18,2) NOT NULL,
        TotalPrice DECIMAL(18,2) NOT NULL,
        CONSTRAINT FK_InvoiceItem_Invoice FOREIGN KEY (InvoiceId) REFERENCES Invoice(InvoiceId)
    );
END
GO

PRINT 'Invoicing database created successfully!';
