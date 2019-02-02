using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
using _Excel = Microsoft.Office.Interop.Excel;

namespace TimerSystem.ExcelHandler
{
    class Excel
    {
        public string Path { get; set; }
        private readonly _Application _excel = new Application();
        private readonly Workbook workBook;
        private readonly Worksheet _workSheet;

        public Excel(string path, int sheet)
        {
            Path = path;
            workBook = _excel.Workbooks.Open(path);
            _workSheet = workBook.Worksheets[sheet];
        }


        public void WriteCell(int row, int column, string value)
        {
            _workSheet.Cells[row, column] = value;
            
        }

        public void Save()
        {
            workBook.Save();
        }
    }
}
