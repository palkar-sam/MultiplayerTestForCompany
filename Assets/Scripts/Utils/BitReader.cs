namespace Utils
{
    public class BitReader
    {
        private readonly byte[] buffer;
        private int bitPosition;

        public BitReader(byte[] buffer)
        {
            this.buffer = buffer;
            bitPosition = 0;
        }

        public int ReadBits(int bitCount)
        {
            int value = 0;
            for (int i = 0; i < bitCount; i++)
            {
                int bytePos = bitPosition / 8;
                int bitOffset = bitPosition % 8;
                int bit = (buffer[bytePos] >> bitOffset) & 1;
                value |= (bit << i);
                bitPosition++;
            }
            return value;
        }

        public int ReadSignedBits(int bitCount)
        {
            int raw = ReadBits(bitCount);
            int signBit = 1 << (bitCount - 1);
            return (raw ^ signBit) - signBit;
        }

        public int BitsRead => bitPosition;
        public int BytesRead => (bitPosition + 7) / 8;
    }
}
