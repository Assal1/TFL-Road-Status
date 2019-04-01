using RoadStatus.Clients;
using RoadStatus.Configuration;
using RoadStatus.Core.Interfaces;
using RoadStatus.Core.Models;
using RoadStatus.OutputWriters;
using System.Linq;using System;
using Unity;

namespace RoadStatus.ConsoleApp
{
    public class Program
    {
        public static int Main(string[] args)
        {
            if (args == null || !args.Any())            {                Console.WriteLine("Please provide a road name");                return 1;            }
            // Configure unity dependencies
            UnityContainer container = new UnityContainer();
            container.RegisterType<IConfigurationManager, AppSettingsConfigurationManager>();
            container.RegisterType<IRoadStatusClient, RoadStatusRestClient>();
            container.RegisterType<IRoadStatusOutputWriter, RoadStatusConsoleOutputWriter>();


            // Get dependencies
            IRoadStatusClient roadStatusClient = container.Resolve<IRoadStatusClient>();
            IRoadStatusOutputWriter roadStatusOutputWriter = container.Resolve<IRoadStatusOutputWriter>(); ;

            return GetRoadStatus(roadStatusClient, roadStatusOutputWriter, string.Join(" ", args));
        }

        public static int GetRoadStatus(IRoadStatusClient roadStatusClient,
                                        IRoadStatusOutputWriter roadStatusOutputWriter,
                                        string roadId)
        {
            try
            {
                RoadStatusResponse roadStatus = roadStatusClient.GetRoadStatusAsync(roadId).Result;
                roadStatusOutputWriter.Write(roadStatus);

                // Set the exit code based on road status
                return roadStatus.IsRoadFound ? 0 : 1;
            }
            catch(Exception)
            {
                Console.WriteLine("UnExpected error occured, please try again!");
                return 1;
            }
        }
    }
}
