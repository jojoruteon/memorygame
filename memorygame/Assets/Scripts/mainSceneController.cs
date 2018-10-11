using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class mainSceneController : MonoBehaviour {

    //objeto que contém o componente text usado para definir o nickname
    GameObject nicknameField;

    //componentes text
    Text nickname;
    Text levelNumber;

    //nome da cena que será chamada
    public string level;

    void Start()
    {
        //o objeto é achado pela Tag
        nicknameField = GameObject.FindGameObjectWithTag("nicknameField");

        //para não retornar NullException
        if(nicknameField != null)
        {
            nickname = nicknameField.GetComponent<Text>();
        }
        levelNumber = GetComponentInChildren<Text>();
    }

    //função que será chamada no botão do nível na tela de seleção
	public void enterLevel ()
    {
        //o Nickname e o número do nível são salvos para serem exibidos na tela de resultados
        PlayerPrefs.SetString("Nickname", nickname.text);
        PlayerPrefs.SetString("Level Number", levelNumber.text);
        Debug.Log(PlayerPrefs.GetString("Nickname"));
        Debug.Log(PlayerPrefs.GetString("Level Number"));
        //para não retornar NullException
        if (level != null)
        {
            SceneManager.LoadScene(level);
        }
    }

    //função que será chamada no botão de voltar na tela de resultados
    public void backToLevelSelect()
    {
        SceneManager.LoadScene("main_scene");
    }
}
