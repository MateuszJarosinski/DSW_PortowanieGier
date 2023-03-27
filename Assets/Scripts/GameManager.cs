using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

public class GameManager : MonoBehaviour
{
    private void Start()
    {
        SteamUserStats.SetAchievement("Welcome!");
    }
}
