using System;
using System.Collections;
using System.Collections.Generic;
using Assets.script;
using UnityEngine;


[Serializable]
public class TankManager
{
    public Color PlayerColor;                             // This is the color this tank will be tinted.
    public Transform SpawnPoint;                          // The position and direction the tank will have when it spawns.
    [HideInInspector] public string ColoredPlayerText;    // A string that represents the player with their number colored to match their tank.
    [HideInInspector] public GameObject _Instance;         // A reference to the instance of the tank when it is created.
    [HideInInspector] public int _Wins;                    // The number of wins this player has so far.


    private SpatialPlayerController _Movement;                        // Reference to tank's movement script, used to disable and enable control.
    private TankShooting _Shooting;                        // Reference to tank's shooting script, used to disable and enable control.
    private GameObject _CanvasGameObject;                  // Used to disable the world space UI during the Starting and Ending phases of each round.

    // Start is called before the first frame update
    public void Setup()
    {
        
        _Movement = _Instance.GetComponent<SpatialPlayerController>();
        _Shooting = _Instance.GetComponent<TankShooting>();
        _CanvasGameObject = _Instance.GetComponentInChildren<Canvas>().gameObject;

        ColoredPlayerText = "<color=#" + ColorUtility.ToHtmlStringRGB(PlayerColor) + ">PLAYER " + "</color>";
        MeshRenderer[] renderers = _Instance.GetComponentsInChildren<MeshRenderer>();

        for (int i = 0; i < renderers.Length; i++)
        {
            // ... set their material color to the color specific to this tank.
            renderers[i].material.color = PlayerColor;
        }

    }

    // Update is called once per frame
    public void DisableControl()
    {
        _Movement.enabled = false;
        _Shooting.enabled = false;
        _CanvasGameObject.SetActive(false);
    }
    public void EnableControl()
    {
        _Movement.enabled = true;
        _Shooting.enabled = true;
        _CanvasGameObject.SetActive(true);
    }
    public void Reset()
    {
        _Instance.transform.position = SpawnPoint.position;
        _Instance.transform.rotation = SpawnPoint.rotation;

        _Instance.SetActive(false);
        _Instance.SetActive(true);
    }

}
