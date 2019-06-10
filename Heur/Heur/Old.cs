using System;
using System.Collections.Generic;
using System.Text;

namespace Heur
{
    class Old
    {
        int n;
        public void CreateProducts(ref int[,] A)
        {
            int t;
            int n0 = n;
            A = new int[n, 8];
            for (int k = 0; k < 8; k++)
            {
                n0 = n0 / 3;
                t = 0;
                while (t * n0 != n)
                {
                    for (int val = 1; val >= -1; val--)
                    {
                        for (int i = 0; i < n0; i++)
                        {
                            A[i + n0 * t, k] = val;
                        }
                        t += 1;
                    }
                }
            }
        }

        public int[] SumArray(int[,] products)
        {
            int[] sum = new int[n];
            int pow = 1;
            for (int h = 0; h < n; h++)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (products[h, i] != 0)
                    {
                        for (int j = 4; j < 8; j++)
                        {
                            if (products[h, j] != 0) { sum[h] += pow; }
                            pow = pow * 2;
                        }
                    }
                    else
                    {
                        pow = (int)Math.Pow(2, i + 1);
                    }
                }
                pow = 1;
            }
            return sum;
        }


        /*haalt het laatst toegevoegde product uit de lijst en past de x weer aan, passed by reference dus hoeft niet gereturnd
        public void Repair(int[] x, List<int>[] g, int index, int[,] p)
        {
            int last;
            //check eerst of er - of + is gedaan
            if (g[index][g[index].Count - 1] < 0)
            {
                last = -1 * g[index][g[index].Count - 1];
                pro = ProductToArray(p, last);
                x = c.Sum(x, pro);
            }
            else
            {
                last = g[index][g[index].Count - 1];
                pro = ProductToArray(p, last);
                x = c.Diff(x, pro);
            }
            g[index].RemoveAt(g[index].Count - 1);
        }*/
    }
}
