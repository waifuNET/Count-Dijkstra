using graphWF.windows;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace graphWF
{
    public class GDEXControl
    {
        public static Dex dex = new Dex();
        public static void Init()
        {
            Program.form1.Invoke(new Action(() => { Program.form1.panel1.MouseClick += GUI.AddVertex; }));
            Program.form1.Invoke(new Action(() => { Program.form1.panel1.MouseDown += ChangeOffset; }));
            Program.form1.Invoke(new Action(() => { Program.form1.panel1.MouseUp += ChangeOffsetOut; }));

            Task.Run(async () => Offset());
        }

        static int CursorOffsetX = 0;
        static int CursorOffsetY = 0;
        public static async Task Offset()
        {
            while (true)
            {
                CursorOffsetX = GUI.CursorX;
                CursorOffsetY = GUI.CursorY;

                while (changeOffsets)
                {
                    GUI.OffsetX += (CursorOffsetX - GUI.CursorX) / 32;
                    GUI.OffsetY += (CursorOffsetY - GUI.CursorY) / 32;

                    GUI.GUIOffsetX += GUI.OffsetX;
                    GUI.GUIOffsetY += GUI.OffsetY;

                    await Task.Delay(10);
                }

                GUI.OffsetX = 0;
                GUI.OffsetY = 0;
                await Task.Delay(50);
            }
        }

        public static void AddVertex(string name, Point position, Button button)
        {
            dex.addVertex(name, position, button);
        }

        public static void addEdge(string ver1, string ver2, int price, Label labelPrice = null)
        {
            dex.addEdge(ver1, ver2, price, labelPrice);
        }

        static bool changeOffsets = false;
        public static void ChangeOffset(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                changeOffsets = true;
            }
        }

        public static void ChangeOffsetOut(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                changeOffsets = false;
            }
        }

        public static void RemoveEdges(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                foreach (Dex.Vertex ver in dex.graph)
                {
                    if (sender == ver.button)
                    {
                        using (dialoge form = new dialoge())
                        {
                            form.Text = "Введите имя ребра для удаления";
                            DialogResult result = form.ShowDialog();
                            if (result == DialogResult.OK)
                            {
                                for (int i = 0; i < ver.edges.Count; i++)
                                {
                                    if (ver.edges[i].vertex.name == form.value)
                                    {
                                        for (int j = 0; j < ver.edges[i].vertex.edges.Count; j++)
                                        {
                                            if (ver.edges[i].vertex.edges[j].vertex.name == ver.name)
                                            {
                                                ver.edges[i].vertex.edges.RemoveAt(j);
                                            }
                                        }
                                        Program.form1.Invoke(new Action(() =>
                                        {
                                            Program.form1.panel1.Controls.Remove(ver.edges[i].labelPrice);
                                        }));
                                        ver.edges.RemoveAt(i);
                                    }
                                }

                                GUI.UpdateGUI();
                            }
                        }
                    }
                }
            }
        }

        public static void MathGraph(string ver1, string ver2)
        {
            dex.pathVertexes.Clear();
            if (dex.FindVertex(ver1) && dex.FindVertex(ver2))
            {
                dex.mathVertexPrice(ver1, ver2);
                dex.printPath(ver1, ver2);

                //string temp = "";
                //for (int i = 0; i < dex.pathVertexes.Count; i++)
                //{
                //    temp += dex.pathVertexes[i] + "\n";
                //}

                //MessageBox.Show(temp);

                GUI.UpdateGUI();
            }
        }

        public static void RemoveAllVertexes(bool quest = true)
        {
            bool result = false;
            if (quest)
            {
                DialogResult dialogResult = MessageBox.Show("Вы действительно хотите удалить весь граф?", "Внимание", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    result = true;
                }
            }
            else
            {
                result = true;
            }
            if (result)
            {
                dex.pathVertexes.Clear();
                for (int i = 0; i < dex.graph.Count; i++)
                {
                    _RemoveVertex(dex.graph[i].name);
                    i = 0;
                }
                dex.graph.Clear();

                Program.form1.Invoke(new Action(() =>
                {
                    Program.form1.panel1.Controls.Clear();
                }));
            }
            GUI.UpdateGUI(); // for windows < 10
        }

        public static void RemoveVertex()
        {
            dex.pathVertexes.Clear();
            using (dialoge form = new dialoge())
            {
                form.Text = "Введите имя вершины для удаления";
                DialogResult result = form.ShowDialog();
                if (result == DialogResult.OK)
                {
                    if (dex.FindVertex(form.value))
                    {
                        _RemoveVertex(form.value);
                    }
                }

            }
            GUI.UpdateGUI(); // for windows < 10
        }

        static void _RemoveVertex(string name)
        {
            for (int i = 0; i < dex.graph.Count; i++)
            {
                for (int j = 0; j < dex.graph[i].edges.Count; j++)
                {
                    if (dex.graph[i].edges[j].vertex.name == name)
                    {
                        dex.graph[i].edges.RemoveAt(j);
                        j = 0;
                    }
                }
            }

            for (int i = 0; i < dex.graph.Count; i++)
            {
                if (dex.graph[i].name == name)
                {
                    for (int j = 0; j < dex.graph[i].edges.Count; j++)
                    {
                        if (dex.graph[i].edges[j].vertex.name == name)
                        {
                            dex.graph[i].edges.RemoveAt(j);
                            j = 0;
                        }
                    }
                }
            }

            for (int i = 0; i < dex.graph.Count; i++)
            {
                if (dex.graph[i].name == name)
                {
                    Program.form1.Invoke(new Action(() =>
                    {
                        Program.form1.panel1.Controls.Remove(dex.graph[i].button);
                    }));
                    dex.graph.RemoveAt(i);
                }
            }

            GUI.UpdateGUI();
        }

        public static Dex.Vertex GetVertex(string name)
        {
            return dex.VertexByName(name.Trim());
        }

        public static void removeAll()
        {
            GUI.findPath = true;

            RemoveAllVertexes(false);
            dex.graph.Clear();
            dex.pathVertexes.Clear();

            changeOffsets = false;

            GUI.GUIOffsetX = 0;
            GUI.GUIOffsetY = 0;

            GUI.OffsetX = 0;
            GUI.OffsetY = 0;

            GUI.scaleOffset = 50;

            GUI.buttonsList.Clear();
            GUI.labelList.Clear();

            Program.form1.Invoke(new Action(() =>
            {
                Program.form1.ProjectName = "Default";
                Program.form1.CheckSS();
            }
            ));

            GUI.UpdateGUI();
        }
    }
}
