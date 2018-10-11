//script geral de pontuação
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class score : MonoBehaviour {

    //componente text
    Text txt;

    //linha editável no inspector para reutilização do script
    public string UIText;

    //valor inicial
    [HideInInspector]
    public int add = 0;

    void Start() {

        txt = GetComponent<Text>();
    }

    //atualiza o valor
    void Update()
    {
        txt.text = UIText + add.ToString();
    }
	
    //adiciona ao valor
	public void addToScore()
    {
        add++;
	}
}
