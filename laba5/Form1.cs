using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using System.IO;
using Newtonsoft.Json;

namespace laba5
{
    public partial class Form1 : Form
    {
        public class Find
        {
            public Parsedresult[] ParsedResults { get; set; }
            public int OCRExitCode { get; set; }
        }
        public class Parsedresult
        {
            public object FileParseExitCode { get; set; }
            public string ParsedText { get; set; }
        }
        public string git { get; set; }
        public Form1()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 8;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            git = "";
            OpenFileDialog fd = new OpenFileDialog();
            if (fd.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = Image.FromFile(fd.FileName);
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                git = fd.FileName;
            }
        }
        private byte[] ImageToBase64(Image img, System.Drawing.Imaging.ImageFormat form)
        {
            using (MemoryStream MS = new MemoryStream())
            {
                img.Save(MS, form);
                byte[] imageBytes = MS.ToArray();

                return imageBytes;
            }
        }
        private string getSelectedLanguage()
        {
            string language = "";
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    language = "ara";
                    break;

                case 1:
                    language = "bul";
                    break;

                case 2:
                    language = "chs";
                    break;
                case 3:
                    language = "cht";
                    break;
                case 4:
                    language = "hrv";
                    break;
                case 5:
                    language = "cze";
                    break;
                case 6:
                    language = "dan";
                    break;
                case 7:
                    language = "dut";
                    break;
                case 8:
                    language = "eng";
                    break;
                case 9:
                    language = "fin";
                    break;
                case 10:
                    language = "fre";
                    break;
                case 11:
                    language = "ger";
                    break;
                case 12:
                    language = "gre";
                    break;
                case 13:
                    language = "hun";
                    break;
                case 14:
                    language = "kor";
                    break;
                case 15:
                    language = "ita";
                    break;
                case 16:
                    language = "jpn";
                    break;
                case 17:
                    language = "pol";
                    break;
                case 18:
                    language = "por";
                    break;
                case 19:
                    language = "rus";
                    break;
                case 20:
                    language = "slv";
                    break;
                case 21:
                    language = "spa";
                    break;
                case 22:
                    language = "swe";
                    break;
                case 23:
                    language = "tur";
                    break;
            }
            return language;
        }
        private async void button2_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            button2.Enabled = false;
            richTextBox1.Text = "";
                HttpClient httpC = new HttpClient();
                MultipartFormDataContent forma = new MultipartFormDataContent();
                forma.Add(new StringContent("K88884882688957"), "apikey"); 
                forma.Add(new StringContent(getSelectedLanguage()), "language");
                forma.Add(new StringContent("2"), "ocrengine");
                forma.Add(new StringContent("true"), "scale");
                forma.Add(new StringContent("true"), "istable");
                if (string.IsNullOrEmpty(git) == false)
                {
                    byte[] imgDate = File.ReadAllBytes(git);
                    forma.Add(new ByteArrayContent(imgDate, 0, imgDate.Length), "image", "image.jpg");
                }
                HttpResponseMessage answer = await httpC.PostAsync("https://api.ocr.space/Parse/Image", forma);
                string strContent = await answer.Content.ReadAsStringAsync();
                Find ans = JsonConvert.DeserializeObject<Find>(strContent);
                if (ans.OCRExitCode == 1)
                {
                    for (int i = 0; i < ans.ParsedResults.Count(); i++)
                    {
                        richTextBox1.Text = richTextBox1.Text + ans.ParsedResults[i].ParsedText;
                    }
                }
                else
                {
                    MessageBox.Show("ERROR: " + strContent);
                }
            button1.Enabled = true;
            button2.Enabled = true;
        }
    }
}
//d2ef7c2e1f88957