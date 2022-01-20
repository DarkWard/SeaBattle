namespace SeaBattleLibrary
{
    public class CombinedShip : Ship, IShootable, IRepareable
    {
        public CombinedShip(int size, int speed, int distance)
            : base(size, speed, distance, ShipType.Combined)
        {
        }

        public void Repare(Coordinate location, Field field)
        {
            if (!field.Coordinates.Contains(location))
            {
                throw new ArgumentOutOfRangeException("Selected location is out of the range");
            }
            else if (!field.Ships.ContainsValue(this))
            {
                throw new ArgumentException("There no such ship on the field. Use field[] to insert it");
            }
            else
            {
                var key = field.Ships.FirstOrDefault(x => x.Value == this).Key;

                if (Math.Sqrt(Math.Pow(location.X - key.X, 2) + Math.Pow(location.Y - key.Y, 2)) <= Distance)
                {
                    //Repare
                }
                else
                {
                    throw new ArgumentException("Chosen location out of ship's range");
                }
            }
        }

        public void Shoot(Coordinate location, Field field)
        {
            if (!field.Coordinates.Contains(location))
            {
                throw new ArgumentOutOfRangeException("Selected location is out of the range");
            }
            else if (!field.Ships.ContainsKey(location))
            {
                throw new ArgumentException("Coordinate does not contains a ship");
            }
            else if (!field.Ships.ContainsValue(this))
            {
                throw new ArgumentException("There no such ship on the field. Use [field].AddShip to insert it");
            }
            else
            {
                var key = field.Ships.FirstOrDefault(x => x.Value == this).Key;

                if (Math.Sqrt(Math.Pow(location.X - key.X, 2) + Math.Pow(location.Y - key.Y, 2)) <= Distance)
                {
                    //Shoot
                }
                else
                {
                        throw new ArgumentException("Chosen location out of ship's range");
                }
            }
        }
    }
}