CREATE TABLE EventMembers (
    eventMembersId INT IDENTITY(1,1) PRIMARY KEY,
    eventId INT,
    userId INT,
	isAdmin BIT

	CONSTRAINT FK_EventMembers_EventId FOREIGN KEY (eventId) REFERENCES Events(eventId),
	CONSTRAINT FK_EventMembers_UserId FOREIGN KEY (userId) REFERENCES UserAccess(userId)
)