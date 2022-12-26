using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Khakaton.DataHandler
{
    public class MathVector
    {
        public double x { get; private set; }
        public double y { get; private set; }

        public double Lenght { get
            {
                if (_lenght != null)
                    return _lenght.Value;
                _lenght = Math.Sqrt(x* x + y * y);
                return _lenght.Value;
            } }

        private double? _lenght;

        private MathVector() { }

        public static MathVector CreateInstance(double x, double y)
        {
            MathVector vector = new MathVector();
            vector.x = x;
            vector.y = y;
            return vector;
        }

        public static MathVector CreateInstance(double x1, double y1, double x2, double y2)
        {
            MathVector vector = new MathVector();
            vector.x = x2 - x1;
            vector.y = y2 - y1;
            return vector;
        }

        public bool Between(MathVector a, MathVector b)
        {
            double x1 = a.x, y1 = a.y, x2 = b.x, y2 = b.y;
            double determinator = x1 * y2 - y1 * x2;
            if (determinator == 0)
            {
                throw new Exception("zero determinator");
            }
            double iDeterminator = x * y2 - y * x2;
            double jDeterminator = x1 * y - y1 * x;
            double i = iDeterminator / determinator;
            double j = jDeterminator / determinator;
            return i >= 0 && j >= 0;
        }

        /// <summary>
        /// Get tangent vector to circle
        /// </summary>
        /// <param name="x">Centre of circle</param>
        /// <param name="y">Centre of circle</param>
        /// <param name="r">Radius of circle</param>
        /// <param name="x1">Start point</param>
        /// <param name="y1">Start point</param>
        /// <param name="x2">End point</param>
        /// <param name="y2">End point</param>
        /// <returns></returns>
        public static MathVector GetVectorToAvoidCircle(double x, double y, double r,
            double x1, double y1,
            double x2, double y2)
        {
            MathVector t1 = null, t2 = null;
            
            MathVector a = CreateInstance(x1, y1, x, y);
            double sinA = r / a.Lenght;
            double cosA = Math.Sqrt(1 - sinA * sinA);
            double lengthB = Math.Sqrt(a.Lenght * a.Lenght - r * r);
            double k = a.Lenght * lengthB * cosA / a.y;
            double g = a.x / a.y;
            SquareEquation squareEquation = new SquareEquation((1 + g * g), -2 * k * g, k * k - lengthB * lengthB);
            if (squareEquation.Solve(out double bx1, out double bx2))
            {
                MathVector mainVector = CreateInstance(x1, y1, x2, y2);
                double by1 = k - g * bx1, by2 = k - g * bx2;
                MathVector b1 = CreateInstance(bx1, by1);
                MathVector b2 = CreateInstance(bx2, by2);
                if (mainVector.Between(a, b1) && x1 + bx1 > 0 && x1 + bx1 < 10000
                    && y1 + by1 > 0 && y1 + by1 < 10000)
                {
                    t1 = b1;
                }
                if (mainVector.Between(a, b2) && x1 + bx2 > 0 && x1 + bx2 < 10000
                    && y1 + by2 > 0 && y1 + by2 < 10000)
                {
                    t1 = b2;
                }
                if (t1 == null)
                {
                    if (!mainVector.Between(a, b1) && x1 + bx1 > 0 && x1 + bx1 < 10000
                    && y1 + by1 > 0 && y1 + by1 < 10000)
                    {
                        t1 = b1;
                    }
                    else if (!mainVector.Between(a, b2) && x1 + bx2 > 0 && x1 + bx2 < 10000
                    && y1 + by2 > 0 && y1 + by2 < 10000)
                    {
                        t1 = b2;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            else
            {
                throw new Exception("square equation didn't solve");
            }

            MathVector b = CreateInstance(x2, y2, x, y);
            sinA = r / b.Lenght;
            cosA = Math.Sqrt(1 - sinA * sinA);
            lengthB = Math.Sqrt(b.Lenght * b.Lenght - r * r);
            k = b.Lenght * lengthB * cosA / b.y;
            g = b.x / b.y;
            squareEquation = new SquareEquation((1 + g * g), -2 * k * g, k * k - lengthB * lengthB);
            if (squareEquation.Solve(out bx1, out bx2))
            {
                MathVector mainVector = CreateInstance(x2, y2, x1, y1);
                double by1 = k - g * bx1, by2 = k - g * bx2;
                MathVector b1 = CreateInstance(bx1, by1);
                MathVector b2 = CreateInstance(bx2, by2);
                if (mainVector.Between(b, b1) && x2 + bx1 > 0 && x2 + bx1 < 10000
                    && y2 + by1 > 0 && y2 + by1 < 10000)
                {
                    t2 = b1;
                }
                if (mainVector.Between(b, b2) && x2 + bx2 > 0 && x2 + bx2 < 10000
                    && y2 + by2 > 0 && y2 + by2 < 10000)
                {
                    t2 = b2;
                }
                if (t2 == null)
                {
                    if (!mainVector.Between(b, b1) && x2 + bx1 > 0 && x2 + bx1 < 10000
                    && y2 + by1 > 0 && y2 + by1 < 10000)
                    {
                        t2 = b1;
                    }
                    else if (!mainVector.Between(b, b2) && x2 + bx2 > 0 && x2 + bx2 < 10000
                    && y2 + by2 > 0 && y2 + by2 < 10000)
                    {
                        t2 = b2;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            else
            {
                throw new Exception("square equation didn't solve");
            }

            MathLine line1 = MathLine.CreateInstance(x1, y1, x1 + t1.x, y1 + t1.y);
            MathLine line2 = MathLine.CreateInstance(x2, y2, x2 + t2.x, y2 + t2.y);
            MathLine.Intersection(line1, line2, out double x0, out double y0);
            if (x0 < x) x0 = (int)x0; else x0 = (int)x0 + 1;
            if (y0 < y) y0 = (int)y0; else y0 = (int)y0 + 1;
            if (x0 < 0 || x0 > 10000)
                return null;
            if (y0 < 0 || y0 > 10000)
                return null;
            return CreateInstance(x0 - x1, y0 - y1);
        }
    }
}
