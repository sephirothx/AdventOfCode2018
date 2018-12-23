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
                public enum Direction
                {
                    Up,
                    Down,
                    Right,
                    Left
                }

                private enum IntersectionDirection
                {
                    Left,
                    Straight,
                    Right
                }

                private IntersectionDirection NextIntersection { get; set; }
                public Direction CurrentDirection { get; set; }
                public int ID { get; }

                public Cart(int id, Direction direction)
                {
                    ID               = id;
                    CurrentDirection = direction;
                    NextIntersection = IntersectionDirection.Left;
                }

                public void Intersection()
                {
                    switch (NextIntersection)
                    {
                    case IntersectionDirection.Left:
                        NextIntersection = IntersectionDirection.Straight;
                        switch (CurrentDirection)
                        {
                        case Direction.Up:
                            CurrentDirection = Direction.Left;
                            break;
                        case Direction.Down:
                            CurrentDirection = Direction.Right;
                            break;
                        case Direction.Right:
                            CurrentDirection = Direction.Up;
                            break;
                        case Direction.Left:
                            CurrentDirection = Direction.Down;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                        }

                        break;

                    case IntersectionDirection.Straight:
                        NextIntersection = IntersectionDirection.Right;
                        break;

                    case IntersectionDirection.Right:
                        NextIntersection = IntersectionDirection.Left;
                        switch (CurrentDirection)
                        {
                        case Direction.Up:
                            CurrentDirection = Direction.Right;
                            break;
                        case Direction.Down:
                            CurrentDirection = Direction.Left;
                            break;
                        case Direction.Right:
                            CurrentDirection = Direction.Down;
                            break;
                        case Direction.Left:
                            CurrentDirection = Direction.Up;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                        }

                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                    }
                }

                public void Print()
                {
                    char v;

                    switch (CurrentDirection)
                    {
                    case Direction.Up:
                        v = CART_UP;
                        break;
                    case Direction.Down:
                        v = CART_DOWN;
                        break;
                    case Direction.Right:
                        v = CART_RIGHT;
                        break;
                    case Direction.Left:
                        v = CART_LEFT;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                    }

                    Console.Write(v);
                }
            }

            private class Mine
            {
                private Cart[,] Carts { get; set; }
                private string[] Map { get; }
                public (int X, int Y) CollisionSpot { get; private set; }
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
                        switch (cart.CurrentDirection)
                        {
                        case Cart.Direction.Up:
                            if (newCarts[y - 1, x] != null)
                            {
                                if (firstCollision)
                                {
                                    CollisionSpot  = (x, y - 1);
                                    firstCollision = false;
                                }

                                RemoveCartByID(newCarts[y, x].ID);
                                RemoveCartByID(newCarts[y - 1, x].ID);
                                newCarts[y, x]     =  null;
                                newCarts[y - 1, x] =  null;
                                CartsCount         -= 2;
                                ret                =  true;
                                break;
                            }

                            switch (Map[y - 1][x])
                            {
                            case TURN_NE:
                                cart.CurrentDirection = Cart.Direction.Right;
                                break;

                            case TURN_SE:
                                cart.CurrentDirection = Cart.Direction.Left;
                                break;

                            case INTERSECTION:
                                cart.Intersection();
                                break;
                            }

                            newCarts[y - 1, x] = cart;
                            newCarts[y, x]     = null;
                            break;

                        case Cart.Direction.Down:
                            if (newCarts[y + 1, x] != null)
                            {
                                if (firstCollision)
                                {
                                    CollisionSpot  = (x, y + 1);
                                    firstCollision = false;
                                }

                                RemoveCartByID(newCarts[y, x].ID);
                                RemoveCartByID(newCarts[y + 1, x].ID);
                                newCarts[y, x]     =  null;
                                newCarts[y + 1, x] =  null;
                                CartsCount         -= 2;
                                ret                =  true;
                                break;
                            }

                            switch (Map[y + 1][x])
                            {
                            case TURN_NE:
                                cart.CurrentDirection = Cart.Direction.Left;
                                break;

                            case TURN_SE:
                                cart.CurrentDirection = Cart.Direction.Right;
                                break;

                            case INTERSECTION:
                                cart.Intersection();
                                break;
                            }

                            newCarts[y + 1, x] = cart;
                            newCarts[y, x]     = null;
                            break;

                        case Cart.Direction.Right:
                            if (newCarts[y, x + 1] != null)
                            {
                                if (firstCollision)
                                {
                                    CollisionSpot  = (x + 1, y);
                                    firstCollision = false;
                                }

                                RemoveCartByID(newCarts[y, x].ID);
                                RemoveCartByID(newCarts[y, x + 1].ID);
                                newCarts[y, x]     =  null;
                                newCarts[y, x + 1] =  null;
                                CartsCount         -= 2;
                                ret                =  true;
                                break;
                            }

                            switch (Map[y][x + 1])
                            {
                            case TURN_NE:
                                cart.CurrentDirection = Cart.Direction.Up;
                                break;

                            case TURN_SE:
                                cart.CurrentDirection = Cart.Direction.Down;
                                break;

                            case INTERSECTION:
                                cart.Intersection();
                                break;
                            }

                            newCarts[y, x + 1] = cart;
                            newCarts[y, x]     = null;
                            break;

                        case Cart.Direction.Left:
                            if (newCarts[y, x - 1] != null)
                            {
                                if (firstCollision)
                                {
                                    CollisionSpot  = (x - 1, y);
                                    firstCollision = false;
                                }

                                RemoveCartByID(newCarts[y, x].ID);
                                RemoveCartByID(newCarts[y, x - 1].ID);
                                newCarts[y, x]     =  null;
                                newCarts[y, x - 1] =  null;
                                CartsCount         -= 2;
                                ret                =  true;
                                break;
                            }

                            switch (Map[y][x - 1])
                            {
                            case TURN_NE:
                                cart.CurrentDirection = Cart.Direction.Down;
                                break;

                            case TURN_SE:
                                cart.CurrentDirection = Cart.Direction.Up;
                                break;

                            case INTERSECTION:
                                cart.Intersection();
                                break;
                            }

                            newCarts[y, x - 1] = cart;
                            newCarts[y, x]     = null;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                        }
                    }

                    Carts = newCarts;

                    return ret;
                }

                public void Print()
                {
                    Console.Clear();

                    for (int y = 0; y < DIMENSION; y++)
                    {
                        for (int x = 0; x < DIMENSION; x++)
                        {
                            if (Carts[y, x] != null)
                            {
                                Carts[y, x].Print();
                                continue;
                            }

                            Console.Write(Map[y][x]);
                        }

                        Console.WriteLine();
                    }
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
                            carts[y, x] = new Cart(cartsCount, Cart.Direction.Up);
                            cartsCount++;
                            substitute    = input[y].ToCharArray();
                            substitute[x] = RAIL_V;
                            input[y]      = new string(substitute);
                            break;

                        case CART_LEFT:
                            carts[y, x] = new Cart(cartsCount, Cart.Direction.Left);
                            cartsCount++;
                            substitute    = input[y].ToCharArray();
                            substitute[x] = RAIL_H;
                            input[y]      = new string(substitute);
                            break;

                        case CART_RIGHT:
                            carts[y, x] = new Cart(cartsCount, Cart.Direction.Right);
                            cartsCount++;
                            substitute    = input[y].ToCharArray();
                            substitute[x] = RAIL_H;
                            input[y]      = new string(substitute);
                            break;

                        case CART_DOWN:
                            carts[y, x] = new Cart(cartsCount, Cart.Direction.Down);
                            cartsCount++;
                            substitute    = input[y].ToCharArray();
                            substitute[x] = RAIL_V;
                            input[y]      = new string(substitute);
                            break;

                        case RAIL_H:
                            break;

                        case RAIL_V:
                            break;

                        case INTERSECTION:
                            break;

                        case TURN_NE:
                            break;

                        case TURN_SE:
                            break;
                        }
                    }
                }

                return cartsCount;
            }

            public static (int x, int y) Day13_1(string[] input)
            {
                var carts      = new Cart[DIMENSION, DIMENSION];
                int cartsCount = GetInput(input, carts);
                var mine       = new Mine(carts, input, cartsCount);

                while (!mine.MoveAll()) {}

                return mine.CollisionSpot;
            }

            public static (int x, int y) Day13_2(string[] input)
            {
                var carts      = new Cart[DIMENSION, DIMENSION];
                int cartsCount = GetInput(input, carts);
                var mine       = new Mine(carts, input, cartsCount);

                while (mine.CartsCount != 1)
                {
                    mine.MoveAll();
                }

                return mine.GetLastCartPosition();
            }
        }
    }
}
