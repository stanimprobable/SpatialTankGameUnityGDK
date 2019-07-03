using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Improbable;
using Improbable.Worker;
using Improbable.Gdk.GameObjectCreation;
using Improbable.Gdk;
using BlankProject;
using Improbable.Gdk.Core;
using Improbable.Gdk.Subscriptions;
using Improbable.Gdk.TransformSynchronization;
using Tankspatial;

public class SpatialShellSpwan : MonoBehaviour
{
    [Require] ShellWriter shellWriter;

    // Start is called before the first frame update
    private void OnEnable()
    {
        setupSpeed();
    }
    private void Update()
    {
        if (transform.position.y == 0)
        {
            Debug.Log("position reset");
        }
    }
    private void setupSpeed()
    {
        gameObject.GetComponent<Rigidbody>().velocity = shellWriter.Data.Speed.ToUnityVector();

    }

}
