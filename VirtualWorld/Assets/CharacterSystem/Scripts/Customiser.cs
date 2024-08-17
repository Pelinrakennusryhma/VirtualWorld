using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;
using Unity.VisualScripting;
using GameKit.Utilities;


public class Customiser : MonoBehaviour
{
    //For selecting the scriptable object that contains all our customisation options.
    public Asset_OSAT Assets;


    //Giving the character a list of available targets for customisation.
    public SkinnedMeshRenderer Head;
    public SkinnedMeshRenderer Hair;
    public SkinnedMeshRenderer Clothes;
    public SkinnedMeshRenderer Accessories;
    public SkinnedMeshRenderer Eyebrows; //eyebrow and lash meshes are not customisable, only textures.
    public SkinnedMeshRenderer Lashes;



    //Body mesh is the same for everybody, but I am listing it here for texturing.
    public SkinnedMeshRenderer Body;


    //Head and body have separate materials (but both use the same skin colour).
    private Material SkinMaterial;
    private Material SkinMaterial2;
    private Material ClothesMaterial;
    private Material LashesMaterial;
    private Material BrowsMaterial;
    private Material HairMaterial;

    private Texture2D ClothesTexture;
    private Texture2D ClothesNorm;

    //variables for use internally and between this script and the bone graph.
    public Color SkColor;
    public Color HairColor;
    public float Fatness;
    public float Fitness;
    public float MascFem;
    public float Height;
    public float Chest;

    private int HeadRandom;
    private int ChestYN;




    //Runs 2 functions on start.
    //Instance to create new materials for character
    //Randomise to assign random values to head mesh, skin colour, and bodyshape variables for testing
    public void Start()
    {

        Instance();

        Randomise();

    }


    //Reads the default materials of the head, body, and clothes meshes and creates new instances of them.
    public void Instance()
    {
        Head.sharedMaterial = new Material(Head.sharedMaterial);
        Body.sharedMaterial = new Material(Body.sharedMaterial);
        Clothes.sharedMaterial = new Material(Clothes.sharedMaterial);
        Hair.sharedMaterial = new Material(Hair.sharedMaterial);
        Eyebrows.sharedMaterial = new Material(Eyebrows.sharedMaterial);
        Lashes.sharedMaterial = new Material(Lashes.sharedMaterial);

        SkinMaterial = Head.GetComponent<SkinnedMeshRenderer>().sharedMaterial;
        SkinMaterial2 = Body.GetComponent<SkinnedMeshRenderer>().sharedMaterial;
        ClothesMaterial = Clothes.GetComponent<SkinnedMeshRenderer>().sharedMaterial;
        LashesMaterial = Lashes.GetComponent<SkinnedMeshRenderer>().sharedMaterial;
        BrowsMaterial = Eyebrows.GetComponent<SkinnedMeshRenderer>().sharedMaterial;
        HairMaterial = Hair.GetComponent<SkinnedMeshRenderer>().sharedMaterial;

        Debug.Log("Materials created");
    }

    //This function just makes sure all variables everywhere match. It is what it is.
    //Is called in the randomise function, and can also be called with a button in the unity interface.
    //Bodyshape modifiers modify the size of clothing meshes slightly more, to avoid clipping issues.
    public void UpdtAppearance()
    {
        MascFem = (float)Variables.Object(this).Get("M_F");
        Fitness = (float)Variables.Object(this).Get("MuskeliAmount");
        Fatness = (float)Variables.Object(this).Get("LihavuusAmount");
        Chest = (float)Variables.Object(this).Get("RintaAmount");
        Height = (float)Variables.Object(this).Get("PituusAmount");
        SkinMaterial.SetFloat("_Sh_Muscle", Fitness);
        SkinMaterial2.SetFloat("_Sh_Muscle", Fitness);
        ClothesMaterial.SetFloat("_Sh_Muscle", (float)(Fitness * 1.02));
        SkinMaterial.SetFloat("_Sh_Fat", Fatness);
        SkinMaterial2.SetFloat("_Sh_Fat", Fatness);
        ClothesMaterial.SetFloat("_Sh_Fat", (float)(Fatness * 1.02));
        SkinMaterial.SetFloat("_Sh_M_F", MascFem);
        SkinMaterial2.SetFloat("_Sh_M_F", MascFem);
        ClothesMaterial.SetFloat("_Sh_M_F", (float)(MascFem * 1.02));
        SkinMaterial.SetColor("_SKN_Color", SkColor);
        SkinMaterial2.SetColor("_SKN_Color", SkColor);
        HairMaterial.SetColor("_Hair_ColorSh", HairColor);

        //Picks alpha for body material from the clothes the character is currently wearing, in order to hide the correct areas of the body under clothes. Fixes clipping issues.
        /*Does the same for face material in order to hide parts of the neck when needed. The clothes' normal map contains an alpha channel for this particular information. It is a strange place to
        keep this info, but I'm saving space by giving textures multiple uses.*/
        ClothesTexture = (Texture2D)ClothesMaterial.GetTexture("_BaseTexture");
        ClothesNorm = (Texture2D)ClothesMaterial.GetTexture("_NormalsTexture");
        SkinMaterial2.SetTexture("_ALPHATexture", ClothesTexture);
        SkinMaterial.SetTexture("_ALPHATexture", ClothesNorm);

        Debug.Log("Character appearance updated");
    }

