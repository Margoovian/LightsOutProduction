using UnityEngine;



public class EyeGestures : MonoBehaviour
{
    enum Gestures
    {
        Default = 0,
        Worried,
        Scared,
        Death
    }
    private Renderer characterRenderer;
    private Material material;
    public Vector2[] positions = new Vector2[4]
    { 
        new(0.0f,0.0f),
        new(0.5f,0.0f),
        new(0.0f,0.5f),
        new(0.5f,0.5f)
    };

    void ChangeEyes(Gestures eyeGesture)
    {
        Vector2 formula = Vector2.zero;


        switch (eyeGesture)
        {
            case Gestures.Death:
                formula = positions[1 * 2 + 1];

                break;
            case Gestures.Scared:
                formula = positions[1 * 2 + 0];

                break;
            case Gestures.Worried:
                formula = positions[0 * 2 + 1];

                break;

            case Gestures.Default:
                formula = positions[0];
                break;

            default:
                Debug.LogError("gesture not handled");

                break;
        }

        
        material.SetFloat("_HorizontalEye", formula.x);
        material.SetFloat("_VerticalEye", formula.y);
    }

    private void Start()
    {
        characterRenderer = GetComponent<Renderer>();
        var matArray = characterRenderer.materials;
        characterRenderer.sharedMaterials = matArray;
        material = matArray[1];

        ChangeEyes(Gestures.Default);
    }

    private void Update()
    {

        float fear = GameManager.Instance.PlayerData.FearLevel / GameManager.Instance.GameSettings.MaxFear;

        if (fear < 0.25)
        {
            ChangeEyes(Gestures.Default);
        }

        else if  (fear < 0.50)
        {
            ChangeEyes(Gestures.Worried);
        }

        else if  (fear < 0.75)
        {
            ChangeEyes(Gestures.Scared);
        }

        else
        {
            ChangeEyes(Gestures.Death);
        }
    }
}
