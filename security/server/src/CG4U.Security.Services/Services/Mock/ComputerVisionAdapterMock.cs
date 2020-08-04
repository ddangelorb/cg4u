using System;
using System.Threading.Tasks;
using CG4U.Security.Services.Data;
using CG4U.Security.Services.Interfaces;

namespace CG4U.Security.Services.Services.Mock
{
    public class ComputerVisionAdapterMock : IComputerVisionAdapter
    {
        public async Task<string> DescribeImageAsync(string language, byte[] image, ConnectionApiData connectionApiData)
        {
            return await Task.FromResult(@"
                {
                  ""description"": {
                    ""tags"": [
                      ""person"",
                      ""man"",
                      ""outdoor"",
                      ""window"",
                      ""glasses""
                    ],
                    ""captions"": [
                      {
                        ""text"": ""Satya Nadella sitting on a bench"",
                        ""confidence"": 0.48293603002174407
                      },
                      {
                        ""text"": ""Satya Nadella is sitting on a bench"",
                        ""confidence"": 0.40037006815422832
                      },
                      {
                        ""text"": ""Satya Nadella sitting in front of a building"",
                        ""confidence"": 0.38035155997373377
                      }
                    ]
                  },
                  ""requestId"": ""ed2de1c6-fb55-4686-b0da-4da6e05d283f"",
                  ""metadata"": {
                    ""width"": 1500,
                    ""height"": 1000,
                    ""format"": ""Jpeg""
                  }
                }                                         
            ");
        }

        public async Task<string> OCRAsync(string language, byte[] image, ConnectionApiData connectionApiData)
        {
            return await Task.FromResult(@"
                {
                  ""language"": ""en"",
                  ""textAngle"": -2.0000000000000338,
                  ""orientation"": ""Up"",
                  ""regions"": [
                    {
                      ""boundingBox"": ""462,379,497,258"",
                      ""lines"": [
                        {
                          ""boundingBox"": ""462,379,497,74"",
                          ""words"": [
                            {
                              ""boundingBox"": ""462,379,41,73"",
                              ""text"": ""A""
                            },
                            {
                              ""boundingBox"": ""523,379,153,73"",
                              ""text"": ""GOAL""
                            },
                            {
                              ""boundingBox"": ""694,379,265,74"",
                              ""text"": ""WITHOUT""
                            }
                          ]
                        },
                        {
                          ""boundingBox"": ""565,471,289,74"",
                          ""words"": [
                            {
                              ""boundingBox"": ""565,471,41,73"",
                              ""text"": ""A""
                            },
                            {
                              ""boundingBox"": ""626,471,150,73"",
                              ""text"": ""PLAN""
                            },
                            {
                              ""boundingBox"": ""801,472,53,73"",
                              ""text"": ""IS""
                            }
                          ]
                        },
                        {
                          ""boundingBox"": ""519,563,375,74"",
                          ""words"": [
                            {
                              ""boundingBox"": ""519,563,149,74"",
                              ""text"": ""JUST""
                            },
                            {
                              ""boundingBox"": ""683,564,41,72"",
                              ""text"": ""A""
                            },
                            {
                              ""boundingBox"": ""741,564,153,73"",
                              ""text"": ""WISH""
                            }
                          ]
                        }
                      ]
                    }
                  ]
                }
            ");
        }

        public async Task<string> RecognizeTextAsync(string mode, byte[] image, ConnectionApiData connectionApiData)
        {
            return await Task.FromResult(@"");
        }
    }
}
