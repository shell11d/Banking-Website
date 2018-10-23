using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Web;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSE545.Group8.Bank.Utilities
{
    public class PDFGenerator
    {
        public static object HttpContext { get; private set; }

        // Method : Generates PDF file in a specified location
        //Arguments Required: Location to be saved, File name, 
        public static string generatePDF( List<String> str, String location, String pdfName)
        {
            //StringBuilder pdfStrings = new StringBuilder();
            try
            {
                Document doc = new Document();
                var folder = @"D:\Newfolder\";
                var pdflocation = folder + pdfName + ".pdf";
                PdfWriter.GetInstance(doc, new FileStream(pdflocation, FileMode.Create));
                doc.Open();
                foreach (String line in str)
                {
                    doc.Add(new Paragraph(line));
                }
                doc.Close();

                return pdflocation;

            }
            catch(Exception e)
            {
                return null;
            }
        }
    }
}
