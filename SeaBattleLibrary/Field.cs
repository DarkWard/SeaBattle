namespace SeaBattleLibrary
{

    public class Field
    {
        public int Size { get; }

        public List<Coordinate> Coordinates { get; set; }

        public Dictionary<Coordinate, Ship> Ships { get; set; }

        public Ship this[int x, int y, Quadrant quadrant]
        {
            get
            {
                x = Math.Abs(x);
                y = Math.Abs(y);

                switch (quadrant)
                {
                    case Quadrant.AxisIntersection :
                        return Ships[new Coordinate(0, 0)];

                    case Quadrant.FirstQuadrant :
                        return Ships[new Coordinate(x, y)];

                    case Quadrant.SecondQuadrant :
                        return Ships[new Coordinate(-x, y)];

                    case Quadrant.ThirdQuadrant :
                        return Ships[new Coordinate(-x, -y)];

                    case Quadrant.FourthQuadrant :
                        return Ships[new Coordinate(x, -y)];

                    default: throw new ArgumentException("There no ship on selected coordinate. Use field[] to insert it");
                }
            }

            set
            {
                x = Math.Abs(x);
                y = Math.Abs(y);

                var coordinate = new Coordinate(x, y);

                switch (quadrant)
                {
                    case Quadrant.AxisIntersection:
                        coordinate = new Coordinate(0, 0);
                        break;

                    case Quadrant.SecondQuadrant:
                        coordinate = new Coordinate(-x, y);
                        break;

                    case Quadrant.ThirdQuadrant:
                        coordinate = new Coordinate(-x, -y);
                        break;

                    case Quadrant.FourthQuadrant:
                        coordinate = new Coordinate(x, -y);
                        break;
                }

                if (!Coordinates.Contains(coordinate))
                {
                    throw new ArgumentOutOfRangeException("Selected location is out of the range");
                }
                else if (!Ships.ContainsKey(coordinate) && value.Size == 1)
                {
                    Ships.Add(coordinate, value);
                }
                else if (!Ships.ContainsKey(coordinate) && Coordinates.Contains(new Coordinate(coordinate.X + value.Size - 1, coordinate.Y)))
                {
                    for (int i = 0; i < value.Size; i++)
                    {
                        if (Ships.ContainsKey(new Coordinate(coordinate.X + i, coordinate.Y)))
                        {
                            throw new ArgumentException($"Coordinate {coordinate} already contains a ship");
                        }
                    }

                    for (int i = 0; i < value.Size; i++)
                    {
                        Ships.Add(new Coordinate(coordinate.X + i, coordinate.Y), value);
                    }
                }
                else
                {
                    throw new ArgumentOutOfRangeException("Ship's size unacceptable");
                }
            }
        }

        public Field(int size)
        {
            Size = size;
            Coordinates = new List<Coordinate>();

            int axisY = size / 2;
            int axisX;

            Ships = new Dictionary<Coordinate, Ship>();

            for (int i = 0; i < size; i++)
            {
                axisX = -size / 2;

                for (int j = 0; j < size; j++)
                {

                    var tmp = new Coordinate(axisX, axisY);

                    Coordinates.Add(tmp);
                    axisX++;
                }
                axisY--;
            }

        }

        public override string ToString()
        {
            return String.Join("\n", Ships.GroupBy(x => x.Value).Select(x => x.First()).OrderBy(x => x.Key));
        }
    }
}