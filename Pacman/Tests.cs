using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using NUnit.Framework;

namespace Pacman
{
    [TestFixture]
    
    public class Tests
    {
        [Test]
        public void CanMove()
        {
            var pacman = new Gamer();
            Assert.AreEqual(false, pacman.CanMove(new Dictionary<Point, PictureBox>(), 0, 0));
        }
    }
}