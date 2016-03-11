using EnemyFortress.Editor;
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
    partial class EditorForm : Form
    {
        private EditorScene parent;

        public EditorForm(EditorScene parent)
        {
            InitializeComponent();
            this.parent = parent;
            mapRadio.Select();
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            parent.SaveMap();
        }

        private void loadButton_Click(object sender, EventArgs e)
        {
            parent.LoadMap();
        }

        private void spawnRadio_CheckedChanged(object sender, EventArgs e)
        {
            parent.SpawnPlacement();
        }

        private void mapRadio_CheckedChanged(object sender, EventArgs e)
        {
            parent.MapPlacement();
        }

        private void deleteRadio_CheckedChanged(object sender, EventArgs e)
        {
            parent.DeleteTool();
        }

        private void gridButton_Click(object sender, EventArgs e)
        {
            parent.ToggleLines();
        }
    }
}
