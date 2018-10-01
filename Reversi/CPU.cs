using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Reversi
{
    class CPU : IPlayer
    {
        private Stone stone { get; }
        private Random ran = new Random();

        public CPU(Stone stone)
        {
            this.stone = stone;
        }

        public void Action(Board board)
        {
            IEnumerable<(int x, int y)> positions = board.GetPutPositions(this.stone);
            if (positions.Count() < 1)
            {
                Console.WriteLine("cpuは置けなかったのでパス");
                return;
            }

            // 置ける場所の中からランダムに選ぶ
            var position = positions.ToList()[this.ran.Next(positions.Count() - 1)];
            Console.WriteLine($"CPUは({position.x}, {position.y})に置いた");

            IEnumerable<(int x, int y)> reverseList = board.FindReverseStones(position, this.stone);
            this.Put(board, position);
            foreach (var pos in reverseList)
                this.Put(board, pos);
        }

        /// <summary>
        /// ボードに置く
        /// </summary>
        private void Put(Board board, (int x, int y) position)
        {
            board.SetStone(position, this.stone);
        }
    }
}
