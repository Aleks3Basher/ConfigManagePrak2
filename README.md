# ConfigManagePrak2 Step 5

## 1. Общее описание

ConfigManagePrak2 - это консольное приложение на C# для анализа зависимостей пакетов и генерации визуальных графов с использованием PlantUML.

## 2. Описание всех функций и настроек

### Пространство имен `ConfigManagePrak2.configuration`

#### Статический класс `InitialConfig`
Статический класс конфигурации приложения:

**Свойства:**
- `PackageName` (string) - имя анализируемого пакета (обязательный)
- `RepositoryUrl` (string) - URL репозитория или путь к тестовому файлу (обязательный)
- `UseTestRepository` (bool) - режим тестового репозитория (по умолчанию: false)
- `OutputFileName` (string) - имя выходного файла (по умолчанию: "dependency_graph.png")
- `FilterSubstring` (string) - подстрока для фильтрации пакетов

**Методы:**
- `ParseFromArgs(string[] args)` - парсит аргументы командной строки

### Пространство имен `ConfigManagePrak2.dependency`

#### Класс `Dependency`
Модель зависимости пакета с поддержкой приоритетов загрузки:

**Свойства:**
- `Name` (string) - имя пакета
- `Version` (string) - версия пакета
- `LoadPriority` (int) - приоритет загрузки (уровень в дереве зависимостей)
- `Dependencies` (List<Dependency>) - список вложенных зависимостей

#### Класс `LocalRepository`
Класс для работы с тестовыми репозиториями:

**Свойства:**
- `Repo` (Dictionary<string, Dictionary<string, string>>) - словарь пакетов и их зависимостей

**Методы:**
- `At(string file)` - создает репозиторий из файла

#### Статический класс `DependencyService`
Сервис для получения зависимостей пакетов с рекурсивным обходом:

**Константы:**
- `MAX_DEPTH = 2` - максимальная глубина рекурсивного обхода зависимостей

**Методы:**
- `GetPackageDependenciesAsync()` - основной метод получения графа зависимостей
- `GetDependenciesFromTestRepository()` - построение графа из тестового файла
- `GetDependenciesFromNuGetRepositoryAsync()` - построение графа из NuGet репозитория
- `PutLocalDeps()` - рекурсивное построение графа для тестового репозитория
- `PutNugetDepends()` - рекурсивное построение графа для NuGet
- `GetPackageDepends()` - получение зависимостей конкретного пакета
- `GetXmlFromNuget()` - получение .nuspec файла
- `GetNugetPackageVersion()` - получение последней версии пакета

### Пространство имен `ConfigManagePrak2.visual`

#### Статический класс `MessageDriver`
Класс для вывода информации в консоль:

**Методы:**
- `DisplayInitialConfig()` - отображение конфигурации
- `DisplayError(string errorMessage)` - отображение ошибок
- `DisplayHelp()` - отображение справки
- `DisplayDependencyGraph(Dependency graph, string filter)` - отображение древовидного графа в консоли
- `DisplayPackageDependencies()` - вспомогательный метод для рекурсивного отображения

#### Класс `PlantUMLGraphGenerator`
Класс для генерации визуальных графов с использованием PlantUML:

**Методы:**
- `PlantUMLGraphGenerator(Dependency root, string filter)` - конструктор с генерацией PlantUML кода
- `GeneratePlantUML()` - генерирует PlantUML код из графа зависимостей
- `GeneratePackageNodes()` - создает узлы пакетов в PlantUML
- `GenerateDependencyRelations()` - создает связи между пакетами
- `GetNodeId()` - преобразует имя пакета в идентификатор узла
- `SaveAsPngAsync(string outputPath)` - сохраняет граф как PNG изображение
- `SavePlantUmlToFile(string filePath)` - сохраняет исходный PlantUML код в файл
- `EncodePlantUml()` - кодирует текст для PlantUML
- `CompressBytes()` - сжимает данные для кодирования
- `Encode64()` - выполняет base64 кодирование для PlantUML

## 3. Команды для сборки проекта и запуска тестов

### Сборка проекта
```bash
dotnet build
```

## 4. Примеры использования
<img width="988" height="265" alt="image" src="https://github.com/user-attachments/assets/bc2a5382-9d6e-4807-9286-5ef062efbc7c" />
<img width="369" height="214" alt="image" src="https://github.com/user-attachments/assets/099109ac-f815-443e-9aa5-4e872ecac113" />

<img width="952" height="281" alt="image" src="https://github.com/user-attachments/assets/77d96b5f-8b27-46a6-aec6-7308ec08eeee" />
<img width="3370" height="565" alt="repo_uml" src="https://github.com/user-attachments/assets/17a272b6-18b2-4e07-a74b-6fec1c0f5189" />
<img width="1506" height="892" alt="image" src="https://github.com/user-attachments/assets/13b7c40d-85d7-4ac1-8540-4d27250a40cc" />

