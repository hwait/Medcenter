using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;

namespace Medcenter.Desktop.Modules.StatusbarModule
{
    [ModuleExport(typeof (StatusbarModule))]
    public class StatusbarModule : IModule
    {
        public void Initialize()
        {
        }
    }
}
