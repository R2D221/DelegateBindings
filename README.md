# DelegateBindings
<a href="https://www.nuget.org/packages/DelegateBindings"><img src="https://img.shields.io/nuget/v/DelegateBindings" /></a>

Easy property bindings using delegates, with the help of
Nito.Mvvm.CalculatedProperties.

## How to use it

First you need view models implemented like this:

```csharp
public class RootViewModel : INotifyPropertyChanged
{
	private readonly PropertyHelper properties;

	public RootViewModel()
	{
		properties = new(RaisePropertyChanged);
	}

	public event PropertyChangedEventHandler? PropertyChanged;

	private void RaisePropertyChanged(PropertyChangedEventArgs args) =>
		PropertyChanged?.Invoke(this, args);

	public string Text
	{
		get => properties.Get("");
		set => properties.Set(value);
	}

	public ObservableCollection<Item> Items =>
		properties.Get(() => new ObservableCollection<Item>());
}

public class ItemViewModel : INotifyPropertyChanged
{
	private readonly PropertyHelper properties;

	public ItemViewModel()
	{
		properties = new(RaisePropertyChanged);
	}

	public event PropertyChangedEventHandler? PropertyChanged;

	private void RaisePropertyChanged(PropertyChangedEventArgs args) =>
		PropertyChanged?.Invoke(this, args);

	public string Content
	{
		get => properties.Get("");
		set => properties.Set(value);
	}
}
```

Then, it's just as simple as calling the binding like this:

```csharp
using static DelegateBindings.DelegateBinder;

// ...

var binding1 = Bind(() => label1.Text = viewModel.Text);
var binding2 = Bind(() => label2.Text = viewModel.Items.FirstOrDefault()?.Content);
```

Whenever `viewModel.Text` changes, `label1.Text` will be changed as well.

It also works for observable collections. Whenever the `Items` list is updated, 
or the first item's `Content` is updated, `label2.Text` will also change.

## Remarks

* The `Bind()` method receives an `Action`, not an `Expression`, which means
  you're not limited to just assignments. It's recommended that you keep the
  actions simple tho.
* The view models MUST be implemented with Nito.Mvvm.CalculatedProperties. If
  you're using other libraries, the bindings WON'T WORK.
* Bindings are one-way. I'm planning to add two-way binding in the future but
  for now you can do two bindings like this:
  ```csharp
  Bind(() => viewModel2.Result = viewModel1.Text);
  Bind(() => viewModel1.Text = viewModel2.Result);
  ```
  Or, for UI controls (WinForms, WPF, etc), use events directy:
  ```csharp
  Bind(() => textBox.Text = viewModel1.Text);
  textBox.Validated += (_, _) => viewModel1.Text = textBox.Text;
  ```

## License

MIT
