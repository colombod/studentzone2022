using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace SignalProcessing;

public class MovingAverage
{
    private readonly int windowSize;
    private readonly Queue<double> samples = new();
    public MovingAverage(int windowSize)
    {
        Average = double.NaN;

        if (windowSize <= 0)
        {
            throw new ArgumentOutOfRangeException($"{nameof(windowSize)} should be greater than zero");
        }
        this.windowSize = windowSize;
    }

    public double Average { get; private set; }

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