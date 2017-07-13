﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;

public class ControladorDePantallas : MonoBehaviour
{
    #region Atributos

    public GameObject Header;
    public GameObject Logo;
    public GameObject Revista;
    public GameObject Destacados;
    public GameObject Contacto;
    public GameObject DestacadoIndividual;
    public Image[] DestacadosImgs = new Image[10];
    public string[] DestacadosDescripciones = new string[10];
    public string[] DestacadosContacto = new string[10];
    public Image DestacadoIndImg;
    public Text DestacadoDescText;
    public Text DestacadoContactoText;
    JsonData data;
    #endregion

    public void continuar()
    {
        Logo.SetActive(false);
        Header.SetActive(true);
        Revista.SetActive(true);
        DestacadoIndividual.SetActive(false);
    }

    void Start()
    {
        Logo.SetActive(true);
        Revista.SetActive(false);
        Destacados.SetActive(false);
        Contacto.SetActive(false);
        DestacadoIndividual.SetActive(false);
        Header.SetActive(false);
        StartCoroutine(DescargarDescripcionesDestacados());
    }

    public IEnumerator DescargarDescripcionesDestacados()
    {
        WWW www = new WWW("http://laguiadetupungato.com/DescripcionesDestacados.js");

        if (www.error == null)
        {
            yield return www;
            data = JsonMapper.ToObject(www.text);
            int i = 1;
            foreach (string str in DestacadosDescripciones)
            {
                DestacadosDescripciones[i - 1] = data["data"]["descripcion-destacado-" + i].ToString();
                //Debug.Log(data["data"]["descripcion-destacado-" + i].ToString());
                i++;
            }
        }
        else
        {
            Debug.LogError("Error en el www: " + www.error);
        }
        yield return www;
    }

    public void MostrarRevista()
    {
        Revista.SetActive(true);
        Destacados.SetActive(false);
        Contacto.SetActive(false);
        DestacadoIndividual.SetActive(false);
    }
    public void MostrarDestacados()
    {
        Revista.SetActive(false);
        Destacados.SetActive(true);
        Contacto.SetActive(false);
        DestacadoIndividual.SetActive(false);
    }
    public void MostrarContacto()
    {
        Revista.SetActive(false);
        Destacados.SetActive(false);
        Contacto.SetActive(true);
        DestacadoIndividual.SetActive(false);
    }
    public void RegresarADestacados()
    {
        DestacadoIndividual.SetActive(false);
        Destacados.SetActive(true);
        Header.SetActive(true);
    }
    public void MostrarDestacadoInd(int destacadonro)
    {
        Revista.SetActive(false);
        Destacados.SetActive(false);
        Contacto.SetActive(false);
        //Header.SetActive(false);
        DestacadoIndividual.SetActive(true);
        switch (destacadonro)
        {
            case 0:
                DestacadoIndImg.sprite = DestacadosImgs[0].sprite;
                DestacadoDescText.text = DestacadosDescripciones[0];
                DestacadoContactoText.text = DestacadosContacto[0];
                break;
            case 1:
                DestacadoIndImg.sprite = DestacadosImgs[1].sprite;
                DestacadoDescText.text = DestacadosDescripciones[1];
                DestacadoContactoText.text = DestacadosContacto[1];
                break;
            case 2:
                DestacadoIndImg.sprite = DestacadosImgs[2].sprite;
                DestacadoDescText.text = DestacadosDescripciones[2];
                DestacadoContactoText.text = DestacadosContacto[2];
                break;
            case 3:
                DestacadoIndImg.sprite = DestacadosImgs[3].sprite;
                DestacadoDescText.text = DestacadosDescripciones[3];
                DestacadoContactoText.text = DestacadosContacto[3];
                break;
            case 4:
                DestacadoIndImg.sprite = DestacadosImgs[4].sprite;
                DestacadoDescText.text = DestacadosDescripciones[4];
                DestacadoContactoText.text = DestacadosContacto[4];
                break;
            case 5:
                DestacadoIndImg.sprite = DestacadosImgs[5].sprite;
                DestacadoDescText.text = DestacadosDescripciones[5];
                DestacadoContactoText.text = DestacadosContacto[5];
                break;
            case 6:
                DestacadoIndImg.sprite = DestacadosImgs[6].sprite;
                DestacadoDescText.text = DestacadosDescripciones[6];
                DestacadoContactoText.text = DestacadosContacto[6];
                break;
            case 7:
                DestacadoIndImg.sprite = DestacadosImgs[7].sprite;
                DestacadoDescText.text = DestacadosDescripciones[7];
                DestacadoContactoText.text = DestacadosContacto[7];
                break;
            case 8:
                DestacadoIndImg.sprite = DestacadosImgs[8].sprite;
                DestacadoDescText.text = DestacadosDescripciones[8];
                DestacadoContactoText.text = DestacadosContacto[8];
                break;
            case 9:
                DestacadoIndImg.sprite = DestacadosImgs[9].sprite;
                DestacadoDescText.text = DestacadosDescripciones[9];
                DestacadoContactoText.text = DestacadosContacto[9];
                break;
            default:
                break;
        }
    }

}
