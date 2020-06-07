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

    public class CountriesModel : Model
    {

        #region MemberVars

        #endregion

        #region Properties

        #endregion

        #region Constructors
        public CountriesModel()
        {
            DataFile = Functions.GetDataFilePath("countries");
        }

        #endregion

        #region Functions

        public string GetFlagPicture(Country country)
        {
            return GetFlagPicture(country.Id);
        }

        public string GetFlagPicture(int countryId)
        {
            string flagPicture = Functions.GetSolutionDirPath() + "\\Data\\FlagsPictures";
            if (File.Exists(flagPicture + "\\" + countryId + ".png")) {
                return flagPicture + "\\" + countryId + ".png";
            }
            return flagPicture + "\\_default.png";
        }

        #endregion

    }

}
