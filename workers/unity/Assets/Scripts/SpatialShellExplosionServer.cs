using System.Collections;
using System.Collections.Generic;
using Improbable.Gdk.Core;
using Improbable.Gdk.Subscriptions;
using UnityEngine;

public class ShellExplosionServer : MonoBehaviour
{
    [Require] private WorldCommandSender worldCommandSender;
    public LayerMask _TankMask;                        // Used to filter what the explosion affects, this should be set to "Players".
    public ParticleSystem _ExplosionParticles;         // Reference to the particles that will play on explosion.
    private float _MaxDamage;                    // The amount of damage done if the explosion is centred on a tank.
    private float _ExplosionForce;              // The amount of force added to a tank at the centre of the explosion.
    private float _MaxLifeTime;                    // The time in seconds before the shell is removed.
    private float _ExplosionRadius;                // The maximum distance away from the explosion tanks can be and are still affected.

    private void Awake()
    {
        _MaxDamage = 100f;
        _ExplosionForce = 1000f;
        _MaxLifeTime = 2f;
        _ExplosionRadius = 5f;
    }
    void Start()
    {
        Destroy(gameObject, _MaxLifeTime);


    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
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

            TankHealth targetHealth = targetRidigbody.GetComponent<TankHealth>();

            if (!targetHealth)
                continue;
            float damage = CalculateDamage(targetRidigbody.transform.position);

            targetHealth.TakeDamage(damage);
        }
        _ExplosionParticles.transform.parent = null;
        var explosion = Instantiate(_ExplosionParticles);
        explosion.Play();
        Destroy(explosion.gameObject, explosion.main.duration);
        Destroy(gameObject);
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

}