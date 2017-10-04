using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using CsvHelper;

namespace TemplateCSVMerger
{
    class Program
    {

        static void Main(string[] args)
        {
            int n_args = args.Length;
            if (n_args != 3)
            {
                System.Console.WriteLine("Usage: TemplateCSVMerger <template_file> <csv_file> <column_name_with_output_file");
                System.Console.WriteLine();
                System.Console.WriteLine("This application merges a template file with placeholders with the rows of a CSV file and generates one file for each row.");
                System.Console.WriteLine("The placeholders have the format «name_of_column»");
                System.Console.WriteLine("The first row of the CSV file contains the column names. Rows starting with # are not considered");
                System.Console.WriteLine("CSV configuration ... ");
                return;
            }

            TextReader templateTextReader;
            string templateText = string.Empty;
            try
            {
                templateTextReader = File.OpenText(args[0]);
                templateText = templateTextReader.ReadToEnd();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(string.Format("Exception opening template_file '{0}'\n{1}", args[0], ex.Message));
                return;
            }

            TextReader csvTextReader;
            try
            {
                csvTextReader = File.OpenText(args[1]);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(string.Format("Exception opening csv_file '{0}'\n{1}", args[1], ex.Message));
                return;
            }

            bool readHeaderRow = true;
            string[] headerRow = null;
            CsvParser csvParser = new CsvParser(csvTextReader);
 
            // Configuration
            csvParser.Configuration.AllowComments = true;
            csvParser.Configuration.Comment = '#';
            csvParser.Configuration.Delimiter = ";";
            csvParser.Configuration.HasHeaderRecord = true;
            csvParser.Configuration.IgnoreHeaderWhiteSpace = true;
            csvParser.Configuration.IsHeaderCaseSensitive = false;

            // loop through CSV rows
            while (true)
            {
                string content = templateText;
                string[] row = csvParser.Read();
                string mergedFilename = string.Empty;

                if (row == null) break;
                if (readHeaderRow) // read header row with column names
                {
                    headerRow = row;
                    readHeaderRow = false;
                    continue;
                }

                // replace template placeholders with row values
                for (int i = 0; i < csvParser.FieldCount; i++)
                {
                    content.Replace("«" + headerRow[i] + "»", row[i]);
                    // if column contains filename
                    if (headerRow[i].Equals(args[3], StringComparison.CurrentCultureIgnoreCase))
                        mergedFilename = row[i];
                }

                // write merged file
                if (mergedFilename.Equals(string.Empty))
                    mergedFilename = csvParser.Row + args[0]; // use template file name with number prefix
                File.WriteAllText(mergedFilename, content);
            }

        }
    }
}
