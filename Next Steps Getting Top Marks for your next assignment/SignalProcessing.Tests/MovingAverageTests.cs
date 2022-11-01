using FluentAssertions;
namespace SignalProcessing.Tests;

public class MovingAverageTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void must_be_create_with_a_positive_and_non_zero_window(int windowSize)
    {
        var creation = () => new MovingAverage(windowSize);
        creation.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Theory]
    [InlineData(new[] { 1.0 }, 1, 1.0)]
    [InlineData(new[] { 1.0, 1.0 }, 1, 1.0)]
    [InlineData(new[] { 1.0, 2.0 }, 2, 1.5)]
    [InlineData(new[] { 1.0, 2.0, 1.0, 4.0 }, 2, 2.5)]
    [InlineData(new[] { 1.0, 2.0, 1.0, 3.0 }, 3, 2.0)]
    public void calculates_as_data_is_pushed(double[] samples, int windowSizeInSamples, double expected)
    {
        var movingAverage = new MovingAverage(windowSizeInSamples);

        foreach (var sample in samples)
        {
            movingAverage.PushSample(sample);
        }
        movingAverage.Average.Should().Be(expected);
    }

    [Theory]
    [InlineData(new[] { 1.0 })]
    [InlineData(new[] { 1.0, 1.0 })]
    [InlineData(new[] { 1.0, 2.0 })]
    [InlineData(new[] { 1.0, 2.0, 1.0, 4.0 })]
    public void does_not_produce_value_if_window_is_not_complete(double[] samples)
    {
        var movingAverage = new MovingAverage(samples.Length + 1);
        foreach (var sample in samples)
        {
            movingAverage.PushSample(sample);
        }
        
        movingAverage.Average.Should().Be(double.NaN);
    }
} 