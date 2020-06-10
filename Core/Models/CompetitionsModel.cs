using Core.Elements;
using Core.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{

    public class CompetitionsModel : Model
    {

        #region Properties

        #endregion

        #region Constructors

        public CompetitionsModel()
        {
            DataFile = Functions.GetXmlFilePath("competitions");
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

        public int GetNextId(ObservableCollection<Competition> competitions)
        {
            if (competitions != null && competitions.Count() > 0) {

                Functions.Sort<Competition>(competitions);

                // Find the first hole
                int currentHole = 1;
                foreach (Competition Competition in competitions) {
                    if (Competition.Id > currentHole) {
                        return currentHole;
                    } else {
                        currentHole++;
                    }
                }

                // If no hole found until the end (users were perfectly ordered)
                return currentHole;
            }
            return 1;
        }

        public int GetNumberOfCompetitions()
        {
            return GetAll<Competition>().Count();
        }

        #endregion

    }

}
