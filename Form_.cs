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
            client = new ClientConnection("207.154.200.103", 9090);
        }

        #region
        private void onClickConnect(object sender, EventArgs e)
        {
            client.Connect();
        }

        private void onClickClose(object sender, EventArgs e)
        {
            client.Close();
        }
        #endregion

    }
}
