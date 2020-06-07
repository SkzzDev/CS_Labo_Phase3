using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Core.Elements.Interfaces
{
    public interface IXMLSavable : IComparable
    {

        void Hydrate(IXMLSavable iXMLSavable);

        bool IsSavable();

        SortedDictionary<string, string> GetInvalidFields();

    }

}
