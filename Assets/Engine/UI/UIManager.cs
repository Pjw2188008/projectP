using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    /*
     * UI는 열리는 순서가 빠른게 아래로 쌓이고 높은 순서가 먼저 닫힌다.
     */

    [ReadOnly, SerializeField]
    private int baseUI_index; // 제거 되지 않는 기본이 되는 UI들의 마지막 인덱스

    public List<GameObject> ui_list;

    private void Start()
    {
        baseUI_index = ui_list.Count - 1;
    }

    public void popUI()
    {
        int index = ui_list.Count - 1;

        if (index <= baseUI_index) return;

        ui_list[index].SetActive(false);
        ui_list.RemoveAt(index);
    }

    public void stackUI(GameObject ui)
    {
        ui_list.Add(ui);
        ui.SetActive(true);
    }
}
