using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigunTurret : MonoBehaviour
{
    [Header("Liste des Prefab")]
    [SerializeField] private GameObject tank; 
    [SerializeField] private GameObject topTurret; 
    [SerializeField] private GameObject bullet; 
    [SerializeField] private Transform spawnPoint;

    [Header("Parametre de la tourelle")]
    [SerializeField] private float rotationInterval = 3f; // Intervalle de rotation (en secondes)
    [SerializeField] private float bulletSpeed = 20f; // Vitesse des balles
    [SerializeField] private float fireDelay = 0.1f; // D�lai entre chaque tir

    private float nextRotationTime = 0f; // Moment o� la prochaine rotation doit se produire
    private Quaternion targetRotation; // Angle de rotation cible
    private float timeSinceLastShot = 0f; // Temps �coul� depuis le dernier tir
    private int bulletsFired = 0; // Compteur de balles tir�es
    private bool isFiring = false; // Indique si la tourelle est en train de tirer
    void Start()
    {
        // Initialisation du premier angle cible
        SetNewRotation();
    }

    void Update()
    {
        // Distance entre la tourelle et le tank
        float distance = Vector3.Distance(transform.position, tank.transform.position);
        // Si le temps est �coul�, on change d'angle
        if (Time.time >= nextRotationTime)
        {
            SetNewRotation();
        }

        // Faire tourner la partie sup�rieure de la tourelle uniquement sur l'axe Z
        topTurret.transform.localRotation = Quaternion.RotateTowards(
            topTurret.transform.localRotation,
            targetRotation,
            Time.deltaTime * 100f // Vitesse de rotation
        );

        // Si le tank est � port�e, tirer des balles
        if (distance < 100f)
        {
            FireBullets();
        }
        else
        {
            // R�initialiser si le tank sort de port�e
            isFiring = false;
            bulletsFired = 0;
        }
    }

    /// <summary>
    /// D�finit un nouvel angle de rotation al�atoire pour la tourelle.
    /// </summary>
    private void SetNewRotation()
    {
        float randomAngle = Random.Range(0, 360); // Angle entre 0� et 360�
        targetRotation = Quaternion.Euler(0f, 0f, randomAngle); // Rotation uniquement sur l'axe Z
        nextRotationTime = Time.time + rotationInterval; // Calcul du prochain intervalle
    }
    /// <summary>
    /// Tire des balles � intervalles r�guliers .
    /// </summary>
    private void FireBullets()
    {
        if (!isFiring)
        {
            isFiring = true;
            timeSinceLastShot = 0f;
        }

        if (isFiring && bulletsFired < 10)
        {
            timeSinceLastShot += Time.deltaTime;

            if (timeSinceLastShot >= fireDelay)
            {
                // Instancier une balle au point de spawn
                GameObject newBullet = Instantiate(bullet, spawnPoint.position, spawnPoint.rotation);

                // Ajouter une force � la balle pour la faire avancer
                Rigidbody rb = newBullet.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.velocity = spawnPoint.forward * bulletSpeed;
                }

                bulletsFired++;
                timeSinceLastShot = 0f;
            }
        }

        if (bulletsFired >= 10)
        {
            isFiring = false; // Arr�ter le tir apr�s 10 balles
            bulletsFired = 0; // R�initialiser le compteur
        }
    }
}

