using Phase3.Helpers;
using Phase3.Elements.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Phase3.Elements
{

    public class User : INotFixedInTime, IXMLSavable
    {

        #region MemberVars

        private int _id = -1;
        private string _firstname = "";
        private string _lastname = "";
        private string _email = "";
        private string _password = "";
        private DateTime _createdAt;
        private DateTime _updatedAt;

        public string[] UniquesFields { get; } = new string[2] { "Id", "Email" };

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
                if (Functions.IsEmailValid(value))
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

        public DateTime CreatedAt { get => _createdAt; set => _createdAt = value; }

        public DateTime UpdatedAt { get => _updatedAt; set => _updatedAt = value; }

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

        public User() { }

        #endregion

        #region Functions

        public bool IsSavable() => GetInvalidFields().Count == 0;

        public Dictionary<string, string> GetInvalidFields()
        {
            Dictionary<string, string> fieldsError = new Dictionary<string, string>();
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

        #endregion

    }
}
