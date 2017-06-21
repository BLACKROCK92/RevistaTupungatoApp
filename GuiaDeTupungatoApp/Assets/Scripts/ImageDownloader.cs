using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using LitJson;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

public class ImageDownloader : MonoBehaviour
{
    #region Atributos
    [SerializeField]
    Image[] contents;
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
    #endregion
    //-----------------------------------------------------------------------------------------------------------------
    private void Start()
    {
        ContinueBtn.enabled = false;
        StartCoroutine(ComprobarFechaCR());
    }
    //------------------------------------------------------------------------------------------------------------------------
    IEnumerator traerImgs()
    {
        int i = 1;
        foreach (Image img in contents)
        {
            if (i < 10)
            {
                url = "http://www.laguiadetupungato.com/img/0" + i + ".jpg";
            }
            else
            {
                url = "http://www.laguiadetupungato.com/img/" + i + ".jpg";
            }
            //---------------------------------------------------------------------
            WWW imglink = new WWW(url);
            if (imglink.error == null)
            {
                yield return imglink;
                //Texture2D texture = new Texture2D(1, 1);
                //imglink.LoadImageIntoTexture(texture);
                //Sprite spr = Sprite.Create(imglink.texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                //contents[i - 1].sprite = spr;
                imglink.LoadImageIntoTexture(contents[i-1].sprite.texture);
                progress = (i * 100) / contents.Length;
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
        CargarFechaLocal();
        yield return StartCoroutine(traerFecha());
        if (!comprobarFecha())
        {
            StartCoroutine(traerImgs());
        }
        else
        {
            if (fechaCorrecta)
            {
                activarSiguienteMenu();
            }
        }
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
            //GuardarFechaLocal();
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
}

[Serializable]
class FechaClass
{
    public string fechaAtr;
    public bool fechaCorrectaAtr;
}