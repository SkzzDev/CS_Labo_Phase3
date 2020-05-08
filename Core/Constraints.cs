using Core.Elements;
using Core.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Core
{

    public class ConstraintsException : Exception
    {

        public ConstraintsException(string message) : base(message) { }

    }

    public enum ConstraintsTypes {
        UNIQUE
    }

    public class Constraints
    {

        #region MemberVars

        private List<Constraint> _constraints = new List<Constraint>() {
            new Constraint("users", "Id", ConstraintsTypes.UNIQUE),
            new Constraint("users", "Email", ConstraintsTypes.UNIQUE)
        };

        private static Constraints _instance; // Used to make singleton

        #endregion

        #region Constructors

        public Constraints()
        {
            RewriteConstraints();
            try {
                string filename = Functions.GetConstaintsFilePath();
                if (new FileInfo(filename).Length != 0) {
                    XmlSerializer xmlFormat = new XmlSerializer(typeof(List<Constraint>));
                    using (Stream fStream = File.OpenRead(filename)) {
                        _constraints = (List<Constraint>)xmlFormat.Deserialize(fStream);
                    }
                }
            } catch (Exception) {
                throw;
            }
        }

        #endregion

        #region Functions

        public static Constraints WakeUp()
        {
            // Singleton
            if (_instance == null)
                _instance = new Constraints();
            return _instance;
        }

        public void RewriteConstraints()
        {
            string filename = Functions.GetConstaintsFilePath();
            XmlSerializer xmlFormat = new XmlSerializer(typeof(List<Constraint>));
            using (Stream fStream = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None)) {
                xmlFormat.Serialize(fStream, _constraints);
            }
        }

        public List<Constraint> GetDataFileConstraints(string filename)
        {
            // Keeping only the base name of the file, not all its path
            filename = Path.GetFileName(filename);
            int pos = filename.LastIndexOf(".");
            if (pos != -1)
                filename = filename.Substring(0, pos);

            List<Constraint> fileConstraints = new List<Constraint>();
            foreach (Constraint constraint in _constraints) {
                if (constraint.DataFile == filename)
                    fileConstraints.Add(new Constraint(constraint));
            }

            return fileConstraints;
        }

        public List<Constraint> GetConstraintsOfType(ConstraintsTypes constraintType, List<Constraint> constraints)
        {
            List<Constraint> toReturn = new List<Constraint>();
            foreach (Constraint constraint in constraints) {
                if (constraint.Type == constraintType)
                    toReturn.Add(new Constraint(constraint));
            }
            return toReturn;
        }

        #endregion

    }
}
