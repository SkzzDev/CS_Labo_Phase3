using Core.Elements.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Core.Elements
{

    public class SerieException : Exception
    {

        public SerieException(string message) : base(message) { }

    }

    public class Result : INotFixedInTime, IXMLSavable
    {

        #region MemberVars

        private float _serie1 = 0;
        private float _serie2 = 0;
        private float _serie3 = 0;
        private float _serie4 = 0;

        #endregion

        #region Properties

        public float Serie1
        {
            get { return _serie1; }
            set {
                if (value >= 0 && value <= 109)
                    _serie1 = value;
            }
        }

        public float Serie2
        {
            get { return _serie2; }
            set {
                if (value >= 0 && value <= 109)
                    _serie2 = value;
            }
        }

        public float Serie3
        {
            get { return _serie3; }
            set {
                if (value >= 0 && value <= 109)
                    _serie3 = value;
            }
        }

        public float Serie4
        {
            get { return _serie4; }
            set {
                if (value >= 0 && value <= 109)
                    _serie4 = value;
            }
        }

        public float Total
        {
            get { return Serie1 + Serie2 + Serie3 + Serie4; }
        }

        [XmlIgnore]
        public Shooter ShootedBy { get; set; } = new Shooter();

        public string ShootedById
        {
            get { return ShootedBy.Id; }
            set { ShootedBy.Id = value; }
        }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        #endregion

        #region Constructors

        public Result(float serie1, float serie2, float serie3, float serie4, string shootedById, DateTime updatedAt, DateTime createdAt)
        {
            Serie1 = serie1;
            Serie2 = serie2;
            Serie3 = serie3;
            Serie4 = serie4;
            ShootedById = shootedById;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }

        public Result(float serie1, float serie2, float serie3, float serie4, string shootedById)
        {
            Serie1 = serie1;
            Serie2 = serie2;
            Serie3 = serie3;
            Serie4 = serie4;
            ShootedById = shootedById;
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public Result() { }

        #endregion

        #region Functions

        public void Hydrate(IXMLSavable iXMLSavable)
        {
            if (iXMLSavable is Result result) {
                Serie1 = result.Serie1;
                Serie2 = result.Serie2;
                Serie3 = result.Serie3;
                Serie4 = result.Serie4;
                ShootedBy = result.ShootedBy;
                CreatedAt = result.CreatedAt;
                UpdatedAt = result.UpdatedAt;
            }
        }

        public bool IsSavable() => GetInvalidFields().Count == 0;

        public SortedDictionary<string, string> GetInvalidFields()
        {
            SortedDictionary<string, string> fieldsError = new SortedDictionary<string, string>();
            if (Serie1 < 0 || Serie1 > 109 || Serie2 < 0 || Serie2 > 109 || Serie3 < 0 || Serie3 > 109 || Serie4 < 0 || Serie4 > 109)
                fieldsError.Add("Series", "The series' points must be between 0 and 109 both included.");
            if (UpdatedAt < CreatedAt)
                fieldsError.Add("UpdatedAt", "The result's UpdatedAt property can't be before its CreatedAt property.");
            return fieldsError;
        }

        public override bool Equals(object obj)
        {
            Result serieToCompare = obj as Result;

            if (serieToCompare == null)
                return false;

            if (!ShootedBy.Id.Equals(serieToCompare.ShootedBy.Id))
                return false;

            return true;
        }

        public override int GetHashCode()
        {
            return ShootedBy.Id.GetHashCode();
        }

        public int CompareTo(object obj)
        {
            if (obj is Result toCompare) {
                return toCompare.Total.CompareTo(Total);
            }
            return -1;
        }

        #endregion

    }
}
