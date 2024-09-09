using System;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Windows.Forms;
using EmfTestCihazi.Classes;
using Microsoft.Office.Interop.Excel;

namespace EmfTestCihazi.Classes
{
    public class ExcelExportHelper
    {
        private string filename;
        private string sourceFile;
        private string destinationFile;
        private string destinationFileDirectory;

        private string _productFullName;
        private string _productSerialNo;
        private string _testOperator;
        private string _companyName;
        private System.Data.DataTable _testValues;

        public ExcelExportHelper(string productFullName, string productSerialNo, string testOperator, string companyName, System.Data.DataTable testValues)
        {
            _productFullName = productFullName;
            _productSerialNo = productSerialNo;
            _testOperator = testOperator;
            _companyName = companyName;
            _testValues = testValues;
        }

        public void ExportToExcel()
        {
            CreateExcelFileWithTemplate();

            object missing = Type.Missing;
            Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();
            Workbook workbook = excelApp.Workbooks.Open(destinationFile, missing, false, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing);
            Worksheet worksheet = (Worksheet)workbook.Sheets[1];
            Range range = worksheet.UsedRange;


            range.Replace("<urun>", _productFullName, XlLookAt.xlWhole, XlSearchOrder.xlByRows, true, missing, missing, missing);
            range.Replace("<seriNo>", _productSerialNo, XlLookAt.xlWhole, XlSearchOrder.xlByRows, true, missing, missing, missing);
            range.Replace("<tarih>", DateTime.Now.ToShortDateString(), XlLookAt.xlWhole, XlSearchOrder.xlByRows, true, missing, missing, missing);
            range.Replace("<firma>", _companyName, XlLookAt.xlWhole, XlSearchOrder.xlByRows, true, missing, missing, missing);
            range.Replace("<operator>", _testOperator, XlLookAt.xlWhole, XlSearchOrder.xlByRows, true, missing, missing, missing);


            int startRow = 12;
            int startColumn = 1;
            int seperate = (_testValues.Rows.Count + 1) / 2;
            for (int rowIndex = 0; rowIndex < _testValues.Rows.Count; rowIndex++)
            {
                if (rowIndex > 0 && rowIndex % seperate == 0)
                    startColumn += 5;
                for (int columnIndex = 0; columnIndex < _testValues.Columns.Count; columnIndex++)
                    worksheet.Cells[startRow + (rowIndex % seperate), startColumn + columnIndex].Value = Convert.ToDouble(_testValues.Rows[rowIndex][columnIndex]);    
            }

            workbook.Save();
            workbook.Close(false, missing, missing);
            excelApp.Quit();
            ReleaseObject(excelApp);
            ReleaseObject(workbook);
            ReleaseObject(worksheet);

        }
        public void CreateExcelFileWithTemplate()
        {
            filename = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            sourceFile = Path.Combine(filename, "AbtfSablon.xlsx");
            destinationFileDirectory = Path.Combine(filename, $"ABTF\\Rapor_{DateTime.Now:yyyy_MM_dd_HH_mm}");
            destinationFile = Path.Combine(destinationFileDirectory, $"Rapor_{DateTime.Now:yyyy_MM_dd_HH_mm}.xlsx");

            if (!Directory.Exists(destinationFileDirectory))
            {
                Directory.CreateDirectory(destinationFileDirectory);
                File.Copy(sourceFile, destinationFile);
                SetFullControlPermission(destinationFileDirectory);
            }
        }

        private void SetFullControlPermission(string folderPath)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(folderPath);
            DirectorySecurity accessControl = directoryInfo.GetAccessControl();
            accessControl.AddAccessRule(new FileSystemAccessRule(
                new SecurityIdentifier(WellKnownSidType.WorldSid, null),
                FileSystemRights.FullControl,
                InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit,
                PropagationFlags.NoPropagateInherit,
                AccessControlType.Allow));
            directoryInfo.SetAccessControl(accessControl);
        }

        private void ReleaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                MessageBox.Show($"Excel uygulamasını serbest bırakılırken bir hata ile karşılaşıldı\nHata Mesajı : {ex.Message}\nStack Trace : {ex.StackTrace}");
            }
            finally
            {
                GC.Collect();
            }
        }
    }
}
