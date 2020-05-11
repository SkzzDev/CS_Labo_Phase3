using Core.Elements;
using Core.Elements.Interfaces;
using Core.Exceptions.XML;
using Core.Helpers;
using System;
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

namespace Core
{

    public static class XML
    {

        #region Functions

        public static void Create<T>(string filename, List<T> objList, bool checkConstraintsInlist = true) where T : IXMLSavable
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
                    if (checkConstraintsInlist)
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

        public static void Add<T>(string filename, T obj) where T : IXMLSavable
        {
            if (obj.IsSavable()) {
                try {
                    VerifyConstraints<T>(filename, obj);
                    List<T> items = GetAll<T>(filename);
                    items.Add((T)obj);
                    Create<T>(filename, items, false);
                } catch (Exception) {
                    throw;
                }
            } else {
                string errorFields = "";
                foreach (KeyValuePair<string, string> item in obj.GetInvalidFields())
                    errorFields += "[" + item.Key + "] " + item.Value;
                throw new XMLException("This object couldn't be serialized because there are errors inside of it. Fields with errors: " + errorFields);
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

        public static void Delete<T>(string filename, T obj) where T : IXMLSavable
        {
            try {
                List<T> all = GetAll<T>(filename);
                List<T> toRemove = new List<T>();
                foreach (T t in all) {
                    if (t.Equals(obj)) {
                        toRemove.Add(t);
                    }
                }
                foreach (T t in toRemove)
                    all.Remove(t);
                Create<T>(filename, all);
            } catch (Exception) {
                throw;
            }
        }

        public static void Update<T>(string filename, T newObj, Dictionary<string, object> conditions) where T : IXMLSavable
        {
            Type type = typeof(T);
            if (Functions.EveryPropertyExistsInClass(type, conditions)) {
                try {
                    List<T> all = GetAll<T>(filename);
                    foreach (T itemToCheck in all) {
                        if (itemToCheck.GetType() == type) {
                            bool currentElementIsOk = true;
                            foreach (KeyValuePair<string, object> condition in conditions) {
                                if (!type.GetProperty(condition.Key).GetValue(itemToCheck).Equals(condition.Value)) {
                                    currentElementIsOk = false;
                                    break;
                                }
                            }
                            if (currentElementIsOk)
                                itemToCheck.Hydrate(newObj);
                        }
                    }
                    Create<T>(filename, all);
                } catch (Exception) {
                    throw;
                }
            }
        }

        public static List<T> Find<T>(string filename, Dictionary<string, object> conditions) where T : IXMLSavable
        {
            List<T> toReturn = new List<T>();
            Type type = typeof(T);
            if (Functions.EveryPropertyExistsInClass(type, conditions)) {
                try {
                    List<T> all = GetAll<T>(filename);
                    foreach (object itemToCheck in all) {
                        if (itemToCheck.GetType() == type) {
                            bool currentElementIsOk = true;
                            foreach (KeyValuePair<string, object> condition in conditions) {
                                if (!type.GetProperty(condition.Key).GetValue(itemToCheck).Equals(condition.Value)) {
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
                    object elemToVerifyValue = typeof(T).GetProperty(constraint.Field).GetValue(elemToVerify);
                    Dictionary<string, object> search = new Dictionary<string, object>();
                    search.Add(constraint.Field, elemToVerifyValue);
                    List<T> results = Find<T>(filename, search);
                    if (results.Count() >= 1)
                        throw new ValueInUniqueFieldAlreadyTaken("A row already exists with the field « " + constraint.Field + " » containing the value « " + elemToVerifyValue.ToString() + " ». Insertion in file « " + constraint.DataFile + ".xml » have been cancelled.");
                }
            }
        }

        // USED WHEN CREATING A NEW FILE, IN ORDER TO CHECK ALL THAT NO ITEMS OF THE LIST ARE GOING AGAINST THE CONSTRAINTS OF THE FILE
        public static void VerifyConstraintsInList<T>(string filename, List<T> listToVerify) where T : IXMLSavable
        {
            List<Constraint> constraintsToCheck = Constraints.WakeUp().GetDataFileConstraints(filename);
            if (constraintsToCheck.Count() > 0 && listToVerify.Count() > 1) {
                List<T> listToCheck = new List<T>(); // List where current T element will need to check if it is ok or not
                Type t = typeof(T);
                foreach (T elemToVerify in listToVerify) {
                    foreach (T elemToCheck in listToCheck) {
                        foreach (Constraint constraint in constraintsToCheck) {
                            if (constraint.Type == ConstraintsTypes.UNIQUE) {
                                PropertyInfo property = t.GetProperty(constraint.Field);
                                if (property.GetValue(elemToVerify).Equals(property.GetValue(elemToCheck)))
                                    throw new ValueInUniqueFieldAlreadyTaken("A row already exists containing the value « " + property.GetValue(elemToVerify).ToString() + " » in the field « " + constraint.Field + " ». Insertions in the file « " + constraint.DataFile + ".xml » have been cancelled.");
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
