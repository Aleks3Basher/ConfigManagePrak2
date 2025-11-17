using ConfigManagePrak2.dependency;
using PlantUml.Net;
using System.IO.Compression;
using System.Text;

namespace ConfigManagePrak2.visual
{
    public class PlantUMLGraphGenerator
    {
        private readonly string plantUML;

        public PlantUMLGraphGenerator(Dependency root, string filter)
        {
            plantUML = GeneratePlantUML(root, filter);
        }

        private string GeneratePlantUML(Dependency root, string filter = "")
        {
            var sb = new StringBuilder();
            var visited = new HashSet<string>();

            sb.AppendLine("@startuml");
            sb.AppendLine("!theme plain");
            sb.AppendLine("skinparam nodesep 10");
            sb.AppendLine("skinparam ranksep 30");
            sb.AppendLine();

            GeneratePackageNodes(root, visited, sb, filter);

            visited.Clear();
            GenerateDependencyRelations(root, visited, sb, filter);

            sb.AppendLine("@enduml");

            return sb.ToString();
        }

        private void GeneratePackageNodes(Dependency package, HashSet<string> visited, StringBuilder sb, string filter)
        {
            if (visited.Contains(package.Name))
                return;

            visited.Add(package.Name);

            bool shouldDisplay = string.IsNullOrEmpty(filter) || !package.Name.Contains(filter);

            if (shouldDisplay)
            {
                sb.AppendLine($"rectangle \"{package.Name}\\n{package.Version}\" as {GetNodeId(package.Name)}");
            }

            foreach (var dependency in package.Dependencies)
            {
                if (string.IsNullOrEmpty(filter) || !dependency.Name.Contains(filter))
                {
                    GeneratePackageNodes(dependency, visited, sb, filter);
                }
            }
        }

        private void GenerateDependencyRelations(Dependency package, HashSet<string> visited, StringBuilder sb, string filter)
        {
            if (visited.Contains(package.Name))
                return;

            visited.Add(package.Name);

            bool packageShouldDisplay = string.IsNullOrEmpty(filter) || !package.Name.Contains(filter);

            foreach (var dependency in package.Dependencies)
            {
                bool dependencyShouldDisplay = string.IsNullOrEmpty(filter) || !dependency.Name.Contains(filter);

                if (packageShouldDisplay && dependencyShouldDisplay)
                {
                    sb.AppendLine($"{GetNodeId(package.Name)} --> {GetNodeId(dependency.Name)}");
                }

                if (dependencyShouldDisplay)
                {
                    GenerateDependencyRelations(dependency, visited, sb, filter);
                }
            }
        }

        private string GetNodeId(string packageName)
        {
            return packageName.Replace(".", "_")
                             .Replace("-", "_")
                             .Replace(" ", "_")
                             .Replace("@", "_");
        }

        public async Task SaveAsPngAsync(string outputPath)
        {
            try
            {
                var factory = new RendererFactory();
                var renderer = factory.CreateRenderer(new PlantUmlSettings());

                // Рендерим в PNG
                byte[] imageData = await renderer.RenderAsync(plantUML, OutputFormat.Png);

                await File.WriteAllBytesAsync(outputPath, imageData);
                Console.WriteLine($"Граф успешно сохранен в файл: {outputPath}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при сохранении графа в PNG: {ex.Message}", ex);
            }
        }

        private static string EncodePlantUml(string text)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(text);
            byte[] compressed = CompressBytes(bytes);
            return Encode64(compressed);
        }

        private static byte[] CompressBytes(byte[] data)
        {
            using var memoryStream = new MemoryStream();

            // Указываем правильный уровень компрессии для совместимости с PlantUML
            using (var deflateStream = new DeflateStream(memoryStream, CompressionLevel.Optimal, true))
            {
                deflateStream.Write(data, 0, data.Length);
            }

            // PlantUML ожидает данные в определенном формате
            // Добавляем Zlib заголовок (RFC1950)
            var result = new byte[memoryStream.Length + 2];
            result[0] = 0x78; // Zlib header
            result[1] = 0x9C; // Zlib header (default compression)
            Buffer.BlockCopy(memoryStream.ToArray(), 0, result, 2, (int)memoryStream.Length);

            return result;
        }

        private static string Encode64(byte[] data)
        {
            // Специальная кодировка для PlantUML
            char[] encode6bit = new char[]
            {
        '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
        'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J',
        'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T',
        'U', 'V', 'W', 'X', 'Y', 'Z', 'a', 'b', 'c', 'd',
        'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n',
        'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x',
        'y', 'z', '-', '_'
            };

            StringBuilder result = new StringBuilder();
            for (int i = 0; i < data.Length; i += 3)
            {
                int b1 = data[i];
                int b2 = (i + 1 < data.Length) ? data[i + 1] : 0;
                int b3 = (i + 2 < data.Length) ? data[i + 2] : 0;

                int c1 = b1 >> 2;
                int c2 = ((b1 & 0x3) << 4) | (b2 >> 4);
                int c3 = ((b2 & 0xF) << 2) | (b3 >> 6);
                int c4 = b3 & 0x3F;

                result.Append(encode6bit[c1 & 0x3F]);
                result.Append(encode6bit[c2 & 0x3F]);
                result.Append(encode6bit[c3 & 0x3F]);
                result.Append(encode6bit[c4 & 0x3F]);
            }

            // Убираем лишние символы в конце
            int length = data.Length;
            if (length % 3 == 1)
            {
                result.Length -= 2;
            }
            else if (length % 3 == 2)
            {
                result.Length -= 1;
            }

            return result.ToString();
        }

        public void SavePlantUmlToFile(string filePath)
        {
            File.WriteAllText(filePath, plantUML, Encoding.UTF8);
            Console.WriteLine($"PlantUML код сохранен в файл: {filePath}");
        }
    }
}