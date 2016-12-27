/*
 * Created by SharpDevelop.
 * User: C357555
 * Date: 05/10/2011
 * Time: 15:37
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Diagnostics.SymbolStore;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Threading;
using System.Collections.Generic;
using Tools4Libraries;

namespace Droid_Image
{
	/// <summary>
	/// Description of ToolStripMenuIMG.
	/// </summary>
	public class ToolStripMenuIMG : ToolStripMenuManager
	{
		#region Attributes
		private GUI gui;

        private RibbonPanel panelMain;
        private RibbonPanel panelNavigation;
        private RibbonPanel panelReturn;
        private RibbonPanel panelSize;
        private RibbonPanel panelTools;
        private RibbonPanel panelParsing;

        private RibbonButton ts_mainOpen;
        
        private RibbonButton ts_returnV;
        private RibbonButton ts_returnH;
        private RibbonButton ts_rotationL;
        private RibbonButton ts_rotationR;
		private RibbonLabel ts_labelrotation;
		private RibbonTextBox ts_valuerotation;
        private RibbonButton ts_rotation;
        
        private RibbonButton ts_imgcenter;
        private RibbonButton ts_imgstretch;
        private RibbonButton ts_imgzoom;
        private RibbonUpDown ts_zoom; 
        
        private RibbonButton ts_toolpanelvisible;
        private RibbonButton ts_back;
        private RibbonButton ts_next;

        private RibbonButton ts_tool_rgb;
        private RibbonButton ts_tool_resize;
        private RibbonButton ts_tool_contrast;
        private RibbonButton ts_tool_light;
        private RibbonButton ts_tool_crop;
        private RibbonButton ts_tool_gray;
        private RibbonButton ts_tool_invert;
        private RibbonButton ts_tool_addImage;
        private RibbonButton ts_tool_addText;

        private RibbonButton ts_code_qr;
        private RibbonButton ts_code_barre;

		private bool visibletoolpanel;
		public static float rotationvalue;
        private Interface_image _intImg;
		#endregion
		
		#region Properties
        public RibbonPanel PanelReturn
        {
            get { return panelReturn; }
            set { panelReturn = value; }
        }
        public RibbonPanel PanelSize
        {
            get { return panelSize; }
            set { panelSize = value; }
        }
        public RibbonPanel PanelTools
        {
            get { return panelTools; }
            set { panelTools = value; }
        }
		#endregion
		
		#region Constructor
        public ToolStripMenuIMG(Interface_image interface_image)
		{
            _intImg = interface_image;
			gui = new GUI();
			
			visibletoolpanel = false;

            BuildMainPanel();
            BuildReturnPanel();
            BuildSizePanel();
            BuildPanelNavigation();
            BuildToolsPanel();
            BuildParsingPanel();

            this.Text = "Pictures";
		}
		#endregion

        #region Methods public
        public void EnableAll()
        {
            panelMain.Enabled = true;
            panelSize.Enabled = true;
            panelTools.Enabled = true;
            panelReturn.Enabled = true;
            panelNavigation.Enabled = true;
            panelParsing.Enabled = true;
        }
        public void DisableAll()
        {
            panelMain.Enabled = false;
            panelSize.Enabled = false;
            panelTools.Enabled = false;
            panelReturn.Enabled = false;
            panelNavigation.Enabled = false;
            panelParsing.Enabled = false;
        }
        public void RefreshComponent(List<string> ListComponents)
		{
			// nothing to do for this kind of file
			// everything is allow always
		}
		public void Dispose(List<string> theList)
		{
			RibbonTabMenu.Dispose();
			theList.Remove("manager_img_" + CurrentTabPage.Text);
		}
        #endregion

        #region Methods private
        private void BuildMainPanel()
        {
            ts_mainOpen = new RibbonButton("Open");
            ts_mainOpen.Click += new EventHandler(tsb_open_Click);
            ts_mainOpen.Image = gui.imageList32.Images[gui.imageList32.Images.IndexOfKey("open")];
            ts_mainOpen.SmallImage = gui.imageListManager16.Images[gui.imageListManager16.Images.IndexOfKey("open")];

            panelMain = new System.Windows.Forms.RibbonPanel();
            panelMain.Text = "Image";
            panelMain.Items.Add(ts_mainOpen);
            this.Panels.Add(panelMain);
        }
        private void BuildReturnPanel()
        {
            ts_returnH = new RibbonButton("Return horizontal");
            ts_returnH.MinSizeMode = RibbonElementSizeMode.DropDown;
            ts_returnH.Click += new EventHandler(tsb_returnH_Click);
            ts_returnH.SmallImage = gui.imageListManager16.Images[gui.imageListManager16.Images.IndexOfKey("returnH")];

            ts_returnV = new RibbonButton("Return vertical");
            ts_returnV.MinSizeMode = RibbonElementSizeMode.DropDown;
            ts_returnV.Click += new EventHandler(tsb_returnV_Click);
            ts_returnV.SmallImage = gui.imageListManager16.Images[gui.imageListManager16.Images.IndexOfKey("returnV")];

            ts_rotationL = new RibbonButton("Rotation left");
            ts_rotationL.MinSizeMode = RibbonElementSizeMode.DropDown;
            ts_rotationL.Click += new EventHandler(tsb_rotationL_Click);
            ts_rotationL.SmallImage = gui.imageListManager16.Images[gui.imageListManager16.Images.IndexOfKey("rotationL")];

            ts_rotationR = new RibbonButton("Rotation right");
            ts_rotationR.MinSizeMode = RibbonElementSizeMode.DropDown;
            ts_rotationR.Click += new EventHandler(tsb_rotationR_Click);
            ts_rotationR.SmallImage = gui.imageListManager16.Images[gui.imageListManager16.Images.IndexOfKey("rotationR")];

            ts_labelrotation = new RibbonLabel();
            ts_labelrotation.Text = "Rotation value : ";

            ts_valuerotation = new RibbonTextBox();
            ts_valuerotation.LabelWidth = 10;

            ts_rotation = new RibbonButton("Rotation");
            ts_rotation.Click += new EventHandler(tsb_rotation_Click);
            ts_rotation.Image = gui.imageList32.Images[gui.imageList32.Images.IndexOfKey("rotation")];
            
            panelReturn = new System.Windows.Forms.RibbonPanel();
            panelReturn.Text = "Rotation";
            panelReturn.Items.Add(ts_returnH);
            panelReturn.Items.Add(ts_returnV);
            panelReturn.Items.Add(ts_labelrotation);
            panelReturn.Items.Add(ts_rotationL);
            panelReturn.Items.Add(ts_rotationR);
            panelReturn.Items.Add(ts_valuerotation);
            panelReturn.Items.Add(ts_rotation);
            this.Panels.Add(panelReturn);
        }
        private void BuildSizePanel()
        {
            ts_imgcenter = new RibbonButton("Center");
            ts_imgcenter.Click += new EventHandler(tsb_imgcenter_Click);
            ts_imgcenter.MinSizeMode = RibbonElementSizeMode.DropDown;
            ts_imgcenter.SmallImage = gui.imageListManager16.Images[gui.imageListManager16.Images.IndexOfKey("imgcenter")];

            ts_imgstretch = new RibbonButton("Stretch");
            ts_imgstretch.Click += new EventHandler(tsb_imgstretch_Click);
            ts_imgstretch.MinSizeMode = RibbonElementSizeMode.DropDown;
            ts_imgstretch.SmallImage = gui.imageListManager16.Images[gui.imageListManager16.Images.IndexOfKey("imgstretch")];

            ts_imgzoom = new RibbonButton("Adjust");
            ts_imgzoom.Click += new EventHandler(tsb_imgzoom_Click);
            ts_imgzoom.MinSizeMode = RibbonElementSizeMode.DropDown;
            ts_imgzoom.SmallImage = gui.imageListManager16.Images[gui.imageListManager16.Images.IndexOfKey("imgzoom")];

            ts_zoom = new RibbonUpDown();
            ts_zoom.Text = "Zoom rate";
            ts_zoom.Value = "100";
            ts_zoom.MinSizeMode = RibbonElementSizeMode.DropDown;
            ts_zoom.TextBoxTextChanged += ts_zoom_TextBoxTextChanged;
            ts_zoom.Image = gui.imageList32.Images[gui.imageList32.Images.IndexOfKey("zoom")];

            panelSize = new System.Windows.Forms.RibbonPanel();
            panelSize.Text = "Size";
            panelSize.Items.Add(ts_imgcenter);
            panelSize.Items.Add(ts_imgstretch);
            panelSize.Items.Add(ts_imgzoom);
            this.Panels.Add(panelSize);
        }
        private void BuildPanelNavigation()
        {
            ts_toolpanelvisible = new RibbonButton("Visible");
            ts_toolpanelvisible.Click += new EventHandler(tsb_visibletoolpanel_Click);
            ts_toolpanelvisible.Image = gui.imageList32.Images[gui.imageList32.Images.IndexOfKey("showtoolpanel")];

            ts_back = new RibbonButton("Back");
            ts_back.Click += new EventHandler(tsb_previews_Click);
            ts_back.Image = gui.imageList32.Images[gui.imageList32.Images.IndexOfKey("back")];

            ts_next = new RibbonButton("Next");
            ts_next.Click += new EventHandler(tsb_next_Click);
            ts_next.Image = gui.imageList32.Images[gui.imageList32.Images.IndexOfKey("next")];

            panelNavigation = new System.Windows.Forms.RibbonPanel();
            panelNavigation.Text = "Navigation";
            panelNavigation.Items.Add(ts_toolpanelvisible);
            panelNavigation.Items.Add(ts_back);
            panelNavigation.Items.Add(ts_next);
            this.Panels.Add(panelNavigation);
        }
        private void BuildToolsPanel()
        {
            ts_tool_contrast = new RibbonButton("Contraste");
            ts_tool_contrast.Click += new EventHandler(tsb_contrast_Click);
            ts_tool_contrast.SmallImage = gui.imageListManager16.Images[gui.imageListManager16.Images.IndexOfKey("contrast")];
            ts_tool_contrast.MinSizeMode = RibbonElementSizeMode.DropDown;

            ts_tool_light = new RibbonButton("Light");
            ts_tool_light.Click += new EventHandler(tsb_light_Click);
            ts_tool_light.SmallImage = gui.imageListManager16.Images[gui.imageListManager16.Images.IndexOfKey("light")];
            ts_tool_light.MinSizeMode = RibbonElementSizeMode.DropDown;

            ts_tool_resize = new RibbonButton("Resize");
            ts_tool_resize.Click += new EventHandler(tsb_layer_resize_Click);
            ts_tool_resize.SmallImage = gui.imageListManager16.Images[gui.imageListManager16.Images.IndexOfKey("layer_resize")];
            ts_tool_resize.MinSizeMode = RibbonElementSizeMode.DropDown;

            ts_tool_crop = new RibbonButton("Crop");
            ts_tool_crop.Click += new EventHandler(tsb_crop_Click);
            ts_tool_crop.SmallImage = gui.imageListManager16.Images[gui.imageListManager16.Images.IndexOfKey("crop")];
            ts_tool_crop.MinSizeMode = RibbonElementSizeMode.DropDown;

            ts_tool_gray = new RibbonButton("Gray");
            ts_tool_gray.Click += new EventHandler(tsb_gray_Click);
            ts_tool_gray.SmallImage = gui.imageListManager16.Images[gui.imageListManager16.Images.IndexOfKey("gray")];
            ts_tool_gray.MinSizeMode = RibbonElementSizeMode.DropDown;

            ts_tool_invert = new RibbonButton("Invert");
            ts_tool_invert.Click += new EventHandler(tsb_invert_Click);
            ts_tool_invert.SmallImage = gui.imageListManager16.Images[gui.imageListManager16.Images.IndexOfKey("invert")];
            ts_tool_invert.MinSizeMode = RibbonElementSizeMode.DropDown;

            ts_tool_addImage = new RibbonButton("Insert image");
            ts_tool_addImage.Click += new EventHandler(tsb_insert_image_Click);
            ts_tool_addImage.SmallImage = gui.imageListManager16.Images[gui.imageListManager16.Images.IndexOfKey("insertimage")];
            ts_tool_addImage.MinSizeMode = RibbonElementSizeMode.DropDown;

            ts_tool_addText = new RibbonButton("Insert text");
            ts_tool_addText.Click += new EventHandler(tsb_insert_text_Click);
            ts_tool_addText.SmallImage = gui.imageListManager16.Images[gui.imageListManager16.Images.IndexOfKey("inserttext")];
            ts_tool_addText.MinSizeMode = RibbonElementSizeMode.DropDown;

            ts_tool_rgb = new RibbonButton();
            ts_tool_rgb.Text = "Color filter";
            ts_tool_rgb.SmallImage = gui.imageListManager16.Images[gui.imageListManager16.Images.IndexOfKey("layer_rgb")];
            ts_tool_rgb.Image = gui.imageList32.Images[gui.imageList32.Images.IndexOfKey("layer_rgb")];
            ts_tool_rgb.Style = RibbonButtonStyle.SplitDropDown;
            ts_tool_rgb.MinSizeMode = RibbonElementSizeMode.DropDown;
            
            RibbonButton item;
            item = new RibbonButton();
            item.Text = "Red";
            item.SmallImage = gui.imageListManager16.Images[gui.imageListManager16.Images.IndexOfKey("red")];
            item.MinSizeMode = RibbonElementSizeMode.Compact;
            item.Click += rl_red_Click;
            ts_tool_rgb.DropDownItems.Add(item);
            item = new RibbonButton();
            item.Text = "Green";
            item.SmallImage = gui.imageListManager16.Images[gui.imageListManager16.Images.IndexOfKey("green")];
            item.MinSizeMode = RibbonElementSizeMode.Compact;
            item.Click += rl_green_Click;
            ts_tool_rgb.DropDownItems.Add(item);
            item = new RibbonButton();
            item.Text = "Blue";
            item.SmallImage = gui.imageListManager16.Images[gui.imageListManager16.Images.IndexOfKey("blue")];
            item.MinSizeMode = RibbonElementSizeMode.Compact;
            item.Click += rl_blue_Click;
            ts_tool_rgb.DropDownItems.Add(item);
            
            panelTools = new System.Windows.Forms.RibbonPanel();
            panelTools.Text = "Tools";
            panelTools.Items.Add(ts_tool_contrast);
            panelTools.Items.Add(ts_tool_light);
            panelTools.Items.Add(ts_tool_resize);
            panelTools.Items.Add(ts_tool_rgb);
            panelTools.Items.Add(ts_tool_crop);
            panelTools.Items.Add(ts_tool_gray);
            panelTools.Items.Add(ts_tool_invert);
            panelTools.Items.Add(ts_tool_addImage);
            panelTools.Items.Add(ts_tool_addText);
            this.Panels.Add(panelTools);
        }
        private void BuildParsingPanel()
        {
            ts_code_qr = new RibbonButton("Uncrypt QR code");
            ts_code_qr.Click += new EventHandler(tsb_codeqr_Click);
            ts_code_qr.Image = gui.imageList32.Images[gui.imageList32.Images.IndexOfKey("code_qr")];

            ts_code_barre = new RibbonButton("Code Barres");
            ts_code_barre.Click += new EventHandler(tsb_codebar_Click);
            ts_code_barre.Image = gui.imageList32.Images[gui.imageList32.Images.IndexOfKey("code_barres")];

            panelParsing = new System.Windows.Forms.RibbonPanel();
            panelParsing.Text = "Parsing";
            panelParsing.Items.Add(ts_code_qr);
            panelParsing.Items.Add(ts_code_barre);
            this.Panels.Add(panelParsing);
        }
        #endregion

        #region Events
        public event EventHandlerAction ActionAppened;
        public void OnAction(EventArgs e)
        {
            try
            {
                if (ActionAppened != null)
                    ActionAppened(this, e);
            }
            catch (Exception expxxx)
            {
                Log.write("[ DEB : xxxx ] Error on the event call for action appened." + expxxx.Message);
            }
        }

		private void tsb_returnH_Click(object sender, EventArgs e)
		{
			ToolBarEventArgs action = new ToolBarEventArgs("returnH");
			OnAction(action);
		}
		private void tsb_returnV_Click(object sender, EventArgs e)
		{
			ToolBarEventArgs action = new ToolBarEventArgs("returnV");
			OnAction(action);
		}
		private void tsb_rotationR_Click(object sender, EventArgs e)
		{
			ToolBarEventArgs action = new ToolBarEventArgs("rotationR");
			OnAction(action);
		}
		private void tsb_rotationL_Click(object sender, EventArgs e)
		{
			ToolBarEventArgs action = new ToolBarEventArgs("rotationL");
			OnAction(action);
		}
		private void tsb_rotation_Click(object sender, EventArgs e)
		{
			try
			{
				if(!string.IsNullOrEmpty(ts_valuerotation.Text))rotationvalue = float.Parse(ts_valuerotation.Text);
				
			}
			catch (Exception exp)
			{
				Log.write("[ERR : 0400 ] Cannot apply the rotation.\n" + exp.Message);
			}
			
			ToolBarEventArgs action = new ToolBarEventArgs("rotation");
			OnAction(action);
		}
		private void tsb_imgzoom_Click(object sender, EventArgs e)
		{
			ToolBarEventArgs action = new ToolBarEventArgs("imgzoom");
			OnAction(action);
		}
		private void tsb_imgstretch_Click(object sender, EventArgs e)
		{
			ToolBarEventArgs action = new ToolBarEventArgs("imgstretch");
			OnAction(action);
		}
		private void tsb_imgcenter_Click(object sender, EventArgs e)
		{
			ToolBarEventArgs action = new ToolBarEventArgs("imgcenter");
			OnAction(action);
		}
        private void tsb_visibletoolpanel_Click(object sender, EventArgs e)
        {
            visibletoolpanel = !visibletoolpanel;
            if (visibletoolpanel)
            {
                ts_toolpanelvisible.Image = gui.imageList32.Images[gui.imageList32.Images.IndexOfKey("hidetoolpanel")];
            }
            else
            {
                ts_toolpanelvisible.Image = gui.imageList32.Images[gui.imageList32.Images.IndexOfKey("showtoolpanel")];
            }

            ToolBarEventArgs action = new ToolBarEventArgs("visibletoolpanel");
            OnAction(action);
        }
        private void tsb_previews_Click(object sender, EventArgs e)
        {
            ToolBarEventArgs action = new ToolBarEventArgs("back");
            OnAction(action);
        }
        private void tsb_next_Click(object sender, EventArgs e)
        {
            try
            {
                ToolBarEventArgs action = new ToolBarEventArgs("next");
                OnAction(action);
            }
            catch (Exception expxxx)
            {
                Log.write("[ DEB : xxxx ] Error on the event call for on action \"next\" execution." + expxxx.Message);
            }
        }
        private void tsb_open_Click(object sender, EventArgs e)
        {
            try
            {
                ToolBarEventArgs action = new ToolBarEventArgs("open");
                OnAction(action);
            }
            catch (Exception expxxx)
            {
                Log.write("[ DEB : xxxx ] Error on the event call for on action \"open\" execution." + expxxx.Message);
            }
        }
        private void tsb_layer_resize_Click(object sender, EventArgs e)
        {
            try
            {
                _intImg.FlagResize = !_intImg.FlagResize;
                ts_tool_crop.Checked = _intImg.FlagCrop;
                ts_tool_resize.Checked = _intImg.FlagResize;
                ToolBarEventArgs action = new ToolBarEventArgs("resize");
                OnAction(action);
            }
            catch (Exception expxxx)
            {
                Log.write("[ DEB : xxxx ] Error on the event call for on action \"layer_resize\" execution." + expxxx.Message);
            }
        }
        private void tsb_contrast_Click(object sender, EventArgs e)
        {
            try
            {
                ToolBarEventArgs action = new ToolBarEventArgs("contrast");
                OnAction(action);
            }
            catch (Exception expxxx)
            {
                Log.write("[ DEB : xxxx ] Error on the event call for on action \"contrast\" execution." + expxxx.Message);
            }
        }
        private void tsb_light_Click(object sender, EventArgs e)
        {
            try
            {
                ToolBarEventArgs action = new ToolBarEventArgs("light");
                OnAction(action);
            }
            catch (Exception expxxx)
            {
                Log.write("[ DEB : xxxx ] Error on the event call for on action \"light\" execution." + expxxx.Message);
            }
        }
        private void tsb_crop_Click(object sender, EventArgs e)
        {
            try
            {
                _intImg.FlagCrop = !_intImg.FlagCrop;
                ts_tool_crop.Checked = _intImg.FlagCrop;
                ts_tool_resize.Checked = _intImg.FlagResize;
                ToolBarEventArgs action = new ToolBarEventArgs("crop");
                OnAction(action);
            }
            catch (Exception expxxx)
            {
                Log.write("[ DEB : xxxx ] Error on the event call for on action \"crop\" execution." + expxxx.Message);
            }
        }
        private void tsb_gray_Click(object sender, EventArgs e)
        {
            try
            {
                ToolBarEventArgs action = new ToolBarEventArgs("gray");
                OnAction(action);
            }
            catch (Exception expxxx)
            {
                Log.write("[ DEB : xxxx ] Error on the event call for on action \"gray\" execution." + expxxx.Message);
            }
        }
        private void tsb_invert_Click(object sender, EventArgs e)
        {
            try
            {
                ToolBarEventArgs action = new ToolBarEventArgs("invert");
                OnAction(action);
            }
            catch (Exception expxxx)
            {
                Log.write("[ DEB : xxxx ] Error on the event call for on action \"invert\" execution." + expxxx.Message);
            }
        }
        private void rl_red_Click(object sender, EventArgs e)
        {
            try
            {
                ToolBarEventArgs action = new ToolBarEventArgs("rgb_red");
                OnAction(action);
            }
            catch (Exception expxxx)
            {
                Log.write("[ DEB : xxxx ] Error on the event call for on action \"rgb_red\" execution." + expxxx.Message);
            }
        }
        private void rl_green_Click(object sender, EventArgs e)
        {
            try
            {
                ToolBarEventArgs action = new ToolBarEventArgs("rgb_green");
                OnAction(action);
            }
            catch (Exception expxxx)
            {
                Log.write("[ DEB : xxxx ] Error on the event call for on action \"rgb_green\" execution." + expxxx.Message);
            }
        }
        private void rl_blue_Click(object sender, EventArgs e)
        {
            try
            {
                ToolBarEventArgs action = new ToolBarEventArgs("rgb_blue");
                OnAction(action);
            }
            catch (Exception expxxx)
            {
                Log.write("[ DEB : xxxx ] Error on the event call for on action \"rgb_blue\" execution." + expxxx.Message);
            }
        }
        private void tsb_insert_image_Click(object sender, EventArgs e)
        {
            try
            {
                ToolBarEventArgs action = new ToolBarEventArgs("insert_image");
                OnAction(action);
            }
            catch (Exception expxxx)
            {
                Log.write("[ DEB : xxxx ] Error on the event call for on action \"insert image\" execution." + expxxx.Message);
            }
        }
        private void tsb_insert_text_Click(object sender, EventArgs e)
        {
            try
            {
                ToolBarEventArgs action = new ToolBarEventArgs("insert_text");
                OnAction(action);
            }
            catch (Exception expxxx)
            {
                Log.write("[ DEB : xxxx ] Error on the event call for on action \"insert text\" execution." + expxxx.Message);
            }
        }
        private void tsb_codeqr_Click(object sender, EventArgs e)
        {
            try
            {
                ToolBarEventArgs action = new ToolBarEventArgs("codeqr");
                OnAction(action);
            }
            catch (Exception expxxx)
            {
                Log.write("[ DEB : xxxx ] Error on the event call for on action \"code QR\" execution." + expxxx.Message);
            }
        }
        private void tsb_codebar_Click(object sender, EventArgs e)
        {
            try
            {
                ToolBarEventArgs action = new ToolBarEventArgs("codebar");
                OnAction(action);
            }
            catch (Exception expxxx)
            {
                Log.write("[ DEB : xxxx ] Error on the event call for on action \"code bar\" execution." + expxxx.Message);
            }
        }
        public void ts_zoom_TextBoxTextChanged(object sender, EventArgs e)
        {
            try
            {
                ToolBarEventArgs action = new ToolBarEventArgs("zoom_" + ts_zoom.Text);
                OnAction(action);
            }
            catch (Exception expxxx)
            {
                Log.write("[ DEB : xxxx ] Error on the event call for on action \"zoom\" execution." + expxxx.Message);
            }
        }
		#endregion
	}
}
