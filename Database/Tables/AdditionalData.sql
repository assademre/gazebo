CREATE TABLE AdditionalData (
	userid INT PRIMARY KEY,
	phoneNumber VARCHAR(50),
	dateOfBirth DATETIME,
	bio VARCHAR(250)

	CONSTRAINT FK_AdditionalData_userId FOREIGN KEY (userId) REFERENCES Users(userId),
);
