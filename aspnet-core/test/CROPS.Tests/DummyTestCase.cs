using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace CROPS.Tests
{
    public class DummyTestCase 
    {
        [Theory]
        [InlineData(2, 2)]
        [InlineData(0, 0)]
        private int Add(int x, int y)
        {
            if (x == 0 && y == 0)
            {
                return 0;
            }
            else
            {
                return x + y;
            }
        }
    }
}
