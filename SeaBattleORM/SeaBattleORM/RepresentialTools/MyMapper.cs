using SeaBattleLibrary;

namespace SeaBattleORM
{
    public static class MyMapper
    {
        public static ShipsModel MapShipToModel(this Ship ship)
        {
            return new ShipsModel { Distance = ship.Distance, SLength = ship.Size, Speed = ship.Speed, TypeID = (int)ship.Type };
        }

        public static Ship MapModelToShip(int size, int speed, int distance, int type)
        {
            switch (type)
            {
                case 1:
                    return new BattleShip(size, speed, distance);

                case 2:
                    return new SupportShip(size, speed, distance);

                case 3:
                    return new CombinedShip(size, speed, distance);

                default: throw new ArgumentOutOfRangeException();
            }
        }

        public static CoordinateModel MapCoordinateToModel(this Coordinate coordinate)
        {
            return new CoordinateModel { X = coordinate.X, Y = coordinate.Y };
        }

        public static Coordinate MapModeltoCoordinate(int x, int y)
        {
            return new Coordinate(x, y);
        }

        public static MappedField MapFieldToModel(this Field field)
        {
            MappedField model = new MappedField() { Size = field.Size };

            var ships = new Dictionary<CoordinateModel, ShipsModel>();

            foreach (var item in field.Ships)
            {
                ships.Add(item.Key.MapCoordinateToModel(), item.Value.MapShipToModel());
            }
            model.Ships = ships;

            return model;
        }
    }
}