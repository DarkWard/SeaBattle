namespace SeaBattleORM
{
    [Table("Coordinates")]
    [ForeignKey("ShipCoordinate", "CoordinateID", typeof(ShipCoordinateModel))]
    public class CoordinateModel
    {
        [PrimaryKey]
        public int ID { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

        public override string ToString()
        {
            return $"{X}, {Y}";
        }
    }
}