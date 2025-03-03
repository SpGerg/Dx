namespace Dx.Core.API.Features.Commands
{
    public class CommandResponse
    {
        public readonly static CommandResponse Successful = new CommandResponse()
        {
            Response = "Команда успешно использована",
            Success = true
        };
        
        public string Response { get; init; }

        public bool Success { get; init; }
    }
}