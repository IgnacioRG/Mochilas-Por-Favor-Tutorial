using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MochilasTutorial : MonoBehaviour
{
    //Gameobject de tipo ui.
    public GameObject paso_ui;
    public GameObject exp_ui;

    //Gameobject de tipo boton.
    public GameObject sig_boton;

    //Assets de explicacion.
    public Sprite lore_sp;
    public Sprite mochila_sp;
    public Sprite requeridos_sp;
    public Sprite baneados_sp;
    public List<Sprite> mecanica_sp;
    public Sprite victoria_sp;
    public Sprite derrota_sp;

    private bool _sig = false;

    //Inicializacion de parametros y gameobjects
    void Start()
    {
        _sig = false;
        sig_boton.transform.GetChild(0).GetComponent<Text>().text = "Siguiente";
        sig_boton.GetComponent<Button>().onClick.RemoveAllListeners();
        sig_boton.GetComponent<Button>().onClick.AddListener(SiguientePaso);

        StartCoroutine(TutorialFlujo());
    }

    /**
     * Metodo para volver a la escena principal tras completar el tutorial.
     */
    public void VolverAlPrincipal()
    {
        SceneManager.LoadScene("MenuMochilas");
    }

    /**
     * SiguientePaso se llama cada vez que el jugador termina de leer la instruccion
     * actual (boton siguiente).
     */
    public void SiguientePaso()
    {
        _sig = true;
    }

    /**
     * Flujo normal del tutorial en el que se explica en 4 pasos las mecanicas del juego.
     * 1. Contexto de la tematica del juego.
     * 2. Primer elemento principal de la interfaz (tabla de mochila).
     * 3. Segundo elemento principal de la interfaz (tabla de objetos requeridos).
     * 4. Tercer elemento principal de la interfaz (tabla de objetos baneados).
     * 5. Mecanica principal y reglas para la toma de decision.
     * 6. Condiciones de victoria.
     * 7. Condiciones de derrota.
     */
    IEnumerator TutorialFlujo()
    {
        exp_ui.GetComponent<Text>().text = "¡Mochilas por favor! Revisa las mochilas de las personas que quieren entrar al trolebús y deja entrar a los que cumplan el reglamento.";
        paso_ui.GetComponent<Image>().sprite = lore_sp;
        while (!_sig)
        {
            yield return null;
        }

        _sig = false;
        exp_ui.GetComponent<Text>().text = "Los pasajeros llevan consigo diferentes objetos en sus mochilas, puedes verlo en la parte superior.";
        paso_ui.GetComponent<Image>().sprite = mochila_sp;
        while (!_sig)
        {
            yield return null;
        }

        _sig = false;
        exp_ui.GetComponent<Text>().text = "Los objetos que deben llevar lo puedes ver en la tabla verde de la izquierda. Recuerda que deben llevar TODO lo de esta tabla.";
        paso_ui.GetComponent<Image>().sprite = requeridos_sp;
        while (!_sig)
        {
            yield return null;
        }

        _sig = false;
        exp_ui.GetComponent<Text>().text = "Los objetos prohibidos los puedes ver en la tabla roja de la derecha. Nadie debe llevar ni uno solo de estos objetos ¡Mucho ojo!";
        paso_ui.GetComponent<Image>().sprite = baneados_sp;
        while (!_sig)
        {
            yield return null;
        }

        _sig = false;
        exp_ui.GetComponent<Text>().text = "Después de revisar la mochila, decide si dejaras pasar a la persona o no.";
        while (!_sig)
        {
            foreach (Sprite sp in mecanica_sp)
            {
                paso_ui.GetComponent<Image>().sprite = sp;
                if (_sig)
                {
                    break;
                }
                yield return new WaitForSeconds(1);
            }
            yield return null;
        }

        _sig = false;
        exp_ui.GetComponent<Text>().text = "¡Realiza bien las inspecciones y sube de nivel a un nuevo reto!";
        paso_ui.GetComponent<Image>().sprite = victoria_sp;
        while (!_sig)
        {
            yield return null;
        }

        exp_ui.GetComponent<Text>().text = "¡Cuidado! Si te equivocas al no dejar pasar una persona que cumple o dejas pasar a una persona que no cumple, bajarás de nivel.";
        paso_ui.GetComponent<Image>().sprite = derrota_sp;

        sig_boton.transform.GetChild(0).GetComponent<Text>().text = "Comenzar";
        sig_boton.GetComponent<Button>().onClick.RemoveAllListeners();
        sig_boton.GetComponent<Button>().onClick.AddListener(VolverAlPrincipal);
    }
}
