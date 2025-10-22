using UnityEngine;

// Starts with Grass, cycles through Fire and Water.

public class PlayerElement : MonoBehaviour
{
    public ElementType Current = ElementType.Grass;


    public void NextElement()
    {
        Current = (ElementType)(((int)Current + 1) % 3);
        
        // TODO: Update UI color/icon.
    }
}