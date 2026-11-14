IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'QuanLyNguoiDung')
BEGIN
    CREATE DATABASE QuanLyNguoiDung;
END
GO

USE QuanLyNguoiDung;
GO

IF OBJECT_ID('NguoiDung', 'U') IS NOT NULL
    DROP TABLE NguoiDung;
GO

CREATE TABLE NguoiDung (
    UserId       INT IDENTITY(1,1) PRIMARY KEY,
    Username     NVARCHAR(50)  NOT NULL UNIQUE,
    PasswordHash NVARCHAR(128) NOT NULL,
    PasswordSalt NVARCHAR(64)  NOT NULL,
    Email        NVARCHAR(100) NOT NULL,
    FullName     NVARCHAR(100) NOT NULL,
    Birthday     DATE          NOT NULL
);
GO
