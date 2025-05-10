using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Objet : MonoBehaviour
{
    [Header("Caracteristique de l'objet")]
    [SerializeField] private int lifePoint ;
    [SerializeField] private int dammageTank;
    [SerializeField] private GameObject tank;

    [Header("Objet a la mort de l'objet")]
    [SerializeField] private GameObject explosionParticle;
    [SerializeField] private GameObject explosionSound;
    [SerializeField] private GameObject damageTextPrefab;



    private float lifeTime = 1f;  // Dur�e avant destruction
    private float floatSpeed = 3.0f; // Vitesse de mont�e
    private GameObject damageText;

    private bool dead; // Savoir si l'objet va mourir

    void Start()
    {
        dead = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(lifePoint <= 0 && !dead)
        {
            HandleDestruction(); // Si l'objet n'a plus de vie, declencher une explosion
            dead = true;    
        }
        if (damageText != null)
        {
            damageText.transform.position += Vector3.up * floatSpeed * Time.deltaTime;
            damageText.transform.LookAt(-tank.transform.position);
        }

    }
    /// <summary>
    /// G�re la destruction de l'objet en d�clenchant une explosion.
    /// </summary>
    private void HandleDestruction()
    {
        Instantiate(explosionSound, transform.position, Quaternion.identity);

        // Instancie les particules si elles sont assign�es
        if (explosionParticle != null)
        {
            Instantiate(explosionParticle, transform.position, Quaternion.identity);
        }
        gameObject.SetActive(false);
    }
    /// <summary>
    /// R�duit les points de vie de l'objet en fonction des d�g�ts re�us.
    /// </summary>
    /// <param name="damageAmount">La quantit� de d�g�ts inflig�s.</param>
    public void TakeDamage(int damageAmount)
    {
        lifePoint -= damageAmount;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet")) 
        {
            collision.gameObject.SetActive(false);
            TakeDamage(10);
            ShowDamageText(10);
        }
        if (collision.gameObject.CompareTag("Shell")) 
        {
            collision.gameObject.SetActive(false);

            TakeDamage(30);
            ShowDamageText(30);
        }
        if (collision.gameObject.CompareTag("PlayerTank") && !dead) 
        {
            Tank tank = collision.gameObject.GetComponent<Tank>();
            tank.TakeDammage(dammageTank);
            ShowDamageText(30);
            TakeDamage(30);

        }
    }
    /// <summary>
    /// Affiche un texte temporaire indiquant les d�g�ts inflig�s.
    /// </summary>
    /// <param name="damage">La quantit� de d�g�ts � afficher.</param>
    private void ShowDamageText(int damage)
    {
        // Instancie le prefab du texte
        damageText = Instantiate(damageTextPrefab, this.gameObject.transform.position, tank.transform.localRotation);
        Destroy(damageText, lifeTime); // D�truire apr�s la dur�e sp�cifi�e

        TMP_Text textComponent = damageText.GetComponentInChildren<TMP_Text>();
            textComponent.text = damage.ToString(); // Mise a jour du texte


    }
}
