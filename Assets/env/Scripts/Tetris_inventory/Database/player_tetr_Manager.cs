using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_tetr_Manager : MonoBehaviour
{
    public TetrisInventoryManager tetrisInventoryManager;
    public player_tetr_database player_tetr_database;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Update_UI()
    {
        // 歷遍player_tetr_database把裡面有的database全部執行create_tetr_obj
        if (player_tetr_database != null && player_tetr_database.tetr_database != null)
        {
            foreach (tetr_database tetr in player_tetr_database.tetr_database)
            {
                if (tetr != null)
                {
                    create_tetr_obj(tetr);
                }
            }
        }
    }
    public void add_tetr_obj(tetr_database tetr_database)
    {
        player_tetr_database.tetr_database.Add(tetr_database);
        Update_UI();
    }
    void create_tetr_obj(tetr_database tetr_database)
    {
        switch(tetr_database.tetr_type)
        {
            case tetr_database.Tetr_type.S_1:
                tetrisInventoryManager.CreateTetrisPiece(
                    new Vector2Int[] { 
                        new Vector2Int(0, 0),
                        new Vector2Int(0, 7),
                        new Vector2Int(1, 1),
                        new Vector2Int(1, 6),
                        new Vector2Int(2, 2),
                        new Vector2Int(2, 5),
                        new Vector2Int(3, 3),
                        new Vector2Int(3, 4),
                        new Vector2Int(4, 3),
                        new Vector2Int(4, 4),
                        new Vector2Int(5, 2),
                        new Vector2Int(5, 5),
                        new Vector2Int(6, 1),
                        new Vector2Int(6, 6),
                        new Vector2Int(7, 0),
                        new Vector2Int(7, 7),
                    }, 
                    Color.yellow,
                    new Vector2(0.5f, 0.5f), 
                    "(S_1)", //居合斬
                    false
                );
                break;

            case tetr_database.Tetr_type.S_2:
                tetrisInventoryManager.CreateTetrisPiece(
                    new Vector2Int[] {
                        new Vector2Int(4, 0),
                        new Vector2Int(3, 1), new Vector2Int(5, 1),
                        new Vector2Int(3, 2), new Vector2Int(6, 2),
                        new Vector2Int(1, 3), new Vector2Int(7, 3),
                        new Vector2Int(0, 4), new Vector2Int(6, 4),
                        new Vector2Int(1, 5), new Vector2Int(4, 5),
                        new Vector2Int(2, 6), new Vector2Int(4, 6),
                        new Vector2Int(3, 7)
                    },
                    Color.yellow,
                    new Vector2(3.5f, 3.5f), // 中心點可依需要微調
                    "(S_2)", //亂砍
                    false
                );
                break;

            case tetr_database.Tetr_type.A_1:
                tetrisInventoryManager.CreateTetrisPiece(
                    new Vector2Int[] {
                        new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(2, 0), new Vector2Int(3, 0),
                        new Vector2Int(0, 1), new Vector2Int(1, 1), new Vector2Int(2, 1), new Vector2Int(3, 1),
                        new Vector2Int(0, 2), new Vector2Int(1, 2),
                        new Vector2Int(0, 3), new Vector2Int(1, 3)
                    },
                    Color.yellow,
                    new Vector2(1.5f, 1.5f),
                    "(A_1)" //飛斧
                );
                break;

            case tetr_database.Tetr_type.C_1:
                tetrisInventoryManager.CreateTetrisPiece(
                    new Vector2Int[] {
                        new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(2, 0),
                        new Vector2Int(3, 0), new Vector2Int(4, 0), new Vector2Int(5, 0),
                        new Vector2Int(0, 5), new Vector2Int(1, 5), new Vector2Int(2, 5),
                        new Vector2Int(3, 5), new Vector2Int(4, 5), new Vector2Int(5, 5),
                        new Vector2Int(0, 1), new Vector2Int(0, 2),
                        new Vector2Int(0, 3), new Vector2Int(0, 4),
                        new Vector2Int(5, 1), new Vector2Int(5, 2),
                        new Vector2Int(5, 3), new Vector2Int(5, 4)
                    },
                    new Color(0.5f, 0f, 1f), // 紫色（可依需求微調）
                    new Vector2(0,0), // 中心點正中央
                    "(C_1)", //圓環
                    false
                );
                break;

            case tetr_database.Tetr_type.F_1:
                tetrisInventoryManager.CreateTetrisPiece(
                    new Vector2Int[] { 
                        new Vector2Int(0, 0), new Vector2Int(1, 0), 
                        new Vector2Int(0, 1), new Vector2Int(1, 1), new Vector2Int(2, 1),
                        new Vector2Int(2, 2),
                        new Vector2Int(1, 3),
                        new Vector2Int(0, 4),
                        new Vector2Int(1, 5),
                        new Vector2Int(2, 6)
                    }, 
                    Color.cyan,
                    new Vector2(1.0f, 2.5f),  // 設置中心點在形狀的幾何中心附近
                    "(F_1)" //跟蹤火球
                );
                break;

            case tetr_database.Tetr_type.A_2:
                tetrisInventoryManager.CreateTetrisPiece(
                    new Vector2Int[] { 
                        new Vector2Int(5, 0), 
                        new Vector2Int(4, 1), new Vector2Int(5, 1),new Vector2Int(6, 1),
                        new Vector2Int(3, 2), new Vector2Int(4, 2),new Vector2Int(5, 2), new Vector2Int(6, 2),new Vector2Int(7, 2),
                        new Vector2Int(2, 3), new Vector2Int(3, 3),new Vector2Int(4, 3), new Vector2Int(5, 3),new Vector2Int(6, 3),new Vector2Int(7, 3),
                        new Vector2Int(1, 4), new Vector2Int(2, 4),new Vector2Int(5, 4), new Vector2Int(6, 4),
                        new Vector2Int(0, 5), new Vector2Int(1, 5),
                        new Vector2Int(0, 6), 
                    }, 
                    new Color(0.5f, 0f, 1f), // 紫色
                    new Vector2(3.5f, 3.0f), // 設置中心點在形狀的大致中心
                    "(A_2)" //巨斧
                );
                break;

            case tetr_database.Tetr_type.F_2:
                tetrisInventoryManager.CreateTetrisPiece(
                    new Vector2Int[] {
                        new Vector2Int(1, 0), new Vector2Int(2, 0), new Vector2Int(3, 0), new Vector2Int(4, 0),
                        new Vector2Int(0, 1), new Vector2Int(1, 1), new Vector2Int(2, 1), new Vector2Int(3, 1), new Vector2Int(4, 1), new Vector2Int(5, 1),
                        new Vector2Int(0, 2), new Vector2Int(5, 2),
                        new Vector2Int(0, 3), new Vector2Int(1, 3),new Vector2Int(2, 3),new Vector2Int(3, 3),new Vector2Int(4, 3),new Vector2Int(5, 3),
                        new Vector2Int(1, 4), new Vector2Int(2, 4),new Vector2Int(3, 4), new Vector2Int(4, 4)
                    }, 
                    new Color(0.5f, 0f, 1f), // 紫色
                    new Vector2(3,3), // 設置中心點在形狀的幾何中心
                    "(F_2)", //單顆大火球
                    false // 設置為不可旋轉
                );
                break;

            case tetr_database.Tetr_type.B_1:
                tetrisInventoryManager.CreateTetrisPiece(
                    new Vector2Int[] {
                        new Vector2Int(2, 0), new Vector2Int(3, 0),
                        new Vector2Int(2, 1), new Vector2Int(3, 1), 
                        new Vector2Int(2, 2), new Vector2Int(3, 2),
                        new Vector2Int(2, 3), new Vector2Int(3, 3),
                        new Vector2Int(2, 4), new Vector2Int(3, 4),
                        new Vector2Int(2, 5), new Vector2Int(3, 5),

                        new Vector2Int(0, 3), new Vector2Int(1, 4),
                        new Vector2Int(4, 4), new Vector2Int(5, 3),
                        
                    },
                    new Color(0.5f, 0f, 1f), // 紫色
                    new Vector2(1f, 1f), // 設置中心點在形狀的幾何中心
                    "(T_1)", //西洋劍全場下戳
                    false //不旋轉
                );
                break;

            case tetr_database.Tetr_type.Pink_1:
                tetrisInventoryManager.CreateTetrisPiece(
                    new Vector2Int[] {
                        new Vector2Int(0, 0),
                        new Vector2Int(0, 1),
                        new Vector2Int(0, 2),
                        new Vector2Int(0, 3),
                        new Vector2Int(0, 4),
                        new Vector2Int(0, 5),
                        new Vector2Int(0, 6),
                        new Vector2Int(0, 7)
                    },
                    new Color(1f, 0.7f, 0.85f), // 粉色
                    new Vector2(1, 1), // 設置中心點在形狀的幾何中心
                    "Pink_1",//粉色組件1
                    false  //不旋轉
                );
                break;

            case tetr_database.Tetr_type.Pink_2:
                tetrisInventoryManager.CreateTetrisPiece(
                    new Vector2Int[] {
                        new Vector2Int(0, 0), new Vector2Int(1, 0),new Vector2Int(2, 0), new Vector2Int(3, 0),
                        new Vector2Int(1, 1), new Vector2Int(2, 1),
                    },
                    new Color(1f, 0.7f, 0.85f), // 粉色
                    new Vector2(1, 1), // 設置中心點在形狀的幾何中心
                    "Pink_2" //粉色組件2
                );
                break;

            case tetr_database.Tetr_type.Pink_3:
                tetrisInventoryManager.CreateTetrisPiece(
                    new Vector2Int[] {
                        new Vector2Int(0, 0), new Vector2Int(1, 0),new Vector2Int(2, 0), 
                        new Vector2Int(0, 1), new Vector2Int(2, 1),
                        new Vector2Int(0, 2), new Vector2Int(1, 2),new Vector2Int(2, 2),
                    },
                    new Color(1f, 0.7f, 0.85f), // 粉色
                    new Vector2(1, 1), // 設置中心點在形狀的幾何中心
                    "Pink_3",//粉色組件3
                    false  //不旋轉
                );
                break;

            case tetr_database.Tetr_type.Pink_4:
                tetrisInventoryManager.CreateTetrisPiece(
                    new Vector2Int[] {
                        new Vector2Int(0, 0),
                        new Vector2Int(0, 1),
                        new Vector2Int(0, 2),
                        new Vector2Int(1, 0),
                        new Vector2Int(1, 1),
                        new Vector2Int(1, 2),   
                        new Vector2Int(2,0),new Vector2Int(3, 0),

                    },
                    new Color(1f, 0.7f, 0.85f), // 粉色
                    new Vector2(1, 1), // 設置中心點在形狀的幾何中心
                    "Pink_4"//粉色組件4
                );
                break;

            case tetr_database.Tetr_type.Pink_5:
                tetrisInventoryManager.CreateTetrisPiece(
                    new Vector2Int[] {
                        new Vector2Int(0, 0),
                        new Vector2Int(0, 1),
                        new Vector2Int(0, 2),
                        new Vector2Int(0, 3),
                        new Vector2Int(0, 4),
                        new Vector2Int(0, 5),
                        new Vector2Int(0, 6),
                        new Vector2Int(0, 7)
                    },
                    new Color(1f, 0.7f, 0.85f), // 粉色
                    new Vector2(1, 1), // 設置中心點在形狀的幾何中心
                    "Pink_5"//粉色組件5
                );
                break;

            case tetr_database.Tetr_type.Pink_6:
                tetrisInventoryManager.CreateTetrisPiece(
                    new Vector2Int[] {
                        new Vector2Int(2, 0),
                        new Vector2Int(1, 1),new Vector2Int(3, 1),
                        new Vector2Int(0, 2),new Vector2Int(2, 2),new Vector2Int(4, 2),
                        new Vector2Int(0, 3),new Vector2Int(2, 3),new Vector2Int(4, 3),
                        new Vector2Int(1, 4),new Vector2Int(3, 4),
                        new Vector2Int(2, 5),   
                    },
                    new Color(1f, 0.7f, 0.85f), // 粉色
                    new Vector2(3, 2), // 設置中心點在形狀的幾何中心
                    "Pink_6"//粉色組件6
                );
                break;

            case tetr_database.Tetr_type.Pink_7:
                tetrisInventoryManager.CreateTetrisPiece(
                    new Vector2Int[] {
                        new Vector2Int(1, 0),
                        new Vector2Int(0, 1),new Vector2Int(0, 2),
                        new Vector2Int(2, 1),new Vector2Int(2, 2),
                        new Vector2Int(1, 3)
                    },
                    new Color(1f, 0.7f, 0.85f), // 粉色
                    new Vector2(3, 2), // 設置中心點在形狀的幾何中心
                    "Pink_7"//粉色組件7
                );
                break;

            case tetr_database.Tetr_type.Pink_8:
                tetrisInventoryManager.CreateTetrisPiece(
                    new Vector2Int[] {
                        new Vector2Int(0, 0),
                        new Vector2Int(1, 0),new Vector2Int(0, 2),
                        new Vector2Int(2, 0),new Vector2Int(3, 2),
                        new Vector2Int(3, 0)
                    },
                    new Color(1f, 0.7f, 0.85f), // 粉色
                    new Vector2(3, 2), // 設置中心點在形狀的幾何中心
                    "Pink_8"//粉色組件8
                );
                break;

            case tetr_database.Tetr_type.Green_1:
                tetrisInventoryManager.CreateTetrisPiece(
                    new Vector2Int[] {
                        new Vector2Int(0, 0),
                        new Vector2Int(0, 1),
                        new Vector2Int(0, 2),new Vector2Int(1, 2),new Vector2Int(2, 2),
                        new Vector2Int(0, 3),new Vector2Int(1, 3),new Vector2Int(2, 3)
                    },
                    new Color(0.5f, 1f, 0.5f), // 亮綠色
                    new Vector2(2, 2), // 設置中心點在形狀的幾何中心
                    "Green_1"//綠色組件1
                );
                break;

            case tetr_database.Tetr_type.Green_2:
                tetrisInventoryManager.CreateTetrisPiece(
                    new Vector2Int[] {
                        new Vector2Int(0, 0),
                        new Vector2Int(0, 1),
                        new Vector2Int(0, 2),new Vector2Int(1, 2),new Vector2Int(2, 2),
                        new Vector2Int(0, 3),new Vector2Int(1, 3),new Vector2Int(2, 3)
                    },
                    new Color(0.5f, 1f, 0.5f), // 亮綠色
                    new Vector2(2, 2), // 設置中心點在形狀的幾何中心
                    "Green_2"//綠色組件2
                );
                break;

            case tetr_database.Tetr_type.Green_3:
                tetrisInventoryManager.CreateTetrisPiece(
                    new Vector2Int[] {
                        new Vector2Int(0, 0),new Vector2Int(1, 0),new Vector2Int(2, 0),
                        new Vector2Int(0, 1),new Vector2Int(1, 1),new Vector2Int(2, 1),
                        new Vector2Int(2, 2),
                        new Vector2Int(2, 3)
                    },
                    new Color(0.5f, 1f, 0.5f), // 亮綠色
                    new Vector2(0, 0), // 設置中心點在形狀的幾何中心
                    "Green_3"//綠色組件3
                );
                break;

            case tetr_database.Tetr_type.Green_4:
                tetrisInventoryManager.CreateTetrisPiece(
                    new Vector2Int[] {
                        new Vector2Int(0, 0),new Vector2Int(1, 0),new Vector2Int(2, 0),new Vector2Int(3, 0),new Vector2Int(4, 0),new Vector2Int(5, 0),
                        new Vector2Int(2, 1),new Vector2Int(3, 1),new Vector2Int(4, 1),new Vector2Int(5, 1),
                        new Vector2Int(2, 2),new Vector2Int(3, 2),new Vector2Int(4, 2),new Vector2Int(5, 2),
                        new Vector2Int(2, 3),new Vector2Int(3, 3),new Vector2Int(4, 3),new Vector2Int(5, 3),
                    },
                    new Color(0.5f, 1f, 0.5f), // 亮綠色
                    new Vector2(0, 0), // 設置中心點在形狀的幾何中心
                    "Green_4"//綠色組件4
                );
                break;

            case tetr_database.Tetr_type.Purple_1:
                tetrisInventoryManager.CreateTetrisPiece(
                    new Vector2Int[] {
                        new Vector2Int(1, 0), new Vector2Int(2, 0),
                        new Vector2Int(0, 1), new Vector2Int(1, 1), new Vector2Int(2, 1), new Vector2Int(3, 1),
                        new Vector2Int(0, 2), new Vector2Int(1, 2), new Vector2Int(2, 2), new Vector2Int(3, 2),
                        new Vector2Int(1, 3), new Vector2Int(2, 3)
                    },
                    new Color(0.5f, 0f, 1f), // 紫色
                    new Vector2(1f, 1f), // 設置中心點在形狀的幾何中心
                    "(Purple_1)", //紫色組件1
                    false //不旋轉
                );
                break;
        }
    }
}
