CREATE TABLE EndPoint(
	EndpointID INT IDENTITY PRIMARY KEY,
	UserID INT,
	UserEndpoint VARCHAR(500),
	ConnectionString VARCHAR(500)
)