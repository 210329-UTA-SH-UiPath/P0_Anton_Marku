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


    }
}
