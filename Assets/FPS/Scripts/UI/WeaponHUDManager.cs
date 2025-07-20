using System.Collections.Generic;
using Unity.FPS.Game;
using Unity.FPS.Gameplay;
using UnityEngine;

namespace Unity.FPS.UI
{
    public class WeaponHUDManager : MonoBehaviour
    {
        [Tooltip("UI panel containing the layoutGroup for displaying weapon ammo")]
        public RectTransform AmmoPanel;

        [Tooltip("Prefab for displaying weapon ammo")]
        public GameObject AmmoCounterPrefab;

        [Tooltip("PlayerWeaponManager actually managing the player")]
        public PlayerWeaponsManager current_WeaponManager;
        List<AmmoCounter> m_AmmoCounters = new List<AmmoCounter>();

        void Start()
        {
            if (current_WeaponManager == null)
            {
                current_WeaponManager = FindObjectOfType<PlayerWeaponsManager>();
                DebugUtility.HandleErrorIfNullFindObject<PlayerWeaponsManager, WeaponHUDManager>(current_WeaponManager,
                    this);
            }
            current_WeaponManager = FindObjectOfType<PlayerWeaponsManager>();
            DebugUtility.HandleErrorIfNullFindObject<PlayerWeaponsManager, WeaponHUDManager>(current_WeaponManager,
                this);

            WeaponController activeWeapon = current_WeaponManager.GetActiveWeapon();
            if (activeWeapon)
            {
                AddWeapon(activeWeapon, current_WeaponManager.ActiveWeaponIndex);
                ChangeWeapon(activeWeapon);
            }

            current_WeaponManager.OnAddedWeapon += AddWeapon;
            current_WeaponManager.OnRemovedWeapon += RemoveWeapon;
            current_WeaponManager.OnSwitchedToWeapon += ChangeWeapon;
        }

        void AddWeapon(WeaponController newWeapon, int weaponIndex)
        {
            GameObject ammoCounterInstance = Instantiate(AmmoCounterPrefab, AmmoPanel);
            AmmoCounter newAmmoCounter = ammoCounterInstance.GetComponent<AmmoCounter>();
            DebugUtility.HandleErrorIfNullGetComponent<AmmoCounter, WeaponHUDManager>(newAmmoCounter, this,
                ammoCounterInstance.gameObject);

            newAmmoCounter.Initialize(newWeapon, weaponIndex);

            m_AmmoCounters.Add(newAmmoCounter);
        }

        void RemoveWeapon(WeaponController newWeapon, int weaponIndex)
        {
            int foundCounterIndex = -1;
            for (int i = 0; i < m_AmmoCounters.Count; i++)
            {
                if (m_AmmoCounters[i].WeaponCounterIndex == weaponIndex)
                {
                    foundCounterIndex = i;
                    Destroy(m_AmmoCounters[i].gameObject);
                }
            }

            if (foundCounterIndex >= 0)
            {
                m_AmmoCounters.RemoveAt(foundCounterIndex);
            }
        }

        void ChangeWeapon(WeaponController weapon)
        {
            UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(AmmoPanel);
        }

        void ChangeWeaponManager(PlayerWeaponsManager new_pwm)
        {
            current_WeaponManager = new_pwm;

        }
    }
}