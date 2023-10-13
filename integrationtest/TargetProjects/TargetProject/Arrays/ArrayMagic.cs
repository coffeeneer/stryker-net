namespace TargetProject.Arrays;

public class ArrayMagic
{
    public static object[] MergeSomeArrays()
    {
        int[] explicitIntArray = new int[] { 1, 2, 3 };
        double[] explicitDoubleArray = new double[] { 1.5, 2.75, 3.25 };
        bool[] explicitBoolArray = new bool[] { true, false, true };
        string[] explicitStringArray = new string[] { "apple", "banana", "orange" };
        char[] explicitCharArray = new char[] { 'a', 'b', 'c' };

        int[] implicitIntArray = new[] { 4, 5, 6 };
        double[] implicitDoubleArray = new[] { 4.5, 5.75, 6.25 };
        bool[] implicitBoolArray = new[] { false, true, false };
        string[] implicitStringArray = new[] { "grape", "pear", "kiwi" };
        char[] implicitCharArray = new[] { 'd', 'e', 'f' };

        object[] mergedArray = new object[explicitIntArray.Length + explicitDoubleArray.Length + explicitBoolArray.Length + explicitStringArray.Length + explicitCharArray.Length + implicitIntArray.Length + implicitDoubleArray.Length + implicitBoolArray.Length + implicitStringArray.Length + implicitCharArray.Length];

        explicitIntArray.CopyTo(mergedArray, 0);
        explicitDoubleArray.CopyTo(mergedArray, explicitIntArray.Length);
        explicitBoolArray.CopyTo(mergedArray, explicitIntArray.Length + explicitDoubleArray.Length);
        explicitStringArray.CopyTo(mergedArray, explicitIntArray.Length + explicitDoubleArray.Length + explicitBoolArray.Length);
        explicitCharArray.CopyTo(mergedArray, explicitIntArray.Length + explicitDoubleArray.Length + explicitBoolArray.Length + explicitStringArray.Length);

        implicitIntArray.CopyTo(mergedArray, explicitIntArray.Length + explicitDoubleArray.Length + explicitBoolArray.Length + explicitStringArray.Length + explicitCharArray.Length);
        implicitDoubleArray.CopyTo(mergedArray, explicitIntArray.Length + explicitDoubleArray.Length + explicitBoolArray.Length + explicitStringArray.Length + explicitCharArray.Length + implicitIntArray.Length);
        implicitBoolArray.CopyTo(mergedArray, explicitIntArray.Length + explicitDoubleArray.Length + explicitBoolArray.Length + explicitStringArray.Length + explicitCharArray.Length + implicitIntArray.Length + implicitDoubleArray.Length);
        implicitStringArray.CopyTo(mergedArray, explicitIntArray.Length + explicitDoubleArray.Length + explicitBoolArray.Length + explicitStringArray.Length + explicitCharArray.Length + implicitIntArray.Length + implicitDoubleArray.Length + implicitBoolArray.Length);
        implicitCharArray.CopyTo(mergedArray, explicitIntArray.Length + explicitDoubleArray.Length + explicitBoolArray.Length + explicitStringArray.Length + explicitCharArray.Length + implicitIntArray.Length + implicitDoubleArray.Length + implicitBoolArray.Length + implicitStringArray.Length);

        return mergedArray;
    }
}
