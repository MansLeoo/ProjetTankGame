using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Tank tank;           // R�f�rence � l'objet � suivre
    public Slider healthSlider;      // R�f�rence au Slider de la barre de vie
    public Image fillImage;          // Image de remplissage pour changer la couleur
    private int maxHealth ;
    void Start()
    {
        maxHealth = tank.pointDeVie;
        healthSlider.maxValue = maxHealth;
    }

    void Update()
    {
        UpdateHealthBar();
    }



    private void UpdateHealthBar()
    {
        // Met � jour la valeur du Slider
        healthSlider.value = tank.pointDeVie;

        // Met � jour la couleur en fonction des PV
        float healthPercentage = (float)tank.pointDeVie / maxHealth;
        fillImage.color = Color.Lerp(Color.red, Color.green, healthPercentage);
    }


}

