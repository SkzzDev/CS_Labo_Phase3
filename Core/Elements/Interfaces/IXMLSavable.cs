using System.Collections.Generic;

namespace Core.Elements.Interfaces
{
    public interface IXMLSavable
    {

        bool IsSavable();

        Dictionary<string, string> GetInvalidFields();

    }

}
