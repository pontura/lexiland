using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text.RegularExpressions;

public class Login : MonoBehaviour {

    public static Login login;

    //public InputField nombre;

    public Text resultField;

    public InputField emailField;

  //  public InputField mail;
    public GameObject panelError;

    public Button botonIngresar;

    public const string MatchEmailPattern =
        @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
        + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
        + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
        + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";


    public void Awake()
	{
        login = this;
	}

	public void OnEnable()
	{
        panelError.SetActive(false);
        emailField.text = "";
        resultField.text = "";
        //nombre.text = "";
        //mail.text = "";
        // CheckData();
    }

    //public void CheckData(){
    //    botonIngresar.interactable = (nombre.text != "" && mail.text != "");
    //}
    void Alert(string text)
    {
        CancelInvoke();
        resultField.text = text;
        Invoke("Reset", 3);
    }
    void Reset()
    {
        resultField.text = "";
    }
    public void Registrar(){

        string email = emailField.text;

       if (!validateEmail(email))
        {
            Alert("Email incorrecto");
            return;
        }
        else
        {
            Alert("Procesando...");
            DatabaseManager.Instance.Login(email, OnLoginDone);
        }
    }

    void OnLoginDone()
    {
        if (DatabaseManager.Instance.teacherData.all.Length == 0)
        {
            Alert("Usuario inexistente");
            return;
        }
        DatabaseManager.TeacherSingleData data = DatabaseManager.Instance.GetData();
        string username = data.name + " " + data.lastname;
        resultField.text = "";
        string directorio = Application.persistentDataPath + "/Users/";
        if (!Directory.Exists(directorio))
        {
            Directory.CreateDirectory(directorio);
        }
        DirectoryInfo di = new DirectoryInfo(directorio);

        foreach (DirectoryInfo d in di.GetDirectories())
        {
            FileInfo[] fi = d.GetFiles("user.xml");
            if (fi.Length > 0)
            {
                Usuario usuario = Usuario.Load(fi[0].FullName);
                if (usuario.mail == emailField.text)
                {
                    PlayerPrefs.SetString("UserID", usuario.ID);
                    Debug.Log("Entra con " + usuario.nombre);
                    CCanvasManager.Instance.IrAMain();
                    break;
                }
            }
        }

        if (PlayerPrefs.GetString("UserID") == "")
        {
            Usuario user = new Usuario("", emailField.text);
            user.Save(true);

            StartCoroutine(DBServices.db.RegisterUser(user));
        }
    }

    public void ErrorConexion(){

        Debug.Log("No hay conexion");
        panelError.SetActive(true);

    }

    public bool validateEmail(string email)
    {
        if (email != null)
            return Regex.IsMatch(email, MatchEmailPattern);
        else
            return false;
    }




}
