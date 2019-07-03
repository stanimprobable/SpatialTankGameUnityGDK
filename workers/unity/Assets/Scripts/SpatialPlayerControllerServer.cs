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
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;


    public class SpatialPlayerControllerServer : MonoBehaviour 
    {
        [Require] private PositionWriter posWriter;
        [Require] private TankInputReader tankPositionReader;

    // Start is called before the first frame update
    // Start is called before the first frame update
    //   public float speed;
    //   public Text CountText;
    //   public Text WinText;
        private float Speed = 12f;
        private float TurnSpeed = 180f;
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
            tankPositionReader.OnMovementinputvalueUpdate+=Move;
            tankPositionReader.OnTurninputvalueUpdate+=Turn;
        }

        private void OnDisable()
        {
            rb.isKinematic = true;
        }

        private void Start()
        {
        }

        private void Move(float movementInputValue)
        {
            _movementInputValue = movementInputValue;
            Vector3 movement = transform.forward*_movementInputValue * Speed * Time.deltaTime;
            rb.MovePosition(rb.position + movement);
        }

        private void Turn(float turnInputValue)
        {
            _turnInputValue = turnInputValue;
            float turn = _turnInputValue * TurnSpeed * Time.deltaTime;
            Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
            rb.MoveRotation(rb.rotation*turnRotation);

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

