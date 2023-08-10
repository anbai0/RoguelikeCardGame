using UnityEngine;
using UnityEngine.SceneManagement;

namespace SelfMadeNamespace
{
    public static class SelfMadeComponent
    {
        /// <summary>
        /// 指定したシーンの指定したコンポーネントを取得を取得します。
        /// <code>"SceneName".GetComponentInScene&lt;T&gt;();</code>
        /// </summary>
        /// <typeparam name="T">取得したいコンポーネントの名前</typeparam>
        /// <param name="sceneName"></param>
        /// <returns></returns>
        public static T GetComponentInScene<T>(this string sceneName) where T : Component
        {
            Scene scene = SceneManager.GetSceneByName(sceneName);

            if (!scene.isLoaded)
            {
                Debug.LogError("指定したシーンがロードされていません。");
                return null;
            }

            foreach (var rootGameObject in scene.GetRootGameObjects())
            {
                T component = rootGameObject.GetComponent<T>();
                if (component != null)
                {
                    return component;
                }
            }

            Debug.LogError("指定したコンポーネントを取得できませんでした。");
            return null;
        }


        /// <summary>
        /// 指定したシーンの表示を切り替えます。
        /// <code>"SceneName".ToggleSceneDisplay(true or false);</code>
        /// </summary>
        /// <param name="sceneName"></param>
        /// <param name="isSceneActive"></param>
        public static void ToggleSceneDisplay(this string sceneName, bool isSceneActive)
        {
            // 指定した名前のシーンを取得
            Scene sceneToHide = SceneManager.GetSceneByName(sceneName);

            // シーンが有効で、ロードされている場合に処理を実行
            if (sceneToHide.IsValid() && sceneToHide.isLoaded)
            {
                // シーン内のすべてのルートゲームオブジェクトを取得
                GameObject[] rootObjects = sceneToHide.GetRootGameObjects();

                // シーン内のすべてのルートゲームオブジェクトに対して処理を実行
                foreach (GameObject obj in rootObjects)
                {
                    // ゲームオブジェクトを非表示にする
                    obj.SetActive(isSceneActive);
                }
            }
        }
    }
}

