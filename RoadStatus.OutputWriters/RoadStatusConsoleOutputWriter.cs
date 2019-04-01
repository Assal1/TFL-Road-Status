using RoadStatus.Core.Interfaces;
using RoadStatus.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadStatus.OutputWriters
{
    public class RoadStatusConsoleOutputWriter : IRoadStatusOutputWriter
    {
        public void Write(RoadStatusResponse roadStatusResponse)
        {
            if (roadStatusResponse == null)
            {
                throw new ArgumentNullException($"Argument '{nameof(roadStatusResponse)}' is null");
            }

            if (roadStatusResponse.IsRoadFound)
            {
                Console.WriteLine($"The status of road { roadStatusResponse.DisplayName } is as follows:");
                Console.WriteLine($"Display Name: { roadStatusResponse.DisplayName }");
                Console.WriteLine($"Road Status: { roadStatusResponse.StatusSeverity }");
                Console.WriteLine($"Road Status Description: { roadStatusResponse.StatusSeverityDescription }");
            }
            else
            {
                Console.WriteLine($"{roadStatusResponse.DisplayName} is not a valid road.");
            }
        }
    }
}
