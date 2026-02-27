using Kaenx.Konnect.Addresses;
using Kaenx.Konnect.Classes;
using Kaenx.Konnect.EMI.DataMessages;
using Kaenx.Konnect.EMI.LData;
using System.Net;
using Xunit;

namespace Kaenx.Konnect.Tests
{
    public class BusCommonTests
    {
        [Fact]
        public async Task GroupValueWrite_Sends_false()
        {
            var endpoint = new IPEndPoint(IPAddress.Parse("192.168.178.167"), 3671);

            var conn = KnxFactory.CreateTunnelingUdp(endpoint);
            await conn.Connect();
            var sut = new BusCommon(conn); 

            MulticastAddress ga = MulticastAddress.FromString("1/4/53");
            await sut.GroupValueWrite(ga, false);
        }

        [Fact]
        public async Task GroupValueWrite_SendsByteArray()
        {
            var endpoint = new IPEndPoint(IPAddress.Parse("192.168.178.167"), 3671);
            var conn = KnxFactory.CreateTunnelingUdp(endpoint);
            await conn.Connect();

            var sut = new BusCommon(conn);
            MulticastAddress ga = MulticastAddress.FromString("1/4/58");

            byte data = 0x33; // 20% - DPT 5.001
            await sut.GroupValueWrite(ga, data);

            await conn.Disconnect();
        }

        [Fact]
        public async Task GroupValueRead_ReceivesValueFromGA()
        {
            var endpoint = new IPEndPoint(IPAddress.Parse("192.168.178.167"), 3671);
            var conn = KnxFactory.CreateTunnelingUdp(endpoint);
            await conn.Connect();

            var sut = new BusCommon(conn);
            MulticastAddress ga = MulticastAddress.FromString("1/4/58");

            LDataBase? response = await sut.GroupValueRead(ga);
            GroupValueResponse? content = response?.GetContent<GroupValueResponse>();
            await conn.Disconnect();

            Assert.NotNull(content);
            Assert.NotEmpty(content.Data);
        }
    }
}

