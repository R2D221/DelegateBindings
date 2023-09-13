using Nito.Disposables;
using System;
using System.Collections.Generic;

namespace DelegateBindings;

internal sealed class DelegateBindingObservable<T> : IObservable<T>
{
	private readonly HashSet<IObserver<T>> observers = new();

	public DelegateBindingObservable(Func<T> valueFunc)
	{
		var x = new DelegateBinding<T>(valueFunc, Action);
	}

	private void Action(T value)
	{
		foreach (var observer in observers)
		{
			observer.OnNext(value);
		}
	}

	public IDisposable Subscribe(IObserver<T> observer)
	{
		_ = observers.Add(observer);

		return new Disposable(() => _ = observers.Remove(observer));
	}
}
