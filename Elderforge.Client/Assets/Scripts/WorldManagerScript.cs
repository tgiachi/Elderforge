using System;
using UnityEngine;

namespace Elderforge.Client.Assets.Scripts
{
    public class WorldManagerScript : MonoBehaviour
    {
        [Header("Player")] [SerializeField] public Transform playerTransform;


        private void Update()
        {
            Debug.Log("Player position: " + playerTransform.position);
        }
    }

}
