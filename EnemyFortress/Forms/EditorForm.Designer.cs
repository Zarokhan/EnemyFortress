namespace EnemyFortress.Forms
{
    partial class EditorForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.saveButton = new System.Windows.Forms.Button();
            this.loadButton = new System.Windows.Forms.Button();
            this.spawnRadio = new System.Windows.Forms.RadioButton();
            this.mapRadio = new System.Windows.Forms.RadioButton();
            this.deleteRadio = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.gridButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(12, 493);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 0;
            this.saveButton.Text = "Save Map";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // loadButton
            // 
            this.loadButton.Location = new System.Drawing.Point(93, 493);
            this.loadButton.Name = "loadButton";
            this.loadButton.Size = new System.Drawing.Size(75, 23);
            this.loadButton.TabIndex = 1;
            this.loadButton.Text = "Load Map";
            this.loadButton.UseVisualStyleBackColor = true;
            this.loadButton.Click += new System.EventHandler(this.loadButton_Click);
            // 
            // spawnRadio
            // 
            this.spawnRadio.AutoSize = true;
            this.spawnRadio.Location = new System.Drawing.Point(12, 288);
            this.spawnRadio.Name = "spawnRadio";
            this.spawnRadio.Size = new System.Drawing.Size(58, 17);
            this.spawnRadio.TabIndex = 2;
            this.spawnRadio.TabStop = true;
            this.spawnRadio.Text = "Spawn";
            this.spawnRadio.UseVisualStyleBackColor = true;
            this.spawnRadio.CheckedChanged += new System.EventHandler(this.spawnRadio_CheckedChanged);
            // 
            // mapRadio
            // 
            this.mapRadio.AutoSize = true;
            this.mapRadio.Location = new System.Drawing.Point(12, 265);
            this.mapRadio.Name = "mapRadio";
            this.mapRadio.Size = new System.Drawing.Size(71, 17);
            this.mapRadio.TabIndex = 3;
            this.mapRadio.TabStop = true;
            this.mapRadio.Text = "Map Tiles";
            this.mapRadio.UseVisualStyleBackColor = true;
            this.mapRadio.CheckedChanged += new System.EventHandler(this.mapRadio_CheckedChanged);
            // 
            // deleteRadio
            // 
            this.deleteRadio.AutoSize = true;
            this.deleteRadio.Location = new System.Drawing.Point(12, 311);
            this.deleteRadio.Name = "deleteRadio";
            this.deleteRadio.Size = new System.Drawing.Size(56, 17);
            this.deleteRadio.TabIndex = 4;
            this.deleteRadio.TabStop = true;
            this.deleteRadio.Text = "Delete";
            this.deleteRadio.UseVisualStyleBackColor = true;
            this.deleteRadio.CheckedChanged += new System.EventHandler(this.deleteRadio_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 249);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Tool selection";
            // 
            // gridButton
            // 
            this.gridButton.Location = new System.Drawing.Point(15, 12);
            this.gridButton.Name = "gridButton";
            this.gridButton.Size = new System.Drawing.Size(75, 23);
            this.gridButton.TabIndex = 6;
            this.gridButton.Text = "Toggle Grid";
            this.gridButton.UseVisualStyleBackColor = true;
            this.gridButton.Click += new System.EventHandler(this.gridButton_Click);
            // 
            // EditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(238, 528);
            this.Controls.Add(this.gridButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.deleteRadio);
            this.Controls.Add(this.mapRadio);
            this.Controls.Add(this.spawnRadio);
            this.Controls.Add(this.loadButton);
            this.Controls.Add(this.saveButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "EditorForm";
            this.Text = "Editor";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button loadButton;
        private System.Windows.Forms.RadioButton spawnRadio;
        private System.Windows.Forms.RadioButton mapRadio;
        private System.Windows.Forms.RadioButton deleteRadio;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button gridButton;
    }
}