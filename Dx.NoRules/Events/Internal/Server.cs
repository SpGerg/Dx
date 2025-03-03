using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using Exiled.Events.EventArgs.Server;
using MEC;
using EventTarget = Exiled.Events.Handlers.Server;

namespace Dx.NoRules.Events.Internal
{
    internal static class Server
    {
        public static void Register()
        {
            EventTarget.RoundStarted += BlockDoorsAndOffLightOnRoundStarted;
        }
        
        public static void Unregister()
        {
            EventTarget.RoundStarted -= BlockDoorsAndOffLightOnRoundStarted;
        }

        /// <summary>
        /// Заблокировать двери, выключить свет и вывести кэсси при старте раунда.
        /// </summary>
        private static void BlockDoorsAndOffLightOnRoundStarted()
        {
            //Другие плагины могут блокировать до начала раунда и также их открывать, заблокируем после открытия.
            Timing.CallDelayed(Timing.WaitForOneFrame, () =>
            {
                foreach (var door in Door.List)
                {
                    door.Lock(5f, DoorLockType.AdminCommand);
                }
                
                Exiled.API.Features.Map.TurnOffAllLights(5f);
                
                Plugin.Config.CassieMessageAtRoundStart.Speak();
            });
        }
    }
}