namespace Agalag
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            this.gameTimer = new System.Windows.Forms.Timer(this.components);
            this.nameLabel = new System.Windows.Forms.Label();
            this.upperTitleLabel = new System.Windows.Forms.Label();
            this.lowerTitleLabel = new System.Windows.Forms.Label();
            this.onePlayerButton = new System.Windows.Forms.Button();
            this.twoPlayerButton = new System.Windows.Forms.Button();
            this.highScoreButton = new System.Windows.Forms.Button();
            this.explosionBox = new System.Windows.Forms.PictureBox();
            this.entryBox = new System.Windows.Forms.TextBox();
            this.enterNameButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.explosionBox)).BeginInit();
            this.SuspendLayout();
            // 
            // gameTimer
            // 
            this.gameTimer.Interval = 16;
            this.gameTimer.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // nameLabel
            // 
            this.nameLabel.AutoSize = true;
            this.nameLabel.Font = new System.Drawing.Font("Liberation Mono", 20.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nameLabel.ForeColor = System.Drawing.Color.White;
            this.nameLabel.Location = new System.Drawing.Point(65, 9);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(221, 30);
            this.nameLabel.TabIndex = 0;
            this.nameLabel.Text = "Gareth Marks\'";
            // 
            // upperTitleLabel
            // 
            this.upperTitleLabel.AutoSize = true;
            this.upperTitleLabel.BackColor = System.Drawing.Color.Transparent;
            this.upperTitleLabel.Font = new System.Drawing.Font("Liberation Mono", 99.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.upperTitleLabel.ForeColor = System.Drawing.Color.White;
            this.upperTitleLabel.Location = new System.Drawing.Point(12, 50);
            this.upperTitleLabel.Name = "upperTitleLabel";
            this.upperTitleLabel.Size = new System.Drawing.Size(544, 151);
            this.upperTitleLabel.TabIndex = 1;
            this.upperTitleLabel.Text = "Nebula";
            // 
            // lowerTitleLabel
            // 
            this.lowerTitleLabel.AutoSize = true;
            this.lowerTitleLabel.BackColor = System.Drawing.Color.Transparent;
            this.lowerTitleLabel.Font = new System.Drawing.Font("Liberation Mono", 99.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lowerTitleLabel.ForeColor = System.Drawing.Color.White;
            this.lowerTitleLabel.Location = new System.Drawing.Point(732, 50);
            this.lowerTitleLabel.Name = "lowerTitleLabel";
            this.lowerTitleLabel.Size = new System.Drawing.Size(624, 151);
            this.lowerTitleLabel.TabIndex = 2;
            this.lowerTitleLabel.Text = "Fighter";
            // 
            // onePlayerButton
            // 
            this.onePlayerButton.BackColor = System.Drawing.Color.Black;
            this.onePlayerButton.Font = new System.Drawing.Font("Courier New", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.onePlayerButton.ForeColor = System.Drawing.Color.Lime;
            this.onePlayerButton.Location = new System.Drawing.Point(100, 297);
            this.onePlayerButton.Name = "onePlayerButton";
            this.onePlayerButton.Size = new System.Drawing.Size(323, 184);
            this.onePlayerButton.TabIndex = 4;
            this.onePlayerButton.Text = "Start One Player";
            this.onePlayerButton.UseVisualStyleBackColor = false;
            this.onePlayerButton.Click += new System.EventHandler(this.onePlayerButton_Click);
            // 
            // twoPlayerButton
            // 
            this.twoPlayerButton.BackColor = System.Drawing.Color.Black;
            this.twoPlayerButton.Font = new System.Drawing.Font("Courier New", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.twoPlayerButton.ForeColor = System.Drawing.Color.Lime;
            this.twoPlayerButton.Location = new System.Drawing.Point(510, 297);
            this.twoPlayerButton.Name = "twoPlayerButton";
            this.twoPlayerButton.Size = new System.Drawing.Size(323, 184);
            this.twoPlayerButton.TabIndex = 5;
            this.twoPlayerButton.Text = "Start Two Player";
            this.twoPlayerButton.UseVisualStyleBackColor = false;
            this.twoPlayerButton.Click += new System.EventHandler(this.twoPlayerButton_Click);
            // 
            // highScoreButton
            // 
            this.highScoreButton.BackColor = System.Drawing.Color.Black;
            this.highScoreButton.Font = new System.Drawing.Font("Courier New", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.highScoreButton.ForeColor = System.Drawing.Color.Lime;
            this.highScoreButton.Location = new System.Drawing.Point(922, 297);
            this.highScoreButton.Name = "highScoreButton";
            this.highScoreButton.Size = new System.Drawing.Size(323, 184);
            this.highScoreButton.TabIndex = 6;
            this.highScoreButton.Text = "High Scores";
            this.highScoreButton.UseVisualStyleBackColor = false;
            this.highScoreButton.Click += new System.EventHandler(this.highScoreButton_Click);
            // 
            // explosionBox
            // 
            this.explosionBox.BackColor = System.Drawing.Color.Transparent;
            this.explosionBox.Image = global::Agalag.Properties.Resources.explosion_graphic;
            this.explosionBox.Location = new System.Drawing.Point(510, 29);
            this.explosionBox.Name = "explosionBox";
            this.explosionBox.Size = new System.Drawing.Size(251, 213);
            this.explosionBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.explosionBox.TabIndex = 3;
            this.explosionBox.TabStop = false;
            // 
            // entryBox
            // 
            this.entryBox.Location = new System.Drawing.Point(541, 406);
            this.entryBox.Name = "entryBox";
            this.entryBox.Size = new System.Drawing.Size(205, 20);
            this.entryBox.TabIndex = 7;
            // 
            // enterNameButton
            // 
            this.enterNameButton.BackColor = System.Drawing.Color.Black;
            this.enterNameButton.Font = new System.Drawing.Font("Courier New", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.enterNameButton.ForeColor = System.Drawing.Color.Lime;
            this.enterNameButton.Location = new System.Drawing.Point(531, 432);
            this.enterNameButton.Name = "enterNameButton";
            this.enterNameButton.Size = new System.Drawing.Size(241, 70);
            this.enterNameButton.TabIndex = 9;
            this.enterNameButton.Text = "Confirm";
            this.enterNameButton.UseVisualStyleBackColor = false;
            this.enterNameButton.Click += new System.EventHandler(this.enterNameButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(1354, 733);
            this.Controls.Add(this.enterNameButton);
            this.Controls.Add(this.entryBox);
            this.Controls.Add(this.highScoreButton);
            this.Controls.Add(this.twoPlayerButton);
            this.Controls.Add(this.onePlayerButton);
            this.Controls.Add(this.explosionBox);
            this.Controls.Add(this.lowerTitleLabel);
            this.Controls.Add(this.upperTitleLabel);
            this.Controls.Add(this.nameLabel);
            this.DoubleBuffered = true;
            this.Name = "Form1";
            this.Text = "Gareth Marks\' Nebula Fighter";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
            ((System.ComponentModel.ISupportInitialize)(this.explosionBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer gameTimer;
        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.Label upperTitleLabel;
        private System.Windows.Forms.Label lowerTitleLabel;
        private System.Windows.Forms.PictureBox explosionBox;
        private System.Windows.Forms.Button onePlayerButton;
        private System.Windows.Forms.Button twoPlayerButton;
        private System.Windows.Forms.Button highScoreButton;
        private System.Windows.Forms.TextBox entryBox;
        private System.Windows.Forms.Button enterNameButton;
    }
}

