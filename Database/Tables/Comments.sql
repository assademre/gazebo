CREATE TABLE Comments (
    commentId INT IDENTITY(1,1) PRIMARY KEY,
    postGroupTypeId TINYINT,
    postGroupId INT,
	commentOwnerId INT,
	commentText VARCHAR(500),
	commentDate DATETIME

	CONSTRAINT FK_Comments_PostGroupTypeId FOREIGN KEY (postGroupTypeId) REFERENCES PostGroups(postGroupTypeId),
	CONSTRAINT FK_Comments_CommentOwnerId FOREIGN KEY (CommentOwnerId) REFERENCES Users(userId)
)