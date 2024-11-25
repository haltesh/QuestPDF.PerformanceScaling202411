using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.PerformanceScaling20241122.Common;
using QuestPDF.PerformanceScaling20241122.Models;

namespace QuestPDF.PerformanceScaling20241122.Documents.Composers
{
    internal class TableComposer
    {
        private float Column1 => 80f;
        private float Column2 => 394f;
        private float Column3 => 264f;
        private float Column4 => 264f;

        public void ComposeTableHeader(IContainer container)
        {
            container
                .DefaultTextStyle(Typography.Normal.Bold())
                .Background(Colors.Grey.Lighten1)
                .PaddingVertical(2f)
                .Column(column =>
                {
                    column.Item()
                        .Element(element =>
                        {
                            ComposeTableRowStructure(
                                element,
                                column1: "Date",
                                column2: "Description",
                                column3: "Column 5",
                                column4: "Column 6"
                            );
                        });
                });
        }

        public void ComposeTableRow(IContainer container, DocumentSectionRow row)
        {
            container
                .DefaultTextStyle(Typography.Normal)
                .Element(element =>
                {
                    ComposeTableRowStructure(
                        element,
                        column1: row.Column1?.ToString("dd.MM.yyyy") ?? "",
                        column2: row.Column2,
                        column3: row.Column3.ToString("N2"),
                        column4: row.Column4.ToString("N2")
                    );
                });
        }

        public void ComposeFooterRow(
            IContainer container,
            DocumentSectionTotal total
        )
        {
            container
                .DefaultTextStyle(Typography.Normal.Bold())
                .Row(row =>
                {
                    row.RelativeItem(Column1 + Column2)
                    .AlignRight()
                    .Text(total.Column1_2);

                    row.RelativeItem(Column3)
                        .AlignRight()
                        .Text(total.Column3.ToString("N2"));

                    row.RelativeItem(Column4)
                        .AlignRight()
                        .Text(total.Column4.ToString("N2"));
                });
        }

        public void ComposeFooterLine(IContainer container)
        {
            container.Row(row =>
            {
                row.RelativeItem(Column1 + Column2);

                row.RelativeItem(Column3 + Column4)
                    .BorderBottom(0.5f);
            });
        }

        #region PRIVATE
        private void ComposeTableRowStructure(
            IContainer container,
            string column1,
            string column2,
            string column3,
            string column4
        )
        {
            container.Row(row =>
            {
                row.RelativeItem(Column1)
                    .Text(column1);

                row.RelativeItem(Column2)
                    .Text(column2);

                row.RelativeItem(Column3)
                    .AlignRight()
                    .Text(column3);

                row.RelativeItem(Column4)
                    .AlignRight()
                    .Text(column4);
            });
        }
        #endregion
    }
}
