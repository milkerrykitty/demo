using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace template
{
    public partial class ReportForm : Form
    {
        public ReportForm()
        {
            InitializeComponent();
            startPeriod.Value = DateTime.Today.AddMonths(-1);
            endPeriod.Value = DateTime.Today;
            TypeBox.Items.AddRange(new object[] {
                "Выберите тип отчета", "Заказ", "Бронь", "Сделка", "ДТП"});
            TypeBox.SelectedIndex = 0;
        }

        public static class DatabaseHelper
        {
            private static string connectionString = "server=localhost;database=demo;uid=root;password=root;";
            public static DataTable ExecuteQuery(string query, MySqlParameter[] parameters)
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        if (parameters != null)
                        {
                            command.Parameters.AddRange(parameters);
                        }

                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                        {
                            DataTable table = new DataTable();
                            adapter.Fill(table);
                            return table;
                        }
                    }
                }
            }
        }

        private void GenerateReportByObjectType(string objectType, DateTime startDate, DateTime endDate)
        {
            string query = @"
                            SELECT 
                                object_name AS 'Название',
                                description AS 'Описание',
                                amount AS 'Сумма',
                                status AS 'Статус',
                                created_at AS 'Дата создания'
                            FROM orders
                            WHERE object_type = @objectType
                              AND created_at BETWEEN @startDate AND @endDate";

            try
            {
                MySqlParameter[] parameters = new MySqlParameter[] 
                {
                    new MySqlParameter("@objectType", objectType),
                    new MySqlParameter("@startDate", startDate),
                    new MySqlParameter("@endDate", endDate)
                };

                DataTable reportData = DatabaseHelper.ExecuteQuery(query, parameters);

                ReportGrid.DataSource = null;
                ReportGrid.Columns.Clear();
                ReportGrid.DataSource = reportData;
                ReportGrid.Refresh();

                NameLbl.Text = $"Отчет по {objectType} ({startDate:dd.MM.yyyy} - {endDate:dd.MM.yyyy})";

                if (reportData.Rows.Count > 0)
                {
                    decimal totalAmount = 0;
                    foreach (DataRow row in reportData.Rows)
                    {
                        if (row["Сумма"] != DBNull.Value)
                            totalAmount += Convert.ToDecimal(row["Сумма"]);
                    }

                    TotalLbl.Text = $"Всего записей: {reportData.Rows.Count}, Общая сумма: {totalAmount:F2}";
                }
                else
                {
                    TotalLbl.Text = "Нет данных за выбранный период";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка генерации отчета: {ex.Message}");
            }
        }

        private void ExportBut_Click(object sender, EventArgs e)
        {
            if (IsDataGridViewEmpty(ReportGrid))
            {
                MessageBox.Show("Нет данных для экспорта!", "Пустой отчет",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "Excel файлы|*.xlsx";
                saveDialog.Title = "Сохранить отчет";
                saveDialog.FileName = "Отчет_" + DateTime.Now.ToString("yyyyMMdd");
                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    ExportToExcel(ReportGrid, saveDialog.FileName);
                }
            }
        }

        private void ExportToExcel(DataGridView reportGrid, string fileName)
        {
            try
            {
                using (SpreadsheetDocument document = SpreadsheetDocument.Create(fileName, SpreadsheetDocumentType.Workbook))
                {
                    WorkbookPart workbookPart = document.AddWorkbookPart();
                    workbookPart.Workbook = new Workbook();

                    WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                    worksheetPart.Worksheet = new Worksheet(new SheetData());

                    Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());
                    Sheet sheet = new Sheet()
                    {
                        Id = workbookPart.GetIdOfPart(worksheetPart),
                        SheetId = 1,
                        Name = "Отчет"
                    };
                    sheets.Append(sheet);

                    SheetData sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();

                    // Заголовки
                    Row headerRow = new Row();
                    foreach (DataGridViewColumn column in reportGrid.Columns)
                    {
                        if (!column.Visible) continue;
                        Cell cell = new Cell()
                        {
                            DataType = CellValues.String,
                            CellValue = new CellValue(column.HeaderText)
                        };
                        headerRow.Append(cell);
                    }
                    sheetData.Append(headerRow);

                    // Данные
                    foreach (DataGridViewRow dgRow in reportGrid.Rows)
                    {
                        if (dgRow.IsNewRow) continue;
                        Row row = new Row();
                        foreach (DataGridViewCell dgCell in dgRow.Cells)
                        {
                            if (!dgCell.OwningColumn.Visible) continue;
                            string cellValue = dgCell.Value?.ToString() ?? "";
                            Cell cell = new Cell()
                            {
                                DataType = CellValues.String,
                                CellValue = new CellValue(cellValue)
                            };
                            row.Append(cell);
                        }
                        sheetData.Append(row);
                    }
                }

                MessageBox.Show("Данные успешно экспортированы в Excel!", "Готово", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка экспорта: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void ExportToExcel(DataGridView dataGridView, System.Windows.Forms.DataVisualization.Charting.Chart chart, string filePath)
        {
            try
            {
                using (SpreadsheetDocument document = SpreadsheetDocument.Create(filePath, SpreadsheetDocumentType.Workbook))
                {
                    WorkbookPart workbookPart = document.AddWorkbookPart();
                    workbookPart.Workbook = new Workbook();

                    WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                    worksheetPart.Worksheet = new Worksheet(new SheetData());

                    Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());
                    Sheet sheet = new Sheet()
                    {
                        Id = workbookPart.GetIdOfPart(worksheetPart),
                        SheetId = 1,
                        Name = "Отчет"
                    };
                    sheets.Append(sheet);

                    SheetData sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();

                    Row headerRow = new Row();
                    foreach (DataGridViewColumn column in dataGridView.Columns)
                    {
                        if (!column.Visible) continue;
                        Cell cell = new Cell()
                        {
                            DataType = CellValues.String,
                            CellValue = new CellValue(column.HeaderText)
                        };
                        headerRow.Append(cell);
                    }
                    sheetData.Append(headerRow);

                    foreach (DataGridViewRow dgRow in dataGridView.Rows)
                    {
                        if (dgRow.IsNewRow) continue;
                        Row row = new Row();
                        foreach (DataGridViewCell dgCell in dgRow.Cells)
                        {
                            if (!dgCell.OwningColumn.Visible) continue;
                            string cellValue = dgCell.Value?.ToString() ?? "";
                            Cell cell = new Cell()
                            {
                                DataType = CellValues.String,
                                CellValue = new CellValue(cellValue)
                            };
                            row.Append(cell);
                        }
                        sheetData.Append(row);
                    }
                }

                MessageBox.Show($"Данные экспортированы в:\n{filePath}", "Готово",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка экспорта: {ex.Message}", "Ошибка",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static bool IsDataGridViewEmpty(DataGridView dataGridView)
        {
            bool hasVisibleColumns = false;
            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                if (column.Visible)
                {
                    hasVisibleColumns = true;
                    break;
                }
            }

            if (!hasVisibleColumns) return true;

            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                if (!row.IsNewRow) return false;
            }

            return true;
        }

        private void GenerateBut_Click(object sender, EventArgs e)
        {
            DateTime startDate = startPeriod.Value.Date;
            DateTime endDate = endPeriod.Value.Date.AddDays(1).AddSeconds(-1);

            if (startDate > endDate)
            {
                MessageBox.Show("Дата начала не может быть больше даты окончания");
                return;
            }

            string selectedType = TypeBox.SelectedItem.ToString();

            switch (selectedType)
            {
                case "Заказ":
                case "Бронь":
                case "Сделка":
                case "ДТП":
                    GenerateReportByObjectType(selectedType, startDate, endDate);
                    break;
                default:
                    MessageBox.Show("Выберите корректный тип отчета");
                    break;
            }
        }
    }
}
