﻿using System;
using System.Collections.Generic;
using System.Text;

namespace GameClame.Model
{
    public class Position
    {
        public int Row { get; set; }

        public int Column { get; set; }

        public Position(int row, int column)
        {
            Row = row;
            Column = column;
        }
    }
}
