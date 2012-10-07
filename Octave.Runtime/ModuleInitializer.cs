using System;
using Microsoft.Win32;

namespace Octave
{
    internal static class ModuleInitializer
    {

        public static void Run()
        {
           
            string libPath = null;

            
            if(String.IsNullOrEmpty(OctaveConfiguration.Default.OctaveLibraryPath))
            {
               
                using (var key = Registry.LocalMachine.OpenSubKey("Software\\Octave"))
                {
                    if (key != null)
                    {
                        using (var subKey = key.OpenSubKey(key.GetSubKeyNames()[0]))
                        {
                            if (subKey != null)
                            {
                                var octavePath = subKey.GetValue("InstallPath") as string;

                                if(!String.IsNullOrEmpty(octavePath))
                                libPath = String.Format("{0}\\bin\\", octavePath);
                            }
                        }
                    }
                }
            }
            else
            {
               
                libPath = OctaveConfiguration.Default.OctaveLibraryPath;
            }

            if (!String.IsNullOrEmpty(libPath))
            {
                
                Environment.SetEnvironmentVariable("PATH",
                                                   String.Format("{0};{1}", Environment.GetEnvironmentVariable("PATH"),
                                                                 libPath));
            }

        }
    }
}
