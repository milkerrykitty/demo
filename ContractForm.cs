using System;
using System.IO;
using System.Windows.Forms;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace template
{
    public partial class ContractForm : Form
    {
        public ContractForm()
        {
            InitializeComponent();
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            string name = txtName.Text;
            string number = txtNumber.Text;
            string amount = txtAmount.Text;
            DateTime date = dtpDate.Value;

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(number))
            {
                MessageBox.Show("Пожалуйста, заполните все обязательные поля");
                return;
            }

            try
            {
                GenerateWordDocument(name, number, amount, date);
                MessageBox.Show("Документ успешно сохранён!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private void GenerateWordDocument(string name, string number, string amount, DateTime date)
        {
            string fileName = $"Договор_{number}_{DateTime.Now:yyyyMMddHHmmss}.docx";
            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), fileName);

            using (WordprocessingDocument package = WordprocessingDocument.Create(filePath, WordprocessingDocumentType.Document))
            {
                var mainPart = package.AddMainDocumentPart();
                mainPart.Document = new Document();

                var body = new Body();

                // Заголовок
                var headingParagraph = new Paragraph(
                    new Run(
                        new Text("ДОГОВОР") { Space = SpaceProcessingModeValues.Preserve }
                    )
                );
                body.Append(headingParagraph);

                // Пустая строка
                body.Append(new Paragraph(new Run(new Text(""))));

                // Содержание
                body.Append(new Paragraph(new Run(new Text($"Номер: {number}"))));
                body.Append(new Paragraph(new Run(new Text($"Клиент: {name}"))));
                body.Append(new Paragraph(new Run(new Text($"Дата: {date.ToShortDateString()}"))));
                if (!string.IsNullOrEmpty(amount))
                {
                    body.Append(new Paragraph(new Run(new Text($"Сумма: {amount} руб."))));
                }

                body.Append(new Paragraph(new Run(new Text(""))));
                body.Append(new Paragraph(new Run(new Text("Подпись ___________________________"))));

                mainPart.Document.AppendChild(body);
            }

            System.Diagnostics.Process.Start(filePath);
        }
    }
}
