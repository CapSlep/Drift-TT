using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class WheelControl : MonoBehaviour
{
    public enum Axel
    {
        Front,
        Rear
    }
    
    public Transform wheelModel;
    public WheelCollider wheelCollider;
    public Axel axel;

    private CarControl _carController;
    private TrailRenderer _trailRenderer;
    

    // Start is called before the first frame update
    private void Start()
    {
        _carController = GetComponentInParent<CarControl>();
        wheelCollider = GetComponent<WheelCollider>();
        _trailRenderer = wheelModel.GetComponent<TrailRenderer>();
        
    }

    private void LateUpdate()
    {
        WheelAnimation();
    }
    

    public void WheelEffect(bool drifting)
    {
        if (drifting && axel == Axel.Rear && _trailRenderer)
        {
            _trailRenderer.emitting = true;
        }
        else
        {
            _trailRenderer.emitting = false;
        }
    }
    
    private void WheelAnimation()
    {
        // Get the Wheel collider's world pose values and
        // use them to set the wheel model's position and rotation
        wheelCollider.GetWorldPose( out var position, out var rotation);
        var wheelTransform = wheelModel.transform;
        wheelTransform.position = position;
        wheelTransform.rotation = rotation;
    }
}