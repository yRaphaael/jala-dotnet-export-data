using System;
using System.Collections.Generic;

public class Subject
{
    private readonly List<IObservable> observers = [];

    public void Add(IObservable observer)
    {
        observers.Add(observer);
    }

    public void Remove(IObservable observer)
    {
        observers.Remove(observer);
    }

    public void Notify(string message)
    {
        try {
            foreach (var observer in observers)
            {
                observer.UpdateMessage(message);
            }
        } catch (Exception err) {
            Console.WriteLine(err.Message);
        }
    }
}
