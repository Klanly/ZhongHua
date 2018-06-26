using UnityEngine;
using UnityEngine.UI;

public class DGame_ChuJiangJiLu : MonoBehaviour
{
    public Text[] Record_awards;


    private void Update()
    {
        for (int i = 0; i < 4; i++)
        {
            Record_awards[i].text = DGameData.Instance.record_awards[i];
        }
    }

}
