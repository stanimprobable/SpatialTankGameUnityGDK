using System.Collections;
using System.Collections.Generic;
using BlankProject;
using Improbable.Gdk.Subscriptions;
using Tankspatial;
using UnityEngine;

[WorkerType(WorkerUtils.UnityClient)]
public class SpatialShellExplosionClient : MonoBehaviour
{
    
    [Require] private ShellReader shellReader;

    public ParticleSystem _ExplosionParticles;         // Reference to the particles that will play on explosion.

    void Start()
    {
        shellReader.OnExplodeEvent+=playExplodeAnimation; ;
    }

    private void playExplodeAnimation(ShellExplodeRequest obj)
    {
        _ExplosionParticles.transform.parent = null;
        _ExplosionParticles.Play();
        Destroy(_ExplosionParticles.gameObject, _ExplosionParticles.main.duration);
        GetComponent<Collider>().enabled = false;
        GetComponent<Renderer>().enabled = false;
        GetComponent<Light>().enabled = false;
    }

    // Update is called once per frame




}
