using ConfigManagePrak2.configuration;
using System.Text.Json;
using System.Xml.Linq;

namespace ConfigManagePrak2.dependency
{
    public static class DependencyService
    {

        private static readonly int MAX_DEPTH = 2;

        public static async Task<Dependency> GetPackageDependenciesAsync()
        {
            return InitialConfig.UseTestRepository ? GetDependenciesFromTestRepository() : await GetDependenciesFromNuGetRepositoryAsync();
        }

        private static Dependency GetDependenciesFromTestRepository()
        {
            LocalRepository repository = LocalRepository.At(InitialConfig.RepositoryUrl);
            Dependency root = new() { Name = InitialConfig.PackageName, Version = "Latest" };

            PutLocalDeps(repository, root, new Stack<string>(), 0);

            return root;
        }

        private static void PutLocalDeps(LocalRepository repository, Dependency parent, Stack<string> currentPath, int depth)
        {
            currentPath.Push(parent.Name);

            Dictionary<string, string>? deps = repository.Repo[parent.Name];
            if (deps == null)
            {
                currentPath.Pop();
                return;
            }

            foreach (var entry in deps)
            {
                string dependencyName = entry.Key;

                if (currentPath.Contains(dependencyName))
                {
                    var cycle = currentPath.Reverse().Concat([dependencyName]);
                    throw new InvalidOperationException(
                        $"Обнаружена циклическая зависимость: {string.Join(" -> ", cycle)}");
                }

                Dependency dependency = new() { Name = dependencyName, Version = entry.Value, LoadPriority = depth};
                PutLocalDeps(repository, dependency, currentPath, depth + 1);
                parent.Dependencies.Add(dependency);
            }

            currentPath.Pop();
        }




        private static async Task<Dependency> GetDependenciesFromNuGetRepositoryAsync()
        {
            try
            {
                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("User-Agent", "ConfigManagePrak2/1.0");

                string package = InitialConfig.PackageName;
                string version = await GetNugetPackageVersion(httpClient, package);

                Dependency root = new() { Name = package, Version = version };
                await PutNugetDepends(httpClient, root, new Stack<string>(), 0);

                return root;
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при получении зависимостей из репозитория: {ex.Message}");
            }
        }

        private static async Task PutNugetDepends(HttpClient client, Dependency parent, Stack<string> currentPath, int depth)
        {
            currentPath.Push(parent.Name);

            Dictionary<string, string> deps = await GetPackageDepends(client, parent.Name, parent.Version);

            foreach (var entry in deps)
            {
                string dependencyName = entry.Key;

                if (currentPath.Contains(dependencyName))
                {
                    var cyclePath = currentPath.Reverse().Concat([dependencyName]);
                    throw new InvalidOperationException(
                        $"Обнаружена циклическая зависимость: {string.Join(" -> ", cyclePath)}");
                }

                Dependency dependency = new() { Name = dependencyName, Version = entry.Value, LoadPriority = depth };
                if(depth < MAX_DEPTH) await PutNugetDepends(client, dependency, currentPath, depth + 1);

                parent.Dependencies.Add(dependency);
            }

            currentPath.Pop();
        }

        private static async Task<Dictionary<string, string>> GetPackageDepends(HttpClient client, string package, string version)
        {
            string xml = await GetXmlFromNuget(client, package, version);
            Dictionary<string, string> map = new();

            XDocument doc = XDocument.Parse(xml);
            var dependencyElements = doc.Descendants().Where(elem => elem.Name.LocalName.Equals("dependency"));

            foreach (var element in dependencyElements)
            {
                string? name = element.Attribute("id")?.Value;
                string? ver = element.Attribute("version")?.Value;

                if (name == null || ver == null || map.ContainsKey(name))
                    continue;

                map[name] = ver;
            }

            return map;
        }

        private static async Task<string> GetXmlFromNuget(HttpClient client, string package, string version)
        {
            //https://api.nuget.org/v3-flatcontainer/serilog.sinks.file/7.0.0/serilog.sinks.file.nuspec
            string p = package.ToLower();
            string v = version.ToLower();
            string url = $"{InitialConfig.RepositoryUrl.TrimEnd('/')}/{p}/{v}/{p}.nuspec";

            return await client.GetStringAsync(url);
        }

        private static async Task<string> GetNugetPackageVersion(HttpClient client, string package)
        {
            string url = $"{InitialConfig.RepositoryUrl.TrimEnd('/')}/{package.ToLower()}/index.json";
            var response = await client.GetStringAsync(url);
            var json = JsonDocument.Parse(response);
            string? version = json.RootElement.GetProperty("versions").EnumerateArray().Last().GetString();
            return version ?? "1.0.0";
        }

    }
}
