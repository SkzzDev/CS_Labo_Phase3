using Core;
using Core.Elements;
using Core.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{

    public class UsersModel : Model
    {

        public UsersModel()
        {
            // Users cannot be stored in the data dir path from the registry beacause we need to
            // use them before the user is connected to check wether or not the account exists
            DataFile = Functions.GetInitialXmlsPath() + "\\users.xml";
            if (!File.Exists(DataFile))
                XML.Create<User>(DataFile, new List<User>());
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
                    List<User> results = XML.Find<User>(DataFile, search);
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

        public int GetNextId(ObservableCollection<User> users)
        {
            if (users != null && users.Count() > 0) {

                Functions.Sort<User>(users);

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

        public string GetUserProfilePicture(User user)
        {
            return GetUserProfilePicture(user.Id);
        }

        public string GetUserProfilePicture(int userId)
        {
            string profilePictures = Functions.GetAssetsPath() + "\\ProfilePictures";
            if (File.Exists(profilePictures + "\\" + userId + ".png")) {
                return profilePictures + "\\" + userId + ".png";
            }
            return profilePictures + "\\_default.png";
        }

    }
}
