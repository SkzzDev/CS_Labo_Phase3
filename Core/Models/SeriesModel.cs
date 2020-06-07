using Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{

    public class SeriesModel : Model
    {

        public SeriesModel()
        {
            DataFile = Functions.GetDataFilePath("series");
        }

    }

}
