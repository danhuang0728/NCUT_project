using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class killed_monster : MonoBehaviour
{
    public TextMeshProUGUI killed_monster_text;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int currentKills = PlayerControl.GetKillCount();
        killed_monster_text.text = currentKills.ToString();
    }
}
