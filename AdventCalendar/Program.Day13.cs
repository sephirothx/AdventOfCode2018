using System;
using System.Collections.Generic;

namespace AdventCalendar
{
    partial class Program
    {
        private static class Day13
        {
            private const int DIMENSION = 150;

            private const char CART_UP = '^';
            private const char CART_LEFT = '<';
            private const char CART_RIGHT = '>';
            private const char CART_DOWN = 'v';
            private const char RAIL_H = '-';
            private const char RAIL_V = '|';
            private const char INTERSECTION = '+';
            private const char TURN_NE = '/';
            private const char TURN_SE = '\\';

            private class Cart
            {
                private enum IntersectionDirection
                {
                    Left,
                    Straight,
                    Right
                }

                private IntersectionDirection NextIntersection { get; set; }
                public (int x, int y) Direction { get; set; }
                public int ID { get; }

                public Cart(int id, (int, int) direction)
                {
                    ID               = id;
                    Direction        = direction;
                    NextIntersection = IntersectionDirection.Left;
                }

                public void Intersection()
                {
                    switch (NextIntersection)
                    {
                    case IntersectionDirection.Left:
                        NextIntersection = IntersectionDirection.Straight;
                        Direction = (Direction.y, -Direction.x);
                        break;

                    case IntersectionDirection.Straight:
                        NextIntersection = IntersectionDirection.Right;
                        break;

                    case IntersectionDirection.Right:
                        NextIntersection = IntersectionDirection.Left;
                        Direction = (-Direction.y, Direction.x);
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                    }
                }
            }

            private class Mine
            {
                private Cart[,] Carts { get; set; }
                private string[] Map { get; }
                public (int x, int y) CollisionSpot { get; private set; }
                public int CartsCount { get; private set; }

                public Mine(Cart[,] carts, string[] map, int cartsCount)
                {
                    Carts      = carts;
                    Map        = map;
                    CartsCount = cartsCount;
                }

                public (int x, int y) GetLastCartPosition()
                {
                    var ret = (x: 0,
                               y: 0);

                    for (int y = 0; y < DIMENSION; y++)
                    for (int x = 0; x < DIMENSION; x++)
                    {
                        if (Carts[y, x] == null) continue;

                        ret = (x, y);
                    }

                    return ret;
                }

                private void RemoveCartByID(int id)
                {
                    for (int y = 0; y < DIMENSION; y++)
                    for (int x = 0; x < DIMENSION; x++)
                    {
                        if (Carts[y, x]?.ID == id)
                        {
                            Carts[y, x] = null;
                        }
                    }
                }

                public bool MoveAll()
                {
                    var newCarts       = (Cart[,])Carts.Clone();
                    var ret            = false;
                    var firstCollision = true;

                    for (int y = 0; y < DIMENSION; y++)
                    for (int x = 0; x < DIMENSION; x++)
                    {
                        if (Carts[y, x] == null) continue;

                        var cart = Carts[y, x];

                        int dy = y + cart.Direction.y;
                        int dx = x + cart.Direction.x;

                        if (newCarts[dy, dx] != null)
                        {
                            if (firstCollision)
                            {
                                CollisionSpot  = (dx, dy);
                                firstCollision = false;
                            }

                            RemoveCartByID(newCarts[y, x].ID);
                            RemoveCartByID(newCarts[dy, dx].ID);
                            newCarts[y, x]   =  null;
                            newCarts[dy, dx] =  null;
                            CartsCount       -= 2;
                            ret              =  true;
                        }
                        else
                        {
                            switch (Map[dy][dx])
                            {
                            case TURN_NE:
                                cart.Direction = (-cart.Direction.y, -cart.Direction.x);
                                break;

                            case TURN_SE:
                                cart.Direction = (cart.Direction.y, cart.Direction.x);
                                break;

                            case INTERSECTION:
                                cart.Intersection();
                                break;
                            }

                            newCarts[dy, dx] = cart;
                            newCarts[y, x]   = null;
                        }
                    }

                    Carts = newCarts;

                    return ret;
                }
            }

            private static int GetInput(IList<string> input, Cart[,] carts)
            {
                int cartsCount = 0;

                for (int y = 0; y < input.Count; y++)
                {
                    string s = input[y];

                    for (int x = 0; x < s.Length; x++)
                    {
                        char   c = s[x];
                        char[] substitute;

                        switch (c)
                        {
                        case CART_UP:
                            carts[y, x] = new Cart(cartsCount, (0, -1));
                            cartsCount++;
                            substitute    = input[y].ToCharArray();
                            substitute[x] = RAIL_V;
                            input[y]      = new string(substitute);
                            break;

                        case CART_LEFT:
                            carts[y, x] = new Cart(cartsCount, (-1, 0));
                            cartsCount++;
                            substitute    = input[y].ToCharArray();
                            substitute[x] = RAIL_H;
                            input[y]      = new string(substitute);
                            break;

                        case CART_RIGHT:
                            carts[y, x] = new Cart(cartsCount, (1, 0));
                            cartsCount++;
                            substitute    = input[y].ToCharArray();
                            substitute[x] = RAIL_H;
                            input[y]      = new string(substitute);
                            break;

                        case CART_DOWN:
                            carts[y, x] = new Cart(cartsCount, (0, 1));
                            cartsCount++;
                            substitute    = input[y].ToCharArray();
                            substitute[x] = RAIL_V;
                            input[y]      = new string(substitute);
                            break;
                        }
                    }
                }

                return cartsCount;
            }

            public static void Day13_1(string[] input)
            {
                var carts      = new Cart[DIMENSION, DIMENSION];
                int cartsCount = GetInput(input, carts);
                var mine       = new Mine(carts, input, cartsCount);

                while (!mine.MoveAll()) {}

                Console.WriteLine(mine.CollisionSpot);
            }

            public static void Day13_2(string[] input)
            {
                var carts      = new Cart[DIMENSION, DIMENSION];
                int cartsCount = GetInput(input, carts);
                var mine       = new Mine(carts, input, cartsCount);

                while (mine.CartsCount != 1)
                {
                    mine.MoveAll();
                }

                Console.WriteLine(mine.GetLastCartPosition());
            }
        }
    }
}
