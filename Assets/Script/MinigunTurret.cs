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
    [SerializeField] private float fireDelay = 0.1f; // Délai entre chaque tir

    private float nextRotationTime = 0f; // Moment où la prochaine rotation doit se produire
    private Quaternion targetRotation; // Angle de rotation cible
    private float timeSinceLastShot = 0f; // Temps écoulé depuis le dernier tir
    private int bulletsFired = 0; // Compteur de balles tirées
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
        // Si le temps est écoulé, on change d'angle
        if (Time.time >= nextRotationTime)
        {
            SetNewRotation();
        }

        // Faire tourner la partie supérieure de la tourelle uniquement sur l'axe Z
        topTurret.transform.localRotation = Quaternion.RotateTowards(
            topTurret.transform.localRotation,
            targetRotation,
            Time.deltaTime * 100f // Vitesse de rotation
        );

        // Si le tank est à portée, tirer des balles
        if (distance < 100f)
        {
            FireBullets();
        }
        else
        {
            // Réinitialiser si le tank sort de portée
            isFiring = false;
            bulletsFired = 0;
        }
    }

    /// <summary>
    /// Définit un nouvel angle de rotation aléatoire pour la tourelle.
    /// </summary>
    private void SetNewRotation()
    {
        float randomAngle = Random.Range(0, 360); // Angle entre 0° et 360°
        targetRotation = Quaternion.Euler(0f, 0f, randomAngle); // Rotation uniquement sur l'axe Z
        nextRotationTime = Time.time + rotationInterval; // Calcul du prochain intervalle
    }
    /// <summary>
    /// Tire des balles à intervalles réguliers .
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

                // Ajouter une force à la balle pour la faire avancer
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
            isFiring = false; // Arrêter le tir après 10 balles
            bulletsFired = 0; // Réinitialiser le compteur
        }
    }
}

