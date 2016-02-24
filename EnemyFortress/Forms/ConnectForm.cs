using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EnemyFortress.Forms
{
    public partial class ConnectForm : Form
    {
        public string IP { get; private set; }
        public string Alias { get; private set; }
        public int Port { get; private set; }

        public ConnectForm()
        {
            InitializeComponent();
        }

        private void connect_button_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrWhiteSpace(IPtextBox.Text) || string.IsNullOrWhiteSpace(PorttextBox.Text) || string.IsNullOrWhiteSpace(AliastextBox.Text))
            {
                MessageBox.Show("Empty textbox", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            IP = IPtextBox.Text;
            Alias = AliastextBox.Text;
            Port = int.Parse(PorttextBox.Text);

            DialogResult = DialogResult.OK;
            Close();
        }

        private void cancel_button_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void PorttextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;
        }
    }
}
