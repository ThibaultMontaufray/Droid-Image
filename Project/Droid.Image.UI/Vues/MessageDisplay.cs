using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Droid.Image
{
    public partial class MessageDisplay : UserControl
    {
        #region Enum
        public enum Message
        {
            FAILED,
            ANALYSING
        }
        #endregion

        #region Attribute
        [DllImport("user32.dll")]
        static extern bool HideCaret(IntPtr hWnd);
        #endregion

        #region Properties
        public override string Text
        {
            get { return _textBox.Text; }
            set
            {
                _textBox.Text = value;
                _textBox.Width = _textBox.Text.Length * (int)(_textBox.Font.Size);
            }
        }
        public Message RichText
        {
            set
            {
                switch (value)
                {
                    case Message.ANALYSING: _textBox.Text = "Analysing ..."; break;
                    case Message.FAILED: _textBox.Text = "Parsing failed !"; break;
                }
                _textBox.Width = _textBox.Text.Length * (int)(_textBox.Font.Size);
            }
        }
        #endregion

        #region Constructor
        public MessageDisplay()
        {
            InitializeComponent();
            this.Name = "MessageDisplay";
            this.GotFocus += MessageDisplay_GotFocus;
        }
        #endregion

        #region Methods private
        #endregion

        #region Methods public
        #endregion

        #region Event
        private void _textBox_MouseClick(object sender, MouseEventArgs e)
        {
            _textBox.SelectAll();
            _textBox.Focus();
        }
        private void MessageDisplay_GotFocus(object sender, EventArgs e)
        {
            HideCaret(_textBox.Handle);
        }
        #endregion
    }
}
