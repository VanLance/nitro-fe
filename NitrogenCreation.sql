CREATE TABLE EquipDbFieldToExcelColumnMap(
	Id INT PRIMARY KEY IDENTITY,
	EquipListNumber INT NOT NULL,
	[Description] INT NOT NULL,
	ControlPanel INT NOT NULL,
	Notes INT NOT NULL,
);

CREATE TABLE EquipSheetFormat(
	Id INT PRIMARY KEY IDENTITY,
	[FileName] NVARCHAR(250) NOT NULL,
	StartingDataLine INT NOT NULL,
	EquipDbFieldToExcelColumnMapId INT NOT NULL,

	CONSTRAINT FK_EquipDbFieldToExcelColumnMap FOREIGN KEY (EquipDbFieldToExcelColumnMapId) REFERENCES EquipDbFieldToExcelColumnMap (Id)
);

CREATE TABLE Project (
	ProjectNumber VARCHAR(100) PRIMARY KEY NOT NULL,
	[Description] VARCHAR(255),
	EquipSheetFormatId INT NOT NULL,
	IoSheetFormatId INT

	CONSTRAINT FK_EquipSheetFormat FOREIGN KEY (EquipSheetFormatId) REFERENCES EquipSheetFormat (Id)
);


CREATE TABLE Vendor (
	Id INT PRIMARY KEY IDENTITY,
	Name VARCHAR(100) NOT NULL,
	Contact VARCHAR(100),
	Email VARCHAR(100),
	Phone VARCHAR(20)
);

CREATE TABLE UserDefinition (
	Id INT PRIMARY KEY IDENTITY,
	TextField1 VARCHAR(100),
	TextField2 VARCHAR(100),
	TextField3 VARCHAR(100),
	TextField4 VARCHAR(100),
	TextField5 VARCHAR(100),
	TextLabel1 VARCHAR(100),
	TextLabel2 VARCHAR(100),
	TextLabel3 VARCHAR(100),
	TextLabel4 VARCHAR(100),
	TextLabel5 VARCHAR(100),
	CbLabel1 VARCHAR(100),
	CbLabel2 VARCHAR(100),
	CbLabel3 VARCHAR(100),
	CbLabel4 VARCHAR(100),
	CbLabel5 VARCHAR(100)
);


CREATE TABLE Equipment (
	Id INT PRIMARY KEY IDENTITY NOT NULL,
	ProjectNumber VARCHAR(100) NOT NULL,
	Area  VARCHAR(50),
	EquipmentId VARCHAR(50) NOT NULL,
	EquipmentSubId VARCHAR(50),
	ParentEquipmentId Int,
	CustomerEquipmentId VARCHAR(50),
	[Description] VARCHAR(255),
	ControlPanel VARCHAR(100),
	Notes VARCHAR(255),
	ModelNumber VARCHAR(50),
	VendorId INT,
	UserDefinitionId Int,
	ExcelRowNumber int

	CONSTRAINT FK_ProjectNumber FOREIGN KEY (ProjectNumber) REFERENCES Project (ProjectNumber),
	CONSTRAINT FK_Vendor FOREIGN KEY (VendorId) REFERENCES Vendor (Id),
	CONSTRAINT FK_UserDefinition FOREIGN KEY (UserDefinitionId) REFERENCES UserDefinition (Id)
);

CREATE TABLE [IO] (
	Id INT PRIMARY KEY IDENTITY,
	EquipmentId INT NOT NULL,

	CONSTRAINT FK_IO FOREIGN KEY (EquipmentId) REFERENCES Equipment (Id)
);

DELETE FROM EquipDbFieldToExcelColumnMap;
DELETE FROM EquipSheetFormat;
DELETE FROM Equipment;
DELETE FROM PROJECT;

SELECT * From EquipDbFieldToExcelColumnMap;
SELECT * FROM EquipSheetFormat;
SELECT * FROM Project;
SELECT * FROM Equipment ORDER BY ExcelRowNumber;
