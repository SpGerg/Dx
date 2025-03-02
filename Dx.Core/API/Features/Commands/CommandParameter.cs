namespace Dx.Core.API.Features.Commands;

public class CommandParameter
{
    public string Name { get; init; }
    
    public bool IsRequired { get; init; }
    
    public string DisplayName { get; init; }
}