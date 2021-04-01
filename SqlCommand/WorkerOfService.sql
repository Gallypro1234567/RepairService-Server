 ------ GET ALL WORKEROFSERVICE
 CREATE PROC sp_GetWorkerOfService
 AS
 BEGIN
	SELECT
	c.Id,
	c.ServiceId,
	c.WorkerId,
	d.Fullname,
	d.Phone,
	d.Email,
	d.RewardPoints,
	a.Code,
	a.Name, 
	c.CreateAt,
	c.isOnline,
	c.isApproval,
	c.Position
	FROM Services A, Workers B, WorkerOfServices C, Users D
	WHERE A.Id  = C.ServiceId
	AND B.Id  = C.WorkerId
	AND B.UserId = D.Id

 END
 
 GO
 ------ GET WORKEROFSERVICE BY ID
 CREATE PROC sp_InsertWorkerOfService
  @ID UNIQUEIDENTIFIER, 
  @ServiceId UNIQUEIDENTIFIER,
  @WorkerId UNIQUEIDENTIFIER,
  @FeelBackId UNIQUEIDENTIFIER,
  @Position NVARCHAR(MAX), 
  @isOnline BIT, 
  @isApproval BIT, 
  @CreateAt DATETIME  
 AS
 BEGIN
	IF NOT EXISTS (SELECT * FROM WorkerOfServices ws  WHERE ws.Id = @ID ) 
	BEGIN 
		INSERT INTO WorkerOfServices 
		(
		   Id,
		   ServiceId,
		   WorkerId,
		   FeelbackId,
		   Position,
		   isOnline,
		   isApproval,
		   CreateAt
		) VALUES
		(
			@ID,
			@ServiceId,
			@WorkerId,
			@FeelBackId,
			@Position,
			@isOnline,
			@isApproval,
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
------  UPDATE WORKEROFSERVICE  
 CREATE PROC sp_UpdateWorkerOfService
  @ID UNIQUEIDENTIFIER, 
  @ServiceId UNIQUEIDENTIFIER,
  @WorkerId UNIQUEIDENTIFIER,
  @FeelBackId UNIQUEIDENTIFIER,
  @Position NVARCHAR(MAX), 
  @isOnline BIT, 
  @isApproval BIT, 
  @CreateAt DATETIME  
 AS
 BEGIN
	IF EXISTS (SELECT * FROM WorkerOfServices ws  WHERE ws.Id = @ID ) 
	BEGIN 
		UPDATE WorkerOfServices set 
			ServiceId = @ServiceId,
			WorkerId = @WorkerId ,
			FeelBackId = @FeelBackId,
			Position = @Position,
			isOnline= @isOnline,
			isApproval = @isApproval
        WHERE Id = @ID
		RETURN 1
	END 
	ELSE
	BEGIN
		RETURN 0
	END
 END

 GO
 ------ DELETE WORKEROFSERVICE BY ID
 CREATE PROC sp_DeleteWorkerOfServicesById
  @ID UNIQUEIDENTIFIER 
 AS
 BEGIN
	IF EXISTS (SELECT * FROM WorkerOfServices ws  WHERE ws.Id = @ID  ) 
	BEGIN  
		 
        DELETE WorkerOfServices WHERE WorkerOfServices.Id = @ID 
		RETURN 1
	END 
	ELSE
	BEGIN
		RETURN 0
	END
 END

