using Nito.Mvvm.CalculatedProperties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace DelegateBindings;

public sealed class DelegateBinding<T> : IDisposable
{
	private readonly IEqualityComparer<T?> equality = EqualityComparer<T?>.Default;
	private readonly PropertyHelper properties;
	private T? value;
	private (Func<T> valueFunc, Action<T> action)? state;

	public DelegateBinding(Func<T> valueFunc, Action<T> action)
	{
		this.properties = new(RaisePropertyChanged);
		this.state = (valueFunc, action);
		Run();
	}

	private void RaisePropertyChanged(PropertyChangedEventArgs args)
	{
		Run();
	}

	private async void Run()
	{
		while (DelegateBinding.isRunning) { await Task.Yield(); }

		try
		{
			DelegateBinding.isRunning = true;
			T? newValue;
			try
			{
				newValue = properties.Calculated(propertyName: "value", calculateValue: () =>
				{
					switch (state)
					{
					case ({ } valueFunc, _):
					{
						return valueFunc();
					}
					default:
					{
						throw new ObjectDisposedException("DelegateBinding");
					}
					}
				});
			}
			finally
			{
				DelegateBinding.isRunning = false;
			}

			if (equality.Equals(value, newValue)) { return; }

			value = newValue;
			state?.action(value);
		}
		catch (ObjectDisposedException ex) when (ex.ObjectName is "DelegateBinding")
		{
			// if the event was invoked after disposal we just ignore it.
		}
	}

	public void Dispose()
	{
		state = null;
		value = default;
	}
}
