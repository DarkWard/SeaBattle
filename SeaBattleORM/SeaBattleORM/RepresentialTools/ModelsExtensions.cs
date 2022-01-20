using SeaBattleLibrary;

namespace SeaBattleORM
{
    public static class ModelsExtensions
    {
        public static Coordinate ViewCoordinate(this CoordinateModel coordinateModel)
        {
            return new Coordinate(coordinateModel.X, coordinateModel.Y);
        }

        public static List<Field> ViewFields(this IEnumerable<FieldModel> fieldModels)
        {
            var result = new List<Field>();

            foreach (var fieldModel in fieldModels.GroupBy(x => x.ID))
            {
                var field = new Field(fieldModel.First().Size);

                foreach (var fm in fieldModel)
                {
                    var coord = new Coordinate(fm.X, fm.Y);
                    field[coord.X, coord.Y, coord.Quadrant] = MyMapper.MapModelToShip(fm.Distance, fm.SLength, fm.Speed, fm.TypeID);
                }

                result.Add(field);
            }

            return result;
        }

        public static Field ViewMappedField(this MappedField field)
        {
            var res = new Field(field.Size);

            foreach (var s in field.Ships)
            {
                res.Ships.Add(s.Key.ViewCoordinate(), s.Value.ViewShip());
            }

            return res;
        }

        public static Ship ViewShip(this ShipsModel shipModel)
        {
            return MyMapper.MapModelToShip(shipModel.SLength, shipModel.Speed, shipModel.Distance, shipModel.TypeID);
        }
    }
}