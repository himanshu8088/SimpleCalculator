﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleCalculator.Core.Contracts
{
    public interface IOperation
    {
        string Name { get;  }

        void Execute();
    }
}
