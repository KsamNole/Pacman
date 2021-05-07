using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using NUnit.Framework;

namespace Pacman
{
    [TestFixture]
    
    public class Tests
    {
        private readonly Field field = new Field();
        private readonly Gamer gamer = new Gamer();
        private readonly Enemy enemy = new Enemy();
        private readonly RandomBox box = new RandomBox();

        [TestCase(100, 50, 150, 50)]
        [TestCase(300, 50, 250, 50)]
        [TestCase(50, 100, 150, 50)]
        [TestCase(50, 50, 150, 50)]
        public void EnemyMoving(int playerX, int playerY, int resultX, int resultY)
        {
            field.FillField();
            enemy.EnemyPicRed.Location = new Point(200, 50);
            gamer.PlayerPic.Location = new Point(playerX, playerY);
            Enemy.Moving(gamer, field.Dic, enemy.EnemyPicRed);
            Assert.AreEqual(new Point(resultX, resultY), enemy.EnemyPicRed.Location);
        }
        
        [TestCase(false,0, 0)]
        [TestCase(false, -1, -1)]
        [TestCase(false, 0, 200000000)]
        [TestCase(true, 50 ,100)]
        public void CanMove(bool expected, int playerX, int playerY)
        {
            field.FillField();
            Assert.AreEqual(expected, gamer.CanMove(field.Dic, playerX, playerY));
        }

        [TestCase(300, 50,"Right")]
        [TestCase(50, 50,"Left")]
        [TestCase(50, 250,"Down")]
        [TestCase(50, 50,"Up")]
        public void AutoMovingPlayer(int expectedX, int expectedY,string direction)
        {
            field.FillField();
            AutoMovePlayer(direction);
            Assert.AreEqual(new Point(expectedX, expectedY), gamer.PlayerPic.Location);
        }

        [TestCase("Right", Keys.D, 150, 150)]
        [TestCase("Left", Keys.A, 150, 150)]
        [TestCase("Down", Keys.S, 150, 150)]
        [TestCase("Up", Keys.W, 150, 150)]
        [TestCase(null, Keys.A, 50, 50)]
        [TestCase(null, Keys.W, 50, 50)]
        public void DirectionPlayer(string expected, Keys key, int playerX, int playerY)
        {
            gamer.PlayerPic.Location = new Point(playerX, playerY);
            field.FillField();
            Assert.AreEqual(expected, gamer.DirectionPlayer(field, gamer, new KeyEventArgs(key), null));
        }

        [TestCase(300 , 50, 300, 150, Keys.D, Keys.S)]
        [TestCase(50 , 250, 150, 250, Keys.S, Keys.D)]
        [TestCase(300 , 50, 50, 50, Keys.D, Keys.A)]
        public void IntegrationTest(int expectedX, int expectedY, int expectedX2, int expectedY2, Keys key, Keys key2)
        {
            gamer.PlayerPic.Location = new Point(50, 50);
            field.FillField();
            var direction = gamer.DirectionPlayer(field, gamer, new KeyEventArgs(key), null); // 1. Выбираем направление
            AutoMovePlayer(direction); // 2. Идем в одну сторону
            Assert.AreEqual(new Point(expectedX, expectedY), gamer.PlayerPic.Location); // Проверяем
            direction = gamer.DirectionPlayer(field, gamer, new KeyEventArgs(key2), null); // 3. Меняем направление
            AutoMovePlayer(direction); // 4. Идем в другую сторону
            Assert.AreEqual(new Point(expectedX2, expectedY2), gamer.PlayerPic.Location); // Проверяем
        }

        private void AutoMovePlayer(string direction)
        {
            var playerLoc = new Point();
            while (true)
            {
                gamer.AutoMoving(field.Dic, direction, enemy.EnemyPicRed, enemy.EnemyPicYellow, box);
                if (playerLoc == gamer.PlayerPic.Location)
                    break;
                playerLoc = gamer.PlayerPic.Location;
            }
        }
    }
}