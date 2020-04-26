using Phase3.Core;
using Phase3.Core.Elements;
using Phase3.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phase3.Core.Models
{

    public class UsersModel : Model
    {

        private Type elementType = typeof(User);

        private static readonly string DEFAULT_DATA_FILE = Functions.GetDataFilePath("users");

        public bool Exists(string param = "id", object value = null)
        {
            Dictionary<string, object> search = new Dictionary<string, object>();
            search.Add(param, value);
            try {
                return XML.Find<User>(DEFAULT_DATA_FILE, typeof(User), search).Count() >= 1;
            } catch (Exception e) {
                Console.WriteLine(e.Message);
            }
            return false;
        }

        public void AddUser(User user)
        {
            try {
                XML.Add<User>(DEFAULT_DATA_FILE, user);
            } catch (Exception e) {
                Console.WriteLine(e.Message);
            }
        }

        public void RemakeUsersFile(List<User> users)
        {
            try {
                XML.Create<User>(DEFAULT_DATA_FILE, users);
            } catch (Exception e) {
                Console.WriteLine(e.Message);
            }
        }

        public List<User> GetAll()
        {
            try {
                return XML.GetAll<User>(DEFAULT_DATA_FILE);
            } catch (Exception e) {
                Console.WriteLine(e.Message);
            }
            return new List<User>();
        }

        public User GetUser(string field = "id", object value = null)
        {
            User toReturn = new User();
            if (typeof(User).GetProperty(field) == null) {
                Console.WriteLine("The parameter « " + field + " » doesn't exist in the class « User ».");
            } else {
                Dictionary<string, object> search = new Dictionary<string, object> {
                    { field, value }
                };
                try {
                    List<User> results = XML.Find<User>(DEFAULT_DATA_FILE, typeof(User), search);
                    if (results.Count() == 1) {
                        toReturn = results[0];
                    } else if (results.Count() > 1) {
                        List<Constraint> usersConstraints = Constraints.WakeUp().GetDataFileConstraints("users");
                        List<Constraint> usersUniqueConstraints = Constraints.WakeUp().GetConstraintsOfType(ConstraintsTypes.UNIQUE, usersConstraints);
                        foreach (Constraint constraint in usersUniqueConstraints) {
                            if (constraint.Field == field) {
                                Console.WriteLine("The file « users » is corrupted. Two rows have the same value on the unique field « " + field + " ».");
                                break;
                            }
                        }
                    }
                } catch (Exception e) {
                    Console.WriteLine(e.Message);
                }
            }
            return toReturn;
        }

    }
}
