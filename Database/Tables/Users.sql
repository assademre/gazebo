CREATE TABLE Users (
    userid INT IDENTITY(1,1) PRIMARY KEY,
    username VARCHAR(20),
	name VARCHAR(50),
    surname VARCHAR(50),
	email VARCHAR(50),
	phoneNumber VARCHAR(50)
);