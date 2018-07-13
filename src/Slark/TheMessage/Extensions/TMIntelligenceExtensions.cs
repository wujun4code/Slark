using System;
using System.Collections.Generic;
using System.Linq;

namespace TheMessage.Extensions
{
    public static class TMIntelligenceExtensions
    {
        public static IEnumerable<TMIntelligence> TryToReceive(this IEnumerable<TMIntelligence> source, TMIntelligence intelligence)
        {
            return source.Append(intelligence);
        }
        public static int CountInBlack(this IEnumerable<TMIntelligence> source)
        {
            return source.CountInColor(TMIntelligence.TMColor.Black);
        }
        public static int CountInRed(this IEnumerable<TMIntelligence> source)
        {
            return source.CountInColor(TMIntelligence.TMColor.Red);
        }
        public static int CountInBlue(this IEnumerable<TMIntelligence> source)
        {
            return source.CountInColor(TMIntelligence.TMColor.Blue);
        }
        public static int CountInColor(this IEnumerable<TMIntelligence> source, TMIntelligence.TMColor color)
        {
            switch (color)
            {
                case TMIntelligence.TMColor.Black:
                    var black = source.Where(i => i.IsBlack);
                    if (black.Any()) return black.Count();
                    break;
                case TMIntelligence.TMColor.Blue:
                    var blue = source.Where(i => i.IsBlue);
                    if (blue.Any()) return blue.Count();
                    break;
                case TMIntelligence.TMColor.Red:
                    var red = source.Where(i => i.IsRed);
                    if (red.Any()) return red.Count();
                    break;
            }

            return 0;
        }
    }
}
