using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Net;
using System.IO;

namespace GTolkServer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            btnVerificarIp.Text = getPublicIP();
        }

        public string getPublicIP()
        {
            string direction;
            WebRequest request = WebRequest.Create("http://checkip.dyndns.org/");
            WebResponse response = request.GetResponse();
            StreamReader stream = new StreamReader(response.GetResponseStream());
            direction = stream.ReadToEnd();
            stream.Close();
            response.Close(); //Search for the ip in the html
            int first = direction.IndexOf("Address: ") + 9;
            int last = direction.IndexOf("</body>"); //direction.LastIndexOf("");
            direction = direction.Substring(first, last-first);
            return direction;
        }
    }
}