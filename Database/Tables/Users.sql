CREATE TABLE Users (
    userid INT PRIMARY KEY,
    name VARCHAR(50),
	username VARCHAR(50),
    surname VARCHAR(50),
    email VARCHAR(50),
    phoneNumber VARCHAR(50),
    FOREIGN KEY (userid) REFERENCES Users(UserID)
);
