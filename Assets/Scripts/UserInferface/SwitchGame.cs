using UnityEngine;
using UnityEngine.SceneManagement;
public class SwitchGame : MonoBehaviour

{

    public void PlayNewScene(string sceneName)

    {

        SceneManager.LoadScene(sceneName);

    }

}
