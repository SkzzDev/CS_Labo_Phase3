using Core.Elements.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Elements
{

    public class Competition : INotFixedInTime, IXMLSavable
    {

        #region MemberVars

        private int _id;
        private string _name;
        private DateTime _startDate;
        private DateTime _endDate;
        private DateTime _createdAt;
        private DateTime _updatedAt;

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

        public DateTime StartDate
        {
            get { return _startDate; }
            set { _startDate = value; }
        }

        public DateTime EndDate
        {
            get { return _endDate; }
            set { _endDate = value; }
        }

        public DateTime CreatedAt { get => _createdAt; set => _createdAt = value; }

        public DateTime UpdatedAt { get => _updatedAt; set => _updatedAt = value; }

        #endregion

        #region Constructors



        #endregion

        #region Functions

        public bool IsSavable() => GetInvalidFields().Count == 0;

        public Dictionary<string, string> GetInvalidFields()
        {
            Dictionary<string, string> fieldsError = new Dictionary<string, string>();
            if (Id <= 0)
                fieldsError.Add("Id", "The competition's id must be strictly positive.");
            if (Name.Length <= 0)
                fieldsError.Add("Name", "The competition's name can't be empty.");
            if (StartDate > EndDate)
                fieldsError.Add("StartDate", "The competition's start date can't be past its end date.");
            if (EndDate < StartDate)
                fieldsError.Add("EndDate", "The competition's end date can't be past or equals to its start date.");
            if (UpdatedAt < CreatedAt)
                fieldsError.Add("UpdatedAt", "The competition's UpdatedAt property can't be before its CreatedAt property.");
            return fieldsError;
        }

        #endregion

    }
}
