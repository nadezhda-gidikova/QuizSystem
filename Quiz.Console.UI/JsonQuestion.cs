﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz.Console.UI
{
   public  class JsonQuestion
    {
        public string Question { get; set; }

        public IEnumerable<JsonAnswer> Answers { get; set; }
    }
}
