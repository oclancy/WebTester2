using ProtoBuf;

namespace Messages
{
    [ProtoContract]
    [ProtoInclude(1000, typeof(DataMessage))]
    [ProtoInclude(1001, typeof(TimeMessage))]
    [ProtoInclude(1002, typeof(StopMessage))]
    [ProtoInclude(1003, typeof(PriceRequestMessage))]
    [ProtoInclude(1004, typeof(PriceMessage))]
    public interface IMessage{ }

    [ProtoContract]
    public class DataMessage : IMessage
    {
        [ProtoMember(1)]
        public string Data { get; set; }
    }

    [ProtoContract]
    public class TimeMessage : IMessage
    {
        [ProtoMember(1)]
        public long Time { get; set; }
    }

    [ProtoContract]
    public class StopMessage : IMessage
    {
        [ProtoMember(1)]
        public string Reason { get; set; }
    }

    [ProtoContract]
    public class PriceRequestMessage : IMessage
    {
        [ProtoMember(1)]
        public string Topic { get; set; }
    }

    [ProtoContract]
    public class PriceMessage : IMessage
    {
        [ProtoMember(1)]
        public string Topic { get; set; }

        [ProtoMember(2)]
        public double BidPrice { get; set; }

        [ProtoMember(31)]
        public double OfferPrice { get; set; }
    }
}
