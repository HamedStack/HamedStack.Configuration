// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable CommentTypo
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

using Microsoft.Extensions.Configuration;

namespace HamedStack.Configuration;

/// <summary>
/// Provides utility functions related to configuration.
/// </summary>
public static class ConfigurationUtility
{
    /// <summary>
    /// Builds the application's configuration by considering command line arguments,
    /// environment variables and appsettings files.
    /// </summary>
    /// <remarks>
    /// The method first checks for the "--environment" command line argument to determine
    /// the environment. If not found, it then looks for the "ASPNETCORE_ENVIRONMENT" 
    /// and "DOTNET_ENVIRONMENT" environment variables.
    /// The method prioritizes the command line argument over the environment variables.
    /// The base appsettings.json and the environment-specific appsettings file (e.g., 
    /// appsettings.Development.json) are loaded if they exist.
    /// </remarks>
    /// <returns>
    /// The built <see cref="IConfigurationRoot"/> object containing the configuration 
    /// from all the considered sources.
    /// </returns>
    public static IConfigurationRoot Build()
    {
        var args = Environment.GetCommandLineArgs();
        var envArg = args.ToList().IndexOf("--environment");
        var envFromArgs = envArg >= 0 ? args[envArg + 1] : null;

        var aspnetcore = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        var dotnetcore = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");

        var environment = envFromArgs ?? (string.IsNullOrWhiteSpace(aspnetcore)
            ? dotnetcore
            : aspnetcore);

        return new ConfigurationBuilder()
            .AddCommandLine(Environment.GetCommandLineArgs())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile(
                $"appsettings.{environment}.json",
                optional: true)
            .Build();
    }
}