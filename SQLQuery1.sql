CREATE DATABASE RegisterLoginDB;
GO

USE RegisterLoginDB;
GO


CREATE TABLE Users (
    UserID INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(255) NOT NULL,
	UpdatedAt DATETIME DEFAULT GETDATE(),
	CreatedAt DATETIME DEFAULT GETDATE()
);
GO


-- Trigger để cập nhật UpdatedAt
-- Trigger để tự động cập nhật cột UpdatedAt mỗi khi có sự thay đổi.
CREATE TRIGGER trgUsersUpdatedAt
ON Users
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Users
    SET UpdatedAt = GETDATE()
    FROM Users u
    INNER JOIN inserted i ON u.UserID = i.UserID;
END;
GO
