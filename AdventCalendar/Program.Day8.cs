using System.Collections.Generic;
using System.Linq;

namespace AdventCalendar
{
    partial class Program
    {
        private static class Day8
        {
            private class Node
            {
                public List<Node> Children { get; }
                public List<int> Metadata { get; }

                public Node()
                {
                    Children = new List<Node>();
                    Metadata = new List<int>();
                }
            }

            public static int Day8_1(string input)
            {
                var numbers = input.Split(' ').Select(int.Parse).ToArray();
                int index   = 0;
                var root    = GetInputTree(numbers, ref index);

                return GetResultCheck1(root);
            }

            public static int Day8_2(string input)
            {
                var numbers = input.Split(' ').Select(int.Parse).ToArray();
                int index   = 0;
                var root    = GetInputTree(numbers, ref index);

                return GetResultCheck2(root);
            }

            private static int GetResultCheck1(Node node)
            {
                int ret = 0;
                foreach (var child in node.Children)
                {
                    ret += GetResultCheck1(child);
                }

                ret += node.Metadata.Sum();

                return ret;
            }

            private static int GetResultCheck2(Node node)
            {
                int ret = 0;

                if (!node.Children.Any())
                {
                    return node.Metadata.Sum();
                }

                var children = node.Children.ToArray();
                foreach (int i in node.Metadata)
                {
                    if (i <= children.Length)
                    {
                        ret += GetResultCheck2(children[i - 1]);
                    }
                }

                return ret;
            }

            private static Node GetInputTree(IReadOnlyList<int> input, ref int index)
            {
                (int children, int metadata) = (input[index++], input[index++]);

                var node = new Node();

                for (int i = 0; i < children; i++)
                {
                    node.Children.Add(GetInputTree(input, ref index));
                }

                for (int i = 0; i < metadata; i++)
                {
                    node.Metadata.Add(input[index++]);
                }

                return node;
            }
        }
    }
}
