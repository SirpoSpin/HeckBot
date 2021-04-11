USE heckdata;

CREATE TABLE `heckuser` (
  `ID` int Auto_increment NOT NULL,
  `GuildID` int NOT NULL,
  `DiscordSnowflake` varchar(100) NOT NULL,
  `username` varchar(100) NOT NULL,
  `AvailableHecks` decimal(9,2) NOT NULL DEFAULT '0.00',
  `CreatedDate` datetime NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
