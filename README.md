# ConfigManagePrak2 Step 4

## 1. Общее описание

ConfigManagePrak2 - это консольное приложение на C# для анализа и визуализации графа зависимостей пакетов с поддержкой приоритетов загрузки.+

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
Сервис для получения зависимостей пакетов с рекурсивным обходом и расчетом приоритетов:

**Константы:**
- `MAX_DEPTH = 2` - максимальная глубина рекурсивного обхода зависимостей

**Методы:**
- `GetPackageDependenciesAsync()` - основной метод получения графа зависимостей
- `GetDependenciesFromTestRepository()` - построение графа из тестового файла
- `GetDependenciesFromNuGetRepositoryAsync()` - построение графа из NuGet репозитория
- `PutLocalDeps()` - рекурсивное построение графа для тестового репозитория с расчетом приоритетов
- `PutNugetDepends()` - рекурсивное построение графа для NuGet с расчетом приоритетов
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
- `DisplayDependencyGraph(Dependency graph, string filter)` - отображение древовидного графа зависимостей с приоритетами
- `DisplayPackageDependencies()` - вспомогательный метод для рекурсивного отображения

## 3. Команды для сборки проекта и запуска тестов

### Сборка проекта
```bash
dotnet build
```

## 4. Примеры использования
<img width="940" height="485" alt="image" src="https://github.com/user-attachments/assets/55f31a18-e3fa-44bb-a35c-1faa16f5de52" />
<img width="986" height="386" alt="image" src="https://github.com/user-attachments/assets/2398b4ab-bb65-4d40-9471-51cf822fd11c" />
<img width="410" height="293" alt="image" src="https://github.com/user-attachments/assets/22ca02ce-0130-4aab-b33e-2b50c6add51a" />

