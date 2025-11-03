using ConfigManagePrak2.configuration;
using System.Text.Json;
using System.Xml.Linq;

namespace ConfigManagePrak2.dependency
{
    public static class DependencyService
    {

        public static async Task<List<Dependency>> GetPackageDependenciesAsync(InitialConfig config)
        {
            return config.UseTestRepository ? GetDependenciesFromTestRepository(config) : await GetDependenciesFromNuGetRepositoryAsync(config);
        }

        private static List<Dependency> GetDependenciesFromTestRepository(InitialConfig config)
        {
            var dependencies = new List<Dependency>();

            try
            {
                if (!File.Exists(config.RepositoryUrl))
                {
                    throw new FileNotFoundException($"Тестовый файл репозитория не найден: {config.RepositoryUrl}");
                }

                var content = File.ReadAllText(config.RepositoryUrl);
                var lines = content.Split('\n', StringSplitOptions.RemoveEmptyEntries);

                foreach (var line in lines)
                {
                    var trimmedLine = line.Trim();
                    if (trimmedLine.StartsWith(config.PackageName + ":")) // PackageName: Dependency1:Version1, Dependency2:Version2
                    {
                        var parts = trimmedLine.Split(':', 2);
                        if (parts.Length == 2)
                        {
                            var dependenciesPart = parts[1];
                            var dependencyEntries = dependenciesPart.Split(',', StringSplitOptions.RemoveEmptyEntries);

                            foreach (var depEntry in dependencyEntries)
                            {
                                var depParts = depEntry.Trim().Split(':', StringSplitOptions.RemoveEmptyEntries);
                                if (depParts.Length >= 2)
                                {
                                    dependencies.Add(new Dependency
                                    {
                                        Name = depParts[0].Trim(),
                                        Version = depParts[1].Trim()
                                    });
                                }
                            }
                        }
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при чтении тестового репозитория: {ex.Message}");
            }

            return dependencies;
        }

        private static async Task<List<Dependency>> GetDependenciesFromNuGetRepositoryAsync(InitialConfig config)
        {
            try
            {
                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("User-Agent", "ConfigManagePrak2/1.0");

                string package = config.PackageName;
                string version = await GetNugetPackageVersion(config, httpClient, package);

                string xml = await GetXmlFromNuget(config, httpClient, package, version);
                return ParseDependencies(xml);
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при получении зависимостей из репозитория: {ex.Message}");
            }
        }

        private static List<Dependency> ParseDependencies(string xml)
        {
            Dictionary<string, Dependency> map = new();

            XDocument doc = XDocument.Parse(xml);
            var dependencyElements = doc.Descendants().Where(elem => elem.Name.LocalName.Equals("dependency"));

            foreach (var element in dependencyElements)
            {
                string? name = element.Attribute("id")?.Value;
                string? version = element.Attribute("version")?.Value;

                if (name == null || version == null || map.ContainsKey(name))
                    continue;

                map[name] = new Dependency
                {
                    Name = name,
                    Version = version
                };
            }

            return map.Values.ToList();
        }


        private static async Task<string> GetXmlFromNuget(InitialConfig config, HttpClient client, string package, string version)
        {
            //https://api.nuget.org/v3-flatcontainer/serilog.sinks.file/7.0.0/serilog.sinks.file.nuspec
            string p = package.ToLower();
            string v = version.ToLower();
            string url = $"{config.RepositoryUrl.TrimEnd('/')}/{p}/{v}/{p}.nuspec";

            return await client.GetStringAsync(url);
        }

        private static async Task<string> GetNugetPackageVersion(InitialConfig config, HttpClient client, string package)
        {
            string url = $"{config.RepositoryUrl.TrimEnd('/')}/{package.ToLower()}/index.json";
            var response = await client.GetStringAsync(url);
            var json = JsonDocument.Parse(response);
            string? version = json.RootElement.GetProperty("versions").EnumerateArray().Last().GetString();
            return version ?? "1.0.0";
        }

    }
}
