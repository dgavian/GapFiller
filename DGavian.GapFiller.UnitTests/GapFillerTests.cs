using DGavian.GapFiller.UnitTests.TestHelpers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;

namespace DGavian.GapFiller.UnitTests
{
    [TestFixture]
    public class GapFillerTests
    {
        private double _offset;

        [Test]
        public void FillGaps_BasicCollectionWithGaps_FillsGaps()
        {
            _offset = 1;
            const int upperGapIndex = 8;
            const int lowerGapIndex = 1;
            var expected = MakeTestDataCollection();
            var itemsWithGaps = MakeTestDataCollection();
            // Need to remove item with larger index first, or it won't be there.
            itemsWithGaps.RemoveAt(upperGapIndex);
            itemsWithGaps.RemoveAt(lowerGapIndex);
            Assert.That(itemsWithGaps.Count, Is.LessThan(expected.Count));
            var sut = MakeSut();

            sut.FillGaps(itemsWithGaps);

            Assert.That(itemsWithGaps.Count, Is.EqualTo(expected.Count));
            itemsWithGaps.ShouldBeEquivalentTo(expected);
        }

        private GapFiller<TestData> MakeSut()
        {
            return new GapFiller<TestData>(_offset);
        }


        private List<TestData> MakeTestDataCollection()
        {
            var result = new List<TestData>();
            for (int i = 0; i < 10; i++)
            {
                var data = MakeTestData(i);
                result.Add(data);
            }
            return result;
        }

        private TestData MakeTestData(double offset)
        {
            var result = new TestData { Offset = offset };
            return result;
        }
    }
}
