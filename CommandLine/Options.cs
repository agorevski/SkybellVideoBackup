using CommandLine;

namespace BackupVideos
{
    internal class Options
    {
        [Option('o', "outputDir", HelpText = "The base output directory to save the arlo files", Required = true)]
        public string OutputDir { get; set; }

        [Option('u', "username", HelpText = "Your login username", Required = true)]
        public string Username { get; set; }

        [Option('p', "password", HelpText = "Your login password", Required = true)]
        public string Password { get; set; }
    }
}
