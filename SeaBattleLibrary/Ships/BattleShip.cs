namespace SeaBattleLibrary
{
    public class BattleShip : Ship, IShootable
    {
        public BattleShip(int size, int speed, int distance)
            : base(size, speed, distance, ShipType.Battle)
        {
        }

        public void Shoot(Coordinate location, Field field)
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