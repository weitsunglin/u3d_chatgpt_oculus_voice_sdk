using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroviewController : MonoBehaviour
{
    [ Header( "Introview" ) ]
    [SerializeField] private GameObject Introview;

    public void Show()
    {
        Introview.SetActive( true );
    }

    public void Hide()
    {
        Introview.SetActive( false );
    }
    
}