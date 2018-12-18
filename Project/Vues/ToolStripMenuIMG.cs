/*
 * Created by SharpDevelop.
 * User: C357555
 * Date: 05/10/2011
 * Time: 15:37
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Windows.Forms;
using System.Collections.Generic;
using Tools4Libraries;
using System.Drawing;

namespace Droid.Image
{
    /// <summary>
    /// Description of ToolStripMenuIMG.
    /// </summary>
    public class ToolStripMenuIMG : RibbonTab
    {
        #region Attributes
        private Panel _currentTabPage;
        private GUI _gui; 

        private RibbonPanel _panelMain;
        private RibbonPanel _panelNavigation;
        private RibbonPanel _panelView;
        private RibbonPanel _panelParsing;
        private RibbonPanel _panelDrawing;
        private RibbonPanel _panelText;

        private RibbonButton _ts_main_open;
        private RibbonButton _ts_undo;
        private RibbonButton _ts_redo;
        private RibbonButton _ts_copy;
        private RibbonButton _ts_cut;
        private RibbonButton _ts_paste;
        private RibbonTextBox _valueLookingFor;
        private RibbonButton _ts_web_research;
        private RibbonButton _ts_save;
        private RibbonButton _ts_saveas;
        private RibbonButton _ts_fullscreen;
        private RibbonButton _ts_zoomin;
        private RibbonButton _ts_zoomout;
        private RibbonButton _ts_delete;

        private RibbonButton _ts_rotations;
        private RibbonButton _ts_returnV;
        private RibbonButton _ts_returnH;
        private RibbonButton _ts_rotationL;
        private RibbonButton _ts_rotationR;
        private RibbonLabel _ts_labelrotation;
        private RibbonTextBox _ts_valuerotation;
        private RibbonButton _ts_rotation;
        
        private RibbonButton _ts_size;
        private RibbonButton _ts_imgcenter;
        private RibbonButton _ts_imgstretch;
        private RibbonButton _ts_imgautosize;
        private RibbonButton _ts_imgzoom;
        private RibbonUpDown _ts_zoom;

        private RibbonButton _ts_tool_adjust;
        private RibbonButton _ts_tool_rgb;
        private RibbonButton _ts_resize;
        private RibbonButton _ts_tool_contrast;
        private RibbonButton _ts_tool_light;
        private RibbonButton _ts_tool_crop;
        private RibbonButton _ts_tool_gray;
        private RibbonButton _ts_tool_invert;
        private RibbonButton _ts_tool_addImage;

        private RibbonLabel _ts_font_label;
        private RibbonButton _ts_font_dropdown;
        private RibbonButton _ts_font_size_dropdown;
        private RibbonButton _ts_font_bold;
        private RibbonButton _ts_font_italic;
        private RibbonButton _ts_font_underline;
        private RibbonButton _ts_font_strikethrought;
        
        private RibbonButton _ts_draw_pencil;
        private RibbonButton _ts_draw_fill;
        private RibbonButton _ts_draw_text;
        private RibbonButton _ts_draw_erase;
        private RibbonButton _ts_draw_color_pick;
        private RibbonButton _ts_draw_line;
        private RibbonButton _ts_draw_shape;
        private RibbonButton _ts_draw_color;
        private RibbonButton _ts_draw_magichands;
        private RibbonButton _ts_draw_paintbrush;
        private RibbonButton _ts_draw_select;
        private RibbonButton _ts_draw_move;

        private RibbonButton _ts_code_qr;
        private RibbonButton _ts_code_barre;
        private RibbonButton _ts_recognition;
        private RibbonButton _ts_compare;

        private RibbonButton _ts_web_google;
        private RibbonButton _ts_web_flikr;
        
        private RibbonPanel _panelMode;
        private RibbonButton _modeView;
        private RibbonButton _modeEdition;
        private RibbonButton _modeAnalyse;

        //private RibbonPanel _panelNavigation;
        private RibbonButton _ts_diaporama;
        private RibbonButton _ts_toolpanelvisible;
        private RibbonButton _ts_back;
        private RibbonButton _ts_next;

        private bool _visibletoolpanel;
        public static float _rotationvalue;
        private Interface_image _intImg;
        #endregion

        #region Properties
        public RibbonPanel PanelTools
        {
            get { return _panelView; }
            set { _panelView = value; }
        }
        public Panel CurrentTabPage
        {
            get { return _currentTabPage; }
            set { _currentTabPage = value; }
        }
        #endregion

        #region Constructor
        public ToolStripMenuIMG(Interface_image interface_image)
        {
            _gui = new GUI();
            _intImg = interface_image;
            _intImg.DiaporamaLaunched += _intImg_DiaporamaLaunched;

            _visibletoolpanel = false;

            BuildToolsAdjustment();
            BuildToolsReturn();
            BuildToolsSize();
            BuildToolsWeb();
            BuildToolsDrawing();
            BuildToolsText();

            BuildPanelMain();
            BuildPanelView();
            BuildPanelMode();
            BuildPanelNavigation();
            BuildPanelDrawing();
            BuildPanelText();
            BuildPanelParsing();
            //BuildPanelNavigation();

            SwitchMode();
            this.Text = "Pictures";
        }
        #endregion

        #region Methods public
        public void EnableAll()
        {
            _panelMain.Enabled = true;
            _panelView.Enabled = true;
            //_panelNavigation.Enabled = true;
            _panelParsing.Enabled = true;
            _panelDrawing.Enabled = true;
            _panelText.Enabled = true;

            _ts_main_open.Enabled = true;
            _ts_web_google.Enabled = true;
            _ts_web_flikr.Enabled = true;
            _valueLookingFor.Enabled = true;
            _ts_web_research.Enabled = true;

            _ts_undo.Enabled = true;
            _ts_redo.Enabled = true;
            _ts_copy.Enabled = true;
            _ts_cut.Enabled = true;
            _ts_paste.Enabled = true;
        }
        public void DisableAll()
        {
            _panelMain.Enabled = false;
            _panelView.Enabled = false;
            //_panelNavigation.Enabled = false;
            _panelParsing.Enabled = false;
            _panelDrawing.Enabled = false;
            _panelText.Enabled = false;

            _ts_main_open.Enabled = true;
            _ts_web_google.Enabled = true;   
            _ts_web_flikr.Enabled = true;
            _valueLookingFor.Enabled = true;
            _ts_web_research.Enabled = true;

            _ts_undo.Enabled = false;
            _ts_redo.Enabled = false;
            _ts_copy.Enabled = false;
            _ts_cut.Enabled = false;
            _ts_paste.Enabled = false;
        }
        public void RefreshComponent(List<string> ListComponents)
        {
            // nothing to do for this kind of file
            // everything is allow always
        }
        public void Dispose(List<string> theList)
        {
            //RibbonTabMenu.Dispose();
            theList.Remove("manager_img_" + _currentTabPage.Text);
        }
        public void SwitchMode()
        {
            _modeView.Image = Tools4Libraries.Resources.ResourceIconSet32Default.shape_square;
            _modeView.SmallImage = Tools4Libraries.Resources.ResourceIconSet16Default.shape_square;
            _modeEdition.Image = Tools4Libraries.Resources.ResourceIconSet32Default.shape_square;
            _modeEdition.SmallImage = Tools4Libraries.Resources.ResourceIconSet16Default.shape_square;
            _modeAnalyse.Image = Tools4Libraries.Resources.ResourceIconSet32Default.shape_square;
            _modeAnalyse.SmallImage = Tools4Libraries.Resources.ResourceIconSet16Default.shape_square;

            if (_intImg != null)
            { 
                switch (_intImg.CurrentMode)
                {
                    case Mode.EDITION:
                        _panelDrawing.Visible = true;
                        _panelNavigation.Visible = false;
                        _panelParsing.Visible = false;
                        _panelText.Visible = true;
                        _modeEdition.Image = Tools4Libraries.Resources.ResourceIconSet32Default.check_box;
                        _modeEdition.SmallImage = Tools4Libraries.Resources.ResourceIconSet16Default.check_box;
                        break;
                    case Mode.ANALYSE:
                        _panelDrawing.Visible = false;
                        _panelNavigation.Visible = false;
                        _panelParsing.Visible = true;
                        _panelText.Visible = false;
                        _modeAnalyse.Image = Tools4Libraries.Resources.ResourceIconSet32Default.check_box;
                        _modeAnalyse.SmallImage = Tools4Libraries.Resources.ResourceIconSet16Default.check_box;
                        break;
                    case Mode.VIEW:
                    default:
                        _panelDrawing.Visible = false;
                        _panelNavigation.Visible = true;
                        _panelParsing.Visible = false;
                        _panelText.Visible = false;
                        _modeView.Image = Tools4Libraries.Resources.ResourceIconSet32Default.check_box;
                        _modeView.SmallImage = Tools4Libraries.Resources.ResourceIconSet16Default.check_box;
                        break;
                }
            }
        }
        public void SwitchDiaporama()
        {
            if (_intImg != null)
            {
                _ts_diaporama.Image = _intImg.DiaporamaRunning ? Tools4Libraries.Resources.ResourceIconSet32Default.control_pause : Tools4Libraries.Resources.ResourceIconSet32Default.control_play;
                _ts_diaporama.SmallImage = _intImg.DiaporamaRunning ? Tools4Libraries.Resources.ResourceIconSet16Default.control_pause : Tools4Libraries.Resources.ResourceIconSet16Default.control_play;
            }
        }
        #endregion

        #region Methods private
        private void BuildPanelMain()
        {
            _ts_main_open = new RibbonButton("Open");
            _ts_main_open.ToolTip = "Open";
            _ts_main_open.Click += new EventHandler(tsb_Click);
            _ts_main_open.Image = Tools4Libraries.Resources.ResourceIconSet32Default.open_folder;
            _ts_main_open.SmallImage = Tools4Libraries.Resources.ResourceIconSet16Default.open_folder;
            _ts_main_open.MinSizeMode = RibbonElementSizeMode.Large;

            _ts_save = new RibbonButton("Save");
            _ts_save.ToolTip = "Save";
            _ts_save.Click += new EventHandler(tsb_Click);
            _ts_save.SmallImage = Tools4Libraries.Resources.ResourceIconSet16Default.picture_save;
            _ts_save.MinSizeMode = RibbonElementSizeMode.Compact;

            _ts_saveas = new RibbonButton("Save as");
            _ts_saveas.ToolTip = "Save as";
            _ts_saveas.Click += new EventHandler(tsb_Click);
            _ts_saveas.SmallImage = Tools4Libraries.Resources.ResourceIconSet16Default.save_as;
            _ts_saveas.MinSizeMode = RibbonElementSizeMode.Compact;

            _ts_zoomin = new RibbonButton("Zoom in");
            _ts_zoomin.ToolTip = "Zoom in";
            _ts_zoomin.Click += new EventHandler(tsb_Click);
            _ts_zoomin.SmallImage = Tools4Libraries.Resources.ResourceIconSet16Default.zoom_in;
            _ts_zoomin.MinSizeMode = RibbonElementSizeMode.Compact;

            _ts_zoomout = new RibbonButton("Zoom out");
            _ts_zoomout.ToolTip = "Zoom out";
            _ts_zoomout.Click += new EventHandler(tsb_Click);
            _ts_zoomout.SmallImage = Tools4Libraries.Resources.ResourceIconSet16Default.zoom_out;
            _ts_zoomout.MinSizeMode = RibbonElementSizeMode.Compact;

            _ts_delete = new RibbonButton("Delete");
            _ts_delete.ToolTip = "Delete";
            _ts_delete.Click += new EventHandler(tsb_Click);
            _ts_delete.SmallImage = Tools4Libraries.Resources.ResourceIconSet16Default.cross;
            _ts_delete.MinSizeMode = RibbonElementSizeMode.Compact;
            
            RibbonItemGroup rig1 = new RibbonItemGroup();
            rig1.Items.Add(_ts_save);
            rig1.Items.Add(_ts_saveas);

            RibbonItemGroup rig2 = new RibbonItemGroup();
            rig2.Items.Add(_ts_zoomin);
            rig2.Items.Add(_ts_zoomout);
            
            RibbonItemGroup rig3 = new RibbonItemGroup();
            rig3.Items.Add(_valueLookingFor);
            rig3.Items.Add(_ts_web_research);
            
            _panelMain = new System.Windows.Forms.RibbonPanel();
            _panelMain.Image = Tools4Libraries.Resources.ResourceIconSet16Default.picture;
            _panelMain.Text = "Image";
            _panelMain.Items.Add(_ts_main_open);
            _panelMain.Items.Add(rig1);
            _panelMain.Items.Add(rig2);
            _panelMain.Items.Add(rig3);
            this.Panels.Add(_panelMain);
        }        
        private void BuildPanelMode()
        {
            _modeView = new RibbonButton();
            _modeView.Name = "View";
            _modeView.Text = _modeView.Name;
            _modeView.Click += new EventHandler(tsb_Click);
            _modeView.Image = Tools4Libraries.Resources.ResourceIconSet32Default.check_box;
            _modeView.SmallImage = Tools4Libraries.Resources.ResourceIconSet16Default.check_box;
            _modeView.MinSizeMode = RibbonElementSizeMode.Medium;
            _modeView.MaxSizeMode = RibbonElementSizeMode.Medium;

            _modeEdition = new RibbonButton();
            _modeEdition.Name = "Edition";
            _modeEdition.Text = _modeEdition.Name;
            _modeEdition.Click += new EventHandler(tsb_Click);
            _modeEdition.Image = Tools4Libraries.Resources.ResourceIconSet32Default.shape_square;
            _modeEdition.SmallImage = Tools4Libraries.Resources.ResourceIconSet16Default.shape_square;
            _modeEdition.MinSizeMode = RibbonElementSizeMode.Medium;
            _modeEdition.MaxSizeMode = RibbonElementSizeMode.Medium;

            _modeAnalyse = new RibbonButton();
            _modeAnalyse.Name = "Analyse";
            _modeAnalyse.Text = _modeAnalyse.Name;
            _modeAnalyse.Click += new EventHandler(tsb_Click);
            _modeAnalyse.Image = Tools4Libraries.Resources.ResourceIconSet32Default.shape_square;
            _modeAnalyse.SmallImage = Tools4Libraries.Resources.ResourceIconSet16Default.shape_square;
            _modeAnalyse.MinSizeMode = RibbonElementSizeMode.Medium;
            _modeAnalyse.MaxSizeMode = RibbonElementSizeMode.Medium;

            _panelMode = new System.Windows.Forms.RibbonPanel("Mode");
            _panelMode.Image = Tools4Libraries.Resources.ResourceIconSet16Default.pictures;
            _panelMode.Items.Add(_modeView);
            _panelMode.Items.Add(_modeEdition);
            _panelMode.Items.Add(_modeAnalyse);
            this.Panels.Add(_panelMode);
        }
        private void BuildPanelNavigation()
        {
            _ts_fullscreen = new RibbonButton("Full screen");
            _ts_fullscreen.ToolTip = "Full screen";
            _ts_fullscreen.Click += new EventHandler(tsb_Click);
            _ts_fullscreen.Image = Tools4Libraries.Resources.ResourceIconSet32Default.monitor;
            _ts_fullscreen.SmallImage = Tools4Libraries.Resources.ResourceIconSet16Default.monitor;
            _ts_fullscreen.MinSizeMode = RibbonElementSizeMode.Medium;

            _ts_diaporama = new RibbonButton("Diaporama");
            _ts_diaporama.ToolTip = "Diaporama";
            _ts_diaporama.Click += new EventHandler(tsb_Click);
            _ts_diaporama.Image = Tools4Libraries.Resources.ResourceIconSet32Default.control_play;
            _ts_diaporama.SmallImage = Tools4Libraries.Resources.ResourceIconSet16Default.control_play;
            _ts_diaporama.MinSizeMode = RibbonElementSizeMode.Medium;

            _ts_toolpanelvisible = new RibbonButton("Visible");
            _ts_toolpanelvisible.Click += new EventHandler(tsb_Click);
            _ts_toolpanelvisible.Image = Tools4Libraries.Resources.ResourceIconSet32Default.setting_tools;
            _ts_toolpanelvisible.SmallImage = Tools4Libraries.Resources.ResourceIconSet16Default.setting_tools;
            _ts_toolpanelvisible.MinSizeMode = RibbonElementSizeMode.Medium;

            _ts_back = new RibbonButton("Back");
            _ts_back.Click += new EventHandler(tsb_Click);
            _ts_back.Image = Tools4Libraries.Resources.ResourceIconSet32Default.document_back;
            _ts_back.SmallImage = Tools4Libraries.Resources.ResourceIconSet16Default.document_back;
            _ts_back.MinSizeMode = RibbonElementSizeMode.Large;

            _ts_next = new RibbonButton("Next");
            _ts_next.Click += new EventHandler(tsb_Click);
            _ts_next.Image = Tools4Libraries.Resources.ResourceIconSet32Default.document_next;
            _ts_next.SmallImage = Tools4Libraries.Resources.ResourceIconSet16Default.document_next;
            _ts_next.MinSizeMode = RibbonElementSizeMode.Large;

            _panelNavigation = new System.Windows.Forms.RibbonPanel();
            _panelNavigation.Image = Tools4Libraries.Resources.ResourceIconSet16Default.node_tree;
            _panelNavigation.Text = "Navigation";
            _panelNavigation.Items.Add(_ts_back);
            _panelNavigation.Items.Add(_ts_next);
            _panelNavigation.Items.Add(_ts_diaporama);
            _panelNavigation.Items.Add(_ts_toolpanelvisible);
            _panelNavigation.Items.Add(_ts_fullscreen);
            this.Panels.Add(_panelNavigation);
        }
        private void BuildPanelView()
        {          
            _ts_tool_addImage = new RibbonButton("Insert image");
            _ts_tool_addImage.Click += new EventHandler(tsb_Click);
            _ts_tool_addImage.SmallImage = Tools4Libraries.Resources.ResourceIconSet16Default.insert_object;
            _ts_tool_addImage.MinSizeMode = RibbonElementSizeMode.DropDown;

            _ts_tool_rgb = new RibbonButton("Color filter");
            _ts_tool_rgb.SmallImage = Tools4Libraries.Resources.ResourceIconSet16Default.layer_rgb;
            _ts_tool_rgb.Image = Tools4Libraries.Resources.ResourceIconSet32Default.layer_rgb;
            _ts_tool_rgb.Style = RibbonButtonStyle.SplitDropDown;
            _ts_tool_rgb.MinSizeMode = RibbonElementSizeMode.DropDown;
            
            RibbonButton itemRed = new RibbonButton("Red");
            itemRed.Name = "rl_red_Click";
            itemRed.SmallImage = Tools4Libraries.Resources.ResourceIconSet16Default.tag_red;
            itemRed.MinSizeMode = RibbonElementSizeMode.Compact;
            itemRed.Style = RibbonButtonStyle.Normal;
            itemRed.Click += tsb_Click;
            _ts_tool_rgb.DropDownItems.Add(itemRed);
            RibbonButton itemGreen = new RibbonButton("Green");
            itemGreen.Name = "rl_green_Click";
            itemGreen.SmallImage = Tools4Libraries.Resources.ResourceIconSet16Default.tag_green;
            itemGreen.MinSizeMode = RibbonElementSizeMode.Compact;
            itemGreen.Style = RibbonButtonStyle.Normal;
            itemGreen.Click += tsb_Click;
            _ts_tool_rgb.DropDownItems.Add(itemGreen);
            RibbonButton itemBlue = new RibbonButton("Blue");
            itemBlue.Name = "rl_blue_Click";
            itemBlue.SmallImage = Tools4Libraries.Resources.ResourceIconSet16Default.tag_blue;
            itemBlue.MinSizeMode = RibbonElementSizeMode.Compact;
            itemBlue.Style = RibbonButtonStyle.Normal;
            itemBlue.Click += tsb_Click;
            _ts_tool_rgb.DropDownItems.Add(itemBlue);

            _panelView = new System.Windows.Forms.RibbonPanel("Tools");
            _panelView.Image = Tools4Libraries.Resources.ResourceIconSet16Default.image_edit;
            _panelView.Items.Add(_ts_tool_adjust);
            _panelView.Items.Add(_ts_size);
            _panelView.Items.Add(_ts_rotations);
            //_panelTools.Items.Add(_ts_labelrotation);
            //_panelTools.Items.Add(_ts_valuerotation);
            //_panelTools.Items.Add(_ts_rotation);
            //_panelTools.Items.Add(_ts_tool_addImage);
            this.Panels.Add(_panelView);
        }
        private void BuildPanelParsing()
        {
            _ts_code_qr = new RibbonButton("QR code");
            _ts_code_qr.Click += new EventHandler(tsb_Click);
            _ts_code_qr.Image = Tools4Libraries.Resources.ResourceIconSet32Default.qrcode;
            _ts_code_qr.SmallImage = Tools4Libraries.Resources.ResourceIconSet16Default.qrcode;
            _ts_code_qr.MinSizeMode = RibbonElementSizeMode.Medium;

            _ts_code_barre = new RibbonButton("Code Barres");
            _ts_code_barre.Click += new EventHandler(tsb_Click);
            _ts_code_barre.Image = Tools4Libraries.Resources.ResourceIconSet32Default.barcode;
            _ts_code_barre.SmallImage = Tools4Libraries.Resources.ResourceIconSet16Default.barcode;
            _ts_code_barre.MinSizeMode = RibbonElementSizeMode.Medium;

            _ts_recognition = new RibbonButton("Global analysing");
            _ts_recognition.Click += new EventHandler(tsb_Click);
            _ts_recognition.Image = Tools4Libraries.Resources.ResourceIconSet32Default.brain_trainer;
            _ts_recognition.SmallImage = Tools4Libraries.Resources.ResourceIconSet16Default.brain_trainer;
            _ts_recognition.MinSizeMode = RibbonElementSizeMode.Medium;

            _ts_compare = new RibbonButton("Compare");
            _ts_compare.Click += new EventHandler(tsb_Click);
            _ts_compare.Image = Tools4Libraries.Resources.ResourceIconSet32Default.picture_frame;
            _ts_compare.SmallImage = Tools4Libraries.Resources.ResourceIconSet16Default.picture_frame;
            _ts_recognition.MinSizeMode = RibbonElementSizeMode.Medium;

            _panelParsing = new System.Windows.Forms.RibbonPanel("Parsing");
            _panelParsing.Image = Tools4Libraries.Resources.ResourceIconSet16Default.scanner;
            _panelParsing.Items.Add(_ts_code_qr);
            _panelParsing.Items.Add(_ts_code_barre);
            _panelParsing.Items.Add(_ts_recognition);
            _panelParsing.Items.Add(_ts_compare);
            this.Panels.Add(_panelParsing);
        }
        private void BuildPanelDrawing()
        {
            _ts_undo = new RibbonButton("Undo");
            _ts_undo.ToolTip = "Undo";
            _ts_undo.Click += new EventHandler(tsb_Click);
            _ts_undo.SmallImage = Tools4Libraries.Resources.ResourceIconSet16Default.arrow_undo;
            _ts_undo.MinSizeMode = RibbonElementSizeMode.Compact;

            _ts_redo = new RibbonButton("Redo");
            _ts_redo.ToolTip = "Redo";
            _ts_redo.Click += new EventHandler(tsb_Click);
            _ts_redo.SmallImage = Tools4Libraries.Resources.ResourceIconSet16Default.arrow_redo;
            _ts_redo.MinSizeMode = RibbonElementSizeMode.Compact;

            _ts_copy = new RibbonButton("Copy");
            _ts_copy.ToolTip = "Copy";
            _ts_copy.Click += new EventHandler(tsb_Click);
            _ts_copy.SmallImage = Tools4Libraries.Resources.ResourceIconSet16Default.page_copy;
            _ts_copy.MinSizeMode = RibbonElementSizeMode.Compact;

            _ts_cut = new RibbonButton("Cut");
            _ts_cut.ToolTip = "Cut";
            _ts_cut.Click += new EventHandler(tsb_Click);
            _ts_cut.SmallImage = Tools4Libraries.Resources.ResourceIconSet16Default.cut;
            _ts_cut.MinSizeMode = RibbonElementSizeMode.Compact;

            _ts_paste = new RibbonButton("Paste");
            _ts_paste.ToolTip = "Paste";
            _ts_paste.Click += new EventHandler(tsb_Click);
            _ts_paste.SmallImage = Tools4Libraries.Resources.ResourceIconSet16Default.paste_plain;
            _ts_paste.MinSizeMode = RibbonElementSizeMode.Compact;

            RibbonItemGroup rig1 = new RibbonItemGroup();
            rig1.Items.Add(_ts_draw_pencil);
            rig1.Items.Add(_ts_draw_paintbrush);
            rig1.Items.Add(_ts_draw_fill);
            rig1.Items.Add(_ts_draw_magichands);
            rig1.Items.Add(_ts_draw_text);
            rig1.Items.Add(_ts_draw_erase);

            RibbonItemGroup rig2 = new RibbonItemGroup();
            rig2.Items.Add(_ts_draw_color_pick);
            rig2.Items.Add(_ts_draw_line);
            rig2.Items.Add(_ts_draw_shape);
            rig2.Items.Add(_ts_draw_color);
            rig2.Items.Add(_ts_draw_select);
            rig2.Items.Add(_ts_draw_move);

            RibbonItemGroup rig3 = new RibbonItemGroup();
            rig3.Items.Add(_ts_undo);
            rig3.Items.Add(_ts_redo);
            rig3.Items.Add(_ts_copy);
            rig3.Items.Add(_ts_cut);
            rig3.Items.Add(_ts_paste);

            _panelDrawing = new System.Windows.Forms.RibbonPanel("Drawing");
            _panelDrawing.Image = Tools4Libraries.Resources.ResourceIconSet16Default.drawer;
            _panelDrawing.Items.Add(rig1);
            _panelDrawing.Items.Add(rig2);
            _panelDrawing.Items.Add(rig3);
            this.Panels.Add(_panelDrawing);
        }
        private void BuildPanelText()
        {
            RibbonItemGroup rig1 = new RibbonItemGroup();
            //rig1.Items.Add(_ts_font_label);
            rig1.Items.Add(_ts_font_dropdown);

            RibbonItemGroup rig2 = new RibbonItemGroup();
            rig2.Items.Add(_ts_font_size_dropdown);
            rig2.Items.Add(_ts_font_bold);
            rig2.Items.Add(_ts_font_italic);
            rig2.Items.Add(_ts_font_underline);
            rig2.Items.Add(_ts_font_strikethrought);

            _panelText = new System.Windows.Forms.RibbonPanel("Text");
            _panelText.Image = Tools4Libraries.Resources.ResourceIconSet16Default.drawer;
            _panelText.Items.Add(rig1);
            _panelText.Items.Add(rig2);
            this.Panels.Add(_panelText);
        }

        private void BuildToolsWeb()
        {
            _valueLookingFor = new RibbonTextBox();
            _valueLookingFor.ToolTip = "Word to search on the web like \"people\"";
            //_valueLookingFor.Text = "Search";
            _valueLookingFor.TextBoxTextChanged += _valueLookingFor_TextBoxTextChanged;
            _valueLookingFor.TextBoxKeyDown += _valueLookingFor_TextBoxKeyDown;

            _ts_web_google = new RibbonButton("Google");
            _ts_web_google.Click += new EventHandler(tsb_Click);
            _ts_web_google.Image = Tools4Libraries.Resources.ResourceIconSet32Default.google;
            _ts_web_google.SmallImage = Tools4Libraries.Resources.ResourceIconSet16Default.google;

            _ts_web_flikr = new RibbonButton("Flikr");
            _ts_web_flikr.Click += new EventHandler(tsb_Click);
            _ts_web_flikr.Image = Tools4Libraries.Resources.ResourceIconSet32Default.flickr;
            _ts_web_flikr.SmallImage = Tools4Libraries.Resources.ResourceIconSet16Default.flickr;
            
            _ts_web_research = new RibbonButton();
            _ts_web_research.Name = "google";
            _ts_web_research.SmallImage = Tools4Libraries.Resources.ResourceIconSet16Default.google;
            _ts_web_research.Style = RibbonButtonStyle.SplitDropDown;
            _ts_web_research.MinSizeMode = RibbonElementSizeMode.Compact;
            _ts_web_research.DropDownItems.Add(_ts_web_google);
            _ts_web_research.DropDownItems.Add(_ts_web_flikr);
        }
        private void BuildToolsAdjustment()
        {
            _ts_tool_contrast = new RibbonButton("Contraste");
            _ts_tool_contrast.Click += new EventHandler(tsb_Click);
            _ts_tool_contrast.SmallImage = Tools4Libraries.Resources.ResourceIconSet16Default.contrast;
            _ts_tool_contrast.MinSizeMode = RibbonElementSizeMode.DropDown;

            _ts_tool_light = new RibbonButton("Light");
            _ts_tool_light.Click += new EventHandler(tsb_Click);
            _ts_tool_light.SmallImage = Tools4Libraries.Resources.ResourceIconSet16Default.lightbulb;
            _ts_tool_light.MinSizeMode = RibbonElementSizeMode.DropDown;

            _ts_tool_crop = new RibbonButton("Crop");
            _ts_tool_crop.Click += new EventHandler(tsb_Click);
            _ts_tool_crop.SmallImage = Tools4Libraries.Resources.ResourceIconSet16Default.transform_crop;
            _ts_tool_crop.MinSizeMode = RibbonElementSizeMode.DropDown;

            _ts_tool_gray = new RibbonButton("Gray");
            _ts_tool_gray.Click += new EventHandler(tsb_Click);
            _ts_tool_gray.SmallImage = Tools4Libraries.Resources.ResourceIconSet16Default.convert_color_to_gray;
            _ts_tool_gray.MinSizeMode = RibbonElementSizeMode.DropDown;

            _ts_tool_invert = new RibbonButton("Invert");
            _ts_tool_invert.Click += new EventHandler(tsb_Click);
            _ts_tool_invert.SmallImage = Tools4Libraries.Resources.ResourceIconSet16Default.image;
            _ts_tool_invert.MinSizeMode = RibbonElementSizeMode.DropDown;

            _ts_tool_adjust = new RibbonButton("Adjust");
            _ts_tool_adjust.SmallImage = Tools4Libraries.Resources.ResourceIconSet16Default.scale_image;
            _ts_tool_adjust.Image = Tools4Libraries.Resources.ResourceIconSet32Default.transform_rotate;
            _ts_tool_adjust.Style = RibbonButtonStyle.SplitDropDown;
            _ts_tool_adjust.MinSizeMode = RibbonElementSizeMode.DropDown;
            _ts_tool_adjust.DropDownItems.Add(_ts_tool_contrast);
            _ts_tool_adjust.DropDownItems.Add(_ts_tool_light);
            _ts_tool_adjust.DropDownItems.Add(_ts_tool_crop);
            _ts_tool_adjust.DropDownItems.Add(_ts_tool_gray);
            _ts_tool_adjust.DropDownItems.Add(_ts_tool_invert);

        }
        private void BuildToolsReturn()
        {
            _ts_returnH = new RibbonButton("Return horizontal");
            _ts_returnH.MinSizeMode = RibbonElementSizeMode.Compact;
            _ts_returnH.Style = RibbonButtonStyle.Normal;
            _ts_returnH.Click += new EventHandler(tsb_Click);
            _ts_returnH.SmallImage = Tools4Libraries.Resources.ResourceIconSet16Default.shape_flip_vertical;

            _ts_returnV = new RibbonButton("Return vertical");
            _ts_returnV.MinSizeMode = RibbonElementSizeMode.Compact;
            _ts_returnV.Style = RibbonButtonStyle.Normal;
            _ts_returnV.Click += new EventHandler(tsb_Click);
            _ts_returnV.SmallImage = Tools4Libraries.Resources.ResourceIconSet16Default.shape_flip_horizontal;

            _ts_rotationL = new RibbonButton("Rotation left");
            _ts_rotationL.MinSizeMode = RibbonElementSizeMode.Compact;
            _ts_rotationL.Style = RibbonButtonStyle.Normal;
            _ts_rotationL.Click += new EventHandler(tsb_Click);
            _ts_rotationL.SmallImage = Tools4Libraries.Resources.ResourceIconSet16Default.shape_rotate_anticlockwise;

            _ts_rotationR = new RibbonButton("Rotation right");
            _ts_rotationR.MinSizeMode = RibbonElementSizeMode.Compact;
            _ts_rotationR.Style = RibbonButtonStyle.Normal;
            _ts_rotationR.Click += new EventHandler(tsb_Click);
            _ts_rotationR.SmallImage = Tools4Libraries.Resources.ResourceIconSet16Default.shape_rotate_clockwise;

            _ts_labelrotation = new RibbonLabel();
            _ts_labelrotation.Text = "Rotation : ";

            _ts_valuerotation = new RibbonTextBox();
            _ts_valuerotation.LabelWidth = 7;

            _ts_rotation = new RibbonButton("Rotation");
            _ts_rotation.Click += new EventHandler(tsb_Click);
            _ts_rotation.Image = Tools4Libraries.Resources.ResourceIconSet16Default.transform_rotate;

            _ts_rotations = new RibbonButton("Rotations");
            _ts_rotations.SmallImage = Tools4Libraries.Resources.ResourceIconSet16Default.transform_rotate;
            _ts_rotations.Image = Tools4Libraries.Resources.ResourceIconSet32Default.transform_rotate;
            _ts_rotations.Style = RibbonButtonStyle.SplitDropDown;
            _ts_rotations.MinSizeMode = RibbonElementSizeMode.DropDown;
            _ts_rotations.DropDownItems.Add(_ts_returnH);
            _ts_rotations.DropDownItems.Add(_ts_returnV);
            _ts_rotations.DropDownItems.Add(_ts_rotationL);
            _ts_rotations.DropDownItems.Add(_ts_rotationR);
        }
        private void BuildToolsSize()
        {
            _ts_resize = new RibbonButton("Resize");
            _ts_resize.Click += new EventHandler(tsb_Click);
            _ts_resize.SmallImage = Tools4Libraries.Resources.ResourceIconSet16Default.layer_to_image_size;
            _ts_resize.MinSizeMode = RibbonElementSizeMode.DropDown;

            _ts_imgcenter = new RibbonButton("Center");
            _ts_imgcenter.Click += new EventHandler(tsb_Click);
            _ts_imgcenter.MinSizeMode = RibbonElementSizeMode.DropDown;
            _ts_imgcenter.SmallImage = Tools4Libraries.Resources.ResourceIconSet16Default.hardware_building_oem;

            _ts_imgstretch = new RibbonButton("Stretch");
            _ts_imgstretch.Click += new EventHandler(tsb_Click);
            _ts_imgstretch.MinSizeMode = RibbonElementSizeMode.DropDown;
            _ts_imgstretch.SmallImage = Tools4Libraries.Resources.ResourceIconSet16Default.transform_scale;

            _ts_imgautosize = new RibbonButton("Autosize");
            _ts_imgautosize.Click += new EventHandler(tsb_Click);
            _ts_imgautosize.MinSizeMode = RibbonElementSizeMode.DropDown;
            _ts_imgautosize.SmallImage = Tools4Libraries.Resources.ResourceIconSet16Default.page_magnifier;

            _ts_imgzoom = new RibbonButton("Adjust");
            _ts_imgzoom.Click += new EventHandler(tsb_Click);
            _ts_imgzoom.MinSizeMode = RibbonElementSizeMode.DropDown;
            _ts_imgzoom.SmallImage = Tools4Libraries.Resources.ResourceIconSet16Default.zoom_extend;

            _ts_zoom = new RibbonUpDown();
            _ts_zoom.Text = "Zoom rate";
            _ts_zoom.Value = "100";
            _ts_zoom.MinSizeMode = RibbonElementSizeMode.DropDown;
            _ts_zoom.TextBoxTextChanged += tsb_Click;
            _ts_zoom.Image = Tools4Libraries.Resources.ResourceIconSet32Default.zoom;

            _ts_size = new RibbonButton("Size");
            _ts_size.SmallImage = Tools4Libraries.Resources.ResourceIconSet16Default.layer_to_image_size;
            _ts_size.Image = Tools4Libraries.Resources.ResourceIconSet32Default.layer_to_image_size;
            _ts_size.Style = RibbonButtonStyle.SplitDropDown;
            _ts_size.MinSizeMode = RibbonElementSizeMode.DropDown;
            _ts_size.DropDownItems.Add(_ts_resize);
            _ts_size.DropDownItems.Add(_ts_imgcenter);
            _ts_size.DropDownItems.Add(_ts_imgstretch);
            _ts_size.DropDownItems.Add(_ts_imgautosize);
            _ts_size.DropDownItems.Add(_ts_imgzoom);
        }
        private void BuildToolsText()
        {
            _ts_font_label = new RibbonLabel();
            _ts_font_label.Text = "Font";

            RibbonButton selectedItem = null;
            RibbonButton buttonItem;
            _ts_font_dropdown = new RibbonButton();
            _ts_font_dropdown.Style = RibbonButtonStyle.SplitDropDown;
            foreach (FontFamily font in System.Drawing.FontFamily.Families)
            {
                buttonItem = new RibbonButton() { Text = font.Name };
                _ts_font_dropdown.DropDownItems.Add(buttonItem);
                if (font.Name.Equals("Calibri")) selectedItem = buttonItem;
                else if (font.Name.Equals("Arial") && selectedItem == null) selectedItem = buttonItem;
            }
            _ts_font_dropdown.SelectedItem = selectedItem;
            if (selectedItem != null) _ts_font_dropdown.Text = _ts_font_dropdown.SelectedItem.Text;
            _ts_font_dropdown.MinSizeMode = RibbonElementSizeMode.Medium;
            _ts_font_dropdown.MaxSizeMode = RibbonElementSizeMode.Medium;
            _ts_font_dropdown.DropDownItemClicked += _ts_font_dropdown_DropDownItemClicked;
            _ts_font_dropdown.TextAlignment = RibbonItem.RibbonItemTextAlignment.Left;

            _ts_font_size_dropdown = new RibbonButton();
            _ts_font_size_dropdown.Style = RibbonButtonStyle.DropDown;
            _ts_font_size_dropdown.MinSizeMode = RibbonElementSizeMode.Medium;
            _ts_font_size_dropdown.MaxSizeMode = RibbonElementSizeMode.Medium;
            _ts_font_size_dropdown.DropDownItems.Add(new RibbonButton("8"));
            _ts_font_size_dropdown.DropDownItems.Add(new RibbonButton("9"));
            _ts_font_size_dropdown.DropDownItems.Add(new RibbonButton("10"));
            _ts_font_size_dropdown.DropDownItems.Add(new RibbonButton("11"));
            _ts_font_size_dropdown.DropDownItems.Add(new RibbonButton("12"));
            _ts_font_size_dropdown.DropDownItems.Add(new RibbonButton("14"));
            _ts_font_size_dropdown.DropDownItems.Add(new RibbonButton("16"));
            _ts_font_size_dropdown.DropDownItems.Add(new RibbonButton("18"));
            _ts_font_size_dropdown.DropDownItems.Add(new RibbonButton("20"));
            _ts_font_size_dropdown.DropDownItems.Add(new RibbonButton("22"));
            _ts_font_size_dropdown.DropDownItems.Add(new RibbonButton("24"));
            _ts_font_size_dropdown.DropDownItems.Add(new RibbonButton("26"));
            _ts_font_size_dropdown.DropDownItems.Add(new RibbonButton("28"));
            _ts_font_size_dropdown.DropDownItems.Add(new RibbonButton("36"));
            _ts_font_size_dropdown.DropDownItems.Add(new RibbonButton("48"));
            _ts_font_size_dropdown.DropDownItems.Add(new RibbonButton("72"));
            _ts_font_size_dropdown.Text = "12";
            _ts_font_size_dropdown.DropDownItemClicked += _ts_font_size_dropdown_DropDownItemClicked;
            _ts_font_size_dropdown.TextAlignment = RibbonItem.RibbonItemTextAlignment.Left;

            _ts_font_bold = new RibbonButton();
            _ts_font_bold.ToolTip = "Bold";
            _ts_font_bold.Click += new EventHandler(tsb_Click);
            _ts_font_bold.SmallImage = Tools4Libraries.Resources.ResourceIconSet16Default.text_bold;
            _ts_font_bold.MinSizeMode = RibbonElementSizeMode.Compact;

            _ts_font_italic = new RibbonButton();
            _ts_font_italic.ToolTip = "Italic";
            _ts_font_italic.Click += new EventHandler(tsb_Click);
            _ts_font_italic.SmallImage = Tools4Libraries.Resources.ResourceIconSet16Default.text_italic;
            _ts_font_italic.MinSizeMode = RibbonElementSizeMode.Compact;

            _ts_font_underline = new RibbonButton();
            _ts_font_underline.ToolTip = "Underline";
            _ts_font_underline.Click += new EventHandler(tsb_Click);
            _ts_font_underline.SmallImage = Tools4Libraries.Resources.ResourceIconSet16Default.text_underline;
            _ts_font_underline.MinSizeMode = RibbonElementSizeMode.Compact;

            _ts_font_strikethrought = new RibbonButton();
            _ts_font_strikethrought.ToolTip = "Strikethrouht";
            _ts_font_strikethrought.Click += new EventHandler(tsb_Click);
            _ts_font_strikethrought.SmallImage = Tools4Libraries.Resources.ResourceIconSet16Default.text_strikethroungh;
            _ts_font_strikethrought.MinSizeMode = RibbonElementSizeMode.Compact;
        }
        private void BuildToolsDrawing()
        {
            _ts_draw_pencil = new RibbonButton();
            _ts_draw_pencil.ToolTip = "Pencil";
            _ts_draw_pencil.Click += new EventHandler(tsb_Click);
            _ts_draw_pencil.SmallImage = Tools4Libraries.Resources.ResourceIconSet16Default.pencil;
            _ts_draw_pencil.MinSizeMode = RibbonElementSizeMode.Compact;

            _ts_draw_fill = new RibbonButton();
            _ts_draw_fill.ToolTip = "Fill color";
            _ts_draw_fill.Click += new EventHandler(tsb_Click);
            _ts_draw_fill.SmallImage = Tools4Libraries.Resources.ResourceIconSet16Default.paintcan;
            _ts_draw_fill.MinSizeMode = RibbonElementSizeMode.Compact;

            _ts_draw_text = new RibbonButton();
            _ts_draw_text.ToolTip = "Add text";
            _ts_draw_text.Name = _ts_draw_pencil.ToolTip;
            _ts_draw_text.Click += new EventHandler(tsb_Click);
            _ts_draw_text.SmallImage = Tools4Libraries.Resources.ResourceIconSet16Default.font;
            _ts_draw_text.MinSizeMode = RibbonElementSizeMode.Compact;

            _ts_draw_erase = new RibbonButton();
            _ts_draw_erase.ToolTip = "Erase";
            _ts_draw_erase.Click += new EventHandler(tsb_Click);
            _ts_draw_erase.SmallImage = Tools4Libraries.Resources.ResourceIconSet16Default.draw_eraser;
            _ts_draw_erase.MinSizeMode = RibbonElementSizeMode.Compact;

            _ts_draw_color_pick = new RibbonButton();
            _ts_draw_color_pick.ToolTip = "Color picker";
            _ts_draw_color_pick.Click += new EventHandler(tsb_Click);
            _ts_draw_color_pick.SmallImage = Tools4Libraries.Resources.ResourceIconSet16Default.select_by_color;
            _ts_draw_color_pick.MinSizeMode = RibbonElementSizeMode.Compact;

            _ts_draw_line = new RibbonButton();
            _ts_draw_line.ToolTip = "Draw line";
            _ts_draw_line.Click += new EventHandler(tsb_Click);
            _ts_draw_line.SmallImage = Tools4Libraries.Resources.ResourceIconSet16Default.draw_line;
            _ts_draw_line.MinSizeMode = RibbonElementSizeMode.Compact;

            _ts_draw_shape = new RibbonButton();
            _ts_draw_shape.ToolTip = "Shape";
            _ts_draw_shape.Click += new EventHandler(tsb_Click);
            _ts_draw_shape.SmallImage = Tools4Libraries.Resources.ResourceIconSet16Default.draw_polygon;
            _ts_draw_shape.MinSizeMode = RibbonElementSizeMode.Compact;

            _ts_draw_color = new RibbonButton();
            _ts_draw_color.ToolTip = "Color pallet";
            _ts_draw_color.Click += new EventHandler(tsb_Click);
            _ts_draw_color.SmallImage = Tools4Libraries.Resources.ResourceIconSet16Default.color_management;
            _ts_draw_color.MinSizeMode = RibbonElementSizeMode.Compact;

            _ts_draw_magichands = new RibbonButton();
            _ts_draw_magichands.ToolTip = "Magic hand";
            _ts_draw_magichands.Name = _ts_draw_pencil.ToolTip;
            _ts_draw_magichands.Click += new EventHandler(tsb_Click);
            _ts_draw_magichands.SmallImage = Tools4Libraries.Resources.ResourceIconSet16Default.magic_wand_2;
            _ts_draw_magichands.MinSizeMode = RibbonElementSizeMode.Compact;

            _ts_draw_paintbrush = new RibbonButton();
            _ts_draw_paintbrush.ToolTip = "Paint brush";
            _ts_draw_paintbrush.Click += new EventHandler(tsb_Click);
            _ts_draw_paintbrush.SmallImage = Tools4Libraries.Resources.ResourceIconSet16Default.paintbrush;
            _ts_draw_paintbrush.MinSizeMode = RibbonElementSizeMode.Compact;

            _ts_draw_select = new RibbonButton();
            _ts_draw_select.ToolTip = "Select";
            _ts_draw_select.Click += new EventHandler(tsb_Click);
            _ts_draw_select.SmallImage = Tools4Libraries.Resources.ResourceIconSet16Default.select;
            _ts_draw_select.MinSizeMode = RibbonElementSizeMode.Compact;

            _ts_draw_move = new RibbonButton();
            _ts_draw_move.ToolTip = "Move";
            _ts_draw_move.Click += new EventHandler(tsb_Click);
            _ts_draw_move.SmallImage = Tools4Libraries.Resources.ResourceIconSet16Default.hand;
            _ts_draw_move.MinSizeMode = RibbonElementSizeMode.Compact;
        }

        private void SelectButton(RibbonButton button)
        {
            _ts_back.Checked = _ts_back == button;
            _ts_code_barre.Checked = _ts_code_barre == button;
            _ts_code_qr.Checked = _ts_code_qr == button;
            _ts_copy.Checked = _ts_copy == button;
            _ts_cut.Checked = _ts_cut == button;
            _ts_draw_color.Checked = _ts_draw_color == button;
            _ts_draw_color_pick.Checked = _ts_draw_color_pick == button;
            _ts_draw_erase.Checked = _ts_draw_erase == button;
            _ts_draw_fill.Checked = _ts_draw_fill == button;
            _ts_draw_line.Checked = _ts_draw_line == button;
            _ts_draw_magichands.Checked = _ts_draw_magichands == button;
            _ts_draw_move.Checked = _ts_draw_move == button;
            _ts_draw_paintbrush.Checked = _ts_draw_paintbrush == button;
            _ts_draw_pencil.Checked = _ts_draw_pencil == button;
            _ts_draw_select.Checked = _ts_draw_select == button;
            _ts_draw_shape.Checked = _ts_draw_shape == button;
            _ts_draw_text.Checked = _ts_draw_text == button;
        }
        #endregion

        #region Events
        private void _ts_font_dropdown_DropDownItemClicked(object sender, RibbonItemEventArgs e)
        {
            _ts_font_dropdown.Text = _ts_font_dropdown.SelectedItem.Text;
        }
        private void _ts_font_size_dropdown_DropDownItemClicked(object sender, RibbonItemEventArgs e)
        {
            _ts_font_size_dropdown.Text = _ts_font_size_dropdown.SelectedItem.Text;
        }
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
                Log.Write("[ DEB : xxxx ] Error on the event call for action appened." + expxxx.Message);
            }
        }
        private void tsb_visibletoolpanel_Click(object sender, EventArgs e)
        {
            _visibletoolpanel = !_visibletoolpanel;
            if (_visibletoolpanel)
            {
                _ts_toolpanelvisible.Image = Tools4Libraries.Resources.ResourceIconSet32Default.setting_tools;
                _ts_toolpanelvisible.SmallImage = Tools4Libraries.Resources.ResourceIconSet16Default.setting_tools;
                _ts_toolpanelvisible.Text = "Hide";
            }
            else
            {
                _ts_toolpanelvisible.Image = Tools4Libraries.Resources.ResourceIconSet32Default.hammer;
                _ts_toolpanelvisible.SmallImage = Tools4Libraries.Resources.ResourceIconSet16Default.hammer;
                _ts_toolpanelvisible.Text = "Unhide";
            }

            ToolBarEventArgs action = new ToolBarEventArgs("visibletoolpanel");
            OnAction(action);
        }
        private void tsb_Click(object sender, EventArgs e)
        {
            try
            {
                if (sender.GetType().Name.Equals("RibbonButton"))
                {
                    RibbonButton button = (RibbonButton)sender;
                    if (!string.IsNullOrEmpty(button.Text))
                    {
                        if (button.Text.Equals("Google"))
                        {
                            _ts_web_research.SmallImage = Tools4Libraries.Resources.ResourceIconSet16Default.google;
                            _ts_web_research.Name = button.Text.ToLower();
                        }
                        if (button.Text.Equals("Flikr"))
                        {
                            _ts_web_research.SmallImage = Tools4Libraries.Resources.ResourceIconSet16Default.flickr;
                            _ts_web_research.Name = button.Text.ToLower();
                        }
                    }
                    SelectButton(button);
                    ToolBarEventArgs action = new ToolBarEventArgs(string.IsNullOrEmpty(button.Text) ? button.ToolTip : button.Text);
                    OnAction(action);
                }
            }
            catch (Exception expxxx)
            {
                Log.Write("[ DEB : xxxx ] Error on the event call for on action \"insert text\" execution." + expxxx.Message);
            }
        }
        private void _valueLookingFor_TextBoxTextChanged(object sender, EventArgs e)
        {
            try
            {
                ToolBarEventArgs action = new ToolBarEventArgs("textchanged_" + _valueLookingFor.TextBoxText);
                OnAction(action);
            }
            catch (Exception expxxx)
            {
                Log.Write("[ DEB : xxxx ] Error on the event call for on action \"layer_resize\" execution." + expxxx.Message);
            }
        }
        private void _valueLookingFor_TextBoxKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (_ts_web_research.Name.Equals("google")) { tsb_Click(_ts_web_google, null); }
                if (_ts_web_research.Name.Equals("flikr")) { tsb_Click(_ts_web_flikr, null); }
            }
        }
        private void _intImg_DiaporamaLaunched(object sender, EventArgs e)
        {
            if ((bool)sender)
            {
                _ts_diaporama.SmallImage = Tools4Libraries.Resources.ResourceIconSet16Default.control_pause;
            }
            else
            {
                _ts_diaporama.SmallImage = Tools4Libraries.Resources.ResourceIconSet16Default.control_play;
            }
        }
        #endregion
    }
}
