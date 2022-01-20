namespace SeaBattleORM
{
    [Table("Ships")]
    [ForeignKey("ShipCoordinate", "ShipID", typeof(ShipCoordinateModel))]
    public class ShipsModel
    {
        [PrimaryKey]
        public int ID { get; set; }

        public int Speed { get; set; }

        public int SLength { get; set; }

        public int Distance { get; set; }

        public int TypeID { get; set; }

        public override string ToString()
        {
            return $"{Speed}, {SLength}, {Distance}, {TypeID + 1}";
        }
    }
}