﻿using Core.Elements;
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

    public class CountriesModel : Model
    {

        #region MemberVars

        #endregion

        #region Properties

        #endregion

        #region Constructors
        public CountriesModel()
        {
            DataFile = Functions.GetXmlFilePath("countries");
        }

        #endregion

        #region Functions

        public string GetFlagPicture(Country country)
        {
            return GetFlagPicture(country.Id);
        }

        public string GetFlagPicture(int countryId)
        {
            string flagPicture = Functions.GetAssetsPath() + "\\FlagsPictures";
            if (File.Exists(flagPicture + "\\" + countryId + ".png")) {
                return flagPicture + "\\" + countryId + ".png";
            }
            return flagPicture + "\\_default.png";
        }

        public Country GetCountryRefInsideList(Country toFind, ObservableCollection<Country> countries)
        {
            foreach (Country country in countries) {
                if (toFind.Equals(country)) {
                    return country;
                }
            }
            return null;
        }

        #endregion

    }

}
