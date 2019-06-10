using System;
using System.Collections.Generic;
using System.Text;

namespace Heur
{
    class Calc
    {
        //add product
        public int[] Sum(int[] x, int[] p)
        {
            for (int k = 0; k < 16; k++)
            { x[k] = x[k] + p[k]; }
            return x;
        }

        //subtract product
        public int[] Diff(int[] x, int[] p)
        {
            for (int k = 0; k < 16; k++)
            { x[k] = x[k] - p[k]; }
            return x;
        }

        //return true if there is a nonzero at i, else false
        public bool NonZero(int i, int sum)
        {
            if (((1 << i) & sum) == 0) { return false; }
            return true;
        }

        //returns the positive value of integer v
        public int ReturnPositive(int v)
        {
            if (v < 0) { return (-1 * v); }
            else { return v; }
        }

        //check if both integers are positive or both negative
        public bool SameValue(int x, int y)
        {
            if (x * y < 0) { return false; }
            else { return true; }
        }
    }
}
