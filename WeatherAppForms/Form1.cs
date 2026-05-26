using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WeatherAppForms
{

    public partial class Form1 : Form
    {
        private static readonly HttpClient client = new HttpClient();

        public Form1()
        {
            InitializeComponent();
            //form setup
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(15, 20, 40);  
            this.Size = new Size(420, 580);
            this.Font = new Font("Segoe UI", 9f);
            this.ForeColor = Color.White;

            
            Panel pnlSearch = new Panel();
            pnlSearch.Size = new Size(378, 48);
            pnlSearch.Location = new Point(20, 20);
            pnlSearch.BackColor = Color.FromArgb(22, 29, 55);
            this.Controls.Add(pnlSearch);
            this.Controls.SetChildIndex(pnlSearch, 0);

            // textbox
            citytxt.Parent = pnlSearch;
            citytxt.Location = new Point(16, 14);
            citytxt.Size = new Size(290, 22);
            citytxt.BackColor = Color.FromArgb(22, 29, 55);  
            citytxt.ForeColor = Color.FromArgb(200, 210, 230);
            citytxt.Font = new Font("Segoe UI", 11f);
            citytxt.BorderStyle = BorderStyle.None;

            // btn
            searchbtn.Parent = pnlSearch;
            searchbtn.Location = new Point(328, 3);
            searchbtn.Size = new Size(42, 42);
            searchbtn.Text = "→";
            searchbtn.FlatStyle = FlatStyle.Flat;
            searchbtn.FlatAppearance.BorderSize = 0;
            searchbtn.ForeColor = Color.White;
            searchbtn.Cursor = Cursors.Hand;
            searchbtn.Font = new Font("Segoe UI", 14f);
            btnstyle(searchbtn,
                Color.FromArgb(55, 138, 221),
                Color.FromArgb(42, 109, 191));

            // icon config
            weatherIcon.Location = new Point(170, 100);
            weatherIcon.Size = new Size(80, 80);
            weatherIcon.BackColor = Color.Transparent;
            weatherIcon.SizeMode = PictureBoxSizeMode.Zoom;

            //labels
            lblCity.BackColor = Color.Transparent;
            lblCity.ForeColor = Color.FromArgb(140, 155, 180);
            lblCity.Font = new Font("Segoe UI", 11f);
            lblCity.Text = "";
            lblCity.AutoSize = true;
            lblCity.Location = new Point(0, 190);  
            lblCity.Anchor = AnchorStyles.None;

            lblTemp.BackColor = Color.Transparent;
            lblTemp.ForeColor = Color.White;
            lblTemp.Font = new Font("Segoe UI Light", 64f);
            lblTemp.Text = "";
            lblTemp.AutoSize = true;
            lblTemp.Location = new Point(0, 230);

            lblDesc.BackColor = Color.Transparent;
            lblDesc.ForeColor = Color.FromArgb(140, 155, 180);
            lblDesc.Font = new Font("Segoe UI", 11f);
            lblDesc.Text = "";
            lblDesc.AutoSize = true;

            // cent labels
            this.Resize += (s, e) => CentLabels();
            this.Load += (s, e) => CentLabels();

            // error
            lblError.BackColor = Color.Transparent;
            lblError.ForeColor = Color.FromArgb(255, 100, 100);
            lblError.Visible = false;
            lblError.Location = new Point(20, 490);

            // search func
            citytxt.KeyDown += (s, e) => {
                if (e.KeyCode == Keys.Enter) searchbtn_Click(s, e);
            };
        }
        private void CentLabels()
        {
            lblCity.Left = (this.ClientSize.Width - lblCity.Width) / 2;
            lblTemp.Left = (this.ClientSize.Width - lblTemp.Width) / 2;
            lblDesc.Left = (this.ClientSize.Width - lblDesc.Width) / 2;
            weatherIcon.Left = (this.ClientSize.Width - weatherIcon.Width) / 2;
        }
        private void btnstyle(Button btn, 
            System.Drawing.Color normalcolor,
            System.Drawing.Color hovercolor)
        {
            btn.BackColor = normalcolor;
            btn.MouseEnter += (s, e) => { btn.BackColor = hovercolor; };
            btn.MouseLeave += (s, e) => { btn.BackColor = normalcolor; };
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private async void searchbtn_Click(object sender, EventArgs e)
        {
            string city = citytxt.Text.Trim();
            if (string.IsNullOrEmpty(city)) return;    

            //loadind
            searchbtn.Enabled = false;
            searchbtn.Text = "...";

            try
            {
                string apiKey = "API_KEY_HERE";
                string url = $"https://api.openweathermap.org/data/2.5/weather" +
                                $"?q={city}&appid={apiKey}&units=metric&lang=eng";


                //data save
                string json = await client.GetStringAsync(url);
                var data = Newtonsoft.Json.Linq.JObject.Parse(json);


                //label update
                lblDesc.Text = data["weather"][0]["description"].ToString();
                lblCity.Text = $"{data["name"]}, {data["sys"]["country"]}";
                lblTemp.Text = $"{Math.Round((double)data["main"]["temp"])}º";
                CentLabels();


                //icons
                string icon = data["weather"][0]["icon"].ToString();
                string iconUrl = $"https://openweathermap.org/img/wn/{icon}@2x.png";
                weatherIcon.Load(iconUrl);
                lblError.Visible = false;
            }
            catch (HttpRequestException)
            {
                lblError.Text = $"City not found";
                lblError.Visible = true;
            }
            finally
            {
                searchbtn.Enabled = true;
                searchbtn.Text = "→";
            }

        }
        private void Form1_load(object sender, EventArgs e) { }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
