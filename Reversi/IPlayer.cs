using System;
using System.Collections.Generic;
using System.Text;

namespace Reversi
{
    interface IPlayer
    {
        /// <summary>
        /// プレイヤーの行動。
        /// ボードを使って石を置いたりする
        /// </summary>
        /// <param name="board"></param>
        void Action(Board board);
    }
}
