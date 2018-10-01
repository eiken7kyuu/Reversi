using System;
using System.Collections.Generic;
using System.Linq;

namespace Reversi
{
    class Player : IPlayer
    {
        private Stone stone { get; }
        public Player(Stone stone)
        {
            this.stone = stone;
        }

        public void Action(Board board)
        {
            while (true)
            {
                Console.WriteLine("");
                if (board.GetPutPositions(this.stone).Count() < 1)
                {
                    Console.WriteLine("あなたは置けなかったのでパス");
                    return;
                }

                ShowPosition(board);

                Console.WriteLine("置く位置を入力してください>>> ");

                (int x, int y) input = Input();
                var IsNotRange = !board.IsBoardRange(input);
                if (IsNotRange || board.FindReverseStones(input, this.stone).Count() < 1)
                {
                    Console.WriteLine("そこは置けません。");
                    continue;
                }

                IEnumerable<(int x, int y)> reverseList = board.FindReverseStones(input, this.stone);
                this.Put(board, (input.x, input.y)); // 指定した場所に置く
                foreach (var pos in reverseList)
                    this.Put(board, pos);

                return;
            }
        }

        /// <summary>
        /// ボードに置く
        /// </summary>
        private void Put(Board board, (int x, int y) position)
        {
            board.SetStone(position, this.stone);
        }

        /// <summary>
        /// 置ける位置を表示する
        /// </summary>
        private void ShowPosition(Board board)
        {
            Console.WriteLine("あなたの置けるマス");
            foreach (var position in board.GetPutPositions(this.stone))
            {
                Console.Write($"({position.x}, {position.y}), ");
            }
        }

        /// <summary>
        /// 置きたい場所をスペース区切りでyとxを入力
        /// </summary>
        private (int x, int y) Input()
        {
            try
            {
                var input = Console.ReadLine().Split(' ').Select(int.Parse).ToList();
                if (input.Count != 2) throw new Exception();
                return (input[0], input[1]);
            }
            catch (Exception e)
            {
                Console.WriteLine("0~7の範囲で、xとyをスペース区切りで入力してくれよな");
                return (9, 9);
            }
        }
    }
}
