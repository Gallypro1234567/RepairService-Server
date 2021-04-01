 ------ GET ALL WORKER
 CREATE PROC sp_GetWorkers
 AS
 BEGIN
	SELECT 
	w.Id,
	u.Fullname,
	u.Email,
	u.Birthday,
	u.Phone,
	u.ImageUrl,
	u.isOnline,
	u.RewardPoints  
	FROM Workers w, Users u
	WHERE w.UserId = u.Id
 END
   
 GO
 ------ GET WORKER BY ID
 CREATE PROC sp_GetWorkerById
  @ID UNIQUEIDENTIFIER 
 AS
 BEGIN
	IF EXISTS (SELECT * FROM Workers w, Users u WHERE w.Id = @ID AND u.Id = @ID ) 
	BEGIN
		SELECT 
		w.Id,
		u.Fullname,
		u.Email,
		u.Birthday,
		u.Phone,
		u.ImageUrl,
		u.isOnline,
		u.RewardPoints 
		FROM Workers w, Users u
		WHERE w.UserId = u.Id
		AND  w.Id = @ID
		AND u.Id = @ID
	END 
	ELSE
	BEGIN
		RETURN 0
	END
 END
  
  GO
------ INSERT WORKER  
 CREATE PROC sp_InsertWorker
  @ID UNIQUEIDENTIFIER, 
  @Phone NVARCHAR(MAX),
  @Password NVARCHAR(MAX),
  @Fullname NVARCHAR(MAX),
  @Email NVARCHAR(MAX),
  @BirthDay NVARCHAR(MAX),
  @ImageURL NVARCHAR(MAX),
  @IsOnline bit,
  @RewardPoints INT
 AS
 BEGIN
	IF NOT EXISTS (SELECT * FROM Workers w, Users u WHERE w.Id = @ID AND u.Id = @ID ) 
	BEGIN 
		INSERT INTO Users 
		(
			Id,
			Phone,
			Password,
			Fullname,
			Email,
			Birthday,
			ImageUrl,  
			isOnline,
			RewardPoints
		) VALUES
		(
			@ID,
			@Phone,
			@Password,
			@Fullname,
			@Email,
			@BirthDay,
			@ImageURL,
			@IsOnline,
			@RewardPoints
		) ;
		INSERT INTO Workers
		( Id, UserId ) VALUES(@ID, @ID)
		RETURN 1
	END 
	ELSE
	BEGIN
		RETURN 0
	END
 END
 
GO
------ UPDATE WORKER  
 CREATE PROC sp_UpdateWorker
  @ID UNIQUEIDENTIFIER, 
  @Phone NVARCHAR(MAX),
  @Password NVARCHAR(MAX),
  @Fullname NVARCHAR(MAX),
  @Email NVARCHAR(MAX),
  @BirthDay NVARCHAR(MAX),
  @ImageURL NVARCHAR(MAX),
  @IsOnline bit,
  @RewardPoints INT
 AS
 BEGIN
	IF EXISTS (SELECT * FROM Workers w, Users u WHERE w.Id = @ID AND u.Id = @ID ) 
	BEGIN 
		UPDATE Users set 
			Phone= @Phone,
			Password = @Password,
			Fullname = @Fullname ,
			Email = @Email,
			BirthDay = @BirthDay,
			ImageURL = @ImageURL,
			IsOnline = @IsOnline,
			RewardPoints = @RewardPoints 
		WHERE Id = @ID
		RETURN 1
	END 
	ELSE
	BEGIN
		RETURN 0
	END
 END 
 GO
  ------ DELETE WORKER BY ID
 CREATE PROC sp_DeleteWorkerById
  @ID UNIQUEIDENTIFIER 
 AS
 BEGIN
	IF EXISTS (SELECT * FROM Workers w, Users u WHERE w.Id = @ID AND w.Id = @ID ) 
	BEGIN  
		IF EXISTS (SELECT * FROM WorkerOfServices WS WHERE WS.WorkerId = @ID ) 
		BEGIN
			DELETE WorkerOfServices   WHERE WorkerOfServices.WorkerId = @ID
		END 
		DELETE Workers WHERE Workers.Id = @ID
		DELETE Users WHERE Users.Id =  @ID
		RETURN 1
	END 
	ELSE
	BEGIN
		RETURN 0
	END
 END