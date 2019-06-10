using System;
using System.Collections.Generic;
using System.Linq;

namespace Heur
{
    class Search
    {//all searching is done in this class
        int n;
        List<int>[] gebruikt; //keeps track of used products per element C 
        double percent; //to keep track of how much is done of the search tree at level 0 
        int a0, a1, a2, a3; //store how many products are used so far 
        int count; //count how much solutions have been found 
        Calc c;
        int s; //to calculate the sum of elements 
        public Search(int Totaal)
        {
            n = Totaal;
            gebruikt = new List<int>[4];
            for (int i = 0; i < 4; i++)
            { gebruikt[i] = new List<int>(); }
            count = 1;
            c = new Calc();
        }

        //transforms a product of 8 elements in an array of 16 
        public int[] ProductToArray(int[,] product, int nr)
        {
            //first 4 elements predict which row a nonzero has to come, last 4 which column 
            int[] total = new int[16];
            for (int i = 0; i < 4; i++)
            {
                if (product[nr, i] != 0)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        if (product[nr, 4 + j] != 0)
                        {
                            total[i * 4 + j] = product[nr, i] * product[nr, 4 + j];
                        }
                    }
                }
            }
            return total;
        }

        int[] pro = new int[16];
        public void C11(int[,] p, int[] som)
        { //c11 = ae + bg, ae if p[0] = 1, bg if p[6] = 1 
            int[] x = new int[16];
            for (int i = 3815; i < n; i++)
            {
                if (som[i] != 0) //skip products with sum value = 0  
                {
                    if (c.NonZero(0, som[i]))
                    {
                        if (c.NonZero(6, som[i]) && c.SameValue(p[i, 0] * p[i, 4], p[i, 1] * p[i, 6]))
                        { //both 0 and 6 have nonzero, but have to be both positive or both negative 
                            pro = ProductToArray(p, i); //get the array of 16 
                            if (pro[0] > 0)
                            {
                                x = c.Sum(x, pro);
                                gebruikt[0].Add(i);
                            }
                            else
                            {
                                x = c.Diff(x, pro);
                                gebruikt[0].Add(-i);
                            }
                            a0 += 1;
                            Combine(x, p, 0, 6, 0, som, a0);
                            Reset(ref gebruikt, 0, ref x);
                            a0 = 0;
                        }
                        else
                        { //search new one for bg
                            for (int j = i; j < n; j++)
                            {
                                if (som[j] != 0 && c.NonZero(6, som[j]))
                                { //nonzero at 6, but already have +1 on x[0] so we now want pro[0] = 0
                                    if (!c.NonZero(0, som[j]))
                                    {
                                        pro = ProductToArray(p, j);
                                        if (pro[6] > 0)
                                        {
                                            x = c.Sum(x, pro);
                                            gebruikt[0].Add(j);
                                        }
                                        else
                                        {
                                            x = c.Diff(x, pro);
                                            gebruikt[0].Add(-j);
                                        }
                                        pro = ProductToArray(p, i);
                                        if (pro[0] > 0)
                                        {
                                            x = c.Sum(x, pro);
                                            gebruikt[0].Add(i);
                                        }
                                        else
                                        {
                                            x = c.Diff(x, pro);
                                            gebruikt[0].Add(-i);
                                        }
                                        a0 += 2;
                                        Combine(x, p, 0, 6, 0, som, a0);
                                        Reset(ref gebruikt, 0, ref x);
                                        a0 = 0;
                                    }
                                }
                            }
                        }
                    }
                    else if (c.NonZero(6, som[i]))
                    { //een +1 of -1 at 6 -> bg 
                        //search new product for ea 
                        for (int j = i; j < n; j++)
                        {
                            if (som[j] != 0 && c.NonZero(0, som[j]))
                            { //but with a zero on pro[6] 
                                if (!c.NonZero(6, som[j]))
                                {
                                    pro = ProductToArray(p, j);
                                    if (pro[0] > 0)
                                    {
                                        x = c.Sum(x, pro);
                                        gebruikt[0].Add(j);
                                    }
                                    else
                                    {
                                        x = c.Diff(x, pro);
                                        gebruikt[0].Add(-j);
                                    }
                                    pro = ProductToArray(p, i);
                                    if (pro[6] > 0)
                                    {
                                        x = c.Sum(x, pro);
                                        gebruikt[0].Add(i);
                                    }
                                    else
                                    {
                                        x = c.Diff(x, pro);
                                        gebruikt[0].Add(-i);
                                    }
                                    a0 += 2;
                                    Combine(x, p, 0, 6, 0, som, a0);
                                    Reset(ref gebruikt, 0, ref x);
                                    a0 = 0;
                                }
                            }
                        }
                    }
                }
            }
        }

        public void C12(int[,] p, int[] som)
        { //c12 = af + bh, af at index 1, bh at index 7 
            int tot = a1; //dummy variabele for #used so far 
            int[] x = new int[16];
            for (int i = 0; i < n; i++)
            {
                if (som[i] != 0)
                {
                    if (c.NonZero(1, som[i]))
                    { //here we have af 
                        if (c.NonZero(7, som[i]) && c.SameValue(p[i, 0] * p[i, 5], p[i, 1] * p[i, 7]))
                        { //now we have both af and bh 
                            //only add to total if product is not already used 
                            if (gebruikt[0].Contains(i) == false && gebruikt[0].Contains(-i) == false) { tot += 1; }
                            if (tot <= 7)
                            {
                                pro = ProductToArray(p, i);
                                if (pro[1] > 0)
                                {
                                    x = c.Sum(x, pro);
                                    gebruikt[1].Add(i);
                                }
                                else
                                {
                                    x = c.Diff(x, pro);
                                    gebruikt[1].Add(-i);
                                }
                                Combine(x, p, 1, 7, 1, som, tot);
                                Reset(ref gebruikt, 1, ref x);
                            }
                            tot = a1;
                        }
                        else
                        { //search for new one with bh 
                            for (int j = i; j < n; j++)
                            {
                                if (som[j] != 0 && c.NonZero(7, som[j]))
                                { //has to be a zero at 1 
                                    if (!c.NonZero(1, som[j]))
                                    {
                                        if (!gebruikt[0].Contains(i) && !gebruikt[0].Contains(-i)) { tot += 1; }
                                        if (!gebruikt[0].Contains(j) && !gebruikt[0].Contains(-j)) { tot += 1; }
                                        if (tot <= 7)
                                        {
                                            pro = ProductToArray(p, j);
                                            if (pro[7] > 0)
                                            {
                                                x = c.Sum(x, pro);
                                                gebruikt[1].Add(j);
                                            }
                                            else
                                            {
                                                x = c.Diff(x, pro);
                                                gebruikt[1].Add(-j);
                                            }
                                            pro = ProductToArray(p, i);
                                            if (pro[1] > 0)
                                            {
                                                x = c.Sum(x, pro);
                                                gebruikt[1].Add(i);
                                            }
                                            else
                                            {
                                                x = c.Diff(x, pro);
                                                gebruikt[1].Add(-i);
                                            }
                                            Combine(x, p, 1, 7, 1, som, tot);
                                            Reset(ref gebruikt, 1, ref x);
                                        }
                                        tot = a1;
                                    }
                                }
                            }
                        }
                    }
                    else if (c.NonZero(7, som[i]))
                    { //here we have bh, search for af 
                        for (int j = i; j < n; j++)
                        {
                            if (som[j] != 0 && c.NonZero(1, som[j]))
                            { //we want that pro[7] = 0 
                                if (!c.NonZero(7, som[j]))
                                {
                                    if (!gebruikt[0].Contains(i) && !gebruikt[0].Contains(-i)) { tot += 1; }
                                    if (!gebruikt[0].Contains(j) && !gebruikt[0].Contains(-j)) { tot += 1; }
                                    if (tot <= 7)
                                    {
                                        pro = ProductToArray(p, j);
                                        if (pro[1] > 0)
                                        {
                                            x = c.Sum(x, pro);
                                            gebruikt[1].Add(j);
                                        }
                                        else
                                        {
                                            x = c.Diff(x, pro);
                                            gebruikt[1].Add(-j);
                                        }
                                        pro = ProductToArray(p, i);
                                        if (pro[7] > 0)
                                        {
                                            x = c.Sum(x, pro);
                                            gebruikt[1].Add(i);
                                        }
                                        else
                                        {
                                            x = c.Diff(x, pro);
                                            gebruikt[1].Add(-i);
                                        }
                                        Combine(x, p, 1, 7, 1, som, tot);
                                        Reset(ref gebruikt, 1, ref x);
                                    }
                                    tot = a1;
                                }
                            }
                        }
                    }
                }
            }
        }

        public void C21(int[,] p, int[] som)
        { //c21 = ce + dg, ce at p[8], dg at p[14] 
            int tot = a2;
            int[] x = new int[16];
            var hs = new HashSet<int>();
            for (int i = 0; i < 2; i++)
            {
                hs.UnionWith(gebruikt[i]);
            }
            List<int> used = hs.ToList();
            for (int i = 0; i < n; i++)
            {
                if (som[i] != 0)
                {
                    if (c.NonZero(8, som[i]))
                    { //here we have ce 
                        if (c.NonZero(14, som[i]) && c.SameValue(p[i, 2] * p[i, 4], p[i, 3] * p[i, 6]))
                        { //now we have both, but both need to be either positive or negative 
                            if (used.Contains(i) == false) { tot += 1; }
                            if (tot <= 7)
                            {
                                pro = ProductToArray(p, i);
                                if (pro[8] > 0)
                                {
                                    x = c.Sum(x, pro);
                                    gebruikt[2].Add(i);
                                }
                                else
                                {
                                    x = c.Diff(x, pro);
                                    gebruikt[2].Add(-i);
                                }
                                Combine(x, p, 8, 14, 2, som, tot);
                                Reset(ref gebruikt, 2, ref x);
                            }
                            tot = a2;
                        }
                        else
                        { //search for new product 
                            for (int j = i; j < n; j++)
                            {
                                if (som[j] != 0 && c.NonZero(14, som[j]))
                                { //now dg, but we need pro[8] = 0 
                                    if (!c.NonZero(8, som[j]))
                                    {
                                        if (!used.Contains(i) && !used.Contains(-i)) { tot += 1; }
                                        if (!used.Contains(j) && !used.Contains(-j)) { tot += 1; }
                                        if (tot <= 7)
                                        {
                                            pro = ProductToArray(p, j);
                                            if (pro[14] > 0)
                                            {
                                                x = c.Sum(x, pro);
                                                gebruikt[2].Add(j);
                                            }
                                            else
                                            {
                                                x = c.Diff(x, pro);
                                                gebruikt[2].Add(-j);
                                            }
                                            pro = ProductToArray(p, i);
                                            if (pro[8] > 0)
                                            {
                                                x = c.Sum(x, pro);
                                                gebruikt[2].Add(i);
                                            }
                                            else
                                            {
                                                x = c.Diff(x, pro);
                                                gebruikt[2].Add(-i);
                                            }
                                            Combine(x, p, 8, 14, 2, som, tot);
                                            Reset(ref gebruikt, 2, ref x);
                                        }
                                        tot = a2;
                                    }
                                }
                            }
                        }
                    }
                    else if (c.NonZero(14, som[i]))
                    { //here dg 
                        for (int j = i; j < n; j++)
                        {
                            if (som[j] != 0 && c.NonZero(8, som[j]))
                            {
                                if (!c.NonZero(14, som[j]))
                                { //only add to total if not already used 
                                    if (!used.Contains(i) && !used.Contains(-i)) { tot += 1; }
                                    if (!used.Contains(j) && !used.Contains(-j)) { tot += 1; }
                                    if (tot <= 7)
                                    {
                                        pro = ProductToArray(p, j);
                                        if (pro[8] > 0)
                                        {
                                            x = c.Sum(x, pro);
                                            gebruikt[2].Add(j);
                                        }
                                        else
                                        {
                                            x = c.Diff(x, pro);
                                            gebruikt[2].Add(-j);
                                        }
                                        pro = ProductToArray(p, i);
                                        if (pro[14] > 0)
                                        {
                                            x = c.Sum(x, pro);
                                            gebruikt[2].Add(i);
                                        }
                                        else
                                        {
                                            x = c.Diff(x, pro);
                                            gebruikt[2].Add(-i);
                                        }
                                        Combine(x, p, 8, 14, 2, som, tot);
                                        Reset(ref gebruikt, 2, ref x);
                                    }
                                    tot = a2;
                                }
                            }
                        }
                    }
                }
            }
        }

        public void C22(int[,] p, int[] som)
        {  //c22 = cf + dh, cf at p[9], dh at p[15] 
            int[] x = new int[16];
            int tot = a3;
            var hs = new HashSet<int>();
            for (int i = 0; i < 3; i++)
            {
                hs.UnionWith(gebruikt[i]);
            }
            List<int> used = hs.ToList();
            for (int i = 0; i < n; i++)
            {
                if (som[i] != 0)
                {
                    if (c.NonZero(9, som[i]))
                    { //now cf 
                        if (c.NonZero(15, som[i]) && c.SameValue(p[i, 2] * p[i, 5], p[i, 3] * p[i, 7]))
                        { //and dh, if both elements have the same sign 
                            if (!used.Contains(i) && !used.Contains(-i)) { tot += 1; }
                            if (tot <= 7)
                            {
                                pro = ProductToArray(p, i);
                                if (pro[9] > 0)
                                {
                                    x = c.Sum(x, pro);
                                    gebruikt[3].Add(i);
                                }
                                else
                                {
                                    x = c.Diff(x, pro);
                                    gebruikt[3].Add(-i);
                                }
                                Combine(x, p, 9, 15, 3, som, tot);
                                Reset(ref gebruikt, 3, ref x);
                            }
                            tot = a3;
                        }
                        else
                        { //search new product 
                            for (int j = i; j < n; j++)
                            {
                                if (som[j] != 0 && c.NonZero(15, som[j]))
                                { //now dh, but pro[9] = 0 
                                    if (!c.NonZero(9, som[j]))
                                    {
                                        if (!used.Contains(i) && !used.Contains(-i)) { tot += 1; }
                                        if (!used.Contains(j) && !used.Contains(-j)) { tot += 1; }
                                        if (tot <= 7)
                                        {
                                            pro = ProductToArray(p, j);
                                            if (pro[15] > 0)
                                            {
                                                x = c.Sum(x, pro);
                                                gebruikt[3].Add(j);
                                            }
                                            else
                                            {
                                                x = c.Diff(x, pro);
                                                gebruikt[3].Add(-j);
                                            }
                                            pro = ProductToArray(p, i);
                                            if (pro[9] > 0)
                                            {
                                                x = c.Sum(x, pro);
                                                gebruikt[3].Add(i);
                                            }
                                            else
                                            {
                                                x = c.Diff(x, pro);
                                                gebruikt[3].Add(-i);
                                            }
                                            Combine(x, p, 9, 15, 3, som, tot);
                                            Reset(ref gebruikt, 3, ref x);
                                        }
                                        tot = a3;
                                    }
                                }
                            }
                        }
                    }
                    else if (c.NonZero(15, som[i]))
                    { //now dh 
                        for (int j = i; j < n; j++)
                        {
                            if (som[j] != 0 && c.NonZero(9, som[j]))
                            {  //and cf, but pro[15] = 0 
                                if (!c.NonZero(15, som[j]))
                                {
                                    if (!used.Contains(i) && !used.Contains(-i)) { tot += 1; }
                                    if (!used.Contains(j) && !used.Contains(-j)) { tot += 1; }
                                    if (tot <= 7)
                                    {
                                        pro = ProductToArray(p, j);
                                        if (pro[9] > 0) //op j staat cf 
                                        {
                                            x = c.Sum(x, pro);
                                            gebruikt[3].Add(j);
                                        }
                                        else
                                        {
                                            x = c.Diff(x, pro);
                                            gebruikt[3].Add(-j);
                                        }
                                        pro = ProductToArray(p, i);
                                        if (pro[15] > 0)
                                        {
                                            x = c.Sum(x, pro);
                                            gebruikt[3].Add(i);
                                        }
                                        else
                                        {
                                            x = c.Diff(x, pro);
                                            gebruikt[3].Add(-i);
                                        }
                                        Combine(x, p, 9, 15, 3, som, tot);
                                        Reset(ref gebruikt, 3, ref x);
                                    }
                                    tot = a3;
                                }
                            }
                        }
                    }
                }
            }
        }

        public void Combine(int[] x, int[,] p, int i1, int i2, int index, int[] som, int aant)
        {
            bool done = true;
            for (int i = 0; i < 16; i++)
            {
                if (x[i] != 0 && i != i1 && i != i2)
                { //nonzero where we want a zero
                    done = false; break;
                }
            }
            if (done)
            { //target is found 
                if (index == 3)
                {
                    Done(gebruikt); return;
                }
                if (index == 0)
                {
                    a1 = aant;
                    C12(p, som);
                    return;
                }
                else if (index == 1)
                {
                    a2 = aant;
                    C21(p, som);
                    return;
                }
                else
                {
                    a3 = aant;
                    C22(p, som);
                    return;
                }
            }
            else
            {
                List<int>[] g = new List<int>[4]; //dummylist 
                g = CopyList(g, gebruikt, index);
                int amnt = aant; //dummy total #used 
                Location[] loc = new Location[8];
                for (int i = 0; i < 8; i++)
                {
                    loc[i] = new Location();
                    if (index == 0)
                    { loc[i].gebruikte = false; }
                }
                RecCombine(amnt, x, p, ref g, index, ref loc, som, i1, i2);
            }
        }
        //recursively add/subtract products
        public void RecCombine(int amount, int[] xn, int[,] p, ref List<int>[] g, int index, ref Location[] loc, int[] som, int i1, int i2)
        {
            bool done = true;
            for (int i = 0; i < 16; i++)
            {
                if (xn[i] != 0 && i != i1 && i != i2)
                { //a nonzero at a index we want a zero
                    done = false; break;
                }
            }
            if (done)
            { //array x is equal to target
                if (index == 3)
                { Done(g); return; }
                else
                {
                    gebruikt = CopyList(gebruikt, g, index);
                    if (index == 0)
                    {
                        a1 = amount;
                        C12(p, som);
                        return;
                    }
                    else if (index == 1)
                    {
                        a2 = amount;
                        C21(p, som);
                        return;
                    }
                    else
                    {
                        a3 = amount;
                        C22(p, som);
                        return;
                    }
                }
            } //stop if we have 7 products and goal is not reached 
            if (amount == 7) { return; }
            //stop when there are too much nonzero's to cancel, only when amount > 3 
            if (amount > 3)
            {
                int x; s = 0;
                for (int i = 0; i < 16; i++)
                {
                    x = c.ReturnPositive(xn[i]);
                    if (x > 7 - amount) { return; }
                    s += x;
                }
                if (s > 4 * (7 - amount) + 2)
                {
                    loc[amount].waar += 1;
                    return;
                }
            }
            else if (loc[amount].waar > n - 1)
            { //tried everything on this level  
                for (int k = amount; k < 8; k++)
                { loc[k].waar = loc[k - 1].waar; }
                return;
            }
            //dummy variables 
            int[] xdummy = new int[16];
            xdummy = CopyArray(xdummy, xn);
            int totaal = amount;
            List<int>[] gdummy = new List<int>[4];
            gdummy = CopyList(gdummy, g, index);
            int product;
            if (loc[amount].gebruikte)
            {
                if (loc[amount].waar == amount - g[index].Count)
                { //tried all used products 
                    for (int l = amount; l < 8; l++)
                    {
                        loc[l].gebruikte = false;
                        loc[l].waar = 0;
                    }
                }
                else
                {
                    var hs = new HashSet<int>();
                    for (int i = 0; i < index; i++)
                    { hs.UnionWith(g[i]); }
                    List<int> total = hs.ToList();
                    //search further in used products 
                    for (int i = loc[amount].waar; i < amount - g[index].Count; i++)
                    { //test from given location 
                        product = c.ReturnPositive(total[i]);
                        if (!c.NonZero(i1, som[product]) && !c.NonZero(i2, som[product]))
                        {
                            pro = ProductToArray(p, product);
                            xn = c.Sum(xn, pro);
                            g[index].Add(product);
                            loc[amount].waar = product + 1;
                            RecCombine(amount, xn, p, ref g, index, ref loc, som, i1, i2);
                            //or subtract 
                            xn = CopyArray(xn, xdummy); g = CopyList(g, gdummy, index);
                            xn = c.Diff(xn, pro);
                            g[index].Add(-product);
                            RecCombine(amount, xn, p, ref g, index, ref loc, som, i1, i2);
                            //repairen before continuing 
                            amount = totaal;
                            xn = CopyArray(xn, xdummy);
                            g = CopyList(g, gdummy, index);
                        }
                        for (int l = amount; l < 8; l++)
                        {//if it didn’t work with i, skip from now on 
                            if (loc[amount].gebruikte)
                            { loc[l].waar = i + 1; }
                        }
                    }
                }
            }
            if (loc[amount].gebruikte == false)
            {
                for (int i = loc[amount].waar; i < n; i++)
                {
                    if (som[i] != 0 && g[index].Contains(i) == false)
                    {
                        if (!c.NonZero(i1, som[i]) && !c.NonZero(i2, som[i]))
                        {
                            pro = ProductToArray(p, i);
                            xn = c.Sum(xn, pro);
                            g[index].Add(i);
                            loc[amount].waar = i + 1;
                            amount += 1;
                            RecCombine(amount, xn, p, ref g, index, ref loc, som, i1, i2);
                            //or subtract the product 
                            xn = CopyArray(xn, xdummy);
                            xn = c.Diff(xn, pro);
                            g = CopyList(g, gdummy, index); g[index].Add(-i);
                            amount = totaal + 1;
                            RecCombine(amount, xn, p, ref g, index, ref loc, som, i1, i2);
                            //repair before continuing 
                            xn = CopyArray(xn, xdummy);
                            g = CopyList(g, gdummy, index);
                            amount = totaal;
                        }
                    }
                    for (int l = amount; l < 8; l++)
                    { loc[l].waar = i + 1; }
                }
            }
        }

        //reset for one element of C 
        public void Reset(ref List<int>[] g, int index, ref int[] x)
        {
            x = new int[16];
            g[index].Clear();
        }

        //makes a copy of array 
        public int[] CopyArray(int[] dummy, int[] c)
        {
            for (int i = 0; i < 16; i++)
            { dummy[i] = c[i]; }
            return dummy;
        }

        //copies the list 
        public List<int>[] CopyList(List<int>[] dummy, List<int>[] a, int index)
        {
            for (int i = 0; i <= index; i++)
            {
                dummy[i] = new List<int>();
                dummy[i].AddRange(a[i]);
            }
            return dummy;
        }

        //writes the solution when target is found 
        public void Done(List<int>[] g)
        {
            Console.WriteLine("Found solution nr {0}:", count);
            for (int i = 0; i < 4; i++)
            {
                foreach (int j in g[i])
                { Console.Write("{0} ", j); }
                Console.Write("\n");
            }
            Console.ReadKey();
            count += 1;
        }
    }
}
