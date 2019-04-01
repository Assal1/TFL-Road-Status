using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RichardSzalay.MockHttp;
using RoadStatus.Clients;
using RoadStatus.ConsoleApp;
using RoadStatus.Core.Interfaces;
using RoadStatus.OutputWriters;
using System;
using System.IO;
using System.Linq;
using System.Net;
using TechTalk.SpecFlow;

namespace RoadStatus.Tests
{
    [Binding]
    public class RoadStatusSteps
    {
        // Input
        private string roadId;

        // Output
        private StringWriter consoleOutput;
        private int programExitCode;

        [Given(@"a valid road ID '(.*)' is specified")]
        public void GivenAValidRoadIDIsSpecified(string validRoadId)
        {
            roadId = validRoadId;
        }

        [Given(@"an invalid road ID '(.*)' is specified")]
        public void GivenAnInvalidRoadIDIsSpecified(string invalidRoadId)
        {
            roadId = invalidRoadId;
        }

        [When(@"the client is run with a valid road ID")]
        public void WhenTheClientIsRunWithAValidRoadID()
        {
            SetupMockAndCallClient(true);
        }

        [When(@"the client is run with an invalid road ID")]
        public void WhenTheClientIsRunWithAnInvalidRoadID()
        {
            SetupMockAndCallClient(false);
        }

        [Then(@"the road displayName should be displayed")]
        public void ThenTheRoadDisplayNameShouldBeDisplayed()
        {
            string[] consoleLines = GetConsoleOutput();
            Assert.IsTrue(consoleLines.Any(line => line == $"Display Name: { roadId }"));
        }

        [Then(@"the road statusSeverity should be displayed as Road Status")]
        public void ThenTheRoadStatusSeverityShouldBeDisplayedAsRoadStatus()
        {
            string[] consoleLines = GetConsoleOutput();
            Assert.IsTrue(consoleLines.Any(line => line == $"Road Status: Good"));
        }

        [Then(@"the road statusSeverityDescription should be displayed as Road Status Description")]
        public void ThenTheRoadStatusSeverityDescriptionShouldBeDisplayedAsRoadStatusDescription()
        {
            string[] consoleLines = GetConsoleOutput();
            Assert.IsTrue(consoleLines.Any(line => line == $"Road Status Description: No Exceptional Delays"));
        }

        [Then(@"the application should return an informative error")]
        public void ThenTheApplicationShouldReturnAnInformativeError()
        {
            string[] consoleLines = GetConsoleOutput();
            Assert.IsTrue(consoleLines.Any(line => line == $"{roadId} is not a valid road."));
        }

        [Then(@"the application should exit with a non-zero System Error code")]
        public void ThenTheApplicationShouldExitWithANon_ZeroSystemErrorCode()
        {
            Assert.IsTrue(programExitCode != 0);
        }

        private string[] GetConsoleOutput()
        {
            return consoleOutput.ToString().Split(
                    new[] { Environment.NewLine },
                    StringSplitOptions.None
                );
        }

        private void SetupMockAndCallClient(bool forValidRoad)
        {
            // Mock the dependencies
            var mockHttpMessageHandler = new MockHttpMessageHandler();
            if (forValidRoad)
            {
                mockHttpMessageHandler.Expect("/Road*")
                .Respond(HttpStatusCode.OK, "application/json", "[ { \"$type\": \"Tfl.Api.Presentation.Entities.RoadCorridor, Tfl.Api.Presentation.Entities\", \"id\": \"a2\", \"displayName\": \"A2\", \"statusSeverity\": \"Good\", \"statusSeverityDescription\": \"No Exceptional Delays\", \"bounds\": \"[[-0.0857,51.44091],[0.17118,51.49438]]\", \"envelope\": \"[[-0.0857,51.44091],[-0.0857,51.49438],[0.17118,51.49438],[0.17118,51.44091],[-0.0857,51.44091]]\", \"url\": \"/Road/a2\" }]");
            }
            else
            {
                mockHttpMessageHandler.Expect("*/Road*")
                .Respond(HttpStatusCode.NotFound, "application/json", "{ \"$type\": \"Tfl.Api.Presentation.Entities.ApiError, Tfl.Api.Presentation.Entities\" \"timestampUtc\": \"2017-11-21T14:37:39.7206118Z\", \"exceptionType\": \"EntityNotFoundException\", \"httpStatusCode\": 404, \"httpStatus\": \"NotFound\", \"relativeUri\": \"/Road/A233\", \"message\": \"The following road id is not recognised: A233\"}");
            }
            Mock<IConfigurationManager> mockConfigurationManager = new Mock<IConfigurationManager>();
            mockConfigurationManager.Setup(m => m.GetConfigValue("AppId")).Returns("DummyId");
            mockConfigurationManager.Setup(m => m.GetConfigValue("AppKey")).Returns("DummyKey");
            mockConfigurationManager.Setup(m => m.GetConfigValue("ApiUri")).Returns("https://api.tfl.gov.uk/Road/");

            RoadStatusRestClient mockRoadStatusRestClient = new RoadStatusRestClient(
                                                                mockConfigurationManager.Object,
                                                                mockHttpMessageHandler.ToHttpClient()
                                                            );

            // Mock console
            consoleOutput = new StringWriter();
            Console.SetOut(consoleOutput);
            var roadStatusResponse = mockRoadStatusRestClient.GetRoadStatusAsync(this.roadId).Result;

            var roadStatusConsoleOutputWriter = new RoadStatusConsoleOutputWriter();
            roadStatusConsoleOutputWriter.Write(roadStatusResponse);

            // Call the program with mocked dependencies
            programExitCode = Program.GetRoadStatus(mockRoadStatusRestClient, roadStatusConsoleOutputWriter, roadId);
        }
    }
}
