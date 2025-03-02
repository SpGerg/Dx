using System.ComponentModel;
using UnityEngine;

namespace Dx.Lobby.API.Features.Serializables
{
    public class LobbySchematicSerializable
    {
        [Description("Имя схемата")]
        public string SchematicName { get; set; }
        
        [Description("Позиция схемата")]
        public Vector3 Position { get; set; }
        
        [Description("Позиция спавна игроков")]
        public Vector3 SpawnPosition { get; set; }
    }
}