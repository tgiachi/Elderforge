namespace Elderforge.Network.Types;

public enum NetworkMessageType : byte
{
    Version,
    Ping,
    Pong,
    Chat,
    Motd,
    MotdRequest,
    ServerReady,
    ServerShutdown,

    LoginRequest,
    LoginResponse,

    WorldChunkRequest,
    WorldChunkResponse,

    GameObjectDestroyResponse,
    GameObjectActionRequest,

    LightUpdateResponse,


    PlayerMoveRequest,
    PlayerMoveResponse,
}
