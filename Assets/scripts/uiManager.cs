using UnityEngine;
using UnityEngine.UI;

public class uiManager : MonoBehaviour
{
    [Header("buttons")]
    public Button start;
    public Button maneMenu;

    private void Start()
    {
        if (maneMenu != null) maneMenu.onClick.AddListener(() => FEEL.gotoScene(0, this)); 
        if (start != null) start.onClick.AddListener(() => FEEL.gotoScene(1, this));
    }
}
