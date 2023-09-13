using System;

namespace DelegateBindings;

/// <summary>
/// Entry point for binding properties using delegates.
/// </summary>
public static class DelegateBinder
{
	/// <summary>
	/// Invokes the action, and binds it so that whenever a property
	/// in it changes, the action is invoked again.
	/// </summary>
	/// <param name="action">
	/// The action that represents the binding.
	/// </param>
	/// <returns>
	/// A <see cref="DelegateBinding"/> that can be disposed to
	/// release the binding.
	/// </returns>
	public static DelegateBinding Bind(Action action)
	{
		return new DelegateBinding(action);
	}

	/// <summary>
	/// Invokes the action whenever any property referenced in the
	/// value function changes. The updated value is passed as a
	/// parameter to the action.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="valueFunc">
	/// The function that will listen to changes in properties.
	/// </param>
	/// <param name="action">
	/// The action that will run whenever any property in valueFunc
	/// changes.
	/// </param>
	/// <returns>
	/// A <see cref="DelegateBinding{T}"/> that can be disposed to
	/// release the binding.
	/// </returns>
	public static DelegateBinding<T> WhenChanged<T>(Func<T> valueFunc, Action<T> action)
	{
		return new DelegateBinding<T>(valueFunc, action);
	}

	/// <summary>
	/// Listens to changes in any property referenced in the value
	/// function, as an <see cref="IObservable{T}"/>.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="valueFunc">
	/// The function that will listen to changes in properties.
	/// </param>
	/// <returns>
	/// An <see cref="IObservable{T}"/> that notifies subscribers
	/// whenever any property in valueFunc changes.
	/// </returns>
	public static IObservable<T> WhenChanged<T>(Func<T> valueFunc)
	{
		return new DelegateBindingObservable<T>(valueFunc);
	}
}
