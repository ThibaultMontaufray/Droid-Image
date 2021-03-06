// code 37 - 12
/*
 * Crée par SharpDevelop.
 * Utilisateur: C357555
 * Date: 05/08/2011
 * Heure: 17:06
 * 
 * Pour changer ce modèle utiliser Outils | Options | Codage | Editer les en-têtes standards.
 */
//using System.Runtime.Remoting.Messaging;
using System;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using System.Net;
using System.ComponentModel;
using System.Collections;
using ZXing;
//using com.google.zxing;
//using com.google.zxing.common;
using System.Threading.Tasks;
using Tools.Slider;
using System.Runtime.InteropServices;
using PdfConverter.Converters;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using System.Diagnostics;
using Tools.Utilities;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Timers;

namespace Droid.Image
{
    public enum Mode
    {
        VIEW,
        EDITION,
        ANALYSE
    }
    public enum Rotation
    {
        LEFT,
        RIGHT,
        HALF,
        NONE
    }

    public delegate void InterfaceImageEventHandler(object sender, EventArgs e);

    /// <summary>
    /// Interface for Tobi Assistant application : take care, some french word here to allow Tobi to speak with natural langage.
    /// </summary>            
	public class Interface_image : GPInterface
    {
        #region Attributes
        public static string WORKINGDIRECTORY = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Servodroid\Droid-Image";
        public static Color THEME_COLOR = Color.DarkOrange;
        public event InterfaceImageEventHandler MessageAvailable;
        public event InterfaceImageEventHandler ImageChanged;
        public event InterfaceImageEventHandler DiaporamaLaunched;

        private bool _altPress;
        private bool _shiftPress;
        private bool _ctrlPress;
        private Mode _currentMode;
        private Tools.Slider.SliderTrackBar _trackBar;
        private static Interface_image _this;
        private new ToolStripMenuIMG _tsm;
        private List<String> _listToolStrip;
        private PictureBox _picturebox;
        private Button _buttonValidation;
        //private PictureBox pictureboxmini;
        private Panel _panelTools;
        private Panel _panelSelection;
        private TrackBar _trackbar;
        private Label _tracklabel;
        private Stream _stream;
        private Bitmap _currentImage;
        private Bitmap _comparisonImage;
        private bool _openned;
        private bool _visibletoolpanel;
        private double _zoomFactor;
        private Color _backColor;
        private Bitmap _originalImage;
        private string _imageviewmode;
        private ImageHandler _handler;
        private bool _flagCrop = false;
        private bool _flagSelection = false;
        private bool _flagDrawLine = false;
        private bool _flagDrawShape = false;
        private bool _diaporamaRunning = false;
        private bool _mouseDown = false;
        private int _initMouseX = 0;
        private int _initMouseY = 0;
        private int _currentMouseX = 0;
        private int _currentMouseY = 0;
        private byte[] _pictureByte;
        private string _textSearch;
        private bool _textSearchChanged = true;
        private Timer _timer;
        private List<string> _webSearchUrls = null;
        private string _lastOrder = string.Empty;
        private int _indexImageWeb;
        private string _serialiseString;
        private Bitmap _mask;
        private int _resizeHeight;
        private int _resizeWidth;
        #endregion

        #region Properties
        public Bitmap Mask
        {
            get { return _mask; }
            set { _mask = value; }
        }
        public ToolStripMenuIMG Tsm
        {
            get { return _tsm; }
            set { _tsm = value; }
        }
        public override bool Openned
        {
            get { return _openned; }
        }
        public Mode CurrentMode
        {
            get { return _currentMode; }
            set { _currentMode = value; }
        }
        /// <summary>
        /// Select view mode zoom | stretch | center
        /// </summary>
        public string ImageViewMode
        {
            get { return _imageviewmode; }
            set { _imageviewmode = value; }
        }
        public PictureBox PicturePreview
        {
            get { return _picturebox; }
            set { _picturebox = value; }
        }
        public string TextSearch
        {
            get { return _textSearch; }
            set { _textSearch = value; _textSearchChanged = true; }
        }
        public Bitmap CurrentImage
        {
            get { return _currentImage; }
            set
            {
                _currentImage = value;
                if (ImageChanged != null) { ImageChanged(null, null); }
            }
        }
        public Bitmap ComparisonImage
        {
            get { return _comparisonImage; }
            set { _comparisonImage = value; }
        }
        public Button ButtonValidation
        {
            get { return _buttonValidation; }
            set { _buttonValidation = value; }
        }
        public bool DiaporamaRunning
        {
            get { return _diaporamaRunning; }
            set { _diaporamaRunning = value; }
        }
        #endregion

        #region Constructor : Destructor
        static Interface_image()
        {
            _this = new Interface_image();
        }
        public Interface_image()
        {
            _currentMode = Mode.VIEW;
            _imageviewmode = "zoom";
            _openned = false;
            _zoomFactor = 1;
            _visibletoolpanel = false;
            _this = this;
            _mask = new Bitmap(Properties.Resource.mask);
            BuildToolBar();
            AddImage();
            LaunchModeView();
        }
        public Interface_image(List<String> lts)
        {
            _currentMode = Mode.VIEW;
            _imageviewmode = "zoom";
            _openned = false;
            _listToolStrip = lts;
            _zoomFactor = 1;
            _visibletoolpanel = false;
            _this = this;
            _mask = new Bitmap(Properties.Resource.mask);
            BuildToolBar();
            AddImage();
            LaunchModeView();
        }
        ~Interface_image()
        {
            if (_stream != null) _stream.Close();
        }
        #endregion

        #region Action
        [Description("french[prendre.image(nom)];english[take.picture(name)]")]
        public static Bitmap ACTION_130_take_picture(string objet)
        {
            string html = GetHtmlCode(objet);
            List<string> urls = GetUrls(html);
            var rnd = new Random();

            int randomUrl = rnd.Next(0, urls.Count - 1);

            string luckyUrl = urls[randomUrl];

            byte[] image = GetImage(luckyUrl);
            using (var ms = new MemoryStream(image))
            {
                return new Bitmap(ms);
                //Form f = new Form();
                //f.Size = new Size(200, 200);
                //f.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                //f.Controls.Add(
                //    new PictureBox() { BackgroundImage = Image.FromStream(ms), Dock = DockStyle.Fill, BackgroundImageLayout = ImageLayout.Zoom }
                //    );
                //f.ShowDialog();
            }
        }
        [Description("french[rogner.image(image,largeur,hauteur,haut,bas)];english[crop.picture(picture,width,height,top,left)]")]
        public static Bitmap ACTION_131_crop_picture(Bitmap picture, int width, int height, int top, int left)
        {
            return cropImage(picture, new Rectangle(left, top, width, height));
        }
        [Description("french[redimentionner.image(image,largeur,hauteur)];english[resize.picture(picture,width,height)]")]
        public static Bitmap ACTION_132_resize_picture(Bitmap picture, int width, int height)
        {
            return resizeImage(picture, new Size(width, height));
        }
        [Description("french[tourner.vertical(image)];english[flip.vertical(picture)]")]
        public static Bitmap ACTION_133_flip_vertical(Bitmap picture)
        {
            picture.RotateFlip(RotateFlipType.Rotate180FlipY);
            return picture;
        }
        [Description("french[tourner.horisontal(image)];english[flip.horizontal(picture)]")]
        public static Bitmap ACTION_134_flip_horizontal(Bitmap picture)
        {
            picture.RotateFlip(RotateFlipType.Rotate180FlipX);
            return picture;
        }
        [Description("french[chercher.internet(image.nom)];english[research.web(picture.name)]")]
        public static Bitmap ACTION_135_research_web(string pictureName)
        {
            string fileName = Path.Combine(WORKINGDIRECTORY, pictureName.Replace(' ', '_'));
            if (File.Exists(fileName))
            {
                Bitmap img = new Bitmap(Path.Combine(WORKINGDIRECTORY, pictureName));
                return img;
            }
            _this.TextSearch = pictureName;
            _this.LaunchGoogleImg(count: 1);
            _this._picturebox.Image?.Save(fileName);
            return _this._picturebox.Image;
        }
        [Description("french[chercher.internet(icon.nom)];english[research.web(icon.name)]")]
        public static Bitmap ACTION_135_research_icon(string iconName)
        {
            try
            {
                string fileName = Path.Combine(WORKINGDIRECTORY, "icon_" + iconName.Replace(' ', '_'));
                if (File.Exists(fileName))
                {
                    Bitmap img = new Bitmap(fileName);
                    return img;
                }
                _this.TextSearch = iconName;
                _this.LaunchGoogleImg(count: 1, icon: true);
                _this._picturebox.Image?.Save(fileName);
                return _this._picturebox.Image;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error in lib Images : " + e.Message);
                return null;
            }
        }
        [Description("french[serialiser.image(image)];english[serialize.image(picture)]")]
        public static string ACTION_136_serialize_image(Bitmap image)
        {
            _this.CurrentImage = image;
            _this.LaunchSerializeImage();
            return _this._serialiseString;
        }
        [Description("french[deserialiser.image(string)];english[unserialize.image(string)]")]
        public static Bitmap ACTION_137_unserialize_image(string serialise)
        {
            _this._serialiseString = serialise;
            _this.LaunchUnserializeString();
            return _this._currentImage;
        }
        [Description("french[appliquer.masque(image,masque)];english[apply.mask(image,mask)]")]
        public static Bitmap ACTION_138_apply_mask(Bitmap image, Bitmap mask = null)
        {
            _this.CurrentImage = image;
            if (mask != null) _this.Mask = new Bitmap(mask);
            _this.LaunchApplyMask();
            return _this._currentImage;
        }
        [Description("french[comparer.image(première_image,seconde_image)];english[compare.image(first_image,second_image)]")]
        public static int ACTION_139_compare(Bitmap first_image, Bitmap second_image)
        {
            _this.CurrentImage = first_image;
            _this.ComparisonImage = second_image;
            return _this.LaunchCompare();
        }
        [Description("french[convertir.pdf(fichier)];english[convert.pdf(file)]")]
        public static void ACTION_140_convert(string path)
        {
            FileInfo fi = new FileInfo(path);
            string file = fi.Directory + "\\" + fi.Name.Replace(fi.Extension, string.Empty) + ".pdf";
            PdfDocument document = new PdfDocument();

            PdfPage page = document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);
            
