using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestPDF.PerformanceScaling20241122.Common
{
    internal class Typography
    {
        public static TextStyle Default => TextStyle.Default
            .LineHeight(1.5f);

        public static TextStyle Normal => Default
            .FontSize(7f);
        public static TextStyle Small => Default
            .FontSize(6f);
        public static TextStyle Big => Default
            .FontSize(9f);

        public static TextStyle Title => Default
            .FontColor(Colors.Black)
            .FontSize(20f)
            .SemiBold();
    }
}
