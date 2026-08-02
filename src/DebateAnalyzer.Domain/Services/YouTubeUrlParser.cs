using System.Web;

namespace DebateAnalyzer.Domain.Services;

public static class YouTubeUrlParser
{
    private static readonly string[] AllowedHosts =
    [
        "youtube.com", "www.youtube.com", "m.youtube.com", "youtu.be"
    ];

    public static bool TryParseVideoId(string? url, out string videoId)
    {
        videoId = string.Empty;

        if (!TryParseAbsoluteUrl(url, out var uri))
        {
            return false;
        }

        if (!IsHttpOrHttpsScheme(uri))
        {
            return false;
        }

        if (!IsAllowedHost(uri.Host))
        {
            return false;
        }

        var id = IsShortLinkHost(uri) ? ExtractFromShortLink(uri) : ExtractFromStandardHost(uri);

        if (string.IsNullOrWhiteSpace(id))
        {
            return false;
        }

        videoId = id;
        return true;
    }

    private static bool TryParseAbsoluteUrl(string? url, out Uri uri)
    {
        var isBlank = string.IsNullOrWhiteSpace(url);
        if (isBlank)
        {
            uri = null!;
            return false;
        }

        return Uri.TryCreate(url, UriKind.Absolute, out uri!);
    }

    private static bool IsHttpOrHttpsScheme(Uri uri)
    {
        var isHttp = uri.Scheme == Uri.UriSchemeHttp;
        var isHttps = uri.Scheme == Uri.UriSchemeHttps;
        return isHttp || isHttps;
    }

    private static bool IsAllowedHost(string host)
        => AllowedHosts.Any(allowedHost => host.Equals(allowedHost, StringComparison.OrdinalIgnoreCase));

    private static bool IsShortLinkHost(Uri uri)
        => uri.Host.Equals("youtu.be", StringComparison.OrdinalIgnoreCase);

    private static string? ExtractFromShortLink(Uri uri)
    {
        var pathSegments = SplitPathSegments(uri);
        return pathSegments.FirstOrDefault();
    }

    private static string? ExtractFromStandardHost(Uri uri)
    {
        var pathSegments = SplitPathSegments(uri);

        if (pathSegments is ["watch"])
        {
            var queryParams = HttpUtility.ParseQueryString(uri.Query);
            return queryParams["v"];
        }

        if (pathSegments is ["shorts", var shortsId])
        {
            return shortsId;
        }

        return null;
    }

    private static string[] SplitPathSegments(Uri uri)
    {
        var trimmedPath = uri.AbsolutePath.Trim('/');
        return trimmedPath.Split('/', StringSplitOptions.RemoveEmptyEntries);
    }
}
