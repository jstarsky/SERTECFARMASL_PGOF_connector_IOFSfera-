using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SERTECFARMASL_PGOF_connector_IOFSfera
{
    public partial class Form_ : Form
    {

        private ClientConnection client;

        public Form_()
        {
            this.InitializeComponent();
            AuthPharmacy _authPharmacy = new AuthPharmacy();
            _authPharmacy.token = "token1";
            string _auth = Newtonsoft.Json.JsonConvert.SerializeObject(_authPharmacy);

            client = new ClientConnection("127.0.0.1", 7070, _auth);
        }

        #region
        private async void onClickConnect(object sender, EventArgs e)
        {
            client.Connect();
        }

        private void onClickClose(object sender, EventArgs e)
        {
            client.Close();
        }
        #endregion

        public class AuthPharmacy
        {
            public string token { get; set; }
        }
    }
}
