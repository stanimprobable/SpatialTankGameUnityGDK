using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Improbable;
using Improbable.Worker;
using Improbable.Gdk.GameObjectCreation;
using Improbable.Transform;
using Improbable.Gdk;
using BlankProject;
using Improbable.Common;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.Commands;
using Improbable.Gdk.Subscriptions;
using Improbable.Worker.CInterop;
using Tankspatial;
using Unity.Entities;


    [WorkerType(WorkerUtils.UnityGameLogic)]
    public class SpatialTankShootingServer : MonoBehaviour
    {
        [Require] private PlayerShootCommandReceiver playerShootCommandReceiver;
        [Require] private WorldCommandSender worldCommandSender;

        // Start is called before the first frame update
        private void OnEnable()
        {
            playerShootCommandReceiver.OnShootRequestReceived += createShellandFire;
        }

        void createShellandFire(PlayerShoot.Shoot.ReceivedRequest receivedRequest)
        {
            Debug.Log("FireRequestReceived");
            var payLoad = receivedRequest.Payload;
            var spwanPosition = payLoad.Spwanposition.ToUnityVector();
            var spwanSpeed = payLoad.Launchforce;
            var explode = false;
            var shell = new WorldCommands.CreateEntity.Request(TankEntityTemplate.TankShell(spwanPosition,transform.rotation,spwanSpeed,explode));
            worldCommandSender.SendCreateEntityCommand(shell,onCreateEntiyResponse);
        }

        private void onCreateEntiyResponse(WorldCommands.CreateEntity.ReceivedResponse response)
        {
            if (response.StatusCode == StatusCode.Success)
            {
                Debug.Log("Shell Has been Created ID is" + response.EntityId.Value);
                
            }
            else
            {
                Debug.Log("Something Went Wrong, Check you shit");
            }
        }
    }



