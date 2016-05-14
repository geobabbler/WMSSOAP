using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WmsSoapTester.wmssoap;

namespace WmsSoapTester
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            WmsClient client = new WmsClient();
            var cap = client.GetCapabilities();
            client.Close();
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string[] lyrs = new string[] { "sf:roads" };
            string crs = "EPSG:26713";
            //589434.8564686741,4914006.337837095,609527.2102150217,4928063.398014731
            wmssoap.BoundingBox bbox = new BoundingBox() { minxField = 589434.8564686741, minyField = 4914006.337837095, maxxField = 609527.2102150217, maxyField = 4928063.398014731 };
            WmsClient client = new WmsClient();
            var stm = client.GetMap(lyrs, new string[0], bbox, 337, 237, crs, "image/png");
            var img = Image.FromStream(stm.BinaryPayload);
            this.pictureBox1.Image = img;
            client.Close();

        }
    }
}
