using Phase3.Core.Elements.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Phase3.Core.Elements
{

    public class CountryException : Exception
    {

        public CountryException(string message) : base(message) { }

    }

    public class Country : IXMLSavable
    {

        #region MemberVars

        // Official United Nations' (UN) codification
        private int _id;
        private string _name;
        private string _iso2;
        private string _iso3;

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

        public string Name
        {
            get { return _name; }
            set {
                if (value.Length > 0)
                    _name = value;
            }
        }

        public string Iso2
        {
            get { return _iso2; }
            set {
                value = value.ToUpper();
                Regex r = new Regex("^[A-Z]{2}$");
                if (r.IsMatch(value))
                    _iso2 = value.ToUpper();
            }
        }

        public string Iso3
        {
            get { return _iso3; }
            set {
                value = value.ToUpper();
                Regex r = new Regex("^[A-Z]{3}$");
                if (r.IsMatch(value))
                    _iso3 = value;
            }
        }

        #endregion

        #region Constructors

        public Country(int id, string name, string iso2, string iso3)
        {
            Id = id;
            Name = name;
            Iso2 = iso2;
            Iso3 = iso3;
        }

        #endregion

        #region Functions

        public bool IsSavable() => GetInvalidFields().Count == 0;

        public Dictionary<string, string> GetInvalidFields()
        {
            Dictionary<string, string> fieldsError = new Dictionary<string, string>();
            if (Id <= 0)
                fieldsError.Add("Id", "The country's id must be strictly positive.");
            if (Name.Length <= 0)
                fieldsError.Add("Name", "The country's name can't be empty.");
            Regex r = new Regex("^[A-Z]{2}$");
            if (!r.IsMatch(Iso2))
                fieldsError.Add("Iso2", "The country's iso2 must be 2 letters long.");
            r = new Regex("^[A-Z]{3}$");
            if (!r.IsMatch(Iso3))
                fieldsError.Add("Iso3", "The country's iso3 must be 3 letters long.");
            return fieldsError;
        }

        #endregion

    }
}
