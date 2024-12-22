using JiraReportsClient.Configurations.Exceptions;
using Microsoft.Extensions.Configuration;

namespace JiraReportsClient.Configurations;

public static class ConfigurationSectionExtensions
{
    public static string GetRequiredValue(this IConfigurationSection section, string key)
    {
        var value = section[key];
        if (string.IsNullOrEmpty(value))
            throw new ClientConfigurationPropertyEmptyException<string>(key, section.Path);
        return value;
    }

    public static T GetRequiredValue<T>(this IConfigurationSection section, string key)
    {
        var value = section.GetValue<T>(key);
        if (value == null)
            throw new ClientConfigurationPropertyEmptyException<T>(key, section.Path);

        return value;
    }
}