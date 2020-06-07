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

    }

}
