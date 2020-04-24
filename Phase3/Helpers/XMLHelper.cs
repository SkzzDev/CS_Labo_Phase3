using Phase3.Elements;
using Phase3.Elements.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Phase3.Helpers
{

    public class XMLHelperException : Exception
    {

        public XMLHelperException(string message) : base(message) { }

    }

    public static class XMLHelper
    {

        #region Functions

        public static void Create<T>(string filename, List<T> objList)
        {
            bool areAllSavable = true;
            foreach (IXMLSavable obj in objList) {
                if (!obj.IsSavable()) {
                    areAllSavable = false;
                    break;
                }
            }
            if (!areAllSavable) {
                throw new XMLHelperException("This list couldn't be serialized because there are objects that contain errors inside of them.");
            } else {
                try {
                    List<T> list = objList as List<T>;
                    XmlSerializer xmlFormat = new XmlSerializer(typeof(List<T>));
                    using (Stream fStream = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None)) {
                        xmlFormat.Serialize(fStream, list);
                    }
                } catch (Exception) {
                    throw;
                }
            }
        }

        public static void Add<T>(string filename, IXMLSavable obj)
        {
            if (obj is IXMLSavable objSavable) {
                if (obj.IsSavable()) {
                    try {
                        List<T> items = GetAll<T>(filename);
                        items.Add((T)objSavable);
                        Create(filename, items);
                    } catch (Exception) {
                        throw;
                    }
                } else {
                    foreach (KeyValuePair<string, string> item in objSavable.GetInvalidFields()) {
                        Console.WriteLine("[{0}] {1}", item.Key, item.Value);
                    }
                    throw new XMLHelperException("This object couldn't be serialized because there are errors inside of it.");
                }
            } else {
                throw new XMLHelperException("This object couldn't be verified for serialization.");
            }
        }

        public static List<T> GetAll<T>(string filename)
        {
            List<T> toReturn = new List<T>();
            XmlSerializer xmlFormat = new XmlSerializer(typeof(List<T>));
            try {
                if (new FileInfo(filename).Length != 0) {
                    using (Stream fStream = File.OpenRead(filename)) {
                        toReturn = (List<T>)xmlFormat.Deserialize(fStream);
                    }
                }
            } catch (Exception) {
                throw;
            }
            return toReturn;
        }

        public static List<T> Find<T>(string filename, Type type, Dictionary<string, object> toSearch)
        {
            List<T> toReturn = new List<T>();
            if (Functions.EveryPropertyExistsInClass(type, toSearch)) {
                try {
                    List<T> items = GetAll<T>(filename);
                    foreach (object itemToCheck in items) {
                        if (itemToCheck.GetType() == type) {
                            bool currentElementIsOk = true;
                            foreach (KeyValuePair<string, object> itemToSearch in toSearch) {
                                if (!type.GetProperty(itemToSearch.Key).GetValue(itemToCheck).Equals(itemToSearch.Value)) {
                                    currentElementIsOk = false;
                                    break;
                                }
                            }
                            if (currentElementIsOk)
                                toReturn.Add((T)itemToCheck);
                        }
                    }
                } catch (Exception) {
                    throw;
                }
            }
            return toReturn;
        }

        public static void VerifyIntegrity(string filename, IXMLSavable toVerify)
        {
            foreach (string field in toVerify.UniquesFields) {
                Dictionary<string, object> search = new Dictionary<string, object>();
                search.Add(field, toVerify.GetType().GetProperty(field).GetValue(toVerify));
                List<User> results = Find<User>(filename, typeof(User), search);
                if (results.Count() >= 1)
                    throw new XMLHelperException("Field " + field + " already exists with the same value.");
            }
        }

        #endregion

    }
}
