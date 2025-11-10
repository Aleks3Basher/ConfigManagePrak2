
using ConfigManagePrak2.configuration;
using ConfigManagePrak2.dependency;

namespace ConfigManagePrak2.visual
{
    public static class MessageDriver
    {

        public static void DisplayInitialConfig()
        {
            Console.WriteLine("Текущая конфигурация:");
            Console.WriteLine($"  Имя пакета: {InitialConfig.PackageName}");
            Console.WriteLine($"  URL репозитория: {InitialConfig.RepositoryUrl}");
            Console.WriteLine($"  Режим тестового репозитория: {InitialConfig.UseTestRepository}");
            Console.WriteLine($"  Имя выходного файла: {InitialConfig.OutputFileName}");
            Console.WriteLine($"  Подстрока для фильтрации: {(string.IsNullOrEmpty(InitialConfig.FilterSubstring) ? "не задана" : InitialConfig.FilterSubstring)}");
        }

        public static void DisplayError(String errorMessage)
        {
            Console.WriteLine($"Ошибка: {errorMessage}");
        }

        public static void DisplayHelp()
        {
            Console.WriteLine("Использование:");
            Console.WriteLine();
            Console.WriteLine("Обязательные опции:");
            Console.WriteLine("  -p, --package NAME    Имя анализируемого пакета");
            Console.WriteLine("  -r, --repository URL  URL репозитория или путь к файлу тестового репозитория");
            Console.WriteLine();
            Console.WriteLine("Дополнительные опции:");
            Console.WriteLine("  -t, --test            Режим работы с тестовым репозиторием");
            Console.WriteLine("  -o, --output FILE     Имя сгенерированного файла с изображением графа (по умолчанию: dependency_graph.png)");
            Console.WriteLine("  -f, --filter SUBSTR   Подстрока для фильтрации пакетов");
            Console.WriteLine("  -h, --help            Показать эту справку");
        }

        public static void DisplayDependencies(List<Dependency> dependencies)
        {
            Console.WriteLine($"\nПрямые зависимости пакета '{InitialConfig.PackageName}':");
            if (dependencies.Count == 0)
            {
                Console.WriteLine("  Зависимости не найдены");
            }
            else
            {
                foreach (var dependency in dependencies)
                {
                    Console.WriteLine($"  - {dependency.Name} {dependency.Version}");
                }
            }
        }


        public static void DisplayDependencyGraph(Dependency graph, string filter)
        {
            Console.WriteLine($"\nГраф зависимостей для пакета '{graph.Name}':");
            var visited = new HashSet<string>();
            DisplayPackageDependencies(graph, visited, 0, filter);
        }

        static void DisplayPackageDependencies(Dependency package, HashSet<string> visited, int level, string filter)
        {
            if (visited.Contains(package.Name))
            {
                Console.WriteLine($"{new string(' ', level * 2)}- {package.Name} {package.Version} [УЖЕ ПОСЕЩЕН]");
                return;
            }

            visited.Add(package.Name);
            var indent = new string(' ', level * 2);

            bool shouldDisplay = string.IsNullOrEmpty(filter) || !package.Name.Contains(filter);

            if (shouldDisplay)
            {
                Console.WriteLine($"{indent}- {package.Name} {package.Version}");
            }

            foreach (var dependency in package.Dependencies)
            {
                if (string.IsNullOrEmpty(filter) || !dependency.Name.Contains(filter))
                {
                    DisplayPackageDependencies(dependency, visited, level + 1, filter);
                }
            }
        }

    }
}
