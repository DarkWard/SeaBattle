namespace SeaBattleORM
{
    [Table("Field")]
    public class FieldModel
    {
        [PrimaryKey]
        [ForeignKey("FieldShipCoordinate", "FieldID", typeof(FieldShipCoordinateModel))]
        public int ID { get; set; }
        public int Size { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int SLength { get; set; }
        public int Speed { get; set; }
        public int Distance { get; set; }
        public int TypeID { get; set; }
    }
}