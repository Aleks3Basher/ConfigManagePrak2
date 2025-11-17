# ConfigManagePrak2 Step 3

## 1. Общее описание

ConfigManagePrak2 - это консольное приложение на C# для анализа и визуализации графа зависимостей пакетов.+

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
Модель зависимости пакета с поддержкой вложенных зависимостей:

**Свойства:**
- `Name` (string) - имя пакета
- `Version` (string) - версия пакета
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
- `DisplayDependencyGraph(Dependency graph, string filter)` - отображение древовидного графа зависимостей
- `DisplayPackageDependencies()` - вспомогательный метод для рекурсивного отображения

## 3. Команды для сборки проекта и запуска тестов

### Сборка проекта
```bash
dotnet build
```

## 4. Примеры использования

<img width="1065" height="782" alt="image" src="https://github.com/user-attachments/assets/8ac3bef8-5ec6-42dd-89fb-286118c3c613" />
<img width="967" height="399" alt="image" src="https://github.com/user-attachments/assets/02d450cf-7383-4b6d-a5ec-29698ba27e96" />
<img width="450" height="253" alt="image" src="https://github.com/user-attachments/assets/e2087069-04a0-4df5-a4e8-e4e76e21df44" />

