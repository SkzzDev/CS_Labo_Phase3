using Core.Elements.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Elements
{

    public class SerieException : Exception
    {

        public SerieException(string message) : base(message) { }

    }

    public class Serie : INotFixedInTime, IXMLSavable
    {

        #region MemberVars

        private int _id;
        private float _points;
        private Shooter _shootedBy;
        private Competition _shootedAt;
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

        public float Points
        {
            get { return _points; }
            set {
                if (value >= 0)
                    _points = value;
            }
        }

        public Shooter ShootedBy { get => _shootedBy; set => _shootedBy = value; }

        public Competition ShootedAt { get => _shootedAt; set => _shootedAt = value; }

        public DateTime CreatedAt { get => _createdAt; set => _createdAt = value; }

        public DateTime UpdatedAt { get => _updatedAt; set => _updatedAt = value; }

        #endregion

        #region Constructors

        public Serie(int id, float points, Shooter shootedBy, Competition shootedAt, DateTime updatedAt, DateTime createdAt)
        {
            Id = id;
            Points = points;
            ShootedBy = shootedBy;
            ShootedAt = shootedAt;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }

        public Serie(float points, Shooter shootedBy, Competition shootedAt)
        {
            Points = points;
            ShootedBy = shootedBy;
            ShootedAt = shootedAt;
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        #endregion

        #region Functions

        public bool IsSavable() => GetInvalidFields().Count == 0;

        public Dictionary<string, string> GetInvalidFields()
        {
            Dictionary<string, string> fieldsError = new Dictionary<string, string>();
            if (Id <= 0)
                fieldsError.Add("Id", "The serie's id must be strictly positive.");
            if (Points < 0)
                fieldsError.Add("Id", "The serie's points must be positive.");
            if (UpdatedAt < CreatedAt)
                fieldsError.Add("UpdatedAt", "The serie's UpdatedAt property can't be before its CreatedAt property.");
            return fieldsError;
        }

        #endregion

    }
}
