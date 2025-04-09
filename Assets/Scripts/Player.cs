using Listeners;
using Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private bool isOpponent;
    [SerializeField] private PlayerMovement movements;

    private void Awake()
    {
        movements.OnMovement += OnMovement;
        movements.IsOpponent = isOpponent;
        if (isOpponent)
        {
            EventManager<PlayerMovementData>.StartListening(Props.GameEvents.NONE, OnRecievedPosition);
        }
    }

    private void Start()
    {

    }

    private void OnDestroy()
    {
        movements.OnMovement -= OnMovement;
    }

    private void OnRecievedPosition(PlayerMovementData data)
    {
        //controller.Move(data.Position * moveSpeed * Time.deltaTime);
        Vector3 newPos = ReceivePos(data.Position);
        movements.MovePlayer(newPos);
    }

    private void OnMovement(Vector3 newPos)
    {
        EventManager<PlayerMovementData>.TriggerEvent(Props.GameEvents.NONE, new PlayerMovementData()
        {
            Position = ConvertData(newPos)
        });
    }

    private Vector3 ReceivePos(byte[] buffer)
    {
        BitReader reader = new BitReader(buffer);

        int playerId = reader.ReadBits(5);
        int xPos = reader.ReadBits(12);
        int yPos = reader.ReadBits(12);
        int zPos = reader.ReadBits(12);

        Vector3 pos = new Vector3(xPos / 10f, yPos / 10f, zPos / 10f);
        //float rotation = rotY;

        Debug.Log($"ReceivePos : Player {playerId} at {pos}");
        return pos;
    }

    private byte[] ConvertData(Vector3 posData)
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

        Debug.Log($"ConvertData : Player {playerId} at {posData}");
        //Send Data here like opp.send(buffer);

        return buffer;
    }
}
