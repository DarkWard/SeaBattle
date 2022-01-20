create database SeaBattleBase
go

use SeaBattleBase
go 
alter database SeaBattleBase set TRUSTWORTHY ON; 
go 
EXEC dbo.sp_changedbowner @loginame = N'sa', @map = false 
go 
sp_configure 'show advanced options', 1; 
go 
reconfigure; 
go 
sp_configure 'clr enabled', 1; 
go 
reconfigure; 
go

create table Field (
	ID int identity(1,1) not null constraint PK_Field primary key,
	Size int not null,
)
go

create table ShipType(
	ID int identity(1,1) not null constraint PK_ShipType primary key,
	SType nvarchar(max) not null
	)

go

create table Ships (
	ID int identity(1,1) not null constraint PK_Ships primary key,
	Speed int not null,
	SLength int not null,
	Distance int not null,
	TypeID int not null,
constraint FK_Ships_ShipType foreign key(TypeID)
references ShipType (ID)
)
go

create table Coordinates (
	ID int identity(1,1) not null constraint PK_Coordinates primary key,
	X int not null,
	Y int not null,
)
go

create table ShipCoordinate (
	ID int identity(1,1) not null constraint PK_ShipCoordinate primary key,
	CoordinateID int not null,
	ShipID int not null,
	
constraint FK_ShipCoordinate_Coordinates foreign key(CoordinateID)
references Coordinates (ID),
constraint FK_ShipCoordinate_Ships foreign key(ShipID)
references Ships (ID)
)
go

create table FieldShipCoordinate (
	ShipCoordinateID int not null,
	FieldID int not null,
	constraint PK_FieldShipCoordinate primary key CLUSTERED
	(
	ShipCoordinateID,
	FieldID
),
constraint FK_FieldShipCoordinate_Field foreign key(FieldID)
references Field (ID),
constraint FK_FieldShipCoordinate_ShipCoordinate foreign key(ShipCoordinateID)
references ShipCoordinate (ID)
)
go