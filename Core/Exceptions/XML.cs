using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Exceptions.XML
{

    public class XMLException : Exception
    {

        public XMLException(string message) : base(message) { }

    }

    public class ValueInUniqueFieldAlreadyTaken : Exception
    {

        public ValueInUniqueFieldAlreadyTaken(string message) : base(message) { }

    }

}
