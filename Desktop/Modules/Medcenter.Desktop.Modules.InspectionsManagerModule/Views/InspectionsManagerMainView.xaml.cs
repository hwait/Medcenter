using System.ComponentModel.Composition;
using System.Windows.Controls;
using Medcenter.Desktop.Modules.InspectionsManagerModule.ViewModels;
using Microsoft.Practices.Prism.Mvvm;

namespace Medcenter.Desktop.Modules.InspectionsManagerModule.Views
{
    [Export("InspectionsManagerMainView")]
    public partial class InspectionsManagerMainView : UserControl, IView
    {
        public InspectionsManagerMainView()
        {
            InitializeComponent();
        }
        [Import]
        public InspectionsManagerMainViewModel ViewModel
        {
            get { return (InspectionsManagerMainViewModel)DataContext; }
            set { DataContext = value; }
        }
    }
}
