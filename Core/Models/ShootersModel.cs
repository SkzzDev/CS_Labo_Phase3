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

    public class ShootersModel : Model
    {

        public ShootersModel()
        {
            DataFile = Functions.GetDataFilePath("shooters");
            if (!File.Exists(DataFile)) {
                try {
                    XML.Create<Shooter>(DataFile, new List<Shooter>());
                } catch (Exception e) {
                    Logs.Write(e.Message);
                }
            }
        }

        public int GetNumberOfShooters()
        {
            return GetAll<Shooter>().Count();
        }

        public void UpdateReferences(string oldId, string newId)
        {
            // Get all competitions's results file
            string resultsDir = Functions.GetSolutionDirPath() + "\\Data\\Data\\results";
            DirectoryInfo d = new DirectoryInfo(resultsDir);
            FileInfo[] Files = d.GetFiles("*.xml");

            Dictionary<string, object> conditions = new Dictionary<string, object>();
            conditions.Add("ShootedById", oldId);

            foreach (FileInfo competitionResultsFile in Files) {
                string competitionIdStr = competitionResultsFile.Name.Replace(".xml", "");
                if (int.TryParse(competitionIdStr, out int competitionId)) {
                    // Model of the competition's results file
                    ResultsModel resultsModel = new ResultsModel(competitionId);

                    // Search through all results to see if the shooter has shooted there
                    // (Very bad method but no time to create a UpdateFields() method
                    // This method is necessary because classic Update() update all fields, and we don't know
                    // without selecting it, the reste of the result infos we want to update
                    ObservableCollection<Result> results = resultsModel.GetAll<Result>();
                    foreach (Result result in results) {
                        if (result.ShootedById.Equals(oldId)) {
                            result.ShootedById = newId;
                            result.UpdatedAt = DateTime.Now;
                            resultsModel.Update<Result>(result, conditions);
                            break; // Only one result by shooter by competition, so when found, stop
                        }
                    }
                }
            }
        }

    }

}
