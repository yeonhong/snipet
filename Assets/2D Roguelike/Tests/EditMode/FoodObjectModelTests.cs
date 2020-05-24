using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Roguelike2D;
using UnityEngine;
using UnityEngine.TestTools;

namespace UnitTests
{
    public class FoodObjectModelTests
    {
        [Test]
        public void 음식은_회복량을_갖고있다()
        {
			var food = new FoodObjectModel(10);

			Assert.AreEqual(10, food.Points);
        }
    }
}
