using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Khakaton.DataHandler
{
    public class SquareEquation
    {
        public double a { get; private set; }
        public double b { get; private set; }
        public double c { get; private set; }
        public SquareEquation(double a, double b, double c) 
        {
            this.a = a;
            this.b = b;
            this.c = c;
        }

        public bool Solve(out double x1 , out double x2)
        {
            if (a == 0 && b == 0)
            {
                x1 = 0;
                x2 = 0;
                return false;
            }
            if (a == 0)
            {
                x1 = x2 = -c / b;
                return true;
            }
            double Ddiv4 = (b / 2.0) * (b / 2.0) - a * c;
            if (Ddiv4 < 0)
            {
                x1 = 0;
                x2 = 0;
                return false;
            }
            if (Ddiv4 == 0)
            {
                x1 = x2 = (-(b / 2.0)) / a;
            }
            else
            {
                x1 = (-(b / 2.0) - Math.Sqrt(Ddiv4)) / a;
                x2 = (-(b / 2.0) + Math.Sqrt(Ddiv4)) / a;
            }
            return true;
        }
    }
}
