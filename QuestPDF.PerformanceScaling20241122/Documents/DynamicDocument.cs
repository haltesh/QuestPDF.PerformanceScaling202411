using QuestPDF.Elements;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.PerformanceScaling20241122.Common;
using QuestPDF.PerformanceScaling20241122.Documents.Composers;
using QuestPDF.PerformanceScaling20241122.Models;

namespace QuestPDF.PerformanceScaling20241122.Documents
{
    internal class DynamicDocument : IDocument
    {
        private DocumentModel Model { get; set; }

        public DynamicDocument(DocumentModel model)
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

                page.Content().Dynamic(new DynamicComponent(Model));
            });
        }

        private class DynamicComponent : IDynamicComponent<DynamicState>
        {
            private DocumentModel Model { get; set; }
            public DynamicState State { get; set; }
            private CommonComposer CommonComposer { get; set; } = new CommonComposer();
            private TableComposer TableComposer { get; set; } = new TableComposer();
            private List<object> InstructionsList { get; set; }

            public DynamicComponent(DocumentModel model)
            {
                Model = model;
                State = new DynamicState()
                {
                    ItemsCompleted = 0
                };

                InstructionsList = GenerateInstructionsList();
            }

            public DynamicComponentComposeResult Compose(DynamicContext context)
            {
                var pageHeaderElement = context.CreateElement(container => container
                    .Width(context.AvailableSize.Width)
                    .Element(element => CommonComposer.ComposePageHeader(element, context.PageNumber))
                );
                var pageFooterElement = context.CreateElement(container => container
                    .Width(context.AvailableSize.Width)
                    .Element(element => CommonComposer.ComposePageFooter(element, Model, context.PageNumber))
                );
                var availableHeight = context.AvailableSize.Height - pageHeaderElement.Size.Height - pageFooterElement.Size.Height;
                var items = ComposeContent(context, availableHeight);
                var itemsCount = 0;

                var content = context.CreateElement(container =>
                {
                    var remainingHeight = context.AvailableSize.Height;

                    container.Column(column =>
                    {
                        column.Item().Element(pageHeaderElement);
                        remainingHeight -= pageHeaderElement.Size.Height;

                        foreach (var item in items)
                        {
                            column.Item().Element(item);
                            itemsCount++;
                            remainingHeight -= item.Size.Height;
                        }

                        if (remainingHeight > pageFooterElement.Size.Height - Size.Epsilon)
                        {
                            column.Item().Height(remainingHeight - pageFooterElement.Size.Height - Size.Epsilon);
                        }

                        column.Item().Element(pageFooterElement);
                    });
                });

                State = new DynamicState
                {
                    ItemsCompleted = State.ItemsCompleted + itemsCount
                };

                return new DynamicComponentComposeResult
                {
                    Content = content,
                    HasMoreContent = State.ItemsCompleted < InstructionsList.Count
                };
            }

            #region PRIVATE
            private List<object> GenerateInstructionsList()
            {
                var list = new List<object>()
                {
                    new Instruction.DocumentHeader()
                };

                foreach (var section in Model.Sections)
                {
                    list.Add(new Instruction.SectionHeader(section));

                    foreach (var row in section.Rows)
                    {
                        list.Add(new Instruction.TableRow(row));
                    }

                    foreach (var row in section.Totals)
                    {
                        list.Add(new Instruction.TableFooterRow(row));
                    }
                }

                return list;
            }

            private IEnumerable<IDynamicElement> ComposeContent(DynamicContext context, float availableHeight)
            {
                var tableHeaderDrawn = false;
                var globalIndex = State.ItemsCompleted;
                var localIndex = 0;
                var totalHeight = 0f;

                while (globalIndex < InstructionsList.Count)
                {
                    IDynamicElement element;
                    var instruction = InstructionsList[globalIndex];

                    element = context.CreateElement(container => container
                        .Width(context.AvailableSize.Width)
                        .Element(element =>
                        {
                            if (instruction is Instruction.DocumentHeader)
                            {
                                CommonComposer.ComposeDocumentHeader(element);
                                return;
                            }

                            if (instruction is Instruction.SectionHeader)
                            {
                                tableHeaderDrawn = false;

                                var itemDefinition = (Instruction.SectionHeader)instruction;

                                element.Column(column =>
                                {
                                    column.Item().Height(25);
                                    column.Item().Element(innerElement => CommonComposer.ComposeSectionHeader(innerElement, itemDefinition.DataItem));
                                });

                                return;
                            }

                            if (instruction is Instruction.TableRow)
                            {
                                element.Column(column =>
                                {
                                    var itemDefinition = (Instruction.TableRow)instruction;

                                    if (!tableHeaderDrawn)
                                    {
                                        column.Item().Element(TableComposer.ComposeTableHeader);
                                        tableHeaderDrawn = true;
                                    }

                                    column.Item().Element(innerElement => TableComposer.ComposeTableRow(innerElement, itemDefinition.DataItem));
                                    column.Item().BorderBottom(0.5f);
                                });

                                return;
                            }

                            if (instruction is Instruction.TableFooterRow)
                            {
                                element.Column(column =>
                                {
                                    var itemDefinition = (Instruction.TableFooterRow)instruction;

                                    if (!tableHeaderDrawn)
                                    {
                                        column.Item().Element(TableComposer.ComposeTableHeader);
                                        tableHeaderDrawn = true;
                                    }

                                    column.Item().Element(innerElement => TableComposer.ComposeFooterRow(innerElement, itemDefinition.DataItem));
                                });

                                return;
                            }
                        })
                    );

                    globalIndex++;
                    localIndex++;

                    var elHeight = element.Size.Height;

                    if (totalHeight + elHeight > availableHeight + Size.Epsilon)
                        break;

                    totalHeight += elHeight;

                    yield return element;
                }
            }
            #endregion
        }

        private struct DynamicState
        {
            public int ItemsCompleted { get; set; }
        }

        private abstract class Instruction
        {
            internal class DocumentHeader
            {
            }

            internal class SectionHeader
            {
                public DocumentSection DataItem { get; set; }

                public SectionHeader(DocumentSection dataItem)
                {
                    DataItem = dataItem;
                }
            }

            internal class TableRow
            {
                public DocumentSectionRow DataItem { get; set; }

                public TableRow(DocumentSectionRow dataItem)
                {
                    DataItem = dataItem;
                }
            }

            internal class TableFooterRow
            {
                public DocumentSectionTotal DataItem { get; set; }

                public TableFooterRow(DocumentSectionTotal dataItem)
                {
                    DataItem = dataItem;
                }
            }
        }
    }
}
