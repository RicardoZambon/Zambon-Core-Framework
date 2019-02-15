using Microsoft.Extensions.Options;
using Zambon.Core.Database;
using Zambon.Core.Module.Operations;
using Zambon.Core.Module.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace Zambon.Core.Module.Services
{
    public class ModelService
    {

        #region Variables

        private readonly string ResourceName;

        private readonly CoreContext _ctx;

        private readonly IOptions<AppSettings> _appConfigs;

        #endregion

        #region Properties

        private string AppVersion { get; set; }

        private string AppCopyright { get; set; }

        private Application _Model;
        private Application Model { get { if (_Model == null) LoadModel(); return _Model; } }

        #endregion

        #region Constructors

        public ModelService(CoreContext ctx, IOptions<AppSettings> appConfigs)
        {
            ResourceName = "ApplicationModel.xml";
            _ctx = ctx;
            _appConfigs = appConfigs;
        }

        public ModelService(string _resourceName, CoreContext ctx, IOptions<AppSettings> appConfigs) : this(ctx, appConfigs)
        {
            ResourceName = _resourceName;
        }

        #endregion

        #region Methods

        private void LoadModel()
        {
            var assembly = Assembly.GetEntryAssembly();

            System.IO.Stream stream;

            //Read from the WebModule
            Application application = null;
            stream = Assembly.Load("Zambon.Core.WebModule").GetManifestResourceStream(string.Format("Zambon.Core.WebModule.{0}", ResourceName));
            if (stream != null)
                using (stream)
                {
                    var serializer = new XmlSerializer(typeof(Application));
                    application = (Application)serializer.Deserialize(stream);
                }

            //Read from the Application
            var path = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(assembly.Location), ResourceName);
            if (System.IO.File.Exists(path))
                stream = System.IO.File.Open(path, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            else
                stream = assembly.GetManifestResourceStream(string.Format("{0}.{1}", assembly.GetName().Name, ResourceName));

            if (stream != null)
                using (stream)
                {
                    var serializer = new XmlSerializer(typeof(Application));

                    if (application != null)
                        application = Merge.MergeObject(application, (Application)serializer.Deserialize(stream));
                    else
                        application = (Application)serializer.Deserialize(stream);
                }

            application.OnLoading(application, _ctx);

            _Model = application;
        }


        internal string GetAppVersion()
        {
            if (string.IsNullOrWhiteSpace(AppVersion))
            {
                var assembly = Assembly.GetEntryAssembly();
                AppVersion = (assembly.GetCustomAttributes(typeof(AssemblyInformationalVersionAttribute)).FirstOrDefault() as AssemblyInformationalVersionAttribute).InformationalVersion;
            }
            return AppVersion;
        }

        internal string GetAppCopyright()
        {
            if (string.IsNullOrWhiteSpace(AppCopyright))
            {
                var assembly = Assembly.GetEntryAssembly();
                AppCopyright = (assembly.GetCustomAttributes(typeof(AssemblyCopyrightAttribute)).FirstOrDefault() as AssemblyCopyrightAttribute).Copyright;
            }
            return AppCopyright;
        }

        internal AppSettings GetAppSettings()
        {
            return _appConfigs.Value;
        }
        

        public Application GetModel()
        {
            if (Model == null)
                LoadModel();
            return Model;
            //return (Application)Model.Clone();
        }

        public string GetPropertyDisplayName(string typeClr, string name)
        {
            if ((Model?.EntityTypes?.Entities?.Length ?? 0) > 0)
            {
                //var entity = Array.Find(Model.EntityTypes.Entities, e => e.TypeClr == typeClr);
                var entity = Array.Find(Model.EntityTypes.Entities, e => e.Id == typeClr);
                if ((entity?.Properties?.Property?.Length ?? 0) > 0)
                {
                    var property = Array.Find(entity.Properties.Property, p => p.Name == name);
                    return property?.DisplayName ?? string.Empty;
                }
            }
            return string.Empty;
        }

        #endregion

    }
}
