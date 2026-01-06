using System;

namespace Server.Common.Utils;

public static class StringUtils
{
    public static string MaskString(string? input, char maskChar = '*')
    {
        if (string.IsNullOrEmpty(input) || input.Length <= 6)
            return input ?? string.Empty;

        int maskLength = input.Length - 6;

        return string.Concat(
            input.Substring(0, 3),
            new string(maskChar, maskLength),
            input.Substring(input.Length - 3)
        );
    }

}
