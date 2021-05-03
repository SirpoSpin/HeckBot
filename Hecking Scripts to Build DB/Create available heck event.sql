USE heckdata;

SHOW EVENTS;

#CREATE
ALTER 
EVENT eve_tbl_available_hecks_update
    ON SCHEDULE EVERY 5 MINUTE
    DO
      UPDATE heckuser SET AvailableHecks = AvailableHecks + 1 WHERE AvailableHecks < 25;