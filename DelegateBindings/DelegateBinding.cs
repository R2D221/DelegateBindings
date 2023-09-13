using Nito.Mvvm.CalculatedProperties;
using System.ComponentModel;
using System.Threading.Tasks;
using System;

namespace DelegateBindings;

/// <summary>
/// An action that will be invoked whenever a property inside it
/// changes.
/// </summary>
public sealed class DelegateBinding : IDisposable
{
	internal static bool isRunning = false;
	private readonly PropertyHelper properties;
	private Action? action;

	internal DelegateBinding(Action action)
	{
		this.properties = new(RaisePropertyChanged);
		this.action = action;
		Run();
	}

	private void RaisePropertyChanged(PropertyChangedEventArgs args)
	{
		Run();
	}

	private async void Run()
	{
		// R2D221 - 2023-08-19
		// isRunning is a static variable. We don't want to run the
		// action recursively since it messes up with the change
		// tracking from Nito.Mvvm.CalculatedProperties.
		// We yield inside an async void function because to exit the
		// recursion we need to schedule the action for later.

		while (isRunning) { await Task.Yield(); }

		isRunning = true;
		try
		{
			_ = properties.Calculated<object?>(propertyName: "action", calculateValue: () =>
			{
				action?.Invoke();
				return null;
			});
		}
		finally
		{
			isRunning = false;
		}
	}

	/// <summary>
	/// Releases the binding so that the action will no longer be
	/// invoked.
	/// </summary>
	public void Dispose()
	{
		action = null;
	}
}