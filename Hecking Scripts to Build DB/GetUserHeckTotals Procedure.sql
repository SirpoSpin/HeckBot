USE heckdata;
DELIMITER //

CREATE PROCEDURE GetUserHeckTotals(IN UserID INT)
BEGIN
	#get heck total 
	SET @HeckTotal = 0.0;
	select SUM(h.Value) INTO @HeckTotal from heck h where h.UserID = UserID AND expirydate >=  CURdate();
	SET  @HeckTotal:=IFNULL(@HeckTotal, 0.0);

		
	#get heck buff total
	SET @HeckBuffTotal = 1.0;
	select SUM(hb.Value) INTO @HeckBuffTotal from heckbuff hb
	join heckuserbuff hub on hub.BuffID = hb.ID
	where hub.UserID = UserID AND hub.expirydate >=  CURdate();

	SET  @HeckBuffTotal:=IFNULL(@HeckBuffTotal, 1.0);

	#get combine buff total
	SET  @HeckActualTotal:= (@HeckBuffTotal*@HeckTotal);
    
	#get available hecks total
	SET @AvailableHecks = 0.0;
	select SUM(AvailableHecks) INTO @AvailableHecks from heckuser h where h.UserID = UserID;
	SET  @AvailableHecks:=IFNULL(@AvailableHecks, 0.0);

	SELECT @HeckTotal AS Hecks, @HeckBuffTotal AS WeightTotal, @HeckActualTotal AS Total, @AvailableHecks AS AvailableHecks;
END //
DELIMITER ;

