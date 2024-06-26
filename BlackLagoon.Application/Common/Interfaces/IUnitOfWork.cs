﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackLagoon.Application.Common.Interfaces
{
    public interface IUnitOfWork
    {
        public IVillaRepository Villa { get; }
        public IAmenityRepository Amenity { get; }
        public IVillaNumberRepository VillaNumber { get; }
        public void Save();
    }
}
