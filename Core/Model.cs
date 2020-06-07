using Core.Elements.Interfaces;
using Core.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{

    public abstract class Model
    {

        protected string DataFile = "";

        public void Add<T>(T t) where T : IXMLSavable
        {
            try {
                XML.Add<T>(DataFile, t);
            } catch (Exception) {
                throw;
            }
        }

        public void Delete<T>(Dictionary<string, object> conditions) where T : IXMLSavable
        {
            try {
                XML.Delete<T>(DataFile, conditions);
            } catch (Exception) {
                throw;
            }
        }

        public void Update<T>(T t, Dictionary<string, object> conditions) where T : IXMLSavable
        {
            XML.Update<T>(DataFile, t, conditions);
        }

        public bool Exists<T>(string field = "Id", object value = null) where T : IXMLSavable
        {
            if (typeof(T).GetProperty(field) == null) {
                Logs.Write("The parameter « " + field + " » doesn't exist in the class « " + typeof(T).ToString() + " ».");
            } else {
                Dictionary<string, object> search = new Dictionary<string, object>();
                search.Add(field, value);
                try {
                    return XML.Find<T>(DataFile, search).Count() >= 1;
                } catch (Exception) {
                    throw;
                }
            }
            return false;
        }

        public ObservableCollection<T> GetAll<T>() where T : IXMLSavable
        {
            ObservableCollection<T> results = new ObservableCollection<T>();
            try {
                results = new ObservableCollection<T>(XML.GetAll<T>(DataFile));
                Functions.Sort<T>(results);
            } catch (Exception e) {
                Logs.Write(e.Message);
            }
            return results;
        }

    }

}
