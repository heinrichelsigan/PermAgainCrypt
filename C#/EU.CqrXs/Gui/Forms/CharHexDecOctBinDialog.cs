using EU.CqrXs.Crypt.EnDeCoding;
using EU.CqrXs.Util;
using System;
using System.Globalization;
using System.Numerics;
using System.Reflection;
using System.Xml;

namespace EU.CqrXs.Gui.Forms
{
    /// <summary>
    /// About Dialog is a modal running about application dialog
    /// </summary>
    public class CharHexDecOctBinDialog : Form
    {

        #region fields 

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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

        private System.Windows.Forms.Button shiftButton;
        private System.Windows.Forms.Button plusButton;
        private System.Windows.Forms.Button minusButton;
        private System.Windows.Forms.Button clearButton;
        private System.Windows.Forms.Button okButton;

        static int instances = 0;
        char tChar = '\0';
        uint tNum = 0x0;
        uint tOct = 0;
        string tBin = "0000 0000";
        Boolean isUpdating = false;

        #endregion fields 

        /// <summary>
        /// Default ctor CharHexDecOctBinDialog
        /// </summary>
        public CharHexDecOctBinDialog()
        {
            InitializeComponent();
            instances++;
            this.Text = String.Format("Char-Hex-Dec-Oct-Bin Dialog {0}", instances);
            ClearDialog();
            tNum = 0x40;
            tChar = (char)tNum;
            ShowMappings(tNum, tChar);
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
            plusButton = new Button();
            labelDec = new Label();
            textBoxDec = new TextBox();
            minusButton = new Button();
            labelOct = new Label();
            textBoxOct = new TextBox();
            shiftButton = new Button();
            labelBin = new Label();
            textBoxBin = new TextBox();
            clearButton = new Button();
            okButton = new Button();
            tableLayoutPanel.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            tableLayoutPanel.ColumnCount = 3;
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 55F));
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel.Controls.Add(labelChar, 0, 0);
            tableLayoutPanel.Controls.Add(textBoxChar, 1, 0);
            tableLayoutPanel.Controls.Add(labelHex, 0, 1);
            tableLayoutPanel.Controls.Add(textBoxHex, 1, 1);
            tableLayoutPanel.Controls.Add(plusButton, 2, 1);
            tableLayoutPanel.Controls.Add(labelDec, 0, 2);
            tableLayoutPanel.Controls.Add(textBoxDec, 1, 2);
            tableLayoutPanel.Controls.Add(minusButton, 2, 2);
            tableLayoutPanel.Controls.Add(labelOct, 0, 3);
            tableLayoutPanel.Controls.Add(textBoxOct, 1, 3);
            tableLayoutPanel.Controls.Add(shiftButton, 2, 3);
            tableLayoutPanel.Controls.Add(labelBin, 0, 4);
            tableLayoutPanel.Controls.Add(textBoxBin, 1, 4);
            tableLayoutPanel.Controls.Add(clearButton, 0, 6);
            tableLayoutPanel.Controls.Add(okButton, 2, 6);
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
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
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
            textBoxChar.Size = new Size(255, 26);
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
            textBoxHex.Size = new Size(255, 26);
            textBoxHex.TabIndex = 13;
            textBoxHex.TextChanged += Text_Changed;
            // 
            // plusButton
            // 
            plusButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            plusButton.Font = new Font("Microsoft Sans Serif", 12F);
            plusButton.Location = new Point(396, 55);
            plusButton.Margin = new Padding(4, 2, 4, 2);
            plusButton.Name = "plusButton";
            plusButton.Size = new Size(72, 32);
            plusButton.TabIndex = 14;
            plusButton.Text = "+";
            plusButton.Click += plusButton_Click;
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
            labelDec.TabIndex = 15;
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
            textBoxDec.Size = new Size(255, 26);
            textBoxDec.TabIndex = 16;
            textBoxDec.TextChanged += Text_Changed;
            // 
            // minusButton
            // 
            minusButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            minusButton.Font = new Font("Microsoft Sans Serif", 12F);
            minusButton.Location = new Point(396, 108);
            minusButton.Margin = new Padding(4, 2, 4, 2);
            minusButton.Name = "minusButton";
            minusButton.Size = new Size(72, 32);
            minusButton.TabIndex = 17;
            minusButton.Text = "-";
            minusButton.Click += minusButton_Click;
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
            labelOct.TabIndex = 18;
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
            textBoxOct.Size = new Size(255, 26);
            textBoxOct.TabIndex = 19;
            textBoxOct.TextChanged += Text_Changed;
            // 
            // shiftButton
            // 
            shiftButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            shiftButton.Font = new Font("Microsoft Sans Serif", 12F);
            shiftButton.Location = new Point(396, 161);
            shiftButton.Margin = new Padding(4, 2, 4, 2);
            shiftButton.Name = "shiftButton";
            shiftButton.Size = new Size(72, 32);
            shiftButton.TabIndex = 17;
            shiftButton.Text = ">>";
            shiftButton.Click += shiftButton_Click;
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
            labelBin.TabIndex = 21;
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
            textBoxBin.Size = new Size(255, 26);
            textBoxBin.TabIndex = 22;
            textBoxBin.TextChanged += Text_Changed;
            // 
            // clearButton
            // 
            clearButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            clearButton.Font = new Font("Microsoft Sans Serif", 12F);
            clearButton.Location = new Point(18, 325);
            clearButton.Margin = new Padding(4, 2, 4, 2);
            clearButton.Name = "clearButton";
            clearButton.Size = new Size(96, 29);
            clearButton.TabIndex = 22;
            clearButton.Text = "Clear";
            clearButton.Click += clearButton_Click;
            // 
            // okButton
            // 
            okButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            okButton.DialogResult = DialogResult.Cancel;
            okButton.Font = new Font("Microsoft Sans Serif", 12F);
            okButton.Location = new Point(381, 325);
            okButton.Margin = new Padding(4, 2, 4, 2);
            okButton.Name = "okButton";
            okButton.Size = new Size(87, 29);
            okButton.TabIndex = 24;
            okButton.Text = "OK";
            okButton.Click += okButton_Click;
            // 
            // CharHexDecOctBinDialog
            // 
            AutoScaleDimensions = new SizeF(9F, 18F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(480, 360);
            Controls.Add(tableLayoutPanel);
            Font = new Font("Microsoft Sans Serif", 11F);
            FormBorderStyle = FormBorderStyle.FixedSingle;
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

        #region gui control members

        protected void ShowMappings(uint num = 0, char ch = '\0')
        {
            isUpdating = true;
            if (ch == '\0' || ch == ' ' || ch == '\n' || ch == '\t' || ch == '\v' || ch == '\r' || ch == '\b')
                this.textBoxChar.Text = "";
            else
                this.textBoxChar.Text = ch.ToString();
            this.textBoxHex.Text = $"{num:x}";
            this.textBoxDec.Text = $"{num:d}";
            this.textBoxOct.Text = MapOct(num);
            this.textBoxBin.Text = MapBin(num);
            Thread.Sleep(20);
            isUpdating = false;
        }

        protected uint MapChar(char ch)
        {
            uint unum = 0;
            if ((uint)ch >= (uint)'0' && (uint)ch <= (uint)'9')
                unum = (uint)ch - (uint)'0';
            else if ((uint)ch >= (uint)'A' && (uint)ch <= (uint)'F')
                unum = 10 + (uint)ch - (uint)'A';
            else if ((uint)ch >= (uint)'a' && (uint)ch <= (uint)'f')
                unum = 10 + (uint)ch - (uint)'a';
            return unum;
        }

        protected string MapOct(uint num)
        {
            string octString = Convert.ToString(num, 8);
            return octString;
        }

        protected string MapBin(uint num)
        {
            string b = "", s = $"{num:x}";
            s = s.Replace("x", "");
            for (int bc = 0; bc < s.Length; bc++)
            {
                b += (bc == 0) ? MapBin(s[0]) : " " + MapBin(s[bc]);
            }            
            return b;
        }

        protected string MapBin(char ch)
        {
            switch (ch)
            {
                case '0': return ("0000");
                case '1': return ("0001");
                case '2': return ("0010");
                case '3': return ("0011");
                case '4': return ("0100");
                case '5': return ("0101");
                case '6': return ("0110");
                case '7': return ("0111");
                case '8': return ("1000");
                case '9': return ("1001");
                case 'A':
                case 'a': return ("1010");
                case 'B':
                case 'b': return ("1011");
                case 'C':
                case 'c': return ("1100");
                case 'D':
                case 'd': return ("1101");
                case 'E':
                case 'e': return ("1110");
                case 'F':
                case 'f': return ("1111");
                default:
                    throw new ArgumentException("Ilegal number char " + ch);
            }
        }

        protected char IMapBin(string s)
        {
            string sf = "";
            for (int i = 0; i < s.Length; i++)
                sf += (s[i] == '0' || s[i] == '1') ? s[i].ToString() : "";
            if (sf == "0000" || sf == "000" || sf == "00" || sf == "0")
                return '0';
            else if (sf == "0001" || sf == "001" || sf == "01" || sf == "1")
                return '1';
            else if (sf == "0010" || sf == "010" || sf == "10")
                return '2';
            else if (sf == "0011" || sf == "011" || sf == "11")
                return '3';
            else if (sf == "0100" || sf == "100")
                return '4';
            else if (sf == "0101" || sf == "101")
                return '5';
            else if (sf == "0110" || sf == "110")
                return '6';
            else if (sf == "0111" || sf == "111")
                return '7';
            else if (sf == "1000")
                return '8';
            else if (sf == "1001")
                return '9';
            else if (sf == "1010")
                return 'a';
            else if (sf == "1011")
                return 'b';
            else if (sf == "1100")
                return 'c';
            else if (sf == "1101")
                return 'd';
            else if (sf == "1110")
                return 'e';
            else if (sf == "1111")
                return 'f';
            else
                throw new ArgumentException("Ilegal number char " + sf);

        }

        protected void ClearDialog()
        {
            this.labelChar.Text = "char";
            this.labelHex.Text = "hex";
            this.labelDec.Text = "decimal";
            this.labelOct.Text = "octal";
            this.labelBin.Text = "dual bin";

            this.textBoxChar.Text = string.Empty;
            this.textBoxHex.Text = string.Empty;
            this.textBoxDec.Text = string.Empty;
            this.textBoxOct.Text = string.Empty;
            this.textBoxBin.Text = string.Empty;
        }

        #endregion gui control members

        #region gui control members

        private void Text_Changed(object sender, EventArgs e)
        {
            if (sender != null && sender is TextBox t && !string.IsNullOrEmpty(t.Text) && !isUpdating)
            {
                uint iNum;
                try
                {
                    switch (t.Name)
                    {
                        case "textBoxChar":
                            if (!string.IsNullOrEmpty(t.Text) &&
                                tChar != (char)textBoxChar.Text[0])
                            {
                                tChar = t.Text[0];
                                tNum = (uint)tChar;
                                ShowMappings(tNum, tChar);
                            }
                            break;
                        case "textBoxHex":
                            if (!string.IsNullOrEmpty(t.Text) && t.Text.Length >= 2)
                            {
                                // TODO:
                                byte[] hexb = (new Hex16()).Decode(this.textBoxHex.Text);
                                iNum = Convert.ToUInt32(hexb[0]);
                                if (iNum != tNum)
                                {
                                    tNum = iNum;
                                    tChar = (char)tNum;
                                    ShowMappings(tNum, tChar);
                                }
                            }
                            break;
                        case "textBoxDec":
                            if (!string.IsNullOrEmpty(t.Text) && t.Text.Length >= 2 &&
                                UInt32.TryParse(this.textBoxDec.Text, out iNum))
                            {
                                if (iNum != tNum)
                                {
                                    tNum = iNum;
                                    tChar = (char)tNum;
                                    ShowMappings(tNum, tChar);
                                }
                            }
                            break;
                        case "textBoxOct":                            
                            if (!string.IsNullOrEmpty(textBoxOct.Text) && textBoxOct.Text.Length >= 2)
                            {
                                string os = this.textBoxOct.Text;
                                iNum = 0;
                                int equalizer = os.Length - 1;
                                for (int oc = os.Length - 1; oc >= 0 ; oc--)
                                    iNum += (uint)Math.Pow(8, (equalizer - oc)) * Convert.ToUInt32(os[oc].ToString(), 8);
                                
                                if (iNum != tNum)
                                {
                                    tNum = iNum;
                                    tChar = (char)tNum;
                                    ShowMappings(tNum, tChar);
                                }
                            }
                            break;

                        case "textBoxBin":
                            if (!string.IsNullOrEmpty(t.Text) && t.Text.Length >= 4)
                            {
                                string ob = this.textBoxBin.Text;
                                string[] obs = ob.Split(" -.".ToCharArray());
                                char c1 = IMapBin(obs[1]);
                                char c0 = IMapBin(obs[0]);
                                iNum = 16 * MapChar(c0) + MapChar(c1);
                                if (iNum != tNum)
                                {
                                    tNum = iNum;
                                    tChar = (char)tNum;
                                    ShowMappings(tNum, tChar);
                                }
                            }
                            break;

                        default:
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Area23Log.LogOriginEx("CharHexDecOctBinDialog.SelectedChanged", ex, 2);
                    throw;
                }
            }
        }

        private void plusButton_Click(object sender, EventArgs e)
        {
            tNum++;
            tChar = (char)tNum;
            ShowMappings(tNum, tChar);
        }

        private void minusButton_Click(object sender, EventArgs e)
        {
            tNum--;
            tChar = (char)tNum;
            ShowMappings(tNum, tChar);
        }

        private void shiftButton_Click(object sender, EventArgs e)
        {
            if (tNum < 255)
                tNum = tNum << 1;
            else if (tNum < 1024)
                tNum = 1024;     
            else if (tNum < 4096)
                tNum = 4096;
            else if (tNum < 8192)
                tNum = 8192;
            else if (tNum < 16384)
                tNum = 16384;
            else if (tNum < 32768)
                tNum = 32768;
            else if (tNum < 65536)
                tNum = 65536;
            else if (tNum < 65536 + 32768)
                tNum = 65536 + 32768;
            tChar = (char)tNum;
            ShowMappings(tNum, tChar);
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            ClearDialog();
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion gui control members

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


    }


}
