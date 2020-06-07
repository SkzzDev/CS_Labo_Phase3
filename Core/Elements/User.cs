using Core.Helpers;
using Core.Elements.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Collections.Specialized;

namespace Core.Elements
{

    public class User : INotFixedInTime, IXMLSavable
    {

        #region MemberVars

        private int _id = -1;
        private string _firstname = "";
        private string _lastname = "";
        private string _email = "";
        private string _password = "";

        #endregion

        #region Properties

        public int Id
        {
            get { return _id; }
            set {
                if (value > 0)
                    _id = value;
            }
        }

        public string Firstname
        {
            get { return _firstname; }
            set {
                if (value.Length > 0)
                    _firstname = value;
            }
        }

        public string Lastname
        {
            get { return _lastname; }
            set {
                if (value.Length > 0)
                    _lastname = value;
            }
        }

        public string Email
        {
            get { return _email; }
            set {
                if (value.Length > 0)
                    _email = value;
            }
        }

        public string Password
        {
            get { return _password; }
            set {
                if (value.Length > 0)
                    _password = value;
            }
        }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        #endregion

        #region Constructors

        public User(int id, string firstname, string lastname, string email, string password, DateTime createdAt, DateTime updatedAt)
        {
            Id = id;
            Firstname = firstname;
            Lastname = lastname;
            Email = email;
            Password = password;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }

        public User(int id, string firstname, string lastname, string email, string password)
        {
            Id = id;
            Firstname = firstname;
            Lastname = lastname;
            Email = email;
            Password = password;
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public User(User user) : this(user.Id, user.Firstname, user.Lastname, user.Email, user.Password, user.CreatedAt, user.UpdatedAt) { }

        public User() { }

        #endregion

        #region Functions

        public void Hydrate(IXMLSavable iXMLSavable)
        {
            if (iXMLSavable is User user) {
                Id = user.Id;
                Firstname = user.Firstname;
                Lastname = user.Lastname;
                Email = user.Email;
                Password = user.Password;
                CreatedAt = user.CreatedAt;
                UpdatedAt = user.UpdatedAt;
            }
        }

        public bool IsSavable() => GetInvalidFields().Count == 0;

        public SortedDictionary<string, string> GetInvalidFields()
        {
            SortedDictionary<string, string> fieldsError = new SortedDictionary<string, string>();
            if (Id <= 0)
                fieldsError.Add("Id", "The user's id must be strictly positive.");
            if (Firstname.Length <= 0)
                fieldsError.Add("Firstname", "The user's firstname can't be empty.");
            if (Lastname.Length <= 0)
                fieldsError.Add("Lastname", "The user's lastname can't be empty.");
            if (Email.Length <= 0)
                fieldsError.Add("Email", "The user's email can't be empty.");
            else if (!Functions.IsEmailValid(Email))
                fieldsError.Add("Email", "The user's email doesn't have a correct format.");
            if (Password.Length <= 0)
                fieldsError.Add("Password", "The user's password can't be empty.");
            if (UpdatedAt < CreatedAt)
                fieldsError.Add("UpdatedAt", "The user's UpdatedAt property can't be before his CreatedAt property.");
            return fieldsError;
        }

        public override bool Equals(object obj)
        {
            User userToCompare = obj as User;

            if (userToCompare == null)
                return false;

            if (!Id.Equals(userToCompare.Id))
                return false;
            if (!Email.Equals(userToCompare.Email))
                return false;

            return true;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public int CompareTo(object obj)
        {
            if (obj is User toCompare) {
                return Id.CompareTo(toCompare.Id);
            }
            return -1;
        }

        #endregion

    }
}
