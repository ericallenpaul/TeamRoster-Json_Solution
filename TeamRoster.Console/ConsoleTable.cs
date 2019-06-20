using System;
using System.Collections.Generic;
using System.Text;

namespace TeamRoster.App
{
    public class ConsoleTable
    {
        private int _TableWidth = 77;

        public ConsoleTable(int tableWidth)
        {
            _TableWidth = tableWidth;
        }

        public void PrintLine()
        {
            Console.Write(" ");
            Console.WriteLine(new string('-', _TableWidth));
        }

        public void PrintRow(params string[] columns)
        {
            int width = (_TableWidth - columns.Length) / columns.Length;
            string row = "|";

            for (int i = 0; i < columns.Length; i++)
            {
                string column = columns[i];

                if (i == 0)
                {
                    column = $" {columns[i]}";
                }

                row += AlignCenter(column, width) + "|";
            }

            Console.WriteLine(row);
        }

        private string AlignCenter(string text, int width)
        {
            text = text.Length > width ? text.Substring(0, width - 3) + "..." : text;

            if (string.IsNullOrEmpty(text))
            {
                return new string(' ', width);
            }
            else
            {
                return text.PadRight(width - (width - text.Length) / 2).PadLeft(width);
            }
        }

    }
}
