//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection;
//using System.Text;

//namespace Zambon.Core.Module.Services
//{
//    public class CurrentAssemblyService
//    {

//        #region Properties

//        public string AppName { get; private set; }

//        public string AppVersion { get; private set; }

//        public string Copyright { get; private set; }

//        public string Company { get; private set; }

//        #endregion

//        #region Constructors

//        public CurrentAssemblyService()
//        {
//            LoadData();
//        }

//        #endregion

//        #region Methods

//        private void LoadData()
//        {
//            var assembly = Assembly.GetEntryAssembly();

//            var appName = assembly.GetCustomAttributes(typeof(AssemblyProductAttribute)).FirstOrDefault() as AssemblyProductAttribute;
//            var appVersion = assembly.GetCustomAttributes(typeof(AssemblyInformationalVersionAttribute)).FirstOrDefault() as AssemblyInformationalVersionAttribute;
//            var copyright = assembly.GetCustomAttributes(typeof(AssemblyCopyrightAttribute)).FirstOrDefault() as AssemblyCopyrightAttribute;
//            var company = assembly.GetCustomAttributes(typeof(AssemblyCompanyAttribute)).FirstOrDefault() as AssemblyCompanyAttribute;

//            AppName = appName.Product;
//            AppVersion = appVersion.InformationalVersion;
//            Copyright = copyright.Copyright;
//            Company = company.Company;
//        }

//        #endregion

//    }
//}