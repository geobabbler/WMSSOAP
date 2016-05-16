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
            var crslist = cap.capabilityField.layerField.cRSField;
            var lyrs = cap.capabilityField.layerField.layer1Field;
            this.cboSrs.DataSource = crslist;
            //lstSRS.DataSource = crslist;
            //foreach(string crs in crslist)
            //{
            //    this.lstSRS.Items.Add(crs);
            //}
            //lstSRS.Items.Insert(0, "EPSG:4326");
            lstLayers.DisplayMember = "nameField";
            lstLayers.DataSource = lyrs;
            client.Close();
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            List<string> lyrlist = new List<string>();
            foreach(wmssoap.Layer itm in this.lstLayers.SelectedItems)
            {
                lyrlist.Add(itm.nameField);
            }
            //589434.8564686741,4914006.337837095,609527.2102150217,4928063.398014731&width=768&height=537&srs=EPSG:26713
            string[] lyrs = lyrlist.ToArray();
            string crs = this.cboSrs.Text;
            //,609527.2102150217,4928063.398014731
            wmssoap.BoundingBox bbox = new BoundingBox() { minxField = Convert.ToDouble(this.txtMinx.Text), minyField = Convert.ToDouble(this.txtMiny.Text), maxxField = Convert.ToDouble(this.txtMaxx.Text), maxyField = Convert.ToDouble(this.txtMaxy.Text) };
            WmsClient client = new WmsClient();
            var stm = client.GetMap(lyrs, new string[0], bbox, 337, 237, crs, this.lstFormat.SelectedItem.ToString());
            var img = Image.FromStream(stm.BinaryPayload);
            this.pictureBox1.Image = img;
            //this.pictureBox1.Refresh();
            client.Close();

        }

        private void lstLayers_SelectedValueChanged(object sender, EventArgs e)
        {
            wmssoap.Layer lyr = this.lstLayers.SelectedItem as wmssoap.Layer;
            wmssoap.BoundingBox bbox = null;
            foreach(wmssoap.BoundingBox box in lyr.boundingBoxField)
            {
                if (box.cRSField.ToLower() == lyr.cRSField[0].ToLower())
                {
                    bbox = box;
                    break;
                }
            }
            this.txtMaxx.Text = bbox.maxxField.ToString();
            this.txtMaxy.Text = bbox.maxyField.ToString();
            this.txtMinx.Text = bbox.minxField.ToString();
            this.txtMiny.Text = bbox.minyField.ToString();
            this.cboSrs.Text = lyr.cRSField[0];
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.lstFormat.SelectedIndex = 2;
        }
    }
}
