using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Subject<T>
{
    private List<IObserver<T>> observers = new List<IObserver<T>>();

    public void Attach(IObserver<T> observer)
    {
        this.observers.Add(observer);
    }

    public void Detach(IObserver<T> observer)
    {
        this.observers.Remove(observer);
    }

    public void Notify(T args)
    {
        foreach (var observer in this.observers)
        {
            observer.OnNotify(args);
        }
    }
}

public interface IObserver<T>
{
    void OnNotify(T args);
}