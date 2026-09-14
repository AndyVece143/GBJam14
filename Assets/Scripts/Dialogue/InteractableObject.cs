using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public Player player;
    public string[] dialogue1;
    public string[] dialogue2;
    public InspectBox inspectBox;

    public BoxCollider2D boxCollider;
    public int react;
    public bool checker = false;
    private int progress = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = Player.FindAnyObjectByType<Player>();
        boxCollider = GetComponent<BoxCollider2D>();

        if (dialogue2.Length == 0)
        {
            Debug.Log("GAming");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateTextBox()
    {
        InspectBox newInspectBox = Instantiate(inspectBox);
        if (progress == 0)
        {
            newInspectBox.lines = dialogue1;

            if (dialogue2.Length == 0)
            {
                checker = true;
            }
            else
            {
                progress++;
            }
        }

        else if (progress == 1 && dialogue2.Length != 0)
        {
            newInspectBox.lines = dialogue2;
            checker = true;
        }
    }
}
