INSERT INTO EventTypes VALUES('Birthday Party')

SET IDENTITY_INSERT EventTypes ON;

INSERT INTO EventTypes(eventTypeId, eventTypeName) VALUES(99, 'Default')

SET IDENTITY_INSERT EventTypes OFF