using Microsoft.Extensions.Caching.Memory;

namespace BusinessLayer;

public static class Constants
{
    public const string GetPublishersCacheKey = "GetPublishers";
    public const string GetGenresCacheKey = "GetGenres";
}