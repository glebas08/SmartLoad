using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml;

// Дополнительные пространства имён для работы с изображениями
using A = DocumentFormat.OpenXml.Drawing;
using DW = DocumentFormat.OpenXml.Drawing.Wordprocessing;
using PIC = DocumentFormat.OpenXml.Presentation;

using SmartLoad.Models;
using System.IO;
using System.Threading.Tasks;
using System;

namespace SmartLoad.Services
{
    public class ReportService : IReportService
    {
        public async Task<byte[]> GenerateWordAsync(LoadingReportModel model)
        {
            using var memoryStream = new MemoryStream();
            using var document = WordprocessingDocument.Create(memoryStream, WordprocessingDocumentType.Document);
            var mainPart = document.AddMainDocumentPart();
            mainPart.Document = new Document();
            var body = mainPart.Document.AppendChild(new Body());

            AddTitle(body, "Отчёт о загрузке полуприцепа");
            AddSection(body, "Общая информация", () =>
            {
                AddParagraph(body, $"ID схемы: {model.SchemeId}");
                AddParagraph(body, $"Транспортное средство: {model.Scheme.Vehicle?.Name}");
                AddParagraph(body, $"Маршрут: {model.Scheme.Rout?.Name}");
                AddParagraph(body, $"Статус: {model.Scheme.Status}");
            });

            AddSection(body, "Информация о грузе", () =>
            {
                foreach (var order in model.Orders)
                {
                    foreach (var product in order.OrderProducts)
                    {
                        AddParagraph(body, $"Товар: {product.Product?.Name}, Кол-во: {product.Quantity}");
                    }
                }
            });

            AddSection(body, "Нагрузка на оси", () =>
            {
                foreach (var load in model.AxleLoads)
                {
                    AddParagraph(body, $"{load.Key}: {load.Value:F0} кг");
                }
            });

            AddSection(body, "Пошаговое размещение блоков", () =>
            {
                foreach (var step in model.PlacementSteps)
                {
                    AddParagraph(body, $"Шаг {step.StepNumber}: {step.ProductName}");
                    AddParagraph(body, $"Позиция: X={step.PositionX:F2}, Y={step.PositionY:F2}, Z={step.PositionZ:F2}");
                    AddParagraph(body, $"Размеры: {step.Length:F2}x{step.Width:F2}x{step.Height:F2} м");
                    AddParagraph(body, $"Вес: {step.Weight:F2} кг");
                    AddParagraph(body, $"Пункт назначения: {step.Destination}");

                    if (!string.IsNullOrEmpty(step.ScreenshotBase64))
                    {
                        var imageBytes = Convert.FromBase64String(
                            step.ScreenshotBase64.Replace("data:image/png;base64,", ""));
                        AddImage(body, imageBytes);
                    }

                    AddParagraph(body, ""); // Разделитель
                }
            });

            mainPart.Document.Save();
            return memoryStream.ToArray();
        }

        private void AddTitle(Body body, string text)
        {
            var paragraph = body.AppendChild(new Paragraph());
            var run = paragraph.AppendChild(new Run());
            run.AppendChild(new Text(text));
            run.PrependChild(new Bold());
            run.AppendChild(new FontSize() { Val = "28" });
        }

        private Paragraph AddParagraph(Body body, string text)
        {
            var paragraph = body.AppendChild(new Paragraph());
            paragraph.AppendChild(new Run(new Text(text)));
            return paragraph;
        }

        private void AddSection(Body body, string title, Action content)
        {
            AddParagraph(body, title).AppendChild(new Bold());
            content();
            AddParagraph(body, "");
        }

        private void AddImage(Body body, byte[] imageBytes)
        {
            using var imageStream = new MemoryStream(imageBytes);

            // Создаем часть изображения
            var imagePart = body.AppendChild(new Drawing());
            var inline = new DW.Inline();

            // Устанавливаем размеры изображения (в эммах, 1 эмма = 0.01 мм)
            const int emusPerPixel = 9525; // 1 пиксель = 9525 EMUs
            int pixelWidth = 800;
            int pixelHeight = 600;

            var extent = new DW.Extent()
            {
                Cx = pixelWidth * emusPerPixel,
                Cy = pixelHeight * emusPerPixel
            };

            var docProperties = new DW.DocProperties()
            {
                Id = new UInt32Value(1U),
                Name = "Picture"
            };

            var graphic = new A.Graphic();
            var graphicData = new A.GraphicData()
            {
                Uri = "http://schemas.openxmlformats.org/drawingml/2006/picture"
            };

            var picture = new PIC.Picture();
            var nonVisualProps = new PIC.NonVisualPictureProperties();
            var blipFill = new PIC.BlipFill();

            // Добавляем данные изображения
            var blip = new A.Blip()
            {
                Embed = "rId1"  // Это ссылка на часть изображения в документе
            };

            // Добавляем стилизацию
            var stretch = new A.Stretch();
            stretch.FillRectangle = new A.FillRectangle();

            blipFill.Append(blip);
            blipFill.Append(stretch);

            var shapeProps = new PIC.ShapeProperties();
            shapeProps.Transform2D = new A.Transform2D();
            shapeProps.Transform2D.Offset = new A.Offset() { X = 0, Y = 0 };
            shapeProps.Transform2D.Extents = new A.Extents()
            {
                Cx = pixelWidth * emusPerPixel,
                Cy = pixelHeight * emusPerPixel
            };

            picture.Append(nonVisualProps);
            picture.Append(blipFill);
            picture.Append(shapeProps);

            graphicData.Append(picture);
            graphic.Append(graphicData);

            inline.Append(extent);
            inline.Append(docProperties);
            inline.Append(graphic);

            imagePart.Append(inline);
        }
    }
}
