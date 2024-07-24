using System;

namespace Triangles.Models
{
    public class Triangle
    {
        public double Side1 { get; set; }
        public double Side2 { get; set; }
        public double Side3 { get; set; }

        public Triangle(double side1, double side2, double side3)
        {
            Side1 = side1;
            Side2 = side2;
            Side3 = side3;
        }

        public bool IsValid()
        {
            return Side1 + Side2 > Side3 && Side1 + Side3 > Side2 && Side2 + Side3 > Side1;
        }

        public double Perimeter()
        {
            return Side1 + Side2 + Side3;
        }

        public double Area()
        {
            double s = Perimeter() / 2;
            return Math.Sqrt(s * (s - Side1) * (s - Side2) * (s - Side3));
        }

        public bool IsRightAngled()
        {
            double[] sides = { Side1, Side2, Side3 };
            Array.Sort(sides);
            return Math.Abs(Math.Pow(sides[0], 2) + Math.Pow(sides[1], 2) - Math.Pow(sides[2], 2)) < 0.001;
        }

        public bool IsEquilateral()
        {
            return Math.Abs(Side1 - Side2) < 0.001 && Math.Abs(Side2 - Side3) < 0.001;
        }

        public bool IsIsosceles()
        {
            return Math.Abs(Side1 - Side2) < 0.001 || Math.Abs(Side1 - Side3) < 0.001 || Math.Abs(Side2 - Side3) < 0.001;
        }

        public double[] NormalizedSides()
        {
            double perimeter = Perimeter();
            return new double[] { Side1 / perimeter, Side2 / perimeter, Side3 / perimeter };
        }

        public bool AreCongruent(Triangle other)
        {
            double[] sides1 = { Side1, Side2, Side3 };
            double[] sides2 = { other.Side1, other.Side2, other.Side3 };
            Array.Sort(sides1);
            Array.Sort(sides2);

            for (int i = 0; i < 3; i++)
            {
                if (Math.Abs(sides1[i] - sides2[i]) > 0.001)
                {
                    return false;
                }
            }

            return true;
        }

        public bool AreSimilar(Triangle other)
        {
            double[] sides1 = { Side1, Side2, Side3 };
            double[] sides2 = { other.Side1, other.Side2, other.Side3 };
            Array.Sort(sides1);
            Array.Sort(sides2);

            double ratio = sides1[0] / sides2[0];
            for (int i = 1; i < 3; i++)
            {
                if (Math.Abs(sides1[i] / sides2[i] - ratio) > 0.001)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
