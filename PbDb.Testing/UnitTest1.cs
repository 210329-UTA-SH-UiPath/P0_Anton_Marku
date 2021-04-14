using System;
using Xunit;
using PbDb.Storing.Entities;
using PbDb.Domain.Models;

namespace PbDb.Testing
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var sut = new RevenueRecord();
            sut.Year = 2;
            var actual = sut.Year;
            Assert.True(actual == 2);
        }

        [Fact]
        public void Test2()
        {
            var sut = new RevenueRecord();
            sut.Year = 1000;
            var actual = sut.Year;
            Assert.True(actual == 1000);
        }

        [Fact]
        public void Test3()
        {
            var sut = new RevenueRecord();
            sut.Year = 42;
            var actual = sut.Year;
            Assert.True(actual == 42);
        }

        [Fact]
        public void Test4()
        {
            var sut = new RevenueRecord();
            sut.Week = 42;
            var actual = sut.Week;
            Assert.True(actual == 42);
        }

        [Fact]
        public void Test5()
        {
            var sut = new RevenueRecord();
            sut.Month = 42;
            var actual = sut.Month;
            Assert.True(actual == 42);
        }

        [Fact]
        public void Test6()
        {
            var sut = new RevenueRecord();
            sut.Revenue = 42;
            var actual = sut.Revenue;
            Assert.True(actual == 42);
        }

        [Fact]
        public void Test7()
        {
            var sut = new RevenueRecord();
            sut.Revenue = 275;
            var actual = sut.Revenue;
            Assert.True(actual == 275);
        }

        [Fact]
        public void Test8()
        {
            var sut = new RevenueRecord();
            sut.Week = 300;
            var actual = sut.Week;
            Assert.True(actual == 300);
        }

        [Fact]
        public void Test9()
        {
            var sut = new RevenueRecord();
            sut.Revenue = (decimal)4000.1;
            var actual = sut.Revenue;
            Assert.True(actual == (decimal)4000.1);
        }

        [Fact]
        public void Test10()
        {
            var sut = new RevenueRecord();
            sut.Revenue = (decimal)42.765;
            var actual = sut.Revenue;
            Assert.True(actual == (decimal)42.765);
        }
    }
}
