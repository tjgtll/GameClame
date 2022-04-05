﻿using System;
using System.Collections.Generic;
using System.Text;

namespace GameClame.Model
{
    public class ZBlock : Block
    {
        public override int Id => 7;

        protected override Position StartOffset => new Position(0, 3);

        protected override Position[][] Tiles => new Position[][] {
            new Position[] {new Position(0,0), new Position(0,1), new Position(1,1), new Position(1,2)},
            new Position[] {new Position(0,2), new Position(1,1), new Position(1,2), new Position(2,1)},
            new Position[] {new Position(1,0), new Position(1,1), new Position(2,1), new Position(2,2)},
            new Position[] {new Position(0,1), new Position(1,0), new Position(1,1), new Position(2,0)}
        };
    }
}
