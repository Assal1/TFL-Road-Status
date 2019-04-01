using RoadStatus.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadStatus.Core.Interfaces
{
    public interface IRoadStatusOutputWriter
    {
        void Write(RoadStatusResponse roadStatusResponse);
    }
}
