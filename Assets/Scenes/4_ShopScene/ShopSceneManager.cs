using UnityEngine;

public class ShopSceneManager: MonoBehaviour
{
    [SerializeField]
    private SceneFader sceneFader;

    public void FieldScene()
    {
        sceneFader.SceneChange("FieldScene");
    }  
}
