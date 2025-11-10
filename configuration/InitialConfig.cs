using ConfigManagePrak2.visual;

namespace ConfigManagePrak2.configuration
{
    public static class InitialConfig
    {
        public static string PackageName { get; set; } = string.Empty;
        public static string RepositoryUrl { get; set; } = string.Empty;
        public static bool UseTestRepository { get; set; } = false;
        public static string OutputFileName { get; set; } = "dependency_graph.png";
        public static string FilterSubstring { get; set; } = string.Empty;

        public static void ParseFromArgs(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i])
                {
                    case "--package":
                    case "-p":
                        if (i + 1 >= args.Length) throw new ArgumentException("Не указано имя пакета после --package");
                        PackageName = args[++i];
                        if (string.IsNullOrWhiteSpace(PackageName))
                            throw new ArgumentException("Имя пакета не может быть пустым");
                        break;

                    case "--repository":
                    case "-r":
                        if (i + 1 >= args.Length) throw new ArgumentException("Не указан URL репозитория после --repository");
                        RepositoryUrl = args[++i];
                        if (string.IsNullOrWhiteSpace(RepositoryUrl))
                            throw new ArgumentException("URL репозитория не может быть пустым");
                        break;

                    case "--test":
                    case "-t":
                        UseTestRepository = true;
                        break;

                    case "--output":
                    case "-o":
                        if (i + 1 >= args.Length) throw new ArgumentException("Не указано имя файла после --output");
                        OutputFileName = args[++i];
                        if (string.IsNullOrWhiteSpace(OutputFileName))
                            throw new ArgumentException("Имя файла не может быть пустым");
                        if (!OutputFileName.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
                            OutputFileName += ".png";
                        break;

                    case "--filter":
                    case "-f":
                        if (i + 1 >= args.Length) throw new ArgumentException("Не указана подстрока для фильтрации после --filter");
                        FilterSubstring = args[++i];
                        break;

                    case "--help":
                    case "-h":
                        MessageDriver.DisplayHelp();
                        Environment.Exit(0);
                        break;

                    default:
                        throw new ArgumentException($"Неизвестный параметр: {args[i]}");
                }
            }

            if (string.IsNullOrWhiteSpace(PackageName))
                throw new ArgumentException("Не указано имя пакета (используйте --package)");

            if (string.IsNullOrWhiteSpace(RepositoryUrl))
                throw new ArgumentException("Не указан URL репозитория (используйте --repository)");

        }

    }
}
