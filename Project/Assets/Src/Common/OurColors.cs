using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace QubeWorld
{
    [CreateAssetMenu(fileName = "OurColors", menuName = "ScriptableObject/OurColors", order = 0)]
    public class OurColors : ScriptableObject
    {
        public Color32[] colors;
    }
}