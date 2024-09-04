using System.CommandLine;
using System.CommandLine.Binding;
using TrackTime.Cli.Shared;

namespace TrackTime.Cli.Stamps.Model;

public record ListStampsModel(
    bool Today,
    DateTime? Day);

class ListStampsModelBinder(Option<bool> today, Argument<CustomDateTimeArgument?> day) : BinderBase<ListStampsModel>
{
    protected override ListStampsModel GetBoundValue(BindingContext bindingContext)
    {
        return new ListStampsModel(bindingContext.ParseResult.GetValueForOption(today), bindingContext.ParseResult.GetValueForArgument(day)?.Value);
    }
}