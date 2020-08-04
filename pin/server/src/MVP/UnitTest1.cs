using System;
using System.Drawing;
using System.IO;
using CG4Pin.Core;
using CG4Pin.Extractor.Medina2012;
using CG4Pin.Extractor.Ratha1995;
using Xunit;

namespace MVP
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            string s;
            try
            {
                //https://www.codeproject.com/Articles/97590/A-Framework-in-C-for-Fingerprint-Verification
                double similarity = 0;
                s = "Hello";

                // Loading fingerprints
                var fingerprintImg1 = ImageLoader.LoadImage2("fp1.jpg");
                var fingerprintImg2 = ImageLoader.LoadImage2("fp1.jpg");
                //var fingerprintImg1 = new Bitmap(stream1);
                //var fingerprintImg2 = new Bitmap(stream2);

                // Building feature extractor and extracting features
                var featExtractor = new MTripletsExtractor() { MtiaExtractor = new Ratha1995MinutiaeExtractor() };
                var features1 = featExtractor.ExtractFeatures(fingerprintImg1);
                var features2 = featExtractor.ExtractFeatures(fingerprintImg2);

                // Building matcher and matching
                var matcher = new M3gl();
                similarity = matcher.Match(features1, features2);

                s = "Bye with similarity: " + similarity.ToString();
            }
            catch (Exception e)
            {
                var errorMsg = ("Error: " + e.ToString());
                s = errorMsg;
            }
        }
    }
}
