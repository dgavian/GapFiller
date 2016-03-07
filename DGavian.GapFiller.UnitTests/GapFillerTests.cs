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
        private double _interval;

        [TestCase(10, 100)]
        [TestCase(1, 10)]
        [TestCase(0.5, 5)]
        [TestCase(0.1, 1)]
        public void FillGaps_BasicCollectionWithGaps_FillsGaps(double interval, double upperBound)
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

        [TestCase(10, 100)]
        [TestCase(1, 10)]
        [TestCase(0.5, 5)]
        [TestCase(0.1, 1)]
        public void FillGaps_GapWithMoreThanOneInterval_FillsGap(double interval, double upperBound)
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


        private List<TestData> MakeTestDataCollection(double upperBound, double step)
        {
            // Convert to decimals to avoid floating point precision errors.
            var upperBoundDecimal = (decimal)upperBound;
            var stepDecimal = (decimal)step;
            var result = new List<TestData>();
            for (decimal i = 0; i < upperBoundDecimal; i += stepDecimal)
            {
                var interval = (double)i;
                var data = MakeTestData(interval);
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
