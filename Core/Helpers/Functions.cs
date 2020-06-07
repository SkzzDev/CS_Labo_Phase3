using Core.Elements;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Core.Helpers
{

    public static class Functions
    {

        #region Functions

        public static string GetSolutionDirPath()
        {
            string startUpProject = "Phase3";
            string dir = Directory.GetCurrentDirectory();
            int pos = dir.LastIndexOf("\\" + startUpProject + "\\bin");
            dir = dir.Substring(0, pos);
            return dir;
        }

        public static string GetDataFilePath(string filename, string ext = "xml")
        {
            return GetSolutionDirPath() + "\\Data\\Data\\" + filename + "." + ext;
        }

        public static string GetConstaintsFilePath()
        {
            return GetSolutionDirPath() + "\\Data\\Constraints\\constraints.xml";
        }

        public static string GetLogsPath()
        {
            return GetSolutionDirPath() + "\\Data\\Logs\\logs.txt";
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

        // Not tested yet
        public static List<T> ExtractionSort<T>(List<T> tElements, string onProperty = "Id")
        {
            if (HasProperty(typeof(T), onProperty)) {
                PropertyInfo property = typeof(T).GetProperty(onProperty);
                if (property.PropertyType is IComparable comparableProperty) {
                    int iMax = 0;
                    for (int len = tElements.Count(); len > 1; len--) {
                        for (int i = 1; i < len; i++) {
                            property.GetValue(tElements[i]);
                            if (((IComparable)property.GetValue(tElements[i])).CompareTo((IComparable)property.GetValue(tElements[i])) > 0) iMax = i;
                        }
                        T temp = tElements[len - 1];
                        tElements[len - 1] = tElements[iMax];
                        tElements[iMax] = temp;
                    }
                }
            }
            return tElements;
        }

        public static bool HasProperty(this Type obj, string propertyName)
        {
            return obj.GetProperty(propertyName) != null;
        }

        public static void Sort<T>(this ObservableCollection<T> collection) where T : IComparable
        {
            List<T> sorted = collection.OrderBy(x => x).ToList();
            collection.Clear();
            for (int i = 0; i < sorted.Count(); i++) {
                collection.Add(sorted[i]);
            }
        }

        #endregion

    }

}
