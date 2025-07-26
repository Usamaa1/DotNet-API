-- Scaffold-DbContext "Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Cart;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False" Microsoft.EntityFrameworkCore.SqlServer -Tables Cart,Orders,OrderItems,Users -OutputDir Models -ContextDir Data -Context AppDbContext -Force

CREATE TABLE Users (
    Id INT PRIMARY KEY IDENTITY,
    Name NVARCHAR(100),
    Email NVARCHAR(100) UNIQUE,
	Password NVARCHAR(100)
);


CREATE TABLE Cart (
    Id INT PRIMARY KEY IDENTITY,
    UserId INT,
    ProductId INT,
    Quantity INT,
    FOREIGN KEY (UserId) REFERENCES Users(Id),
    FOREIGN KEY (ProductId) REFERENCES Products(Id)
);

CREATE TABLE Orders (
    Id INT PRIMARY KEY IDENTITY,
    UserId INT,
    OrderDate DATETIME DEFAULT GETDATE()
);

CREATE TABLE OrderItems (
    Id INT PRIMARY KEY IDENTITY,
    OrderId INT,
    ProductId INT,
    Quantity INT,
    FOREIGN KEY (OrderId) REFERENCES Orders(Id),
    FOREIGN KEY (ProductId) REFERENCES Products(Id)
);
