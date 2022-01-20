use SeaBattleBase
go

declare @BattleShipType int;
declare @SupportShipType int;
declare @CombinedShipType int;

insert into ShipType (SType)
	values 
	('Battle'),
	('Support'),
	('Combined')

select @BattleShipType = ID
					from ShipType
					where SType IN ('Battle');

select @SupportShipType = ID
					from ShipType
					where SType IN ('Support');

select @CombinedShipType = ID
					from ShipType
					where SType IN ('Combined');

insert into Ships (Speed, Size, Distance, TypeID)
	values
	(5, 2, 3, @BattleShipType),
	(7, 1, 7, @SupportShipType),
	(3, 3, 2, @CombinedShipType)

insert into Field (Size)
	values
	(5),
	(21)
						
insert into Coordinates (X, Y)
	values
	(1, 2),
	(3, -1),
	(-2, 4),
	(2, -4)

insert into ShipCoordinate (CoordinateID, ShipID)
	values
	(1, 1),
	(2, 1),
	(3, 2),
	(4, 3)

declare @temp table 
(
CoordID int,
FieID int
);

insert into FieldShipCoordinate (ShipCoordinateID, FieldID)
select s.ID as ShipCoordinateID, f.ID as FieldID
from ShipCoordinate s, Field f