using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 功能：旋转
/// </summary>
public class ItemRotate : MonoBehaviour {

	public string str;

	public string winnings;

	public bool IsTurn;
	public bool IsSlowDown;
	public bool IsStop;
	public bool IsLightning;

	public static string Ludan;

	private float interval;			//间隔
	private float timer;
	private int lastpos;
	private int pos=0;
	private int count;

	private float Stoptime;
	private float Lightningtime;

    public bool IsNotFirst;

	private GameObject LastObject;

	void Start () {
		interval = 0.2f;
	}
	

	void Update () {
		if(IsTurn)
		{
			timer += Time.deltaTime;
			Stoptime += Time.deltaTime;
			if(timer>=interval)
			{
				timer = 0;

				if (LastObject != null) {
					LastObject.transform.GetChild(0).gameObject.SetActive (false);
				}
				this.transform.GetChild (pos).GetChild(0).gameObject.SetActive (true);
				LastObject = this.transform.GetChild (pos).gameObject;
				//	lastpos = pos;
				pos++;
				if(pos>=this.transform.childCount)
				{
					pos = 0;	//0		12		[0]
								//1	3	5	7	9	11	13	15	17	19	21	23[2]
								//2	4	6	8	10	14	16	18	20	22		[1]
				}
			}
			if(Stoptime>=5f)
			{
                //Debug.Log("位置："+(pos+18).ToString()+"路单"+Ludan);
				switch (pos+18) {
				case 42:
				case 34:
				case 40:
				case 38:
				case 18:
				case 20:
				case 22:
				case 26:
				case 28:
				case 30:
				case 32:
					//目标显示为【8】
					if(Ludan=="2")
					{
						IsTurn=false;
						IsSlowDown=true;
						Debug.Log ("减速开始时间"+Stoptime);
					}
					break;
				case 33:
				case 35:
				case 37:
				case 39:
				case 41:
				case 19:
				case 21:
				case 23:
				case 25:
				case 27:
				case 29:
				case 31:
					//目标显示为【2】
					if(Ludan=="0")
					{
						IsTurn=false;
						IsSlowDown=true;
						Debug.Log ("减速开始时间"+Stoptime);
					}
					break;
				case 24:
				case 36:
					//目标显示为【0】
					if(Ludan=="1")
					{
						IsTurn=false;
						IsSlowDown=true;
						Debug.Log ("减速开始时间"+Stoptime);
					}
					break;
				}
			}
		}
		if(IsSlowDown)
		{
			interval += 0.003f;
			if(interval>=0.45f)
			{
				Debug.Log ("减速停止时间"+Stoptime);
				IsSlowDown = false;
				IsStop = true;
				count = 0;
				Debug.Log ("CC"+CC);
				interval = 0.5f;
			}

			timer += Time.deltaTime;
			Stoptime += Time.deltaTime;
			if(timer>=interval)
			{
				timer = 0;

				if (LastObject != null) {
					LastObject.transform.GetChild(0).gameObject.SetActive (false);
				}
				this.transform.GetChild (pos).GetChild(0).gameObject.SetActive (true);
				LastObject = this.transform.GetChild (pos).gameObject;
				//	lastpos = pos;
				pos++;
				CC++;
				if(pos>=this.transform.childCount)
				{
					pos = 0;	
                    //0		12		[0]
					//1	3	5	7	9	11	13	15	17	19	21	23[2]
					//2	4	6	8	10	14	16	18	20	22		[8]
				}
			}
		}
		if(IsStop)
		{
			timer += Time.deltaTime;
			Stoptime += Time.deltaTime;
			if (timer >= interval) {
				timer = 0;

				if (LastObject != null) {
					LastObject.transform.GetChild (0).gameObject.SetActive (false);
				}
				this.transform.GetChild (pos).GetChild (0).gameObject.SetActive (true);
				LastObject = this.transform.GetChild (pos).gameObject;
				pos++;
				if (pos >= this.transform.childCount) {
					pos = 0;	//0		12		[0]
					//1	3	5	7	9	11	13	15	17	19	21	23[1]
					//2	4	6	8	10	14	16	18	20	22		[1]
				}
				count++;
				if(count>=0)
				{
					switch (pos-1) {
					case 0:
					case 12:
						if(Ludan=="2")
						{
							IsStop = false;
							pos -= 1;
							str+=pos+",";
							Debug.Log ("位置："+str);
							Debug.Log ("最终停止时间"+Stoptime);
							if(winnings!="")
							{
								Audiomanger._instenc.PlayElbWin(int.Parse(winnings));
							}

						}
						break;
					case 1:
					case 3:
					case 5:
					case 7:
					case 9:
					case 13:
					case 15:
					case 17:
					case 19:
					case 21:
					case 23:
						if(Ludan=="0")
						{
							IsStop = false;
							pos -= 1;
							str+=pos+",";
							Debug.Log ("位置："+str);
							Debug.Log ("最终停止时间"+Stoptime);
							if (winnings != "") {
								Audiomanger._instenc.PlayElbWin (int.Parse (winnings));
							}
						}
						break;
					case 2:
					case 4:
					case 6:
					case 8:
					case 10:
					case 14:
					case 16:
					case 18:
					case 20:
					case 22:
						if(Ludan=="1")
						{
							IsStop = false;
							pos -= 1;
							str+=pos+",";
							Debug.Log ("位置："+str);
							Debug.Log ("最终停止时间"+Stoptime);
							if (winnings != "") {
								Audiomanger._instenc.PlayElbWin (int.Parse (winnings));
							}
						}
						break;
					}
				}
			}
		}

		if(IsLightning)
		{
			Lightningtime += Time.deltaTime;
			if(Lightningtime>0.2f)
			{
				Lightningtime = 0;
				if (this.transform.GetChild (pos).GetChild (0).gameObject.activeSelf) {
					this.transform.GetChild (pos).GetChild(0).gameObject.SetActive (false);
				} else {
					this.transform.GetChild (pos).GetChild(0).gameObject.SetActive (true);
				}
			}
		}

	}

