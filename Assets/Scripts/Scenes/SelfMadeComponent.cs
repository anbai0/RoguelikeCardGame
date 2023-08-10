using UnityEngine;
using UnityEngine.SceneManagement;

namespace SelfMadeNamespace
{
    public static class SelfMadeComponent
    {
        /// <summary>
        /// �w�肵���V�[���̎w�肵���R���|�[�l���g���擾���擾���܂��B
        /// <code>"SceneName".GetComponentInScene&lt;T&gt;();</code>
        /// </summary>
        /// <typeparam name="T">�擾�������R���|�[�l���g�̖��O</typeparam>
        /// <param name="sceneName"></param>
        /// <returns></returns>
        public static T GetComponentInScene<T>(this string sceneName) where T : Component
        {
            Scene scene = SceneManager.GetSceneByName(sceneName);

            if (!scene.isLoaded)
            {
                Debug.LogError("�w�肵���V�[�������[�h����Ă��܂���B");
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

            Debug.LogError("�w�肵���R���|�[�l���g���擾�ł��܂���ł����B");
            return null;
        }


        /// <summary>
        /// �w�肵���V�[���̕\����؂�ւ��܂��B
        /// <code>"SceneName".ToggleSceneDisplay(true or false);</code>
        /// </summary>
        /// <param name="sceneName"></param>
        /// <param name="isSceneActive"></param>
        public static void ToggleSceneDisplay(this string sceneName, bool isSceneActive)
        {
            // �w�肵�����O�̃V�[�����擾
            Scene sceneToHide = SceneManager.GetSceneByName(sceneName);

            // �V�[�����L���ŁA���[�h����Ă���ꍇ�ɏ��������s
            if (sceneToHide.IsValid() && sceneToHide.isLoaded)
            {
                // �V�[�����̂��ׂẴ��[�g�Q�[���I�u�W�F�N�g���擾
                GameObject[] rootObjects = sceneToHide.GetRootGameObjects();

                // �V�[�����̂��ׂẴ��[�g�Q�[���I�u�W�F�N�g�ɑ΂��ď��������s
                foreach (GameObject obj in rootObjects)
                {
                    // �Q�[���I�u�W�F�N�g���\���ɂ���
                    obj.SetActive(isSceneActive);
                }
            }
        }
    }
}

