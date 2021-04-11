USE heckdata;

CREATE TABLE `heckuserbuff` (
  `ID` int Auto_increment NOT NULL,
  `UserID` int NOT NULL,
  `BuffID` int NOT NULL,
  `CreatedDate` datetime NOT NULL,
  `ExpiryDate` datetime NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;