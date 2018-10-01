using System;
using System.Collections.Generic;

namespace Reversi
{
    public class Game
    {
        /// <summary>
        /// 勝敗表示
        /// </summary>
        /// <param name="board"></param>
        public static void ShowResult(Board board)
        {
            Console.WriteLine($"あなたの点数: {board.BlackCount}, CPUの点数: {board.WhiteCount}");
            if (board.BlackCount > board.WhiteCount)
                Console.WriteLine("あなたの勝ち");
            else
                Console.WriteLine("あなたの負け");
        }

        /// <summary>s
        /// メイン処理
        /// </summary>
        public static void Run()
        {
            var board = new Board();
            var players = new List<IPlayer> { new Player(Stone.BLACK), new CPU(Stone.WHITE) };

            board.ShowBoard();

            while (true)
            {
                foreach (IPlayer player in players)
                {
                    player.Action(board);
                    board.ShowBoard();
                }

                if (board.IsFilled)
                    break;
            }
            ShowResult(board);
        }
    }
}
