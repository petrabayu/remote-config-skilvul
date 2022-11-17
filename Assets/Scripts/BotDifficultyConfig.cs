using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.RemoteConfig;
using System;

public class BotDifficultyConfig : MonoBehaviour
{
    [SerializeField] Bot bot;
    [SerializeField] int selectedDifficulty;

    [Header("Remote Config Parameters: ")]
    [SerializeField] BotStats[] botDifficulties;
    [SerializeField] bool enableRemoteConfig = false;
    [SerializeField] string difficultyKey = "Difficulty";


    struct userAttributes { };
    struct appAttributes { };
    IEnumerator Start()
    {
        //tunggu bot selesai set up
        yield return new WaitUntil(() => bot.IsReady);

        // set stats default dari diffucluty manager
        // sesuai dengan selected difficulty dari inspector
        var newStats = botDifficulties[selectedDifficulty];
        bot.SetStats(newStats, true);

        // ambil difficulty dari remote config bila enabled
        if (enableRemoteConfig == false)
            yield break;
        //menuggu hingga unity service siap
        yield return new WaitUntil(() => UnityServices.State == ServicesInitializationState.Initialized &&
        AuthenticationService.Instance.IsSignedIn
        );

        //daftar dulu untuk event fetch completed
        RemoteConfigService.Instance.FetchCompleted += OnRemoteConfigFetched;

        //lalu fetch disini,cukup sekali diawal permainan
        RemoteConfigService.Instance.FetchConfigs(new userAttributes(), new appAttributes());
    }

    private void OnDestroy()
    {
        //jangan lupa untuk unregister event untuk menghindari memeroy leak
        RemoteConfigService.Instance.FetchCompleted -= OnRemoteConfigFetched;
    }
    //setiap kali data baru didapatkan melalui fetch fungsi ini akan dipanggi;
    private void OnRemoteConfigFetched(ConfigResponse response)
    {
        if (RemoteConfigService.Instance.appConfig.HasKey(difficultyKey) == false)
        {
            Debug.LogWarning($"Difficulty key : {difficultyKey} not found on remote config server");
            return;
        }

        switch (response.requestOrigin)
        {
            case ConfigOrigin.Default:
            case ConfigOrigin.Cached:
                break;
            case ConfigOrigin.Remote:
                selectedDifficulty = RemoteConfigService.Instance.appConfig.GetInt(difficultyKey);
                selectedDifficulty = Mathf.Clamp(selectedDifficulty, 0, botDifficulties.Length - 1);

                var newStats = botDifficulties[selectedDifficulty];
                bot.SetStats(newStats, true);
                break;
        }
    }
}
