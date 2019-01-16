using System;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Runtime.InteropServices;

namespace Droid.Image
{
    public partial class Service : ServiceBase
    {
        #region Enum
        public enum ServiceState
        {
            SERVICE_STOPPED = 0x00000001,
            SERVICE_START_PENDING = 0x00000002,
            SERVICE_STOP_PENDING = 0x00000003,
            SERVICE_RUNNING = 0x00000004,
            SERVICE_CONTINUE_PENDING = 0x00000005,
            SERVICE_PAUSE_PENDING = 0x00000006,
            SERVICE_PAUSED = 0x00000007,
        }
        public enum ServiceAction
        {
            ACTION_130 = 130,
            ACTION_131 = 131,
            ACTION_132 = 132,
            ACTION_133 = 133,
            ACTION_134 = 134,
            ACTION_135 = 135,
            ACTION_136 = 136,
            ACTION_137 = 137,
            ACTION_138 = 138
        }
        #endregion

        #region Struct
        [StructLayout(LayoutKind.Sequential)]
        public struct ServiceStatus
        {
            public long dwServiceType;
            public ServiceState dwCurrentState;
            public long dwControlsAccepted;
            public long dwWin32ExitCode;
            public long dwServiceSpecificExitCode;
            public long dwCheckPoint;
            public long dwWaitHint;
        };
        #endregion

        #region Attribute
        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool SetServiceStatus(IntPtr handle, ref ServiceStatus serviceStatus);

        private const string SERVICENAME = "Droid.image";
        private int eventId;
        #endregion

        #region Constructor
        public Service(string[] args)
        {
            InitializeComponent();
            string eventSourceName = string.Format("{0}_source", SERVICENAME);
            string logName = string.Format("{0}_logs", SERVICENAME);
            if (args != null && args.Count() > 0)
            {
                eventSourceName = args[0];
            }
            if (args != null && args.Count() > 1) { logName = args[1]; }
            _eventLog = new System.Diagnostics.EventLog();
            if (!System.Diagnostics.EventLog.SourceExists(eventSourceName))
            {
                System.Diagnostics.EventLog.CreateEventSource(eventSourceName, logName);
            }
            _eventLog.Source = eventSourceName;
            _eventLog.Log = logName;
        }
        #endregion

        #region Methods public
        public void OnTimer(object sender, System.Timers.ElapsedEventArgs args)
        {
            // TODO: Insert monitoring activities here.
            _eventLog.WriteEntry(string.Format("{0} current uptime : {1} minutes", SERVICENAME, eventId.ToString()), EventLogEntryType.Information, eventId++);
        }
        #endregion

        #region Methods protected
        protected override void OnStart(string[] args)
        {
            // Update the service state to Start Pending.
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_START_PENDING;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

            eventId = 0;
            _eventLog.WriteEntry("In OnStart");
            // Set up a timer to trigger every minute.
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 60000; // 60 seconds
            timer.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimer);
            timer.Start();

            // Update the service state to Running.
            serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
        }
        protected override void OnStop()
        {
            _eventLog.WriteEntry("In onStop.");
        }
        protected override void OnContinue()
        {
            _eventLog.WriteEntry("In OnContinue.");
        }
        protected override void OnCustomCommand(int command)
        {
            try
            {
                _eventLog.WriteEntry("Command code : " + command, EventLogEntryType.SuccessAudit, eventId++);

                base.OnCustomCommand(command);
                switch (command)
                {
                    case (int)ServiceAction.ACTION_130:
                        _eventLog.WriteEntry("ACTION_130_take_picture");
                        Interface_image.ACTION_130_take_picture(null);
                        break;
                    case (int)ServiceAction.ACTION_131:
                        _eventLog.WriteEntry("ACTION_131_crop_picture");
                        Interface_image.ACTION_131_crop_picture(null, 0, 0, 0, 0);
                        break;
                    case (int)ServiceAction.ACTION_132:
                        _eventLog.WriteEntry("ACTION_132_resize_picture");
                        Interface_image.ACTION_132_resize_picture(null, 0, 0);
                        break;
                    case (int)ServiceAction.ACTION_133:
                        _eventLog.WriteEntry("ACTION_133_flip_vertical");
                        Interface_image.ACTION_133_flip_vertical(null);
                        break;
                    case (int)ServiceAction.ACTION_134:
                        _eventLog.WriteEntry("ACTION_134_flip_horizontal");
                        Interface_image.ACTION_134_flip_horizontal(null);
                        break;
                    case (int)ServiceAction.ACTION_135:
                        _eventLog.WriteEntry("ACTION_135_research_web");
                        Interface_image.ACTION_135_research_web(null);
                        break;
                    case (int)ServiceAction.ACTION_136:
                        _eventLog.WriteEntry("ACTION_136_serialize_image");
                        Interface_image.ACTION_136_serialize_image(null);
                        break;
                    case (int)ServiceAction.ACTION_137:
                        _eventLog.WriteEntry("ACTION_137_unserialize_image");
                        Interface_image.ACTION_137_unserialize_image(null);
                        break;
                    case (int)ServiceAction.ACTION_138:
                        _eventLog.WriteEntry("ACTION_138_apply_mask");
                        Interface_image.ACTION_138_apply_mask(null);
                        break;
                    default:
                        _eventLog.WriteEntry("Default action : " + command);
                        break;
                }
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.Message);
            }
        }
        #endregion
    }
}
