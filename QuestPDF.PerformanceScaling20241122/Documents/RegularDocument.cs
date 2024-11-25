using QuestPDF.Elements;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.PerformanceScaling20241122.Common;
using QuestPDF.PerformanceScaling20241122.Documents.Composers;
using QuestPDF.PerformanceScaling20241122.Models;

namespace QuestPDF.PerformanceScaling20241122.Documents
{
    internal class RegularDocument : IDocument
    {
        private CommonComposer CommonComposer { get; set; } = new CommonComposer();
        private TableComposer TableComposer { get; set; } = new TableComposer();
        private DocumentModel Model { get; set; }

        public RegularDocument(DocumentModel model)
        {
            Model = model;
        }

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.MarginLeft(15, Unit.Millimetre);
                page.MarginRight(10, Unit.Millimetre);
                page.MarginTop(10, Unit.Millimetre);
                page.MarginBottom(10, Unit.Millimetre);
                page.DefaultTextStyle(Typography.Normal);
                page.Size(PageSizes.A4);

                page.Header().Element(ComposeHeader);
                page.Content().Element(ComposeContent);
                page.Footer().Element(ComposeFooter);
            });
        }

        private void ComposeHeader(IContainer container)
        {
            container.Dynamic(new RegularDocumentHeader());
        }

        private void ComposeContent(IContainer container)
        {
            container.Column(column =>
            {
                column.Item().Element(CommonComposer.ComposeDocumentHeader);

                foreach (var section in Model.Sections)
                {
                    column.Item().Element(element => CommonComposer.ComposeSectionHeader(element, section));
                    column.Item().Element(element => ComposeSectionTable(element, section));
                    column.Item().Height(25);
                }
            });
        }

        private void ComposeSectionTable(IContainer container, DocumentSection section)
        {
            container.Decoration(decoration =>
            {
                decoration.Before(before => TableComposer.ComposeTableHeader(before));
                decoration.Content(content => content.Column(column =>
                {
                    foreach (var row in section.Rows)
                    {
                        column.Item().Element(element => TableComposer.ComposeTableRow(element, row));
                        column.Item().BorderBottom(0.5f);
                    }

                    foreach (var row in section.Totals)
                    {
                        column.Item().Element(element => TableComposer.ComposeFooterRow(element, row));
                    }
                }));
            });
        }

        private void ComposeFooter(IContainer container)
        {
            container.Dynamic(new RegularDocumentFooter(Model));
        }

        private class RegularDocumentHeader : IDynamicComponent
        {
            private CommonComposer Composer { get; set; } = new CommonComposer();

            public DynamicComponentComposeResult Compose(DynamicContext context)
            {
                var content = context.CreateElement(element => Composer.ComposePageHeader(element, context.PageNumber));

                return new DynamicComponentComposeResult()
                {
                    Content = content,
                    HasMoreContent = false
                };
            }
        }

        private class RegularDocumentFooter : IDynamicComponent
        {
            private DocumentModel Model { get; set; }
            private CommonComposer Composer { get; set; } = new CommonComposer();

            public RegularDocumentFooter(DocumentModel model)
            {
                Model = model;
            }

            public DynamicComponentComposeResult Compose(DynamicContext context)
            {
                var content = context.CreateElement(element => Composer.ComposePageFooter(element, Model, context.PageNumber));

                return new DynamicComponentComposeResult()
                {
                    Content = content,
                    HasMoreContent = false
                };
            }
        }
    }
}
