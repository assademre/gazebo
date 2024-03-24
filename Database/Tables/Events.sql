CREATE TABLE Events (
    eventId INT IDENTITY(1,1) PRIMARY KEY,
    createrId INT,
	eventTypeId INT,
    eventName VARCHAR(100),
	budget DECIMAL(10,2),
	place VARCHAR(50),
	currencyId INT,
	statusId INT,
	createdDate DATETIME,
	updatedDate DATETIME,
	eventDate DATETIME

	CONSTRAINT FK_Events_Currency FOREIGN KEY (currencyId) REFERENCES Currency(currencyId),
	CONSTRAINT FK_Events_CreaterId FOREIGN KEY (createrId) REFERENCES Users(userId),
	CONSTRAINT FK_Events_EventTypeId FOREIGN KEY (eventTypeId) REFERENCES EventTypes(eventTypeId),
	CONSTRAINT FK_Events_Status FOREIGN KEY (statusId) REFERENCES Status(statusId)
)