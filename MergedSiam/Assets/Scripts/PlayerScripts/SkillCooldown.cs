using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SkillCooldown : MonoBehaviour {

    public Image imageCooldown;
    public Text textCd;
    public float cooldown;
    private float cooldownTimer;
    private Button btn;
    bool isCooldown;
    void Start()
    {
        textCd.text = null;
        imageCooldown.fillAmount = 0;
        cooldownTimer = cooldown;
        btn = GetComponent<Button>();
        btn.onClick.AddListener(CooldownOnClick);
    }
    // Update is called once per frame
    void Update () {
        if (isCooldown)
        { 
            btn.interactable = false;
            imageCooldown.fillAmount -=( 1 / cooldown )* Time.deltaTime;
            cooldownTimer -= Time.deltaTime;
            textCd.text = Mathf.RoundToInt(cooldownTimer).ToString(); 
            if (imageCooldown.fillAmount <= 0)
            {
                textCd.text = null;
                imageCooldown.fillAmount = 0;
                isCooldown = false;
                btn.interactable = true;
            }
        }
	}
    void CooldownOnClick()
    {
        isCooldown = true;
        imageCooldown.fillAmount = 1;
        textCd.text = cooldown.ToString();
        Debug.Log("Skill went on cooldown for "+cooldown+" seconds");
    }
}
