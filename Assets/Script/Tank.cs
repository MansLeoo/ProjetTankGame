using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Tank : MonoBehaviour
{ 
   [SerializeField] private RenderTexture renderTexture;
   [SerializeField] private GameObject hautTank;
   [SerializeField] private GameObject cannon;
   [SerializeField] private GameObject mitrailleuse;
   [SerializeField] public int pointDeVie;
   [SerializeField] private int vitesse;
   [SerializeField] public float switchWeaponTime ;
   public float currentsSwitchWeaponTime;
   [SerializeField] private AudioSource engineSound;
   [SerializeField] private List<GameObject> bullets = new List<GameObject>();
   [SerializeField] private GameObject explosionParticle;
   [SerializeField] private GameObject explosionSound;
   [SerializeField] private List<Camera> cameraList;
   [SerializeField] private List<float> waitTime = new List<float> { 5.0f, 1.0f };
   [SerializeField] private List<GameObject> weapons = new List<GameObject>();
    private int rotationSpeed;
    private Rigidbody rb;
    private Vector3 moveDirection;
    private float timerShoot;
    private int indexCamera = 0;
    private int shootPower = 50;
    private int indexBullet = 0;

    void Start()
    {
        rotationSpeed = 8;
        rb = GetComponent<Rigidbody>();
        timerShoot = waitTime[0];
        currentsSwitchWeaponTime = switchWeaponTime;
    }

    // Update is called once per frame
    void Update()
    {
        timerShoot -= Time.deltaTime;
        currentsSwitchWeaponTime -= Time.deltaTime;

        #region Movements
        // Récupération des inputs utilisateur
        float speedX = Input.GetAxis("Vertical"); // Flèches haut/bas pour avancer/reculer
        float rotationInput = Input.GetAxis("Horizontal"); // Flèches gauche/droite pour tourner

        // Définir les axes locaux de mouvement
        Vector3 forward = transform.right; // Direction pour avancer/reculer (Vector3.right car la voiture avance selon l'axe X local)

        // Calcul de la direction de déplacement
        moveDirection = forward * speedX * vitesse;

        // Appliquer une force pour déplacer la voiture
        rb.MovePosition(rb.position + moveDirection * Time.fixedDeltaTime);

        // Rotation réaliste seulement si la voiture avance/recul
        if (Mathf.Abs(speedX) > 0.1f) // Vérifie si la voiture est en mouvement
        {
            // Calculer la rotation dépendant de la vitesse et de l'input
            float rotationAmount = -rotationInput * rotationSpeed * Time.fixedDeltaTime;
            Quaternion rotation = Quaternion.Euler(0, 0, -rotationAmount); // Rotation uniquement sur Z
            rb.MoveRotation(rb.rotation * rotation);
        }

        // Gestion du son du moteur
        if (speedX > 0.1f) // Si la voiture se déplace
        {
            if (!engineSound.isPlaying)
            {
                engineSound.Play(); // Démarrer le son du moteur
            }
        }
        else // Si la voiture est immobile
        {
            if (engineSound.isPlaying)
            {
                engineSound.Stop(); // Arrêter le son du moteur
            }
        }



        #endregion
        #region Cannon
        if (Input.GetKey(KeyCode.O))
        {
            if(indexBullet == 0) // si cannon
            {
                RotateCannon(-rotationSpeed * Time.deltaTime); // Rotation vers le haut

            }
            else
            {
                RotateMitrailleuse(rotationSpeed*1.5f * Time.deltaTime); // Rotation vers le haut

            }
        }

        if (Input.GetKey(KeyCode.L))
        {
            if (indexBullet == 0) // si cannon
            {
                RotateCannon(rotationSpeed * Time.deltaTime); // Rotation vers le bas
            }
            else
            {
                RotateMitrailleuse(-rotationSpeed * 1.5f * Time.deltaTime); // Rotation vers le haut

            }
        }
        #endregion
        #region CameraSwitch

        if (Input.GetKeyDown(KeyCode.R) && currentsSwitchWeaponTime <= 0)
        {
            //Change la camera,le type de munition ainsi que le point d'apparition des munitions
            currentsSwitchWeaponTime = switchWeaponTime;
            cameraList[indexCamera].enabled = false;
            indexCamera++;
            indexBullet++;
            if (indexCamera >= cameraList.Count)
            {
                indexCamera = 0;
                indexBullet = 0;
                timerShoot = waitTime[indexBullet];

            }
            cameraList[indexCamera].enabled = true;
            cameraList[indexCamera].targetTexture = renderTexture;
        }
        #endregion
        #region shoot
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (timerShoot <= 0)
            {
                ShootBullet(weapons[indexBullet].transform);
                weapons[indexBullet].GetComponent<AudioSource>().Play();
                timerShoot = waitTime[indexBullet];
            }

        }
        #endregion
        #region rotation
        if (Input.GetKey(KeyCode.E)) // Rotation vers la droite
        {
            if (indexBullet == 0)
            {
                RotateRight();

            }
            else
            {
                RotateRightMitrailleuse();
            }
        }
        else if (Input.GetKey(KeyCode.A)) // Rotation vers la gauche
        {
            {
                if (indexBullet == 0)
                {
                    RotateLeft();

                }
                else
                {
                    RotateLeftMitrailleuse();
                }
            }
        }
        #endregion
        #region GameOver
        if(this.pointDeVie <= 0)
        {
            Instantiate(explosionParticle, transform.position, Quaternion.identity);
            gameObject.SetActive(false);
            SceneManager.LoadScene("MainMenu"); // Retour au menu principale
        }
        #endregion
    }

    
    /// <summary>
    /// Permet de basculer entre différentes caméras en fonction de l'index spécifié.
    /// </summary>
    /// <param name="index">Index de la caméra à activer.</param>
    public void SwitchCamera(int index)
    {
        currentsSwitchWeaponTime = switchWeaponTime;

        cameraList[indexCamera].enabled = false;
        indexCamera = index;
        indexBullet = index;
        cameraList[indexCamera].enabled = true;
        cameraList[indexCamera].targetTexture = renderTexture;
    }
    /// <summary>
    /// Tire un projectile depuis une position donnée, en ajustant la rotation pour correspondre au canon ou à la mitrailleuse.
    /// </summary>
    /// <param name="pos">La position de tir.</param>
    private void ShootBullet(Transform pos)
    {
        Quaternion cannonRotation;
        // Obtenir la rotation du canon/mitrailleuse
        if(indexBullet == 0)
        {
            cannonRotation = hautTank.transform.rotation;

        }
        else
        {
            cannonRotation = mitrailleuse.transform.rotation;

        }

        // Compense l'alignement des axes entre le canon et l'obus
        Quaternion adjustedRotation = Quaternion.Euler(cannonRotation.eulerAngles.x , cannonRotation.eulerAngles.y + 90, cannonRotation.eulerAngles.z);

        GameObject newBullet;

           newBullet = Instantiate(bullets[indexBullet], pos.position, adjustedRotation);


        newBullet.GetComponent<Rigidbody>().velocity = pos.forward * shootPower;
    }
    /// <summary>
    /// Fait pivoter le haut du tank vers la droite.
    /// </summary>
    void RotateRight()
    {
        Vector3 currentEulerAngles = hautTank.transform.localEulerAngles;

        // Calculer l'angle de rotation proposé
        float proposedAngle = currentEulerAngles.x + (rotationSpeed * Time.deltaTime);

        // Appliquer la rotation uniquement si elle ne dépasse pas 359
        if (proposedAngle < 359)
        {
            hautTank.transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
        }

    }

    /// <summary>
    /// Fait pivoter le haut du tank vers la gauche.
    /// </summary>
    void RotateLeft()
    {
        Vector3 currentEulerAngles = hautTank.transform.localEulerAngles;

        // Calculer l'angle de rotation proposé
        float proposedAngle = currentEulerAngles.x - (rotationSpeed * Time.deltaTime);
        // Appliquer la rotation uniquement si elle ne dépasse pas 0
        if ( proposedAngle > 0)
        {
            hautTank.transform.Rotate(Vector3.down * rotationSpeed * Time.deltaTime);
        }


    }
    /// <summary>
    /// Fait pivoter la mitrailleuse vers la droite.
    /// </summary>
    void RotateRightMitrailleuse()
    {

            mitrailleuse.transform.Rotate(Vector3.up * rotationSpeed * 2 * Time.deltaTime);

    }
    /// <summary>
    /// Fait pivoter la mitrailleuse vers la gauche.
    /// </summary>
    void RotateLeftMitrailleuse()
    {
            mitrailleuse.transform.Rotate(Vector3.down * rotationSpeed*2 * Time.deltaTime);
    }
    /// <summary>
    /// Fait pivoter la mitrailleuse d'un angle donné tout en le contraignant entre -20° et 10°.
    /// </summary>
    /// <param name="angle">L'angle de rotation à appliquer.</param>
    private void RotateMitrailleuse(float angle)
    {
        float currentAngle = mitrailleuse.transform.localEulerAngles.z;

        // Convertit l'angle actuel à une plage de -180 à 180
        if (currentAngle > 180f)
        {
            currentAngle -= 360f;
        }

        // Calcule le nouvel angle tout en le limitant entre -20 et 10
        float newAngle = Mathf.Clamp(currentAngle + angle, -20f, 10f);

         mitrailleuse.transform.localEulerAngles = new Vector3(mitrailleuse.transform.localEulerAngles.x, mitrailleuse.transform.localEulerAngles.y, newAngle);
    }
    /// <summary>
    /// Fait pivoter le canon d'un angle donné tout en le contraignant entre -20° et 10°.
    /// </summary>
    /// <param name="angle">L'angle de rotation à appliquer.</param>
    private void RotateCannon(float angle)
    {
        float currentAngle = cannon.transform.localEulerAngles.x;

        // Convertit l'angle actuel à une plage de -180 à 180
        if (currentAngle > 180f)
        {
            currentAngle -= 360f;
        }

        // Calcule le nouvel angle tout en le limitant entre -20 et 10
        float newAngle = Mathf.Clamp(currentAngle + angle, -20f, 10f);

        cannon.transform.localEulerAngles = new Vector3(newAngle, cannon.transform.localEulerAngles.y, cannon.transform.localEulerAngles.z);
    }
    /// <summary>
    /// Applique des dégâts au tank en réduisant ses points de vie.
    /// </summary>
    /// <param name="dammage">La quantité de dégâts infligés.</param>
    public void TakeDammage(int dammage)
    {
        this.pointDeVie-=dammage;
    }

}

