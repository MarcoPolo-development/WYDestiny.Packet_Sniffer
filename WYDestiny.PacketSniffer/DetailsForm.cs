using System;
using System.Text;
using System.Windows.Forms;
using WYDestiny.PacketSniffer.Core;
using Be.Windows.Forms;

namespace WYDestiny.PacketSniffer
{
    public partial class DetailsForm : Form
    {
        public DetailsForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes the details form with the given packet data.
        /// </summary>
        /// <param name="packet"></param>
        public void InitializePacketDetails(WYDestinyPacketEventArgs packet)
        {
            hexPacketView.ByteProvider = new DynamicByteProvider(packet.Buffer);

            StringBuilder sb = new StringBuilder();
            sb.Append("Details of Packet: ")
                .Append($"0x{packet.PacketHeader.PacketId.ToString("X2")} ")
                .Append(packet.Direction.ToString())
                .Append($" Size: {packet.PacketHeader.Size}");


            txtPacketCheckSum.Text = packet.PacketHeader.Checksum.ToString();
            txtPacketClientId.Text = packet.PacketHeader.ClientId.ToString();
            txtPacketKey.Text = packet.PacketHeader.Key.ToString();
            txtPacketOpCode.Text = packet.PacketHeader.PacketId.ToString("X2");
            txtPacketSize.Text = packet.PacketHeader.Size.ToString();
            txtPacketTimeStamp.Text = packet.PacketHeader.TimeStamp.ToString();

            this.Text = sb.ToString();
        }

        private byte[] tmpBuffer;

        private unsafe void hexPacketView_PositionChanged(object sender, EventArgs e)
        {
            var currentSelectionStart = hexPacketView.SelectionStart;

            if (tmpBuffer == null)
                tmpBuffer = new byte[sizeof(ulong)];

            var byteProviderLength = hexPacketView.ByteProvider.Length;

            if ((currentSelectionStart + sizeof(ulong)) <= byteProviderLength)
            {
                for (int i = 0; i < sizeof(ulong); i++)
                    tmpBuffer[i] = hexPacketView.ByteProvider.ReadByte(currentSelectionStart + i);

                fixed (byte* pBuffer = tmpBuffer)
                {
                    var asUint64 = *(ulong*)&pBuffer[0];
                    txtAsUInt64.Text = asUint64.ToString();
                    txtAsInt64.Text = ((long)asUint64).ToString();
                }
            }
            else
                txtAsUInt64.Text = txtAsInt64.Text = "Not enough data.";

            if ((currentSelectionStart + sizeof(uint)) <= byteProviderLength)
            {
                for (int i = 0; i < sizeof(uint); i++)
                    tmpBuffer[i] = hexPacketView.ByteProvider.ReadByte(currentSelectionStart + i);

                fixed (byte* pBuffer = tmpBuffer)
                {
                    var asUInt32 = *(uint*)&pBuffer[0];
                    txtAsUInt32.Text = asUInt32.ToString();
                    txtAsInt32.Text = ((int)asUInt32).ToString();
                }
            }
            else
                txtAsInt32.Text = txtAsUInt32.Text = "Not enough data.";

            if ((currentSelectionStart + sizeof(ushort)) <= byteProviderLength)
            {
                for (int i = 0; i < sizeof(ushort); i++)
                    tmpBuffer[i] = hexPacketView.ByteProvider.ReadByte(currentSelectionStart + i);

                fixed (byte* pBuffer = tmpBuffer)
                {
                    var asUInt16 = *(ushort*)&pBuffer[0];
                    txtAsUShort.Text = asUInt16.ToString();
                    txtAsShort.Text = ((short)asUInt16).ToString();
                }
            }
            else
                txtAsUShort.Text = txtAsShort.Text = "Not enough data.";

            if ((currentSelectionStart + sizeof(byte)) <= byteProviderLength)
            {
                tmpBuffer[0] = hexPacketView.ByteProvider.ReadByte(currentSelectionStart);

                var asByte = tmpBuffer[0];
                txtAsByte.Text = asByte.ToString();
                txtAsSByte.Text = ((sbyte)asByte).ToString();
            }
            else
                txtAsByte.Text = txtAsSByte.Text = "Not enough data.";
        }
    }
}
