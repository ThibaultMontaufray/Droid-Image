using System.Resources;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;

namespace Droid_Image
{
    public partial class Demo : Form
    {
        #region Attribute
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
            _intImg = new Interface_image();
            _intImg.Tsm.ActionAppened += Tsm_ActionAppened;
            _intImg.MessageAvailable += _intImg_MessageAvailable;
            _intImg.ImageChanged += _intImg_ImageChanged;

            _intImg.Sheet.Dock = DockStyle.Fill;
            this.Controls.Add(_intImg.Sheet);

            InitRibbon();
            InitMessageDisplay();
        }
        private void InitRibbon()
        {
            Ribbon rb = new Ribbon();
            rb.Tabs.Add(_intImg.Tsm);
            rb.Height = 150;
            rb.ThemeColor = RibbonTheme.Black;
            rb.OrbDropDown.Width = 150;
            rb.OrbStyle = RibbonOrbStyle.Office_2013;
            rb.OrbText = "File";
            rb.QuickAccessToolbar.MenuButtonVisible = false;
            rb.QuickAccessToolbar.Visible = false;
            rb.QuickAccessToolbar.MinSizeMode = RibbonElementSizeMode.Compact;

            //rb.QuickAccessToolbar.Visible = false;

            RibbonButton b_open = new RibbonButton("Open");
            b_open.SmallImage = Tools4Libraries.Resources.ResourceIconSet32Default.open_folder;
            b_open.Click += B_open_Click;

            RibbonButton b_exit = new RibbonButton("Exit");
            b_exit.SmallImage = Tools4Libraries.Resources.ResourceIconSet32Default.door_out;
            b_exit.Click += B_exit_Click;

            rb.OrbDropDown.MenuItems.Add(b_open);
            rb.OrbDropDown.MenuItems.Add(b_exit);

            this.Controls.Add(rb);
        }
        private void InitMessageDisplay()
        {
            _messageDisplay = new MessageDisplay();
            _messageDisplay.Dock = DockStyle.Top;
            _messageDisplay.Hide();
            _intImg.PicturePreview.Controls.Add(_messageDisplay);
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
            Tools4Libraries.ToolBarEventArgs eventArg = (Tools4Libraries.ToolBarEventArgs)e;
            if (eventArg.EventText.Equals("exit")) this.Close();
            _intImg.GlobalAction(sender, e);
        }
        private void B_open_Click(object sender, System.EventArgs e)
        {
            Tsm_ActionAppened(null, new Tools4Libraries.ToolBarEventArgs("open"));
        }
        private void B_exit_Click(object sender, System.EventArgs e)
        {
            Tsm_ActionAppened(null, new Tools4Libraries.ToolBarEventArgs("exit"));
        }
        private void _intImg_MessageAvailable(object sender, System.EventArgs e)
        {
            MessageShow(sender);
        }
        private void _intImg_ImageChanged(object sender, System.EventArgs e)
        {
            _messageDisplay.Hide();
        }
        private void Demo_Resize(object sender, System.EventArgs e)
        {
            _messageDisplay.Anchor = AnchorStyles.Top & AnchorStyles.Left;
        }
        #endregion
    }
}
