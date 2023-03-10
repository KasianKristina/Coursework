using System;
using System.Collections.Generic;
using Classes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestCoursework
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        /// <summary>
        /// Тест для проверки метода moveNearbyPosition. Все позиции достижимы.
        /// </summary>
        public void moveNearbyPositionTest1()
        {
            Field GameField = new Field(8, 8);
            Player player1 = new Player(Color.White, ref GameField);
            Player player2 = new Player(Color.Black, ref GameField);
            player1.Сompetitor = player2;
            player2.Сompetitor = player1;

            bool check = player1.queen.moveNearbyPosition(player2.king.offset.Row, player2.king.offset.Column, 1, player1.history);
            Assert.AreEqual(true, check);
        }

        [TestMethod]
        /// <summary>
        /// Тест для проверки метода moveNearbyPosition. Одна возможная позиция.
        /// </summary>
        public void moveNearbyPositionTest2()
        {
            Field GameField = new Field(8, 8);
            Player player1 = new Player(Color.White, ref GameField);
            Player player2 = new Player(Color.Black, ref GameField);
            player1.Сompetitor = player2;
            player2.Сompetitor = player1;

            player2.king.MoveBlock(0, 4);

            bool check = player1.queen.moveNearbyPosition(player2.king.offset.Row, player2.king.offset.Column, 1, player1.history);
            Assert.AreEqual(true, check);
        }

        [TestMethod]
        /// <summary>
        /// Тест для проверки метода moveNearbyPosition. Нет возможных позиций.
        /// </summary>
        public void moveNearbyPositionTest3()
        {
            Field GameField = new Field(8, 8);
            Player player1 = new Player(Color.White, ref GameField);
            Player player2 = new Player(Color.Black, ref GameField);
            player1.Сompetitor = player2;
            player2.Сompetitor = player1;

            player2.king.MoveBlock(1, 4);
            GameField[0, 2] = -5; // стена

            bool check = player1.queen.moveNearbyPosition(player2.king.offset.Row, player2.king.offset.Column, 1, player1.history);
            Assert.AreEqual(false, check);
        }

        [TestMethod]
        /// <summary>
        /// Тест для проверки метода getObstaclesPosition. Есть позиции блокировки.
        /// </summary>
        public void getObstaclesPositionTest()
        {
            Field GameField = new Field(8, 8);
            Player player1 = new Player(Color.White, ref GameField);
            Player player2 = new Player(Color.Black, ref GameField);
            player1.Сompetitor = player2;
            player2.Сompetitor = player1;

            player1.king.MoveBlock(5, 5);
            GameField[6, 7] = -5; // стена
            GameField[6, 5] = -5; // стена

            List<Position> listObstacles = player2.queen.getObstaclesPosition(player1.king.offset.Row, player1.king.offset.Column, player2.Color);
            List<Position> expectedList = new List<Position>() {
                            new Position(6, 6),
                            new Position(6, 4),
                            new Position(6, 3),
                            new Position(6, 2),
                            new Position(6, 1),
                            new Position(6, 0)};
            CollectionAssert.AreEqual(expectedList, listObstacles);
        }

        [TestMethod]
        /// <summary>
        /// Тест для проверки метода getObstaclesPosition. Есть позиции блокировки. Стена преграждает путь.
        /// </summary>
        public void getObstaclesPositionTest2()
        {
            Field GameField = new Field(8, 8);
            Player player1 = new Player(Color.White, ref GameField);
            Player player2 = new Player(Color.Black, ref GameField);
            player1.Сompetitor = player2;
            player2.Сompetitor = player1;

            player1.king.MoveBlock(5, 5);
            GameField[6, 7] = -5; // стена
            GameField[6, 5] = -5; // стена
            GameField[6, 2] = -5; // стена

            List<Position> listObstacles = player2.queen.getObstaclesPosition(player1.king.offset.Row, player1.king.offset.Column, player2.Color);
            List<Position> expectedList = new List<Position>() {
                            new Position(6, 6),
                            new Position(6, 4),
                            new Position(6, 3) };
            CollectionAssert.AreEqual(expectedList, listObstacles);
        }

        [TestMethod]
        /// <summary>
        /// Тест для проверки метода getObstaclesPosition. Нет позиций блокировки.
        /// </summary>
        public void getObstaclesPositionTest3()
        {
            Field GameField = new Field(8, 8);
            Player player1 = new Player(Color.White, ref GameField);
            Player player2 = new Player(Color.Black, ref GameField);
            player1.Сompetitor = player2;
            player2.Сompetitor = player1;

            player1.king.MoveBlock(5, 5);
            GameField[6, 7] = -5; // стена
            GameField[6, 6] = -5;
            GameField[6, 4] = -5;
            GameField[6, 3] = -5;
            GameField[6, 2] = -5;
            GameField[6, 1] = -5;
            GameField[6, 0] = -5;

            List<Position> listObstacles = player2.queen.getObstaclesPosition(player1.king.offset.Row, player1.king.offset.Column, player2.Color);
            List<Position> expectedList = new List<Position>() { };
            CollectionAssert.AreEqual(expectedList, listObstacles);
        }
    }
}
