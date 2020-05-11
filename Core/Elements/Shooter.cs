using Core.Elements.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Core.Elements
{

    public class Shooter : INotFixedInTime, IXMLSavable
    {

        #region MemberVars

        private string _id = "";
        private string _firstname = "";
        private string _lastname = "";
        private DateTime _birthday;
        private Country _nationality;
        private DateTime _createdAt;
        private DateTime _updatedAt;

        #endregion

        #region Properties

        public string Id
        {
            get { return _id; }
            set {
                if (value.Length > 0) {
                    value = value.ToUpper();
                    Regex r = new Regex("^[0-9]{4}[A-Z]{6}[0-9]{2}$");
                    if (r.IsMatch(value))
                        _id = value;
                }
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

        public DateTime Birthday
        {
            get { return _birthday; }
            set {
                if (value < DateTime.Now && value.Year >= 1907 - 100)
                    _birthday = value;
            }
        }

        public Country Nationality
        {
            get { return _nationality; }
            set { _nationality = value; }
        }

        public DateTime CreatedAt { get => _createdAt; set => _createdAt = value; }

        public DateTime UpdatedAt { get => _updatedAt; set => _updatedAt = value; }

        #endregion

        #region Constructors

        public Shooter(string id, string firstname, string lastname, DateTime birthday, Country nationality, DateTime createdAt, DateTime updatedAt)
        {
            Id = id;
            Firstname = firstname;
            Lastname = lastname;
            Birthday = birthday;
            Nationality = nationality;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }

        public Shooter(string id, string firstname, string lastname, DateTime birthday, Country nationality) : this (id, firstname, lastname, birthday, nationality, DateTime.Now, DateTime.Now) { }

        public Shooter() { }

        #endregion

        #region Functions

        public void Hydrate(IXMLSavable iXMLSavable)
        {
            if (iXMLSavable is Shooter shooter) {
                Id = shooter.Id;
                Firstname = shooter.Firstname;
                Lastname = shooter.Lastname;
                Birthday = shooter.Birthday;
                Nationality = shooter.Nationality;
                CreatedAt = shooter.CreatedAt;
                UpdatedAt = shooter.UpdatedAt;
            }
        }

        public bool IsSavable() => GetInvalidFields().Count == 0;

        public SortedDictionary<string, string> GetInvalidFields()
        {
            SortedDictionary<string, string> fieldsError = new SortedDictionary<string, string>();
            Regex r = new Regex("^[0-9]{4}[A-Z]{6}[0-9]{2}$");
            if (Id.Length <= 0)
                fieldsError.Add("Id", "The shooter's id can't be empty.");
            else if (!r.IsMatch(Id))
                fieldsError.Add("Id", "The shooter's id must match the pattern.");
            if (Firstname.Length <= 0)
                fieldsError.Add("Firstname", "The shooter's firstname can't be empty.");
            if (Lastname.Length <= 0)
                fieldsError.Add("Lastname", "The shooter's lastname can't be empty.");
            if (Birthday < DateTime.Now)
                fieldsError.Add("Birthday", "The shooter's birthday can't be later than now.");
            if (Birthday.Year < 1907 - 100) // ISSF Foundation, less 100 years
                fieldsError.Add("Birthday", "The shooter's birthday can't be before year 1807 (100 years before the ISSF foundation in 1907).");
            if (UpdatedAt < CreatedAt)
                fieldsError.Add("UpdatedAt", "The shooter's UpdatedAt property can't be before his CreatedAt property.");

            if (!fieldsError.ContainsKey("Id") && !fieldsError.ContainsKey("Firstname") && !fieldsError.ContainsKey("Lastname")) {
                string lastnameLetters = Lastname.Substring(0, 4).ToUpper();
                string firstnameLetters = Firstname.Substring(0, 2).ToUpper();
                r = new Regex("^(19|20)[0-9]{2}" + lastnameLetters + firstnameLetters + "[0-9]{2}$");
                if (!r.IsMatch(Id))
                    fieldsError.Add("Id", "The shooter's id doesn't match with the others shooter's data.");
            }

            return fieldsError;
        }

        public override bool Equals(object obj)
        {
            User userToCompare = obj as User;
            if (userToCompare == null) {
                return false;
            }
            if (!Id.Equals(userToCompare.Id))
                return false;

            return true;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        #endregion

    }
}
