using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;
using System.Threading.Tasks; // UniTask için gerekli

namespace Scene.MainMenu
{
    public class SceneController : MonoBehaviour
    {
        private void OnEnable()
        {
            GameEventHandler.OnClickStartButton += (sceneId) => HandleSwitchSceneAsync(sceneId).Forget();;
        }

        void OnDisable()
        {
            GameEventHandler.OnClickStartButton -= (sceneId) => HandleSwitchSceneAsync(sceneId).Forget();
        }

        private async UniTask HandleSwitchSceneAsync(int sceneId)
        {
            if (sceneId < 0 || sceneId >= UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings)
            {
                Debug.LogWarning("Undefined scene ID!!");
                return;
            }

            var loadOperation = SceneManager.LoadSceneAsync(sceneId);
            loadOperation.allowSceneActivation = false;

            while (!loadOperation.isDone)
            {
                Debug.Log(loadOperation.progress * 100);

                
                if (loadOperation.progress >= 0.9f)
                {
                    await UniTask.Delay(500);
                    loadOperation.allowSceneActivation = true;
                }

                await UniTask.Yield(); 
            }
        }
    }
}
