CREATE PROCEDURE ExportUserQueries(
	@BeginDate DATETIME = NULL,
	@EndDate DATETIME = NULL,
	@UserID INT
)
AS
BEGIN
	IF @BeginDate IS NULL
	BEGIN
		SELECT @BeginDate = '1900-01-01 00:00:00.000'
	END

	IF @EndDate IS NULL
	BEGIN
		SELECT @EndDate = GETDATE()
	END

	SELECT QueryID, Input, [Output], UserID, dt_Created
	FROM Queries
	WHERE dt_Created BETWEEN @BeginDate AND @EndDate
		  AND UserID = @UserID
END