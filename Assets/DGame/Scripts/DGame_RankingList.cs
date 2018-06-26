
using UnityEngine;
using UnityEngine.UI;

public class DGame_RankingList : MonoBehaviour
{
    private GameObject[] list = new GameObject[100];
    private Text[] NumText = new Text[100];
    private Image[] HeadImage = new Image[100];
    private Text[] NameText = new Text[100];
    private Text[] TitleText = new Text[100];
    private Text[] TypeText = new Text[100];
    private Text[] MoneyText = new Text[100];

    string Address = "Scroll View/Viewport/Content/1 (i)";

    private void Awake()
    {
        for (int i = 1; i < 101; i++)
        {
            Address = "Scroll View/Viewport/Content/1 (" + i + ")";
            list[i - 1] = GameObject.Find(Address);
            Address = "Scroll View/Viewport/Content/1 (" + i + ")/Head";
            HeadImage[i - 1] = GameObject.Find(Address).GetComponent<Image>();
            Address = "Scroll View/Viewport/Content/1 (" + i + ")/Name";
            NameText[i - 1] = GameObject.Find(Address).GetComponent<Text>();
            Address = "Scroll View/Viewport/Content/1 (" + i + ")/title";
            TitleText[i - 1] = GameObject.Find(Address).GetComponent<Text>();
            Address = "Scroll View/Viewport/Content/1 (" + i + ")/type";
            TypeText[i - 1] = GameObject.Find(Address).GetComponent<Text>();
            Address = "Scroll View/Viewport/Content/1 (" + i + ")/money";
            MoneyText[i - 1] = GameObject.Find(Address).GetComponent<Text>();
            Address = "Scroll View/Viewport/Content/1 (" + i + ")/Num";
            NumText[i - 1] = GameObject.Find(Address).GetComponent<Text>();
        }
        for (int i = 1; i < 100; i++)
        {
            if (i < 10)
            {
                NumText[i - 1].text = "0" + i;
            }
            else
            {
                NumText[i - 1].text = i.ToString();
            }
        }
    }

    private void Update()
    {
        for (int i = 0; i < 100; i++)
        {
            if (i < DGameData.Instance.award_num)
            {
                list[i].SetActive(true);
                NameText[i].text = DGameData.Instance.award_username[i];
                TitleText[i].text = DGameData.Instance.award_title[i];
                TypeText[i].text = DGameData.Instance.award_type[i];
                MoneyText[i].text = DGameData.Instance.award_money[i];
            }
            else
            {
                list[i].SetActive(false);
            }
        }
    }
}
