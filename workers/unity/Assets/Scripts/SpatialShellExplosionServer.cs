using System.Collections;
using System.Collections.Generic;
using Improbable;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.Commands;
using Improbable.Gdk.Subscriptions;
using Improbable.Worker.CInterop;
using Tankspatial;
using Unity.Entities;
using UnityEngine;

public class SpatialShellExplosionServer : MonoBehaviour
{
    [Require] private WorldCommandSender worldCommandSender;
    [Require] private EntityId selfEntityID;
    [Require] private PositionWriter posWriter;
    [Require] private ShellWriter shellWriter;
    [Require] private TankHealthCommandSender tankHealthCommandSender;

    public LayerMask _TankMask;                        // Used to filter what the explosion affects, this should be set to "Players".
    private float _MaxDamage;                    // The amount of damage done if the explosion is centred on a tank.
    private float _ExplosionForce;              // The amount of force added to a tank at the centre of the explosion.
    private float _MaxLifeTime;                    // The time in seconds before the shell is removed.
    private float _ExplosionRadius;                // The maximum distance away from the explosion tanks can be and are still affected.

    private void Awake()
    {
        _MaxDamage = 100f;
        _ExplosionForce = 1000f;
        _MaxLifeTime = 10f;
        _ExplosionRadius = 5f;
    }
    private void Update()
    {
        timeout();
    }

    private void OnEnable()
    {
        Debug.Log(selfEntityID);
    }

    private void OnDisable()
    {
        Debug.Log("shit happens" + selfEntityID);
     
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (!this.enabled)
        {
            return;
        }

        Collider[] colliders = Physics.OverlapSphere(transform.position, _ExplosionRadius, _TankMask);
        for (int i = 0; i < colliders.Length; i++)
        {
            Rigidbody targetRidigbody = colliders[i].GetComponent<Rigidbody>();

            if (!targetRidigbody)
                continue;

            targetRidigbody.AddExplosionForce(_ExplosionForce, transform.position, _ExplosionRadius);

            Vector3 forceDir = targetRidigbody.position - transform.position;
            forceDir.Normalize();

            Debug.DrawLine(targetRidigbody.position, targetRidigbody.position + forceDir * 10, Color.green, 2);

            float damage = CalculateDamage(targetRidigbody.transform.position);

            var hitTargetEntityID = other.gameObject.GetComponent<LinkedEntityComponent>().EntityId;

            if (hitTargetEntityID == null)
            {
                Debug.LogWarning("Hit Object doest not exisited on Spatial");
                return;
            }

            tankHealthCommandSender.SendDamageCommand(hitTargetEntityID, new TakeDamnageRquest
            {
                Amount = (uint)damage 
            },OnTargetHitReponse);
        }

        shellWriter.SendExplodeEvent(new ShellExplodeRequest());
        GetComponent<Collider>().enabled = false;
        GetComponent<Renderer>().enabled = false;
        GetComponent<Light>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
        timeout();

    }

    private void OnTargetHitReponse(Tankspatial.TankHealth.Damage.ReceivedResponse reponse)
    {
        if (reponse.StatusCode == StatusCode.Success)
        {
            Debug.Log(reponse.EntityId + "get hit,health reduced" + reponse.RequestPayload.Amount);
        }

    }

    private float CalculateDamage(Vector3 targetPosition)
    {
        Vector3 explosionToTarget = targetPosition - transform.position;

        float explosionDistance = explosionToTarget.magnitude;
        float relativeDistance = (_ExplosionRadius - explosionDistance) / _ExplosionRadius;
        float damage = relativeDistance * _MaxDamage;

        damage = Mathf.Max(0f, damage);
        return damage;
    }

    private void timeout()
    {
        _MaxLifeTime -= Time.deltaTime;
        if (_MaxLifeTime < 0)
        {
            DeleteEntity();
        }
    }

    private void DeleteEntity()
    {
        var request = new WorldCommands.DeleteEntity.Request(selfEntityID);
        worldCommandSender.SendDeleteEntityCommand(request, OnDeleteEntityReponse);
        
    }

    private void OnDeleteEntityReponse(WorldCommands.DeleteEntity.ReceivedResponse response)
    {
        if (response.StatusCode == StatusCode.Success)
        {
            Debug.Log("Shell has been deleted ID is +" + response.EntityId.Id);
        }
        else
        {
            Debug.Log("Shell " + response.EntityId.Id + "delete fail check you shit");
        }
    }

}