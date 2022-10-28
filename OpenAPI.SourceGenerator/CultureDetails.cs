using System.Globalization;

internal class CultureDetails
{
    public static readonly TextInfo TextInfo = new CultureInfo("en-AU", false).TextInfo;
}
