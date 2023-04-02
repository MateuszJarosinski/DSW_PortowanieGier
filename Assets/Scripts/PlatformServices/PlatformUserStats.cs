using UnityEngine;

namespace PlatformServices
{
    public class PlatformUserStats : MonoBehaviour
    {
        private static IPlatformUserStats _platform;

        [RuntimeInitializeOnLoadMethod]
        public static void Initialize()
        {
            Debug.Log("Init");
#if !DISABLESTEAMWORKS
            _platform = new SteamUserStats();
            return;
#endif

#if UNITY_ANDROID
            _platform = new AndroidUserStats();
            return;
#endif
        }

        public static bool SetAchievement(string achievementID)
        {
            Debug.Log($"Achievement: {achievementID}");
            return _platform.SetAchievement(achievementID);
        }
    }

    public interface IPlatformUserStats
    {
        public bool SetAchievement(string achievementID);
    }
    
#if !DISABLESTEAMWORKS
    public class SteamUserStats : IPlatformUserStats
    {
        public bool SetAchievement(string achievementID)
        {
            return Steamworks.SteamUserStats.SetAchievement(achievementID);
        }
    }
#endif
    
#if UNITY_ANDROID
    public class AndroidUserStats : IPlatformUserStats
    {
        public bool SetAchievement(string achievementID)
        {
            Social.ReportProgress(achievementID, 100f, null);
            return true;
        }
    }
#endif
}
