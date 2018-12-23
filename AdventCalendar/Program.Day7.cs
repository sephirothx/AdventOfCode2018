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
                public List<Node> Parents { get; }
                public List<Node> Children { get; }
                private int Time { get; set; }
                public bool Ready { get; private set; }

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
                    return Children.Where(p => p.Ready).ToList();
                }

                public bool DecreaseTimer()
                {
                    Time--;
                    if (Time > 0) return false;

                    DeleteFromChildren();
                    return true;
                }
            }

            public static string Day7_1(IEnumerable<string> input)
            {
                (char first, char second)[] a = input.Select(p => (p[5], p[36])).ToArray();

                string result = "";

                var list = new List<Node>();

                foreach (var valueTuple in a)
                {
                    var first  = list.Find(p => p.Value == valueTuple.first);
                    var second = list.Find(p => p.Value == valueTuple.second);

                    if (first == null)
                    {
                        first = new Node(valueTuple.first);
                        list.Add(first);
                    }

                    if (second == null)
                    {
                        second = new Node(valueTuple.second);
                        list.Add(second);
                    }

                    first.Children.Add(second);
                    second.Parents.Add(first);
                }

                while (list.Count() != 0)
                {
                    var doableNodes = new List<Node>();

                    foreach (var node in list)
                    {
                        if (!node.Parents.Any())
                        {
                            doableNodes.Add(node);
                        }
                    }

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

                (char first, char second)[] a = input.Select(p => (p[5], p[36])).ToArray();

                int result = 0;
                var list   = new List<Node>();

                int workersAvailable = MAX_WORKERS;
                var inProgress       = new Node[MAX_WORKERS];

                foreach (var valueTuple in a)
                {
                    var first  = list.Find(p => p.Value == valueTuple.first);
                    var second = list.Find(p => p.Value == valueTuple.second);

                    if (first == null)
                    {
                        first = new Node(valueTuple.first);
                        list.Add(first);
                    }

                    if (second == null)
                    {
                        second = new Node(valueTuple.second);
                        list.Add(second);
                    }

                    first.Children.Add(second);
                    second.Parents.Add(first);
                }

                while (list.Count != 0)
                {
                    var doableNodes = new List<Node>();

                    foreach (var node in list)
                    {
                        if (!node.Parents.Any())
                        {
                            doableNodes.Add(node);
                        }
                    }

                    while (doableNodes.Any() && workersAvailable != 0)
                    {
                        var next = doableNodes.Aggregate((min, x) => min == null || x.Value < min.Value ? x : min);

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

            public static int Day7_3(IEnumerable<string> input)
            {
                const int MAX_WORKERS = 5;

                (char first, char second)[] a = input.Select(p => (p[5], p[36])).ToArray();

                int result = 0;
                var list   = new List<Node>();

                int workersAvailable = MAX_WORKERS;
                var inProgress       = new Node[MAX_WORKERS];

                foreach (var valueTuple in a)
                {
                    var first  = list.Find(p => p.Value == valueTuple.first);
                    var second = list.Find(p => p.Value == valueTuple.second);

                    if (first == null)
                    {
                        first = new Node(valueTuple.first);
                        list.Add(first);
                    }

                    if (second == null)
                    {
                        second = new Node(valueTuple.second);
                        list.Add(second);
                    }

                    first.AddChild(second);
                }

                var doableNodes = new List<Node>();

                foreach (var node in list)
                {
                    if (node.Ready)
                    {
                        doableNodes.Add(node);
                    }
                }

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