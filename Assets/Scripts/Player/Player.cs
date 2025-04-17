using Listeners;
using Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(PlayerMovement))]
    public class Player : MonoBehaviour
    {
        [SerializeField] private bool isOpponent;

        private PlayerMovement _movements;

        private void Awake()
        {
            _movements = GetComponent<PlayerMovement>();
            _movements.IsOpponent = isOpponent;
            if (isOpponent)
            {
                EventManager<PlayerMovementData>.StartListening(Props.GameEvents.PLAYER_MOVE, OnRecievedPosition);
            }
        }

        private void OnEnable()
        {
            _movements.OnMovement += OnMovement;
        }

        private void OnDisable()
        {
            _movements.OnMovement -= OnMovement;
        }

        private void OnRecievedPosition(PlayerMovementData data)
        {
            Vector3 newPos = NetworkManager.Instance.DataHandler.ReceivePos(data.Position);
            _movements.MovePlayer(newPos);
        }

        private void OnMovement(Vector3 newPos)
        {
            EventManager<PlayerMovementData>.TriggerEvent(Props.GameEvents.PLAYER_MOVE, new PlayerMovementData()
            {
                Position = NetworkManager.Instance.DataHandler.ConvertData(newPos)
            });
        }


    }
}
