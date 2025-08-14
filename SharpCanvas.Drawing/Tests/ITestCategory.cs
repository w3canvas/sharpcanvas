using System.Collections.Generic;

namespace SharpCanvas.Tests
{
    public delegate string TestCase(ICanvasRenderingContext2D ctx);

    public interface ITestCategory
    {
        string Name { get; }
        List<TestCase> GetTestCases();
    }
}