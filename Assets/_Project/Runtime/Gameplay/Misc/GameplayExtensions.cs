namespace Runtime.Gameplay.Misc
{
    public static class GameplayExtensions
    {
        public static string ToGameString(this int value)
        {
            if (value < 1000)
                return value.ToString();
            if (value < 1000000)
            {
                var mod = value % 1000;
                return value / 1000 + "." + (mod / 100) + (mod / 10) + "K";
            }

            var modM = value % 1000000;
            return value / 1000000 + "." + (modM / 100000) + (modM / 10000) + "M";
        }
    }
}