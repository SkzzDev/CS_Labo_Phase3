using Core.Elements.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Core.Elements
{

    public class Shooter : INotFixedInTime, IXMLSavable
    {

        #region MemberVars

        private string _id = "";
        private string _firstname = "";
        private string _lastname = "";
        private DateTime _birthday;

        #endregion

        #region Properties

        public string Id
        {
            get { return _id; }
            set {
                if (value.Length > 0) {
                    _id = value.ToUpper();
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
                if (value <= DateTime.Now && value.Year >= 1907 - 100)
                    _birthday = value;
            }
        }

        [XmlIgnore]
        public string BirthdayDate
        {
            get { return _birthday.ToString("dd/MM/yyyy"); }
        }

        [XmlIgnore]
        public Country Nationality { get; set; } = new Country();

        public int NationalityCountryId {
            get { return Nationality.Id; }
            set { Nationality.Id = value; }
        }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        #endregion

        #region Constructors

        public Shooter(string id, string firstname, string lastname, DateTime birthday, Country nationality, DateTime createdAt, DateTime updatedAt)
        {
            Id = id;
            Firstname = firstname;
            Lastname = lastname;
            Birthday = birthday;
            Nationality = new Country(nationality);
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }

        public Shooter(string id, string firstname, string lastname, DateTime birthday, int nationalityCountryId, DateTime createdAt, DateTime updatedAt) : this(id, firstname, lastname, birthday, null, createdAt, updatedAt)
        {
            NationalityCountryId = nationalityCountryId;
        }

        public Shooter(string id, string firstname, string lastname, DateTime birthday, Country nationality) : this(id, firstname, lastname, birthday, nationality, DateTime.Now, DateTime.Now) { }

        public Shooter(string id, string firstname, string lastname, DateTime birthday, int nationalityCountryId) : this(id, firstname, lastname, birthday, null, DateTime.Now, DateTime.Now)
        {
            NationalityCountryId = nationalityCountryId;
        }

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
                fieldsError.Add("Id", "The shooter's id must match the pattern ^[0-9]{4}[A-Z]{6}[0-9]{2}$.");
            if (Firstname.Length <= 0)
                fieldsError.Add("Firstname", "The shooter's firstname can't be empty.");
            if (Lastname.Length <= 0)
                fieldsError.Add("Lastname", "The shooter's lastname can't be empty.");
            if (Birthday > DateTime.Now)
                fieldsError.Add("Birthday", "The shooter's birthday can't be later than now.");
            if (Birthday.Year < 1907 - 100) // ISSF Foundation, less 100 years
                fieldsError.Add("Birthday", Birthday.Year.ToString() + "The shooter's birthday can't be before year 1807 (100 years before the ISSF foundation in 1907).");
            if (UpdatedAt < CreatedAt)
                fieldsError.Add("UpdatedAt", "The shooter's UpdatedAt property can't be before his CreatedAt property.");

            return fieldsError;
        }

        public override bool Equals(object obj)
        {
            Shooter shooterToCompare = obj as Shooter;

            if (shooterToCompare == null)
                return false;

            if (!Id.Equals(shooterToCompare.Id))
                return false;

            return true;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public int CompareTo(object obj)
        {
            if (obj is Shooter toCompare) {
                return Id.CompareTo(toCompare.Id);
            }
            return -1;
        }

        public override string ToString()
        {
            return Id;
        }

        #endregion

    }
}
