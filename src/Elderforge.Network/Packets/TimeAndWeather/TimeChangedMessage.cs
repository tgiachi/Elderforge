using Elderforge.Network.Interfaces.Messages;
using Elderforge.Shared.Types;
using ProtoBuf;

namespace Elderforge.Network.Packets.TimeAndWeather;

[ProtoContract]
public class TimeChangedMessage : INetworkMessage
{
    [ProtoMember(1)] public int Hours { get; set; }

    [ProtoMember(2)] public int Minutes { get; set; }


    [ProtoMember(3)] public float NormalizedTime { get; set; }

    [ProtoMember(4)] public DayPhase Phase { get; set; }

    [ProtoMember(5)] public bool IsDayTime { get; set; }


    public override string ToString()
    {
        return $"{Hours:00}:{Minutes:00} ({Phase})";
    }
}
