CREATE PROCEDURE ImportUserQueries(
	@Input VARCHAR(MAX),
	@Output VARCHAR(MAX),
	@UserID INT,
	@dt_Created DATETIME
)
AS
BEGIN
	INSERT INTO Queries(Input, Output, UserID, dt_Created)
	SELECT @Input, @Output, @UserID, @dt_Created
END