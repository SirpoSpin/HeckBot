USE heckdata;

CREATE TABLE `heck` (
  `ID` int AUTO_INCREMENT NOT NULL,
  `UserID` int NOT NULL,
  `Name` varchar(200) NOT NULL,
  `Value` decimal(9,2) NOT NULL DEFAULT '0.000000000',
  `CreatedByUserID` int NOT NULL,
  `CreatedDate` datetime NOT NULL,
  `ExpiryDate` datetime NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
