using System.CommandLine;
using System.CommandLine.Binding;

namespace TrackTime.Cli.Stamps.Model;

public class ListStampsModel
{
    public bool Today { get; init; }
}

class ListStampsModelBinder(Option<bool> today) : BinderBase<ListStampsModel>
{
    protected override ListStampsModel GetBoundValue(BindingContext bindingContext)
    {
        return new ListStampsModel { Today = bindingContext.ParseResult.GetValueForOption(today) };
    }
}