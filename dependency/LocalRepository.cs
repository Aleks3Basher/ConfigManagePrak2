
namespace ConfigManagePrak2.dependency
{
    public class LocalRepository
    {

        public Dictionary<string, Dictionary<string, string>> Repo { get; } = new();

        public static LocalRepository At(string file)
        {
            LocalRepository repa = new();
            try
            {
                if (!File.Exists(file))
                {
                    throw new FileNotFoundException($"Тестовый файл репозитория не найден: {file}");
                }

                var content = File.ReadAllText(file);
                var lines = content.Split('\n', StringSplitOptions.RemoveEmptyEntries);

                foreach (var line in lines)
                {
                    var trimmedLine = line.Trim();

                    var parts = trimmedLine.Split(':', 2);
                    if (parts.Length != 2) continue;

                    Dictionary<string, string> deps = new();
                    var depEntries = parts[1].Split(',', StringSplitOptions.RemoveEmptyEntries);
                    
                    foreach(var depEntry in depEntries)
                    {
                        var depParts = depEntry.Split(':');
                        if (depParts.Length != 2) continue;
                        deps[depParts[0].Trim()] = depParts[1].Trim();
                    }
                    repa.Repo[parts[0].Trim()] = deps;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при чтении тестового репозитория: {ex.Message}");
            }
            return repa;
        }

    }
}
