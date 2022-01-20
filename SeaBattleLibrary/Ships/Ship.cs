namespace SeaBattleLibrary
{
    public abstract class Ship
    {
        public static bool operator ==(Ship ship1, Ship ship2)
        {
            return (ship1 is null && ship2 is null)
                || (ship1 is not null && ship2 is not null
                    && ship1.Type == ship2.Type
                    && ship1.Speed == ship2.Speed
                    && ship1.Size == ship2.Size);
        }

        public static bool operator !=(Ship ship1, Ship ship2)
        {
            return !((ship1 is null && ship2 is null)
                || (ship1 is not null && ship2 is not null
                    && ship1.Type == ship2.Type
                    && ship1.Speed == ship2.Speed
                    && ship1.Size == ship2.Size));
        }

        public int Speed { get; }

        public int Size { get; }

        public int Distance { get; }

        public ShipType Type { get; set; }

        public Ship(int size, int speed, int distance, ShipType type)
        {
            if (size <= 0)
            {
                throw new ArgumentException("Size should be greater than 0");
            }
            else
            {
                Size = size;
            }

            if (speed < 0 || distance < 0)
            {
                throw new ArgumentException("Speed and distance should be greater than 0");
            }
            else
            {
                Speed = speed;
                Distance = distance;

            }

            Type = type;
        }

        public void Move(Coordinate newLocation, Field field)
        {
            if (!field.Coordinates.Contains(newLocation))
            {
                throw new ArgumentOutOfRangeException("Selected location is out of the range");
            }
            else if (field.Ships.ContainsKey(newLocation))
            {
                throw new ArgumentException("Coordinate already contains a ship");
            }
            else if (!field.Ships.ContainsValue(this))
            {
                throw new ArgumentException("There no such ship on the field. Use field[] to insert it");
            }

            if (Size == 1)
            {
                var ship = field.Ships.FirstOrDefault(x => x.Value == this);
                field.Ships.Remove(ship.Key);
                field.Ships.Add(newLocation, ship.Value);
            }
            else if (Size > 1)
            {
                var ship = field.Ships.Where(x => x.Value == this).Select(x => x.Key);

                foreach (var item in ship)
                {
                    if (!field.Ships.ContainsKey(new Coordinate((item.X + 1), item.Y)))
                    {
                        continue;
                    }
                    else
                    {
                        throw new ArgumentException("Coordinate already contains a ship");
                    }
                }

                foreach (var item in ship)
                {
                    field.Ships.Remove(item);
                    field.Ships.Add(new Coordinate((item.X + 1), item.Y), this);
                }
            }
        }

        public override string ToString()
        {
            return $"{Type} Ship - Size: {Size}, Speed: {Speed} Distance: {Distance}";
        }
    }
}