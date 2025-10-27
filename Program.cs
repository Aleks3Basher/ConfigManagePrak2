using ConfigManagePrak2.configuration;
using ConfigManagePrak2.visual;

namespace ConfigManagePrak2
{
    partial class Program
    {

        public static void Main(string[] args)
        {
            try
            {
                var config = new InitialConfig();
                config.ParseFromArgs(args);

                MessageDriver.DisplayInitialConfig(config);
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