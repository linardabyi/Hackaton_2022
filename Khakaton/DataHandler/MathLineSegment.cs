using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Khakaton.DataHandler
{
    public class MathLineSegment
    {
        public double x1 { get; private set; }
        public double x2 { get; private set; }
        public double y1 { get; private set; }
        public double y2 { get; private set; }

        public MathLineSegment(double x1, double y1, double x2, double y2)
        {
            this.x1 = x1;
            this.x2 = x2;
            this.y1 = y1;
            this.y2 = y2;
        }

        public bool IntersectCircle(double x, double y, double r)
        {
            MathLine mathLine = MathLine.CreateInstance(x1, y1, x2, y2);
            if (mathLine.DistanceFromPoint(x, y) > r)
                return false;
            double a = mathLine.a, b = mathLine.b, c = mathLine.c;
            double sqA = a * a + b * b;
            double sqB = 2 * (a * c + a * b * y - b * b * x);
            double sqC = b * b * x * x + c * c
                + b * b * y * y + 2 * b * c * y - b * b * r * r;
            SquareEquation squareEquation = new SquareEquation(sqA, sqB, sqC);
            squareEquation.Solve(out double xd1, out double xd2);
            if (xd1 < x1 && xd1 > x2 || xd1 > x1 && xd1 < x2)
                return true;
            if (xd2 < x1 && xd2 > x2 || xd2 > x1 && xd2 < x2)
                return true;
            return false;
        }
    }
}
