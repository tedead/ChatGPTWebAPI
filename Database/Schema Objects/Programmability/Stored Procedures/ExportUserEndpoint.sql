CREATE PROCEDURE ExportUserEndpoint(
	@UserID INT
)
AS
BEGIN
	SELECT EndpointID, UserID, UserEndpoint, ConnectionString
	FROM Endpoint
	WHERE UserID = @UserID
END