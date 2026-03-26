using ClosedXML.Excel;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Windows;
using WpfApps.Models;

namespace WpfApps.Helper;

public class FileHelper
{
    public static void WriteXLS(ObservableCollection<Product> products, string fileName)
    {
        if (products.Count == 0 || fileName == null)
            return;

        using (var workbook = new XLWorkbook())
        {
            var worksheet = workbook.Worksheets.Add(fileName);

            // Headers
            worksheet.Cell(1, 1).Value = "Id";
            worksheet.Cell(1, 2).Value = "Name";
            worksheet.Cell(1, 3).Value = "SKU";
            worksheet.Cell(1, 4).Value = "Stock";
            worksheet.Cell(1, 5).Value = "Price";

            // Data
            for (int i = 0; i < products.Count; i++)
            {
                worksheet.Cell(i + 2, 1).Value = products[i].Id;
                worksheet.Cell(i + 2, 2).Value = products[i].Name;
                worksheet.Cell(i + 2, 3).Value = products[i].SKU;
                worksheet.Cell(i + 2, 4).Value = products[i].StockQuantity;
                worksheet.Cell(i + 2, 5).Value = products[i].Price;
            }

            workbook.SaveAs(fileName);
            MessageBox.Show($"Excel file ({fileName}.xlsx) created sucessfully.");
        }
    }

    public static List<Product> ImportXLS()
    {
        List<Product> products = [];
        var openFileDialog = new OpenFileDialog
        {
            Filter = "Excel Workbook|*.xlsx"
        };
        if (openFileDialog.ShowDialog() == true)
        {
            using var workbook = new XLWorkbook(openFileDialog.FileName);
            var worksheet = workbook.Worksheet(1);
            var rows = worksheet.RangeUsed().RowsUsed().Skip(1); // skip header

            foreach (var row in rows)
            {
                var product = new Product
                {                   
                    Name = row.Cell(2).GetValue<string>(),
                    SKU = row.Cell(3).GetValue<string>(),
                    StockQuantity = row.Cell(4).GetValue<int>(),
                    Price = row.Cell(5).GetValue<decimal>()
                };
                products.Add(product);
            }
        }
        return products;
    }
}
