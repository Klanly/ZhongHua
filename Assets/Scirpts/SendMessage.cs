using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[SerializeField]
public class OnBetDown
{
    public string type;
    public string user_id;
    public string room_id;
    public string coinDown;
    public string pokerCode;

    public OnBetDown(string type,string user_id, string room_id ,string coinDown, string pokerCode)
    {
        this.type = type;
        this.user_id = user_id;
        this.room_id = room_id;
        this.coinDown = coinDown;
        this.pokerCode = pokerCode;
    }
	
}

[SerializeField]
public class OnLogin
{
    public string type;
    public string user_id;
    public string room_id;
    public string game_id;

    public OnLogin(string type,string user_id,string room_id,string game_id)
    {
        this.type = type;
        this.user_id = user_id;
        this.room_id = room_id;
        this.game_id = game_id;
    }
}

[SerializeField]
public class OnPing
{
	public string type;
	public string user_id;
	public string room_id;
	public string game_id;

	public OnPing(string type,string user_id,string room_id,string game_id)
	{
		this.type = type;
		this.user_id = user_id;
		this.room_id = room_id;
		this.game_id = game_id;
	}
}

