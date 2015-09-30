using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Medcenter.Desktop.Modules.CabinetModule.Model
{
    public class PhraseTemplateSelector : DataTemplateSelector
    {
        private DataTemplate _phraseTemplate;

        public DataTemplate PhraseTemplate
        {
            get { return _phraseTemplate; }
            set { _phraseTemplate = value; }
        }

        private DataTemplate _headerTemplate;

        public DataTemplate HeaderTemplate
        {
            get { return _headerTemplate; }
            set { _headerTemplate = value; }
        }
        private DataTemplate _pictureTemplate;

        public DataTemplate PictureTemplate
        {
            get { return _pictureTemplate; }
            set { _pictureTemplate = value; }
        }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            PrintPhrase printPhrase = (PrintPhrase)item;

            if (printPhrase.Type == 5)
            {
                return _pictureTemplate;
            }
            if (printPhrase.Type == 0)
            {
                return _headerTemplate;
            }
            return _phraseTemplate;
        }
    }
}
