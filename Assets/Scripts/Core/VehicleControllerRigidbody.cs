using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class VehicleControllerRigidbody : MonoBehaviour, IVehicle
{
    [Header("References")]
    [SerializeField] private InputReader inputReader;
    [SerializeField] private Rigidbody rb;

    [Header("Movement")]
    [SerializeField] private float maxForwardSpeed = 12f;
    [SerializeField] private float maxReverseSpeed = 5f;
    [SerializeField] private float acceleration = 8f;
    [SerializeField] private float deceleration = 5f;
    [SerializeField] private float brakeForce = 15f;

    [Header("Steering")]
    [SerializeField] private float steerSpeed = 90f;
    [SerializeField] private float minSteerSpeedAtMaxSpeed = 30f;
    [SerializeField] private float minSpeedToSteer = 0.5f;

    [Header("Suspension")]
    [SerializeField] private LayerMask groundLayer = ~0;
    [SerializeField] private float suspensionLength = 0.5f;
    [SerializeField] private float springStrength = 50f;
    [SerializeField] private float springDamping = 5f;
    [SerializeField] private Vector3 wheelFL = new Vector3(-0.8f, 0f,  1.5f);
    [SerializeField] private Vector3 wheelFR = new Vector3( 0.8f, 0f,  1.5f);
    [SerializeField] private Vector3 wheelRL = new Vector3(-0.8f, 0f, -1.5f);
    [SerializeField] private Vector3 wheelRR = new Vector3( 0.8f, 0f, -1.5f);

    [Header("Downforce")]
    [SerializeField] private float downforce = 5f;

    [Header("Traction")]
    [SerializeField] private float tractionStrength = 10f;
    [SerializeField] private float driftTraction = 1f;

    [Header("Visuals")]
    [SerializeField] private Transform visualRoot;
    [SerializeField] private float visualSlipMultiplier = 0.4f;
    [SerializeField] private float visualRotationSpeed = 8f;

    [Header("Features")]
    [SerializeField] private bool useSuspension = true;
    [SerializeField] private bool useAcceleration = true;
    [SerializeField] private bool useReverseGear = true;
    [SerializeField] private bool useSpeedDependentSteering = true;
    [SerializeField] private bool useTraction = true;
    [SerializeField] private bool useDownforce = true;

    public float CurrentSpeed { get; private set; }
    public float NormalizedSpeed { get; private set; }
    public bool IsGrounded { get; private set; }
    public bool IsBoosting => false;
    public bool IsDrifting { get; private set; }

    private Vector2 moveInput;
    private Vector3[] wheelOffsets;

    private void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();
        wheelOffsets = new Vector3[] { wheelFL, wheelFR, wheelRL, wheelRR };
        inputReader.EnableGameplayInput();
    }

    private void OnEnable()
    {
        inputReader.MoveEvent += OnMove;
        inputReader.SprintEvent += OnDrift;
    }

    private void OnDisable()
    {
        inputReader.MoveEvent -= OnMove;
        inputReader.SprintEvent -= OnDrift;
    }

    private void FixedUpdate()
    {
        float dt = Time.fixedDeltaTime;

        if (useSuspension)
            HandleSuspension();
        else
            IsGrounded = Physics.Raycast(transform.position, -transform.up, 1f, groundLayer);

        HandleDriving(dt);
        HandleSteering(dt);

        if (useTraction) HandleTraction(dt);
        if (useDownforce) HandleDownforce();
    }

    private void HandleSuspension()
    {
        int groundedCount = 0;

        foreach (Vector3 localOffset in wheelOffsets)
        {
            Vector3 wheelWorld = transform.TransformPoint(localOffset);

            if (Physics.Raycast(wheelWorld, -transform.up, out RaycastHit hit,
                suspensionLength, groundLayer, QueryTriggerInteraction.Ignore))
            {
                groundedCount++;

                float compression = suspensionLength - hit.distance;
                float springForce = compression * springStrength;
                float dampVel     = Vector3.Dot(rb.GetPointVelocity(wheelWorld), transform.up);
                float dampForce   = dampVel * springDamping;

                rb.AddForceAtPosition(transform.up * Mathf.Max(0f, springForce - dampForce), wheelWorld);
            }
        }

        IsGrounded = groundedCount > 0;
    }

    private void HandleDriving(float dt)
    {
        float throttle = moveInput.y;

        float targetSpeed;
        if (throttle > 0f)
            targetSpeed = throttle * maxForwardSpeed;
        else if (throttle < 0f)
            targetSpeed = useReverseGear ? throttle * maxReverseSpeed : 0f;
        else
            targetSpeed = 0f;

        CurrentSpeed = Vector3.Dot(rb.linearVelocity, transform.forward);

        if (useAcceleration)
        {
            bool isBraking = (throttle < 0f && CurrentSpeed > 0f) || (throttle > 0f && CurrentSpeed < 0f);
            float rate = isBraking ? brakeForce : Mathf.Abs(throttle) < 0.01f ? deceleration : acceleration;
            CurrentSpeed = Mathf.MoveTowards(CurrentSpeed, targetSpeed, rate * dt);
        }
        else
        {
            CurrentSpeed = targetSpeed;
        }

        NormalizedSpeed = Mathf.Clamp01(Mathf.InverseLerp(0f, maxForwardSpeed, Mathf.Abs(CurrentSpeed)));

        rb.linearVelocity = rb.linearVelocity
            - transform.forward * Vector3.Dot(rb.linearVelocity, transform.forward)
            + transform.forward * CurrentSpeed;
    }

    private void HandleSteering(float dt)
    {
        if (Mathf.Abs(CurrentSpeed) < minSpeedToSteer) return;

        float effectiveSteer = useSpeedDependentSteering
            ? Mathf.Lerp(steerSpeed, minSteerSpeedAtMaxSpeed, Mathf.Abs(CurrentSpeed) / maxForwardSpeed)
            : steerSpeed;

        float steerDir = (useReverseGear && CurrentSpeed < 0f) ? -1f : 1f;
        rb.MoveRotation(rb.rotation * Quaternion.Euler(0f, moveInput.x * effectiveSteer * steerDir * dt, 0f));
    }

    private void HandleTraction(float dt)
    {
        float traction = IsDrifting ? driftTraction : tractionStrength;
        Vector3 localVel = transform.InverseTransformDirection(rb.linearVelocity);
        localVel.x = Mathf.Lerp(localVel.x, 0f, traction * dt);
        rb.linearVelocity = transform.TransformDirection(localVel);
    }

    private void HandleDownforce()
    {
        rb.AddForce(-transform.up * downforce * Mathf.Abs(CurrentSpeed));
    }

    private void Update()
    {
        if (visualRoot == null) return;

        Vector3 localVel = transform.InverseTransformDirection(rb.linearVelocity);
        float slipAngle = -Mathf.Atan2(localVel.x, Mathf.Abs(localVel.z)) * Mathf.Rad2Deg;
        Quaternion targetRot = Quaternion.Euler(0f, slipAngle * visualSlipMultiplier, 0f);
        visualRoot.localRotation = Quaternion.Lerp(visualRoot.localRotation, targetRot, Time.deltaTime * visualRotationSpeed);
    }

    private void OnMove(Vector2 input) => moveInput = input;
    private void OnDrift(bool isPressed) => IsDrifting = isPressed;

    private void OnDrawGizmos()
    {
        Vector3[] wheels = { wheelFL, wheelFR, wheelRL, wheelRR };
        Color[] colors = { Color.red, Color.cyan, Color.yellow, Color.green };
        for (int i = 0; i < wheels.Length; i++)
        {
            Vector3 worldPos = transform.TransformPoint(wheels[i]);
            Gizmos.color = colors[i];
            Gizmos.DrawWireSphere(worldPos, 0.08f);
            Gizmos.DrawRay(worldPos, -transform.up * suspensionLength);
        }

        if (!Application.isPlaying || rb == null) return;

        Vector3 origin = transform.position + transform.up * 0.5f;

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(origin, transform.forward * 3f);

        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(origin, rb.linearVelocity.normalized * 3f);
    }
}
