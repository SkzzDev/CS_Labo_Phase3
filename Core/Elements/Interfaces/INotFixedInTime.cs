﻿using System;

namespace Core.Elements.Interfaces
{

    public interface INotFixedInTime
    {

        DateTime CreatedAt { get; set; }

        DateTime UpdatedAt { get; set; }

    }
}
