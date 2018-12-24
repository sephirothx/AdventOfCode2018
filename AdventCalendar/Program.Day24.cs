using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventCalendar
{
    partial class Program
    {
        private static class Day24
        {
            private const int MAX_ALLOWED_ITERATIONS = 10000;
            private class UnitGroup
            {
                public enum Races
                {
                    ImmuneSystem,
                    Infection
                }

                public Races Race { get; }
                public int Units { get; private set; }
                private int HP { get; }
                public int Attack { get; set; }
                private string AttackType { get; }
                public int Initiative { get; }

                private readonly int _startingUnits;

                public int EffectivePower => Units * Attack;

                private List<string> Weaknesses { get; }
                private List<string> Immunities { get; }

                public UnitGroup Target { get; set; }

                public UnitGroup(Races race, int units, int hp, int attack, string attackType, int initiative, List<string> weaknesses, List<string> immunities)
                {
                    Race       = race;
                    Units      = units;
                    HP         = hp;
                    Attack     = attack;
                    AttackType = attackType;
                    Initiative = initiative;
                    Weaknesses = weaknesses;
                    Immunities = immunities;

                    _startingUnits = units;
                }

                public void Restore()
                {
                    Units = _startingUnits;
                }

                public int CalculateDamage(UnitGroup opponent)
                {
                    if (opponent.Immunities.Contains(AttackType))
                    {
                        return 0;
                    }

                    if (opponent.Weaknesses.Contains(AttackType))
                    {
                        return EffectivePower * 2;
                    }

                    return EffectivePower;
                }

                public bool DoAttack()
                {
                    if (Target == null)
                    {
                        return false;
                    }

                    int damage = CalculateDamage(Target);
                    Target.Units = Target.Units - damage / Target.HP;

                    return Target.Units <= 0;
                }
            }

            private static List<UnitGroup> GetArmy(UnitGroup.Races race, string path)
            {
                var regex = new Regex(@"(?<units>\d+) units.*?(?<hp>\d+).*?(?<parenthesis>\((?<mode1>immune|weak) to (?<type11>\w+)(\, (?<type12>\w+)(\, (?<type13>\w+))?)?(\; (?<mode2>immune|weak) to (?<type21>\w+)(\, (?<type22>\w+))?)?\))? with .*?(?<attack>\d+) (?<attacktype>\w+).*?(?<initiative>\d+)");
                var input = File.ReadAllLines(path);
                var army  = new List<UnitGroup>();

                foreach (string line in input)
                {
                    var match = regex.Match(line);

                    int    units      = int.Parse(match.Groups["units"].Value);
                    int    hp         = int.Parse(match.Groups["hp"].Value);
                    int    attack     = int.Parse(match.Groups["attack"].Value);
                    string attackType = match.Groups["attacktype"].Value;
                    int    initiative = int.Parse(match.Groups["initiative"].Value);

                    var weaknesses = new List<string>();
                    var immunities = new List<string>();

                    for (int i = 1; i <= 2; i++)
                    {
                        if (!match.Groups[$"mode{i}"].Success) break;

                        var current = match.Groups[$"mode{i}"].Value == "weak"
                                          ? weaknesses
                                          : immunities;

                        for (int j = 1; j <= 3; j++)
                        {
                            if (match.Groups[$"type{i}{j}"].Success)
                            {
                                current.Add(match.Groups[$"type{i}{j}"].Value);
                            }
                        }
                    }

                    var group = new UnitGroup(race, units, hp, attack, attackType, initiative, weaknesses, immunities);
                    army.Add(group);
                }

                return army;
            }

            private static void TargetSelectionPhase(List<UnitGroup> army1,
                                                     List<UnitGroup> army2)
            {
                army1 = army1.OrderByDescending(g => g.EffectivePower)
                             .ThenByDescending(g => g.Initiative)
                             .ToList();
                army2 = army2.OrderByDescending(g => g.EffectivePower)
                             .ThenByDescending(g => g.Initiative)
                             .ToList();

                for (int i = 0; i < 2; i++)
                {
                    var attacking = new List<UnitGroup>(i == 0 ? army1 : army2);
                    var opponents = new List<UnitGroup>(i == 0 ? army2 : army1);

                    foreach (var group in attacking)
                    {
                        UnitGroup target    = null;
                        int       maxDamage = 0;

                        foreach (var opponent in opponents)
                        {
                            int damage = group.CalculateDamage(opponent);
                            if (damage > maxDamage)
                            {
                                maxDamage = damage;
                                target    = opponent;
                            }
                            else if (damage == maxDamage)
                            {
                                target = opponent.EffectivePower == target?.EffectivePower
                                             ? opponent.Initiative > target.Initiative
                                                   ? opponent
                                                   : target
                                             : opponent.EffectivePower > target?.EffectivePower
                                                 ? opponent
                                                 : target;
                            }
                        }

                        group.Target = target;
                        if (target != null)
                        {
                            opponents.Remove(target);
                        }
                    }
                }
            }

            private static void AttackingPhase(ICollection<UnitGroup> allGroups,
                                               ICollection<UnitGroup> immuneSystemArmy,
                                               ICollection<UnitGroup> infectionArmy)
            {
                var attackers = new List<UnitGroup>(allGroups);
                
                foreach (var attacker in attackers)
                {
                    if (attacker.Units > 0 &&
                        attacker.DoAttack())
                    {
                        allGroups.Remove(attacker.Target);
                        if (attacker.Race == UnitGroup.Races.ImmuneSystem)
                        {
                            infectionArmy.Remove(attacker.Target);
                        }
                        else
                        {
                            immuneSystemArmy.Remove(attacker.Target);
                        }
                    }
                }
            }

            private static void BoostAttack(IEnumerable<UnitGroup> army)
            {
                foreach (var group in army)
                {
                    group.Attack++;
                }
            }

            private static void Restore(IEnumerable<UnitGroup> army)
            {
                foreach (var group in army)
                {
                    group.Restore();
                }
            }

            public static void Day24_1()
            {
                const string immuneSystemPath = @"C:\Users\User\Documents\immunesystem.txt";
                const string infectionPath    = @"C:\Users\User\Documents\infection.txt";

                var immuneSystemArmy = GetArmy(UnitGroup.Races.ImmuneSystem, immuneSystemPath);
                var infectionArmy    = GetArmy(UnitGroup.Races.Infection,    infectionPath);
                
                var allGroups = new List<UnitGroup>();
                allGroups.AddRange(immuneSystemArmy);
                allGroups.AddRange(infectionArmy);
                allGroups = allGroups.OrderByDescending(p => p.Initiative).ToList();

                while (immuneSystemArmy.Any() &&
                       infectionArmy.Any())
                {
                    TargetSelectionPhase(immuneSystemArmy, infectionArmy);
                    AttackingPhase(allGroups, immuneSystemArmy, infectionArmy);
                }

                var winners = immuneSystemArmy.Any()
                                  ? immuneSystemArmy
                                  : infectionArmy;

                Console.WriteLine(winners.Sum(g => g.Units));
            }

            public static void Day24_2()
            {
                const string immuneSystemPath = @"C:\Users\User\Documents\immunesystem.txt";
                const string infectionPath    = @"C:\Users\User\Documents\infection.txt";

                var ImmuneSystemArmy = GetArmy(UnitGroup.Races.ImmuneSystem, immuneSystemPath);
                var InfectionArmy    = GetArmy(UnitGroup.Races.Infection,    infectionPath);

                var AllGroups = new List<UnitGroup>();
                AllGroups.AddRange(ImmuneSystemArmy);
                AllGroups.AddRange(InfectionArmy);
                AllGroups = AllGroups.OrderByDescending(p => p.Initiative).ToList();

                for (int i = 0; i < int.MaxValue; i++)
                {
                    var allGroups        = new List<UnitGroup>(AllGroups);
                    var immuneSystemArmy = new List<UnitGroup>(ImmuneSystemArmy);
                    var infectionArmy    = new List<UnitGroup>(InfectionArmy);

                    BoostAttack(immuneSystemArmy);
                    Restore(immuneSystemArmy);
                    Restore(infectionArmy);

                    int iterations = 0;
                    while (immuneSystemArmy.Any() &&
                           infectionArmy.Any())
                    {
                        TargetSelectionPhase(immuneSystemArmy, infectionArmy);
                        AttackingPhase(allGroups, immuneSystemArmy, infectionArmy);

                        if (++iterations > MAX_ALLOWED_ITERATIONS)
                        {
                            immuneSystemArmy = new List<UnitGroup>();
                            break;
                        }
                    }

                    if (immuneSystemArmy.Any())
                    {
                        Console.WriteLine($"{i+1}, {immuneSystemArmy.Sum(g => g.Units)}");
                        break;
                    }
                }
            }
        }
    }
}
