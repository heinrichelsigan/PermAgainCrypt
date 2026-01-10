namespace EU.CqrXs.Gui.Forms
{
    partial class CharHexDecOctBinDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
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
            tableLayoutPanel = new TableLayoutPanel();
            labelChar = new Label();
            textBoxChar = new TextBox();
            labelHex = new Label();
            textBoxHex = new TextBox();
            labelDec = new Label();
            textBoxDec = new TextBox();
            labelOct = new Label();
            textBoxOct = new TextBox();
            labelBin = new Label();
            textBoxBin = new TextBox();
            clearButton = new Button();
            okButton = new Button();
            tableLayoutPanel.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            tableLayoutPanel.ColumnCount = 2;
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 75F));
            tableLayoutPanel.Controls.Add(labelChar, 0, 0);
            tableLayoutPanel.Controls.Add(textBoxChar, 1, 0);
            tableLayoutPanel.Controls.Add(labelHex, 0, 1);
            tableLayoutPanel.Controls.Add(textBoxHex, 1, 1);
            tableLayoutPanel.Controls.Add(labelDec, 0, 2);
            tableLayoutPanel.Controls.Add(textBoxDec, 1, 2);
            tableLayoutPanel.Controls.Add(labelOct, 0, 3);
            tableLayoutPanel.Controls.Add(textBoxOct, 1, 3);
            tableLayoutPanel.Controls.Add(labelBin, 0, 4);
            tableLayoutPanel.Controls.Add(textBoxBin, 1, 4);
            tableLayoutPanel.Controls.Add(clearButton, 0, 6);
            tableLayoutPanel.Controls.Add(okButton, 1, 6);
            tableLayoutPanel.Dock = DockStyle.Fill;
            tableLayoutPanel.Location = new Point(4, 2);
            tableLayoutPanel.Margin = new Padding(4, 2, 4, 2);
            tableLayoutPanel.Name = "tableLayoutPanel";
            tableLayoutPanel.RowCount = 7;
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 15F));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 15F));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 15F));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 15F));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 15F));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 15F));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            tableLayoutPanel.Size = new Size(472, 356);
            tableLayoutPanel.TabIndex = 0;
            // 
            // labelChar
            // 
            labelChar.Dock = DockStyle.Fill;
            labelChar.Font = new Font("Microsoft Sans Serif", 12F);
            labelChar.Location = new Point(4, 2);
            labelChar.Margin = new Padding(4, 2, 4, 2);
            labelChar.MaximumSize = new Size(0, 20);
            labelChar.Name = "labelChar";
            labelChar.Size = new Size(110, 20);
            labelChar.TabIndex = 10;
            labelChar.Text = "Char";
            labelChar.TextAlign = ContentAlignment.MiddleRight;
            // 
            // textBoxChar
            // 
            textBoxChar.Dock = DockStyle.Fill;
            textBoxChar.Font = new Font("Microsoft Sans Serif", 12F);
            textBoxChar.Location = new Point(120, 2);
            textBoxChar.Margin = new Padding(2);
            textBoxChar.Name = "textBoxChar";
            textBoxChar.Size = new Size(350, 26);
            textBoxChar.TabIndex = 11;
            textBoxChar.TextChanged += Text_Changed;
            // 
            // labelHex
            // 
            labelHex.Dock = DockStyle.Fill;
            labelHex.Font = new Font("Microsoft Sans Serif", 12F);
            labelHex.Location = new Point(7, 55);
            labelHex.Margin = new Padding(7, 2, 4, 2);
            labelHex.MaximumSize = new Size(0, 20);
            labelHex.Name = "labelHex";
            labelHex.Size = new Size(107, 20);
            labelHex.TabIndex = 12;
            labelHex.Text = "Hex";
            labelHex.TextAlign = ContentAlignment.MiddleRight;
            // 
            // textBoxHex
            // 
            textBoxHex.Dock = DockStyle.Fill;
            textBoxHex.Font = new Font("Microsoft Sans Serif", 12F);
            textBoxHex.Location = new Point(120, 55);
            textBoxHex.Margin = new Padding(2);
            textBoxHex.Name = "textBoxHex";
            textBoxHex.Size = new Size(350, 26);
            textBoxHex.TabIndex = 13;
            textBoxHex.TextChanged += Text_Changed;
            // 
            // labelDec
            // 
            labelDec.Dock = DockStyle.Fill;
            labelDec.Font = new Font("Microsoft Sans Serif", 12F);
            labelDec.Location = new Point(4, 108);
            labelDec.Margin = new Padding(4, 2, 4, 2);
            labelDec.MaximumSize = new Size(0, 20);
            labelDec.Name = "labelDec";
            labelDec.Size = new Size(110, 20);
            labelDec.TabIndex = 14;
            labelDec.Text = "Decimal";
            labelDec.TextAlign = ContentAlignment.MiddleRight;
            // 
            // textBoxDec
            // 
            textBoxDec.Dock = DockStyle.Fill;
            textBoxDec.Font = new Font("Microsoft Sans Serif", 12F);
            textBoxDec.Location = new Point(120, 108);
            textBoxDec.Margin = new Padding(2);
            textBoxDec.Name = "textBoxDec";
            textBoxDec.Size = new Size(350, 26);
            textBoxDec.TabIndex = 15;
            textBoxDec.TextChanged += Text_Changed;
            // 
            // labelOct
            // 
            labelOct.Dock = DockStyle.Fill;
            labelOct.Font = new Font("Microsoft Sans Serif", 12F);
            labelOct.Location = new Point(4, 161);
            labelOct.Margin = new Padding(4, 2, 4, 2);
            labelOct.MaximumSize = new Size(0, 20);
            labelOct.Name = "labelOct";
            labelOct.Size = new Size(110, 20);
            labelOct.TabIndex = 16;
            labelOct.Text = "Octal";
            labelOct.TextAlign = ContentAlignment.MiddleRight;
            // 
            // textBoxOct
            // 
            textBoxOct.Dock = DockStyle.Fill;
            textBoxOct.Font = new Font("Microsoft Sans Serif", 12F);
            textBoxOct.Location = new Point(120, 161);
            textBoxOct.Margin = new Padding(2);
            textBoxOct.Name = "textBoxOct";
            textBoxOct.Size = new Size(350, 26);
            textBoxOct.TabIndex = 17;
            textBoxOct.TextChanged += Text_Changed;
            // 
            // labelBin
            // 
            labelBin.Dock = DockStyle.Fill;
            labelBin.Font = new Font("Microsoft Sans Serif", 12F);
            labelBin.Location = new Point(4, 214);
            labelBin.Margin = new Padding(4, 2, 4, 2);
            labelBin.MaximumSize = new Size(0, 20);
            labelBin.Name = "labelBin";
            labelBin.Size = new Size(110, 20);
            labelBin.TabIndex = 18;
            labelBin.Text = "Dual-Bin";
            labelBin.TextAlign = ContentAlignment.MiddleRight;
            // 
            // textBoxBin
            // 
            textBoxBin.Dock = DockStyle.Fill;
            textBoxBin.Font = new Font("Microsoft Sans Serif", 12F);
            textBoxBin.Location = new Point(120, 214);
            textBoxBin.Margin = new Padding(2);
            textBoxBin.Name = "textBoxBin";
            textBoxBin.Size = new Size(350, 26);
            textBoxBin.TabIndex = 19;
            textBoxBin.TextChanged += Text_Changed;
            // 
            // clearButton
            // 
            clearButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            clearButton.DialogResult = DialogResult.Cancel;
            clearButton.Font = new Font("Microsoft Sans Serif", 12F);
            clearButton.Location = new Point(18, 322);
            clearButton.Margin = new Padding(4, 2, 4, 2);
            clearButton.Name = "clearButton";
            clearButton.Size = new Size(96, 32);
            clearButton.TabIndex = 22;
            clearButton.Text = "Clear";
            clearButton.Click += clearButton_Click;
            // 
            // okButton
            // 
            okButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            okButton.DialogResult = DialogResult.Cancel;
            okButton.Font = new Font("Microsoft Sans Serif", 12F);
            okButton.Location = new Point(372, 322);
            okButton.Margin = new Padding(4, 2, 4, 2);
            okButton.Name = "okButton";
            okButton.Size = new Size(96, 32);
            okButton.TabIndex = 24;
            okButton.Text = "&OK";
            // 
            // CharHexDecOctBinDialog
            // 
            AcceptButton = okButton;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(480, 360);
            Controls.Add(tableLayoutPanel);
            Font = new Font("Microsoft Sans Serif", 9.25F);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(4, 2, 4, 2);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "CharHexDecOctBinDialog";
            Opacity = 0.8D;
            Padding = new Padding(4, 2, 4, 2);
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Char-Hex-Dec-Oct-Bin Dialog";
            tableLayoutPanel.ResumeLayout(false);
            tableLayoutPanel.PerformLayout();
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Label labelChar;
        private System.Windows.Forms.Label labelHex;
        private System.Windows.Forms.Label labelDec;
        private System.Windows.Forms.Label labelOct;
        private System.Windows.Forms.Label labelBin;
        private System.Windows.Forms.TextBox textBoxChar;
        private System.Windows.Forms.TextBox textBoxHex;
        private System.Windows.Forms.TextBox textBoxDec;
        private System.Windows.Forms.TextBox textBoxOct;
        private System.Windows.Forms.TextBox textBoxBin;

        private System.Windows.Forms.Button clearButton;
        private System.Windows.Forms.Button okButton;
    }
}
