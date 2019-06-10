using System;
using System.Collections.Generic;
using System.Text;

namespace Heur
{
    class Location
    {
        public bool gebruikte; //true: search first in used products
        public int waar; //at what product doe we have to continue the search
        public Location()
        {
            gebruikte = true;
            waar = 0;
        }
    }
}
