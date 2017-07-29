using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using TiqUtils.Utils;

namespace CoralTravelAnalyzer.FileDestinations.Office
{
    public static class Excel
    {
        public static void GenerateExcelDocument<T>(this IEnumerable<T> data, string sheetName, string savePath)
        {
            var excel = new ExcelPackage();

            List<ColumnProperty> columnWidthList = new List<ColumnProperty>();
            var dataSource = GeneratDataTable(data, columnWidthList);

            var ws = excel.Workbook.Worksheets.Add($"{sheetName.Substring(0, 24)} at {DateTime.Now:dd.MM.yyyy}");

            InitWorkSheet(ws, dataSource, columnWidthList);

            ws.Cells[2, 1].LoadFromDataTable(dataSource, false);

            try
            {
                using (var file = File.Create(savePath))
                    excel.SaveAs(file);
            }
            catch (Exception ex)
            {
                Logging.ErrorLog(ex.Message);
            }
        }

        private static void InitWorkSheet(ExcelWorksheet ws, DataTable data, List<ColumnProperty> columnWidthList, bool withFilter = true, bool freezePanes = true, bool fullDateTime = false)
        {
            var i = 0;
            foreach (DataColumn item in data.Columns)
            {
                ws.Cells[1, ++i].Value = item.Caption;
                if (item.DataType == typeof(DateTime))
                {
                    ws.Column(i).Style.Numberformat.Format = fullDateTime ? "yyyy.mm.dd h:mm:ss" : "dd.mm.yyyy";
                    ws.Column(i).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }
                else if (item.DataType == typeof(TimeSpan))
                {
                    ws.Column(i).Style.Numberformat.Format = "h:mm:ss";
                    ws.Column(i).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }
            }

            var columnsNumber = data.Columns.Count;
            var rowsNumber = data.Rows.Count + 1;

            var table = ws.Cells[2, 1, rowsNumber, columnsNumber];
            table.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            table.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            table.Style.Border.Right.Style = ExcelBorderStyle.Thin;
            table.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

            table.Style.WrapText = true;

            for (var col = 1; col <= data.Columns.Count; col++)
                ws.Column(col).AlignTextInColumn(ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center);

            i = 0;
            foreach (var columnProp in columnWidthList)
            {
                ws.Column(++i).Style.HorizontalAlignment = columnProp.Align;
                ws.Column(i).Width = columnProp.Width;
            }

            var header = ws.Cells[1, 1, 1, columnsNumber];
            FormatHeadrer(header, Color.FromArgb(31, 78, 120), Color.WhiteSmoke);

            if (withFilter) header.AutoFilter = true;
            if (freezePanes) ws.View.FreezePanes(2, 1);

        }

        private static DataTable GeneratDataTable<T>(IEnumerable<T> data, ICollection<ColumnProperty> columnWidthList)
        {
            var dataSource = new DataTable();

            var dataProperties = typeof(T).GetProperties();

            var mappedProperties = new List<PropertyInfo>();

            foreach (var prop in dataProperties)
            {
                var attribute = prop.GetCustomAttribute(typeof(ExcelDataAttribute), true) as ExcelDataAttribute;

                if (attribute == null) continue;
                
                dataSource.Columns.Add(attribute.ColumnName, prop.PropertyType);
                columnWidthList.Add(new ColumnProperty{
                    Width = attribute.ColumnWidthPt,
                    Align = ParseAlign(attribute.ColumnAlign)
                });
                mappedProperties.Add(prop);
            }

            foreach (var item in data)
            {
                var rowData = mappedProperties.Select(prop => prop.GetValue(item)).ToArray();
                dataSource.Rows.Add(rowData);
            }

            return dataSource;
        }

        private static void AlignTextInColumn(this ExcelColumn col, ExcelHorizontalAlignment horizontal, ExcelVerticalAlignment vertical)
        {
            col.Style.HorizontalAlignment = horizontal;
            col.Style.VerticalAlignment = vertical;
        }

        private static ExcelHorizontalAlignment ParseAlign(ExcelDataAttribute.Align align)
        {
            switch (align)
            {
                case ExcelDataAttribute.Align.Center:
                    return ExcelHorizontalAlignment.Center;
                case ExcelDataAttribute.Align.Left:
                    return ExcelHorizontalAlignment.Left;
                case ExcelDataAttribute.Align.Right:
                    return ExcelHorizontalAlignment.Right;
                case ExcelDataAttribute.Align.Justify:
                    return ExcelHorizontalAlignment.Justify;
                default:
                    return ExcelHorizontalAlignment.Left;
            }
        }

        private static void DrawNormalBorders(ExcelRange range)
        {
            range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
            range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
        }

        private static void FormatHeadrer(ExcelRange header, Color headerColor, Color fontColor)
        {
            header.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            header.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            header.Style.Fill.PatternType = ExcelFillStyle.Solid;
            header.Style.Fill.BackgroundColor.SetColor(headerColor);
            header.Style.Font.Color.SetColor(fontColor);
            header.Style.Font.Size = 12;

            DrawNormalBorders(header);

            header.Style.WrapText = true;
            header.Style.Font.Bold = true;
        }

        private struct ColumnProperty
        {
            public double Width;
            public ExcelHorizontalAlignment Align;
        }
    }
}
