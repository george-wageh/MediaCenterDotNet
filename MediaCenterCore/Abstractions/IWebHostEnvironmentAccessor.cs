using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaCenterCore.Abstractions
{
    public interface IWebHostEnvironmentAccessor
    {
        string WebRootPath { get; }
        string ContentRootPath { get; }
    }
}
