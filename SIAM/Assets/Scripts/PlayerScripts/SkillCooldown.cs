using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SkillCooldown : MonoBehaviour {

    public Image imageCooldown;
    public float cooldown;
    private Button btn;
    bool isCooldown;
    void Start()
    {
        imageCooldown.fillAmount = 0;
        btn = GetComponent<Button>();
        btn.onClick.AddListener(CooldownOnClick);
    }
    // Update is called once per frame
    void Update () {
        if (isCooldown)
        { 
            btn.interactable = false;
            imageCooldown.fillAmount -= 1 / cooldown * Time.deltaTime;

            if (imageCooldown.fillAmount <= 0)
            {
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
        Debug.Log("Skill went on cooldown for "+cooldown+" seconds");
    }
}
