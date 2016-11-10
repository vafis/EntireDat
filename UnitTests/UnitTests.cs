using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Extensions;
using Moq;
using EntireData;
using EntireData.DTO;


namespace UnitTests
{
    public class UnitTests
    {
        [Fact]
        public void GetMeasureAsync_UnitTest()
        {
            var mock = new Mock<IProvider>();
            mock.Setup(x => x.GetMeasureAsync(It.IsAny<string>())).Returns(Task<StationResults>.Factory.StartNew(() => new StationResults()
            {
                Measures = new List<Measure>
                {
                    new Measure() {value = 0.983},
                    new Measure() {value = 1.983}
                },
                RLOIid = "Station1"
            }));
            var provider = mock.Object;
            var ret = provider.GetMeasureAsync("station1");

            Assert.NotNull(ret);
            Assert.Equal(ret.Result.Measures.Count,2);
        }

        [Theory]
        [InlineData("5151TH")]
        public void GetMeasureAsync_IntegrationTest(string station)
        {
            var provider=new Provider();
            var ret = provider.GetMeasureAsync(station);
            Assert.NotNull(ret);
            Assert.NotEqual(ret.Result.Measures.Count,0);
        }
    }
}
