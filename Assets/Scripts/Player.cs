using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private void ReceivePos(byte[] buffer)
    {
        BitReader reader = new BitReader(buffer);

        int playerId = reader.ReadBits(5);
        int xPos = reader.ReadBits(12);
        int yPos = reader.ReadBits(12);
        int zPos = reader.ReadBits(12);
        int rotY = reader.ReadBits(9);
        bool grounded = reader.ReadBits(1) == 1;
        int timestamp = reader.ReadBits(24);

        Vector3 pos = new Vector3(xPos / 10f, yPos / 10f, zPos / 10f);
        float rotation = rotY;

        Debug.Log($"Player {playerId} at {pos} RotY: {rotation} Grounded: {grounded} Time: {timestamp}");
    }

    private void SendData(Vector3 posData)
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
        
        //Send Data here like opp.send(buffer);
    }
}
