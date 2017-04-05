using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeaderButtons : MonoBehaviour
{

    public GameObject revistaObj;
    public GameObject destacadosObj;
    public GameObject contactoObj;

    void Start()
    {

        destacadosObj.SetActive(false);
        contactoObj.SetActive(false);
    }

    public void MostrarRevista()
    {
        revistaObj.SetActive(true);
        destacadosObj.SetActive(false);
        contactoObj.SetActive(false);
    }

    public void MostrarDestacados()
    {
        revistaObj.SetActive(false);
        destacadosObj.SetActive(true);
        contactoObj.SetActive(false);
    }

    public void Mostrarcontacto()
    {
        revistaObj.SetActive(false);
        destacadosObj.SetActive(false);
        contactoObj.SetActive(true);
    }
}
