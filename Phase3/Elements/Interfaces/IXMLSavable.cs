using System;
using System.Collections.Generic;
using System.Text;

namespace Phase3.Elements.Interfaces
{
    public interface IXMLSavable
    {

        bool IsSavable();

        Dictionary<string, string> GetInvalidFields();

        string[] UniquesFields { get; }

    }
}
