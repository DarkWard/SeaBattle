namespace SeaBattleORM
{
    [Table("ShipCoordinate")]
    [ForeignKey("FieldShipCoordinate", "ShipCoordinateID", typeof(FieldShipCoordinateModel))]
    public class ShipCoordinateModel
    {
        [PrimaryKey]
        public int Id { get; set; }

        public int CoordinateID { get; set; }

        public int ShipID { get; set; }
    }
}