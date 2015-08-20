using System.ComponentModel.Composition;
using System.Windows.Controls;
using Medcenter.Desktop.Modules.SurveysManagerModule.ViewModels;
using Microsoft.Practices.Prism.Mvvm;

namespace Medcenter.Desktop.Modules.SurveysManagerModule.Views
{
    [Export("SurveysManagerMainView")]
    public partial class SurveysManagerMainView : UserControl, IView
    {
        public SurveysManagerMainView()
        {
            InitializeComponent();
        }
        [Import]
        public SurveysManagerMainViewModel ViewModel
        {
            get { return (SurveysManagerMainViewModel)DataContext; }
            set { DataContext = value; }
        }
    }
}
