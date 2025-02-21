using TMPro;
using UnityEngine;
using Wana;

public class PawnGO : MonoBehaviour
{
    [SerializeField] private TextMeshPro label;
    private Pawn pawn;

    public Pawn GetPawn()
    {
        return pawn;
    }

    public void SetPawn(Pawn pawn)
    {
        this.pawn = pawn;
    }

    public void SetLabel(string value)
    {
        label.text = value;
    }

    public void SetColor(Color color)
    {
        GetComponent<SpriteRenderer>().color = color;
    }

}
