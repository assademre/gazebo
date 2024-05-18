CREATE VIEW vwProfiles

AS

SELECT TOP(1)
	U.userid,
	U.Username,
	AD.PhoneNumber,
	AD.DateOfBirth,
	AD.Bio
FROM Users U 
	INNER JOIN AdditionalData AD
		ON U.userid = AD.userId
