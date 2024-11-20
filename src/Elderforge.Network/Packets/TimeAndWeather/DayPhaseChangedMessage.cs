using Elderforge.Network.Interfaces.Messages;
using Elderforge.Shared.Types;
using ProtoBuf;

namespace Elderforge.Network.Packets.TimeAndWeather;

[ProtoContract]
public class DayPhaseChangedMessage : INetworkMessage
{
    [ProtoMember(1)] public DayPhase Phase { get; set; }

    public DayPhaseChangedMessage()
    {
    }

    public DayPhaseChangedMessage(DayPhase phase)
    {
        Phase = phase;
    }
}
