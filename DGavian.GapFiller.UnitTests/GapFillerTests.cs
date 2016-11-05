using DGavian.GapFiller.UnitTests.TestHelpers;
using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;

namespace DGavian.GapFiller.UnitTests
{
    [TestFixture]
    public class GapFillerTests
    {
        private decimal _interval;

        [TestCase(100, 1000)]
        [TestCase(10, 100)]
        [TestCase(1, 10)]
        [TestCase(0.5, 5)]
        [TestCase(0.1, 1)]
        [TestCase(0.01, 0.1)]
        public void FillGaps_BasicCollectionWithGaps_FillsGaps(decimal interval, decimal upperBound)
        {
            _interval = interval;
            const int upperGapIndex = 8;
            const int lowerGapIndex = 1;
            var expected = MakeTestDataCollection(upperBound, interval);
            var itemsWithGaps = MakeTestDataCollection(upperBound, interval);
            // Need to remove item with larger index first, or it won't be there.
            itemsWithGaps.RemoveAt(upperGapIndex);
            itemsWithGaps.RemoveAt(lowerGapIndex);
            Assert.That(itemsWithGaps.Count, Is.LessThan(expected.Count));
            var sut = MakeSut();

            sut.FillGaps(itemsWithGaps);

            Assert.That(itemsWithGaps.Count, Is.EqualTo(expected.Count));
            itemsWithGaps.ShouldAllBeEquivalentTo(expected);
        }

        [TestCase(100, 1000)]
        [TestCase(10, 100)]
        [TestCase(1, 10)]
        [TestCase(0.5, 5)]
        [TestCase(0.1, 1)]
        [TestCase(0.01, 0.1)]
        public void FillGaps_GapWithMoreThanOneInterval_FillsGap(decimal interval, decimal upperBound)
        {
            _interval = interval;
            var expected = MakeTestDataCollection(upperBound, interval);
            var itemsWithGaps = MakeTestDataCollection(upperBound, interval);
            // Need to remove items with larger index first, or they won't be there.
            itemsWithGaps.RemoveAt(4);
            itemsWithGaps.RemoveAt(3);
            itemsWithGaps.RemoveAt(2);
            itemsWithGaps.RemoveAt(1);
            Assert.That(itemsWithGaps.Count, Is.EqualTo(6));
            var sut = MakeSut();

            sut.FillGaps(itemsWithGaps);

            Assert.That(itemsWithGaps.Count, Is.EqualTo(expected.Count));
            itemsWithGaps.ShouldAllBeEquivalentTo(expected);
        }

        private GapFiller<TestData> MakeSut()
        {
            return new GapFiller<TestData>(_interval);
        }


        private List<TestData> MakeTestDataCollection(decimal upperBound, decimal step)
        {
            var result = new List<TestData>();
            for (decimal i = 0; i < upperBound; i += step)
            {
                var data = MakeTestData(i);
                result.Add(data);
            }
            return result;
        }

        private TestData MakeTestData(decimal offset)
        {
            var result = new TestData { Offset = offset };
            return result;
        }
    }
}
