USE heckdata;
DELIMITER //

CREATE DEFINER=`heckadmin`@`%` PROCEDURE `InsertHeck`(IN UserID INT,IN HeckedUserID INT,IN Reason varchar(200),IN HeckValue decimal(9,2))
BEGIN

	#insert heck
    INSERT INTO heck (UserID, Name, Value, CreatedByUserID, CreatedDate, ExpiryDate)
    VALUES(HeckedUserID,Reason,HeckValue,UserID,NOW(),DATE_ADD(NOW(), INTERVAL 30 DAY));
    #update available hecks
    UPDATE HeckUser set AvailableHecks = (AvailableHecks - ABS(HeckValue)) where ID = UserID;
    select 1;
END //
DELIMITER ;