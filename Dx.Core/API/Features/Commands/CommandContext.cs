using System;
using System.Collections.Generic;
using CommandSystem;
using Dx.Core.API.Extensions;
using Exiled.API.Features;
using UnityEngine;

namespace Dx.Core.API.Features.Commands
{
    public class CommandContext
    {
        public CommandContext(ICommandSender commandSender, IReadOnlyDictionary<string, string> arguments, ArraySegment<string> arraySegment)
        {
            CommandSender = commandSender;
            Arguments = arguments;
            ArraySegment = arraySegment;
        }

        public ICommandSender CommandSender { get; }
        
        public IReadOnlyDictionary<string, string> Arguments { get; }

        public ArraySegment<string> ArraySegment { get; }

        public bool TryParseVector3(int start, out Vector3 value)
        {
            if (!TryGetAt(start, out var xCoordinate) || !float.TryParse(xCoordinate, out var xValue))
            {
                value = Vector3.zero;
                return false;
            }
            
            if (!TryGetAt(start + 1, out var yCoordinate) || !float.TryParse(yCoordinate, out var yValue))
            {
                value = Vector3.zero;
                return false;
            }
            
            if (!TryGetAt(start + 2, out var zCoordinate) || !float.TryParse(zCoordinate, out var zValue))
            {
                value = Vector3.zero;
                return false;
            }

            value = new Vector3(xValue, yValue, zValue);
            return true;
        }

        public bool TryGetPlayersOrPlayer(string name, out IReadOnlyCollection<Player> players)
        {
            if (!TryGet(name, out var id))
            {
                players = null;
                return false;
            }

            if (id.IsEveryone())
            {
                players = Player.List;
                return true;
            }

            if (Player.TryGet(id, out var player))
            {
                players = new List<Player>()
                {
                    player
                };
                
                return true;
            }

            players = null;
            return false;
        }

        public bool TryGet(string name, out string value)
        {
            return Arguments.TryGetValue(name, out value);
        }
        
        public string Get(string name)
        {
            return Arguments[name];
        }

        public bool TryGetAt(int index, out string value)
        {
            if (ArraySegment.Count - 1 < index)
            {
                value = null;
                return false;
            }

            value = ArraySegment.At(index);
            return true;
        }
    }
}