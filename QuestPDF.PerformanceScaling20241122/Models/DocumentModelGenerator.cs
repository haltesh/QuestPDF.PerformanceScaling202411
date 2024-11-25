
namespace QuestPDF.PerformanceScaling20241122.Models
{
    internal static class DocumentModelGenerator
    {
        public static DocumentModel GenerateSmallShallowModel()
        {
            return GenerateShallowModel(100);
        }

        public static DocumentModel GenerateLargeShallowModel()
        {
            return GenerateShallowModel(10000);
        }

        public static DocumentModel GenerateMassiveShallowModel()
        {
            return GenerateShallowModel(30000);
        }

        #region PRIVATE
        private static DocumentModel GenerateShallowModel(int numberOfSections)
        {
            var model = new DocumentModel();
            var random = new Random();

            model.DocumentDate = DateTime.Today.AddDays(-100 + random.Next() % 100);

            for (var i = 0; i < numberOfSections; i++)
            {
                var section = new DocumentSection();
                var numberOfRows = 1 + random.Next() % 6;
                var numberOfTotals = 1 + random.Next() % 3;

                section.Title = Guid.NewGuid().ToString();
                section.Subtitle = Guid.NewGuid().ToString();
                section.Rows = GenerateRows(random, numberOfRows);
                section.Totals = GenerateTotals(random, numberOfTotals);

                model.Sections.Add(section);
            }

            return model;
        }

        private static List<DocumentSectionRow> GenerateRows(Random random, int numberOfRows)
        {
            var list = new List<DocumentSectionRow>();

            for (var i = 0; i < numberOfRows; i++)
            {
                var item = new DocumentSectionRow();

                item.Column1 = DateTime.Today.AddDays(-100 + random.Next() % 100);
                item.Column2 = Guid.NewGuid().ToString();
                item.Column3 = 100m + (decimal)(random.NextDouble() * 5000);
                item.Column4 = 100m + (decimal)(random.NextDouble() * 5000);
                list.Add(item);
            }

            return list;
        }

        private static List<DocumentSectionTotal> GenerateTotals(Random random, int numberOfRows)
        {
            var list = new List<DocumentSectionTotal>();

            for (var i = 0; i < numberOfRows; i++)
            {
                var item = new DocumentSectionTotal();

                item.Column1_2 = Guid.NewGuid().ToString().Substring(0, 5);
                item.Column3 = 100m + (decimal)(random.NextDouble() * 5000);
                item.Column4 = 100m + (decimal)(random.NextDouble() * 5000);
                list.Add(item);
            }

            return list;
        }
        #endregion
    }
}
