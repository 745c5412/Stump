using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Stump.Tools.Sniffer
{
    public partial class DialogInterfaceSelect : Form
    {
        public DialogInterfaceSelect()
        {
            InitializeComponent();
        }

        public string[] Interfaces
        {
            get
            {
                return listBoxInterfaces.Items.Cast<string>().ToArray();
            }
            set
            {
                listBoxInterfaces.Items.Clear();
                listBoxInterfaces.Items.AddRange(value);
            }
        }

        public string SelectedInterface
        {
            get { return listBoxInterfaces.SelectedIndex != -1 ? (string)listBoxInterfaces.SelectedItem : null; }
        }

        private void listBoxInterfaces_SelectedIndexChanged(object sender, EventArgs e)
        {
            buttonOk.Enabled = listBoxInterfaces.SelectedIndex != -1;
        }
    }
}