# ConfigManagePrak2 Step 2

## 1. Общее описание

ConfigManagePrak2 - это консольное приложение на C# для анализа зависимостей пакетов.

## 2. Описание всех функций и настроек

### Пространство имен `ConfigManagePrak2.configuration`

#### Класс `InitialConfig`
Основной класс конфигурации приложения:

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
Модель зависимости пакета:

**Свойства:**
- `Name` (string) - имя пакета
- `Version` (string) - версия пакета
- `Dependencies` (List<Dependency>) - список зависимостей

#### Статический класс `DependencyService`
Сервис для получения зависимостей пакетов:

**Методы:**
- `GetPackageDependenciesAsync(InitialConfig config)` - основной метод получения зависимостей
- `GetDependenciesFromTestRepository(InitialConfig config)` - чтение из тестового файла
- `GetDependenciesFromNuGetRepositoryAsync(InitialConfig config)` - получение из NuGet репозитория
- `ParseDependencies(string xml)` - парсинг XML с зависимостями
- `GetXmlFromNuget(InitialConfig config, HttpClient client, string package, string version)` - получение .nuspec файла
- `GetNugetPackageVersion(InitialConfig config, HttpClient client, string package)` - получение последней версии пакета

### Пространство имен `ConfigManagePrak2.visual`

#### Статический класс `MessageDriver`
Класс для вывода информации в консоль:

**Методы:**
- `DisplayInitialConfig(InitialConfig config)` - отображение конфигурации
- `DisplayError(string errorMessage)` - отображение ошибок
- `DisplayHelp()` - отображение справки
- `DisplayDependencies(InitialConfig config, List<Dependency> dependencies)` - отображение списка зависимостей

## 3. Команды для сборки проекта и запуска тестов

### Сборка проекта
```bash
dotnet build
```

## 4. Примеры использования

<img width="989" height="283" alt="image" src="https://github.com/user-attachments/assets/f9096c6e-1613-4631-b050-f31902fd653b" />
<img width="967" height="251" alt="image" src="https://github.com/user-attachments/assets/549b475c-7638-4193-9b04-93eece598439" />
<img width="223" height="184" alt="image" src="https://github.com/user-attachments/assets/7b67674f-38fe-4658-ac66-37e4116edf43" />


