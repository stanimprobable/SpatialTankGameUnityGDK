using System.Collections;
using System.Collections.Generic;
using Improbable;
using Improbable.Worker;
using Improbable.Gdk.GameObjectCreation;
using Improbable.Transform;
using Improbable.Gdk;
using System.Collections;
using System.Collections.Generic;
using BlankProject;
using Improbable.Gdk.Subscriptions;
using UnityEngine;
using UnityEngine.UI;



namespace Tankspatial
{
    [WorkerType(WorkerUtils.UnityGameLogic)]
    public class SpatialTankHealthServer : MonoBehaviour
    {
        [Require] private TankHealthWriter tankHealthWriter;
        [Require] private TankHealthCommandReceiver tankHealthCommandReceiver;
        private float _StartingHealth;               // The amount of health each tank starts with.
        private float _CurrentHealth;                      // How much health the tank currently has.
        private bool _Dead;

        private void OnEnable()
        {
            tankHealthCommandReceiver.OnDamageRequestReceived += reduceHealth;
        }

        private void reduceHealth(TankHealth.Damage.ReceivedRequest damage)
        {
            tankHealthWriter?.SendUpdate(new TankHealth.Update
            {
                CurrentHealth =- damage.Payload.Amount
            });

        }
    }
}

