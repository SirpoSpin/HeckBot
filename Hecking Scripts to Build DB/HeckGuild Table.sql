USE heckdata;

CREATE TABLE `heckguild` (
  `ID` int Auto_increment NOT NULL,
  `DiscordSnowflake` varchar(100) NOT NULL,
  `CreatedDate` datetime NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;