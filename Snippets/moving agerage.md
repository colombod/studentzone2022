# setup

## setup tests
```csharp
    [Fact]
    public void calculates_as_data_is_pushed()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public void does_not_produce_value_if_window_is_not_complete(double[] samples, int windowSizeInSamples)
    {
       throw new NotImplementedException();
    }
```

## final shape
```csharp
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
```

```csharp
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace SignalProcessing;

// we sample at constant rate, so we only care to define sliding widow as a sample size and not time interval
public class MovingAverage : IObservable<double>
{
    private readonly Subject<double> _subject = new();
    private readonly IObservable<double> _movingWindow;

    public MovingAverage(int windowSizeInSamples)
    {
        _movingWindow = _subject
            .Buffer(windowSizeInSamples, 1)           // sliding window of size windowSizeInSamples that slides of 1 value
            .Select(b => b.Average());              // calculate the average of the window
    }

    public void PushSample(double sample)
    {
        _subject.OnNext(sample);
    }

    public IDisposable Subscribe(IObserver<double> observer)
    {
        return _movingWindow.Subscribe(observer);
    }
}
```


```csharp
public class MovingAverage
{
    private int windowSize;
    public double Average { get; private set; }

    private readonly Queue<double> samples = new Queue<double>();
    public MovingAverage(int windowSize)
    {
        if (windowSize <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(windowSize));
        }
        Average = double.NaN;
        this.windowSize = windowSize;
    }

    public void PushSample(double sample)
    {
        samples.Enqueue(sample);

        if (samples.Count == windowSize)
        {
            Average = samples.Average();
            samples.Dequeue();
        }        
    }
}
```

```csharp
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
```