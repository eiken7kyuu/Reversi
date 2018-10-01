using System;
using System.Collections.Generic;
using System.Linq;

namespace Reversi
{
    public class Board
    {
        private List<List<Stone>> _board;

        public Stone GetStone((int x, int y) pos)
        {
            if (IsBoardRange(pos))
                return _board[pos.y][pos.x];

            throw new Exception();
        }

        public void SetStone((int x, int y) pos, Stone stone)
        {
            if (IsBoardRange(pos))
            {
                _board[pos.y][pos.x] = stone;
                return;
            }

            throw new Exception();
        }

        /// <summary>
        /// 8方向の位置を取得するための辞書
        /// </summary>
        private readonly Dictionary<string, (int x, int y)> directions = new Dictionary<string, (int, int)>
        {
            { "up", (0, -1) },
            { "down", (0, 1) },
            { "right", (1, 0) },
            { "left", (-1, 0) },
            { "up-right", (1, -1) },
            { "down-right", (1, 1) },
            { "up-left", (-1, -1) },
            { "down-left", (-1, 1) }
        };

        /// <summary>
        /// 黒の石の数
        /// </summary>
        /// <value>The black count.</value>
        public int BlackCount
        {
            get => this._board.Sum(b => b.Count(x => x == Stone.BLACK));
        }

        /// <summary>
        /// 白の石の数
        /// </summary>
        /// <value>The white count.</value>
        public int WhiteCount
        {
            get => this._board.Sum(b => b.Count(x => x == Stone.WHITE));
        }

        /// <summary>
        /// ボードが全部黒か白で埋まっているか調べる
        /// </summary>
        public bool IsFilled
        {
            get => this._board.All(b => b.All(x => x == Stone.BLACK || x == Stone.WHITE));
        }


        /// <summary>
        /// 初期化
        /// </summary>
        public Board()
        {
            this._board = new List<List<Stone>>();
            for (var i = 0; i < 8; i++)
                this._board.Add(new List<Stone>
            {
                Stone.NONE,
                Stone.NONE,
                Stone.NONE,
                Stone.NONE,
                Stone.NONE,
                Stone.NONE,
                Stone.NONE,
                Stone.NONE
            });

            SetStone((3, 3), Stone.WHITE);
            SetStone((4, 4), Stone.WHITE);
            SetStone((4, 3), Stone.BLACK);
            SetStone((3, 4), Stone.BLACK);
        }

        /// <summary>
        /// 置く位置から特定の方向1ライン上にあるマスを取得
        /// </summary>
        public IEnumerable<(int x, int y)> GetLine((int x, int y) position, string direction)
        {
            int nx = position.x + this.directions[direction].x;
            int ny = position.y + this.directions[direction].y;

            while (IsBoardRange((nx, ny)))
            {
                yield return (nx, ny);
                nx += this.directions[direction].x;
                ny += this.directions[direction].y;
            }
        }


        /// <summary>
        /// 置く位置から全方向(8方向)のライン上にあるマスを取得
        /// </summary>
        public IEnumerable<IEnumerable<(int x, int y)>> GetLines((int x, int y) position)
        {
            return directions.Keys.Select(x => this.GetLine(position, x));
        }


        /// <summary>
        /// 8方向のラインをチェックしてひっくり返せる石があるか調べる
        /// 置いた位置から特定の方向に相手の石が1個以上連続したあと、自分の石があればその間の石をひっくり返す石とする
        /// </summary>
        public IEnumerable<(int x, int y)> FindReverseStones((int x, int y) position, Stone myStone)
        {
            var allReverseList = new List<(int x, int y)>();
            if (GetStone(position) != Stone.NONE)
                return allReverseList;

            foreach (var line in GetLines(position).Select(x => x.ToList()))
            {
                var reverseList = new List<(int x, int y)>();
                var i = 0;
                while (line.Count > 0 && line.Count > i && GetStone(line[i]) == GetRivalStone(myStone))
                {
                    reverseList.Add(line[i]);
                    i++;
                }

                var hasReverseStone = reverseList.Count > 0;                    // ひっくり返す石が1つ以上あるか
                Func<bool> isMyStoneColor = () => GetStone(line[i]) == myStone; // iで例外発生する可能性があるので関数
                var isNotSameCount = reverseList.Count != line.Count;           // 同じ数 = 全部相手の石で最後に自分の石がない
                if (hasReverseStone && isNotSameCount && isMyStoneColor())
                {
                    allReverseList.AddRange(reverseList);
                }
            }
            return allReverseList;
        }

        /// <summary>
        /// 全マスを表示
        /// </summary>
        public void ShowBoard()
        {
            Console.WriteLine("  0 1 2 3 4 5 6 7");
            foreach (var row in _board.Select((v, i) => new { masu = v, index = i }))
            {
                Console.Write($"{row.index} ");
                foreach (var m in row.masu)
                {
                    if (m == Stone.BLACK)
                        Console.Write("B ");
                    else if (m == Stone.WHITE)
                        Console.Write("W ");
                    else
                        Console.Write("* ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        /// <summary>
        /// 全マスを一つずつ調べて置けるマスの位置を返す
        /// </summary>
        public IEnumerable<(int x, int y)> GetPutPositions(Stone Stone)
        {
            for (var i = 0; i < 8; i++)
            {
                for (var j = 0; j < 8; j++)
                {
                    if (this.FindReverseStones((x: j, y: i), Stone).Count() > 0)
                        yield return (x: j, y: i);
                }
            }
        }

        /// <summary>
        /// 反対の色取得
        /// </summary>
        private Stone GetRivalStone(Stone Stone)
        {
            return Stone == Stone.BLACK ? Stone.WHITE : Stone.BLACK;
        }

        /// <summary>
        /// xとyがボードの範囲内か調べる
        /// </summary>
        public bool IsBoardRange((int x, int y) pos)
        {
            return pos.x >= 0 && pos.x < 8 && pos.y >= 0 && pos.y < 8;
        }

    }
}
