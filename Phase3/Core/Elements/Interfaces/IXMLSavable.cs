using System.Collections.Generic;

namespace Phase3.Core.Elements.Interfaces
{
    public interface IXMLSavable
    {

        bool IsSavable();

        Dictionary<string, string> GetInvalidFields();

    }

}
