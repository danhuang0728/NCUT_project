using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "tetr_database", menuName = "Tetris/tetr_database")]
public class tetr_database : ScriptableObject
{
    public enum Tetr_type
    {
        // 武器進化方塊
        S_1,        // 居合
        S_2,        // 亂砍
        C_1,        // 無限圓環
        F_1,        // 追蹤火球
        F_2,        // 巨大火球
        B_1,        // 無限彈射
        A_1,        // 飛斧進化
        A_2,        // 巨斧進化
        T_1,        // 西洋劍進化(全場下戳)

        // 基礎數值方塊(粉)
        Pink_1,     // 傷害+10
        Pink_2,     // 傷害+5, 爆擊傷害+12
        Pink_3,     // 爆擊率+10
        Pink_4,     // 爆擊率+10, 傷害+30
        Pink_5,     // 生命+25, 傷害+25
        Pink_6,     // 碰撞傷害, 速度+3, 生命+80
        Pink_7,     // 速度+1, 生命+30
        Pink_8,     // 碰撞傷害, 速度+10, 傷害+20
        
        // 基礎數值方塊(綠)
        Green_1,    // 生命+12, 傷害+10, 受傷+10
        Green_2,    // 生命+15, 傷害+15, 受傷+20
        Green_3,    // 生命+15, 傷害+15, 受傷+20
        Green_4,    // 生命+40, 傷害+80, 受傷+15
        
        // 基礎數值方塊(紫)
        Purple_1,   // 速度+20, 傷害+20, 生命+70
    }
    public string tetr_name;
    public Sprite tetr_sprite;
    public Tetr_type tetr_type;

}

