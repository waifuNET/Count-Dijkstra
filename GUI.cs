using graphWF.windows;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace graphWF
{
    public static class GUI
    {
        public static int CursorX = Cursor.Position.X;
        public static int CursorY = Cursor.Position.Y;

        public static int OffsetX = 0;
        public static int OffsetY = 0;

        public static int GUIOffsetX = 0;
        public static int GUIOffsetY = 0;

        public static int scaleOffset = 50;

        public static List<Button> buttonsList = new List<Button>();
        public static List<Label> labelList = new List<Label>();

        public static bool findPath = false;

        class Edge
        {
            public string ver1;
            public string ver2;

            public Point start;
            public Point stop;

            public bool Fstart;
            public bool Fstop;

            public Edge()
            {
                this.start = new Point();
                this.stop = new Point();

                this.Fstart = false;
                this.Fstop = false;
            }

            public Edge(Point start, Point stop)
            {
                this.start = start;
                this.stop = stop;

                this.Fstart = true;
                this.Fstop = true;
            }
        }

        public static void Init()
        {
            GDEXControl.Init();
            Task.Run(async () => _Update());
        }

        public static void AddVertex(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Temp = new Edge();

                int _localX = CursorX;
                int _localY = CursorY;
                using (dialoge form = new dialoge())
                {
                    form.Text = "Введите имя вершины";
                    DialogResult result = form.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        if (GDEXControl.dex.graph.Find(x => x.name == form.value) != null)
                        {
                            MessageBox.Show("Такая вершина уже существует!", "Ошибка", MessageBoxButtons.OK);
                            return;
                        }

                        Button button = new Button();
                        button.Size = new Size(50, 50);
                        button.Location = new Point(_localX - button.Size.Width / 2, _localY - button.Size.Height / 2);
                        button.BackColor = Color.Transparent;
                        button.FlatStyle = FlatStyle.Flat;
                        button.BackgroundImage = Properties.Resources.sircle;
                        button.BackgroundImageLayout = ImageLayout.Zoom;
                        button.Text = form.value;
                        button.ForeColor = Color.White;
                        if (form.value.Length <= 2)
                            button.Font = new Font(button.Font.Name, 14);
                        else
                            button.Font = new Font(button.Font.Name, 8);

                        button.MouseClick += AddEdge;
                        button.MouseDown += GDEXControl.RemoveEdges;

                        GDEXControl.AddVertex(form.value.Trim(), new Point(_localX, _localY), button);
                        buttonsList.Add(button);

                        Program.form1.Invoke(new Action(() =>
                        {
                            Program.form1.panel1.Controls.Add(button);
                        }));
                    }
                }
                GUI.UpdateGUI(); // for windows < 10
            }
        }

        public static void AddVertexFromSave(string name, Point location)
        {
            Temp = new Edge();

            Button button = new Button();
            button.Size = new Size(50, 50);
            button.Location = location;
            button.BackColor = Color.Transparent;
            button.FlatStyle = FlatStyle.Flat;
            button.BackgroundImage = Properties.Resources.sircle;
            button.BackgroundImageLayout = ImageLayout.Zoom;
            button.Text = name;
            button.ForeColor = Color.White;
            if (name.Length <= 2)
                button.Font = new Font(button.Font.Name, 14);
            else
                button.Font = new Font(button.Font.Name, 8);

            button.MouseClick += AddEdge;
            button.MouseDown += GDEXControl.RemoveEdges;

            GDEXControl.AddVertex(name.Trim(), new Point(location.X, location.Y), button);
            buttonsList.Add(button);

            Program.form1.Invoke(new Action(() =>
            {
                Program.form1.panel1.Controls.Add(button);
            }));
            GUI.UpdateGUI(); // for windows < 10
        }

        static Edge Temp = new Edge();
        public static void AddEdge(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                foreach (Dex.Vertex ver in GDEXControl.dex.graph)
                {
                    if (sender == ver.button)
                    {
                        if (!Temp.Fstart)
                        {
                            Temp.Fstart = true;
                            Temp.ver1 = ver.name;
                            Temp.start = ver.position;
                        }
                        else if (!Temp.Fstop)
                        {
                            if (Temp.ver1 != ver.name)
                            {
                                Temp.Fstop = true;
                                Temp.ver2 = ver.name;
                                Temp.stop = ver.position;
                            }
                        }

                        if (Temp.Fstop && Temp.Fstart)
                        {
                            if (!findPath)
                            {
                                using (dialoge form = new dialoge())
                                {
                                    form.type = "int";
                                    form.default_rich = ((int)Math.Sqrt(Math.Pow(GDEXControl.GetVertex(Temp.ver2).button.Location.X - GDEXControl.GetVertex(Temp.ver1).button.Location.X, 2) + Math.Pow(GDEXControl.GetVertex(Temp.ver2).button.Location.Y - GDEXControl.GetVertex(Temp.ver1).button.Location.Y, 2)) / 4).ToString();
                                    form.Text = "Введите цену ребра " + Temp.ver1 + " => " + Temp.ver2;
                                    DialogResult result = form.ShowDialog();
                                    if (result == DialogResult.OK)
                                    {
                                        int price = 0;
                                        if (Int32.TryParse(form.value, out int res))
                                            price = res;

                                        Label label = new Label();
                                        label.Text = price.ToString();
                                        label.BackColor = Color.Black;
                                        label.ForeColor = Color.White;
                                        label.Font = new Font(label.Font.Name, 14);
                                        label.AutoSize = true;

                                        label.Location = new Point((Temp.start.X + Temp.stop.X) / 2, (Temp.start.Y + Temp.stop.Y) / 2);

                                        Program.form1.Invoke(new Action(() =>
                                        {
                                        //Program.form1.panel1.Controls.Add(label);
                                    }));

                                        GDEXControl.addEdge(Temp.ver1, Temp.ver2, price, label);
                                        //labelList.Add(label);

                                        UpdateGUI();
                                    }
                                }
                            }
                            else {
                                GDEXControl.MathGraph(Temp.ver1, Temp.ver2);
                            }
                            GUI.UpdateGUI(); // for windows < 10
                            Temp = new Edge();
                        }
                    }
                }
            }
        }

        static async Task _Update()
        {
            while (true)
            {
                Program.form1.Invoke(new Action(() =>
                {
                    CursorX = Program.form1.panel1.PointToClient(Cursor.Position).X;
                    CursorY = Program.form1.panel1.PointToClient(Cursor.Position).Y;

                    if (CursorX > Program.form1.panel1.Width + OffsetX)
                        CursorX = Program.form1.panel1.Width + OffsetX;
                    else if (CursorX <= 0)
                        CursorX = 0;

                    if (CursorY > Program.form1.panel1.Height + OffsetY)
                        CursorY = Program.form1.panel1.Height + OffsetY;
                    else if (CursorY <= 0)
                        CursorY = 0;

                    Program.form1.label1.Text = (CursorX + GUIOffsetX) + " : " + (CursorY + GUIOffsetY);

                    for (int i = 0; i < buttonsList.Count; i++)
                    {
                        Size size = new Size(scaleOffset, scaleOffset);
                        buttonsList[i].Size = size;
                    }

                    for (int i = 0; i < buttonsList.Count; i++)
                    {
                        Point p = buttonsList[i].Location;
                        p = new Point(p.X + OffsetX, p.Y + OffsetY);
                        buttonsList[i].Location = p;
                    }

                    for (int i = 0; i < labelList.Count; i++)
                    {
                        Point p = labelList[i].Location;
                        p = new Point(p.X + OffsetX, p.Y + OffsetY);
                        labelList[i].Location = p;
                    }

                }));

                if(OffsetX != 0 || OffsetY != 0)
                    GUI.UpdateGUI();

                await Task.Delay(10);
            }
        }

        public static void UpdateGUI()
        {
            Program.form1.Invoke(new Action(() =>
            {
                Graphics formGraphics = Program.form1.panel1.CreateGraphics();
                formGraphics.Clear(Color.White);

                if (GDEXControl.dex.graph.Count > 1)
                {
                    Pen myPen = new Pen(Color.Black, 2);
                    Pen myPenRed = new Pen(Color.Red, 2);
                    formGraphics.Clear(Color.White);
                    formGraphics.SmoothingMode = SmoothingMode.AntiAlias;

                    for (int i = 0; i < GDEXControl.dex.graph.Count; i++)
                    {
                        for (int e = 0; e < GDEXControl.dex.graph[i].edges.Count; e++)
                        {
                            Point p1 = new Point(GDEXControl.dex.graph[i].button.Location.X + GDEXControl.dex.graph[i].button.Size.Width / 2,
                                GDEXControl.dex.graph[i].button.Location.Y + GDEXControl.dex.graph[i].button.Size.Height / 2);
                            Point p2 = new Point(GDEXControl.dex.graph[i].edges[e].vertex.button.Location.X + GDEXControl.dex.graph[i].edges[e].vertex.button.Size.Width / 2,
                                GDEXControl.dex.graph[i].edges[e].vertex.button.Location.Y + GDEXControl.dex.graph[i].edges[e].vertex.button.Size.Height / 2);
                            formGraphics.DrawLine(myPen, p1, p2);

                            Point textP = new Point((p1.X + p2.X) / 2, (p1.Y + p2.Y) / 2);

                            SizeF size = formGraphics.MeasureString(GDEXControl.dex.graph[i].edges[e].price.ToString(), new Font("Microsoft Sans Serif", 14));
                            formGraphics.FillRectangle(Brushes.Black, new Rectangle(textP.X, textP.Y, (int)size.Width, (int)size.Height));
                            formGraphics.DrawString(GDEXControl.dex.graph[i].edges[e].price.ToString(), new Font("Microsoft Sans Serif", 14), Brushes.Red, textP);
                        }
                    }

                    for (int jojo = 0; jojo < GDEXControl.dex.pathVertexes.Count - 1; jojo++)
                    {
                        Point p1 = new Point(GDEXControl.GetVertex(GDEXControl.dex.pathVertexes[jojo]).button.Location.X + GDEXControl.GetVertex(GDEXControl.dex.pathVertexes[jojo]).button.Size.Width / 2,
                             GDEXControl.GetVertex(GDEXControl.dex.pathVertexes[jojo]).button.Location.Y + GDEXControl.GetVertex(GDEXControl.dex.pathVertexes[jojo]).button.Size.Height / 2);
                        Point p2 = new Point(GDEXControl.GetVertex(GDEXControl.dex.pathVertexes[jojo + 1]).button.Location.X + GDEXControl.GetVertex(GDEXControl.dex.pathVertexes[jojo + 1]).button.Size.Width / 2,
                           GDEXControl.GetVertex(GDEXControl.dex.pathVertexes[jojo + 1]).button.Location.Y + GDEXControl.GetVertex(GDEXControl.dex.pathVertexes[jojo + 1]).button.Size.Height / 2);

                        formGraphics.DrawLine(myPenRed, p1, p2);
                    }

                    for (int jojo = 0; jojo < GDEXControl.dex.pathVertexes.Count; jojo++)
                    {
                        Point textP = new Point(GDEXControl.GetVertex(GDEXControl.dex.pathVertexes[jojo]).button.Location.X + 25, GDEXControl.GetVertex(GDEXControl.dex.pathVertexes[jojo]).button.Location.Y - 25);
                        SizeF size = formGraphics.MeasureString(GDEXControl.GetVertex(GDEXControl.dex.pathVertexes[jojo]).price.ToString(), new Font("Microsoft Sans Serif", 14));
                        formGraphics.FillRectangle(Brushes.White, new Rectangle(textP.X, textP.Y, (int)size.Width, (int)size.Height));
                        formGraphics.DrawString(GDEXControl.GetVertex(GDEXControl.dex.pathVertexes[jojo]).price.ToString(), new Font("Microsoft Sans Serif", 14), Brushes.Red, textP);
                    }

                    myPen.Dispose();
                    formGraphics.Dispose();
                }

                formGraphics.Dispose();
            }));
        }
    }
}
