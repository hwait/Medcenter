using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Medcenter.Service.Model.Misc;
using Medcenter.Service.Model.Types;
using ServiceStack;

namespace Medcenter.Desktop.Modules.CabinetModule.Model
{
    public class PrintDocument
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Header { get; set; }
        public string Name { get; set; }
        public Patient Patient { get; set; }
        public DocumentPicture Picture { get; set; }
        public List<PrintPhrase> PrintPhrases { get; set; }
        public string Signature { get; set; }

        public PrintDocument()
        {
        }
        public PrintDocument(Survey survey)
        {
            Id = survey.Id;
            Date = survey.Date;
            Header = survey.Header;
            Name = survey.Name;
            Patient = survey.CurrentPatient;
            Picture = new DocumentPicture(survey.Picture, survey.PictureType);
            PrintPhrases = PhrasesTransform(survey.Phrases);
            //Signature = survey.Signature;
        }

        private List<PrintPhrase> PhrasesTransform(ObservableCollection<Phrase> phrases)
        {
            var printPhrases = new List<PrintPhrase>();
            var printPhrase = new PrintPhrase();
            foreach (var phrase in phrases)
            {
                var text = phrase.PrintName;
                
                switch (phrase.Type)
                {
                    case 0://Header
                        if (printPhrase.Text != "")
                        {
                            printPhrases.Add(printPhrase);
                            printPhrase = new PrintPhrase(); 
                        }
                        printPhrase.Type = 0;
                        printPhrase.Header = phrase.PositionName;
                        printPhrases.Add(printPhrase);
                        printPhrase=new PrintPhrase();
                        break;
                    case 1://List
                        printPhrase.Type = 1;
                        text = phrase.Text;
                        switch (phrase.ValuesCount)
                        {
                            case 1:
                                text = string.Format(text, phrase.V1);
                                break;
                            case 2:
                                text = string.Format(text, phrase.V1, phrase.V2);
                                break;
                            case 3:
                                text = string.Format(text, phrase.V1, phrase.V2, phrase.V3);
                                break;
                        }
                        switch ((DecorationTypes)phrase.DecorationType)
                        {
                            case DecorationTypes.InText:
                                printPhrase.Text += text;
                                break;
                            case DecorationTypes.InTextWithPosition:
                                printPhrase.Text += phrase.PrintName;
                                printPhrase.Text += text;
                                break;
                            case DecorationTypes.StartsWithNewParagraph:
                                printPhrases.Add(printPhrase);
                                printPhrase = new PrintPhrase(1);
                                printPhrase.Text += text;
                                break;
                            case DecorationTypes.StartsWithNewParagraphWithPosition:
                                printPhrases.Add(printPhrase);
                                printPhrase = new PrintPhrase(1);
                                printPhrase.Header = phrase.PrintName;
                                printPhrase.Text = text;
                                break;
                            case DecorationTypes.StartsAndEndsWithNewParagraph:
                                printPhrases.Add(printPhrase);
                                printPhrase = new PrintPhrase(1);
                                printPhrase.Text += text;
                                printPhrases.Add(printPhrase);
                                printPhrase = new PrintPhrase(1);
                                break;
                            case DecorationTypes.StartsAndEndsWithNewParagraphWithPosition:
                                printPhrases.Add(printPhrase);
                                printPhrase = new PrintPhrase(1);
                                printPhrase.Header = phrase.PrintName;
                                printPhrase.Text = text;
                                printPhrases.Add(printPhrase);
                                printPhrase = new PrintPhrase(1);
                                break;
                        }
                        break;
                    case 2://Digit
                    case 3://Formula
                        switch (phrase.ValuesCount)
                        {
                            case 0:
                                text = phrase.Text;
                                break;
                            case 1:
                                text = string.Format(text, phrase.V1,"");
                                break;
                            case 2:
                                text = string.Format(text, phrase.V1, "", phrase.V2, "");
                                break;
                            case 3:
                                text = string.Format(text, phrase.V1, "", phrase.V2, "", phrase.V3, "");
                                break;
                        }
                        if (phrase.V1 + phrase.V2 + phrase.V3 > 0)
                        {
                            if (printPhrase.Text != "")
                            {
                                printPhrases.Add(printPhrase);
                                printPhrase = new PrintPhrase(1);
                            }
                            printPhrase.Type = 1;
                            printPhrase.Text = text;
                            printPhrases.Add(printPhrase);
                            printPhrase = new PrintPhrase(1);
                        }
                        break;
                }
                
            }
            printPhrases.Add(printPhrase);
            printPhrase = new PrintPhrase(1);
            return printPhrases;
        }
    }
    public class PrintPhrase
    {
        public int Type { get; set; }
        public string Text { get; set; }
        public string Header { get; set; }
        public int DecorationType { get; set; }
        public PrintPhrase(int type)
        {
            Type = type;
            Text = "";
            Header = "";
        }
        public PrintPhrase()
        {
            Text = "";
            Header = "";
        }
    }
    public class DocumentPicture
    {
        public string Path { get; set; }
        public int DecorationType { get; set; }

        public DocumentPicture()
        {
        }
        public DocumentPicture(string path, int decorationType)
        {
            Path = string.Format("{0}Pictures\\{1}", AppDomain.CurrentDomain.BaseDirectory, path);
            //Path = @"c:\Users\Nikk\Documents\Projects\Medcenter\Desktop\Modules\Medcenter.Desktop.Modules.CabinetModule\Pictures\"+path;
            DecorationType = decorationType;
        }
    }
}
