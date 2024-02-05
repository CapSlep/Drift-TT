using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class CarControl : MonoBehaviour
{
    [Header("Car Settings")] 
    [Tooltip("Car motor torque")]
    public float maxAcceleration = 1000.0f;
    [Tooltip("Car breaking torque")]
    public float brakeAcceleration = 1500.0f;

    [Tooltip("Sensitivity of steering wheels")]
    public float turnSensitivity = 1.0f;
    [Tooltip("Maximal steering angle of steering wheels")]
    public float maxSteerAngle = 30.0f;
    
    [Header("Drift Settings")]
    [Range(1f,10f)] [Tooltip("Threshold to count drifting. [1 - 10]\n" +
                             "1 - Drifting at low lateral velocity\n" +
                             "10 - Drifting at high lateral velocity")]
    public float driftThreshold = 5.0f;


    [Header("Car Audio")] 
    public float minAudioSpeed = 0.3f;
    public float maxAudioSpeed = 50f;
    public float minPitch = 0.2f;
    public float maxPitch = 0f;
    
    [Header("Wheels")]
    public List<WheelControl> wheels;
    
    public int DriftScore { get; private set; }

    private bool _isBreaking;

    private float _moveInput;
    private float _steerInput;
    private float _currentSpeed;
    private float _pitchFromCar;

    private Vector3 _centerOfMass;

    private AudioSource _carAudioSource;
    private Rigidbody _carRb;
    public GeneralControls _generalControls;

    private void Awake()
    {
        _generalControls = new GeneralControls();
        _carRb = GetComponent<Rigidbody>();
        _carAudioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        _carRb.centerOfMass = _centerOfMass;
    }
    private void OnEnable()
    {
        _generalControls.Gameplay.Enable();
    }

    private void OnDisable()
    {
        _generalControls.Gameplay.Disable();
    }

    private void Update()
    {
        GetInputs();
        WheelDrifting();
        EngineSound();
    }

    private void FixedUpdate()
    {
        Move();
        Steer();
        Brake();
    }

    private void GetInputs()
    {
        _moveInput = _generalControls.Gameplay.VerticalMove.ReadValue<float>();
        _steerInput = _generalControls.Gameplay.HorizontalMove.ReadValue<float>();
        _isBreaking = _generalControls.Gameplay.Break.inProgress;
    }

    private void Move()
    {
        foreach (var wheel in wheels)
        {
            wheel.wheelCollider.motorTorque = _moveInput * maxAcceleration;
        }
    }

    private void Steer()
    {
        foreach (var wheel in wheels)
        {
            if (wheel.axel != WheelControl.Axel.Front) continue;
            var steerAngle = _steerInput * turnSensitivity * maxSteerAngle;
            wheel.wheelCollider.steerAngle = Mathf.Lerp(wheel.wheelCollider.steerAngle, steerAngle, 0.6f);
        }
    }

    private void Brake()
    {
        if (_moveInput == 0)
        {
            foreach (var wheel in wheels)
            {
                wheel.wheelCollider.brakeTorque = brakeAcceleration;
            }
        }
        else if(_isBreaking)
        {
            foreach (var wheel in wheels.Where(wheel => wheel.axel == WheelControl.Axel.Rear))
            {
                wheel.wheelCollider.brakeTorque = brakeAcceleration;
            }
        }
        else
        {
            foreach (var wheel in wheels)
            {
                wheel.wheelCollider.brakeTorque = 0;
            }
        }
    }

    private void WheelDrifting()
    {
            foreach (var wheel in wheels)
            {
                wheel.WheelEffect(IsDrifting());
            }
    }
    
    private bool IsDrifting()
    {
        // Calculate the lateral velocity of the wheel
        Vector3 lateralVelocity = transform.InverseTransformDirection(_carRb.velocity);
        float lateralSpeed = lateralVelocity.x;

        // Check if the lateral speed is greater than the threshold
        var isDrifting = Mathf.Abs(lateralSpeed) > driftThreshold;
        if (!isDrifting) return false;
        DriftScore++;
        return true;
    }


    private void EngineSound()
    {
        _currentSpeed = _carRb.velocity.magnitude;
        _pitchFromCar = _carRb.velocity.magnitude / 50f;

        if (_currentSpeed < minAudioSpeed)
        {
            _carAudioSource.pitch = minPitch;
        }

        if (_currentSpeed > minAudioSpeed && _currentSpeed < maxAudioSpeed)
        {
            _carAudioSource.pitch = minPitch + _pitchFromCar;
        }

        if (_currentSpeed > maxAudioSpeed)
        {
            _carAudioSource.pitch = maxPitch;
        }
    }

    public void ShopExposed()
    {
        _carAudioSource.mute = true;
        _generalControls.Gameplay.Disable();
    }
}