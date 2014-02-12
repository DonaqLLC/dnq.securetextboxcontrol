using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Security;

namespace DNQ.Controls
{
    public partial class SecureTextBox 
        : UserControl
    {
        private SecureString _securetext;
        private bool _nextChangeOk = false;

        private string _pattern = string.Empty;

        public SecureTextBox()
        {
            InitializeComponent();
            _securetext = new SecureString();

            Height = textbox.Height;
        }

        public SecureString SecureText
        {
            get
            {
                return _securetext.Copy();                
            }            
        }

        public void ClearSecureText()
        {
            _securetext.Clear();
            _pattern = "";
            textbox.Text = "";
            _nextChangeOk = false;

            OnTextChanged(EventArgs.Empty);
        }

        public int GetSecureTextHashCode()
        {
            int chkSum = 0;
            IntPtr valuePtr = IntPtr.Zero;
            try
            {
                valuePtr = System.Runtime.InteropServices.Marshal.SecureStringToGlobalAllocUnicode(_securetext);

                var md5 = System.Security.Cryptography.MD5.Create();
                byte[] buff = new byte[_securetext.Length * 2];
                for (int i = 0; i < _securetext.Length; i++)
                {
                    buff[i*2] ^= (byte)System.Runtime.InteropServices.Marshal.ReadByte(valuePtr, i * 2);
                    buff[i*2 + 1] ^= (byte)System.Runtime.InteropServices.Marshal.ReadByte(valuePtr, i * 2 + 1);        
                }
                var hashCode = md5.ComputeHash(buff);
                int A = BitConverter.ToInt32(hashCode, 0);
                int B = BitConverter.ToInt32(hashCode, 4);
                int C = BitConverter.ToInt32(hashCode, 8);
                int D = BitConverter.ToInt32(hashCode, 12);
                chkSum = A ^ B ^ C ^ D;
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ZeroFreeGlobalAllocUnicode(valuePtr);
            }
            return chkSum;
        }

        public string GetSecureTextPattern()
        {
            return _pattern;
        }

        private void SecureTextBox_Load(object sender, EventArgs e)
        {
            textbox.BackColor = BackColor;
            textbox.ForeColor = ForeColor;
        }

        public HorizontalAlignment TextAlign
        {
            get { return textbox.TextAlign; }
            set { textbox.TextAlign = value; }
        }        

        private void SecureTextBox_BackColorChanged(object sender, EventArgs e)
        {
            textbox.BackColor = this.BackColor;
        }

        private void textbox_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }

        private char PatternChar(char chr)
        {
            if (chr < 48)
                return '_';
            else if (chr < 58)
                return 'd';
            else if (chr >= 65 && chr <= 90)
                return 'X';
            else if (chr > 96 && chr <= 122)
                return 'x';            
            else
                return '_';
        }

        private void textbox_KeyPress(object sender, KeyPressEventArgs e)
        {
            int selStart = textbox.SelectionStart;
            int selLength = textbox.SelectionLength;

            if (e.KeyChar > 31 && e.KeyChar < 127)
            {
                if (selLength == 0)
                {                    
                    char pchr = PatternChar(e.KeyChar);
                    _pattern = _pattern.Insert(selStart, new String(pchr, 1));                    
                    _securetext.InsertAt(selStart, e.KeyChar);

                    _nextChangeOk = true;
                    textbox.Text = _pattern;

                    textbox.Select(selStart + 1, 0);
                }
                else
                {
                    for (int i = 0; i < selLength; i++)
                        _securetext.RemoveAt(selStart);
                    _securetext.InsertAt(selStart, e.KeyChar);
                    
                    _pattern = _pattern.Remove(selStart, selLength);
                    char pchr = PatternChar(e.KeyChar);
                    
                    _pattern = _pattern.Insert(selStart, new String(pchr, 1));

                    _nextChangeOk = true;
                    textbox.Text = _pattern;

                    textbox.Select(selStart + 1, 0);
                }

                OnTextChanged(EventArgs.Empty);
            }
            else if (e.KeyChar > 127)
            {                                   // this is a Unicode character                
                if (selLength == 0)
                {
                    _nextChangeOk = true;
                    _pattern = _pattern.Insert(selStart, char.IsLower(e.KeyChar) ? "x" : "X");

                    _securetext.InsertAt(selStart, e.KeyChar);

                    _nextChangeOk = true;
                    textbox.Text = _pattern;

                    textbox.Select(selStart + 1, 0);
                }
                else
                {
                    for (int i = 0; i < selLength; i++)
                        _securetext.RemoveAt(selStart);
                    _securetext.InsertAt(selStart, e.KeyChar);
                    
                    _pattern = _pattern.Remove(selStart, selLength);
                    char pchr = PatternChar(e.KeyChar);

                    _nextChangeOk = true;
                    _pattern = _pattern.Insert(selStart, new String(pchr, 1));

                    _nextChangeOk = true;
                    textbox.Text = _pattern;

                    textbox.Select(selStart + 1, 0);
                }

                OnTextChanged(EventArgs.Empty);
            }
            
            e.Handled = true;
        }

