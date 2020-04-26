using Phase3.Core;
using Phase3.Core.Elements;
using Phase3.Core.Elements.Interfaces;
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

namespace Phase3.Core.Helpers
{

    public class XMLException : Exception
    {

        public XMLException(string message) : base(message) { }

    }

    public static class XML
    {

        #region Functions

        public static void Create<T>(string filename, List<T> objList) where T : IXMLSavable
        {
            bool areAllSavable = true;
            foreach (IXMLSavable obj in objList) {
                if (!obj.IsSavable()) {
                    areAllSavable = false;
                    break;
                }
            }
            if (!areAllSavable) {
                throw new XMLException("This list couldn't be serialized because there are objects that contain errors inside of them.");
            } else {
                try {
                    VerifyConstraintsInList<T>(filename, objList);
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

        public static void Add<T>(string filename, IXMLSavable obj) where T : IXMLSavable
        {
            if (obj is IXMLSavable objSavable) {
                if (obj.IsSavable()) {
                    try {
                        XML.VerifyConstraints<T>(filename, obj);
                        List<T> items = GetAll<T>(filename);
                        items.Add((T)objSavable);
                        Create(filename, items);
                    } catch (Exception) {
                        throw;
                    }
                } else {
                    string errorFields = "";
                    foreach (KeyValuePair<string, string> item in objSavable.GetInvalidFields())
                        errorFields += "[" + item.Key + "] " + item.Value;
                    throw new XMLException("This object couldn't be serialized because there are errors inside of it. Fields with errors: " + errorFields);
                }
            } else {
                throw new XMLException("This object couldn't be verified for serialization.");
            }
        }

        public static List<T> GetAll<T>(string filename) where T : IXMLSavable
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

        public static List<T> Find<T>(string filename, Type type, Dictionary<string, object> toSearch) where T : IXMLSavable
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

        // USED WHEN ADDING ONE ELEMENT TO A FILE, ON ORDER TO CHECK THAT IT DOESNT GO AGAINST THE CONSTRAINTS OF THE FILE
        public static void VerifyConstraints<T>(string filename, IXMLSavable elemToVerify) where T : IXMLSavable
        {
            foreach (Constraint constraint in Constraints.WakeUp().GetDataFileConstraints(filename)) {
                if (constraint.Type == ConstraintsTypes.UNIQUE) {
                    object elemToVerifyValue = elemToVerify.GetType().GetProperty(constraint.Field).GetValue(elemToVerify);
                    Dictionary<string, object> search = new Dictionary<string, object>();
                    search.Add(constraint.Field, elemToVerifyValue);
                    List<T> results = Find<T>(filename, typeof(T), search);
                    if (results.Count() >= 1)
                        throw new XMLException("A row already exists with the field « " + constraint.Field + " » containing the value « " + elemToVerifyValue.ToString() + " ». In file « " + constraint.DataFile + ".xml ».");
                }
            }
        }

        // USED WHEN CREATING A NEW FILE, IN ORDER TO CHECK ALL THAT NO ITEMS OF THE LIST ARE GOING AGAINST THE CONSTRAINTS OF THE FILE
        public static void VerifyConstraintsInList<T>(string filename, List<T> listToVerify) where T : IXMLSavable
        {
            List<Constraint> constraintsToCheck = Constraints.WakeUp().GetDataFileConstraints(filename);
            if (constraintsToCheck.Count() > 0 && listToVerify.Count() > 1) {
                List<T> listToCheck = new List<T>(); // List where current T element will need to check if it is ok or not
                foreach (T elemToVerify in listToVerify) {
                    foreach (T elemToCheck in listToCheck) {
                        foreach (Constraint constraint in constraintsToCheck) {
                            if (constraint.Type == ConstraintsTypes.UNIQUE) {
                                object elemToVerifyValue = elemToVerify.GetType().GetProperty(constraint.Field).GetValue(elemToVerify);
                                object elemToCheckValue = elemToCheck.GetType().GetProperty(constraint.Field).GetValue(elemToCheck);
                                if (elemToVerifyValue.Equals(elemToCheckValue))
                                    throw new XMLException("A row already exists containing the value « " + elemToVerifyValue.ToString() + " » in the field « " + constraint.Field + " ». Insertions in the file « " + constraint.DataFile + ".xml » have been cancelled.");
                            }
                        }
                    }
                    listToCheck.Add(elemToVerify);
                }
            }
        }

        #endregion

    }
}
