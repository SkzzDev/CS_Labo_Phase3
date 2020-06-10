using Core.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{

    public class ResultsModel : Model
    {

        public ResultsModel(int competitionId)
        {
            if (competitionId > 0) {
                DataFile = Functions.GetDataFilePath("results/" + competitionId.ToString());
                CreateIfDontExist();
            }
        }

        public void CreateIfDontExist()
        {
            if (!DataFile.Equals(""))
                using (var fileStream = File.Open(DataFile, FileMode.OpenOrCreate, FileAccess.ReadWrite)) { };
        }

    }

}
