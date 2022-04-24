using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Swibbl.Test
{
    public class Cart
    {
        public int Discount { get; set; }
        public int Price { get; set; }
    }
    public class Tests
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
            //Arrange
            var cart = new Cart();
            var carts = new List<Cart>();
            carts.Add(new Cart { Discount = 10, Price = 199});
            if (carts != null)
            {
                var price = new List<int>();
                var savings = new List<int>();
                foreach (var obj in carts)
                {
                    if (obj is Cart item)
                    {
                        savings.Add(item.Discount);
                        price.Add(item.Price);
                    }
                }
                var p = price.Sum();
                var s = savings.Sum();
                var cartTotal = p - ( p * s / 100);
            }
        }
    }
}