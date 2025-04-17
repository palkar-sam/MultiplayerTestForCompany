namespace Utils
{
    public class BitWriter
    {
        public int BitsWritten => bitPosition;
        public int BytesWritten => (bitPosition + 7) / 8; // ceil

        private readonly byte[] buffer;
        private int bitPosition;


        public BitWriter(byte[] buffer)
        {
            this.buffer = buffer;
            bitPosition = 0;
        }

        public void WriteBits(int value, int bitCount)
        {
            for (int i = 0; i < bitCount; i++)
            {
                int bit = (value >> i) & 1;
                int bytePos = bitPosition / 8;
                int bitOffset = bitPosition % 8;
                buffer[bytePos] |= (byte)(bit << bitOffset);
                bitPosition++;
            }
        }

        public byte[] GetBuffer() => buffer;
    }
}