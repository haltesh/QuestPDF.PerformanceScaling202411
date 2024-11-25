namespace QuestPDF.PerformanceScaling20241122.Models
{
    internal class DocumentModel
    {
        public DateTime DocumentDate { get; set; }
        public List<DocumentSection> Sections { get; set; } = new();
    }

    internal class DocumentSection
    {
        public string Title { get; set; } = "";
        public string Subtitle { get; set; } = "";
        public List<DocumentSectionRow> Rows { get; set; } = new();
        public List<DocumentSectionTotal> Totals { get; set; } = new();
    }

    internal class DocumentSectionRow
    {
        public DateTime? Column1 { get; set; }
        public string Column2 { get; set; } = "";
        public decimal Column3 { get; set; } = 0;
        public decimal Column4 { get; set; } = 0;
    }

    internal class DocumentSectionTotal
    {
        public string Column1_2 { get; set; } = "";
        public decimal Column3 { get; set; } = 0;
        public decimal Column4 { get; set; } = 0;
    }
}
