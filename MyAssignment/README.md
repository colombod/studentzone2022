# Ace your next assignment with .NET

How to get it done? The important question here is : **what does *DONE* mean ?**. When creating software is important to be sure we are building the right thing, the code is fulfulling the expectation our our users and customer. To do that we start by pinning down the metrics that define success.

We then code those as tests we want to get passing. 

Testing is powerful tool read more [here](https://learn.microsoft.com/en-us/training/modules/visual-studio-test-concepts/) later.

Testing is not jsut a verification that what we have works, is a practice that helps in design and refactoring. Think of testing as a canvas and safety net at the same time.

In Visual Studio there is some interesting tool that will get us focusing only on the product developemtn because our pinned behaviour are tested as we go, this si called **Live Unit Testing** or **LUT**.

As we develop our classes the tests are continuolsy run and we will get feedback directly in the code editor.

At school our teachers becomes customers nad the **Test Driven Development** approach can help us remove the question **will I pass?**. Of course there si no guarantedd on the grade but with the right question and clarification upfront you can be 100% sure that the assigment is done correctly.

Let's revisit the exercise so far, there are few parts that we should really focus on!

```
produce a streaming with the moving average from sensor reading.
```

Step one, how do I make sure this is working? Moving average is computed across a sliding window of values, eveytime such a windo is complete the average of the values in teh windo is created. So we can write some tests!

```csharp
    var movingAverage = new MovingAvegage(2);
    var observed = double.Nan;
    movingAverage.Subscribe(m => observed = m);
    movingAverage.PushSample(1);
    movingAverage.PushSample(1);
    Assert.AreEqual(0.5, observed)
```

Kind of good but having some more interesting message when thigns are wrong would be cool and useful, let use Fluent Assertions:

```csharp
    observed.Should().Be(0.5);
```

Seems good but before we implement we can keep on using this example to test our theory. We can make our test parametric and keep on reusing the code.

```csharp
    [Theory]
    [InlineData(new[] {1.0},1, 1.0)]
    [InlineData(new[] { 1.0,1.0 }, 1, 1.0)]
    [InlineData(new[] { 1.0, 2.0 },2, 1.5)]
    [InlineData(new[] { 1.0, 2.0,1.0,4.0 },2, 2.5)]
    public void calculates_as_data_is_pushed(double[] samples, int windowSize, double expected)
    {
        var observed = double.NaN;
        var movingAverage = new MovingAverage(windowSize);
        movingAverage.Subscribe(m => observed = m);
        foreach (var sample in samples)
        {
            movingAverage.PushSample(sample);
        }
        observed.Should().Be(expected);
    }
```

Now lets turn on **LUT** and let's trust refactoring a littel bit.

Ok we compile but it doesn't work! Well this is how far the refactoring tools have helpd.

```csharp
public class MovingAverage
{
    public MovingAverage(int windowSize)
    {
        throw new NotImplementedException();
    }

    public void Subscribe(Action<double> func)
    {
        throw new NotImplementedException();
    }

    public void PushSample(double sample)
    {
        throw new NotImplementedException();
    }
}
```

Let's start building our code then and create the logic.