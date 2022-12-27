namespace WYDestiny.PacketSniffer.Core
{
    /// Represents the default WYDestiny Packet Header.
    public class PacketHeader
    {
        public ushort Size;
        public byte Key;
        public byte Checksum;
        public ushort PacketId;
        public ushort ClientId;
        public uint TimeStamp;

        /// Parses a buffer containing the WYDestiny Packet Header data.
        /// <param name="pBuffer">The unsafe pointer to the first element of the buffer.</param>
        /// <returns>A new PacketHeader object.</returns>
        public static unsafe PacketHeader Parse(byte* pBuffer)
        {
            var header = new PacketHeader();

            header.Size = *(ushort*)&pBuffer[0];
            header.Key = pBuffer[2];
            header.Checksum = pBuffer[3];
            header.PacketId = *(ushort*)&pBuffer[4];
            header.ClientId = *(ushort*)&pBuffer[6];
            header.TimeStamp = *(uint*)&pBuffer[8];

            return header;
        }
    }
}
