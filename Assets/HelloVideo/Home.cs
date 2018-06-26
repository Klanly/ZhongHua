using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Home : MonoBehaviour {

    public GameObject go;
    public GameObject goFull;
    string str_;
	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {
	}

	static HelloUnityVideo app = null;

	private void onJoinButtonClicked() {
        // get parameters (channel name, channel profile, etc.)
        //GameObject go = GameObject.Find ("ChannelName");
        //InputField field = go.GetComponent<InputField>();
       
       
        // create app if nonexistent
        go.SetActive(true);
		if (ReferenceEquals (app, null)) {
			app = new HelloUnityVideo (); // create app
			app.loadEngine (); // load engine
		}

		// join channel and jump to next scene
		app.join (LoginInfo.Instance().mylogindata.nearURL);
        str_ = LoginInfo.Instance().mylogindata.nearURL;
		//SceneManager.sceneLoaded += OnLevelFinishedLoading; // configure GameObject after scene is loaded
		//SceneManager.LoadScene ("SceneHelloVideo", LoadSceneMode.Single);
	}

	private void onLeaveButtonClicked() {
		if (!ReferenceEquals (app, null)) {
			app.leave (); // leave channel
           
            app.unloadEngine (); // delete engine
			app = null; // delete app
			//SceneManager.LoadScene ("SceneHome", LoadSceneMode.Single);
		}
	}

	public void onButtonJoin() {
        // which GameObject?

        onJoinButtonClicked();

       
		
		
	}
    public void OnButtonLeave()
    {
        onLeaveButtonClicked();
        //go.SetActive(false);
        //goFull.SetActive(false);
    }

    public void QuiteVideo()
    {
        onLeaveButtonClicked();
        LoginInfo.Instance().mylogindata.isFull = true;
        go.SetActive(false);
        goFull.SetActive(false);
    }

    public void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode) {
		if (scene.name.CompareTo("SceneHelloVideo") == 0) {
			if (!ReferenceEquals (app, null)) {
				app.onSceneHelloVideoLoaded (); // call this after scene is loaded
			}
			SceneManager.sceneLoaded -= OnLevelFinishedLoading;
		}
	}
    public void OnChangeFar()
    {
        //app.mRtcEngine.MuteAllRemoteAudioStreams(true);
        //app.mRtcEngine.MuteAllRemoteAudioStreams(true);
        onLeaveButtonClicked();
        //Destroy(LoginInfo.Instance().mylogindata.forDel);
        go.SetActive(false);
        go.SetActive(true);
        
        if (ReferenceEquals(app, null))
        {
            app = new HelloUnityVideo(); // create app
            app.loadEngine(); // load engine
        }
        app.join(LoginInfo.Instance().mylogindata.farURL);
        str_ = LoginInfo.Instance().mylogindata.farURL;
        //app.mRtcEngine.MuteAllRemoteAudioStreams(false);

    }
    public void OnChangeNear()
    {
        //app.mRtcEngine.MuteAllRemoteAudioStreams(true);
        onLeaveButtonClicked();
        //Destroy(LoginInfo.Instance().mylogindata.forDel);
        go.SetActive(false);
        go.SetActive(true);
        if (ReferenceEquals(app, null))
        {
            app = new HelloUnityVideo(); // create app
            app.loadEngine(); // load engine
        }
        
        app.join(LoginInfo.Instance().mylogindata.nearURL);
        str_ = LoginInfo.Instance().mylogindata.nearURL;
        //app.mRtcEngine.MuteAllRemoteAudioStreams(false);
    }

    bool isFull;
    public void OnChangeFull()
    {
        //if (!isFull)
        //{
        //    //全屏未激活  点击激活
        //    isFull = true;
        //    onLeaveButtonClicked();
        //    go.SetActive(false);
        //    goFull.SetActive(true);
        //    LoginInfo.Instance().mylogindata.isFull = false;
        //    if (ReferenceEquals(app, null))
        //    {
        //        app = new HelloUnityVideo(); // create app
        //        app.loadEngine(); // load engine
        //    }

        //    app.join(LoginInfo.Instance().mylogindata.fullURL);
        //    str_ = LoginInfo.Instance().mylogindata.fullURL;
        //} else
        //{
        //    //全屏已激活  点击关闭
        //    isFull = false;
        //    onLeaveButtonClicked();
        //    go.SetActive(true);
        //    goFull.SetActive(false);
        //    LoginInfo.Instance().mylogindata.isFull = true;
        //    if (ReferenceEquals(app, null))
        //    {
        //        app = new HelloUnityVideo(); // create app
        //        app.loadEngine(); // load engine
        //    }

        //    app.join(LoginInfo.Instance().mylogindata.fullURL);
        //    str_ = LoginInfo.Instance().mylogindata.fullURL;
        //}



        if (!isFull)
        {
            isFull = true;
            //app.mRtcEngine.MuteAllRemoteAudioStreams(true);
            onLeaveButtonClicked();
            //Destroy(LoginInfo.Instance().mylogindata.forDel);
            go.SetActive(false);
            goFull.SetActive(true);
            LoginInfo.Instance().mylogindata.isFull = false;
            if (ReferenceEquals(app, null))
            {
                app = new HelloUnityVideo(); // create app
                app.loadEngine(); // load engine
            }
            app.join(str_);
            //app.mRtcEngine.MuteAllRemoteAudioStreams(false);

        }
        else
        {
            isFull = false;
            //app.mRtcEngine.MuteAllRemoteAudioStreams(true);
            onLeaveButtonClicked();
            //Destroy(LoginInfo.Instance().mylogindata.forDel);
            goFull.SetActive(false);
            go.SetActive(true);
            LoginInfo.Instance().mylogindata.isFull = true;
            if (ReferenceEquals(app, null))
            {
                app = new HelloUnityVideo(); // create app
                app.loadEngine(); // load engine
            }
            app.join(str_);
            //app.mRtcEngine.MuteAllRemoteAudioStreams(false);
        }
    }

}
