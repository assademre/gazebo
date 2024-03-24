CREATE VIEW vwEventMembers 
AS
SELECT 
	E.eventId,
	T.ownerId,
	T.taskId,
	U.userId,
	T.taskName
FROM Events E
INNER JOIN Tasks T ON E.eventId = T.eventId
INNER JOIN Users U ON U.userId = T.ownerId