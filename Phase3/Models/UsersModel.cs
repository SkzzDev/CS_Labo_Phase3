using Phase3.Elements;
using Phase3.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phase3.Models
{

    public class UsersModel : Model
    {

        private Type elementType = typeof(User);

        private static readonly string DEFAULT_DATA_FILE = Functions.GetDataFile("users");

        public bool Exists(string param = "id", object value = null)
        {
            Dictionary<string, object> search = new Dictionary<string, object>();
            search.Add(param, value);
            try {
                return XMLHelper.Find<User>(DEFAULT_DATA_FILE, typeof(User), search).Count() >= 1;
            } catch (Exception e) {
                Console.WriteLine(e.Message);
            }
            return false;
        }

        public void AddUser(User user)
        {
            try {
                bool uniqueFieldsOk = true;
                XMLHelper.VerifyIntegrity(DEFAULT_DATA_FILE, user);
                foreach (string field in user.UniquesFields) {
                    Dictionary<string, object> search = new Dictionary<string, object>();
                    search.Add(field, typeof(User).GetProperty(field).GetValue(user));
                    List<User> results = XMLHelper.Find<User>(DEFAULT_DATA_FILE, typeof(User), search);
                    if (results.Count() >= 1) {
                        uniqueFieldsOk = false;
                        Console.WriteLine("Couldn't add the user, another one already exists with the same " + field + ".");
                    }
                }
                if (uniqueFieldsOk) {
                    XMLHelper.Add<User>(DEFAULT_DATA_FILE, user);
                }
            } catch (Exception e) {
                Console.WriteLine(e.Message);
            }
        }

        public void RemakeUsersFile(List<User> users)
        {
            try {
                XMLHelper.Create<User>(DEFAULT_DATA_FILE, users);
            } catch (Exception e) {
                Console.WriteLine(e.Message);
            }
        }

        public List<User> GetAll()
        {
            try {
                return XMLHelper.GetAll<User>(DEFAULT_DATA_FILE);
            } catch (Exception e) {
                Console.WriteLine(e.Message);
            }
            return new List<User>();
        }

        public User GetUser(string param = "id", object value = null)
        {
            User toReturn = new User();
            Dictionary<string, object> search = new Dictionary<string, object>();
            search.Add(param, value);
            try {
                List<User> results = XMLHelper.Find<User>(DEFAULT_DATA_FILE, typeof(User), search);
                if (results.Count() > 1) {
                    Console.WriteLine("Couldn't return a single user, " + results.Count().ToString() + " users found with the search made.");
                } else if (results.Count() == 0) {
                    Console.WriteLine("No user found with the search made.");
                } else {
                    toReturn = results[0];
                }
            } catch (Exception e) {
                Console.WriteLine(e.Message);
            }
            return toReturn;
        }

    }
}
