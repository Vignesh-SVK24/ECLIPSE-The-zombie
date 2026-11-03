using UnityEngine;

namespace Eclipse.Networking.Players
{
    public interface IPlayerIdentity
    {
        ulong ClientId { get; }
        string PlayerName { get; }
        bool IsReady { get; }
        bool IsHost { get; }
        int PingMilliseconds { get; }
        GameObject PlayerGameObject { get; }
    }
}
