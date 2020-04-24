using Phase3.Elements;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Phase3.Helpers
{

    public static class Functions
    {

        #region Functions

        public static string GetSolutionDir()
        {
            string dir = Directory.GetCurrentDirectory();
            int pos = dir.LastIndexOf("\\Phase3\\bin");
            dir = dir.Substring(0, pos);
            return dir;
        }

        public static string GetDataFile(string filename, string ext = "xml")
        {
            return GetSolutionDir() + "\\Phase3\\Data\\" + filename + "." + ext;
        }

        public static bool EveryPropertyExistsInClass(Type type, Dictionary<string, object> toSearch)
        {
            bool everyPropertyExistsInClass = true;
            foreach (KeyValuePair<string, object> item in toSearch) {
                if (type.GetProperty(item.Key) == null) {
                    everyPropertyExistsInClass = false;
                    break;
                }
            }
            return everyPropertyExistsInClass;
        }

        public static bool IsEmailValid(string email)
        {
            try {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            } catch {
                return false;
            }
        }

        public static bool IsPasswordValid(string password)
        {
            Regex r = new Regex("(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])");
            return r.IsMatch(password);
        }

        #endregion

    }
}
