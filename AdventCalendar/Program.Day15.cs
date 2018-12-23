using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventCalendar
{
    partial class Program
    {
        private static class Day15
        {
            private const int DIMENSION = 32;

            private const int WALL = -1;
            private const int ELF = -2;
            private const int GOBLIN = -3;

            private const int BASE_ATTACK = 3;
            private const int BASE_HEALTH = 200;

            private class Unit
            {
                public enum Races
                {
                    Elf,
                    Goblin
                }

                public (int x, int y) Location { get; set; }
                public Races Race { get; }
                public int Health { get; set; }
                public int Attack { get; set; }
                public bool HasMoved { get; set; }

                public Unit(int x, int y, Races race)
                {
                    Race     = race;
                    Health   = BASE_HEALTH;
                    Attack   = BASE_ATTACK;
                    Location = (x, y);
                    HasMoved = false;
                }

                public Unit(Unit src)
                {
                    Location = src.Location;
                    Race     = src.Race;
                    Health   = src.Health;
                    Attack   = src.Attack;
                    HasMoved = src.HasMoved;
                }
            }

            private static IEnumerable<(int x, int y)> GetNeighbours((int x, int y) cell)
            {
                var ret = new List<(int x, int y)>
                          {
                              (cell.x, cell.y - 1),
                              (cell.x         - 1, cell.y),
                              (cell.x         + 1, cell.y),
                              (cell.x, cell.y + 1)
                          };

                return ret;
            }

            private static int[,] FindPaths(int[,] map, (int x, int y) destination)
            {
                var current = new HashSet<(int x, int y)> {destination};

                var outputMap = new int[DIMENSION, DIMENSION];
                Array.Copy(map, outputMap, map.Length);

                bool noMorePaths = false;

                while (!noMorePaths)
                {
                    var newCurrent = new HashSet<(int x, int y)>();
                    noMorePaths = true;

                    foreach (var i in current)
                    {
                        int value = outputMap[i.x, i.y] < 0
                                        ? 1
                                        : outputMap[i.x, i.y] + 1;
                        var neighbours = GetNeighbours(i);

                        foreach (var cell in neighbours)
                        {
                            if (cell.x >= DIMENSION ||
                                cell.x < 0          ||
                                cell.y >= DIMENSION ||
                                cell.y < 0)
                            {
                                continue;
                            }

                            if (!newCurrent.Contains(cell))
                            {
                                newCurrent.Add(cell);
                            }

                            if (outputMap[cell.x, cell.y] > value ||
                                outputMap[cell.x, cell.y] == 0)
                            {
                                outputMap[cell.x, cell.y] = value;
                            }

                            if (outputMap[cell.x, cell.y] < value)
                            {
                                newCurrent.Remove(cell);
                            }
                        }
                    }

                    if (newCurrent.Any())
                    {
                        noMorePaths = false;
                    }

                    current = newCurrent;
                }

                return outputMap;
            }

            private static bool MakeMoves(int[,] map,
                                          Unit[,] unitsMap,
                                          ICollection<Unit> elves,
                                          ICollection<Unit> goblins)
            {
                bool ret = false;

                var elvesMap  = PopulateDistanceMap(map, elves);
                var goblinMap = PopulateDistanceMap(map, goblins);

                for (int y = 0; y < DIMENSION; y++)
                for (int x = 0; x < DIMENSION; x++)
                {
                    if (unitsMap[x, y] == null ||
                        unitsMap[x, y].HasMoved)
                        continue;

                    var unit = unitsMap[x, y];
                    var currentMap = unit.Race == Unit.Races.Elf
                                         ? goblinMap
                                         : elvesMap;

                    var nextMove = DecideNextMove(unitsMap, unit, currentMap);

                    if (nextMove.distance < int.MaxValue &&
                        !nextMove.isAttacking)
                    {
                        unitsMap[unit.Location.x, unit.Location.y] = null;
                        map[unit.Location.x, unit.Location.y]      = 0;
                        unit.Location                              = (nextMove.x, nextMove.y);
                        unitsMap[nextMove.x, nextMove.y]           = unit;
                        map[nextMove.x, nextMove.y] = unit.Race == Unit.Races.Elf
                                                          ? ELF
                                                          : GOBLIN;

                        elvesMap  = PopulateDistanceMap(map, elves);
                        goblinMap = PopulateDistanceMap(map, goblins);
                    }

                    if (nextMove.distance == 1)
                    {
                        nextMove = DecideNextMove(unitsMap, unit, currentMap);
                    }

                    if (nextMove.isAttacking)
                    {
                        var target = unitsMap[nextMove.x, nextMove.y];
                        target.Health -= unit.Attack;

                        if (target.Health <= 0)
                        {
                            unitsMap[nextMove.x, nextMove.y]   = null;
                            currentMap[nextMove.x, nextMove.y] = 0;
                            map[nextMove.x, nextMove.y]        = 0;

                            if (target.Race == Unit.Races.Elf)
                            {
                                elves.Remove(target);
                                ret = true;
                            }
                            else
                            {
                                goblins.Remove(target);
                            }

                            elvesMap  = PopulateDistanceMap(map, elves);
                            goblinMap = PopulateDistanceMap(map, goblins);
                        }
                    }

                    unit.HasMoved = true;
                }

                return ret;
            }

            private static (bool isAttacking, int x, int y, int distance) DecideNextMove(Unit[,] unitsMap,
                                                                                         Unit unit,
                                                                                         int[,] currentMap)
            {
                (bool isAttacking, int x, int y, int distance) nextMove  = (false, 0, 0, int.MaxValue);
                var                                            neighbors = GetNeighbours(unit.Location);

                foreach ((int X, int Y) in neighbors)
                {
                    switch (currentMap[X, Y])
                    {
                    case ELF:
                        if (unit.Race == Unit.Races.Goblin)
                        {
                            if (nextMove.isAttacking)
                            {
                                if (unitsMap[X, Y].Health >=
                                    unitsMap[nextMove.x, nextMove.y].Health)
                                    break;
                            }

                            nextMove = (true, X, Y, 0);
                        }

                        break;

                    case GOBLIN:
                        if (unit.Race == Unit.Races.Elf)
                        {
                            if (nextMove.isAttacking)
                            {
                                if (unitsMap[X, Y].Health >=
                                    unitsMap[nextMove.x, nextMove.y].Health)
                                    break;
                            }

                            nextMove = (true, X, Y, 0);
                        }

                        break;

                    default:
                        if (currentMap[X, Y] > 0 &&
                            currentMap[X, Y] < nextMove.distance)
                        {
                            nextMove = (false, X, Y, currentMap[X, Y]);
                        }

                        break;
                    }
                }

                return nextMove;
            }

            private static int[,] PopulateDistanceMap(int[,] map, IEnumerable<Unit> units)
            {
                var outputMap = new int[DIMENSION, DIMENSION];
                Array.Copy(map, outputMap, map.Length);

                return units.Aggregate(outputMap, (current, unit) => FindPaths(current, unit.Location));
            }

            public static int Day15_1(IReadOnlyList<string> input)
            {
                var map      = new int[DIMENSION, DIMENSION];
                var unitsMap = new Unit[DIMENSION, DIMENSION];
                var elves    = new List<Unit>();
                var goblins  = new List<Unit>();

                for (int y = 0; y < DIMENSION; y++)
                for (int x = 0; x < DIMENSION; x++)
                {
                    char tile = input[y][x];

                    map[x, y] = tile == '#'
                                    ? WALL
                                    : tile == 'E'
                                        ? ELF
                                        : tile == 'G'
                                            ? GOBLIN
                                            : 0;
                    switch (tile)
                    {
                    case 'E':
                        var elf = new Unit(x, y, Unit.Races.Elf);
                        elves.Add(elf);
                        unitsMap[x, y] = elf;
                        break;
                    case 'G':
                        var goblin = new Unit(x, y, Unit.Races.Goblin);
                        goblins.Add(goblin);
                        unitsMap[x, y] = goblin;
                        break;
                    }
                }

                var round = 0;

                while (elves.Any() &&
                       goblins.Any())
                {
                    round++;

                    foreach (var elf in elves)
                    {
                        elf.HasMoved = false;
                    }

                    foreach (var goblin in goblins)
                    {
                        goblin.HasMoved = false;
                    }

                    MakeMoves(map, unitsMap, elves, goblins);
                }

                return (goblins.Sum(g => g.Health) +
                        elves.Sum(e => e.Health)) *
                       (round - 1);
            }

            public static int Day15_2(IReadOnlyList<string> input)
            {
                var Map      = new int[DIMENSION, DIMENSION];
                var UnitsMap = new Unit[DIMENSION, DIMENSION];
                var Elves    = new List<Unit>();
                var Goblins  = new List<Unit>();

                for (int y = 0; y < DIMENSION; y++)
                for (int x = 0; x < DIMENSION; x++)
                {
                    char tile = input[y][x];

                    Map[x, y] = tile == '#'
                                    ? WALL
                                    : tile == 'E'
                                        ? ELF
                                        : tile == 'G'
                                            ? GOBLIN
                                            : 0;
                    switch (tile)
                    {
                    case 'E':
                        var elf = new Unit(x, y, Unit.Races.Elf);
                        Elves.Add(elf);
                        UnitsMap[x, y] = elf;
                        break;
                    case 'G':
                        var goblin = new Unit(x, y, Unit.Races.Goblin);
                        Goblins.Add(goblin);
                        UnitsMap[x, y] = goblin;
                        break;
                    }
                }

                int  round         = -1;
                bool hasAnElfDied  = false;
                int  currentAttack = BASE_ATTACK;

                var map      = new int[DIMENSION, DIMENSION];
                var unitsMap = new Unit[DIMENSION, DIMENSION];
                var goblins  = new List<Unit>();
                var elves    = new List<Unit>();

                while (hasAnElfDied ||
                       round == -1)
                {
                    round = 0;
                    currentAttack++;

                    elves   = Elves.Select(e => new Unit(e)).ToList();
                    goblins = Goblins.Select(g => new Unit(g)).ToList();

                    Array.Copy(Map, map, Map.Length);

                    foreach (var elf in elves)
                    {
                        unitsMap[elf.Location.x, elf.Location.y] = elf;
                        elf.Attack                               = currentAttack;
                    }

                    foreach (var goblin in goblins)
                    {
                        unitsMap[goblin.Location.x, goblin.Location.y] = goblin;
                    }

                    hasAnElfDied = false;

                    while (!hasAnElfDied &&
                           goblins.Any())
                    {
                        round++;

                        foreach (var elf in elves)
                        {
                            elf.HasMoved = false;
                        }

                        foreach (var goblin in goblins)
                        {
                            goblin.HasMoved = false;
                        }

                        hasAnElfDied = MakeMoves(map, unitsMap, elves, goblins);
                    }
                }

                return (goblins.Sum(g => g.Health) +
                        elves.Sum(e => e.Health)) *
                       (round - 1);
            }
        }
    }
}
