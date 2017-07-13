using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using LitJson;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using System.Collections.Generic;

public class ImageDownloader : MonoBehaviour
{
    #region Atributos
    [SerializeField]
    List<Image> contents = new List<Image>();
    [SerializeField]
    List<Image> contentDestacados = new List<Image>();
    [SerializeField]
    Slider progressBar;
    [SerializeField]
    GameObject loadingTxt;
    [SerializeField]
    GameObject continueTxt;
    [SerializeField]
    Button ContinueBtn;
    [SerializeField]
    GameObject progressBarGO;
    float progress;
    string url;
    [SerializeField]
    string fecha = "";
    [SerializeField]
    string fechaLocal = "";
    [SerializeField]
    bool fechaCorrecta;
    string url2 = "";
    #endregion
    //-----------------------------------------------------------------------------------------------------------------
    private void Start()
    {
        ContinueBtn.enabled = false;
        StartCoroutine(ComprobarFechaCR());
        StartCoroutine(DescargarDestacados());
    }
    //------------------------------------------------------------------------------------------------------------------------
    IEnumerator DescargarRevista()
    {
        int i = 1;
        foreach (Image img in contents)
        {
            if (i < 10)
            {
                url = "http://www.laguiadetupungato.com/img/Revista/0" + i + ".jpg";
            }
            else
            {
                url = "http://www.laguiadetupungato.com/img/Revista/" + i + ".jpg";
            }
            //---------------------------------------------------------------------
            WWW imglink = new WWW(url);
            if (imglink.error == null)
            {
                yield return imglink;
                imglink.LoadImageIntoTexture(contents[i - 1].sprite.texture);
                progress = (i * 100) / contents.Count;
                progressBar.value = progress;
                i++;
            }
            else
            {
                print(imglink.error);
            }
        }

        if (progress >= 100)
        {
            activarSiguienteMenu();
        }
    }
    //--------------------------------------------------------------------------------------------------------------------------
    public bool comprobarFecha()
    {
        if (fecha.Equals(fechaLocal))
        {
            fechaCorrecta = true;
            return fechaCorrecta;
        }
        else
        {
            fechaCorrecta = false;
            return fechaCorrecta;
        }
    }
    //--------------------------------------------------------------------------------------------------------------------------
    public IEnumerator ComprobarFechaCR()
    {
        int pags = 0;
        WWW numeroDePaginas = new WWW("http://www.laguiadetupungato.com/Paginas.js");
        yield return numeroDePaginas;
        if (numeroDePaginas.error == null)
        {

            JsonData dataPags = JsonMapper.ToObject(numeroDePaginas.text);
            pags = int.Parse(dataPags["data"]["Paginas"].ToString());
            //Debug.Log("Pags: " + pags);
            //Debug.Log("Contents.Count: " + contents.Count);
        }
        for (int j = pags; j < contents.Count; j++)
        {
            Destroy(contents[j].gameObject);
        }
        if (contents.Count > pags)
        {
            contents.RemoveRange(pags, contents.Count - pags);

        }
        //--------------------------------------------------------------------------------------
        CargarFechaLocal();
        yield return StartCoroutine(traerFecha());
        //if (!comprobarFecha())
        //{
        //    StartCoroutine(traerImgs());
        //}
        //else
        //{
        //    if (fechaCorrecta)
        //    {
        //        activarSiguienteMenu();
        //    }
        //}
        StartCoroutine(DescargarRevista());
    }
    //--------------------------------------------------------------------------------------------------------------------------
    public IEnumerator traerFecha()
    {

        WWW www = new WWW("http://www.laguiadetupungato.com/Fecha.js");
        if (www.error == null)
        {
            yield return www;
            JsonData data = JsonMapper.ToObject(www.text);
            fecha = data["data"]["Fecha"].ToString();
        }
        else
        {
            print(www.error);
        }
        yield return www;
    }
    //--------------------------------------------------------------------------------------------------------------------------
    void activarSiguienteMenu()
    {
        GuardarFechaLocal();
        progressBarGO.SetActive(false);
        loadingTxt.SetActive(false);
        continueTxt.SetActive(true);
        ContinueBtn.enabled = true;
        StopAllCoroutines();
    }
    //--------------------------------------------------------------------------------------------------------------------------
    void CargarFechaLocal()
    {
        if (File.Exists(Application.persistentDataPath + "/fecha.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/fecha.dat", FileMode.Open);
            FechaClass fechaInst = (FechaClass)bf.Deserialize(file);
            file.Close();
            fechaLocal = fechaInst.fechaAtr;
        }
    }
    //--------------------------------------------------------------------------------------------------------------------------
    void GuardarFechaLocal()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/fecha.dat", FileMode.OpenOrCreate);

        FechaClass fechaInstancia = new FechaClass();
        fechaInstancia.fechaAtr = fecha;

        bf.Serialize(file, fechaInstancia);
        file.Close();
    }
    //--------------------------------------------------------------------------------------------------------------------------
    public void CargarDestacados()
    {
        StartCoroutine(DescargarDestacados());
    }
    //--------------------------------------------------------------------------------------------------------------------------
    public IEnumerator DescargarDestacados()
    {
        int i = 1;
        foreach (Image img in contentDestacados)
        {
            url2 = "http://laguiadetupungato.com/img/Destacados/Destacados-" + i + ".jpg";
            WWW destWWW = new WWW(url2);
            if (destWWW.error == null)
            {
                yield return destWWW;
                destWWW.LoadImageIntoTexture(img.sprite.texture);
            }
            else
            {
                Debug.LogError("Error en el www: " + destWWW.error);
            }
            i++;
        }
    }

}

[Serializable]
class FechaClass
{
    public string fechaAtr;
    public bool fechaCorrectaAtr;
}