namespace Blokora.Domain
{
    public static class ScoreRules
    {
        public static int Placement(int cells) => cells * 5;
        public static int Lines(int lines, int combo) => lines * 100 + (lines > 1 ? lines * 50 : 0) + combo * 25;
    }
}
