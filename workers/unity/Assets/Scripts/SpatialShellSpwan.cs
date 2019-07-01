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
using Improbable.Gdk.Subscriptions;
using Tankspatial;

public class SpatialShellSpwan : MonoBehaviour
{
    [Require] ShellWriter shellWriter;
    [Require] TransformInternalWriter tranformWriter;

    // Start is called before the first frame update
    private void OnEnable()
    {
        setupSpeed();
    }
    private void setupSpeed()
    {
        gameObject.GetComponent<Rigidbody>().velocity = new Vector3(
            tranformWriter.Data.Velocity.X,
            tranformWriter.Data.Velocity.Y,
            tranformWriter.Data.Velocity.Z
        ) * shellWriter.Data.Speed;
    }

}
