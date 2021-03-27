--- Get All Customer
CREATE PROCEDURE [dbo].[sp_GetCustomer]
AS
BEGIN
    SELECT * 
    FROM  Users A, Customers B 
    WHERE A.Id = B.Userid
    
END
-- 
GO
--- Insert Customer
Create PROCEDURE  [dbo].[sp_InsertCustomer]
    @ID uniqueidentifier,
    @name nvarchar(250),
    @address nvarchar(250),
    @birthday datetime2,
    @CompanyName nvarchar(500)
AS
BEGIN
    IF EXISTS (SELECT *
    FROM Users
    WHERE  Users.id  = @ID)
                            BEGIN
        RAISERROR ('USER này dã tồn tại', 16,1)
        RETURN
    END
    INSERT INTO Users
        ([id],[name], [address] , [Birthday])
    VALUES
        (@ID, @name, @address, @birthday)
    INSERT INTO Customers
        (id,CompanyName, Userid )
    VALUES
        (@ID, @CompanyName, @ID)

END 
            