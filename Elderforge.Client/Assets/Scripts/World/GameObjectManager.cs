using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Extensions;
using Elderforge.Network.Packets.GameObjects;
using Elderforge.Network.Packets.Player;
using UnityEngine;

namespace Assets.Scripts.World
{
    public class GameObjectManager : MonoBehaviour
    {

        [SerializeField] private GameObject networkGameObjectPrefab;

        [SerializeField] private GameObject playerGameObjectPrefab;


        private readonly Dictionary<string, GameObject> networkGameObjects = new();


        public void OnGameObjectCreated(GameObjectCreateMessage message)
        {


            Debug.Log("Creating gameObject");

            if (networkGameObjects.ContainsKey(message.Id))
            {
                Debug.LogWarning($"GameObject with id {message.Id} already exists");
                return;
            }

            var networkGameObject = Instantiate(
                networkGameObjectPrefab,
                message.Position.ToUnityVector3(),
                Quaternion.Euler(message.Rotation.ToUnityVector3()),
                transform
            );
            Debug.Log("Network gameobject created");
            networkGameObjects[message.Id] = networkGameObject;


            networkGameObject.transform.localScale = message.Scale.ToUnityVector3();

        }

        public void OnGameObjectDestroyed(GameObjectDestroyMessage message)
        {

            if (!networkGameObjects.TryGetValue(message.Id, out var o))
            {
                Debug.LogWarning($"GameObject with id {message.Id} does not exist");
                return;
            }

            Debug.Log("Network gameobject destroyed");

            Destroy(o);
            networkGameObjects.Remove(message.Id);
        }

        public void OnGameObjectMoved(GameObjectMoveMessage message)
        {

            if (!networkGameObjects.TryGetValue(message.Id, out var o))
            {
                Debug.LogWarning($"GameObject with id {message.Id} does not exist");
                return;
            }



            o.transform.position = message.Position.ToUnityVector3();
            o.transform.rotation = Quaternion.Euler(message.Rotation.ToUnityVector3());

        }

        public void OnPlayerGameObjectCreated(PlayerMoveResponseMessage message)
        {
            Debug.Log("Creating player gameObject");

            if (networkGameObjects.TryGetValue(message.Id, out var exPl))
            {
                Debug.LogWarning($"GameObject with id {message.Id} already exists, updating");

                exPl.transform.position = message.Position.ToUnityVector3();
                exPl.transform.rotation = Quaternion.Euler(message.Rotation.ToUnityVector3());
                return;
            }

            var playerGameObject = Instantiate(
                playerGameObjectPrefab,
                message.Position.ToUnityVector3(),
                Quaternion.Euler(message.Rotation.ToUnityVector3()),
                transform
            );
            Debug.Log("Player gameobject created");
            networkGameObjects[message.Id] = playerGameObject;
        }
    }
}
