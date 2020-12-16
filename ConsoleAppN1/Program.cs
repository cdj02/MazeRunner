using NodeModel.MazeGraph;
using NodeModel.GraphModel;
using System;
using PathfindAlgorithm.BFSAlgorithm;
using PathfindAlgorithm.Dijkstra;
using NodeModel.Pathfind;

namespace ConsoleAppN1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");



            //MazeGraph map = MazeGraphFactory.CreateGridNodeMap(50, 50);

            var map = new Graph();

            var Node1 = new Node("Node A_1");
            var Node2 = new Node("Node A_2");
            var Node3 = new Node("Node A_3");
            var Node4 = new Node("Node B_1");
            var Node5 = new Node("Node B_2");
            var Node6 = new Node("Node B_3");
            var Node7 = new Node("Node C_1");
            var Node8 = new Node("Node C_2");
            var Node9 = new Node("Node C_3");

            map.Nodes.Add(Node1);
            map.Nodes.Add(Node2);
            map.Nodes.Add(Node3);
            map.Nodes.Add(Node4);
            map.Nodes.Add(Node5);
            map.Nodes.Add(Node6);
            map.Nodes.Add(Node7);
            map.Nodes.Add(Node8);
            map.Nodes.Add(Node9);

            map.AddConnection("con1", Node1, Node2, 10);
            map.AddConnection("con2", Node1, Node3, 1);
            map.AddConnection("con3", Node2, Node3, 1);




            Console.WriteLine("nr of nodes : " + map.Nodes.Count);
            Console.WriteLine("nr of connections : " + map.Connections.Count);
            
            Console.WriteLine("-----------------------");
            map.Nodes.ForEach( n => Console.WriteLine("node : " + n.Id));
            
            Console.WriteLine("-----------------------");
            map.Connections.ForEach(n => Console.WriteLine("connection " + n.Id));

            
            foreach(var node in map.Nodes)
            {
                Console.WriteLine("-----------------------");
                node.Connections.ForEach(x => Console.WriteLine($"{node.Id} -> {x.NodeTo(node).Id} Weight = {x.Weight}"));
            }


            map.StartNode = Node1;
            map.EndNode = Node2;

            Console.WriteLine("start berekenen");

            Dijkstra test = new Dijkstra();
            var resultaat = test.Calculate(map);
            
            Console.WriteLine($"tijd in miliseconden : {resultaat.ElapsedMilliseconds}");

            Console.WriteLine($"oplossing gevonden   : {resultaat.SolutionFound}");
            foreach (var solNode in resultaat.Solution)
            {
                Console.WriteLine($"{solNode.Id}");
            }
            Console.WriteLine($"{map.EndNode.Id}");


        }
    }
}
