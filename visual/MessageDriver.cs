
using ConfigManagePrak2.configuration;
using ConfigManagePrak2.dependency;

namespace ConfigManagePrak2.visual
{
    public static class MessageDriver
    {

        public static void DisplayInitialConfig(InitialConfig config)
        {
            Console.WriteLine("Текущая конфигурация:");
            Console.WriteLine($"  Имя пакета: {config.PackageName}");
            Console.WriteLine($"  URL репозитория: {config.RepositoryUrl}");
            Console.WriteLine($"  Режим тестового репозитория: {config.UseTestRepository}");
            Console.WriteLine($"  Имя выходного файла: {config.OutputFileName}");
            Console.WriteLine($"  Подстрока для фильтрации: {(string.IsNullOrEmpty(config.FilterSubstring) ? "не задана" : config.FilterSubstring)}");
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

        public static void DisplayDependencies(InitialConfig config, List<Dependency> dependencies)
        {
            Console.WriteLine($"\nПрямые зависимости пакета '{config.PackageName}':");
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

    }
}
