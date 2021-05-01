using System;
using System.Threading.Tasks;
using Amazon;
using Amazon.KinesisVideo;
using Amazon.KinesisVideo.Model;
using Amazon.KinesisVideoArchivedMedia;
using Amazon.KinesisVideoArchivedMedia.Model;
using InvalidArgumentException = Amazon.KinesisVideo.Model.InvalidArgumentException;

namespace TestProject
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            SetStreamUriKinesisHLSAsync().Wait();
            Console.WriteLine("Bye,bye World!");
        }

        private static async Task SetStreamUriKinesisHLSAsync()
        {
            try
            {
                string awsAccessKeyID = @"";
                string awsSecretAccessKey = @"";
                var streamName = @"";

                var video = new AmazonKinesisVideoClient(
                    awsAccessKeyID,
                    awsSecretAccessKey,
                    new AmazonKinesisVideoConfig()
                    {
                        RegionEndpoint = RegionEndpoint.USWest2,
                    }
                );
                var endPoint = await video.GetDataEndpointAsync(
                    new GetDataEndpointRequest()
                    {
                        //StreamARN = streamARN,
                        StreamName = streamName,
                        APIName = APIName.GET_HLS_STREAMING_SESSION_URL
                    }
                );

                var videoArchived = new AmazonKinesisVideoArchivedMediaClient(
                    awsAccessKeyID,
                    awsSecretAccessKey,
                    new AmazonKinesisVideoArchivedMediaConfig()
                    {
                        ServiceURL = endPoint.DataEndpoint
                    }
                );

                /*var fragments = await videoArchived.ListFragmentsAsync(
                    new ListFragmentsRequest()
                    {
                        StreamName = streamName,
                        FragmentSelector = new FragmentSelector()
                    }
                );
                foreach(var f in fragments.Fragments)
                {
                    Console.WriteLine($"FragmentLengthInMilliseconds:{f.FragmentLengthInMilliseconds}, FragmentNumber:{f.FragmentNumber}, FragmentSizeInBytes: {f.FragmentSizeInBytes}, ProducerTimestamp:{f.ProducerTimestamp}, ServerTimestamp:{f.ServerTimestamp}");
                }*/

                var hlsSession = await videoArchived.GetHLSStreamingSessionURLAsync(
                    new GetHLSStreamingSessionURLRequest()
                    {
                        //StreamARN = streamARN,
                        StreamName = streamName,
                        PlaybackMode = PlaybackMode.ON_DEMAND,
                        HLSFragmentSelector = new HLSFragmentSelector()
                        {
                            FragmentSelectorType = HLSFragmentSelectorType.SERVER_TIMESTAMP,
                            TimestampRange = new HLSTimestampRange()
                            {
                                StartTimestamp = new DateTime(2018, 9, 19, 19, 55, 20),
                                EndTimestamp = new DateTime(2018, 9, 19, 19, 55, 30)
                            }
                        }
                    }
                );

                var ss = hlsSession.HLSStreamingSessionURL;
                Console.WriteLine(ss);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
