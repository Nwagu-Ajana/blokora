using System;

namespace Blokora.Domain
{
    public static class ScoreRules
    {
        public static int Placement(int cells) => cells * 5;
        public static int Lines(int lines, int combo)
        {
            if (lines <= 0) return 0;
            var lineValue = lines * 100 + (lines > 1 ? (lines - 1) * 75 : 0);
            return lineValue + Math.Max(0, combo - 1) * 50;
        }

        public static string ClearLabel(int rows, int columns)
        {
            var lines = rows + columns;
            if (lines == 0) return string.Empty;
            if (lines >= 3) return "TRIPLE CLEAR";
            if (rows > 0 && columns > 0) return "CROSS CLEAR";
            return lines == 2 ? "DOUBLE CLEAR" : "SINGLE CLEAR";
        }
    }
}
