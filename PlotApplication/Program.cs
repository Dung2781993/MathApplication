using GemBox.Spreadsheet;
using GemBox.Spreadsheet.Charts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace PlotApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var inputX = "";
                var coordinateY = new List<String>();
                var coordinateX = new List<String>();
                Console.WriteLine("Enter math formula");
                var input = Console.ReadLine();
                do
                {
                    var trimmed = String.Concat(input.Where(c => !Char.IsWhiteSpace(c)));
                    Console.WriteLine("Enter x value or type exit to quit the program and start plotting");
                    inputX = Console.ReadLine();
                    if (inputX != "exit")
                    {
                        var stringArr = trimmed.ToCharArray().Select(c => c.ToString()).ToArray(); ;

                        for (var i = 0; i < stringArr.Length; i++)
                        {
                            if (stringArr[i] == "x") stringArr[i] = inputX;
                        }

                        string formula = string.Join("", stringArr);

                        var calculator = new RegexCalculator.RegexCalculator();
                        var result = calculator.Calculate(formula);

                        Console.WriteLine("The x value " + inputX);
                        Console.WriteLine("The y value " + result);

                        coordinateX.Add(inputX);
                        coordinateY.Add(result.ToString());
                    }
                } while (inputX != "exit");
                Console.WriteLine("The diagram will be shown in the excel file");
                plotGraph(coordinateX, coordinateY);
            } catch (Exception e)
            {
                Console.WriteLine("Wrong input format");
            }

        }

        public static void plotGraph(List<String> coordinateX, List<String> coordinateY)
        {
            // If using Professional version, put your serial key below.
            SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");

            var workbook = new ExcelFile();
            var worksheet = workbook.Worksheets.Add("Chart");

            // Add data which will be used by the Excel chart.
            worksheet.Cells["A1"].Value = "X";
            for (var i = 0; i < coordinateX.Count; i++)
            {
                worksheet.Cells["A2"].Value = int.Parse(coordinateX[0]);
                worksheet.Cells["A3"].Value = int.Parse(coordinateX[1]);
                worksheet.Cells["A4"].Value = int.Parse(coordinateX[2]);
                worksheet.Cells["A5"].Value = int.Parse(coordinateX[3]);
            }

            worksheet.Cells["B1"].Value = "Y";
            for (var i = 0; i < coordinateY.Count; i++)
            {
                worksheet.Cells["B2"].Value = int.Parse(coordinateY[0]);
                worksheet.Cells["B3"].Value = int.Parse(coordinateY[1]);
                worksheet.Cells["B4"].Value = int.Parse(coordinateY[2]);
                worksheet.Cells["B5"].Value = int.Parse(coordinateY[3]);
            }

            // Set header row and formatting.
            worksheet.Rows[0].Style.Font.Weight = ExcelFont.BoldWeight;
            worksheet.Columns[0].Width = (int)LengthUnitConverter.Convert(3, LengthUnit.Centimeter, LengthUnit.ZeroCharacterWidth256thPart);
            //worksheet.Columns[1].Style.NumberFormat = "\"$\"#,##0";

            // Make entire sheet print on a single page.
            worksheet.PrintOptions.FitWorksheetWidthToPages = 1;
            worksheet.PrintOptions.FitWorksheetHeightToPages = 1;

            // Create Excel chart and select data for it.
            var chart = worksheet.Charts.Add(ChartType.Line, "D2", "M25");
            chart.SelectData(worksheet.Cells.GetSubrangeAbsolute(0, 0, 4, 1), true);

            workbook.Save("Chart.xlsx");
        }
    }
}
