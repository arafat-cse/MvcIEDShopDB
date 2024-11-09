CREATE DATABASE ShopDB;
GO
USE ShopDB;
GO
CREATE TABLE Users
(
UserId int primary key identity(1,1),
UserName varchar(50),
Age varchar(50),
UserDate Date,
IsAudil bit,
ImagePath nvarchar(50)
);
GO

CREATE TABLE Product
(
ProductId int primary key identity(1,1),
ProductName varchar(50)
);
GO

INSERT INTO Product
VALUES
('Sofa'),
('Computer')
GO

CREATE TABLE Details
(
DetailsId int primary key identity(1,1),
ProductName varchar(50),
UserId INT REFERENCES Users(UserId),
ProductId INT REFERENCES Product(ProductId)
);
GO