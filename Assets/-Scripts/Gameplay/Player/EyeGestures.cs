using UnityEngine;



public class EyeGestures : MonoBehaviour
{
    //STATES
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
        // possistional co-ords for sliders
        // Defualt
        new(0.0f,0.0f),
        // Worried
        new(0.5f,0.0f),
        //Scared
        new(0.0f,0.5f),
        //Death
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
        // in relation to the Vingnette
        float fear = GameManager.Instance.PlayerData.FearLevel / GameManager.Instance.GameSettings.MaxFear;

        if (fear < 0.40)
        {
            ChangeEyes(Gestures.Default);
        }

        else if  (fear < 0.70)
        {
            ChangeEyes(Gestures.Worried);
        }

        else if  (fear < 0.90)
        {
            ChangeEyes(Gestures.Scared);
        }

        else
        {
            ChangeEyes(Gestures.Death);
        }
    }
}
