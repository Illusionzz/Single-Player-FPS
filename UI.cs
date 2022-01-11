using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    public Slider healthBar;
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI grenadeText;

    private Player player;
    private Weapon weapon;

    public static UI instance;

    void Awake()
    {
        instance = this;
        player = GetComponent<Player>();
        weapon = GetComponent<Weapon>();
    }

    public void UpdateHealthBar(int curHP, int maxHP) 
    {
        healthBar.value = (float)curHP / (float)maxHP;
    }

    public void UpdateAmmoText() 
    {
        ammoText.text = Weapon.instance.curAmmo + " / " + Weapon.instance.maxAmmo;
    }

    public void UpdateGrenadeText() 
    {
        grenadeText.text = GrenadeThrower.instance.curGrenadeAmmo + " / " + GrenadeThrower.instance.maxGrenadeAmmo;
    }
}
