    ------ GET ALL SERVICE
 CREATE PROC sp_GetService
 AS
 BEGIN
	SELECT a.Id, a.Code, a.Name, a.Note, a.CreateAt 
	FROM Services A 
 END
 
 GO
   ------ GET SERVICE BY ID
 CREATE PROC sp_InsertService
  @ID UNIQUEIDENTIFIER, 
  @Code NVARCHAR(MAX), 
  @Name NVARCHAR(MAX), 
  @Note NVARCHAR(MAX), 
  @CreateAt DATETIME  
 AS
 BEGIN
	IF NOT EXISTS (SELECT * FROM Services s  WHERE s.Id = @ID ) 
	BEGIN 
		INSERT INTO Services 
		(
			Id,
			Code,
			Name,
			Note,
			CreateAt
		) VALUES
		(
			@Id,
			@Code,
			@Name,
			@Note,
			@CreateAt
		) ;
		 
		RETURN 1
	END 
	ELSE
	BEGIN
		RETURN 0
	END
 END
  
   GO
------ UPDATE SERVICE  
 CREATE PROC sp_UpdateService
  @ID UNIQUEIDENTIFIER, 
  @Code NVARCHAR(MAX), 
  @Name NVARCHAR(MAX), 
  @Note NVARCHAR(MAX), 
  @CreateAt DATETIME  
 AS
 BEGIN
	IF EXISTS (SELECT * FROM Services S WHERE S.Id = @ID  ) 
	BEGIN 
		UPDATE Services set 
			Code = @Code,
			Name = @Name ,
			Note = @Note,
			CreateAt = @CreateAt 
        WHERE Id = @ID
		RETURN 1
	END 
	ELSE
	BEGIN
		RETURN 0
	END
 END
 
 GO
 ------ DELETE SERVICE BY ID
 CREATE PROC sp_DeleteServiceById
  @ID UNIQUEIDENTIFIER 
 AS
 BEGIN
	IF EXISTS (SELECT * FROM Services S  WHERE S.Id = @ID  ) 
	BEGIN 
		
		IF EXISTS (SELECT * FROM WorkerOfServices ws WHERE ws.ServiceId = @ID ) 
		BEGIN
			DELETE WorkerOfServices WHERE WorkerOfServices.ServiceId  = @ID
		END
		IF EXISTS (SELECT * FROM PreferentialOfServices p WHERE p.ServiceId = @ID ) 
		BEGIN
			DELETE PreferentialOfServices  WHERE PreferentialOfServices.ServiceId = @ID 
		END  
        DELETE Services WHERE Services.Id = @ID 
		RETURN 1
	END 
	ELSE
	BEGIN
		RETURN 0
	END
 END