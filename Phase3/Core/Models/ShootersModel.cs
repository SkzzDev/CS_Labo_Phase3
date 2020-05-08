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

    public class ShootersModel
    {

        private string DataFile = Functions.GetDataFilePath("shooters");

        public ShootersModel()
        {
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
            try {
                return XML.GetAll<Shooter>(DataFile).Count();
            } catch (Exception e) {
                Logs.Write(e.Message);
            }
            return 0;
        }

    }

}
