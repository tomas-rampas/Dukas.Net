﻿using System.Collections.Generic;
using Bi5.Net.Models;

namespace Bi5.Net.IO
{
    public class OhlcvFileWriter : FileWriter<Bar>
    {
        protected override bool Write(IEnumerable<Bar> data)
        {
            throw new System.NotImplementedException();
        }
    }
}