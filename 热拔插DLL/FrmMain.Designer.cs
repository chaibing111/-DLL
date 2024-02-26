namespace 热拔插DLL
{
    partial class FrmMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            menuMain = new MenuStrip();
            pluginsMenu = new ToolStripMenuItem();
            label1 = new Label();
            menuMain.SuspendLayout();
            SuspendLayout();
            // 
            // menuMain
            // 
            menuMain.Items.AddRange(new ToolStripItem[] { pluginsMenu });
            menuMain.Location = new Point(0, 0);
            menuMain.Name = "menuMain";
            menuMain.Size = new Size(454, 25);
            menuMain.TabIndex = 0;
            menuMain.Text = "menuStrip1";
            // 
            // pluginsMenu
            // 
            pluginsMenu.Name = "pluginsMenu";
            pluginsMenu.Size = new Size(44, 21);
            pluginsMenu.Text = "插件";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Microsoft YaHei UI", 22F, FontStyle.Bold, GraphicsUnit.Point);
            label1.ForeColor = Color.Fuchsia;
            label1.Location = new Point(64, 100);
            label1.Name = "label1";
            label1.Size = new Size(327, 120);
            label1.TabIndex = 1;
            label1.Text = "热插拔 DLL\r\n像积木一样构建应用\r\nAI先锋   AutoWorkAI";
            label1.Visible = false;
            // 
            // FrmMain
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(454, 321);
            Controls.Add(label1);
            Controls.Add(menuMain);
            MainMenuStrip = menuMain;
            Name = "FrmMain";
            Text = "应用程序主窗口";
            Click += FrmMain_Click;
            menuMain.ResumeLayout(false);
            menuMain.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuMain;
        private ToolStripMenuItem pluginsMenu;
        private Label label1;
    }
}