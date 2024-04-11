CREATE TABLE Notifications(
	notificationId INT IDENTITY(1,1) PRIMARY KEY,
	userId INT,
	subject VARCHAR(100),
	body VARCHAR(MAX),
	createdDate DATETIME,
	isRead BIT

	CONSTRAINT FK_Notifications_UserId FOREIGN KEY (userId) REFERENCES Users(userId)
)