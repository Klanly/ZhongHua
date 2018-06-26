using UnityEngine;
using UnityEngine.UI;

public class DGame_BiBeiJiLu : MonoBehaviour
{
    private Image[,] doubly_sign = new Image[15, 15];
    public Sprite[] BiBeiSprite;
    private Text[,] doubly_money = new Text[15, 15];
    private Image[,] doubly_guessin_object = new Image[15, 15];
    public Sprite[] Big_Small;
    private Image[,] doubly_result = new Image[15, 15];
    public Sprite[] CardSprite;
    private GameObject[,] obj = new GameObject[15, 15];

    string Address = "Scroll View/Viewport/Content/1 (i)/Scroll View/Viewport/Content/1 (i)";


    private int num = 15;//页数
    public Text PageNum;

    public RectTransform posx;
    private float x = 0;
    float time;

    private AudioSource Audio;

    private void Awake()
    {
        for (int i = 1; i < 16; i++)
        {
            for (int e = 1; e < 16; e++)
            {
                try
                {
                    Address = "Scroll View/Viewport/Content/1 (" + i + ")/Scroll View/Viewport/Content/1 (" + i + ")/BIBEI";
                    doubly_sign[(i - 1), (e - 1)] = GameObject.Find(Address).GetComponent<Image>();
                }
                catch
                {
                    Debug.Log("寻找比倍图片错误");
                }
                try
                {
                    Address = "Scroll View/Viewport/Content/1 (" + i + ")/Scroll View/Viewport/Content/1 (" + i + ")/Text";
                    doubly_money[(i - 1), (e - 1)] = GameObject.Find(Address).GetComponent<Text>();
                }
                catch
                {
                    Debug.Log("寻找比倍文字错误");
                }
                try
                {
                    Address = "Scroll View/Viewport/Content/1 (" + i + ")/Scroll View/Viewport/Content/1 (" + i + ")/BIG@SMALL";
                    doubly_guessin_object[(i - 1), (e - 1)] = GameObject.Find(Address).GetComponent<Image>();
                }
                catch
                {
                    Debug.Log("寻找比倍大小错误");
                }
                try
                {
                    Address = "Scroll View/Viewport/Content/1 (" + i + ")/Scroll View/Viewport/Content/1 (" + i + ")/Card";
                    doubly_result[(i - 1), (e - 1)] = GameObject.Find(Address).GetComponent<Image>();
                }
                catch
                {
                    Debug.Log("寻找比倍结果错误");
                }
                try
                {
                    Address = "Scroll View/Viewport/Content/1 (" + i + ")/Scroll View/Viewport/Content/1 (" + i + ")";
                    obj[(i - 1), (e - 1)] = GameObject.Find(Address);
                }
                catch
                {
                    Debug.Log("寻找比倍次数列表错误");
                }


            }
        }
    }


    private void Update()
    {
        time += Time.deltaTime;
        x = Mathf.Lerp(x, (num - 1) * -800, time);
        posx.anchoredPosition = new Vector2(x, 0);

        OnUI();
    }

    void OnUI()
    {
        PageNum.text = (16 - num).ToString(); //(16 - num).ToString();DGameData.Instance.Model_List.Count.ToString()

        for (int i = 0; i < DGameData.Instance.Model_List.Count; i++)
        {
            for (int e = 0; e < DGameData.Instance.Model_List[i].newList.Count; e++)
            {
                obj[15 - i - 1, e].SetActive(true);
                switch (DGameData.Instance.Model_List[i].newList[e].doubly_sign)
                {
                    case "一倍":
                        doubly_sign[15 - i - 1, e].sprite = BiBeiSprite[0];
                        break;
                    case "半倍":
                        doubly_sign[15 - i - 1, e].sprite = BiBeiSprite[2];
                        break;
                    default:
                        //双倍
                        doubly_sign[15 - i - 1, e].sprite = BiBeiSprite[1];
                        break;
                }
                doubly_money[15 - i - 1, e].text = ((int)(float.Parse(DGameData.Instance.Model_List[i].newList[e].money.ToString()))).ToString();
                switch (DGameData.Instance.Model_List[i].newList[e].doubly_guessing_object)
                {
                    case "小":
                        doubly_guessin_object[15 - i - 1, e].sprite = Big_Small[1];
                        break;
                    case "大":
                        doubly_guessin_object[15 - i - 1, e].sprite = Big_Small[0];
                        break;
                    default:
                        doubly_guessin_object[15 - i - 1, e].sprite = Big_Small[0];
                        break;
                }
                doubly_result[15 - i - 1, e].sprite = CardSprite[Card_Num(DGameData.Instance.Model_List[i].newList[e].doubly_result)];
            }
        }
    }


    int Card_Num(string data)
    {
        int a = 0;
        try
        {
            if (data == "")
                return a = num + 9;
            char[] s = { 'A', 'B', 'C', 'D', 'E' };
            string[] b = data.Split(s);
            //正常类型的牌
            switch (data[0])
            {
                case 'A':
                    //黑桃              
                    a = int.Parse(b[1]) - 1;
                    break;
                case 'B':
                    //红桃
                    a = int.Parse(b[1]) + 12;
                    break;
                case 'C':
                    a = int.Parse(b[1]) + 25;
                    //梅花
                    break;
                case 'D':
                    //方块
                    a = int.Parse(b[1]) + 38;
                    break;
                case 'E':
                    //鬼牌
                    a = int.Parse(b[1]) + 51;
                    break;
                default:
                    Debug.Log("解析牌类错误，不属于ABCDE类牌");
                    a = 52;
                    break;
            }
            if (a >= 55)
                a = 52;
            return a;
        }
        catch
        {
            return a = num + 9;
        }
    }


    public void PuKe_LetfButton()
    {
        Audio.Play();
        if (num <= 1)
            return;
        if (num <= 16 - DGameData.Instance.BiBeiDataNum)
            return;
        time = 0;
        num--;
    }

    public void PuKe_RightButton()
    {
        Audio.Play();
        if (num >= 15)
            return;
        time = 0;
        num++;
    }
}
