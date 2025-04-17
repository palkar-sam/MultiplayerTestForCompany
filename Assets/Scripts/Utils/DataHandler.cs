using UnityEngine;

namespace Utils
{
    public class DataHandler
    {
        public Vector3 ReceivePos(byte[] buffer)
        {
            BitReader reader = new BitReader(buffer);

            int playerId = reader.ReadBits(5);
            int xPos = reader.ReadSignedBits(12);
            int yPos = reader.ReadSignedBits(12);
            int zPos = reader.ReadSignedBits(12);

            Vector3 pos = new Vector3(xPos / 10f, yPos / 10f, zPos / 10f);
            //float rotation = rotY;

            LoggerUtil.Log($"ReceiveData : Player {playerId} at {pos}");
            return pos;
        }

        public byte[] ConvertData(Vector3 posData)
        {
            byte[] buffer = new byte[11]; // 86 bits = 10.75 bytes
            BitWriter writer = new BitWriter(buffer);

            int playerId = 7;
            int xPos = Mathf.FloorToInt(posData.x * 10); // scale to int
            int yPos = Mathf.FloorToInt(posData.y * 10);
            int zPos = Mathf.FloorToInt(posData.z * 10);


            writer.WriteBits(playerId, 5);
            writer.WriteBits(xPos, 12);
            writer.WriteBits(yPos, 12);
            writer.WriteBits(zPos, 12);

            LoggerUtil.Log($"SendData : Player {playerId} at {posData} : Bits : {writer.BitsWritten} : Bytes : {writer.BytesWritten}");
            //Send Data here like opp.send(buffer);

            return buffer;
        }
    }
}
