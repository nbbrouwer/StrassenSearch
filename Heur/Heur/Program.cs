using System;

namespace Heur
{
    class Program
    {
        static void Main(string[] args)
        {
            int n = (int)Math.Pow(3, 8); //#total products
            int[,] products = new int[n, 8]; //squared array with all products 
            Search s = new Search(n);
            Prods pcalc = new Prods(n);
            int[][] P = new int[n][]; //Jagged array to make the sorting easier
            pcalc.CreateProductsJag(ref P); //fill jagged array with all products
            int[] sum = pcalc.SumArray(P); //create array with binary sum values
            pcalc.RemoveProducts(P, sum); //remove products
            Array.Sort(sum, P); //sort by ascending sum value
            products = pcalc.JaggedToSquare(P);
            s.C11(products, sum); //start with C11
            Console.WriteLine("Done!");
        }
    }
}
