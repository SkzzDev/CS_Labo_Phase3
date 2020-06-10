using Core.Helpers;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class Registry
    {

        public static string USER_ID = "";

        public static RegistryKey GetUserRegistry()
        {
            if (!USER_ID.Equals("")) {
                RegistryKey SRA = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("Software").CreateSubKey("SRA", true);
                RegistryKey userRegistry = SRA.OpenSubKey(USER_ID, true);

                // Create user's registry if it doesn't exist
                if (userRegistry == null) {
                    userRegistry = SRA.CreateSubKey(USER_ID, true);

                    // Create the keys we need the user to have
                    userRegistry.SetValue("XmlsPath", Functions.GetInitialXmlsPath());
                }

                return userRegistry;
            } else {
                Console.WriteLine("No user connected. Cannot use registry functionalities.");
                return null;
            }
        }

        public static string GetXmlsPath()
        {
            RegistryKey userRegistry = GetUserRegistry();
            if (userRegistry != null)
                return (string)userRegistry.GetValue("XmlsPath");
            return "";
        }

        public static void SetXmlsPath(string newPath)
        {
            RegistryKey userRegistry = GetUserRegistry();
            if (userRegistry != null)
                userRegistry.SetValue("XmlsPath", newPath);
        }

    }
}
