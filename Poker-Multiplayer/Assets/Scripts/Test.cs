using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    
    
    [ContextMenu("test")]
    private void test(){
        string num = "9_of_clubs";
        string newString = num.Substring(num.IndexOf("_",num.IndexOf("_",3))+1);
        Debug.Log(newString);
    }

}
