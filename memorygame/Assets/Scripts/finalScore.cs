//script que mostra o resultado final no final do nível e envia o resultado para o endpoint
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class finalScore : MonoBehaviour
{
    //componente text
    Text results;

    // Use this for initialization
    void Start()
    {

        results = GetComponent<Text>();

        //exibe os resultados chamando as ints gravadas no controlador geral e no controlador da main scene
        results.text =
            (PlayerPrefs.GetString("Nickname")) + " - " + (PlayerPrefs.GetString("Level Number")) + " resultados:" +
            "\nAcertos: " + PlayerPrefs.GetInt("Acertos") +
            "\nErros: " + PlayerPrefs.GetInt("Erros") +
            "\n \nAcertos multiplicados por 100 = " + (PlayerPrefs.GetInt("Acertos") * 100) +
            "\nErros multiplicados por 20 = " + (PlayerPrefs.GetInt("Erros") * 20) +
            "\n" + (PlayerPrefs.GetInt("Acertos") * 100) + " - " + (PlayerPrefs.GetInt("Erros") * 20) + " = " +
            ((PlayerPrefs.GetInt("Acertos") * 100) - (PlayerPrefs.GetInt("Erros") * 20)) +
            "\n \nPontuação final: " + ((PlayerPrefs.GetInt("Acertos") * 100) - (PlayerPrefs.GetInt("Erros") * 20));

        StartCoroutine(Upload());

    }

    //envia o resultado para o endpoint
    IEnumerator Upload()
    {
        //dados a serem enviados
        byte[] data = System.Text.Encoding.UTF8.GetBytes(results.text);
        //realiza o WebRequest como POST
        UnityWebRequest www = UnityWebRequest.Post("https://us-central1-huddle-team.cloudfunctions.net/api/memory/ayumi_nanda@hotmail.com", data.ToString());
        //envia para o endpoint
        yield return www.SendWebRequest();

        //checa se ocorreu algum erro
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        //se não, verificar o response code
        else
        {
            Debug.Log(www.responseCode);
        }
    }
}
