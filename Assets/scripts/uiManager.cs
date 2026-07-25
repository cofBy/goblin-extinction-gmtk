using UnityEngine;
using UnityEngine.UI;

public class uiManager : MonoBehaviour
{
    [Header("buttons")]
    public Button start;
    public Button maneMenu;

    private void Start()
    {
        maneMenu.onClick.AddListener(() => FEEL.gotoScene(0, this));
        start.onClick.AddListener(() => FEEL.gotoScene(1, this));
    }
}
