IF DB_ID('CustomerDB') IS NULL CREATE DATABASE [CustomerDB];
GO

USE [CustomerDB];
GO

IF OBJECT_ID('Customers') IS NULL
CREATE TABLE Customers (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    Name NVARCHAR(255) NOT NULL,
    TaxCode NVARCHAR(50),
    Address NVARCHAR(255),
    Phone NVARCHAR(20),
    Phone2 NVARCHAR(20),
    Phone3 NVARCHAR(20),
    Email NVARCHAR(100),
    Nationality NVARCHAR(100),
    Province NVARCHAR(100),
    District NVARCHAR(100),
    Gender INT NOT NULL,
    DateOfBirth DATETIME,
    BankAccount NVARCHAR(50),
    BankName NVARCHAR(100),
    CustomerType NVARCHAR(50),
    PearlCustomerCode NVARCHAR(50),
    CreatedAt DATETIME NOT NULL,
    UpdatedAt DATETIME NOT NULL,
    CreatedClientSourceCode NVARCHAR(50) NOT NULL,
    UpdatedClientSourceCode NVARCHAR(50) NOT NULL,
    HashCode NVARCHAR(255) NOT NULL
);
GO

IF OBJECT_ID('CustomerHistories') IS NULL
CREATE TABLE CustomerHistories (
    Id NVARCHAR(50) NOT NULL,
    Name NVARCHAR(255) NOT NULL,
    TaxCode NVARCHAR(50),
    Address NVARCHAR(255),
    Phone NVARCHAR(20),
    Phone2 NVARCHAR(20),
    Phone3 NVARCHAR(20),
    Email NVARCHAR(100),
    Nationality NVARCHAR(100),
    Province NVARCHAR(100),
    District NVARCHAR(100),
    Gender INT NOT NULL,
    DateOfBirth DATETIME,
    BankAccount NVARCHAR(50),
    BankName NVARCHAR(100),
    CustomerType NVARCHAR(50),
    PearlCustomerCode NVARCHAR(50),
    CreatedAt DATETIME NOT NULL,
    UpdatedAt DATETIME NOT NULL,
    CreatedClientSourceCode NVARCHAR(50) NOT NULL,
    UpdatedClientSourceCode NVARCHAR(50) NOT NULL,
    HashCode NVARCHAR(255) NOT NULL,
    ActionTime DATETIME NOT NULL,
    AddedByAction NVARCHAR(100) NOT NULL,
    ChangedByClient NVARCHAR(100) NOT NULL
);
GO

IF OBJECT_ID('ClientSources') IS NULL
CREATE TABLE ClientSources (
    ClientCode NVARCHAR(50) PRIMARY KEY,
    ClientName NVARCHAR(100) NOT NULL,
    Description NVARCHAR(255),
    IsActive BIT NOT NULL DEFAULT 1
);

GO
IF OBJECT_ID('ClientCredentials') IS NULL
CREATE TABLE ClientCredentials (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    ClientCode NVARCHAR(50) NOT NULL,
    SecretHash NVARCHAR(255) NOT NULL,
    CreatedAt DATETIME NOT NULL,
    IsRevoked BIT NOT NULL
);

GO
IF OBJECT_ID('InvalidTokens') IS NULL
CREATE TABLE [InvalidTokens](
	[jti] [nvarchar](512) NOT NULL,
	[createdAt] [datetime] NOT NULL,
	[note] [nvarchar](512) NULL
);
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'idx_date_id' AND object_id = OBJECT_ID('Customers'))
    CREATE INDEX idx_date_id ON Customers(CreatedAt, Id);
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'idx_phone' AND object_id = OBJECT_ID('Customers'))
    CREATE INDEX idx_phone ON Customers(Phone);
GO


