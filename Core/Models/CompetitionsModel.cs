using Core.Elements;
using Core.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{

    public class CompetitionsModel
    {

        #region Properties

        private string DataFile = Functions.GetDataFilePath("competitions");

        #endregion

        #region Constructors

        public CompetitionsModel()
        {
            if (!File.Exists(DataFile)) {
                try {
                    XML.Create<Competition>(DataFile, new List<Competition>());
                } catch (Exception e) {
                    Logs.Write(e.Message);
                }
            }
        }

        #endregion

        #region Functions

        public List<Competition> GetAll()
        {
            try {
                return XML.GetAll<Competition>(DataFile);
            } catch (Exception e) {
                Logs.Write(e.Message);
            }
            return new List<Competition>();
        }

        public int GetNumberOfCompetitions()
        {
            return GetAll().Count();
        }

        #endregion

    }

}
