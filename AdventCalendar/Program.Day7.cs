using System.Collections.Generic;
using System.Linq;

namespace AdventCalendar
{
    partial class Program
    {
        private static class Day7
        {
            private class Node
            {
                public char Value { get; }
                public bool Ready { get; private set; }

                private List<Node> Parents { get; }
                private List<Node> Children { get; }
                private int Time { get; set; }

                public Node(char value)
                {
                    Value    = value;
                    Parents  = new List<Node>();
                    Children = new List<Node>();
                    Time     = 60 + value - 'A' + 1;
                    Ready    = true;
                }

                public void AddChild(Node node)
                {
                    Children.Add(node);
                    node.AddParent(this);
                }

                private void AddParent(Node node)
                {
                    Parents.Add(node);
                    Ready = false;
                }

                private void DeleteParent(Node node)
                {
                    Parents.Remove(node);

                    if (!Parents.Any())
                    {
                        Ready = true;
                    }
                }

                public void DeleteFromChildren()
                {
                    foreach (var child in Children)
                    {
                        child.DeleteParent(this);
                    }
                }

                public IEnumerable<Node> GetReadyChildren()
                {
                    return Children.Where(c => c.Ready).ToList();
                }

                public bool DecreaseTimer()
                {
                    Time--;
                    if (Time > 0) return false;

                    DeleteFromChildren();
                    return true;
                }
            }

            private static List<Node> GetGraph(IEnumerable<string> input)
            {
                (char first, char second)[] steps = input.Select(p => (p[5], p[36])).ToArray();

                var list = new List<Node>();

                foreach ((char first, char second) in steps)
                {
                    var parent = list.Find(p => p.Value == first);
                    var child  = list.Find(p => p.Value == second);

                    if (parent == null)
                    {
                        parent = new Node(first);
                        list.Add(parent);
                    }

                    if (child == null)
                    {
                        child = new Node(second);
                        list.Add(child);
                    }

                    parent.AddChild(child);
                }

                return list;
            }

            public static string Day7_1(IEnumerable<string> input)
            {
                string result = "";
                var    list   = GetGraph(input);

                while (list.Count() != 0)
                {
                    var doableNodes = list.Where(node => node.Ready);

                    var next = doableNodes.Aggregate((min, x) => min == null || x.Value < min.Value ? x : min);

                    list.Remove(next);
                    next.DeleteFromChildren();

                    result += next.Value;
                }

                return result;
            }

            public static int Day7_2(IEnumerable<string> input)
            {
                const int MAX_WORKERS = 5;

                int result = 0;
                var list   = GetGraph(input);

                int workersAvailable = MAX_WORKERS;
                var inProgress       = new Node[MAX_WORKERS];

                while (list.Count != 0)
                {
                    var doableNodes = list.Where(node => node.Ready).ToList();

                    while (doableNodes.Any() && workersAvailable != 0)
                    {
                        var next = doableNodes.Aggregate((min, x) => min == null || x.Value < min.Value
                                                                         ? x
                                                                         : min);

                        for (int i = 0; i < inProgress.Length; i++)
                        {
                            if (inProgress[i] == null)
                            {
                                inProgress[i] = next;
                                doableNodes.Remove(next);
                                list.Remove(next);
                                workersAvailable--;
                                break;
                            }
                        }
                    }

                    bool repeat = true;
                    while (repeat)
                    {
                        result++;
                        for (int i = 0; i < inProgress.Length; i++)
                        {
                            bool b = inProgress[i]?.DecreaseTimer() ?? false;
                            if (b)
                            {
                                repeat        = false;
                                inProgress[i] = null;
                                workersAvailable++;
                            }
                        }
                    }
                }

                return result;
            }

            public static int Day7_2_V2(IEnumerable<string> input)
            {
                const int MAX_WORKERS = 5;

                int result = 0;
                var list   = GetGraph(input);

                int workersAvailable = MAX_WORKERS;
                var inProgress       = new Node[MAX_WORKERS];

                var doableNodes = list.Where(node => node.Ready);

                int i                  = 0;
                var orderedDoableNodes = doableNodes.OrderBy(p => p.Value).ToList();
                while (orderedDoableNodes.Any() && workersAvailable != 0)
                {
                    var next = orderedDoableNodes.First();

                    inProgress[i++] = next;
                    orderedDoableNodes.Remove(next);
                    list.Remove(next);
                    workersAvailable--;
                }

                while (list.Count != 0)
                {
                    result++;
                    for (i = 0; i < inProgress.Length; i++)
                    {
                        bool b = inProgress[i]?.DecreaseTimer() ?? false;
                        if (b)
                        {
                            var readyChildren = inProgress[i].GetReadyChildren();
                            orderedDoableNodes = readyChildren.Union(orderedDoableNodes)
                                                              .OrderBy(p => p.Value)
                                                              .ToList();

                            list.Remove(inProgress[i]);
                            inProgress[i] = orderedDoableNodes.Any()
                                                ? orderedDoableNodes.First()
                                                : null;
                            orderedDoableNodes.Remove(inProgress[i]);

                            if (inProgress[i] == null)
                            {
                                workersAvailable++;
                            }
                        }
                        else if (inProgress[i] == null &&
                                 orderedDoableNodes.Any())
                        {
                            inProgress[i] = orderedDoableNodes.First();
                            orderedDoableNodes.Remove(inProgress[i]);
                            workersAvailable--;
                        }
                    }
                }

                return result;
            }
        }
    }
}