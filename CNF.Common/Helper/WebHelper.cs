using System.Text.RegularExpressions;

namespace CNF.Common.Helper;

public class WebHelper
{
    /// <summary>
    /// 移除文本字符的a标签
    /// </summary>
    public static string ReplaceStrHref(string content)
    {
        var r = new Regex(@"<a\s+(?:(?!</a>).)*?>|</a>", RegexOptions.IgnoreCase);
        Match m;
        for (m = r.Match(content); m.Success; m = m.NextMatch())
        {
            content = content.Replace(m.Groups[0].ToString(), "");
        }

        return content;
    }

    /// <summary>
    /// 移除字符文本Img里面Alt关键字包裹的内链
    /// </summary>
    public static string RemoveStrImgAlt(string content)
    {
        Regex rg2 = new Regex("(?<=alt=\"<a[^<]*)</a>\"");
        if (rg2.Match(content).Success)
        {
            content = rg2.Replace(content, "");
        }

        Regex rg = new Regex("(?<=alt=\")<a href=\"[^>]*>");
        if (rg.Match(content).Success)
        {
            content = rg.Replace(content, "");
        }

        return content;
    }
}