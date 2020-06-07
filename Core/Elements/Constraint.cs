using Core.Elements.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Elements
{
    public class Constraint : IXMLSavable
    {

        #region MemberVars

        private string _dataFile = "";
        private string _field = "";
        private ConstraintsTypes _type;

        #endregion

        #region Properties

        public string DataFile
        {
            get => _dataFile;
            set {
                if (value.Length > 0)
                    _dataFile = value;
            }
        }

        public string Field
        {
            get => _field;
            set {
                if (value.Length > 0)
                    _field = value;
            }
        }

        public ConstraintsTypes Type
        {
            get => _type;
            set => _type = value;
        }

        #endregion

        #region Constructors

        public Constraint(string dataFile, string field, ConstraintsTypes type) // Initialization constructor
        {
            DataFile = dataFile;
            Field = field;
            Type = type;
        }

        public Constraint(Constraint constraint) : this (constraint.DataFile, constraint.Field, constraint.Type) { } // Copy constructor

        public Constraint() { } // Default constructor

        #endregion

        #region Functions

        public void Hydrate(IXMLSavable iXMLSavable)
        {
            if (iXMLSavable is Constraint constraint) {
                DataFile = constraint.DataFile;
                Field = constraint.Field;
                Type = constraint.Type;
            }
        }

        public bool IsSavable() => GetInvalidFields().Count == 0;

        public SortedDictionary<string, string> GetInvalidFields()
        {
            SortedDictionary<string, string> fieldsError = new SortedDictionary<string, string>();
            if (DataFile.Length <= 0)
                fieldsError.Add("DataFile", "The constraint's data file can't be empty.");
            if (Field.Length <= 0)
                fieldsError.Add("Field", "The constraint's field can't be empty.");
            return fieldsError;
        }

        public int CompareTo(object obj)
        {
            if (obj is Constraint toCompare) {
                return Field.CompareTo(toCompare.Field);
            }
            return -1;
        }

        #endregion

    }
}
