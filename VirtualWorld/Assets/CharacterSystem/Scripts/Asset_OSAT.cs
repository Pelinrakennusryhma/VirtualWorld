using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Blueprint for a scriptable object.

[CreateAssetMenu(menuName = "Character Assets")]

public class Asset_OSAT : ScriptableObject
{
    /* Option arrays for Heads, Hairs, Clothes, Eyebrows, Lashes, Accessories, Colours (please note American spelling "Color") */

    public Color[] SkinColor;
    public Color[] HairColor;
    public Color[] EyeColor; //not in use currently

    public Texture2D[] LashTexture;
    public Texture2D[] EyebrowTexture;

    public Mesh[] Heads;
    public Mesh[] Lashes;
    public Mesh[] Eyebrows;
    public Mesh[] Hairs;
    public Mesh[] Clothes;
    public Mesh[] Accessories;






}