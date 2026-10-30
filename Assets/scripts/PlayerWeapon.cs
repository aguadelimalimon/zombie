using UnityEngine;
using System.Collections;

public class PlayerWeapon : MonoBehaviour
{
    [Header("Referencias de Disparo")]
    public GameObject bulletPrefab;
    public Transform firePoint; 

    [Header("Modelos Visuales en Pantalla")]
    // Arrastra aquí los GameObjects 3D de tus armas (hijos de la cámara)
    public GameObject[] weaponModels; 

    [Header("Sistema de Armas (Depurador)")]
    public int currentWeaponIndex = 0;
    
    private string[] weaponNames = { "Pistola Táctica", "Escopeta de Combate", "Rifle de Asalto" };
    private float[] fireRates = { 0.3f, 0.9f, 0.12f };     
    private bool[] isAutomatic = { false, false, true };    
    
    [Header("Sistema de Munición y Recarga")]
    public int[] maxAmmoInMag = { 12, 8, 30 };      
    public int[] currentAmmoInMag = { 12, 8, 30 };  
    public int[] reserveAmmo = { 60, 32, 120 };     
    public float[] reloadTimes = { 1.5f, 2.2f, 2.0f }; 
    
    private bool isReloading = false;
    private float nextFireTime = 0f;

    void Start()
    {
        // Al iniciar el juego, activamos solo el arma inicial y ocultamos las demás
        UpdateWeaponVisuals();
    }

    void Update()
    {
        if (isReloading) return;

        if (Input.GetKeyDown(KeyCode.R))
        {
            TryReload();
            return;
        }

        HandleWeaponSwitching();

        bool shouldShoot = false;

        if (isAutomatic[currentWeaponIndex])
        {
            shouldShoot = Input.GetMouseButton(0) && Time.time >= nextFireTime;
        }
        else
        {
            shouldShoot = Input.GetMouseButtonDown(0) && Time.time >= nextFireTime;
        }

        if (shouldShoot)
        {
            if (isReloading) return; 

            if (currentAmmoInMag[currentWeaponIndex] > 0)
            {
                nextFireTime = Time.time + fireRates[currentWeaponIndex];
                Shoot();
            }
            else
            {
                Debug.Log("⚠️ ¡CARGADOR VACÍO! Recargando...");
                TryReload();
            }
        }
    }

    void HandleWeaponSwitching()
    {
        if (isReloading) return;

        int previousWeapon = currentWeaponIndex;

        if (Input.GetKeyDown(KeyCode.Alpha1)) currentWeaponIndex = 0;
        else if (Input.GetKeyDown(KeyCode.Alpha2)) currentWeaponIndex = 1;
        else if (Input.GetKeyDown(KeyCode.Alpha3)) currentWeaponIndex = 2;

        // Si cambiamos de arma, actualizamos los modelos visuales en pantalla
        if (previousWeapon != currentWeaponIndex)
        {
            UpdateWeaponVisuals();
            Debug.Log("🔄 Arma equipada: " + weaponNames[currentWeaponIndex] + " | Balas: " + currentAmmoInMag[currentWeaponIndex] + "/" + reserveAmmo[currentWeaponIndex]);
        }
    }

    // Método para apagar las armas que no usas y prender la activa
    void UpdateWeaponVisuals()
    {
        for (int i = 0; i < weaponModels.Length; i++)
        {
            if (weaponModels[i] != null)
            {
                // Si es el índice actual, se muestra; si no, se oculta
                weaponModels[i].SetActive(i == currentWeaponIndex);
            }
        }
    }

    void TryReload()
    {
        if (isReloading) return;

        if (currentAmmoInMag[currentWeaponIndex] < maxAmmoInMag[currentWeaponIndex] && reserveAmmo[currentWeaponIndex] > 0)
        {
            StartCoroutine(ReloadRoutine());
        }
        else if (reserveAmmo[currentWeaponIndex] <= 0)
        {
            Debug.Log("❌ ¡Sin munición de reserva!");
        }
    }

    IEnumerator ReloadRoutine()
    {
        isReloading = true;
        Debug.Log("🔄 Recargando " + weaponNames[currentWeaponIndex] + "...");

        yield return new WaitForSeconds(reloadTimes[currentWeaponIndex]);

        int bulletsNeeded = maxAmmoInMag[currentWeaponIndex] - currentAmmoInMag[currentWeaponIndex];

        if (reserveAmmo[currentWeaponIndex] >= bulletsNeeded)
        {
            reserveAmmo[currentWeaponIndex] -= bulletsNeeded;
            currentAmmoInMag[currentWeaponIndex] = maxAmmoInMag[currentWeaponIndex];
        }
        else
        {
            currentAmmoInMag[currentWeaponIndex] += reserveAmmo[currentWeaponIndex];
            reserveAmmo[currentWeaponIndex] = 0;
        }

        nextFireTime = Time.time + 0.3f;
        isReloading = false;

        Debug.Log("✅ Recarga lista. Balas: " + currentAmmoInMag[currentWeaponIndex]);
    }

    void Shoot()
    {
        if (isReloading) return;
        if (currentAmmoInMag[currentWeaponIndex] <= 0) return;

        if (bulletPrefab != null && firePoint != null)
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        }

        currentAmmoInMag[currentWeaponIndex]--;
        Debug.Log("💥 [DISPARO] " + weaponNames[currentWeaponIndex] + " - Balas: " + currentAmmoInMag[currentWeaponIndex] + "/" + maxAmmoInMag[currentWeaponIndex]);
    }
}