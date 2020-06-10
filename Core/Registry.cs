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

        public static RegistryKey GetUserRegistry(string userId = null)
        {
            if (userId == null)
                userId = USER_ID; // Default = connected user
            if (!userId.Equals("")) {
                RegistryKey SRA = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("Software").CreateSubKey("SRA", true);
                RegistryKey userRegistry = SRA.OpenSubKey(userId, true);

                // Create user's registry if it doesn't exist
                if (userRegistry == null) {
                    userRegistry = SRA.CreateSubKey(userId, true);

                    // Create the keys we need the user to have
                    userRegistry.SetValue("XmlsPath", Functions.GetInitialXmlsPath());
                }

                return userRegistry;
            } else {
                Console.WriteLine("The parameter « userId » cannot be empty.");
                return null;
            }
        }

        public static void MoveUserRegistry(string oldUserId, string newUserId)
        {
            RegistryKey SRA = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("Software").CreateSubKey("SRA", true);

            // Create new user registry
            RegistryKey newUserRegistry = SRA.CreateSubKey(newUserId, true);

            // Assign old registry values
            newUserRegistry.SetValue("XmlsPath", GetXmlsPath(oldUserId));

            SRA.DeleteSubKeyTree(oldUserId);
        }

        public static void DeleteUserRegistry(string userId)
        {
            RegistryKey userRegistry = GetUserRegistry(userId);
            if (userRegistry != null) {
                RegistryKey SRA = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("Software").CreateSubKey("SRA", true);
                SRA.DeleteSubKeyTree(userId);
            }
        }

        public static string GetXmlsPath(string userId = null)
        {
            if (userId == null)
                userId = USER_ID; // Default = connected user
            RegistryKey userRegistry = GetUserRegistry(userId);
            if (userRegistry != null)
                return (string)userRegistry.GetValue("XmlsPath");
            return "";
        }

        public static void SetXmlsPath(string newPath, string userId = null)
        {
            if (userId == null)
                userId = USER_ID; // Default = connected user
            RegistryKey userRegistry = GetUserRegistry(userId);
            if (userRegistry != null)
                userRegistry.SetValue("XmlsPath", newPath);
        }

    }
}
