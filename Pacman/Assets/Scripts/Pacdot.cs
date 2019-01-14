using UnityEngine;

public class Pacdot : MonoBehaviour
{
    public bool isSuperPacdot = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Pacman")
        {
            if (isSuperPacdot)
            {
                //TODO                
                GameManager.Instance.OnEatPacdot(gameObject);
                GameManager.Instance.OnEatSpuerPacdot();
                Destroy(gameObject);
            }
            else
            {
                GameManager.Instance.OnEatPacdot(gameObject);
                Destroy(gameObject);
            }
        }       
    }
}
