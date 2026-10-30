
using UnityEngine;
using System.Collections;

public class PlayerWeapon : MonoBehaviour
{
    [Header("Referencias de Disparo")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 30f;

    [Header("Modelos Visuales en Pantalla")]
    public GameObject[] weaponModels;

    [Header("Sistema de Armas")]
    public int currentWeaponIndex = 0;

    private string[] weaponNames = {
        "Pistola Táctica",
        "Escopeta de Combate",
        "Rifle de Asalto"
    };

    // Tiempo entre disparos de cada arma
    private float[] fireRates = { 0.3f, 0.9f, 0.12f };

    // Armas automáticas
    private bool[] isAutomatic = { false, false, true };

    // Control individual del tiempo de disparo
    private float[] nextFireTimes = new float[3];

    [Header("Sistema de Munición y Recarga")]
    public int[] maxAmmoInMag = { 12, 8, 30 };
    public int[] currentAmmoInMag = { 12, 8, 30 };
    public int[] reserveAmmo = { 60, 32, 120 };

    public float[] reloadTimes = { 1.5f, 2.2f, 2.0f };

    private bool isReloading = false;

    void Start()
    {
        UpdateWeaponVisuals();
    }

    void Update()
    {
        // Bloquear acciones durante la recarga
        if (isReloading)
            return;

        HandleWeaponSwitching();

        // Recargar con R
        if (Input.GetKeyDown(KeyCode.R))
        {
            TryReload();
            return;
        }

        HandleShooting();
    }

    // CONTROL DE DISPARO
    void HandleShooting()
    {
        int weapon = currentWeaponIndex;

        bool shouldShoot;

        if (isAutomatic[weapon])
        {
            // Rifle automático
            shouldShoot = Input.GetMouseButton(0);
        }
        else
        {
            // Pistola y escopeta semiautomáticas
            shouldShoot = Input.GetMouseButtonDown(0);
        }

        // Comprobar cooldown
        if (!shouldShoot || Time.time < nextFireTimes[weapon])
            return;

        // Comprobar munición
        if (currentAmmoInMag[weapon] <= 0)
        {
            TryReload();
            return;
        }

        Shoot();
    }

    // CAMBIAR DE ARMA
    void HandleWeaponSwitching()
    {
        int previousWeapon = currentWeaponIndex;

        if (Input.GetKeyDown(KeyCode.Alpha1))
            currentWeaponIndex = 0;

        else if (Input.GetKeyDown(KeyCode.Alpha2))
            currentWeaponIndex = 1;

        else if (Input.GetKeyDown(KeyCode.Alpha3))
            currentWeaponIndex = 2;

        currentWeaponIndex = Mathf.Clamp(currentWeaponIndex, 0, 2);

        if (previousWeapon != currentWeaponIndex)
        {
            UpdateWeaponVisuals();

            Debug.Log(
                "Arma equipada: " + weaponNames[currentWeaponIndex] +
                " | Balas: " + currentAmmoInMag[currentWeaponIndex] +
                "/" + reserveAmmo[currentWeaponIndex]
            );
        }
    }

    // ACTUALIZAR MODELOS VISUALES
    void UpdateWeaponVisuals()
    {
        for (int i = 0; i < weaponModels.Length; i++)
        {
            if (weaponModels[i] != null)
            {
                weaponModels[i].SetActive(i == currentWeaponIndex);
            }
        }
    }

    // INTENTAR RECARGAR
    void TryReload()
    {
        int weapon = currentWeaponIndex;

        if (isReloading)
            return;

        if (currentAmmoInMag[weapon] >= maxAmmoInMag[weapon])
            return;

        if (reserveAmmo[weapon] <= 0)
        {
            Debug.Log("Sin munición de reserva.");
            return;
        }

        StartCoroutine(ReloadRoutine(weapon));
    }

    // SISTEMA DE RECARGA
    IEnumerator ReloadRoutine(int weapon)
    {
        isReloading = true;

        Debug.Log("Recargando " + weaponNames[weapon] + "...");

        yield return new WaitForSeconds(reloadTimes[weapon]);

        int bulletsNeeded =
            maxAmmoInMag[weapon] - currentAmmoInMag[weapon];

        int bulletsToReload =
            Mathf.Min(bulletsNeeded, reserveAmmo[weapon]);

        currentAmmoInMag[weapon] += bulletsToReload;
        reserveAmmo[weapon] -= bulletsToReload;

        // Pequeña espera antes del siguiente disparo
        nextFireTimes[weapon] = Time.time + 0.2f;

        isReloading = false;

        Debug.Log(
            "Recarga completada: " +
            currentAmmoInMag[weapon] + "/" +
            reserveAmmo[weapon]
        );
    }

    // DISPARAR
    void Shoot()
    {
        int weapon = currentWeaponIndex;

        if (isReloading || currentAmmoInMag[weapon] <= 0)
            return;

        if (bulletPrefab == null || firePoint == null)
        {
            Debug.LogWarning(
                "Falta asignar Bullet Prefab o Fire Point."
            );
            return;
        }

        // Crear proyectil
        GameObject bullet = Instantiate(
            bulletPrefab,
            firePoint.position,
            firePoint.rotation
        );

        // Aplicar velocidad al proyectil
        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.velocity = firePoint.forward * bulletSpeed;
        }
        else
        {
            Debug.LogWarning("La bala no tiene Rigidbody.");
        }

        // Destruir bala después de 3 segundos
        Destroy(bullet, 3f);

        // Descontar munición
        currentAmmoInMag[weapon]--;

        // Activar cooldown del arma
        nextFireTimes[weapon] = Time.time + fireRates[weapon];

        Debug.Log(
            "[DISPARO] " + weaponNames[weapon] +
            " | Cargador: " + currentAmmoInMag[weapon] +
            "/" + maxAmmoInMag[weapon]
        );
    }
}
