using System;
using System.Collections.Generic;
using System.Text;

namespace Project_K.Models
{
    internal class JidlaModels
    {
        public class Rootobject
        {
            public Day[] days { get; set; }
        }

        public class Day
        {
            public string date { get; set; }
            public Food[] foods { get; set; }
        }

        public class Food
        {
            public int type { get; set; }
            public string name { get; set; }
            public bool ordered { get; set; }
        }
    }
}
