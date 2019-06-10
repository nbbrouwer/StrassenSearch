using System;
using System.Collections.Generic;
using System.Text;

namespace Heur
{
    class Prods
    {//in this class are the set ups for the product-matrices
        int n;
        public Prods(int total)
        {
            n = total;
        }

        //fill array with all possible products
        public void CreateProductsJag(ref int[][] P)
        {
            int t = 0;
            int n0 = n;
            P = new int[n][];
            for (int i = 0; i < n; i++)
            {
                P[i] = new int[8];
            }
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
                            P[i + n0 * t][k] = val;
                        }
                        t += 1;
                    }
                }
            }
        }

        //binary sum of the product (array of 16)
        public int[] SumArray(int[][] P)
        {
            int[] sum = new int[n];
            int pow = 1;
            for (int h = 0; h < n; h++)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (P[h][i] != 0)
                    {
                        for (int j = 4; j < 8; j++)
                        {
                            if (P[h][j] != 0) { sum[h] += pow; }
                            pow = pow * 2;
                        }
                    }
                    else
                    {
                        pow = (int)Math.Pow(2, 4 * (i + 1));
                    }
                }
                pow = 1;
            }
            return sum;
        }

        //make sum value 0 of the products we can skip 
        int max = 4; //maximum nr of nonzeroes in product (heuristic)
        public void RemoveProducts(int[][] p, int[] sum)
        {
            for (int i = 0; i < n; i++)
            {   //eerste alpha alleen 1 of 0
                if (p[i][0] == -1) { sum[i] = 0; }
                //eerste beta alleen 1 of 0
                else if (p[i][4] == -1) { sum[i] = 0; }
                //products with all elements of A and B
                else if (sum[i] == 65535) { sum[i] = 0; }
                else //we skip products with more than 4 nonzeroes
                {
                    int l = 0;
                    for (int k = 0; k < 8; k++)
                    {
                        if (p[i][k] != 0) { l += 1; }
                    }
                    if (l > max) { sum[i] = 0; }
                }
            }
        }

        public int[,] JaggedToSquare(int[][] P)
        {
            int[,] products = new int[n, 8];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < 8; j++)
                { products[i, j] = P[i][j]; }
            }
            return products;
        }
    }
}
