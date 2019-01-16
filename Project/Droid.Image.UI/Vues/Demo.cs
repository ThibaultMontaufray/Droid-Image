using System.Resources;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;

namespace Droid.Image
{
    public partial class Demo : Form
    {
        #region Attribute
        private Ribbon _ribbon;
        private Interface_image _intImg;
        private MessageDisplay _messageDisplay;
        #endregion

        #region Properties
        #endregion

        #region Constructor
        public Demo()
        {
            InitializeComponent();
            Init();
        }
        #endregion

        #region Methods public
        #endregion

        #region Methods private
        private void Init()
        {
            InitRibbon();
            InitInterface();
            InitMessageDisplay();

            this.KeyDown += Demo_KeyDown;
            this.KeyUp += Demo_KeyUp;
        }
        private void InitInterface()
        {
            _intImg = new Interface_image();
            _intImg.Tsm.ActionAppened += Tsm_ActionAppened;
            _intImg.MessageAvailable += _intImg_MessageAvailable;
            _intImg.ImageChanged += _intImg_ImageChanged;
            _intImg.Sheet.Dock = DockStyle.None;
            _intImg.Sheet.Top = 126;
            _intImg.Sheet.Left = 0;
            _intImg.Sheet.Width = this.Width - 16;
            _intImg.Sheet.Height = this.Height - 145;
            _intImg.Sheet.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right) | System.Windows.Forms.AnchorStyles.Bottom)));
            this.Controls.Add(_intImg.Sheet);
            _ribbon.Tabs.Add(_intImg.Tsm);
        }
        private void InitRibbon()
        {
            _ribbon = new Ribbon();
            _ribbon.Height = 150;
            _ribbon.ThemeColor = RibbonTheme.Black;
            _ribbon.OrbDropDown.Width = 150;
            _ribbon.OrbStyle = RibbonOrbStyle.Office_2013;
            _ribbon.OrbText = "File";
            _ribbon.QuickAccessToolbar.MenuButtonVisible = false;
            _ribbon.QuickAccessToolbar.Visible = false;
            _ribbon.QuickAccessToolbar.MinSizeMode = RibbonElementSizeMode.Compact;
            _ribbon.Dock = DockStyle.None;
            _ribbon.Top = -25;
            _ribbon.Left = 0;
            _ribbon.Width = this.Width;
            _ribbon.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));

            //rb.QuickAccessToolbar.Visible = false;

            RibbonButton b_open = new RibbonButton("Open");
            b_open.SmallImage = Tools.Utilities.Resources.ResourceIconSet32Default.open_folder;
            b_open.Click += B_open_Click;

            RibbonButton b_exit = new RibbonButton("Exit");
            b_exit.SmallImage = Tools.Utilities.Resources.ResourceIconSet32Default.door_out;
            b_exit.Click += B_exit_Click;

            _ribbon.OrbDropDown.MenuItems.Add(b_open);
            _ribbon.OrbDropDown.MenuItems.Add(b_exit);

            this.Controls.Add(_ribbon);
        }
        private void InitMessageDisplay()
        {
            //_messageDisplay = new MessageDisplay();
            //_messageDisplay.Dock = DockStyle.None;
            //_messageDisplay.Hide();
            //_intImg.PicturePreview.Controls.Add(_messageDisplay);
            //_messageDisplay.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
        }
        private void MessageShow(object message)
        {
            if (message.GetType() == typeof(MessageDisplay.Message)) _messageDisplay.RichText = (MessageDisplay.Message)message;
            else _messageDisplay.Text = message.ToString();
            _messageDisplay.Show();
        }
        #endregion

        #region Event
        private void Tsm_ActionAppened(object sender, System.EventArgs e)
        {
            Tools.Utilities.ToolBarEventArgs eventArg = (Tools.Utilities.ToolBarEventArgs)e;
            if (eventArg.EventText.Equals("exit")) this.Close();
            _intImg.GlobalAction(sender, e);
        }
        private void B_open_Click(object sender, System.EventArgs e)
        {
            Tsm_ActionAppened(null, new Tools.Utilities.ToolBarEventArgs("open"));
        }
        private void B_exit_Click(object sender, System.EventArgs e)
        {
            Tsm_ActionAppened(null, new Tools.Utilities.ToolBarEventArgs("exit"));
        }
        private void _intImg_MessageAvailable(object sender, System.EventArgs e)
        {
            MessageShow(sender);
        }
        private void _intImg_ImageChanged(object sender, System.EventArgs e)
        {
            //_messageDisplay.Hide();
        }
        private void Demo_Resize(object sender, System.EventArgs e)
        {
            //_messageDisplay.Anchor = AnchorStyles.Top & AnchorStyles.Left;
        }
        private void Demo_KeyDown(object sender, KeyEventArgs e)
        {
            _intImg.ProcessKeyDown(e.KeyCode);
        }
        private void Demo_KeyUp(object sender, KeyEventArgs e)
        {
            _intImg.ProcessKeyUp(e.KeyCode);
        }
        #endregion
    }
}
