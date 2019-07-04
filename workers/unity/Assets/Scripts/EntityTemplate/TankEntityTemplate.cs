using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Improbable;
using Improbable.Worker;
using Improbable.Gdk.GameObjectCreation;
using Improbable.Gdk;
using System.Collections;
using System.Collections.Generic;
using BlankProject;
using Improbable.Gdk.Core;
using Improbable.Gdk.PlayerLifecycle;
using Improbable.Gdk.Subscriptions;
using Improbable.Gdk.TransformSynchronization;
using Quaternion = UnityEngine.Quaternion;

public class TankEntityTemplate 
{

    public static EntityTemplate TankShell(Vector3 position, Quaternion rotation ,Vector3f launchspeed)
    {
        // Create a HealthPickup component snapshot which is initially active and grants "heathValue" on pickup.

        var entityTemplate = new EntityTemplate();
        var speed = new Tankspatial.Shell.Snapshot(launchspeed);
        var serverAttribute = WorkerUtils.UnityGameLogic;
        entityTemplate.AddComponent(new Position.Snapshot(new Coordinates(position.x, position.y,position.z)), serverAttribute);//Add position Component to Template 
        entityTemplate.AddComponent(new Metadata.Snapshot("Shell"), serverAttribute);
        entityTemplate.AddComponent(speed, serverAttribute);
        entityTemplate.SetReadAccess(WorkerUtils.UnityGameLogic, WorkerUtils.UnityClient);
        entityTemplate.SetComponentWriteAccess(EntityAcl.ComponentId, serverAttribute);
        TransformSynchronizationHelper.AddTransformSynchronizationComponents(
            entityTemplate,
            serverAttribute,
            location: position,
            rotation: rotation       
            );

        return entityTemplate;
    }

    public static EntityTemplate CreatePlayerEntityTemplate(string workerId, byte[] serializedArguments)
    {
        var template = new EntityTemplate();
        var clientAttribute = EntityTemplate.GetWorkerAccessAttribute(workerId);
        var serverAttribute = WorkerUtils.UnityGameLogic;
        var tankHealth = new Tankspatial.TankHealth.Snapshot(100, 100);
        var playerShootFeature = new Tankspatial.PlayerShoot.Snapshot();
        var TankInput = new Tankspatial.TankInput.Snapshot(0, 0);
        template.AddComponent(new Position.Snapshot(), serverAttribute);
        template.AddComponent(new Metadata.Snapshot("Tank"), serverAttribute);
        template.AddComponent(TankInput, clientAttribute);
        template.AddComponent(tankHealth, serverAttribute);//Add Health Component to Tank 
        template.AddComponent(playerShootFeature, serverAttribute);
        TransformSynchronizationHelper.AddTransformSynchronizationComponents(
            template,
            serverAttribute
        );
        PlayerLifecycleHelper.AddPlayerLifecycleComponents(template, workerId, serverAttribute);
        template.SetReadAccess(UnityClientConnector.WorkerType, MobileClientWorkerConnector.WorkerType, serverAttribute);
        template.SetComponentWriteAccess(EntityAcl.ComponentId, serverAttribute);

        return template;
    }

}
