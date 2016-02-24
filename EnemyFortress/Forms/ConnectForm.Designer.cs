namespace EnemyFortress.Forms
{
    partial class ConnectForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConnectForm));
            this.iplabel = new System.Windows.Forms.Label();
            this.portlabel = new System.Windows.Forms.Label();
            this.IPtextBox = new System.Windows.Forms.TextBox();
            this.PorttextBox = new System.Windows.Forms.TextBox();
            this.connect_button = new System.Windows.Forms.Button();
            this.cancel_button = new System.Windows.Forms.Button();
            this.aliaslabel = new System.Windows.Forms.Label();
            this.AliastextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // iplabel
            // 
            resources.ApplyResources(this.iplabel, "iplabel");
            this.iplabel.Name = "iplabel";
            // 
            // portlabel
            // 
            resources.ApplyResources(this.portlabel, "portlabel");
            this.portlabel.Name = "portlabel";
            // 
            // IPtextBox
            // 
            resources.ApplyResources(this.IPtextBox, "IPtextBox");
            this.IPtextBox.Name = "IPtextBox";
            // 
            // PorttextBox
            // 
            resources.ApplyResources(this.PorttextBox, "PorttextBox");
            this.PorttextBox.Name = "PorttextBox";
            this.PorttextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.PorttextBox_KeyPress);
            // 
            // connect_button
            // 
            resources.ApplyResources(this.connect_button, "connect_button");
            this.connect_button.Name = "connect_button";
            this.connect_button.UseVisualStyleBackColor = true;
            this.connect_button.Click += new System.EventHandler(this.connect_button_Click);
            // 
            // cancel_button
            // 
            resources.ApplyResources(this.cancel_button, "cancel_button");
            this.cancel_button.Name = "cancel_button";
            this.cancel_button.UseVisualStyleBackColor = true;
            this.cancel_button.Click += new System.EventHandler(this.cancel_button_Click);
            // 
            // aliaslabel
            // 
            resources.ApplyResources(this.aliaslabel, "aliaslabel");
            this.aliaslabel.Name = "aliaslabel";
            // 
            // AliastextBox
            // 
            resources.ApplyResources(this.AliastextBox, "AliastextBox");
            this.AliastextBox.Name = "AliastextBox";
            // 
            // ConnectForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.AliastextBox);
            this.Controls.Add(this.aliaslabel);
            this.Controls.Add(this.cancel_button);
            this.Controls.Add(this.connect_button);
            this.Controls.Add(this.PorttextBox);
            this.Controls.Add(this.IPtextBox);
            this.Controls.Add(this.portlabel);
            this.Controls.Add(this.iplabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConnectForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label iplabel;
        private System.Windows.Forms.Label portlabel;
        private System.Windows.Forms.TextBox IPtextBox;
        private System.Windows.Forms.TextBox PorttextBox;
        private System.Windows.Forms.Button connect_button;
        private System.Windows.Forms.Button cancel_button;
        private System.Windows.Forms.Label aliaslabel;
        private System.Windows.Forms.TextBox AliastextBox;
    }
}