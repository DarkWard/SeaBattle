namespace SeaBattleLibrary
{
    public struct Coordinate
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Quadrant Quadrant
        {
            get { return CalculateQuadrant(X, Y); }
        }

        public Coordinate(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Quadrant CalculateQuadrant(int x, int y)
        {
            if (x == 0 && y == 0)
            {
                return Quadrant.AxisIntersection;
            }
            else if (x >= 0 && y >= 0)
            {
                return Quadrant.FirstQuadrant;
            }
            else if (x < 0 && y > 0)
            {
                return Quadrant.SecondQuadrant;
            }
            else if (x <= 0 && y <= 0)
            {
                return Quadrant.ThirdQuadrant;
            }
            else if (x > 0 && y < 0)
            {
                return Quadrant.FourthQuadrant;
            }

            throw new ArgumentException();
        }

        public override string ToString()
        {
            return $"{X};{Y}";
        }
    }
}