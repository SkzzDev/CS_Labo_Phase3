using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phase3.Elements.Interfaces
{

    public interface INotFixedInTime
    {

        DateTime CreatedAt { get; set; }

        DateTime UpdatedAt { get; set; }

    }
}
