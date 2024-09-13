using System;
using System.Data;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Windows.Forms;
using EmfTestCihazi.Classes;
using Microsoft.Office.Interop.Excel;

namespace EmfTestCihazi.Classes
{
    public class YbfTestExcelExportHelper
    {
        private string filename;
        private string sourceFile;
        private string destinationFile;
        private string destinationFileDirectory;

        private string _productFullName;
        private string _companyName;
        private System.Data.DataTable _testValues;

        public YbfTestExcelExportHelper(string productFullName, string companyName, System.Data.DataTable Datatbl)
        {
            _productFullName = productFullName;
            _companyName = companyName;
            _testValues = Datatbl;
        }

        public void ExportToExcel()
        {
            System.Data.DataTable newTable = new System.Data.DataTable();
            newTable.Columns.Add("Seri No ve Tarih", typeof(string));
            newTable.Columns.Add("Hava Aralığı (mm)", typeof(string));
            newTable.Columns.Add("Bobin Direnci", typeof(string));
            newTable.Columns.Add("Endüktans", typeof(string));
            newTable.Columns.Add("Bırakma Gerilimi (V DC)", typeof(string));
            newTable.Columns.Add("Yakalama Gerilimi (V DC)", typeof(string));
            newTable.Columns.Add("Balata Alıştırma (sn.)", typeof(string));
            newTable.Columns.Add("Dinamik Tork", typeof(string));
            newTable.Columns.Add("Statik Tork", typeof(string));

            // Orijinal verilerden yeni tabloya veri taşıma
            foreach (DataRow row in _testValues.Rows)
            {
                // Her veri satırını sırasına göre ekleme
                newTable.Rows.Add(
                    //row["SERI NO"].ToString() + " - " + Convert.ToDateTime(row["TEST TARIH"]).ToString("dd.MM.yyyy"),
                    row["SERI NO"] + " - " + Convert.ToDateTime(row["TEST TARIH"]).ToString("dd.MM.yyyy"),
                    row["HAVA ARALIK"],
                    row["BOBIN DIRENC"],
                    row["ENDUKTANS"],
                    row["BIRAKMA VOLTAJ"],
                    row["YAKALAMA VOLTAJ"],
                    row["ALISTIRMA SURE"],
                    row["DINAMIK TORK"],
                    row["STATIK TORK"]
                );
            }
            CreateExcelFileWithTemplate();

            object missing = Type.Missing;
            Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();
            Workbook workbook = excelApp.Workbooks.Open(destinationFile, missing, false, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing);
            Worksheet worksheet = (Worksheet)workbook.Sheets[1];
            Range range = worksheet.UsedRange;


            range.Replace("<urun>", _productFullName, XlLookAt.xlWhole, XlSearchOrder.xlByRows, true, missing, missing, missing);
            range.Replace("<raporTarih>", DateTime.Now.ToShortDateString(), XlLookAt.xlWhole, XlSearchOrder.xlByRows, true, missing, missing, missing);
            range.Replace("<firma>", _companyName, XlLookAt.xlWhole, XlSearchOrder.xlByRows, true, missing, missing, missing);


            int startRow = 7;
            int startColumn = 1;
            //for (int rowIndex = 0; rowIndex < newTable.Rows.Count; rowIndex++)
            //{
            //    for (int columnIndex = 0; columnIndex < newTable.Columns.Count; columnIndex++)
            //    {
            //        worksheet.Cells[startRow + (rowIndex), startColumn + columnIndex].Value = newTable.Rows[rowIndex][columnIndex];
            //    }
            //}

            for (int rowIndex = 0; rowIndex < newTable.Rows.Count; rowIndex++)
            {
                int currentColumn = startColumn;

                for (int columnIndex = 0; columnIndex < newTable.Columns.Count; columnIndex++)
                {
                    // Hücreleri birleştir ve veriyi ekle
                    Range mergedRange = worksheet.Range[
                        worksheet.Cells[startRow + rowIndex, currentColumn],
                        worksheet.Cells[startRow + rowIndex, currentColumn + 1]
                    ];
                    mergedRange.Merge();
                    mergedRange.Value = newTable.Rows[rowIndex][columnIndex];

                    // Bir sonraki veriyi yazmak için 2 sütun ilerle
                    currentColumn += 2;
                }
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
            sourceFile = Path.Combine(filename, "YbfSablon1.xlsx");
            destinationFileDirectory = Path.Combine(filename, $"YBF\\Rapor_{DateTime.Now:yyyy_MM_dd_HH_mm}");
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
