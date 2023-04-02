using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoogleManager : MonoBehaviour
{
#if UNITY_ANDROID
    private void Awake()
    {
        GooglePlayGames.PlayGamesPlatform.Activate();
        Social.localUser.Authenticate(status => Debug.Log($"Authenticate status : {status}"));
    }
#endif
}
