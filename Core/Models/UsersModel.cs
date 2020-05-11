using Core;
using Core.Elements;
using Core.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{

    public class UsersModel : Model
    {

        private string DataFile = Functions.GetDataFilePath("users");

        public UsersModel()
        {
            if (!File.Exists(DataFile))
                XML.Create<User>(DataFile, new List<User>());
        }

        public bool Exists(string field = "Id", object value = null)
        {
            if (typeof(User).GetProperty(field) == null) {
                Logs.Write("The parameter « " + field + " » doesn't exist in the class « User ».");
            } else {
                Dictionary<string, object> search = new Dictionary<string, object>();
                search.Add(field, value);
                try {
                    return XML.Find<User>(DataFile, typeof(User), search).Count() >= 1;
                } catch (Exception) {
                    throw;
                }
            }
            return false;
        }

        public void AddUser(User user)
        {
            try {
                XML.Add<User>(DataFile, user);
            } catch (Exception) {
                throw;
            }
        }

        public void RemakeUsersFile(List<User> users)
        {
            try {
                XML.Create<User>(DataFile, users);
            } catch (Exception) {
                throw;
            }
        }

        public List<User> GetAll()
        {
            try {
                return XML.GetAll<User>(DataFile);
            } catch (Exception) {
                throw;
            }
        }

        public User GetUser(string field = "Id", object value = null)
        {
            User toReturn = new User();
            if (typeof(User).GetProperty(field) == null) {
                Logs.Write("The parameter « " + field + " » doesn't exist in the class « User ».");
            } else {
                Dictionary<string, object> search = new Dictionary<string, object> {
                    { field, value }
                };
                try {
                    List<User> results = XML.Find<User>(DataFile, typeof(User), search);
                    if (results.Count() == 1) {
                        toReturn = results[0];
                    } else if (results.Count() > 1) {
                        List<Constraint> usersConstraints = Constraints.WakeUp().GetDataFileConstraints("users");
                        List<Constraint> usersUniqueConstraints = Constraints.WakeUp().GetConstraintsOfType(ConstraintsTypes.UNIQUE, usersConstraints);
                        foreach (Constraint constraint in usersUniqueConstraints) {
                            if (constraint.Field.Equals(field)) {
                                Logs.Write("The file « users.xml » is corrupted. Two rows have the same value on the unique field « " + field + " ».");
                                break;
                            }
                        }
                    }
                } catch (Exception) {
                    throw;
                }
            }
            return toReturn;
        }

        public int GetNextId(List<User> users)
        {
            // Pretty bad approach but there isn't many users and users will usually be near perfectly ordered if not so...
            if (users != null && users.Count() > 0) {
                // Sort users
                int iMax = 0;
                for (int len = users.Count(); len > 1; len--) {
                    for (int i = 1; i < len; i++) {
                        if (users[i].Id > users[iMax].Id) iMax = i;
                    }
                    User temp = new User(users[len - 1]);
                    users[len - 1] = users[iMax];
                    users[iMax] = temp;
                }

                // Find the first hole
                int currentHole = 1;
                foreach (User user in users) {
                    if (user.Id > currentHole) {
                        return currentHole;
                    } else {
                        currentHole++;
                    }
                }

                // If no hole found until the end (users were perfectly ordered)
                return currentHole;
            }
            return 1;
        }

        public void DeleteUser(User user)
        {
            try {
                XML.Delete<User>(DataFile, user);
            } catch (Exception) {
                throw;
            }
        }

        public string GetUserProfilePicture(User user)
        {
            return GetUserProfilePicture(user.Id);
        }

        public string GetUserProfilePicture(int userId)
        {
            string profilePictures = Functions.GetSolutionDirPath() + "\\Data\\ProfilePictures";
            if (File.Exists(profilePictures + "\\" + userId + ".png")) {
                return profilePictures + "\\" + userId + ".png";
            }
            return profilePictures + "\\default.png";
        }

    }
}
