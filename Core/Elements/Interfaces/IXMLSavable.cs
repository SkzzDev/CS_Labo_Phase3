using System.Collections.Generic;
using System.Collections.Specialized;

namespace Core.Elements.Interfaces
{
    public interface IXMLSavable
    {

        bool IsSavable();

        SortedDictionary<string, string> GetInvalidFields();

    }

}
