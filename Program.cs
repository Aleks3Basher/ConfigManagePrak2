using ConfigManagePrak2.configuration;
using ConfigManagePrak2.dependency;
using ConfigManagePrak2.visual;

namespace ConfigManagePrak2
{
    partial class Program
    {
        public static async Task Main(string[] args)
        {
            try
            {
                InitialConfig.ParseFromArgs(args);

                MessageDriver.DisplayInitialConfig();

                Dependency dependencies = await DependencyService.GetPackageDependenciesAsync();

                var generator = new PlantUMLGraphGenerator(dependencies, InitialConfig.FilterSubstring);
                await generator.SaveAsPngAsync("repo_uml.png");
                generator.SavePlantUmlToFile("repo_uml_text.puml");
            }
            catch (Exception ex)
            {
                MessageDriver.DisplayError(ex.Message);
                MessageDriver.DisplayHelp();
                Environment.Exit(1);
            }
        }

    }
}