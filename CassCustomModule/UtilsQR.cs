﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CassCustomModule
{
    class UtilsQR
    {
        public string cleanQR(string value)
        {
            string cleanQR = "";

            cleanQR = value.Replace("$|", " - ");

            return cleanQR;
        }

    }
}
