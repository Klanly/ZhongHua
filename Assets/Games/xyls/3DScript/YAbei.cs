using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YAbei : MonoBehaviour {

    public int id;
    public string num;
    public string tp;
    public string rate;
    public string dnum;
    public void Yabei(int Id, string Tp, string Num, string Rate, string Dnum)
    {
        this.id = Id;
        this.num = Num;
        this.tp= Tp;
        this.rate = Rate;
        this.dnum= Dnum;
    }
       
}
