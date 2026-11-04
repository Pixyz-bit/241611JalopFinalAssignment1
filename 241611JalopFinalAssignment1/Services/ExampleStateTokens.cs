using System.Security.Cryptography;
using System.Text.Json;
using Microsoft.AspNetCore.DataProtection;

namespace _241611JalopFinalAssignment1.Services;

// Protected values carry example progress between native POSTs without a database.
public sealed class ExampleStateTokens(IDataProtectionProvider provider)
{
    // Keep the original root purpose so existing loop-game rounds remain readable.
    public string Protect<T>(string purpose, T value) =>
        provider.CreateProtector("Showcase.Loops", purpose, "v1").Protect(JsonSerializer.Serialize(value));

    public bool TryRead<T>(string purpose, string? token, out T? value) where T : class
    {
        value = null;
        if (string.IsNullOrWhiteSpace(token) || token.Length > 8192) return false;
        try
        {
            // The purpose limits a token to the feature that created it.
            var json = provider.CreateProtector("Showcase.Loops", purpose, "v1").Unprotect(token);
            value = JsonSerializer.Deserialize<T>(json);
            return value is not null;
        }
        catch (Exception error) when (error is CryptographicException or JsonException or FormatException)
        {
            return false; // Invalid round data becomes a validation message, not a server error.
        }
    }
}
