using UnityEngine;
using UnityEngine.UI;

public class ButtonCooldown : MonoBehaviour
{
    
    private Button button ;           
    [SerializeField] private  Image cooldownImage;     // L'image grise couvrant le bouton
    [SerializeField] private Tank tank;     
    [SerializeField] private int indexWeaponButton;
    public float cooldownTime ; // Temps de délai en secondes

    void Start()
    {
        button = this.gameObject.GetComponent<Button>();
        button.interactable = false;
        cooldownImage.fillAmount = 0; // Aucune zone grise visible
        cooldownTime = tank.switchWeaponTime;
    }

    void Update()
    {

            // Met à jour la zone grise
            cooldownImage.fillAmount = tank.currentsSwitchWeaponTime / cooldownTime;

        // Si le délai est terminé, réactiver le bouton
        if (tank.currentsSwitchWeaponTime <= 0)
        {
            tank.currentsSwitchWeaponTime = 0;
        }
        else
        {
            button.interactable = true;
        }
    }

    public void OnButtonClicked()
    {
        if (tank.currentsSwitchWeaponTime <= 0)
        {
            tank.SwitchCamera(indexWeaponButton); // Change la camera
        }
    }


}
