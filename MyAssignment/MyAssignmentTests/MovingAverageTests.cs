using FluentAssertions;
using SignalProcessing;


namespace MyAssignmentTests;

public class MovingAverageTests
{
    [Theory]
    [InlineData(new[] {1.0},1, 1.0)]
    [InlineData(new[] { 1.0,1.0 }, 1, 1.0)]
    [InlineData(new[] { 1.0, 2.0 },2, 1.5)]
    [InlineData(new[] { 1.0, 2.0,1.0,4.0 },2, 2.5)]
    public void calculates_as_data_is_pushed(double[] samples, int windowSizeInSamples, double expected)
    {
        var observed = double.NaN;
        var movingAverage = new MovingAverage(windowSizeInSamples);
        movingAverage.Subscribe(m => observed = m);
        foreach (var sample in samples)
        {
            movingAverage.PushSample(sample);
        }
        observed.Should().Be(expected);
    }

    [Theory]
    [InlineData(new[] { 1.0 }, 2)]
    [InlineData(new[] { 1.0, 1.0 }, 3)]
    [InlineData(new[] { 1.0, 2.0 }, 4)]
    [InlineData(new[] { 1.0, 2.0, 1.0, 4.0 }, 6)]
    public void does_not_produce_value_if_window_is_not_complete(double[] samples, int windowSizeInSamples)
    {
        var observed = double.NaN;
        var movingAverage = new MovingAverage(windowSizeInSamples);
        movingAverage.Subscribe(m => observed = m);

        foreach (var sample in samples)
        {
            movingAverage.PushSample(sample);
        }

        observed.Should().Be(double.NaN);
    }

}

