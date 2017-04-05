using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogoInicial : MonoBehaviour
{
    public GameObject Header;
    public GameObject Logo;
    public GameObject Revista;

    public void continuar()
    {
        Logo.SetActive(false);
        Header.SetActive(true);
        Revista.SetActive(true);
    }
}
