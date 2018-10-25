using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RoyaltyScanService.Tests
{
    [TestClass()]
    public class RoyaltyScanServiceTests
    {
        [TestMethod()]
        public void ScanAndUploadTest()
        {
            var obj = new RoyaltyScanService();
            var x = obj.ScanAndUpload().Result;
            Assert.Fail();
        }
    }
}