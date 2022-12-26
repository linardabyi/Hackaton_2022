using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Khakaton.DataHandler
{
    public class MathLine
    {
        public double a { get; private set; }
        public double b { get; private set; }
        public double c { get; private set; }
        private MathLine() { }

        public static MathLine CreateInstance(double x1, double y1, double x2, double y2)
        {
            MathLine mathLine = new MathLine();
            mathLine.a = y2- y1;
            mathLine.b = x1 - x2;
            mathLine.c = (x2 - x1) * y1 - (y2 - y1) * x1;
            return mathLine;
        }

        public static void Intersection(MathLine line1, MathLine line2, out double x, out double y)
        {
            double determinator = line1.a * line2.b - line2.a * line1.b;
            if (determinator == 0)
                throw new Exception("zero determinator. intercetion");
            double xDeterminator = -line1.c * line2.b + line2.c * line1.b;
            double yDeterminator = -line1.a * line2.c + line2.a * line1.c;
            x = xDeterminator / determinator;
            y = yDeterminator / determinator;
        }

        public double DistanceFromPoint(double x0, double y0)
        {
            return Math.Abs(a * x0 + b * y0 + c) / Math.Sqrt(a * a + b * b);
        }
    }
}