        private void textbox_KeyUp(object sender, KeyEventArgs e)
        {            
            OnKeyUp(e);
        }

        private void textbox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            int selStart = textbox.SelectionStart;
            int selLength = textbox.SelectionLength;

            if (e.KeyCode == Keys.Left || e.KeyCode == Keys.Right || e.KeyCode == Keys.Home || e.KeyCode == Keys.End)
            {
                // handle navigation keys
                if (e.KeyCode == Keys.Left)
                {
                    if (selStart > 0)
                    {
                        if (e.Shift)
                        {
                            textbox.Select(selStart - 1, selLength + 1);
                        }
                        else
                        {
                            textbox.Select(selStart - 1, 0);
                        }
                    }
                    else if(!e.Shift)
                    {
                        textbox.Select(0, 0);
                    }
                }
                else if (e.KeyCode == Keys.Right)
                {
                    if (selStart < textbox.TextLength)
                    {
                        if (e.Shift)
                        {
                            textbox.Select(selStart, selLength + 1);
                        }
                        else
                        {                            
                            textbox.Select(Math.Min(selStart + selLength + 1, textbox.TextLength), 0);
                        }
                    }
                    else if(!e.Shift)
                    {
                        textbox.Select(textbox.TextLength, 0);
                    }
                }
                else if (e.KeyCode == Keys.Home)
                {
                    if (e.Shift)
                    {
                        textbox.Select(0, selStart);
                    }
                    else
                    {
                        textbox.Select(0, 0);
                    }
                }
                else if (e.KeyCode == Keys.End)
                {
                    if (e.Shift)
                    {
                        textbox.Select(selStart, textbox.TextLength - selStart);
                    }
                    else
                    {
                        textbox.Select(textbox.TextLength, 0);
                    }
                }
            }
            else if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
            {
                _nextChangeOk = true;

                if (e.KeyCode == Keys.Back)
                {
                    if (selLength == 0)
                    {
                        if (selStart > 0)
                        {
                            _pattern = _pattern.Remove(selStart - 1, 1);
                            _securetext.RemoveAt(selStart - 1);

                            _nextChangeOk = true;
                            textbox.Text = _pattern;

                            textbox.Select(selStart - 1, 0);
                        }
                    }
                    else
                    {
                        _pattern = _pattern.Remove(selStart, selLength);
                        for(int i=0; i < selLength; i++)
                            _securetext.RemoveAt(selStart);

                        _nextChangeOk = true;
                        textbox.Text = _pattern;

                        textbox.Select(selStart, 0);
                    }
                }
                else if (e.KeyCode == Keys.Delete)
                {
                    if (selLength == 0)
                    {
                        if (selStart < textbox.TextLength)
                        {
                            _pattern = _pattern.Remove(selStart, 1);
                            _securetext.RemoveAt(selStart);

                            _nextChangeOk = true;
                            textbox.Text = _pattern;

                            textbox.Select(selStart, 0);
                        }
                    }
                    else
                    {
                        _pattern = _pattern.Remove(selStart, selLength);
                        for (int i = 0; i < selLength; i++)
                            _securetext.RemoveAt(selStart);

                        _nextChangeOk = true;
                        textbox.Text = _pattern;
                        
                        textbox.Select(selStart, 0);
                    }
                }

                OnTextChanged(EventArgs.Empty);
            }           
        }

        private void textbox_TextChanged(object sender, EventArgs e)
        {
            if (!_nextChangeOk)
            {
                _pattern = "";
                textbox.Text = "";
                _securetext.Clear();

                OnTextChanged(EventArgs.Empty);
                
                return;
            }

            _nextChangeOk = false;            
        }

        private void SecureTextBox_FontChanged(object sender, EventArgs e)
        {
            this.textbox.Font = this.Font;
        }
    }
}
