using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace CSV_to_JSON
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public List<string> tags = new List<string>();

        public string[] lines = null;

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        public void OpenCSV(string file)
        {
            lines = File.ReadAllLines(file);

            for (int i = 0; i < lines.Length; i++)
            {
                if (i == 0)
                {
                    textBox1.Text = lines[i];
                    string[] col = lines[i].Split(',');

                    for (int v = 0; v < col.Length; v++)
                    {
                        dataGridView1.Columns.Add(v.ToString(), col[v]);
                    }
                }
                else
                {
                    string[] rw = lines[i].Split(',');
                    dataGridView1.Rows.Add(1);

                    for (int v = 0; v < rw.Length; v++)
                    {
                        
                        dataGridView1.Rows[i-1].Cells[v].Value = rw[v];
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "CSV Files(*.csv)|*.csv";
            ofd.ShowDialog();

            OpenCSV(ofd.FileName);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (lines != null)
            {
                if (lines.Length > 0)
                {
                    string[] tg = textBox1.Text.Split(',');
                    tags = tg.ToList();

                    List<string> json = new List<string>();

                    string[] oldTags = lines[0].Split(',');

                    for (int i = 1; i < lines.Length; i++)
                    {
                        string[] vals = lines[i].Split(',');

                        string nLine = "";

                        for (int v = 0; v < vals.Length; v++)
                        {
                            if (tags.Contains(oldTags[v]))
                            {
                                nLine += vals[v] + ",";
                            }
                        }

                        nLine = nLine.Remove(nLine.Length - 1);

                        json.Add(nLine);
                    }

                    File.WriteAllText("csvtojs.json", ConvertLinesToJSON(json));
                }
                else
                {
                    MessageBox.Show("Load A File First");
                }
            }
            else
            {
                MessageBox.Show("Load A File First");
            }
        }

        public string ConvertLinesToJSON(List<string> Lns)
        {
            string json = "{ \"data\": {";

            for (int i = 0; i < Lns.Count; i++)
            {
                json += " \"" + i.ToString() +"\": {";

                string[] lin = Lns[i].Split(',');

                for(int j = 0; j < lin.Length; j++)
                {
                    json += "\"" + tags[j] + "\": ";
                    json += "\"" + lin[j] + "\"";

                    if (j < lin.Length - 1) json += ",";
                }

                json += "}";

                if (i < Lns.Count - 1) json += ",";
            }

            json += "}}";

            return json;
        }
    }
}
