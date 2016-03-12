using EnemyFortress.Editor;
using EnemyFortress.Utilities;
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
    partial class SpawnForm : Form
    {
        public string teamName { get; private set; }
        EditorScene parent;

        public SpawnForm(EditorScene parent)
        {
            InitializeComponent();
            this.parent = parent;

            if (parent.spawnList.Count == 0)
            {
                this.teamListBox.Enabled = false;
                this.existingButton.Enabled = false;
            }
            else
            {
                for (int i = 0; i < parent.spawnList.Count; i++)
                    this.teamListBox.Items.Add(parent.spawnList[i].TeamName);
            }
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(teamTextBox.Text))
            {
                MessageBox.Show("Not valid", "Team name", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            teamName = teamTextBox.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void existingButton_Click(object sender, EventArgs e)
        {

        }
    }
}
