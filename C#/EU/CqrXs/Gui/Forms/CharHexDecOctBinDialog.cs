using EU.CqrXs.Crypt.EnDeCoding;
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
    partial class CharHexDecOctBinDialog : Form
    {
        static int instances = 0;
        char tChar = '\0';
        uint tNum = 0x0;
        uint tOct = 0;
        string tBin = "0000 0000";
        Boolean isUpdating = false;

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


    }


}
