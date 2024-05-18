CREATE DATABASE RegisterLoginDB;
GO

USE RegisterLoginDB;
GO

CREATE TABLE Roles (
    RoleID INT PRIMARY KEY IDENTITY(1,1),
    RoleName NVARCHAR(50) NOT NULL
);
GO

-- Thêm một số vai trò mặc định
INSERT INTO Roles (RoleName) VALUES ('Admin'), ('Publisher'), ('Reviewer'), ('Moderator');
GO
SELECT * FROM Roles


CREATE TABLE Users (
    UserID INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(255) NOT NULL,
    RoleID INT FOREIGN KEY REFERENCES Roles(RoleID),
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
