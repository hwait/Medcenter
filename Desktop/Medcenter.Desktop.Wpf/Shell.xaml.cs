using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Medcenter.Desktop.Infrastructure;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.ServiceLocation;

namespace Medcenter.Desktop.Wpf
{
    /// <summary>
    /// Interaction logic for Shell.xaml
    /// </summary>
    [Export]
    public partial class Shell : Window, IPartImportsSatisfiedNotification
    {
        private const string LoginModuleName = "LoginModule";
        private static readonly Uri LoginViewUri = new Uri("/LoginFormView", UriKind.Relative);
     
        public Shell()
        {
            InitializeComponent();
        }
        

        [Import(AllowRecomposition = false)]
        public IModuleManager ModuleManager;

        [Import(AllowRecomposition = false)]
        public IRegionManager RegionManager;

        public void OnImportsSatisfied()
        {
            this.ModuleManager.LoadModuleCompleted += ModuleManagerLoadModuleCompleted;
        }

        private void ModuleManagerLoadModuleCompleted(object sender, LoadModuleCompletedEventArgs e)
        {
            if (e.ModuleInfo.ModuleName == LoginModuleName)
            {
                this.RegionManager.RequestNavigate(
                    RegionNames.MainRegion,
                    LoginViewUri);
            }
        }
    }
}
