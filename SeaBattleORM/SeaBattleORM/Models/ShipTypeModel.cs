namespace SeaBattleORM
{
    [Table("ShipType")]
    public class ShipTypeModel
    {
        [PrimaryKey]
        public int ID { get; set; }

        public string SType { get; set; }
    }
}