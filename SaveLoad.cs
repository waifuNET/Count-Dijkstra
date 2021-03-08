using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace graphWF
{
    public class SaveLoad
    {
        public class sv_edges
        {
            public string from;
            public string to;

            public int price;
        }

        public class sv_button
        {
            public string name;
            public Point location;
        }

        public class sv
        {
            public string projectName;

            public int GUIOffsetX;
            public int GUIOffsetY;

            public int scaleOffset;

            public List<sv_button> buttons = new List<sv_button>();
            public List<sv_edges> edges = new List<sv_edges>();
        }

        public static void Save()
        {
            sv save = new sv();
            save.projectName = Program.form1.ProjectName;
            save.GUIOffsetX = GUI.GUIOffsetX;
            save.GUIOffsetY = GUI.GUIOffsetY;
            save.scaleOffset = GUI.scaleOffset;

            for(int i = 0; i < GDEXControl.dex.graph.Count; i++)
            {
                sv_button button = new sv_button();
                button.location = GDEXControl.dex.graph[i].button.Location;
                button.name = GDEXControl.dex.graph[i].name;

                for(int e = 0; e < GDEXControl.dex.graph[i].edges.Count; e++)
                {
                    sv_edges edge = new sv_edges();
                    edge.from = button.name;
                    edge.to = GDEXControl.dex.graph[i].edges[e].vertex.name;
                    edge.price = GDEXControl.dex.graph[i].edges[e].price;

                    save.edges.Add(edge);
                }

                save.buttons.Add(button);
            }

            string output = JsonConvert.SerializeObject(save);

            using (StreamWriter sw = new StreamWriter(Program.form1.pathToSave, false, System.Text.Encoding.Default))
            {
                sw.WriteLine(output);

                sw.Close();
                sw.Dispose();
            }
        }

        public static void Load(string file)
        {
            GDEXControl.removeAll();

            sv save = new sv();
            using (StreamReader sr = new StreamReader(file))
            {
                save = JsonConvert.DeserializeObject<sv>(sr.ReadToEnd());

                sr.Close();
                sr.Dispose();
            }

            Program.form1.Invoke(new Action(() =>
            {
                Program.form1.ProjectName = save.projectName;
                Program.form1.LoadPath();
            }));
            GUI.GUIOffsetX = save.GUIOffsetX;
            GUI.GUIOffsetY = save.GUIOffsetY;

            GUI.scaleOffset = save.scaleOffset;

            for (int i = 0; i < save.buttons.Count; i++)
            {
                GUI.AddVertexFromSave(save.buttons[i].name, save.buttons[i].location);
            }

            for (int i = 0; i < save.edges.Count; i++)
            {
                GDEXControl.addEdge(save.edges[i].from, save.edges[i].to, save.edges[i].price);
            }
        }
    }
}