            gfx.DrawImage(XImage.FromFile(path), 0, 0, page.Width, page.Height);
            document.Save(file);
        }
        [Description("french[télécharger.image(url)];english[download.image(url)]")]
        public static Bitmap ACTION_141_download_image(string url)
        {
            try
            {
                System.Net.HttpWebRequest webRequest = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(url);
                webRequest.AllowWriteStreamBuffering = true;
                webRequest.Timeout = 30000;
                System.Net.WebResponse webResponse = webRequest.GetResponse();
                System.IO.Stream stream = webResponse.GetResponseStream();
                Bitmap image = new Bitmap(stream);
                webResponse.Close();
                return image;
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.Message);
                return null;
            }
        }
        #endregion

        #region Methods Public
        public override bool Open(object o)
        {
            if (o != null)
            {
                _stream = (Stream)o;
            }
            ProcessOpenned();
            return _openned;
        }
        public override void Print()
        {

        }
        public override void Close()
        {
            try
            {
                _stream.Close();
            }
            catch
            {

            }
        }
        public override bool Save()
        {
            string path = null;
            string filename = "";
            long quality = 0;

            // Encoder parameter for image quality
            EncoderParameter qualityParam = new EncoderParameter(Encoder.Quality, quality);

            // Jpeg image codec
            ImageCodecInfo jpegCodec = GetEncoderInfo(filename);

            if (jpegCodec == null)
                return false;

            EncoderParameters encoderParams = new EncoderParameters(1);
            encoderParams.Param[0] = qualityParam;

            _picturebox.Image.Save(path, jpegCodec, encoderParams);
            return true;
        }
        public override void ZoomIn()
        {

        }
        public override void ZoomOut()
        {

        }
        public override void Copy()
        {

        }
        public override void Cut()
        {

        }
        public override void Paste()
        {

        }
        public new void Resize()
        {
            if (_visibletoolpanel)
            {
                _sheet.Size = new Size(_tsm.CurrentTabPage.Parent.Width - 130, _tsm.CurrentTabPage.Parent.Height - 45);
                _panelTools.Size = new Size(120, _tsm.CurrentTabPage.Parent.Height - 45);
                _panelTools.Left = _sheet.Width;

                _picturebox.Refresh();
                //pictureboxmini.Refresh();
            }
            else
            {
                if (_tsm.CurrentTabPage != null && _sheet != null && _picturebox != null && _tsm.CurrentTabPage.Parent != null)
                {
                    _sheet.Size = new Size(_tsm.CurrentTabPage.Parent.Width - 10, _tsm.CurrentTabPage.Parent.Height - 45);
                }
                if (_picturebox != null) _picturebox.Refresh();
            }
        }
        public new void Refresh()
        {
            Resize();
        }
        public new void GlobalAction(object sender, EventArgs e)
        {
            try
            {
                ToolBarEventArgs tbea = e as ToolBarEventArgs;
                string action = tbea.EventText;

                if (string.IsNullOrEmpty(action)) return;
                if (action.StartsWith("textchanged_"))
                {
                    if (action.Split('_').Length > 0)
                    {
                        _textSearch = action.Split('_')[1];
                        _textSearchChanged = true;
                    }
                    return;
                }

                _lastOrder = action.ToLower();
                GoAction(_lastOrder);
            }
            catch (Exception exp3711)
            {
                Log.Write("[ CRT : 3711 ] Cannot execute the command." + exp3711.Message);
            }
        }
        public override void GoAction(string action)
        {
            if (!string.IsNullOrEmpty(action))
            {
                switch (action.ToLower())
                {
                    case "edition":
                        LaunchModeEdition();
                        break;
                    case "analyse":
                        LaunchModeAnalyse();
                        break;
                    case "view":
                        LaunchModeView();
                        break;
                    case "apply mask":
                        LaunchApplyMask();
                        break;
                    case "open":
                        LaunchOpen();
                        break;
                    case "rotation":
                        LaunchRotation();
                        break;
                    case "rotation left":
                        LaunchRotationL();
                        break;
                    case "rotation right":
                        LaunchRotationR();
                        break;
                    case "return vertical":
                        LaunchReturnV();
                        break;
                    case "return horizontal":
                        LaunchReturnH();
                        break;
                    case "center":
                        LaunchCenterImage();
                        break;
                    case "stretch":
                        LaunchStretchImage();
                        break;
                    case "autosize":
                        LaunchAutosizeImage();
                        break;
                    case "adjust":
                        LaunchAdjustImage();
                        break;
                    case "visibletoolpanel":
                        LaunchVisibletoolpanel();
                        break;
                    case "back":
                        LaunchBack();
                        break;
                    case "next":
                        LaunchNext();
                        break;
                    case "resize":
                        LaunchResize();
                        break;
                    case "crop":
                        LaunchCrop();
                        break;
                    case "select":
                        LaunchCrop();
                        break;
                    case "draw_line":
                        LaunchCrop();
                        break;
                    case "draw_shape":
                        LaunchCrop();
                        break;
                    case "light":
                        LaunchBrightness();
                        break;
                    case "constraste":
                        LaunchContrast();
                        break;
                    case "green":
                        LaunchFilterGreen();
                        break;
                    case "red":
                        LaunchFilterRed();
                        break;
                    case "blue":
                        LaunchFilterBlue();
                        break;
                    case "gray":
                        LaunchGrayscale();
                        break;
                    case "invert":
                        LaunchInvert();
                        break;
                    case "code barres":
                        LaunchCodeBar();
                        break;
                    case "qr code":
                        LaunchCodeQr();
                        break;
                    case "google":
                        LaunchGoogleImg();
                        break;
                    case "flikr":
                        LaunchFlikrImg();
                        break;
                    case "picture analysing":
                        LaunchRecognition();
                        break;
                    case "copy":
                        Copy();
                        break;
                    case "cut":
                        Cut();
                        break;
                    case "paste":
                        Paste();
                        break;
                    case "undo":
                        LaunchUndo();
                        break;
                    case "redo":
                        LaunchRedo();
                        break;
                    case "diaporama":
                        LaunchDiaporama();
                        break;
                    case "compare":
                        LaunchCompare();
                        break;
                    case "save":
                        LaunchSave();
                        break;
                    case "save as":
                        LaunchSaveAs();
                        break;
                }
            }
        }
        public RibbonTab BuildToolBar()
        {
            _tsm = new ToolStripMenuIMG(this);
            return _tsm;
        }
        public void ProcessKeyDown(Keys key)
        {
            if (key == Keys.Alt || key == Keys.Menu) { _altPress = true; }
            else if (key == Keys.ShiftKey) { _shiftPress = true; }
            else if (key == Keys.ControlKey) { _ctrlPress = true; }
            else if (_ctrlPress && _shiftPress && key == Keys.S) { LaunchSaveAs(); }
            else if (_ctrlPress && key == Keys.S) { LaunchSave(); }
            else
            {
                switch (_currentMode)
                {
                    case Mode.EDITION:
                        break;
                    case Mode.ANALYSE:
                        break;
                    case Mode.VIEW:
                        if (_altPress && key == Keys.Left) { LaunchRotationL(); }
                        else if (_altPress && key == Keys.Right) { LaunchRotationR(); }
                        else if (_altPress && key == Keys.Up) { LaunchCenterImage(); }
                        else if (_altPress && key == Keys.Down) { LaunchAdjustImage(); }
                        else if (key == Keys.Left) { LaunchBack(); }
                        else if (key == Keys.Right) { LaunchNext(); }
                        break;
                }
            }
        }
        public void ProcessKeyUp(Keys key)
        {
            if (key == Keys.Alt || key == Keys.Menu)
            {
                _altPress = false;
            }
            else if (key == Keys.Shift)
            {
                _shiftPress = false;
            }
            else if (key == Keys.Control)
            {
                _ctrlPress = false;
            }
            else
            {
                switch (_currentMode)
                {
                    case Mode.EDITION:
                        break;
                    case Mode.ANALYSE:
                        break;
                    case Mode.VIEW:
                        break;
                }
            }
        }

        public static Color ChangeColorBrightness(Color color, float correctionFactor)
        {
            float red = (float)color.R;
            float green = (float)color.G;
            float blue = (float)color.B;

            if (correctionFactor < 0)
            {
                correctionFactor = 1 + correctionFactor;
                red *= correctionFactor;
                green *= correctionFactor;
                blue *= correctionFactor;
            }
            else
            {
                red = (255 - red) * correctionFactor + red;
                green = (255 - green) * correctionFactor + green;
                blue = (255 - blue) * correctionFactor + blue;
            }

            return Color.FromArgb(color.A, (int)red, (int)green, (int)blue);
        }
        public static Bitmap Brightness(Bitmap img, float correctionFactor)
        {
            float FinalValue = (float)correctionFactor / 255.0f;
            ColorMatrix TempMatrix = new ColorMatrix();
            TempMatrix.Matrix = new float[][]{
                 new float[] {1, 0, 0, 0, 0},
                 new float[] {0, 1, 0, 0, 0},
                 new float[] {0, 0, 1, 0, 0},
                 new float[] {0, 0, 0, 1, 0},
                 new float[] {FinalValue, FinalValue, FinalValue, 1, 1}
             };
            return TempMatrix.Apply(img);
        }
        public static Bitmap ConvertSepiaTone(Bitmap img)
        {
            ColorMatrix TempMatrix = new ColorMatrix();
            TempMatrix.Matrix = new float[][]{
                    new float[] {.393f, .349f, .272f, 0, 0},
                    new float[] {.769f, .686f, .534f, 0, 0},
                    new float[] {.189f, .168f, .131f, 0, 0},
                    new float[] {0, 0, 0, 1, 0},
                    new float[] {0, 0, 0, 0, 1}
                };
            return TempMatrix.Apply(img);
        }
        public static Bitmap KuwaharaBlur(Bitmap img, int Size)
        {
            Bitmap TempBitmap = new Bitmap(img);
            Bitmap NewBitmap = new Bitmap(TempBitmap.Width, TempBitmap.Height);
            Graphics NewGraphics = Graphics.FromImage(NewBitmap);
            NewGraphics.DrawImage(TempBitmap, new Rectangle(0, 0, TempBitmap.Width, TempBitmap.Height), new Rectangle(0, 0, TempBitmap.Width, TempBitmap.Height), GraphicsUnit.Pixel);
            NewGraphics.Dispose();
            Random TempRandom = new Random();
            int[] ApetureMinX = { -(Size / 2), 0, -(Size / 2), 0 };
            int[] ApetureMaxX = { 0, (Size / 2), 0, (Size / 2) };
            int[] ApetureMinY = { -(Size / 2), -(Size / 2), 0, 0 };
            int[] ApetureMaxY = { 0, 0, (Size / 2), (Size / 2) };
            for (int x = 0; x < NewBitmap.Width; ++x)
            {
                for (int y = 0; y < NewBitmap.Height; ++y)
                {
                    int[] RValues = { 0, 0, 0, 0 };
                    int[] GValues = { 0, 0, 0, 0 };
                    int[] BValues = { 0, 0, 0, 0 };
                    int[] NumPixels = { 0, 0, 0, 0 };
                    int[] MaxRValue = { 0, 0, 0, 0 };
                    int[] MaxGValue = { 0, 0, 0, 0 };
                    int[] MaxBValue = { 0, 0, 0, 0 };
                    int[] MinRValue = { 255, 255, 255, 255 };
                    int[] MinGValue = { 255, 255, 255, 255 };
                    int[] MinBValue = { 255, 255, 255, 255 };
                    for (int i = 0; i < 4; ++i)
                    {
                        for (int x2 = ApetureMinX[i]; x2 < ApetureMaxX[i]; ++x2)
                        {
                            int TempX = x + x2;
                            if (TempX >= 0 && TempX < NewBitmap.Width)
                            {
                                for (int y2 = ApetureMinY[i]; y2 < ApetureMaxY[i]; ++y2)
                                {
                                    int TempY = y + y2;
                                    if (TempY >= 0 && TempY < NewBitmap.Height)
                                    {
                                        Color TempColor = TempBitmap.GetPixel(TempX, TempY);
                                        RValues[i] += TempColor.R;
                                        GValues[i] += TempColor.G;
                                        BValues[i] += TempColor.B;
                                        if (TempColor.R > MaxRValue[i])
                                        {
                                            MaxRValue[i] = TempColor.R;
                                        }
                                        else if (TempColor.R < MinRValue[i])
                                        {
                                            MinRValue[i] = TempColor.R;
                                        }

                                        if (TempColor.G > MaxGValue[i])
                                        {
                                            MaxGValue[i] = TempColor.G;
                                        }
                                        else if (TempColor.G < MinGValue[i])
                                        {
                                            MinGValue[i] = TempColor.G;
                                        }

                                        if (TempColor.B > MaxBValue[i])
                                        {
                                            MaxBValue[i] = TempColor.B;
                                        }
                                        else if (TempColor.B < MinBValue[i])
                                        {
                                            MinBValue[i] = TempColor.B;
                                        }
                                        ++NumPixels[i];
                                    }
                                }
                            }
                        }
                    }
                    int j = 0;
                    int MinDifference = 10000;
                    for (int i = 0; i < 4; ++i)
                    {
                        int CurrentDifference = (MaxRValue[i] - MinRValue[i]) + (MaxGValue[i] - MinGValue[i]) + (MaxBValue[i] - MinBValue[i]);
                        if (CurrentDifference < MinDifference && NumPixels[i] > 0)
                        {
                            j = i;
                            MinDifference = CurrentDifference;
                        }
                    }

                    Color MeanPixel = Color.FromArgb(RValues[j] / NumPixels[j],
                        GValues[j] / NumPixels[j],
                        BValues[j] / NumPixels[j]);
                    NewBitmap.SetPixel(x, y, MeanPixel);
                }
            }
            return (Bitmap)NewBitmap;
        }
        public static Bitmap AdjustContrast(Bitmap OriginalImage, float Value)
        {
            Bitmap NewBitmap = new Bitmap(OriginalImage.Width, OriginalImage.Height);
            //BitmapData NewData = Image.LockImage(NewBitmap);
            //BitmapData OldData = Image.LockImage(OriginalImage);
            //int NewPixelSize = Image.GetPixelSize(NewData);
            //int OldPixelSize = Image.GetPixelSize(OldData);
            //Value = (100.0f + Value) / 100.0f;
            //Value *= Value;

            //for (int x = 0; x < NewBitmap.Width; ++x)
            //{
            //    for (int y = 0; y < NewBitmap.Height; ++y)
            //    {
            //        Color Pixel = Image.GetPixel(OldData, x, y, OldPixelSize);
            //        float Red = Pixel.R / 255.0f;
            //        float Green = Pixel.G / 255.0f;
            //        float Blue = Pixel.B / 255.0f;
            //        Red = (((Red - 0.5f) * Value) + 0.5f) * 255.0f;
            //        Green = (((Green - 0.5f) * Value) + 0.5f) * 255.0f;
            //        Blue = (((Blue - 0.5f) * Value) + 0.5f) * 255.0f;
            //        Image.SetPixel(NewData, x, y,
            //            Color.FromArgb(MathHelper.Clamp((int)Red, 255, 0),
            //            MathHelper.Clamp((int)Green, 255, 0),
            //            MathHelper.Clamp((int)Blue, 255, 0)),
            //            NewPixelSize);
            //    }
            //}
            //Image.UnlockImage(NewBitmap, NewData);
            //Image.UnlockImage(OriginalImage, OldData);
            return NewBitmap;
        }
        public static Bitmap Dilate(Bitmap img, int Size)
        {
            Bitmap TempBitmap = new Bitmap(img);
            Bitmap NewBitmap = new Bitmap(TempBitmap.Width, TempBitmap.Height);
            Graphics NewGraphics = Graphics.FromImage(NewBitmap);
            NewGraphics.DrawImage(TempBitmap, new Rectangle(0, 0, TempBitmap.Width, TempBitmap.Height), new Rectangle(0, 0, TempBitmap.Width, TempBitmap.Height), GraphicsUnit.Pixel);
            NewGraphics.Dispose();
            Random TempRandom = new Random();
            int ApetureMin = -(Size / 2);
            int ApetureMax = (Size / 2);
            for (int x = 0; x < NewBitmap.Width; ++x)
            {
                for (int y = 0; y < NewBitmap.Height; ++y)
                {
                    int RValue = 0;
                    int GValue = 0;
                    int BValue = 0;
                    for (int x2 = ApetureMin; x2 < ApetureMax; ++x2)
                    {
                        int TempX = x + x2;
                        if (TempX >= 0 && TempX < NewBitmap.Width)
                        {
                            for (int y2 = ApetureMin; y2 < ApetureMax; ++y2)
                            {
                                int TempY = y + y2;
                                if (TempY >= 0 && TempY < NewBitmap.Height)
                                {
                                    Color TempColor = TempBitmap.GetPixel(TempX, TempY);
                                    if (TempColor.R > RValue)
                                        RValue = TempColor.R;
                                    if (TempColor.G > GValue)
                                        GValue = TempColor.G;
                                    if (TempColor.B > BValue)
                                        BValue = TempColor.B;
                                }
                            }
                        }
                    }
                    Color TempPixel = Color.FromArgb(RValue, GValue, BValue);
                    NewBitmap.SetPixel(x, y, TempPixel);
                }
            }
            return NewBitmap;
        }
        /// <summary>
        /// Adjusts the Gamma
        /// </summary>
        /// <param name="OriginalImage">Image to change</param>
        /// <param name="Value">Used to build the gamma ramp (usually .2 to 5)</param>
        /// <returns>A bitmap object</returns>
        public static Bitmap AdjustGamma(Bitmap OriginalImage, float Value)
        {
            Bitmap NewBitmap = new Bitmap(OriginalImage.Width, OriginalImage.Height);
            //BitmapData NewData = Image.LockImage(NewBitmap);
            //BitmapData OldData = Image.LockImage(OriginalImage);
            //int NewPixelSize = Image.GetPixelSize(NewData);
            //int OldPixelSize = Image.GetPixelSize(OldData);

            //int[] RedRamp = new int[256];
            //int[] GreenRamp = new int[256];
            //int[] BlueRamp = new int[256];
            //for (int x = 0; x < 256; ++x)
            //{
            //    RedRamp[x] = MathHelper.Clamp((int)((255.0 * System.Math.Pow(x / 255.0, 1.0 / Value)) + 0.5), 255, 0);
            //    GreenRamp[x] = MathHelper.Clamp((int)((255.0 * System.Math.Pow(x / 255.0, 1.0 / Value)) + 0.5), 255, 0);
            //    BlueRamp[x] = MathHelper.Clamp((int)((255.0 * System.Math.Pow(x / 255.0, 1.0 / Value)) + 0.5), 255, 0);
            //}

            //for (int x = 0; x < NewBitmap.Width; ++x)
            //{
            //    for (int y = 0; y < NewBitmap.Height; ++y)
            //    {
            //        Color Pixel = Image.GetPixel(OldData, x, y, OldPixelSize);
            //        int Red = RedRamp[Pixel.R];
            //        int Green = GreenRamp[Pixel.G];
            //        int Blue = BlueRamp[Pixel.B];
            //        Image.SetPixel(NewData, x, y, Color.FromArgb(Red, Green, Blue), NewPixelSize);
            //    }
            //}

            //Image.UnlockImage(NewBitmap, NewData);
            //Image.UnlockImage(OriginalImage, OldData);
            return NewBitmap;
        }
        //public static Image BoxBlur(Image Image, int Size)
        //{
        //    Filter TempFilter = new Filter(Size, Size);
        //    for (int x = 0; x < Size; ++x)
        //    {
        //        for (int y = 0; y < Size; ++y)
        //        {
        //            TempFilter.MyFilter[x, y] = 1;
        //        }
        //    }
        //    return TempFilter.ApplyFilter(Image);
        //}
        public static Bitmap MedianFilter(Bitmap Image, int Size)
        {
            Bitmap TempBitmap = Image;
            Bitmap NewBitmap = new Bitmap(TempBitmap.Width, TempBitmap.Height);
            Graphics NewGraphics = Graphics.FromImage(NewBitmap);
            NewGraphics.DrawImage(TempBitmap, new System.DrawingCore.Rectangle(0, 0, TempBitmap.Width, TempBitmap.Height), new Rectangle(0, 0, TempBitmap.Width, TempBitmap.Height), GraphicsUnit.Pixel);
            NewGraphics.Dispose();
            Random TempRandom = new Random();
            int ApetureMin = -(Size / 2);
            int ApetureMax = (Size / 2);
            for (int x = 0; x < NewBitmap.Width; ++x)
            {
                for (int y = 0; y < NewBitmap.Height; ++y)
                {
                    List<int> RValues = new List<int>();
                    List<int> GValues = new List<int>();
                    List<int> BValues = new List<int>();
                    for (int x2 = ApetureMin; x2 < ApetureMax; ++x2)
                    {
                        int TempX = x + x2;
                        if (TempX >= 0 && TempX < NewBitmap.Width)
                        {
                            for (int y2 = ApetureMin; y2 < ApetureMax; ++y2)
                            {
                                int TempY = y + y2;
                                if (TempY >= 0 && TempY < NewBitmap.Height)
                                {
                                    Color TempColor = TempBitmap.GetPixel(TempX, TempY);
                                    RValues.Add(TempColor.R);
                                    GValues.Add(TempColor.G);
                                    BValues.Add(TempColor.B);
                                }
                            }
                        }
                    }
                    RValues.Sort();
                    GValues.Sort();
                    BValues.Sort();
                    Color MedianPixel = Color.FromArgb(RValues[RValues.Count / 2],
                        GValues[GValues.Count / 2],
                        BValues[BValues.Count / 2]);
                    NewBitmap.SetPixel(x, y, MedianPixel);
                }
            }
            return NewBitmap;
        }
        #endregion

        #region Methods Protected
        protected void OnMessageAvailable(object sender, EventArgs e)
        {
            if (MessageAvailable != null)
            {
                MessageAvailable(sender, e);
            }
        }
        protected void OnDiaporamaLaunched(object sender, EventArgs e)
        {
            if (DiaporamaLaunched != null)
            {
                DiaporamaLaunched(sender, e);
            }
        }
        #endregion

        #region Methods Private Launchers
        private void LaunchOpen()
        {
            _webSearchUrls = null;
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                if (_stream != null) { _stream.Close(); }
                PathFile = ofd.FileName;
                _stream = File.OpenRead(PathFile);
                CurrentImage = new Bitmap(PathFile);
            }
            ProcessOpenned();
        }
        private void LaunchSaveAs()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.FileName = PathFile;
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                PathFile = sfd.FileName;
                _currentImage.Save(PathFile);
            }
        }
        private void LaunchSave()
        {
            if (!string.IsNullOrEmpty(PathFile)) { _currentImage.Save(PathFile); }
            else { LaunchSaveAs(); }
        }
        private void LaunchVisibletoolpanel()
        {
            _visibletoolpanel = !_visibletoolpanel;
            Resize();
        }
        private void LaunchCenterImage()
        {
            _picturebox.SizeMode = PictureBoxSizeMode.CenterImage;
        }
        private void LaunchStretchImage()
        {
            _picturebox.SizeMode = PictureBoxSizeMode.StretchImage;
        }
        private void LaunchAutosizeImage()
        {
            _picturebox.SizeMode = PictureBoxSizeMode.AutoSize;
        }
        private void LaunchAdjustImage()
        {
            _picturebox.SizeMode = PictureBoxSizeMode.Zoom;
        }
        private void LaunchReturnV()
        {
            Bitmap img = _picturebox.Image;
            img.RotateFlip(RotateFlipType.Rotate180FlipY);
            _picturebox.Image = img;
        }
        private void LaunchReturnH()
        {
            Bitmap img = _picturebox.Image;
            img.RotateFlip(RotateFlipType.Rotate180FlipX);
            _picturebox.Image = img;
        }
        private void LaunchRotationL()
        {
            Bitmap img = _picturebox.Image;
            img.RotateFlip(RotateFlipType.Rotate270FlipNone);
            _picturebox.Image = img;
        }
        private void LaunchRotationR()
        {
            Bitmap img = _picturebox.Image;
            img.RotateFlip(RotateFlipType.Rotate90FlipNone);
            _picturebox.Image = img;
        }
        private void LaunchRotation()
        {
            RotateImage(ToolStripMenuIMG._rotationvalue);
        }
        private void LaunchBack()
        {
            try
            {
                if (_webSearchUrls != null)
                {
                    LaunchGoogleImg("back");
                }
                else if (!string.IsNullOrEmpty(PathFile))
                {
                    int index = -1;

                    string folder = Path.GetDirectoryName(PathFile);
                    string[] fichiers;
                    fichiers = System.IO.Directory.GetFiles(folder);

                    for (int i = 0; i < fichiers.Length - 1; i++)
                    {
                        if (this.PathFile.Contains(fichiers[i]))
                        {
                            index = i;
                            break;
                        }
                    }
                    index = determinePrevPictureIndex(fichiers, index);

                    if (_stream != null) { _stream.Close(); }
                    PathFile = fichiers[index];
                    _stream = File.OpenRead(PathFile);
                    CurrentImage = new Bitmap(PathFile);
                    ProcessOpenned();
                }
            }
            catch (Exception exp3709)
            {
                Log.Write("[ ERR : 3709 ] Cannot load the previews picture." + exp3709.Message);
            }
        }
        private void LaunchNext()
        {
            try
            {
                if (_webSearchUrls != null)
                {
                    LaunchGoogleImg("next");
                }
                else if (!string.IsNullOrEmpty(PathFile))
                {
                    int index = -1;

                    string folder = Path.GetDirectoryName(PathFile);
                    string[] fichiers;
                    fichiers = System.IO.Directory.GetFiles(folder);

                    for (int i = 0; i < fichiers.Length - 1; i++)
                    {
                        if (this.PathFile.Contains(fichiers[i]))
                        {
                            index = i;
                        }
                    }
                    index = determineNextPictureIndex(fichiers, index);

                    if (_stream != null) { _stream.Close(); }
                    PathFile = fichiers[index];
                    _stream = File.OpenRead(PathFile);
                    CurrentImage = new Bitmap(PathFile);
                    ProcessOpenned();
                }
            }
            catch (Exception exp3710)
            {
                Log.Write("[ ERR : 3710 ] Cannot load the next picture." + exp3710.Message);
            }
        }
        private void LaunchUndo()
        {
            if (_handler != null)
            {
                _handler.ResetBitmap();
                _picturebox.Image = _handler.CurrentBitmap;
            }
        }
        private void LaunchRedo()
        {
        }
        private void LaunchClearImage()
        {
            if (_handler != null)
            {
                _handler.RestorePrevious();
                _handler.ClearImage();
                _picturebox.Image = _handler.CurrentBitmap;
            }
        }
        private void LaunchImageInfo()
        {
            if (_handler != null)
            {
            }
        }
        private void LaunchZoom50()
        {
            //_zoomFactor = 0.5;
            //_cZoom.Checked = false;
            //menuItemZoom50.Checked = true;
            //_cZoom = menuItemZoom50;
            //this.AutoScrollMinSize = new Size(Convert.ToInt32(_handler.CurrentBitmap.Width * zoomFactor), Convert.ToInt32(_handler.CurrentBitmap.Height * zoomFactor));
            //this.Invalidate();
        }
        private void LaunchZoom100()
        {
            //zoomFactor = 1.0;
            //_cZoom.Checked = false;
            //menuItemZoom100.Checked = true;
            //_cZoom = menuItemZoom100;
            //this.AutoScrollMinSize = new Size(Convert.ToInt32(_handler.CurrentBitmap.Width * zoomFactor), Convert.ToInt32(_handler.CurrentBitmap.Height * zoomFactor));
            //this.Invalidate();
        }
        private void LaunchZoom150()
        {
            //zoomFactor = 1.5;
            //_cZoom.Checked = false;
            //menuItemZoom150.Checked = true;
            //_cZoom = menuItemZoom150;
            //this.AutoScrollMinSize = new Size(Convert.ToInt32(_handler.CurrentBitmap.Width * zoomFactor), Convert.ToInt32(_handler.CurrentBitmap.Height * zoomFactor));
            //this.Invalidate();
        }
        private void LaunchFilterRed()
        {
            if (_handler != null)
            {
                _handler.RestorePrevious();
                _handler.SetColorFilter(ImageHandler.ColorFilterTypes.Red);
                _picturebox.Image = _handler.CurrentBitmap;
            }
        }
        private void LaunchFilterGreen()
        {
            if (_handler != null)
            {
                _handler.RestorePrevious();
                _handler.SetColorFilter(ImageHandler.ColorFilterTypes.Green);
                _picturebox.Image = _handler.CurrentBitmap;
            }
        }
        private void LaunchFilterBlue()
        {
            if (_handler != null)
            {
                _handler.RestorePrevious();
                _handler.SetColorFilter(ImageHandler.ColorFilterTypes.Blue);
                _picturebox.Image = _handler.CurrentBitmap;
            }
        }
        private void LaunchGamma()
        {
            if (_handler != null)
            {
                //GammaForm gFrm = new GammaForm();
                //gFrm.RedComponent = gFrm.GreenComponent = gFrm.BlueComponent = 0;
                //if (gFrm.ShowDialog() == DialogResult.OK)
                //{
                //    _handler.RestorePrevious();
                //    _handler.SetGamma(gFrm.RedComponent, gFrm.GreenComponent, gFrm.BlueComponent);
                //}
            }
        }
        private void LaunchBrightness()
        {
            if (_handler != null)
            {
                //BrightnessForm bFrm = new BrightnessForm();
                //bFrm.BrightnessValue = 0;
                //if (bFrm.ShowDialog() == DialogResult.OK)
                //{
                //    _handler.RestorePrevious();
                //    _handler.SetBrightness(bFrm.BrightnessValue);
                //}
            }
        }
        private void LaunchContrast()
        {
            if (_handler != null)
            {
                //ContrastForm cFrm = new ContrastForm();
                //cFrm.ContrastValue = 0;
                //if (cFrm.ShowDialog() == DialogResult.OK)
                //{
                //    this.Cursor = Cursors.WaitCursor;
                //    _handler.RestorePrevious();
                //    _handler.SetContrast(cFrm.ContrastValue);
                //    this.Invalidate();
                //    this.Cursor = Cursors.Default;
                //}
            }
        }
        private void LaunchGrayscale()
        {
            if (_handler != null)
            {
                _handler.RestorePrevious();
                _handler.SetGrayscale();
                _picturebox.Image = _handler.CurrentBitmap;
            }
        }
        private void LaunchInvert()
        {
            if (_handler != null)
            {
                _handler.RestorePrevious();
                _handler.SetInvert();
                _picturebox.Image = _handler.CurrentBitmap;
            }
        }
        private void LaunchResize()
        {
            var destRect = new Rectangle(0, 0, _resizeWidth, _resizeHeight);
            var destImage = new Bitmap(_resizeWidth, _resizeHeight);

            destImage.SetResolution(_currentImage.HorizontalResolution, _currentImage.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(_currentImage, destRect, 0, 0, _currentImage.Width, _currentImage.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }
            _currentImage = destImage;



            //InsertTextForm1 rFrm = new InsertTextForm1();
            //rFrm.NewWidth = _handler.CurrentBitmap.Width;
            //rFrm.NewHeight = _handler.CurrentBitmap.Height;
            //if (rFrm.ShowDialog() == DialogResult.OK)
            //{
            //    this.Cursor = Cursors.WaitCursor;
            //    _handler.RestorePrevious();
            //    _handler.Resize(rFrm.NewWidth, rFrm.NewHeight);
            //    this.AutoScrollMinSize = new Size(Convert.ToInt32(_handler.CurrentBitmap.Width * zoomFactor), Convert.ToInt32(_handler.CurrentBitmap.Height * zoomFactor));
            //    this.Invalidate();
            //    this.Cursor = Cursors.Default;
            //}
        }
        private void LaunchRotate90()
        {
            if (_handler != null)
            {
                //_handler.RotateFlip(RotateFlipType.Rotate90FlipNone);
                //this.AutoScroll = true;
                //this.AutoScrollMinSize = new Size(Convert.ToInt32(_handler.CurrentBitmap.Width * zoomFactor), Convert.ToInt32(_handler.CurrentBitmap.Height * zoomFactor));
                //this.Invalidate();
            }
        }
        private void LaunchRotate180()
        {
            if (_handler != null)
            {
                //_handler.RotateFlip(RotateFlipType.Rotate180FlipNone);
                //this.AutoScroll = true;
                //this.AutoScrollMinSize = new Size(Convert.ToInt32(_handler.CurrentBitmap.Width * zoomFactor), Convert.ToInt32(_handler.CurrentBitmap.Height * zoomFactor));
                //this.Invalidate();
            }
        }
        private void LaunchRotate270()
        {
            if (_handler != null)
            {
                //_handler.RotateFlip(RotateFlipType.Rotate270FlipNone);
                //this.AutoScroll = true;
                //this.AutoScrollMinSize = new Size(Convert.ToInt32(_handler.CurrentBitmap.Width * zoomFactor), Convert.ToInt32(_handler.CurrentBitmap.Height * zoomFactor));
                //this.Invalidate();
            }
        }
        private void LaunchFlipH()
        {
            if (_handler != null)
            {
                //_handler.RotateFlip(RotateFlipType.RotateNoneFlipX);
                //this.AutoScroll = true;
                //this.AutoScrollMinSize = new Size(Convert.ToInt32(_handler.CurrentBitmap.Width * zoomFactor), Convert.ToInt32(_handler.CurrentBitmap.Height * zoomFactor));
                //this.Invalidate();
            }
        }
        private void LaunchFlipV()
        {
            if (_handler != null)
            {
                //_handler.RotateFlip(RotateFlipType.RotateNoneFlipY);
                //this.AutoScroll = true;
                //this.AutoScrollMinSize = new Size(Convert.ToInt32(_handler.CurrentBitmap.Width * zoomFactor), Convert.ToInt32(_handler.CurrentBitmap.Height * zoomFactor));
                //this.Invalidate();
            }
        }
        private void LaunchInsertText()
        {
            if (_handler != null)
            {
                //InsertTextForm itFrm = new InsertTextForm();
                //itFrm.XPosition = itFrm.YPosition = 0;
                //if (itFrm.ShowDialog() == DialogResult.OK)
                //{
                //    _handler.RestorePrevious();
                //    _handler.InsertText(itFrm.DisplayText, itFrm.XPosition, itFrm.YPosition, itFrm.DisplayTextFont, itFrm.DisplayTextFontSize, itFrm.DisplayTextFontStyle, itFrm.DisplayTextForeColor1, itFrm.DisplayTextForeColor2);
                //    this.Invalidate();
                //}
            }
        }
        private void LaunchInsertImage()
        {
            if (_handler != null)
            {
                //InsertImageForm iiFrm = new InsertImageForm();
                //iiFrm.XPosition = iiFrm.YPosition = 0;
                //if (iiFrm.ShowDialog() == DialogResult.OK)
                //{
                //    _handler.RestorePrevious();
                //    _handler.InsertImage(iiFrm.DisplayImagePath, iiFrm.XPosition, iiFrm.YPosition);
                //    this.Invalidate();
                //}
            }
        }
        private void LaunchInsertShape()
        {
            if (_handler != null)
            {
                //InsertShapeForm isFrm = new InsertShapeForm();
                //isFrm.XPosition = isFrm.YPosition = 0;
                //if (isFrm.ShowDialog() == DialogResult.OK)
                //{
                //    _handler.RestorePrevious();
                //    _handler.InsertShape(isFrm.ShapeType, isFrm.XPosition, isFrm.YPosition, isFrm.ShapeWidth, isFrm.ShapeHeight, isFrm.ShapeColor);
                //    this.Invalidate();
                //}
            }
        }
        private void LaunchGoogleImg(string direction = "", int count = 42, bool icon = false)
        {
            if (_textSearchChanged)
            {
                _webSearchUrls = icon ? Droid.Web.Web.GetIcon(_textSearch) : Droid.Web.Web.GetImages(_textSearch);
                _indexImageWeb = -1;
            }

            for (int i = 0; i < count; i++)
            {
                try
                {
                    if (direction.Equals("back")) _indexImageWeb--;
                    else _indexImageWeb++;

                    if (_indexImageWeb >= _webSearchUrls.Count) { _indexImageWeb = 0; }
                    if (_indexImageWeb < 0) { _indexImageWeb = _webSearchUrls.Count - 1; }

                    string luckyUrl = _webSearchUrls[_indexImageWeb];

                    HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(luckyUrl);
                    webRequest.AllowWriteStreamBuffering = true;
                    webRequest.Timeout = 30000;
                    using (WebResponse webResponse = webRequest.GetResponse())
                    {
                        using (Stream stream = webResponse.GetResponseStream())
                        { 
                            CurrentImage = new Bitmap(stream);
                            if (_currentImage != null)
                            {
                                _pictureByte = imageToByteArray(_currentImage);
                                break;
                            }
                        }
                    }
                }
                catch (WebException wex)
                {
                    count++;
                }
                catch (Exception ex)
                {
                }
            }
            AddImage();
            _textSearchChanged = false;
        }
        private void LaunchFlikrImg()
        {
            // TODO
        }
        private void LaunchRecognition()
        {
            _pictureByte = imageToByteArray(_currentImage);
            List<ParsingPicture.DetectZone> detection = ParsingPicture.ProcessAll(_pictureByte);
            if (detection != null && detection.Count > 0) Console.WriteLine("found");
            foreach (ParsingPicture.DetectZone zone in detection)
            {
                System.DrawingCore.Brush b;
                b.c
                using (Pen pen = new Pen(zone.Color, 1))
                {
                    using (Graphics g = Graphics.FromImage(CurrentImage))
                    {
                        if (zone.Style == ParsingPicture.FormStyle.RECTANGLE)
                        {
                            BuildRectangle(g, zone, pen);
                        }
                        else if (zone.Style == ParsingPicture.FormStyle.ELIPSE)
                        {
                            BuildEllipse(g, zone, pen);
                        }
                        else
                        {
                            Console.WriteLine("No format, that's strange boss !");
                        }
                    }
                }
            }
            if (_stream != null) { _stream.Close(); }
            _picturebox.Image.Save(_stream, CurrentImage.RawFormat);
            AddImage();
        }
        private void LaunchCodeBar()
        {
            parsePictureCode();
        }
        private void LaunchCodeQr()
        {
            parsePictureCode();
        }
        private void LaunchCrop()
        {
            _picturebox.SizeMode = PictureBoxSizeMode.AutoSize;
            DisableMouseScan(); // to avoid multiple event calls
            _flagCrop = !_flagCrop;
            if (_flagCrop) EnableMouseScan();

            _flagSelection = false;
            _flagDrawLine = false;
            _flagDrawShape = false;
        }
        private void LaunchSelecting()
        {
            DisableMouseScan(); // to avoid multiple event calls
            _flagSelection = !_flagSelection;
            if (_flagSelection) EnableMouseScan();

            _flagCrop = false;
            _flagDrawLine = false;
            _flagDrawShape = false;
        }
        private void LaunchDrawLine()
        {
            DisableMouseScan(); // to avoid multiple event calls
            _flagDrawLine = !_flagDrawLine;
            if (_flagDrawLine) EnableMouseScan();

            _flagCrop = false;
            _flagSelection = false;
            _flagDrawShape = false;
        }
        private void LaunchDrawShape()
        {
            DisableMouseScan(); // to avoid multiple event calls
            _flagDrawShape = !_flagDrawShape;
            if (_flagDrawShape) EnableMouseScan();

            _flagCrop = false;
            _flagSelection = false;
            _flagDrawLine = false;
        }
        private void LaunchDiaporama()
        {
            _diaporamaRunning = !_diaporamaRunning;
            if (_diaporamaRunning) { _timer.Start(); }
            else { _timer.Stop(); }
            OnDiaporamaLaunched(_diaporamaRunning, null);
            _tsm.SwitchDiaporama();
        }
        private void LaunchSerializeImage()
        {
            if (_currentImage != null)
            {
                MemoryStream ms = new MemoryStream();
                _currentImage.Save(ms, _currentImage.RawFormat);
                byte[] array = ms.ToArray();
                string imgString = Convert.ToBase64String(array);
                _serialiseString = Convert.ToBase64String(array);
            }
            else
            {
                _serialiseString = string.Empty;
            }
        }
        private void LaunchUnserializeString()
        {
            PathFile = null;
            if (!string.IsNullOrEmpty(_serialiseString))
            {
                if (_serialiseString == null)
                {
                    Log.Write("[ INF : 2702 ] Image string doesn't exist.");
                    _currentImage = null;
                }
                byte[] array = Convert.FromBase64String(_serialiseString);
                Bitmap image = new Bitmap(new MemoryStream(array));
                _currentImage = image;
            }
            else
            {
                _currentImage = null;
            }
        }
        private void LaunchApplyMask()
        {
            if (_currentImage != null && _mask != null)
            {
                Bitmap original = new Bitmap(_currentImage); ;
                _resizeWidth = original.Width;
                _resizeHeight = original.Height;
                _currentImage = _mask;
                LaunchResize();
                Bitmap mask = new Bitmap(_currentImage);

                int width = original.Width;
                int height = original.Height;

                // This is the color that will be replaced in the mask
                Color key = Color.Black;

                // Processing one pixel at a time is slow, but easy to understand
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        // Is this pixel "green" ?
                        if (mask.GetPixel(x, y).R == key.R && mask.GetPixel(x, y).G == key.G && mask.GetPixel(x, y).B == key.B)
                        {
                            // Copy the pixel color from the original
                            Color c = original.GetPixel(x, y);

                            // Into the mask
                            mask.SetPixel(x, y, Color.FromArgb(0, 0, 0, 0));
                            mask.SetPixel(x, y, c);
                        }
                        else
                        {
                            mask.SetPixel(x, y, Color.FromArgb(0, 0, 0, 0));
                        }
                    }
                }
                _currentImage = mask;
            }
        }
        private int LaunchCompare()
        {
            int difference = 0;
            try
            {
                if (_comparisonImage == null)
                {
                    OpenFileDialog ofd = new OpenFileDialog();
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        _comparisonImage = Bitmap.FromFile(ofd.FileName);
                    }
                }
                Bitmap clone = (Bitmap)_currentImage.Clone();
                difference = (int)(ImageTool.GetPercentageDifference(clone, _comparisonImage) * 100);
                _comparisonImage = null;
            }
            catch (Exception exp)
            {
                Log.Write("[ INF : 3712 ] Cannot compare images.\r\nException : " + exp.Message);
            }
            return difference;
        }
        private void LaunchModeEdition()
        {
            _trackBar.Visible = false;
            _picturebox.Height = _sheet.Height;
            _picturebox.Top = 0;

            _currentMode = Mode.EDITION;
            _tsm.SwitchMode();
        }
        private void LaunchModeView()
        {
            _trackBar.Visible = true;
            _picturebox.Height = _sheet.Height - _trackBar.Height;
            _picturebox.Top = _trackBar.Height;

            _currentMode = Mode.VIEW;
            _tsm.SwitchMode();
        }
        private void LaunchModeAnalyse()
        {
            _trackBar.Visible = false;
            _picturebox.Height = _sheet.Height;
            _picturebox.Top = 0;

            _currentMode = Mode.ANALYSE;
            _tsm.SwitchMode();
        }
        #endregion

        #region Methods Private
        private byte[] imageToByteArray(Bitmap imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.DrawingCore.Imaging.ImageFormat.Gif);
            return ms.ToArray();
        }
        private Bitmap byteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Bitmap returnImage = new Bitmap((Stream)ms);
            return returnImage;
        }
        private void EnableMouseScan()
        {
            _picturebox.MouseDown += Picturebox_MouseDown;
        }
        private void DisableMouseScan()
        {
            ButtonValidationDisplay(false);
            _picturebox.MouseDown -= Picturebox_MouseDown;
        }
        private void BuildEllipse(Graphics g, ParsingPicture.DetectZone zone, Pen pen)
        {
            int offset = 76;
            Pen penBis;
            if (zone.Color.R > offset && zone.Color.G > offset && zone.Color.B > offset) penBis = new Pen(Color.FromArgb(100, zone.Color.R - offset, zone.Color.G - offset, zone.Color.B - offset), 2);
            else if (zone.Color.R < (255 - offset) && zone.Color.G < (255 - offset) && zone.Color.B < (255 - offset)) penBis = new Pen(Color.FromArgb(100, zone.Color.R + offset, zone.Color.G + offset, zone.Color.B + offset), 2);
            else penBis = new Pen(Color.FromArgb(100, 180, 180, 180), 2);

            Rectangle rect;
            rect = new Rectangle(zone.Point.X - 1, zone.Point.Y - 1, zone.Width + 2, zone.Height + 2);
            g.DrawEllipse(penBis, rect);
            rect = new Rectangle(zone.Point.X + 1, zone.Point.Y + 1, zone.Width - 2, zone.Height - 2);
            g.DrawEllipse(penBis, rect);
            rect = new Rectangle(zone.Point.X, zone.Point.Y, zone.Width, zone.Height);
            g.DrawEllipse(pen, rect);
        }
        private void BuildRectangle(Graphics g, ParsingPicture.DetectZone zone, Pen pen)
        {
            g.DrawRectangle(pen, new Rectangle(
                zone.Point.X,
                zone.Point.Y,
                zone.Width,
                zone.Height
            ));
        }
        private void ProcessOpenned()
        {
            AddImage();
            BuildToolBar();
            //if (_panelTools == null) AddToolsPanel();
            //else _panelTools.Refresh();
            //LaunchZoomImage();
            _openned = true;
        }
        private int determineNextPictureIndex(string[] fichiers, int curindex)
        {
            bool notfound = true;
            string[] ext = { "png", "jpg", "bmp", "gif" };
            while (notfound)
            {
                curindex = curindex + 1;
                if (curindex == fichiers.Length) curindex = 0;
                if (fichiers[curindex].Split('.').Length > 0)
                {
                    foreach (string se in ext)
                    {
                        if (fichiers[curindex].ToLower().EndsWith(se))
                        {
                            notfound = false;
                            break;
                        }
                    }
                }
            }
            return curindex;
        }
        private int determinePrevPictureIndex(string[] fichiers, int curindex)
        {
            bool notfound = true;
            string[] ext = { "png", "jpg", "bmp", "gif" };
            while (notfound)
            {
                curindex = curindex - 1;
                if (curindex == 0) curindex = fichiers.Length - 1;
                if (fichiers[curindex].Split('.').Length > 0)
                {
                    foreach (string se in ext)
                    {
                        if (fichiers[curindex].ToLower().EndsWith(se))
                        {
                            notfound = false;
                            break;
                        }
                    }
                }
            }
            return curindex;
        }
        private Rotation GetPictureOrientation()
        {
            string val = string.Empty;
            if (CurrentImage != null)
            { 
                foreach (var prop in CurrentImage.PropertyItems)
                {
                    if (prop.Id == 0x112)
                    {
                        if (prop.Value[0] == 6) { return Rotation.RIGHT; }
                        else if (prop.Value[0] == 8) { return Rotation.LEFT; }
                        else if (prop.Value[0] == 3) { return Rotation.HALF; }
                        else { return Rotation.NONE; }
                    }
                }
            }
            return Rotation.NONE;
        }
        private void AddImage()
        {
            BuildSheet();
            try
            {
                if (CurrentImage != null)
                {
                    _handler = new ImageHandler();
                    _handler.CurrentBitmap = new Bitmap(CurrentImage);
                    _tsm.EnableAll();
                }
                else
                {
                    _handler = null;
                    _tsm.DisableAll();
                }
            }
            catch (Exception exp3700)
            {
                _tsm.DisableAll();
                Log.Write("[ DEB : 3700 ]\n" + exp3700.Message);
            }

            _picturebox.Image = CurrentImage;
            switch (GetPictureOrientation())
            {
                case Rotation.LEFT:
                    LaunchRotationL();
                    break;
                case Rotation.RIGHT:
                    LaunchRotationR();
                    break;
                case Rotation.HALF:
                    LaunchRotate180();
                    break;
            }
        }
        private void BuildSheet()
        {
            if (_sheet != null)
            {
                try
                {
                    //if (picturebox.SizeMode == PictureBoxSizeMode.Zoom) this.ImageViewMode.Equals("zoom");
                    //if (picturebox.SizeMode == PictureBoxSizeMode.StretchImage) this.ImageViewMode.Equals("stretch");
                    //if (picturebox.SizeMode == PictureBoxSizeMode.CenterImage) this.ImageViewMode.Equals("center");
                    //_sheet.Dispose();
                }
                catch (Exception exp3702)
                {
                    Log.Write("[ ERR : 3702 ] Error while closing the old picture panel \n" + exp3702.Message);
                }
            }
            else
            {
                _sheet = new PanelScrollableCustom();
                _sheet.AutoScroll = true;
                _sheet.BorderStyle = BorderStyle.None;
                _sheet.AutoScroll = true;
                _sheet.Controls.Add(_picturebox);
                _sheet.BackColor = Color.Pink;

                _trackBar = new SliderTrackBar();
                _trackBar.BackColor = Color.Transparent;
                _trackBar.EmptyTrackColor = Color.Black;
                _trackBar.AllowUserValueChange = false;
                _trackBar.AllowMouseWheelChange = false;
                _trackBar.AllowDrop = false;
                _trackBar.Height = 5;
                _trackBar.Width = _sheet.Width + 10;
                _trackBar.Left = -5;
                _trackBar.Top = 0;
                _trackBar.ScaleType = SliderTrackBar.SliderTrackBarScaleType.None;
                _trackBar.SliderButtonSize = new Size(14, 7);
                _trackBar.ShowSlider = SliderTrackBar.SliderTrackBarShowSlider.Never;
                _trackBar.TrackLowerColor = Color.FromName("SaddleBrown");
                _trackBar.TrackUpperColor = Color.FromName("Orange");
                _trackBar.UseSeeking = false;
                _trackBar.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right))));
                _sheet.Controls.Add(_trackBar);

                _picturebox = new PictureBox();
                _picturebox.BackColor = Color.DimGray;
                _picturebox.BorderStyle = BorderStyle.None;
                _picturebox.SizeMode = PictureBoxSizeMode.Zoom;
                _picturebox.Image = CurrentImage;
                _picturebox.Top = 0;
                _picturebox.Left = 0;
                _picturebox.Width = _sheet.Width;
                _picturebox.Height = _sheet.Height - _trackBar.Height;
                _picturebox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right) | System.Windows.Forms.AnchorStyles.Bottom)));
                _sheet.Controls.Add(_picturebox);

                _buttonValidation = new Button();
                _buttonValidation.Image = Tools.Utilities.Resources.ResourceIconSet32Default.accept;
                _buttonValidation.BackColor = Color.Transparent;
                _buttonValidation.UseVisualStyleBackColor = false;
                _buttonValidation.FlatAppearance.BorderSize = 0;
                _buttonValidation.FlatStyle = FlatStyle.Flat;
                _buttonValidation.FlatAppearance.MouseDownBackColor = Color.Transparent;
                _buttonValidation.FlatAppearance.MouseOverBackColor = Color.Transparent;
                _buttonValidation.Width = 32;
                _buttonValidation.Height = 32;
                _buttonValidation.MouseHover += _buttonValidation_MouseHover;
                _buttonValidation.MouseLeave += _buttonValidation_MouseLeave;
                _buttonValidation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Top)));
                _buttonValidation.Click += _buttonValidation_Click;
                _buttonValidation.Visible = false;

                _timer = new Timer();
                _timer.Interval = 5000;
                _timer.Elapsed += _timer_Tick;
            }
        }

        private void AddToolsPanel()
        {
            //buildpaneltools();
            ////buildpictureboxmini();
            //buildtrackbar();
            //buildtracklabel();

            ////panelTools.Controls.Add(pictureboxmini);
            //_panelTools.Controls.Add(_trackbar);
            //_panelTools.Controls.Add(_tracklabel);
            //         _sheet.Controls.Add(_panelTools);
            //         //_tsm.CurrentTabPage.Controls.Add(_panelTools);
        }
        private static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }
        private static Bitmap cropImage(Bitmap img, Rectangle cropArea)
        {
            Bitmap bmpImage = new Bitmap(img);
            Bitmap bmpCrop = bmpImage.Clone(cropArea,
                                            bmpImage.PixelFormat);
            return bmpCrop;
        }
        private static Bitmap resizeImage(Bitmap imgToResize, Size size)
        {
            int sourceWidth = imgToResize.Width;
            int sourceHeight = imgToResize.Height;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)size.Width / (float)sourceWidth);
            nPercentH = ((float)size.Height / (float)sourceHeight);

            if (nPercentH < nPercentW)
                nPercent = nPercentH;
            else
                nPercent = nPercentW;

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage(b);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();

            return b;
        }
        private void RotateImage(float angle)
        {
            if (_picturebox.Image == null)
                return;

            Bitmap oldImage = _picturebox.Image;
            Bitmap newImage = RotateImage(_picturebox.Image, angle);

            _picturebox.Image = newImage;
            _picturebox.Refresh();

            if (oldImage != null)
            {
                oldImage.Dispose();
            }
        }
        private static Bitmap RotateImage(Bitmap image, float angle)
        {
            if (image == null) Log.Write("[ERR : 0300] No Image found !");

            PointF offset = new PointF((float)image.Width / 2, (float)image.Height / 2);

            //create a new empty bitmap to hold rotated image
            Bitmap rotatedBmp = new Bitmap(image.Width, image.Height); ;
            rotatedBmp.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            //make a graphics object from the empty bitmap
            Graphics g = Graphics.FromImage(rotatedBmp);

            //Put the rotation point in the center of the image
            g.TranslateTransform(offset.X, offset.Y);

            //rotate the image
            g.RotateTransform(angle);

            //move the image back
            g.TranslateTransform(-offset.X, -offset.Y);

            //draw passed in image onto graphics object
            g.DrawImage(image, new PointF(0, 0));

            return rotatedBmp;
        }
        private void ResizeAndDisplayImage()
        {
            // Set the backcolor of the pictureboxes
            _picturebox.BackColor = _backColor;
            //pictureboxmini.BackColor = BackColor;

            // sourceWidth and sourceHeight store the original image's width and height
            // targetWidth and targetHeight are calculated to fit into the picImage picturebox.
            int sourceWidth = _originalImage.Width;
            int sourceHeight = _originalImage.Height;
            int targetWidth;
            int targetHeight;
            double ratio;

            // Calculate targetWidth and targetHeight, so that the image will fit into
            // the picImage picturebox without changing the proportions of the image.
            if (sourceWidth > sourceHeight)
            {
                // Set the new width
                targetWidth = _picturebox.Width;
                // Calculate the ratio of the new width against the original width
                ratio = (double)targetWidth / sourceWidth;
                // Calculate a new height that is in proportion with the original image
                targetHeight = (int)(ratio * sourceHeight);
            }
            else if (sourceWidth < sourceHeight)
            {
                // Set the new height
                targetHeight = _picturebox.Height;
                // Calculate the ratio of the new height against the original height
                ratio = (double)targetHeight / sourceHeight;
                // Calculate a new width that is in proportion with the original image
                targetWidth = (int)(ratio * sourceWidth);
            }
            else
            {
                // In this case, the image is square and resizing is easy
                targetHeight = _picturebox.Height;
                targetWidth = _picturebox.Width;
            }

            // Calculate the targetTop and targetLeft values, to center the image
            // horizontally or vertically if needed
            int targetTop = (_picturebox.Height - targetHeight) / 2;
            int targetLeft = (_picturebox.Width - targetWidth) / 2;

            // Create a new temporary bitmap to resize the original image
            // The size of this bitmap is the size of the picImage picturebox.
            Bitmap tempBitmap = new Bitmap((int)_picturebox.Width, (int)_picturebox.Height, PixelFormat.Format24bppRgb);

            // Set the resolution of the bitmap to match the original resolution.
            tempBitmap.SetResolution(_originalImage.HorizontalResolution, _originalImage.VerticalResolution);

            // Create a Graphics object to further edit the temporary bitmap
            Graphics bmGraphics = Graphics.FromImage(tempBitmap);

            // First clear the image with the current backcolor
            bmGraphics.Clear(_backColor);

            // Set the interpolationmode since we are resizing an image here
            bmGraphics.InterpolationMode = InterpolationMode.HighQualityBicubic;

            // Draw the original image on the temporary bitmap, resizing it using
            // the calculated values of targetWidth and targetHeight.
            bmGraphics.DrawImage(_originalImage,
                                 new Rectangle(targetLeft, targetTop, targetWidth, targetHeight),
                                 new Rectangle(0, 0, sourceWidth, sourceHeight),
                                 GraphicsUnit.Pixel);

            // Dispose of the bmGraphics object
            bmGraphics.Dispose();

            // Set the image of the picImage picturebox to the temporary bitmap
            _picturebox.Image = tempBitmap;
        }
        private void Crop(int startX, int startY, int widthImg, int heightImg)
        {
            if (_handler != null)
            {
                _handler.RestorePrevious();
                _handler.DrawOutCropArea(startX, startY, widthImg, heightImg);
                _handler.Crop(startX, startY, widthImg, heightImg);
                CurrentImage = _handler.CurrentBitmap;
                _picturebox.Image = CurrentImage;
                _picturebox.Invalidate();
            }
        }
        
        #region Build ToolPanel
        private void buildpictureboxmini()
        {
            //pictureboxmini = new PictureBox();
            //pictureboxmini.Width = 100;
            //pictureboxmini.Height = 100;
            //pictureboxmini.Top = 10;
            //pictureboxmini.Left = 10;
            //pictureboxmini.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            //pictureboxmini.Visible = false;
        }
        private void buildtrackbar()
        {
            _trackbar = new TrackBar();
            _trackbar.LargeChange = 1;
            _trackbar.BackColor = Color.WhiteSmoke;
            _trackbar.Maximum = 6;
            _trackbar.Minimum = 2;
            _trackbar.Name = "trackbar";
            _trackbar.Size = new Size(100, 20);
            _trackbar.Value = 3;
            _trackbar.Top = 120;
            _trackbar.Left = 0;
            _trackbar.ValueChanged += new System.EventHandler(this.trackbar_ValueChanged);
        }
        private void buildtracklabel()
        {
            _tracklabel = new Label();
            _tracklabel.BackColor = Color.WhiteSmoke;
            _tracklabel.Name = "tracklabel";
            _tracklabel.Size = new Size(18, 13);
            _tracklabel.TabIndex = 4;
            _tracklabel.Text = "x3";
            _tracklabel.Top = 125;
            _tracklabel.Left = 100;
        }
        private void buildpaneltools()
        {
            _panelTools = new Panel();
            _panelTools.Width = 200;
            _panelTools.Top = 0;
            _panelTools.Width = 120;
            _panelTools.BackColor = Color.WhiteSmoke;
        }
        private async void parsePictureCode()
        {
            try
            {
                ZXing.MultiFormatReader reader = new ZXing.MultiFormatReader();
                Hashtable hints = new Hashtable();
                ArrayList fmts = new ArrayList();
                fmts.Add(BarcodeFormat.DATA_MATRIX);
                fmts.Add(BarcodeFormat.QR_CODE);
                fmts.Add(BarcodeFormat.PDF_417);
                fmts.Add(BarcodeFormat.UPC_E);
                fmts.Add(BarcodeFormat.UPC_A);
                fmts.Add(BarcodeFormat.CODE_128);
                fmts.Add(BarcodeFormat.CODE_39);
                fmts.Add(BarcodeFormat.ITF);
                fmts.Add(BarcodeFormat.EAN_8);
                fmts.Add(BarcodeFormat.EAN_13);
                hints.Add(DecodeHintType.TRY_HARDER, true);
                hints.Add(DecodeHintType.POSSIBLE_FORMATS, fmts);
                reader.Hints = hints;

                RGBLuminanceSource lumi = new RGBLuminanceSource(CurrentImage, CurrentImage.Width, CurrentImage.Height);

                Result result = new Result(string.Empty, null, null, BarcodeFormat.QR_CODE);
                OnMessageAvailable(MessageDisplay.Message.ANALYSING, null);
                await Task.Run(() => { result = reader.decode(new BinaryBitmap(new HybridBinarizer(lumi)), hints); });

                if (!string.IsNullOrEmpty(result.Text)) { OnMessageAvailable(result.Text.ToString(), null); }
            }
            catch (Exception exp)
            {
                OnMessageAvailable(MessageDisplay.Message.FAILED, null);
                Console.WriteLine(exp.Message);
            }
        }
        #endregion

        private static string GetHtmlCode(string topic)
        {
            var rnd = new Random();

            string url = "https://www.google.com/search?q=" + topic + "&tbm=isch";
            string data = "";

            var request = (HttpWebRequest)WebRequest.Create(url);
            var response = (HttpWebResponse)request.GetResponse();

            using (Stream dataStream = response.GetResponseStream())
            {
                if (dataStream == null)
                    return "";
                using (var sr = new StreamReader(dataStream))
                {
                    data = sr.ReadToEnd();
                }
            }
            return data;
        }
        private static List<string> GetUrls(string html)
        {
            var urls = new List<string>();
            //int ndx = html.IndexOf("class=\"images_table\"", StringComparison.Ordinal);
            int ndx = html.IndexOf("<img", StringComparison.Ordinal);

            while (ndx >= 0)
            {
                ndx = html.IndexOf("src=\"", ndx, StringComparison.Ordinal);
                ndx = ndx + 5;
                int ndx2 = html.IndexOf("\"", ndx, StringComparison.Ordinal);
                string url = html.Substring(ndx, ndx2 - ndx);
                urls.Add(url);
                ndx = html.IndexOf("<img", ndx, StringComparison.Ordinal);
            }
            return urls;
        }
        private static byte[] GetImage(string url)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            var response = (HttpWebResponse)request.GetResponse();

            using (Stream dataStream = response.GetResponseStream())
            {
                if (dataStream == null)
                    return null;
                using (var sr = new BinaryReader(dataStream))
                {
                    byte[] bytes = sr.ReadBytes(100000);

                    return bytes;
                }
            }
        }
        private void MouseMove()
        {
            if (_mouseDown)
            {
                if (_flagCrop) { DrawSelection(); }
                else if (_flagSelection) { DrawSelection(); }
            }
        }
        private void MouseEnd()
        {
            if (!_mouseDown)
            {
                _picturebox.MouseMove -= Picturebox_MouseMove;
                _picturebox.MouseUp -= Picturebox_MouseUp;
                _picturebox.MouseLeave -= Picturebox_MouseLeave;
                if (_flagCrop)
                {
                    ButtonValidationDisplay(true);
                }
                else if (_flagSelection)
                {

                }
            }
        }
        private void ButtonValidationDisplay(bool enable)
        {
            if (enable)
            {
                _buttonValidation.Visible = true;
                _picturebox.Controls.Add(_buttonValidation);
            }
            else
            {
                _buttonValidation.Visible = false;
                _picturebox.Controls.Remove(_buttonValidation);
            }
        }
        private void DrawSelection()
        {
            if (_panelSelection != null)
            {
                bool changed = false;

                if (_initMouseX > _currentMouseX)
                {
                    if (_panelSelection.Left != _currentMouseX || _panelSelection.Width != _initMouseX - _currentMouseX) changed = true;
                    _panelSelection.Left = _currentMouseX;
                    _panelSelection.Width = _initMouseX - _currentMouseX;
                }
                else
                {
                    if (_panelSelection.Left != _initMouseX || _panelSelection.Width != _currentMouseX - _initMouseX) changed = true;
                    _panelSelection.Left = _initMouseX;
                    _panelSelection.Width = _currentMouseX - _initMouseX;
                }

                if (_initMouseY > _currentMouseY)
                {
                    if (_panelSelection.Top != _currentMouseY || _panelSelection.Height != _initMouseY - _currentMouseY) changed = true;
                    _panelSelection.Top = _currentMouseY;
                    _panelSelection.Height = _initMouseY - _currentMouseY;
                }
                else
                {
                    if (_panelSelection.Top != _initMouseY || _panelSelection.Height != _currentMouseY - _initMouseY) changed = true;
                    _panelSelection.Top = _initMouseY;
                    _panelSelection.Height = _currentMouseY - _initMouseY;
                }

                if (_panelSelection.Top < 0) _panelSelection.Top = 0;
                if (_panelSelection.Left < 0) _panelSelection.Left = 0;
                if (_panelSelection.Top + _panelSelection.Height > _picturebox.Image.Height) _panelSelection.Height = _picturebox.Image.Height - _panelSelection.Top;
                if (_panelSelection.Left + _panelSelection.Width > _picturebox.Image.Width) _panelSelection.Width = _picturebox.Image.Width - _panelSelection.Left;

                if (changed)
                {
                    _panelSelection.Invalidate();
                    _picturebox.Invalidate();
                    if (_sheet != null) _sheet.Invalidate();
                }
                if (_sheet != null) _sheet.ResumeLayout();
            }

        }
        private void ValidationDone()
        {
            if (_flagCrop)
            {
                Point p1 = OffsetToImage(_picturebox, new Point(_initMouseX, _initMouseY));
                Point p2 = OffsetToImage(_picturebox, new Point(_currentMouseX - _initMouseX, _currentMouseY - _initMouseY));
                //Crop(_initMouseX, _initMouseY, _currentMouseX - _initMouseX, _currentMouseY - _initMouseY);
                Crop(p1.X, p1.Y, _currentMouseX - _initMouseX, _currentMouseY - _initMouseY);
                ButtonValidationDisplay(false);
                _panelSelection.Dispose();
            }
            else if (_flagSelection)
            {

            }
        }
        private static Point OffsetToImage(PictureBox pbox, Point p)
        {
            // Calculer les taux d'étirement/compression de l'image 
            double xRatio = 1;
            double yRatio = 1;
            if (pbox.SizeMode == PictureBoxSizeMode.StretchImage)
            {
                xRatio = (double)((pbox.Width * 1000) / pbox.Image.Width) / 1000;
                yRatio = (double)((pbox.Height * 1000) / pbox.Image.Height) / 1000;
            }
            else if (pbox.SizeMode == PictureBoxSizeMode.Zoom)
            {
                xRatio = pbox.Width > pbox.Height ? (double)((pbox.Height * 1000) / pbox.Image.Height) / 1000 : 1;
                yRatio = pbox.Width > pbox.Height ? 1 : (double)((pbox.Width * 1000) / pbox.Image.Width) / 1000;
            }

            // Calculer la taille de l'image affichée 
            Size imgSize = new Size((int)(pbox.Image.Width * xRatio),
            (int)(pbox.Image.Height * yRatio));

            // Déterminer la position du coin supérieur droit de l'image 
            // par rapport au coin supérieur droit du picture box. 
            // En mode Normal ou AutoSize, c'est (0,0). 
            Point imgPosition = new Point();
            if (pbox.SizeMode == PictureBoxSizeMode.CenterImage || pbox.SizeMode == PictureBoxSizeMode.StretchImage || pbox.SizeMode == PictureBoxSizeMode.Zoom)
            {
                // Dans ces trois modes, l'image est centrée dans le PB. 
                imgPosition.X = pbox.Width / 2 - imgSize.Width / 2;
                imgPosition.Y = pbox.Height / 2 - imgSize.Height / 2;
            }

            // Transformer les coordonnées fournies pour les rendre 
            // relatives au coin supérieur droit de l'image 
            Point result = new Point(p.X - imgPosition.X, p.Y - imgPosition.Y);

            // Appliquer les taux d'étirement/compression 
            result.X = (int)(result.X / xRatio);
            result.Y = (int)(result.Y / yRatio);

            // Enfin, comme en mode Normal, Center et Stretch l'image n'occupe 
            // pas forcément la totalité de la zone cliente du picture box, il faut 
            // vérifier que le point n'est pas en dehors de l'image. S'il est, on le 
            // "ramène" de force vers le bord de l'image le plus proche. 
            if (result.X < 0) result.X = 0;
            else if (result.X >= pbox.Image.Width) result.X = pbox.Image.Width - 1;

            if (result.Y < 0) result.Y = 0;
            else if (result.Y >= pbox.Image.Height) result.Y = pbox.Image.Height - 1;

            return result;
        }
        #endregion

        #region Event
        private void btnSelectColor_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();

            colorDialog.AllowFullOpen = true;
            colorDialog.AnyColor = true;
            colorDialog.Color = _backColor;
            colorDialog.FullOpen = true;
            colorDialog.ShowHelp = true;
            colorDialog.SolidColorOnly = false;

            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                _backColor = colorDialog.Color;
                ResizeAndDisplayImage();
            }

            colorDialog.Dispose();
        }
        private void trackbar_ValueChanged(object sender, EventArgs e)
        {
            _zoomFactor = _trackbar.Value;
            _tracklabel.Text = string.Format("x{0}", _zoomFactor);
        }
        private void Picturebox_MouseUp(object sender, MouseEventArgs e)
        {
            _mouseDown = false;
            _currentMouseX = e.X;
            _currentMouseY = e.Y;
            MouseEnd();
        }
        private void Picturebox_MouseDown(object sender, MouseEventArgs e)
        {
            _picturebox.MouseUp += Picturebox_MouseUp;
            _picturebox.MouseLeave += Picturebox_MouseLeave;
            _picturebox.MouseMove += Picturebox_MouseMove;
            if (_panelSelection != null) _panelSelection.Dispose();
            _panelSelection = new Panel();
            _panelSelection.Width = 1;
            _panelSelection.Height = 1;
            _panelSelection.Top = e.Y;
            _panelSelection.Left = e.X;
            _panelSelection.BackColor = Color.FromArgb(100, 200, 200, 200);
            _panelSelection.BorderStyle = BorderStyle.FixedSingle;
            _picturebox.Controls.Add(_panelSelection);

            _initMouseX = e.X;
            _initMouseY = e.Y;
            _mouseDown = true;
        }
        private void Picturebox_MouseMove(object sender, MouseEventArgs e)
        {
            _currentMouseX = e.X;
            _currentMouseY = e.Y;
            MouseMove();
        }
        private void Picturebox_MouseLeave(object sender, EventArgs e)
        {
            _mouseDown = false;
            _currentMouseX = Cursor.Position.X;
            _currentMouseY = Cursor.Position.Y;
            MouseEnd();
        }
        private void _buttonValidation_Click(object sender, EventArgs e)
        {
            ValidationDone();
        }
        private void _buttonValidation_MouseLeave(object sender, EventArgs e)
        {
            _buttonValidation.Image = Tools.Utilities.Resources.ResourceIconSet32Default.accept;
        }
        private void _buttonValidation_MouseHover(object sender, EventArgs e)
        {
            _buttonValidation.Image = Tools.Utilities.Resources.ResourceIconSet32Default.accept;
        }
        private void _timer_Tick(object sender, EventArgs e)
        {
            LaunchNext();
            _picturebox.Invalidate();
        }
        #endregion
    }
}


