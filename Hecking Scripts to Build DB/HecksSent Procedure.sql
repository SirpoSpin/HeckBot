USE heckdata;
DELIMITER //

CREATE DEFINER=`heckadmin`@`%` PROCEDURE `HecksSent`(IN UserID INT)
BEGIN
	select CONCAT(h.Value, ' heck(s) sent to ', huRecBy.username, '. Reason: "', h.Name, '".') AS HeckItem from heck h
	join heckuser huRecBy on huRecBy.ID = h.UserID
	join heckuser huSentBy on huSentBy.ID = h.CreatedByUserID
	where huSentBy.ID = UserID
    ORDER BY h.CreatedDate desc
	LIMIT 5;
END //
DELIMITER ;