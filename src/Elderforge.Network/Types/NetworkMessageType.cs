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

    GameObjectCreateResponse,
    GameObjectDestroyResponse,
    GameObjectMoveResponse,


    PlayerMoveRequest,
    PlayerMoveResponse,

    LightResponse

}
