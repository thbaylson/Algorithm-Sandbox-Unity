using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    // Slightly overkill, but this will make it easier to update if scene names or the build index change
    public enum Scenes
    {
        MainMenu,
        ObjectPoolExample,
        ColorPickerUIExample,
        SphereDanceExample,
        DOTS_Example
    }

    public void LoadObjectPoolScene()
    {
        SceneManager.LoadScene((int)Scenes.ObjectPoolExample);
    }

    public void LoadDancingSpheresScene()
    {
        SceneManager.LoadScene((int)Scenes.SphereDanceExample);
    }

    public void LoadColorPickerScene()
    {
        SceneManager.LoadScene((int)Scenes.ColorPickerUIExample);
    }

    public void LoadDOTSExampleScene()
    {
        SceneManager.LoadScene((int)Scenes.DOTS_Example);
    }
}
