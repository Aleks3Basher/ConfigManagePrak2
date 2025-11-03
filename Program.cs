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
                var config = new InitialConfig();
                config.ParseFromArgs(args);

                MessageDriver.DisplayInitialConfig(config);

                var dependencies = await DependencyService.GetPackageDependenciesAsync(config);

                MessageDriver.DisplayDependencies(config, dependencies);
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