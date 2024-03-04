CREATE TABLE Project (
	ProjectNumber INT PRIMARY KEY IDENTITY,
	[Description] VARCHAR,
	EquipListFormatDef VARCHAR NOT NULL,
	IoListFormatDef VARCHAR NOT NULL
);

CREATE TABLE Vendor (
	Id INT PRIMARY KEY IDENTITY,
	Name VARCHAR NOT NULL,
	Contact VARCHAR,
	Email VARCHAR NOT NULL,
	Phone VARCHAR NOT NULL,
);

CREATE TABLE UserDefinition (
	Id INT PRIMARY KEY IDENTITY,
	TextField1 VARCHAR,
	TextField2 VARCHAR,
	TextField3 VARCHAR,
	TextField4 VARCHAR,
	TextField5 VARCHAR,
	TextLabel1 VARCHAR,
	TextLabel2 VARCHAR,
	TextLabel3 VARCHAR,
	TextLabel4 VARCHAR,
	TextLabel5 VARCHAR,
	CbLabel1 VARCHAR,
	CbLabel2 VARCHAR,
	CbLabel3 VARCHAR,
	CbLabel4 VARCHAR,
	CbLabel5 VARCHAR,
);

CREATE TABLE Equipment (
	Id INT PRIMARY KEY IDENTITY NOT NULL,
	ProjectNumber INT NOT NULL,
	Area  VARCHAR,
	EquipmentId VARCHAR NOT NULL,
	EquipmentSubId VARCHAR,
	ParentEquipmentId Int,
	CustomerEquipmentId VARCHAR,
	[Description] VARCHAR,
	ControlPanel VARCHAR,
	Notes VARCHAR,
	ModelNumber VARCHAR,
	VendorId INT,
	UserDefinitionId Int NOT NULL,

	CONSTRAINT FK_ProjectNumber FOREIGN KEY (ProjectNumber) REFERENCES Project (ProjectNumber),
	CONSTRAINT FK_Vendor FOREIGN KEY (VendorId) REFERENCES Vendor (Id),
	CONSTRAINT FK_UserDefinition FOREIGN KEY (UserDefinitionId) REFERENCES UserDefinition (Id),
);

CREATE TABLE [IO] (
	Id INT PRIMARY KEY,
	EquipmentId INT NOT NULL,

	CONSTRAINT FK_IO FOREIGN KEY (EquipmentId) REFERENCES Equipment (Id)
);