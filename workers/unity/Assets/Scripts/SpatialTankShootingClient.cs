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

[WorkerType(WorkerUtils.UnityClient)]

public class SpatialTankShootingClient : MonoBehaviour
{
    [Require] private PlayerShootCommandSender playerShootCommandSender;
    [Require] private EntityId selfEntityID;

    public Slider _AimSlider;                  // A child of the tank that displays the current launch force.
    public Transform _FireTransform;           // A child of the tank where the shells are spawned.

    private float _MinLaunchForce = 1f;        // The force given to the shell if the fire button is not held.
    private float _MaxLaunchForce = 10f;        // The force given to the shell if the fire button is held for the max charge time.
    private float CurrentLaunchForce;
    private float _MaxChargeTime = 1.0f;       // How long the shell can charge for before it is fired at max force.
    private float _ChargeSpeed;                // How fast the launch force increases, based on the max charge time.
    private bool _Fired;                       // Whether or not the shell has been launched with this button press.

    // Start is called before the first frame update
    void OnEnable()
    {
        CurrentLaunchForce = _MinLaunchForce;
        _AimSlider.value = _MinLaunchForce;
    }

    // Update is called once per frame
    void Start()
    {
        _ChargeSpeed = (_MaxLaunchForce - _MinLaunchForce) / _MaxChargeTime;
        
    }

    private void Update()
    {
        _AimSlider.value = _MinLaunchForce;

        if (CurrentLaunchForce >= _MaxLaunchForce && !_Fired)
        {
            CurrentLaunchForce = _MaxLaunchForce;
            FiredRequst();
            Debug.Log("Fire!1");
        }
        else if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Fire!2");
            _Fired = false;
            CurrentLaunchForce = _MinLaunchForce;
        }
        else if (Input.GetMouseButton(0) && !_Fired)
        {
            CurrentLaunchForce += Time.deltaTime * _ChargeSpeed;
            _AimSlider.value = CurrentLaunchForce;
        }
        else if (Input.GetMouseButtonUp(0) && !_Fired)
        {
            FiredRequst();
            Debug.Log("Fire!3");
        }              
    }
    
    private void FiredRequst()
    {       
        _Fired = true;
        var spwanPosition = Vector3f.FromUnityVector(_FireTransform.position);
        playerShootCommandSender.SendShootCommand(selfEntityID, new ShootRequest(CurrentLaunchForce,spwanPosition));
        Debug.Log("FireRequestSent");
    }
}
