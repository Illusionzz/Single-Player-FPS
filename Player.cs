using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [Header("Stats")]
    public int curHP;
    public int maxHP;
    public float jogSpeed;
    private float moveSpeed;
    public float sprintSpeed;
    public float jumpForce;

    [Header("Camera")]
    public float lookSensitivity;
    private float rotX;
    public float minXRot;
    public float maxXRot;
    
    private bool isDead;
    private bool flashingDamage;

    

    [Header("Components")]
    private Camera cam;
    public Rigidbody rig;
    private Material material;
    public MeshRenderer mr;
    private Weapon weapon;

    public static Player instance;
    private Grenade grenade;

    void Awake()
    {
        instance = this;
        cam = Camera.main;
        rig = GetComponent<Rigidbody>(); 
        weapon = GetComponent<Weapon>();
        Cursor.lockState = CursorLockMode.Locked;   
        grenade = GetComponent<Grenade>();
    }

    void Start()
    {
        UI.instance.UpdateHealthBar(curHP, maxHP);
        moveSpeed = jogSpeed;
    }

    void Update()
    {
        Move();
        CamLook();

        if (Input.GetKeyDown(KeyCode.Space))
            TryJump();

        if (Input.GetButton("Fire1"))
            if(weapon.TryShoot() == true)
                weapon.Shoot();
        
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();

        if (Input.GetKeyDown(KeyCode.R))
            weapon.Reload();

        // if (Input.GetKeyDown(KeyCode.G))
        //     if (grenade.TryThrow() == true)
        //         grenade.ThrowGernade();
    }

    void Move() 
    {
        float x = Input.GetAxis("Horizontal") * moveSpeed;
        float z = Input.GetAxis("Vertical") * moveSpeed;

        Vector3 dir = transform.right * x + transform.forward * z;
        dir.y = rig.velocity.y;

        if (Input.GetKeyDown(KeyCode.LeftShift)) {
            moveSpeed = sprintSpeed;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift)) {
            moveSpeed = jogSpeed;
        }

        rig.velocity = dir;
    }

    void CamLook() 
    {
        float y = Input.GetAxis("Mouse X") * lookSensitivity;
        rotX += Input.GetAxis("Mouse Y") * lookSensitivity;

        rotX = Mathf.Clamp(rotX, minXRot, maxXRot);

        cam.transform.localRotation = Quaternion.Euler(-rotX, 0, 0);
        transform.eulerAngles += Vector3.up * y;
    }

    void TryJump() 
    {
        Ray ray = new Ray(transform.position, Vector3.down);

        if (Physics.Raycast(ray, 1.1f)) 
            rig.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    public void TakeDamage(int damage) 
    {
        damage = 10;
        curHP -= damage;
        if (curHP == 0)
            Die();

        UI.instance.UpdateHealthBar(curHP, maxHP);
    }

    void DamageFlash() 
    {
        if (flashingDamage)
            return;

        StartCoroutine(FlashingDamage());

        IEnumerator FlashingDamage() 
        {
            flashingDamage = true;

            Color defaultColor = mr.material.color;
            mr.material.color = Color.red;

            yield return new WaitForSeconds(0.05f);

            mr.material.color = defaultColor;
            flashingDamage = false;
        }
    }

    public void GiveHealth(int amountToGive)
    {
        curHP = Mathf.Clamp(curHP += amountToGive, 0, maxHP);

        UI.instance.UpdateHealthBar(curHP, maxHP);
    }

    public void GiveAmmo(int amountToGive)
    {
        weapon.maxAmmo = Mathf.Clamp(weapon.maxAmmo += amountToGive, 0, weapon.maxAmmo);

        UI.instance.UpdateAmmoText();
    }

    void Die() 
    {
        curHP = 0;
        isDead = true;
        SceneManager.LoadScene("SampleScene");
    }
}
