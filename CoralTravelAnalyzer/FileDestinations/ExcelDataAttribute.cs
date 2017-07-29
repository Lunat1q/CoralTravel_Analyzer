using System;
using System.Drawing;

namespace CoralTravelAnalyzer.FileDestinations
{
    public class ExcelDataAttribute : Attribute
    {
        public string ColumnName { get; }
        public int ColumnWidthPx { get; }
        public Color Color { get; }
        public double ColumnWidthPt => ColumnWidthPx / 7.5;

        public Align ColumnAlign { get; }

        public ExcelDataAttribute(string columnName, int columnWidthPx, Align colAlign = Align.Left)
        {
            ColumnName = columnName;
            ColumnWidthPx = columnWidthPx;
            ColumnAlign = colAlign;
        }

        public ExcelDataAttribute(string columnName, int columnWidthPx, Color color, Align colAlign = Align.Left)
        {
            ColumnName = columnName;
            ColumnWidthPx = columnWidthPx;
            ColumnAlign = colAlign;
            Color = color;
        }

        public enum Align
        {
            Center,
            Left,
            Right,
            Justify
        }
    }
}
