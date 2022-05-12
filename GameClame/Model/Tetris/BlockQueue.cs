using System;
using System.Collections.Generic;
using System.Text;

namespace GameClame.Tetris.Model
{
    public class BlockQueue
    {
        private readonly Block[] blocks = new Block[]
        {
            new IBlock(),
            new JBlock(),
            new LBlock(),
            new OBlock(),
            new SBlock(),
            new TBlock(),
            new ZBlock()
        };
        
        private readonly int[] randomBlocksInit = { 0, 1, 2, 3, 4, 5, 6 };

        private List<int> randomBlocks = new List<int>();

        private readonly Random random = new Random();

        public Block NextBlock { get; private set; }

        public BlockQueue()
        {
            NextBlock = RandomBlock();
        }

        private Block RandomBlock()
        {
            if (randomBlocks.Count == 0)
            {
                randomBlocks = new List<int>(randomBlocksInit);
            }

            int randomBlock = randomBlocks[random.Next(randomBlocks.Count)];
            randomBlocks.Remove(randomBlock);
            return blocks[randomBlock];
        }

        public Block GetAndUpdate()
        {
            Block block = NextBlock;

            do
            {
                NextBlock = RandomBlock();
            }
            while (block.Id == NextBlock.Id);

            return block;
        }
    }
}
