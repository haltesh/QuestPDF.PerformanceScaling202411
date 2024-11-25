using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.PerformanceScaling20241122.Common;
using QuestPDF.PerformanceScaling20241122.Models;
using System.Globalization;

namespace QuestPDF.PerformanceScaling20241122.Documents.Composers
{
    internal class CommonComposer
    {
        public void ComposePageHeader(IContainer container, int pageNumber)
        {
            container.EnsureSpace().Column(column =>
            {
                column.Item()
                    .DefaultTextStyle(Typography.Normal)
                    .Row(row =>
                    {
                        row.RelativeItem(1).AlignLeft()
                            .Text(text =>
                            {
                                text.ClampLines(1);
                                text.Span("Header left");
                            });
                        row.ConstantItem(10f, Unit.Millimetre);
                        row.RelativeItem(1).AlignRight()
                            .Text(text =>
                            {
                                text.ClampLines(1);
                                text.Span("Header right");
                            });
                    });

                column.Item().PaddingTop(1).BorderBottom(0.5f).BorderColor(Colors.Black);
                column.Item().Height(15);
            });
        }

        public void ComposeDocumentHeader(IContainer container)
        {
            container
                .Column(column =>
                {
                    column.Item().Text("Document title").Style(Typography.Title);

                    column.Item().Element(ColumnStyle).Text(text =>
                    {
                        text.Span("Lorem ipsum dolor sit amet, consectetur adipiscing elit.");
                    });

                    column.Item().Element(ColumnStyle).Text(text =>
                    {
                        text.Span("Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas.");
                    });

                    column.Item().Element(ColumnStyle).Text(text =>
                    {
                        text.Span("Vestibulum auctor neque id cursus rhoncus");
                    });

                    column.Item().Height(25);
                });
        }

        public void ComposeSectionHeader(IContainer container, DocumentSection section)
        {
            container.Column(column =>
            {
                column.Item().PaddingTop(5).Text(text =>
                {
                    text.Span(section.Title).Style(Typography.Big).Bold();
                });

                column.Item().Element(ColumnStyle).Text(text =>
                {
                    text.Span(section.Subtitle);
                });
            });
        }

        public void ComposePageFooter(IContainer container, DocumentModel model, int pageNumber)
        {
            container.Column(column =>
            {
                column.Item().Height(10);

                column.Item().BorderBottom(0.5f).BorderColor(Colors.Black);

                column.Item().Height(2);

                column.Item().Row(row =>
                {
                    row.RelativeItem(2).AlignLeft().Text($"Prepared on: {model.DocumentDate.ToString("F", new CultureInfo("en-US"))}").Style(Typography.Small);

                    row.RelativeItem(2).AlignRight().Text(text =>
                    {
                        text.CurrentPageNumber().Style(Typography.Small);
                        text.Span("/").Style(Typography.Small);
                        text.TotalPages().Style(Typography.Small);
                    });
                });
            });
        }

        #region PRIVATE
        private static IContainer ColumnStyle(IContainer container)
        {
            return container.PaddingBottom(1).PaddingTop(3);
        }
        #endregion
    }
}
