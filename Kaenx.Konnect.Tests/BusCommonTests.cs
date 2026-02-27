using Kaenx.Konnect.Classes;
using System.Net;
using Kaenx.Konnect.Addresses;
using Xunit;

namespace Kaenx.Konnect.Tests
{
    public class BusCommonTests
    {
        [Fact]
        public async Task GroupValueWrite_SendsPercentage_false()
        {
            var endpoint = new IPEndPoint(IPAddress.Parse("192.168.178.167"), 3671);

            var conn = KnxFactory.CreateTunnelingUdp(endpoint);
            await conn.Connect();
            var sut = new BusCommon(conn); 

            MulticastAddress ga = MulticastAddress.FromString("1/4/53");
            byte data = 0x00;
            await sut.GroupValueWrite(ga, data);
        }

        [Fact]
        public async Task GroupValueWrite_SendsByteArray()
        {
            var endpoint = new IPEndPoint(IPAddress.Parse("192.168.178.167"), 3671);
            var conn = KnxFactory.CreateTunnelingUdp(endpoint);
            await conn.Connect();

            var sut = new BusCommon(conn);
            MulticastAddress ga = MulticastAddress.FromString("1/4/58");

            byte[] data = new byte[] { 0x33 }; // 20% - DPT 5.001
            await sut.GroupValueWrite(ga, data);

            await conn.Disconnect();
        }
    }
}

