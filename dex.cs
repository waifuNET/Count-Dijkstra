using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace graphWF
{
    public class Dex
    {
        public class Edge
        {
            public Vertex vertex;
            public int price { get; set; }
            public Label labelPrice { get; set; }
        }

        public class Vertex
        {
            public bool bannded = false;

            public string name = "";
            public int price = Int32.MaxValue;
            public List<Edge> edges = new List<Edge>();
            public Vertex parent;

            // gui //
            public Point position = new Point();
            public Button button;

            public Vertex(string name, Point position, Button button)
            {
                this.name = name;
                this.position = position;
                this.button = button;
            }
        }

        public List<string> pathVertexes = new List<string>();

        public List<Vertex> graph = new List<Vertex>();

        public void addVertex(string name, Point position, Button button)
        {
            Vertex vertex = new Vertex(name, position, button);

            graph.Add(vertex);
        }

        public bool FindVertex(string name)
        {
            for(int i = 0; i < graph.Count; i++)
            {
                if (graph[i].name == name)
                    return true;
            }
            return false;
        }

        public void addEdge(string ver1, string ver2, int price, Label labelPrice = null)
        {
            for (int i = 0; i < graph.Count; i++)
            {
                if (graph[i].name == ver1)
                {
                    for (int j = 0; j < graph.Count; j++)
                    {
                        if (graph[j].name == ver2)
                        {
                            graph[i].edges.Add(new Edge { vertex = graph[j], price = price, labelPrice = labelPrice });
                            graph[j].edges.Add(new Edge { vertex = graph[i], price = price, labelPrice = labelPrice });
                        }
                    }
                }
            }
        }

        public int min(List<Vertex> ver)
        {
            int minIndex = 0;
            Vertex min = ver[0];
            for (int i = 0; i < ver.Count; i++)
            {
                if (min.price >= ver[i].price)
                {
                    min = ver[i];
                    minIndex = i;
                }
            }
            return minIndex;
        }

        public Vertex VertexByName(string name)
        {
            for (int i = 0; i < graph.Count; i++)
            {
                if (graph[i].name == name)
                    return graph[i];
            }
            return null;
        }

        public void mathVertexPrice(string v1, string v2)
        {
            start = null;
            for(int i = 0; i < graph.Count; i++)
            {
                graph[i].bannded = false;
                graph[i].parent = null;
                graph[i].price = Int32.MaxValue;
            }
            mathVertexPrice(VertexByName(v1), VertexByName(v2));
        }

        public void mathVertexsEdge(Vertex vertex)
        {
            for (int i = 0; i < vertex.edges.Count; i++)
            {
                if (!vertex.edges[i].vertex.bannded)
                {
                    if (vertex.price + vertex.edges[i].price < vertex.edges[i].vertex.price)
                    {
                        vertex.edges[i].vertex.price = vertex.price + vertex.edges[i].price;
                        vertex.edges[i].vertex.parent = vertex;
                    }
                }
            }
        }

        Vertex start = null;
        public void mathVertexPrice(Vertex startV, Vertex endV)
        {
            Console.WriteLine("Vertex: " + startV.name + " : " + startV.bannded + " | ");
            if (start == null)
            {
                startV.price = 0;
                start = startV;
            }
            startV.bannded = true;

            List<Vertex> vertices = new List<Vertex>();

            mathVertexsEdge(startV);

            for (int i = 0; i < graph.Count; i++)
            {
                if (!graph[i].bannded)
                {
                    vertices.Add(graph[i]);
                }
            }

            if (vertices.Count <= 0)
                return;

            int minIndex = min(vertices);
            mathVertexPrice(vertices[minIndex], endV);
            vertices.Clear();
        }

        public void printVertexAndSmej()
        {
            for (int i = 0; i < graph.Count; i++)
            {
                Console.WriteLine(graph[i].name + ": " + graph[i].price);
            }
            Console.WriteLine();
        }

        public void printPath(string v1, string v2)
        {
            pathVertexes.Clear();
            _printPath(v1, v2);
            pathVertexes.Add(v2);
        }

        string _printPath(string v1, string v2)
        {
            string path = "";
            Vertex start = VertexByName(v1);
            Vertex end = VertexByName(v2);

            if (end.parent == null)
                return "";

            pathVertexes.Add(_printPath(start.name, end.parent.name) + end.parent.name);

            return path;
        }
    }
}
