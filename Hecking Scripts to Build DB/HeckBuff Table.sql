USE heckdata;

CREATE TABLE `heckbuff` (
  `ID` int Auto_increment NOT NULL,
  `Name` varchar(200) NOT NULL,
  `Value` decimal(9,2) NOT NULL,
  `CreatedDate` datetime NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
