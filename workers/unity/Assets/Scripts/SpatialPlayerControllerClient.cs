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
using Tankspatial;
using Quaternion = UnityEngine.Quaternion;


public class SpatialPlayerControllerClient : MonoBehaviour
{
    [Require] private TankInputWriter tankInputWriter;

    

    // Start is called before the first frame update
    //   public float speed;
    //   public Text CountText;
    //   public Text WinText;
    private bool _fireCannonBall;
    private Rigidbody rb;

    private float _movementInputValue;         // The current value of the movement input.
    private float _turnInputValue;             // The current value of the turn input.
    private float _originalPitch;              // The pitch of the audio source at the start of the scene.


    private void Awake()
    {
        //textDisplay();
        //WinText.text ="";
        rb = GetComponent<Rigidbody>();
    }
    private void OnEnable()
    {
        rb.isKinematic = false;
        _movementInputValue = 0f;
        _turnInputValue = 0f;
    }

    private void OnDisable()
    {
        rb.isKinematic = true;
    }

    private void Start()
    {
    }
    void Update()
    {
        _movementInputValue = Input.GetAxis("Vertical");
        _turnInputValue = Input.GetAxis("Horizontal");
        tankInputWriter?.SendUpdate(new TankInput.Update
        {
            Turninputvalue = _turnInputValue,
            Movementinputvalue = _movementInputValue
        });
    }
  


    //private void textDisplay()
    //{
    //    CountText.text = "Count: " + count.ToString();
    //    if (count >= 12)
    //    {
    //        WinText.text = "WIN!!!!";
    //    }
    //}



}

