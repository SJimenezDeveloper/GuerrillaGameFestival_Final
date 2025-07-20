using UnityEngine;



namespace Unity.FPS.Game
{
    public class Damageable : MonoBehaviour
    {
        [Tooltip("Multiplier to apply to the received damage")]
        public float DamageMultiplier = 1f;

        [Range(0, 1)] [Tooltip("Multiplier to apply to self damage")]
        public float SensibilityToSelfdamage = 0.5f;

        public Health Health { get; private set; }
        public Actor Actor { get; private set; }

        public GameObject cameraHolder;

        public Camera Camera { get; private set; }

        
        
       
        void Awake()
        {
            // find the health component either at the same level, or higher in the hierarchy
            Health = GetComponent<Health>();
            if (!Health)
            {
                Health = GetComponentInParent<Health>();
            }
            if (!Actor)
            {
                Actor = GetComponentInParent<Actor>();
            }
            
        }

        public void InflictDamage(float damage, bool isExplosionDamage, GameObject damageSource)
        {
            if (Health)
            {
                var totalDamage = damage;

                // skip the crit multiplier if it's from an explosion
                if (!isExplosionDamage)
                {
                    totalDamage *= DamageMultiplier;
                }

                // potentially reduce damages if inflicted by self
                if (Health.gameObject == damageSource)
                {
                    totalDamage *= SensibilityToSelfdamage;
                }

                // apply the damages
                Health.TakeDamage(totalDamage, damageSource);
            }
            
        }

        public void ChangeBodys(Camera playerCamera)
        {

            if (cameraHolder != null)
            {
                // Desactivar la cámara actual del owner
                Camera currentCamera = playerCamera;
                if (currentCamera != null)
                {
                    currentCamera.enabled = false;
                }
                
                // Mover la cámara del owner a la posición del cameraHolder del jugador impactado
                playerCamera.transform.SetParent(cameraHolder.transform);
                playerCamera.transform.localPosition = Vector3.zero;
                playerCamera.transform.localRotation = Quaternion.identity;
                playerCamera.enabled = true;
                

                
            }

        }
    }
}