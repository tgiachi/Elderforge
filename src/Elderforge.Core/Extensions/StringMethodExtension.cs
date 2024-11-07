using Elderforge.Core.Utils;

namespace Elderforge.Core.Extensions;

public static class StringMethodExtension
{

    public static string ToSnakeCase(this string text)
    {
        return StringUtils.ToSnakeCase(text);
    }

}
