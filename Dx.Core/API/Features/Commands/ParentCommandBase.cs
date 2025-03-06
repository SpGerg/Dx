using System;
using System.Text;
using CommandSystem;
using Exiled.API.Features.Pools;

namespace Dx.Core.API.Features.Commands;

public abstract class ParentCommandBase<T> : ParentCommand where T : ICommand
{
    public ParentCommandBase()
    {
        LoadGeneratedCommands();
        
        _stringBuilderPool = StringBuilderPool.Pool.Get();
        _stringBuilderPool.AppendLine($"{Command}: ");

        foreach (var child in Children)
        {
            _stringBuilderPool.AppendLine($"  - {child.Command}: {child.Description}");
        }
    }

    ~ParentCommandBase()
    {
        StringBuilderPool.Pool.Return(_stringBuilderPool);
    }

    public abstract T[] Children { get; }

    private readonly StringBuilder _stringBuilderPool;

    public override void LoadGeneratedCommands()
    {
        foreach (var child in Children)
        {
            RegisterCommand(child);
        }
    }

    protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        response = _stringBuilderPool.ToString();
        return true;
    }
}