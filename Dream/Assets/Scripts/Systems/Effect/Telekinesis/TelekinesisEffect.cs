using Effects;
using UnityEngine;

public class TelekinesisEffect : Effect
{
    private TelekinesisEffectData telekinesisEffectData;
    private Entity sourceEntity;
    private Vision sourceVision;
    private Rigidbody targetRigidbody;

    private Vector3 targetPosition;
    private float currentDistance;

    public override void Initialize(EffectData effectData)
    {
        base.Initialize(effectData);
        telekinesisEffectData = (TelekinesisEffectData)effectData;
    }

    private void FixedUpdate()
    {
        if (!IsActive || targetRigidbody == null) return;

        UpdateTargetPosition();
        ApplySmartMovementForces();
    }

    private void UpdateTargetPosition()
    {
        targetPosition = sourceEntity.transform.position +
                       sourceVision.GetLookDirection() * telekinesisEffectData.Distance;
        currentDistance = Vector3.Distance(targetRigidbody.position, targetPosition);
    }

    private void ApplySmartMovementForces()
    {
        // Calculate base direction force
        Vector3 toTarget = (targetPosition - targetRigidbody.position).normalized;
        float distanceFactor = Mathf.Clamp01(currentDistance / telekinesisEffectData.SlowdownDistance);
        Vector3 targetForce = distanceFactor * telekinesisEffectData.Strength * toTarget;

        // Calculate velocity damping
        Vector3 velocityDamping = -targetRigidbody.linearVelocity * telekinesisEffectData.DampingStrength;

        // Combine forces
        Vector3 totalForce = targetForce + velocityDamping;

        // Apply force while maintaining physics interactions
        targetRigidbody.AddForce(totalForce, ForceMode.Force);

        // Add upward stabilization
        if (telekinesisEffectData.StabilizeHeight)
        {
            float heightDifference = targetPosition.y - targetRigidbody.position.y;
            Vector3 liftForce = Vector3.up * (heightDifference * telekinesisEffectData.LiftFactor);
            targetRigidbody.AddForce(liftForce, ForceMode.Acceleration);
        }
    }

    protected override void Activate(Context effectContext)
    {
        if (IsActive) return;
        base.Activate(effectContext);
        sourceEntity = effectContext.SourceEntity;

        if (sourceEntity.TryGetComponent(out sourceVision))
        {
            // Get target from context
            if (effectContext.TargetEntity != null)
            {
                targetRigidbody = effectContext.TargetEntity.GetComponent<Rigidbody>();
            }
        }
    }

    protected override void Deactivate(Context effectContext)
    {
        if (!IsActive) return;
        base.Deactivate(effectContext);
        targetRigidbody = null;
        sourceEntity = null;
        sourceVision = null;
    }

    public override void UpdateAnimationProgress(float progress)
    {
        // Not needed for basic implementation
    }
}