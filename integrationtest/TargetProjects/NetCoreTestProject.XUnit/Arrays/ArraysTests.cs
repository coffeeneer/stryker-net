using TargetProject.Arrays;
using Xunit;

namespace NetCoreTestProject.XUnit.Arrays;

public class ArraysTests
{
    [Fact]
    public void TestArrayMagic()
    {
        var result = ArrayMagic.MergeSomeArrays();

        Assert.Equal(22, result.Length);
    }
}
