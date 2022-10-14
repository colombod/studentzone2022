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

//public class MovingAverage
//{
//    public MovingAverage(int windowSizeInSamples)
//    {
//        throw new NotImplementedException();
//    }

//    public void Subscribe(Action<double> func)
//    {
//        throw new NotImplementedException();
//    }

//    public void PushSample(double sample)
//    {
//        throw new NotImplementedException();
//    }
//}