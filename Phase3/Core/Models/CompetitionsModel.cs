using Phase3.Core.Elements;
using Phase3.Core.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phase3.Core.Models
{

    public class CompetitionsModel
    {

        private string DataFile = Functions.GetDataFilePath("competitions");

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

        public int GetNumberOfCompetitions()
        {
            try {
                return XML.GetAll<Competition>(DataFile).Count();
            } catch (Exception e) {
                Logs.Write(e.Message);
            }
            return 0;
        }

    }

}
