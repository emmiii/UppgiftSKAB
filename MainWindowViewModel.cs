using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace SileoKonvertering
{
    class MainWindowViewModel : BaseViewModel
    {
        //Alla kommandon som finns 
        public ICommand OpenFileCommand { get; set; }
        //public ICommand ConvertCommand { get; set; }
        public string SelectedFile { get; set; }
        public MainWindowViewModel()
        {
            OpenFileCommand = new RelayCommand(OpenFile);
            //ConvertCommand = new RelayCommand(Convert);
            SelectedFile = "";
        }

        //Det som ska hända när användaren klickar på knappen Välj fil
        private void OpenFile()
        {
            // Skapa dialogrutan  
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Sätter filtret för filer till excelfiler
            dlg.Filter = "Text Files|*.txt";

            // Visa dialogrutan
            Nullable<bool> result = dlg.ShowDialog();

            // om en fil valdes så uppdateras propertyn
            if (result == true)
            {
                // ta in filnamnet och lägg det i propertyn
                SelectedFile = dlg.FileName;
                //textboxen uppdateras inte...
                Convert(SelectedFile);
            }
        }
        private void Convert(string fileName) 
        {
            //används för att skapa raden till den nya filen
            string ssn, name, adress, zipCode, city;
            double principal, interest, fees;
            string newLine;
            char separator = ';';

            //för att läsa filen
            System.IO.StreamReader file = new System.IO.StreamReader(fileName);
            string line;

            //låt användaren döpa den nya filen
            // Skapa dialogrutan  
            Microsoft.Win32.SaveFileDialog saveFile = new Microsoft.Win32.SaveFileDialog();
            saveFile.FileName = "Konverterad"; // Default file name
            saveFile.DefaultExt = ".csv"; // Default file extension

            // Visa dialogrutan
            Nullable<bool> result = saveFile.ShowDialog();

            //för att skriva till en ny fil
            System.IO.StreamWriter newFile = new System.IO.StreamWriter(saveFile.FileName);

            while ((line = file.ReadLine()) != null)
            {
                //plocka ut varje del
                //Personnummer: startposition 2, slutposition 13, 12 tecken långt
                ssn = (line.Substring(1, 12)).Trim();
                //Namn: startposition 119, slutposition 168, 50 tecken långt
                name = (line.Substring(118, 50)).Trim();
                //Adress: startposition 44, slutposition 93, 50 tecken långt
                adress = (line.Substring(43, 50)).Trim();
                //Postnummer: startposition 114, slutposition 118, 5 tecken långt
                zipCode = (line.Substring(113, 5)).Trim();
                //Ort: startposition 94, slutposition 113, 20 tecken långt
                city = (line.Substring(93, 20)).Trim();
                //Kapital: startposition 14, slutposition 23, 10 tecken långt (de två sista siffrorna motsvarar decimalpositionerna i talet)
                principal = System.Convert.ToDouble((line.Substring(13, 8) + "," + line.Substring(21, 2)).Trim());
                //Ränta: startposition 24, slutposition 33, 10 tecken långt
                interest = System.Convert.ToDouble((line.Substring(23, 10)).Trim());
                //Kostnader: startposition 34, sluposition 43, 10 tecken långt
                fees = System.Convert.ToDouble((line.Substring(33, 10)).Trim());

                //Ska skrivas till en csv-fil i ordningen: SSN;Name;Address;ZipCode;City;Principal;Interest;Fees
                newLine = ssn + separator + name + separator + adress + separator + zipCode + separator + city + separator + principal + separator + interest + separator + fees;
                newFile.WriteLine(newLine);
            }

            //stäng filströmmarna (sparar även ned den nya filen ordentligt)
            file.Close();
            newFile.Close();

        }

    }
}
