using System.Configuration;

namespace Octave
{
    internal class OctaveConfiguration : ConfigurationSection
    {
        private static readonly OctaveConfiguration _instance =
            ConfigurationManager.GetSection("octave") as OctaveConfiguration ?? new OctaveConfiguration();

        public static OctaveConfiguration Default
        {
            get { return _instance; }
        }

        private OctaveConfiguration()
        {
        }

        [ConfigurationProperty("lib", IsRequired = false),]
        public string OctaveLibraryPath
        {
            get { return this["lib"] as string; }
        }

        [ConfigurationProperty("args", IsRequired = false)]
        public string RuntimeArguments
        {
            get { return this["args"] as string; }
        }
    }
}