    public void RandomPos(string str)
    {
        //0		12		[0]
        //1	3	5	7	9	11	13	15	17	19	21	23[2]
        //2	4	6	8	10	14	16	18	20	22		[8]
        if (!IsNotFirst)
        {
            IsNotFirst = true;
            for (int i = 0; i < this.transform.childCount; i++)
            {
                this.transform.GetChild(i).GetChild(0).gameObject.SetActive(false);
            }
            switch (str)
            {
                case "0":
                    int[] two_js = new int[] {1,3,5,7,9,11,13,15,17,19,21,23};
                    pos = two_js[Random.Range(0, two_js.Length)];
                    break;
                case "1":
                    int[] eight_js = new int[] {2,4,6,8,10,14,16,18,20,22};
                    pos = eight_js[Random.Range(0, eight_js.Length)];
                    break;
                case "2":
                    int[] zero_js = new int[] { 0,12 };
                    pos = zero_js[Random.Range(0, zero_js.Length)];
                    break;
            }
        }
        

      
    }

    //重置
    public void ResetItem()
	{
		CC = 0;
		IsLightning = false;
		Stoptime = 0;
		timer = 0;
        for (int i = 0; i < this.transform.childCount; i++)
        {
            this.transform.GetChild(pos).GetChild(0).gameObject.SetActive(false);
        }
		this.transform.GetChild (pos).GetChild(0).gameObject.SetActive (true);
		
	}

    //停止转动
    public void StopTurn()
    {
        IsTurn = false;
        IsSlowDown = false;
        IsLightning = false;
    }

	public void SetOrigin()
	{
		for (int i = 0; i < this.transform.childCount; i++) {
			this.transform.GetChild (i).GetChild (0).gameObject.SetActive (false);
		}

		pos = Random.Range (0,24);

		Debug.Log ("随机起始点："+pos);
		this.transform.GetChild (pos).GetChild(0).gameObject.SetActive (true);
	}


	//设置时间间隔
	public float SetInterval
	{
		set
		{
			interval = value;
		}
	}

	public int CC;


}
