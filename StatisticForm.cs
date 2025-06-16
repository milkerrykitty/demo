using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using ClosedXML.Excel; 
using ClosedXML.Excel.Drawings;
using static template.ReportForm;


namespace template
{
    public partial class StatisticForm : Form
    {
        public StatisticForm()
        {
            InitializeComponent();
            LoadStatistics();
        }

        private void LoadStatistics()
        {
            try
            {
                // 1. Загрузка данных из БД
                DataTable data = GetPopularRoomClasses();

                // 2. Настройка таблицы
                SetupDataGridView(data);

                // 3. Настройка диаграммы
                SetupChart(data);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки статистики: {ex.Message}", "Ошибка",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private DataTable GetPopularRoomClasses()
        {
            string query = @"
                    SELECT 
                        class AS 'Класс',
                        COUNT(*) AS 'Количество'
                    FROM fond
                    GROUP BY class
                    ORDER BY COUNT(*) DESC";

            return DatabaseHelper.ExecuteQuery(query, null); // Передан второй параметр
        }

        private void SetupDataGridView(DataTable data)
        {
            dataGridView.DataSource = null;
            dataGridView.DataSource = data;

            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                Font = new Font("Arial", 10, FontStyle.Bold),
                BackColor = Color.LightSteelBlue
            };

            dataGridView.RowHeadersVisible = false;
            dataGridView.EnableHeadersVisualStyles = false;
        }

        private void SetupChart(DataTable data)
        {
            chrtStatistic.Series.Clear();
            chrtStatistic.Titles.Clear();

            Series series = new Series("Классы номеров")
            {
                ChartType = SeriesChartType.Pie,
                IsValueShownAsLabel = true,
                LabelForeColor = Color.White,
                Font = new Font("Arial", 9, FontStyle.Bold)
            };

            foreach (DataRow row in data.Rows)
            {
                string roomClass = row["Класс"].ToString();
                int count = Convert.ToInt32(row["Количество"]);
                series.Points.AddXY(roomClass, count);
            }

            chrtStatistic.Series.Add(series);

            chrtStatistic.ChartAreas[0].AxisX.LabelStyle.Enabled = false;
            chrtStatistic.ChartAreas[0].AxisY.LabelStyle.Enabled = false;

            chrtStatistic.Titles.Add("Самые популярные классы комнат");
            chrtStatistic.Titles[0].Font = new Font("Arial", 14, FontStyle.Bold);
            chrtStatistic.Titles[0].ForeColor = Color.DarkBlue;

            chrtStatistic.Legends[0].Enabled = true;
            chrtStatistic.Legends[0].Docking = Docking.Bottom;
        }

        public static void ShowSaveDialog(DataGridView dataGridView, Chart chart)
        {
            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "Excel файлы|*.xlsx";
                saveDialog.Title = "Сохранить отчет";
                saveDialog.FileName = "Отчет_" + DateTime.Now.ToString("yyyyMMdd_HHmmss");
                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    ExportToExcel(dataGridView, chart, saveDialog.FileName);
                }
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadStatistics();
        }

        public static class ExcelExporter
        {
            public static void ExportToExcel(DataGridView dataGridView, Chart chart, string filePath)
            {
                if (IsDataGridViewEmpty(dataGridView))
                {
                    MessageBox.Show("Нет данных для экспорта!", "Пустой отчет",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                try
                {
                    using (var workbook = new XLWorkbook())
                    {
                        var dataWorksheet = workbook.Worksheets.Add("Данные");
                        ExportDataGridView(dataGridView, dataWorksheet);

                        var chartImagePath = Path.Combine(Path.GetTempPath(), "chart_temp.png");
                        chart.SaveImage(chartImagePath, ImageFormat.Png);

                        var chartWorksheet = workbook.Worksheets.Add("Диаграмма");
                        var image = chartWorksheet.AddPicture(chartImagePath);
                        image.Placement = XLPicturePlacement.FreeFloating;
                        image.Width = 800;
                        image.Height = 500;

                        workbook.SaveAs(filePath);

                        try { File.Delete(chartImagePath); } catch { }
                    }

                    MessageBox.Show($"Отчет успешно экспортирован в:\n{filePath}", "Готово",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка экспорта: {ex.Message}", "Ошибка",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            private static void ExportDataGridView(DataGridView dataGridView, IXLWorksheet worksheet)
            {
                int colIndex = 1;

                foreach (DataGridViewColumn column in dataGridView.Columns)
                {
                    if (column.Visible)
                    {
                        worksheet.Cell(1, colIndex).Value = column.HeaderText;
                        worksheet.Cell(1, colIndex).Style.Font.SetBold();
                        worksheet.Cell(1, colIndex).Style.Fill.SetBackgroundColor(XLColor.LightSteelBlue);
                        colIndex++;
                    }
                }

                int rowIndex = 2;

                foreach (DataGridViewRow row in dataGridView.Rows)
                {
                    if (row.IsNewRow) continue;

                    colIndex = 1;

                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.OwningColumn.Visible)
                        {
                            worksheet.Cell(rowIndex, colIndex).Value = cell.Value?.ToString();
                            colIndex++;
                        }
                    }

                    rowIndex++;
                }

                worksheet.Columns().AdjustToContents();
                IXLTable table = worksheet.RangeUsed().AsTable();
                table.Theme = XLTableTheme.None; 
            }

            private static bool IsDataGridViewEmpty(DataGridView dataGridView)
            {
                foreach (DataGridViewColumn column in dataGridView.Columns)
                {
                    if (column.Visible) break;
                }

                foreach (DataGridViewRow row in dataGridView.Rows)
                {
                    if (!row.IsNewRow) return false;
                }

                return true;
            }

            public static void ShowSaveDialog(DataGridView dataGridView, Chart chart)
            {
                using (SaveFileDialog saveDialog = new SaveFileDialog())
                {
                    saveDialog.Filter = "Excel файлы|*.xlsx";
                    saveDialog.Title = "Сохранить отчет";
                    saveDialog.FileName = "Отчет_" + DateTime.Now.ToString("yyyyMMdd_HHmmss");

                    if (saveDialog.ShowDialog() == DialogResult.OK)
                    {
                        ExportToExcel(dataGridView, chart, saveDialog.FileName);
                    }
                }
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            ExcelExporter.ShowSaveDialog(dataGridView, chrtStatistic);
        }
    }
}

       