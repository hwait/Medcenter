using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Commands;

namespace Medcenter.Desktop.Infrastructure
{
    public static class GlobalCommands
    {
        public static CompositeCommand NavigateCommand = new CompositeCommand();
    }
}
