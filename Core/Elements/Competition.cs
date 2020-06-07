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

        private int _id = -1;
        private string _name = "";

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

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        #endregion

        #region Constructors

        public Competition(int id, string name, DateTime startDate, DateTime endDate, DateTime createdAt, DateTime updatedAt)
        {
            Id = id;
            Name = name;
            StartDate = startDate;
            EndDate = endDate;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }

        public Competition(Competition competition) : this (competition.Id, competition.Name, competition.StartDate, competition.EndDate, competition.CreatedAt, competition.UpdatedAt) { }

        public Competition() { }

        #endregion

        #region Functions

        public void Hydrate(IXMLSavable iXMLSavable)
        {
            if (iXMLSavable is Competition competition) {
                Id = competition.Id;
                Name = competition.Name;
                StartDate = competition.StartDate;
                EndDate = competition.EndDate;
                CreatedAt = competition.CreatedAt;
                UpdatedAt = competition.UpdatedAt;
            }
        }

        public bool IsSavable() => GetInvalidFields().Count == 0;

        public SortedDictionary<string, string> GetInvalidFields()
        {
            SortedDictionary<string, string> fieldsError = new SortedDictionary<string, string>();
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

        public override bool Equals(object obj)
        {
            Competition competitionToCompare = obj as Competition;

            if (competitionToCompare == null)
                return false;

            if (!Id.Equals(competitionToCompare.Id))
                return false;

            return true;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public int CompareTo(object obj)
        {
            if (obj is Competition toCompare) {
                return Id.CompareTo(toCompare.Id);
            }
            return -1;
        }

        #endregion

    }
}
