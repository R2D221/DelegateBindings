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
}
