using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;


namespace Lab4inf
{
    public partial class Form1 : Form
    {
        Bitmap panelBitmap;
       
        public Form1()
        {
            InitializeComponent();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "image files(*.PNG;*.JPG;*.TIFF;*.PSD;*.BMP)|*.PNG;*.JPG;*.TIFF;*.PSD;*.BMP";
            if (openFile.ShowDialog() == DialogResult.OK)
            {               
                 pictureBox1.Image = new Bitmap(openFile.FileName);
                 FileInfo inf = new FileInfo(openFile.FileName);
                 pictureBox1.Name = inf.Name;
                flag = false;
            }
        }
        Bitmap[] arraybitmap = new Bitmap[1000];
        string[] arrayname = new string[1000];
        private void openDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if(fbd.ShowDialog() == DialogResult.OK)
            {
                int i = 0;
                foreach (string imageFile in Directory.EnumerateFiles(fbd.SelectedPath))
                {
                    try
                    {
                        Bitmap img = new Bitmap(imageFile);
                        i++;
                    }
                    catch (Exception ex)
                    {
                    
                    }
                }
               
                panelBitmap = new Bitmap(panel1.Width, 90 * i + 15);
                panel1.AutoScrollMinSize = new Size(0, 90 * i + 15);
                i = 0;
                foreach(string imageFile in Directory.EnumerateFiles(fbd.SelectedPath))
                {
                    try
                    {
                        Bitmap img = new Bitmap(imageFile);
                        arraybitmap[i] = img;
                        FileInfo inf = new FileInfo(imageFile);                   
                        arrayname[i] = inf.Name;
                        Graphics dc = Graphics.FromImage(panelBitmap);
                        dc.DrawImage(img, 10, 90 * i + 15, 90, 90);
                        i++;
                    }
                    catch(Exception ex)
                    {

                    }
                }
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Transform = new System.Drawing.Drawing2D.Matrix(1, 0, 0, 1, panel1.AutoScrollPosition.X, panel1.AutoScrollPosition.Y);
            if (panelBitmap != null)
                e.Graphics.DrawImage(panelBitmap, 0, 0);
        }
        int indexOfSelectImg ;
        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            int realY = e.Y - panel1.AutoScrollPosition.Y;
            indexOfSelectImg = realY;
            pictureBox1.Image = arraybitmap[(realY - 15)/90];
            pictureBox1.Name = arrayname[(realY - 15) / 90];
        }

        private void copyrightTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            text = Microsoft.VisualBasic.Interaction.InputBox("Enter the text:");
        }
        string text = "@Copyrighter";
        string path = @"C:\Users\Sevch\Desktop\Lab4\Lab4\Lab4iikt";
        private void copyrightDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
                path = fbd.SelectedPath;
            
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        bool flag = true;
        private void button1_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                Graphics g = Graphics.FromImage(pictureBox1.Image);
                g.DrawString(text, new Font("Tahoma", 400), new SolidBrush(Color.GreenYellow), 15, 15);
                pictureBox1.Refresh();
                TimeSpan time = DateTime.Now.TimeOfDay;
                dataGridView1.Rows.Add(pictureBox1.Name, pictureBox1.Image.Width, pictureBox1.Image.Height, text + " [" + time + "]");
                if (panelBitmap!=null&&flag)
                {
                    try
                    {
                        Graphics gr = Graphics.FromImage(panelBitmap);
                        var img = Image.FromFile(@"C:\Users\Sevch\Desktop\Lab4\Lab4\Lab4iikt\ok.png");
                        gr.DrawImage(img, 15, indexOfSelectImg, 30, 30);                        
                        panel1.Refresh();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error");
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog { DefaultExt = ".PNG", Filter = "image files(*.PNG;*.JPG;*.TIFF;*.PSD;*.BMP)|*.PNG;*.JPG;*.TIFF;*.PSD;*.BMP" } ;
            if (save.ShowDialog() == DialogResult.OK)
                pictureBox1.Image.Save(save.FileName);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (panelBitmap!=null)
            {
                for (int i = 0; arraybitmap[i] != null; i++)
                {
                    Graphics g = Graphics.FromImage(arraybitmap[i]);
                    g.DrawString(text, new Font("Tahoma", 400), new SolidBrush(Color.GreenYellow), 15, 15);
                    TimeSpan time = DateTime.Now.TimeOfDay;
                    dataGridView1.Rows.Add(arrayname[i], arraybitmap[i].Width, arraybitmap[i].Height, text + " [" + time + "]");
                        try
                        {
                            Graphics gr = Graphics.FromImage(panelBitmap);
                            var img = Image.FromFile(@"C:\Users\Sevch\Desktop\Lab4\Lab4\Lab4iikt\ok.png");
                            gr.DrawImage(img, 15, i*90+15, 30, 30);
                            panel1.Refresh();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error");
                        }
                    string namePath = path + "\\" + arrayname[i]; 
                    arraybitmap[i].Save(namePath, System.Drawing.Imaging.ImageFormat.Bmp);
                }
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (panelBitmap != null)
                {
                    int max = 0;
                    for (int i = 0; arraybitmap[i] != null; i++)
                    {
                        if (i >= (indexOfSelectImg - 15) / 90)
                        {
                            arraybitmap[i] = arraybitmap[i + 1];
                            arrayname[i] = arrayname[i + 1];
                            max = i;
                        }
                    }
                    arraybitmap[max + 1] = null;
                    panelBitmap = new Bitmap(panel1.Width, 90 * max + 15);
                    panel1.AutoScrollMinSize = new Size(0, 90 * max + 15);
                    int p = 0;
                    while(arraybitmap[p]!=null)
                    {                      
                        try
                        {
                            Graphics gr = Graphics.FromImage(panelBitmap);
                            gr.DrawImage(arraybitmap[p], 10, 90 * p + 15, 90, 90);
                            p++;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error");
                        }

                    }
                    panel1.Refresh();
                }
                pictureBox1.Image = null;
            }
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Made by Zipchenko");
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
