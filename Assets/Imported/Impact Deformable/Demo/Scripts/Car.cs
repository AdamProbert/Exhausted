using UnityEngine;
using System.Collections;

// Car for the CarDerby demo scene
public class Car : MonoBehaviour 
{
    // Car color
    public Color Color;

    // Tires
    public Transform RTire;
    public Transform LTire;

    // Current control (steering on x, accel on y)
    public Vector2 Control;

    // Car sounds
    public AudioSource EngineSound;
    public AudioSource DriftSound;

    // Damage smoke
    public ParticleSystem Smoke;

    // Impact Deformable on car hull
    public ImpactDeformable CarHullDeformable;

    float RPM;
    Rigidbody body;
    bool AI;
    ImpactDeformable impactDeformable;
    bool Broken;

    void Awake()
    {
        body = GetComponent<Rigidbody>();
        StartCoroutine(StartVolume());
    }

    void Start()
    {
        AI = GetComponent<AIControl>() != null;
        transform.Find("CarHull").GetComponent<MeshRenderer>().material.color = Color;
    }

    // Slowly increase volume on game start
    IEnumerator StartVolume()
    {
        while (EngineSound.volume < 1)
        {
            EngineSound.volume += Time.deltaTime * 0.5f;
            yield return null;
        }
    }

    public void FixedUpdate()
    {
        float vl = body.velocity.magnitude;
        float am = body.angularVelocity.magnitude;

        // Will not move if broken
        if (Broken)
            Control = Vector3.zero;

        // Apply car control to engine and direction
        if (transform.position.y < 0.1f)
        {
            body.AddForce(transform.forward * Control.y * 20);
            if (am < 2)
            {
                float x = Control.x;
                if (Control.y < 0)
                    x *= -1;
                body.AddTorque(transform.up * x * 20);
            }
        }

        // Keep car on right rotation
        if ((am < 0.1f) && (vl < 0.1f))
        {
            Vector3 e = transform.eulerAngles;
            if (e.x > 180)
                e.x -= 360;
            if (e.z > 180)
                e.z -= 360;
            e.x = Mathf.Abs(e.x);
            e.z = Mathf.Abs(e.z);
            if ((e.x > 1) || (e.z > 1))
                Invoke("UnturnCar", 2);
        }

        // Apply direction to tires
        Vector3 we = new Vector3(0, Control.x * 45, 0);
        LTire.transform.localEulerAngles = we;
        RTire.transform.localEulerAngles = we;
    }

    // Unturn the car
    void UnturnCar()
    {
        Vector3 e = transform.eulerAngles;
        e.x = 0;
        e.z = 0;
        transform.eulerAngles = e;
    }

    void Update()
    {
        ProcessEngineSound();

        if (!AI)
            ProcessDriftSound();

        CheckDamage();
    }

    // Engine sound with acceleration
    void ProcessEngineSound()
    {
        float v = body.velocity.magnitude / 20;
        if (Control.y != 0)
        {
            v += 0.25f;
            v *= 2;
        }
        v += 0.2f;
        if (Broken)
            v = 0;
        RPM = Mathf.Lerp(RPM, v, Time.deltaTime * 3);
        EngineSound.pitch = RPM;
    }

    // Drift sound with X rotation movements
    void ProcessDriftSound()
    {
        float v = Mathf.Abs(body.angularVelocity.y);
        if ((v >= 1) && (!DriftSound.isPlaying))
            DriftSound.Play();
        if ((v < 1) && (DriftSound.isPlaying))
            DriftSound.Stop();
        if (DriftSound.isPlaying)
            DriftSound.volume = v * 0.5f;
    }

    // Estimate car structure damage in 0..1 range
    public float CarDamage
    {    
        get
        {
            return Mathf.Clamp01(CarHullDeformable.StructuralDamage / 0.065f);
        }
    }

    // Check if there is enough damage to stop the car
    void CheckDamage()
    {
        Broken = CarDamage >= 1;
        Smoke.enableEmission = Broken;
    }
}
