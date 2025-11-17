# ConfigManagePrak2 Step 1

## 1. Общее описание

ConfigManagePrak2 - это консольное приложение на C# для управления конфигурацией анализа зависимостей пакетов.

## 2. Описание всех функций и настроек

### Класс `InitialConfig`

Основной класс конфигурации, содержащий следующие свойства:

- **PackageName** (`string`) - имя анализируемого пакета (обязательный параметр)
- **RepositoryUrl** (`string`) - URL репозитория или путь к файлу тестового репозитория (обязательный параметр)
- **UseTestRepository** (`bool`) - флаг режима работы с тестовым репозиторием (по умолчанию: `false`)
- **OutputFileName** (`string`) - имя сгенерированного файла с изображением графа (по умолчанию: `"dependency_graph.png"`)
- **FilterSubstring** (`string`) - подстрока для фильтрации пакетов (по умолчанию: пустая строка)

#### Методы:
- **ParseFromArgs(string[] args)** - парсит аргументы командной строки и заполняет свойства конфигурации

### Статический класс `MessageDriver`

Класс для вывода информации в консоль:

#### Методы:
- **DisplayInitialConfig(InitialConfig config)** - отображает текущую конфигурацию
- **DisplayError(string errorMessage)** - отображает сообщение об ошибке
- **DisplayHelp()** - отображает справку по использованию приложения

## 3. Команды для сборки проекта и запуска тестов

### Сборка проекта
```bash
dotnet build
```

## 4. Примеры использования
<img width="1135" height="144" alt="image" src="https://github.com/user-attachments/assets/7581ac69-befd-44a5-8e82-083e13d9ddf6" />
<img width="1010" height="186" alt="image" src="https://github.com/user-attachments/assets/63aea60e-22b1-4086-9952-5acc753e968e" />


