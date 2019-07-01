using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TankShooting : MonoBehaviour
{
    public Rigidbody _Shell;                   // Prefab of the shell.
    public Transform _FireTransform;           // A child of the tank where the shells are spawned.
    public Slider _AimSlider;                  // A child of the tank that displays the current launch force.
    public float _MinLaunchForce = 15f;        // The force given to the shell if the fire button is not held.
    public float _MaxLaunchForce = 30f;        // The force given to the shell if the fire button is held for the max charge time.
    public float _MaxChargeTime = 1f;       // How long the shell can charge for before it is fired at max force.

    private string _FireButton;                // The input axis that is used for launching shells.
    private float _CurrentLaunchForce;         // The force that will be given to the shell when the fire button is released.
    private float _ChargeSpeed;                // How fast the launch force increases, based on the max charge time.
    private bool _Fired;                       // Whether or not the shell has been launched with this button press.



    void OnEnable()
    {
        _CurrentLaunchForce = _MinLaunchForce;
        _AimSlider.value = _MinLaunchForce;     
    }

    // Update is called once per frame
    void Start()
    {
        _FireButton = "Fire";
        _ChargeSpeed = (_MaxLaunchForce - _MinLaunchForce) /_MaxChargeTime;
    }

    private void Update()
    {
        _AimSlider.value = _MinLaunchForce;

        if (_CurrentLaunchForce >= _MaxLaunchForce && !_Fired)
        {
            _CurrentLaunchForce = _MaxLaunchForce;
            Fired();
        }
        else if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Fire!");
            _Fired = false;
            _CurrentLaunchForce = _MinLaunchForce;

        }
        else if (Input.GetMouseButton(0) && !_Fired)
        {
            _CurrentLaunchForce += Time.deltaTime * _ChargeSpeed;
            _AimSlider.value = _CurrentLaunchForce;
        }
        else if (Input.GetMouseButtonUp(0) && !_Fired)
        {
            Fired();
        }
    }

    private void Fired()
    {
        _Fired = true;
       Rigidbody shellInstance = Instantiate(_Shell, _FireTransform.position, _FireTransform.rotation) as Rigidbody;
       shellInstance.velocity = _CurrentLaunchForce * _FireTransform.forward;
       
        _CurrentLaunchForce = _MinLaunchForce;
    }
}
