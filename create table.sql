CREATE TABLE Category (
    Id INTEGER PRIMARY KEY NOT NULL identity,
    Name VARCHAR(250) NOT NULL,
    IsActive bit NOT NULL,
    UpdateDate DATETIME NOT NULL,
    CategoryCode VARCHAR(250) NOT NULL
)


go


CREATE TABLE Product (
    Id INTEGER PRIMARY KEY NOT NULL identity,
	CategoryId INTEGER NOT NULL,
    ProductCode VARCHAR(100) NOT NULL,
    Name VARCHAR(250) NOT NULL,
    Description VARCHAR(250) NULL,
    CategoryName VARCHAR(250) NOT NULL,
    Price DECIMAL(10,2) NOT NULL,
    Image VARCHAR(250) NULL,
    UpdateDate DATETIME NOT NULL,
	FOREIGN KEY (CategoryId) REFERENCES Category(Id)
)

go

CREATE TABLE Audit (
    Id INT PRIMARY KEY NOT NULL IDENTITY,
    TableName VARCHAR(100) NOT NULL,
    Action VARCHAR(10), -- Insert, Update, Delete
    RecordId INT, -- The ID of the changed record
    OldValue VARCHAR(MAX) NULL, -- Old values before the change
    NewValue VARCHAR(MAX) NULL, -- New values after the change
    AuditDate DATETIME
)