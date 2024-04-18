CREATE TABLE Tasks (
    taskId INT IDENTITY(1,1) PRIMARY KEY,
    eventId INT,
    taskName VARCHAR(100),
	budget DECIMAL(10,2),
	place VARCHAR(50),
	currencyId INT,
	statusId INT,
	ownerId INT,
	createdDate DATETIME,
	updatedDate DATETIME,
	taskDate DATETIME

	CONSTRAINT FK_Tasks_Currency FOREIGN KEY (currencyId) REFERENCES Currency(currencyId),
	CONSTRAINT FK_Tasks_CreaterId FOREIGN KEY (ownerId) REFERENCES Users(userId),
	CONSTRAINT FK_Tasks_EventId FOREIGN KEY (eventId) REFERENCES Events(eventId),

	CONSTRAINT UQ_EventId_TaskName UNIQUE (eventId, taskName)
)