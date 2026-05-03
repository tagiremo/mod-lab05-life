using cli_life;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LifeTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Test1_CellIsAlive()
        {
            Cell cell = new Cell(true);
            Assert.IsTrue(cell.IsAlive);
        }

        [TestMethod]
        public void Test2_CellIsDeadByDefault()
        {
            Cell cell = new Cell();
            Assert.IsFalse(cell.IsAlive);
        }

        [TestMethod]
        public void Test3_SetAlive()
        {
            Board board = new Board(3, 3);
            board.SetAlive(1, 1);
            Assert.IsTrue(board.IsAlive(1, 1));
        }

        [TestMethod]
        public void Test4_SetDead()
        {
            Board board = new Board(3, 3);
            board.SetAlive(1, 1);
            board.SetDead(1, 1);
            Assert.IsFalse(board.IsAlive(1, 1));
        }

        [TestMethod]
        public void Test5_CountAlive()
        {
            Board board = new Board(3, 3);
            board.SetAlive(0, 0);
            board.SetAlive(1, 1);
            Assert.AreEqual(2, board.CountAlive());
        }

        [TestMethod]
        public void Test6_CountNeighbors()
        {
            Board board = new Board(3, 3);
            board.SetAlive(0, 0);
            board.SetAlive(0, 1);
            board.SetAlive(1, 0);
            Assert.AreEqual(3, board.CountNeighbors(1, 1));
        }

        [TestMethod]
        public void Test7_Underpopulation()
        {
            Board board = new Board(3, 3);
            board.SetAlive(1, 1);
            board.NextGeneration();
            Assert.IsFalse(board.IsAlive(1, 1));
        }

        [TestMethod]
        public void Test8_Reproduction()
        {
            Board board = new Board(3, 3);
            board.SetAlive(0, 1);
            board.SetAlive(1, 0);
            board.SetAlive(1, 2);
            board.NextGeneration();
            Assert.IsTrue(board.IsAlive(1, 1));
        }

        [TestMethod]
        public void Test9_SurvivalWithTwoNeighbors()
        {
            Board board = new Board(3, 3);
            board.SetAlive(1, 1);
            board.SetAlive(0, 1);
            board.SetAlive(1, 0);
            board.NextGeneration();
            Assert.IsTrue(board.IsAlive(1, 1));
        }

        [TestMethod]
        public void Test10_SurvivalWithThreeNeighbors()
        {
            Board board = new Board(3, 3);
            board.SetAlive(1, 1);
            board.SetAlive(0, 1);
            board.SetAlive(1, 0);
            board.SetAlive(2, 1);
            board.NextGeneration();
            Assert.IsTrue(board.IsAlive(1, 1));
        }

        [TestMethod]
        public void Test11_Overpopulation()
        {
            Board board = new Board(3, 3);
            board.SetAlive(1, 1);
            board.SetAlive(0, 0);
            board.SetAlive(0, 1);
            board.SetAlive(0, 2);
            board.SetAlive(1, 0);
            board.NextGeneration();
            Assert.IsFalse(board.IsAlive(1, 1));
        }

        [TestMethod]
        public void Test12_BlockStable()
        {
            Board board = new Board(4, 4);
            board.SetAlive(1, 1);
            board.SetAlive(1, 2);
            board.SetAlive(2, 1);
            board.SetAlive(2, 2);
            board.NextGeneration();
            Assert.AreEqual(4, board.CountAlive());
        }

        [TestMethod]
        public void Test13_BlinkerFirstStep()
        {
            Board board = new Board(5, 5);
            board.SetAlive(1, 2);
            board.SetAlive(2, 2);
            board.SetAlive(3, 2);
            board.NextGeneration();
            Assert.IsTrue(board.IsAlive(2, 1));
            Assert.IsTrue(board.IsAlive(2, 2));
            Assert.IsTrue(board.IsAlive(2, 3));
        }

        [TestMethod]
        public void Test14_BlinkerSecondStep()
        {
            Board board = new Board(5, 5);
            board.SetAlive(1, 2);
            board.SetAlive(2, 2);
            board.SetAlive(3, 2);
            board.NextGeneration();
            board.NextGeneration();
            Assert.IsTrue(board.IsAlive(1, 2));
            Assert.IsTrue(board.IsAlive(2, 2));
            Assert.IsTrue(board.IsAlive(3, 2));
        }

        [TestMethod]
        public void Test15_EmptyBoardStaysEmpty()
        {
            Board board = new Board(5, 5);
            board.NextGeneration();
            Assert.AreEqual(0, board.CountAlive());
        }

        [TestMethod]
        public void Test16_SaveAndLoad()
        {
            string path = "life_test.txt";

            Board board = new Board(3, 3);
            board.SetAlive(1, 1);
            board.Save(path);

            Board loaded = new Board(3, 3);
            loaded.Load(path);

            Assert.IsTrue(loaded.IsAlive(1, 1));
        }
    }
}
