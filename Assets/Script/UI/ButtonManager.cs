using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    [SerializeField]
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickDown()
    {
        anim.SetBool("Push", true);
    }

    public void OnClickUp()
    {
        anim.SetBool("Push", false);
    }
}
