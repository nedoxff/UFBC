using System.Text.RegularExpressions;

namespace UNDERFELLBattleCreator.addons.underfell_battle_creator.Extensions;

public static class StringExtensions
{
    private static Regex _capitalLetterRegex = new(@"
                (?<=[A-Z])(?=[A-Z][a-z]) |
                 (?<=[^A-Z])(?=[A-Z]) |
                 (?<=[A-Za-z])(?=[^A-Za-z])", RegexOptions.IgnorePatternWhitespace);

    public static string SplitByCapitalLetter(this string s) => _capitalLetterRegex.Replace(s, " ");
}