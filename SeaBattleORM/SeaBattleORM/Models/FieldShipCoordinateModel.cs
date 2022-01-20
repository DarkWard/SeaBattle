namespace SeaBattleORM
{
    [Table("FieldShipCoordinate")]
    public class FieldShipCoordinateModel
    {
        [PrimaryKey]
        public int ShipCoordinateID { get; set; }

        [PrimaryKey]
        public int FieldID { get; set; }
    }
}