    //The actual randomise function. Body material picks the same values that the head material rolls.
    //Eyebrow and Lash meshes have to pick the same array item number as the head. They must always match the head.
    //Brow and Lash textures are randomised.
    //Randomise currently also includes bodyshape variables, excluding height.
    public void Randomise()
    {
        ChestYN = Random.Range(0, 2);
        Debug.Log(ChestYN);
        HeadRandom = Random.Range(0, Assets.Heads.Length);

        Head.sharedMesh = Assets.Heads[HeadRandom];
        Eyebrows.sharedMesh = Assets.Eyebrows[HeadRandom];
        Lashes.sharedMesh = Assets.Lashes[HeadRandom];

        LashesMaterial.SetTexture("_BaseMap", Assets.LashTexture[Random.Range(0, Assets.LashTexture.Length)]);
        BrowsMaterial.SetTexture("_BaseMap", Assets.EyebrowTexture[Random.Range(0, Assets.EyebrowTexture.Length)]);


        SkinMaterial.SetColor("_SKN_Color", Assets.SkinColor[Random.Range(0, Assets.SkinColor.Length)]);
        SkColor = SkinMaterial.GetColor("_SKN_Color");
        SkinMaterial2.SetColor("_SKN_Color", SkinMaterial.GetColor("_SKN_Color"));

        
        SkinMaterial.SetFloat("_Sh_Fat", Random.Range(0, 10));
        Fatness = SkinMaterial.GetFloat("_Sh_Fat");
        SkinMaterial2.SetFloat("_Sh_Fat", SkinMaterial.GetFloat("_Sh_Fat"));

        SkinMaterial.SetFloat("_Sh_Muscle", Random.Range(0, 10));
        Fitness = SkinMaterial.GetFloat("_Sh_Muscle");
        SkinMaterial2.SetFloat("_Sh_Muscle", SkinMaterial.GetFloat("_Sh_Muscle"));

        SkinMaterial.SetFloat("_Sh_M_F", Random.Range(0, 10));
        MascFem = SkinMaterial.GetFloat("_Sh_M_F");
        SkinMaterial2.SetFloat("_Sh_M_F", SkinMaterial.GetFloat("_Sh_M_F"));

        Hair.sharedMesh = Assets.Hairs[Random.Range(0, Assets.Hairs.Length)];
        HairColor = Assets.HairColor[Random.Range(0, Assets.HairColor.Length)];
        HairMaterial.SetColor("_Hair_ColorSh", HairColor);

        //Making the probability of the character having a larger chest than 0, 50/50
        if (ChestYN == 1)
        {
            Chest = Random.Range(1, 10);
        }
        else
        {
            Chest = 0;
        }
            
            

        //juggling variables, yay

        Variables.Object(this).Set("M_F", "MascFem");
        Variables.Object(this).Set("MuskeliAmount", "Fitness");
        Variables.Object(this).Set("LihavuusAmount", "Fatness");
        Variables.Object(this).Set("RintaAmount", "Chest");
        Variables.Object(this).Set("PituusAmount", "Height");


        //this triggers a custom event in the bone graph, so that the bones get correctly morphed as well.
        CustomEvent.Trigger(this.gameObject, "Randomised");


        Debug.Log("Randomised");

        UpdtAppearance();
        
    }
 
}


//Creating two buttons in Unity interface for testing purposes. They call the "Randomise" and "Update Appearance" functions.
//The buttons only work correctly during play.

#if UNITY_EDITOR
[CustomEditor(typeof(Customiser))]

public class CustomiserEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var Customiser = (Customiser)target;

        if (Customiser != null)
        {
            if (GUILayout.Button("Randomise"))
            {
                Customiser.Randomise();
            }
            if (GUILayout.Button("Update Appearance"))
            {
                Customiser.UpdtAppearance();
            }


        }

    }

}
#endif
