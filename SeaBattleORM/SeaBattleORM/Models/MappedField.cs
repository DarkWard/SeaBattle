using System.Collections.Generic;

namespace SeaBattleORM
{
    [Table("Field")]
    public class MappedField
    {
        public int Size { get; set; }

        public Dictionary<CoordinateModel, ShipsModel> Ships { get; set; } = new Dictionary<CoordinateModel, ShipsModel>();
    }
}