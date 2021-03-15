USE [ae_code_challange]
GO

INSERT INTO [dbo].[server_response_log]
           ([StartTimeUTC]
           ,[EndTimeUTC]
           ,[HTTPStatusCode]
           ,[DataString]
           ,[Status]
           ,[StatusString])
     VALUES
           (<StartTimeUTC, datetime,>
           ,<EndTimeUTC, datetime,>
           ,<HTTPStatusCode, int,>
           ,<DataString, varchar(max),>
           ,<Status, int,>
           ,<StatusString, varchar(max),>)
GO


