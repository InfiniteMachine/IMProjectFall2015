using UnityEngine;
using System.Collections;

public class BitableObject : MonoBehaviour {
    public int bites = 2;

    public void Bite()
    {
        bites--;
        if(bites <= 0)
        {
            Destroy(gameObject);
        }
    }
}
