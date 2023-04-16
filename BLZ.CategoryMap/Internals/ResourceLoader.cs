using System.Reflection;
using System.Text.Json;

namespace BLZ.CategoryMap.Internals;

static internal class ResourceLoader
{
    private readonly static JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
    };

    private static Stream GetResourceReader(string name)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourcePaths = assembly.GetManifestResourceNames();
        var resourcePath = resourcePaths.Single(str => str.EndsWith(name));
        return assembly.GetManifestResourceStream(resourcePath)!;
    }

    public static async Task<string> ReadResourceAsync(string name)
    {
        using Stream stream = GetResourceReader(name)!;
        using StreamReader reader = new(stream);
        return await reader.ReadToEndAsync();
    }

    public static async Task<T?> ReadResourceJsonAsync<T>(string name)
    {
        using Stream stream = GetResourceReader(name)!;
        var str = await JsonSerializer.DeserializeAsync<T>(stream, SerializerOptions);
        return str;
    }
}
