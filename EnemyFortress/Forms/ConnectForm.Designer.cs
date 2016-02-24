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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.connect_button = new System.Windows.Forms.Button();
            this.cancel_button = new System.Windows.Forms.Button();
            this.aliaslabel = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
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
            // textBox1
            // 
            resources.ApplyResources(this.textBox1, "textBox1");
            this.textBox1.Name = "textBox1";
            // 
            // textBox2
            // 
            resources.ApplyResources(this.textBox2, "textBox2");
            this.textBox2.Name = "textBox2";
            // 
            // connect_button
            // 
            resources.ApplyResources(this.connect_button, "connect_button");
            this.connect_button.Name = "connect_button";
            this.connect_button.UseVisualStyleBackColor = true;
            // 
            // cancel_button
            // 
            resources.ApplyResources(this.cancel_button, "cancel_button");
            this.cancel_button.Name = "cancel_button";
            this.cancel_button.UseVisualStyleBackColor = true;
            // 
            // aliaslabel
            // 
            resources.ApplyResources(this.aliaslabel, "aliaslabel");
            this.aliaslabel.Name = "aliaslabel";
            // 
            // textBox3
            // 
            resources.ApplyResources(this.textBox3, "textBox3");
            this.textBox3.Name = "textBox3";
            // 
            // ConnectForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.aliaslabel);
            this.Controls.Add(this.cancel_button);
            this.Controls.Add(this.connect_button);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
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
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button connect_button;
        private System.Windows.Forms.Button cancel_button;
        private System.Windows.Forms.Label aliaslabel;
        private System.Windows.Forms.TextBox textBox3;
    }
}