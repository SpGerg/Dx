using System;
using System.Linq;
using CommandSystem;
using Exiled.API.Features;
using Exiled.API.Features.Pools;
using Exiled.Permissions.Extensions;

namespace Dx.Core.API.Features.Commands
{
    public abstract class CommandBase : ICommand
    {
        protected CommandBase()
        {
            var parameters = Parameters.Select(parameter => $"<{parameter.DisplayName}>").ToArray();
            
            Usage = $"{Command} {string.Join(" ", parameters)}";
            _requiredParametersCount = Parameters.Count(parameter => parameter.IsRequired);
        }
        
        public abstract string Command { get; }
        
        public abstract string[] Aliases { get; }
        
        public abstract string Description { get; }
        
        public virtual string[] Permissions { get; }
        
        public string Usage { get; }
        
        public abstract CommandParameter[] Parameters { get; }

        private readonly int _requiredParametersCount;

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (Permissions is not null && Permissions.Length > 0 && Player.TryGet(sender, out var player))
            {
                if (Permissions.Any(permission => !player.CheckPermission(permission)))
                {
                    response = "У вас недостаточно прав";
                    return false;
                }
            }
            
            if (arguments.Count < _requiredParametersCount)
            {
                response = $"Использование команды: {Usage}";
                return false;
            }
            
            var context = new CommandContext(sender, CommandParser.ParseArguments(Parameters, arguments.Array), arguments);

            CommandResponse commandResponse;
            
            try
            {
                commandResponse = Execute(context);
            }
            catch (Exception exception)
            {
                var stringBuilder = StringBuilderPool.Pool.Get();

                var guid = Guid.NewGuid();
                
                stringBuilder.AppendLine("Ошибка выполнения команды.");
                stringBuilder.AppendLine($"Уникальный номер ошибки: {guid}");
                
                Log.Error($"{guid}: {exception}");

                response = StringBuilderPool.Pool.ToStringReturn(stringBuilder);
                return false;
            }
            
            response = commandResponse.Response;
            return commandResponse.Success;
        }
        
        public abstract CommandResponse Execute(CommandContext context);
    }
}