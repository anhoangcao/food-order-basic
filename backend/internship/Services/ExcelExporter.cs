using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Collections.Generic;
using internship.Models;
using System.Linq;

namespace internship.Services
{
    public class ExcelExporter
    {
        public void ExportToExcel(List<MenuItemModel> items, string filePath)
        {
            using (SpreadsheetDocument document = SpreadsheetDocument.Create(filePath, SpreadsheetDocumentType.Workbook))
            {
                WorkbookPart workbookPart = document.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();

                // Create styles
                WorkbookStylesPart stylesPart = workbookPart.AddNewPart<WorkbookStylesPart>();
                stylesPart.Stylesheet = CreateStylesheet();
                stylesPart.Stylesheet.Save();

                WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                SheetData sheetData = new SheetData();
                worksheetPart.Worksheet = new Worksheet(sheetData);

                Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());
                Sheet sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Items" };
                sheets.Append(sheet);

                // Adding title row that spans two rows and two columns
                Row titleRow1 = new Row();
                Row titleRow2 = new Row();

                Cell titleCell1 = new Cell
                {
                    CellValue = new CellValue("ITEMS"),
                    DataType = CellValues.String,
                    StyleIndex = 2U // Ensure this is correct
                };
                titleRow1.Append(titleCell1);
                titleRow2.Append(new Cell());

                sheetData.Append(titleRow1);
                sheetData.Append(titleRow2);

                // Merge cells for the title
                MergeCells mergeCells = new MergeCells();
                mergeCells.Append(new MergeCell() { Reference = new StringValue("A1:B2") });
                worksheetPart.Worksheet.InsertAfter(mergeCells, worksheetPart.Worksheet.Elements<SheetData>().First());

                // Adding an empty row before the header
                Row emptyRow = new Row();
                sheetData.Append(emptyRow);

                // Adding header row
                Row headerRow = new Row();
                headerRow.Append(
                    new Cell
                    {
                        CellValue = new CellValue("ItemID"),
                        DataType = CellValues.String,
                        StyleIndex = 1U
                    },
                    new Cell
                    {
                        CellValue = new CellValue("ItemName"),
                        DataType = CellValues.String,
                        StyleIndex = 1U
                    },
                    new Cell
                    {
                        CellValue = new CellValue("Description"),
                        DataType = CellValues.String,
                        StyleIndex = 1U
                    },
                    new Cell
                    {
                        CellValue = new CellValue("Price"),
                        DataType = CellValues.String,
                        StyleIndex = 1U
                    },
                    new Cell
                    {
                        CellValue = new CellValue("Available"),
                        DataType = CellValues.String,
                        StyleIndex = 1U
                    },
                    new Cell
                    {
                        CellValue = new CellValue("ImageUrl"),
                        DataType = CellValues.String,
                        StyleIndex = 1U
                    },
                    new Cell
                    {
                        CellValue = new CellValue("CreatedAt"),
                        DataType = CellValues.String,
                        StyleIndex = 1U
                    },
                    new Cell
                    {
                        CellValue = new CellValue("UpdatedAt"),
                        DataType = CellValues.String,
                        StyleIndex = 1U
                    }
                );
                sheetData.Append(headerRow);

                // Adding data rows
                foreach (var item in items)
                {
                    Row dataRow = new Row();
                    dataRow.Append(
                        new Cell
                        {
                            CellValue = new CellValue(item.ItemID.ToString()),
                            DataType = CellValues.Number
                        },
                        new Cell
                        {
                            CellValue = new CellValue(item.ItemName),
                            DataType = CellValues.String
                        },
                        new Cell
                        {
                            CellValue = new CellValue(item.Description),
                            DataType = CellValues.String
                        },
                        new Cell
                        {
                            CellValue = new CellValue(item.Price.ToString("F2")),
                            DataType = CellValues.Number
                        },
                        new Cell
                        {
                            CellValue = new CellValue(item.Available ? "Yes" : "No"),
                            DataType = CellValues.String
                        },
                        new Cell
                        {
                            CellValue = new CellValue(item.ImageUrl),
                            DataType = CellValues.String
                        },
                        new Cell
                        {
                            CellValue = new CellValue(item.CreatedAt.ToString("g")),
                            DataType = CellValues.String
                        },
                        new Cell
                        {
                            CellValue = new CellValue(item.UpdatedAt.ToString("g")),
                            DataType = CellValues.String
                        }
                    );
                    sheetData.Append(dataRow);
                }

                workbookPart.Workbook.Save();
            }
        }

        private Stylesheet CreateStylesheet()
        {
            Stylesheet stylesheet = new Stylesheet();

            Fonts fonts = new Fonts(
                new Font(),
                new Font
                {
                    Bold = new Bold(),
                    FontSize = new FontSize { Val = 11 }
                },
                new Font
                {
                    Bold = new Bold(),
                    FontSize = new FontSize { Val = 14 }
                }
            );

            Fills fills = new Fills(
                new Fill(new PatternFill
                {
                    PatternType = PatternValues.None
                }),
                new Fill(new PatternFill
                {
                    PatternType = PatternValues.Gray125
                }),
                new Fill(new PatternFill
                {
                    PatternType = PatternValues.Solid,
                    ForegroundColor = new ForegroundColor { Rgb = new HexBinaryValue("FFFF00") },
                    BackgroundColor = new BackgroundColor { Indexed = 64 }
                })
            );

            Borders borders = new Borders(
                new Border(
                    new LeftBorder(),
                    new RightBorder(),
                    new TopBorder(),
                    new BottomBorder(),
                    new DiagonalBorder()
                )
            );

            CellFormats cellFormats = new CellFormats(
                new CellFormat(),
                new CellFormat
                {
                    FontId = 1,
                    FillId = 0,
                    BorderId = 0,
                    ApplyFont = true
                },
                new CellFormat
                {
                    FontId = 2,
                    FillId = 2,
                    BorderId = 0,
                    ApplyFont = true,
                    ApplyFill = true,
                    Alignment = new Alignment { Horizontal = HorizontalAlignmentValues.Center, Vertical = VerticalAlignmentValues.Center }
                }
            );

            stylesheet.Append(fonts);
            stylesheet.Append(fills);
            stylesheet.Append(borders);
            stylesheet.Append(cellFormats);

            return stylesheet;
        }
    }
}
