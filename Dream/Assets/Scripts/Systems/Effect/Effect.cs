using System.Collections.Generic;
using UnityEngine;

namespace Effects
{
    public enum UseType
    {
        None,
        Create,
        Control,
        Transform,
        Negate,
        Destroy,
        Percieve,
        Apply,
        Summon
    }

    public enum SourceType
    {
        None,
        Magic,
        Ability,
        Item,
        Environment,
    }

    public enum ActivationType
    {
        None,
        OnUse,
        OnCancel,
        OnReady,
        OnStopUse,
        OnActivate,
        OnDeactivate,
    }

    public struct ConditionResult
    {
        public bool Result;
        public EffectConditionData ConditionData;
    }

    public struct Context
    {
        public SourceType SourceType;
        public Entity SourceEntity;
        public ScriptableObject SourceData;

        public Entity TargetEntity;
        public Vector3 TargetPosition;
        public RaycastHit? RaycastHit;

        public Dictionary<string, object> Parameters;

        public static Context ConstructContext(SourceType sourceType, UseContext useContext, ScriptableObject sourceData, Dictionary<string, object> parameters)
        {
            Entity sourceEntity = useContext.SourceEntity;
            Context context = new Context
            {
                SourceType = sourceType,
                SourceEntity = sourceEntity,
                SourceData = sourceData,

                TargetEntity = null,
                TargetPosition = sourceEntity.transform.position + sourceEntity.transform.forward * 10f,
                RaycastHit = null,

                Parameters = parameters
            };


            if (sourceEntity.TryGetComponent(out Vision vision))
            {
                bool hasTarget = false;
                RaycastHit[] raycastHits = vision.QueryVision();
                foreach (RaycastHit hit in raycastHits)
                {
                    if (hit.collider.TryGetComponent(out Entity entity) && entity != sourceEntity)
                    {
                        context.TargetEntity = entity;
                        context.TargetPosition = hit.point;
                        context.RaycastHit = hit;
                        hasTarget = true;
                        break;
                    }
                }

                if (!hasTarget && raycastHits.Length > 0)
                {
                    context.TargetPosition = raycastHits[0].point;
                    context.RaycastHit = raycastHits[0];
                }
            }

            return context;
        }
    }

    public abstract class Effect : MonoBehaviour
    {
        public delegate void EffectHandler(Effect effect, bool isActive);
        public event EffectHandler OnEffect;

        [SerializeField] private EffectData effectData;
        private bool isActive;

        public EffectData EffectData => effectData;
        public bool IsActive => isActive;

        public virtual void Initialize(EffectData effectData)
        {
            this.effectData = effectData;
            isActive = false;
        }

        public abstract void UpdateAnimationProgress(float progress);

        public void HandleActivationEvent(Context effectContext, ActivationType activationType)
        {
            if (activationType == effectData.EffectActivationType || activationType == ActivationType.OnActivate)
            {
                Activate(effectContext);
            }
            else if (activationType == effectData.EffectDeactivationType || activationType == ActivationType.OnDeactivate)
            {
                Deactivate(effectContext);
            }
        }

        public virtual void HandleEntityEvent()
        {

        }

        protected virtual void Activate(Context effectContext)
        {
            if (isActive) return;
            isActive = true;
            OnEffect?.Invoke(this, isActive);
        }

        protected virtual void Deactivate(Context effectContext)
        {
            if (!isActive) return;
            isActive = false;
            OnEffect?.Invoke(this, isActive);
        }
    }
}