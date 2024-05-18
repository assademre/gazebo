CREATE TABLE Friendship (
    friendshipId INT IDENTITY(1,1) PRIMARY KEY,
    senderId INT,
    receiverId INT,
	friendshipStatusId INT,
	updateDate DATETIME

	CONSTRAINT FK_Friendship_Status FOREIGN KEY (friendshipStatusId) REFERENCES FriendshipStatus(friendshipStatusId),
	CONSTRAINT FK_Friendship_SenderId FOREIGN KEY (senderId) REFERENCES Users(userId),
	CONSTRAINT FK_Friendship_ReceiverId FOREIGN KEY (receiverId) REFERENCES Users(userId),
	CONSTRAINT UQ_Friendship_sender_receiver UNIQUE (senderId, receiverId)
)