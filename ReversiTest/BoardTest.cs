using System;
using Xunit;
using Reversi;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

namespace ReversiTest
{
    public class BoardTest
    {
        private Board board;

        public BoardTest()
        {
            this.board = new Board();
        }

        private void BoardAllBlack()
        {
            for (var i = 0; i < 8; i++)
            {
                for (var j = 0; j < 8; j++)
                {
                    board.SetStone((j, i), Stone.BLACK);
                }
            }
        }

        [Fact(DisplayName = "ボードが埋まっていること")]
        public void IsFilledTest()
        {
            Assert.False(board.IsFilled);
            BoardAllBlack();
            Assert.True(board.IsFilled);
        }

        [Fact(DisplayName = "石の数が集計できること")]
        public void DiskCountTest()
        {
            Assert.Equal(2, board.WhiteCount);
            Assert.Equal(2, board.BlackCount);
            BoardAllBlack();
            Assert.Equal(0, board.WhiteCount);
            Assert.Equal(64, board.BlackCount);
        }

        [Fact(DisplayName = "石を置ける場所が返ってくること")]
        public void GetCanPutStonesTest()
        {
            var list = board.GetPutPositions(Stone.BLACK).ToList();
            Assert.Equal((3, 2), list[0]);
            Assert.Equal((2, 3), list[1]);
            Assert.Equal((5, 4), list[2]);
            Assert.Equal((4, 5), list[3]);
            BoardAllBlack();
            var list2 = board.GetPutPositions(Stone.BLACK);
            Assert.Empty(list2);
        }

        [Fact(DisplayName = "指定した位置から8方向の位置を取得できること")]
        public void GetLinesTest()
        {
            var pos1 = board.GetLines((2, 5)).ToList();
            Assert.Equal(pos1[0], new List<(int, int)> { (2, 4), (2, 3), (2, 2), (2, 1), (2, 0) });
            Assert.Equal(pos1[1], new List<(int, int)> { (2, 6), (2, 7) });
            Assert.Equal(pos1[2], new List<(int, int)> { (3, 5), (4, 5), (5, 5), (6, 5), (7, 5) });
            Assert.Equal(pos1[3], new List<(int, int)> { (1, 5), (0, 5) });
            Assert.Equal(pos1[4], new List<(int, int)> { (3, 4), (4, 3), (5, 2), (6, 1), (7, 0) });
            Assert.Equal(pos1[5], new List<(int, int)> { (3, 6), (4, 7) });
            Assert.Equal(pos1[6], new List<(int, int)> { (1, 4), (0, 3) });
            Assert.Equal(pos1[7], new List<(int, int)> { (1, 6), (0, 7) });

            var pos2 = board.GetLines((0, 0)).ToList();
            Assert.Empty(pos2[0]);
            Assert.Equal(pos2[1], new List<(int, int)> { (0, 1), (0, 2), (0, 3), (0, 4), (0, 5), (0, 6), (0, 7) });
            Assert.Equal(pos2[2], new List<(int, int)> { (1, 0), (2, 0), (3, 0), (4, 0), (5, 0), (6, 0), (7, 0) });
            Assert.Empty(pos2[3]);
            Assert.Empty(pos2[4]);
            Assert.Equal(pos2[5], new List<(int, int)> { (1, 1), (2, 2), (3, 3), (4, 4), (5, 5), (6, 6), (7, 7) });
            Assert.Empty(pos2[6]);
            Assert.Empty(pos2[7]);
        }

        [Fact(DisplayName = "指定した位置がボードの範囲内")]
        public void IsBoardRangeTest()
        {
            Assert.True(board.IsBoardRange((0, 0)));
            Assert.True(board.IsBoardRange((7, 7)));
            Assert.True(board.IsBoardRange((0, 7)));
            Assert.False(board.IsBoardRange((0, -1)));
            Assert.False(board.IsBoardRange((-1, 0)));
            Assert.False(board.IsBoardRange((-1, -1)));
            Assert.False(board.IsBoardRange((-99, -99)));
            Assert.False(board.IsBoardRange((7, 8)));
            Assert.False(board.IsBoardRange((8, 7)));
            Assert.False(board.IsBoardRange((8, 8)));
            Assert.False(board.IsBoardRange((99, 99)));
            Assert.False(board.IsBoardRange((99, -99)));
        }
    }
}
