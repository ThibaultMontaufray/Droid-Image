using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Threading.Tasks;

namespace Droid.Image
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public const string DESCRIPTION = "Service for image manage by Droid.";
        public const string DIPSLAYNAME = "Droid image service";
        public const string SERVICENAME = "Droid.image";
        public ProjectInstaller()
        {
            InitializeComponent();
        }
        protected override void OnBeforeInstall(IDictionary savedState)
        {
            string parameter = string.Format("\"{0}_source\" \"{0}_logs\"", SERVICENAME);
            Context.Parameters["assemblypath"] = "\"" + Context.Parameters["assemblypath"] + "\" \"" + parameter + "\""; base.OnBeforeInstall(savedState);
        }
    }
}
