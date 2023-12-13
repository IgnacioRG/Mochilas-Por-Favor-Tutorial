using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AdminMochila : MonoBehaviour
{
    [Tooltip("Arreglo con los prefabs de los objetos.")]
    public GameObject[] objetos;

    [Tooltip("Arreglo con los prefabs de las personas.")]
    public GameObject[] personas;

    [Tooltip("Arreglo de Transform de objetos en panel Debe Llevar.")]
    public Transform[] posObjsDebeLlevar;

    [Tooltip("Arreglo de Transform de objetos en panel No Debe Llevar.")]
    public Transform[] posObjsNoDebeLlevar;

    [Tooltip("Arreglo de Transform de objetos en panel Mochila.")]
    public Transform[] posObjsMochila;

    [Tooltip("Arreglo de Image para resaltar objetos en panel Mochila.")]
    public Image[] resaltadoMochila;

    [Tooltip("Arreglo de Image para resaltar objetos en panel Debe Llevar.")]
    public Image[] resaltadoDebeLlevar;

    [Tooltip("Arreglo de Image para resaltar objetos en panel No Debe Llevar.")]
    public Image[] resaltadoNoDebeLlevar;

    [Tooltip("Objeto donde se van a mostrar las personas.")]
    public Transform posPersona;

    [Tooltip("Panel que se muestra cuando el ejercicio se resuelve correctamente.")]
    public GameObject correctoMenu;

    [Tooltip("Panel que se muestra cuando el ejercicio se resuelve incorrectamente.")]
    public GameObject incorrectoMenu;

    [Tooltip("Panel que se muestra cuando se sube de nivel.")]
    public GameObject subesNivelMenu;

    [Tooltip("Panel que se muestra cuando se baja de nivel.")]
    public GameObject bajasNivelMenu;

    [Tooltip("Panel que se muestra cuando se completa el nivel 10.")]
    public GameObject felicidadesMenu;

    [Tooltip("Panel que se muestra cuando se falla el nivel 1.")]
    public GameObject repiteNivel1Menu;

    [Tooltip("Texto donde se muestra el nivel.")]
    public Text levelText;

    [Tooltip("Texto donde se muestra el número de aciertos.")]
    public Text aciertosText;

    [Tooltip("Texto donde se muestra el número de fallos.")]
    public Text fallosText;

    [Tooltip("Texto donde se muestra retroalimentación cuando se demora mucho en contestar un ejercicio.")]
    public Text demoraFallosText;

    [Tooltip("Texto donde se muestra retroalimentación cuando se falla un ejercicio.")]
    public Text ejercicioFallidoText;

    [Tooltip("Número de segundos antes de que se considere un fallo en el primer nivel.")]
    public float segundosDeEsperaInicial = 120;

    [Tooltip("Número de segundos que se le suman al tiempo de espera inicial con cada nivel.")]
    public float segundosAdicionalesCadaNivel = 20;

    [Tooltip("Nivel del juego. Tiene valor de 1 a 10.")]
    [Range(1, 10)]
    public int level = 1;
    public AudioSource soundEffects;
    public AudioClip puertaAbriendoSound;
    public AudioClip subeNivelSound;
    public AudioClip bajaNivelSound;
    public AudioClip aciertoSound;
    public AudioClip falloSound;
    public AudioClip[] vocesSound;

    [SerializeField]
    int _NeurocoinsRecibidasAlGanar = 5;
    [SerializeField]
    int _NeurocoinsRecibidasAlSubirDeNivel = 20;

    public GameObject mascota;
    public GameObject coins_panel;
    public Text coins_text;

    private Ejercicio _tipoEjercicioActual;
    private MochilasPorFavor.LevelManager _levelManager = new MochilasPorFavor.LevelManager(1);
    private int _aciertos = 0;
    private int _fallos = 0;

    private List<int> _posFaltan = new List<int>(); // Lista con índices de imágenes en resaltadoDebeLlevar que mostraran si se falla un ejercicio RechazarFaltan
    private List<int> _posNoLlevar = new List<int>(); // Lista con índices de imágenes en resaltadoNoDebeLlevar que mostraran si se falla un ejercicio RechazarNoLlevar
    private List<int> _posMochilaNoLlevar = new List<int>(); // Lista con índices de imágenes en resaltadoMochila que mostraran si se falla un ejercicio RechazarNoLlevar

    private IEnumerator coroutineFallarPorTiempo;

    private enum Ejercicio 
    {
        Aceptar, 
        RechazarFaltan, 
        RechazarNoLlevar
    };

    void Start()
    {
        _levelManager.Level = level;
        levelText.text = _levelManager.Level.ToString();
        aciertosText.text = _aciertos.ToString();
        fallosText.text = _fallos.ToString();
        PrepararEjercicio();
    }

    /// <summary>
    /// Método que se ejecuta cuando se presiona el botón de Aceptar
    /// </summary>
    public void Aceptar()
    {
        StopCoroutine(coroutineFallarPorTiempo);
        //soundEffects.clip = puertaAbriendoSound;
        //soundEffects.Play();

        if (_tipoEjercicioActual == Ejercicio.Aceptar)
        {
            Debug.Log("Ejercicio contestado correctamente");
            _aciertos++;
            _fallos = 0;
            GuardaPartida(true, false);
            if (_aciertos < 3) {
                mascota.GetComponent<Animator>().Rebind();
                mascota.GetComponent<Animator>().SetInteger("Estado", 1); //aplauso
                coins_panel.SetActive(true);
                coins_text.text = "¡+" + _NeurocoinsRecibidasAlGanar + " Neurocoins!";
                ActualizaNeurocoins(_NeurocoinsRecibidasAlGanar);
            }            
            correctoMenu.SetActive(true);
            soundEffects.clip = aciertoSound;
            soundEffects.Play();
        }
        else
        {
            Debug.Log("Ejercicio contestado incorrectamente");
            soundEffects.clip = falloSound;
            soundEffects.Play();
            _fallos++;
            _aciertos = 0;
            GuardaPartida(false, false);
            incorrectoMenu.SetActive(true);
            if (_tipoEjercicioActual == Ejercicio.RechazarFaltan)
            {
                foreach (int pos in _posFaltan)
                    resaltadoDebeLlevar[pos].enabled = true;
                ejercicioFallidoText.text = "Faltan objetos en la mochila";
            }
            if (_tipoEjercicioActual == Ejercicio.RechazarNoLlevar)
            {
                foreach (int pos in _posNoLlevar)
                    resaltadoNoDebeLlevar[pos].enabled = true;
                foreach (int pos in _posMochilaNoLlevar)
                    resaltadoMochila[pos].enabled = true;
                ejercicioFallidoText.text = "Hay objetos que no debe llevar en la mochila";
            }
        }
        UpdateText();
    }

    /// <summary>
    /// Método que se ejecuta cuando se presiona el botón de Rechazar
    /// </summary>
    public void Rechazar()
    {
        StopCoroutine(coroutineFallarPorTiempo);

        //if (Random.Range(0.0f, 1.0f) <= 0.5f)
        //{
        //    soundEffects.clip = vocesSound[Random.Range(0, vocesSound.Length)];
        //    soundEffects.Play();
        //}

        if (_tipoEjercicioActual == Ejercicio.Aceptar)
        {
            Debug.Log("Ejercicio contestado incorrectamente");
            _fallos++;
            _aciertos = 0;
            soundEffects.clip = falloSound;
            soundEffects.Play();
            incorrectoMenu.SetActive(true);
            ejercicioFallidoText.text = "Tenías que dejarlo pasar";
            GuardaPartida(false,false);
        }
        else
        {
            Debug.Log("Ejercicio contestado correctamente");
            _aciertos++;
            _fallos = 0;
            soundEffects.clip = aciertoSound;
            soundEffects.Play();
            if (_aciertos < 3) {
                mascota.GetComponent<Animator>().Rebind();
                mascota.GetComponent<Animator>().SetInteger("Estado", 1); //aplauso
                coins_panel.SetActive(true);
                coins_text.text = "¡+" + _NeurocoinsRecibidasAlGanar + " Neurocoins!";
                ActualizaNeurocoins(_NeurocoinsRecibidasAlGanar);
            }            
            correctoMenu.SetActive(true);
            GuardaPartida(true,false);
        }
        UpdateText();
    }
    
    /// <summary>
    /// Cambia la escena. Se ejecuta al presionar el botón Salir
    /// </summary>
    /// <param name="sceneName"> Nombre de la escena en Assets/Scenes </param>
    public void ChangeScene(string sceneName)
    {
        Debug.Log("Changing Scene to " + sceneName);
        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// Método que se ejecuta al presionar el botón Continuar en correctoMenu e incorrectoMenu
    /// </summary>
    public void Continuar()
    {
        coins_panel.SetActive(false);
        correctoMenu.SetActive(false);
        incorrectoMenu.SetActive(false);
        ClearAll();

        if (_aciertos >= 3)
        {
            Debug.Log("Activa menú para subir nivel");
            soundEffects.clip = subeNivelSound;
            soundEffects.Play();
            mascota.GetComponent<Animator>().Rebind();
            mascota.GetComponent<Animator>().SetInteger("Estado", 0); //celebrando
            coins_panel.SetActive(true);
            coins_text.text = "¡+" + _NeurocoinsRecibidasAlSubirDeNivel + " Neurocoins!";
            ActualizaNeurocoins(_NeurocoinsRecibidasAlSubirDeNivel);
            if (_levelManager.Level >= 10)
                felicidadesMenu.SetActive(true);
            else
                subesNivelMenu.SetActive(true);
        } 
        else if (_fallos >= 3)
        {
            Debug.Log("Activa menú bajar de nivel");
            soundEffects.clip = bajaNivelSound;
            soundEffects.Play();
            coins_panel.SetActive(true);
            coins_text.text = "¡No te rindas!";
            mascota.GetComponent<Animator>().Rebind();
            mascota.GetComponent<Animator>().SetInteger("Estado", 2); //triste
            if (_levelManager.Level <= 1)
                repiteNivel1Menu.SetActive(true);
            else
                bajasNivelMenu.SetActive(true);
        }
        else
            PrepararEjercicio();
    }

    /// <summary>
    /// Método que se ejecuta al presionar el botón Continuar en subesNivelMenu, bajasNivelMenu, felicidadesMenu y repiteMenu
    /// </summary>
    public void CambiarNivel()
    {
        subesNivelMenu.SetActive(false);
        bajasNivelMenu.SetActive(false);
        felicidadesMenu.SetActive(false);
        repiteNivel1Menu.SetActive(false);
        coins_panel.SetActive(false);

        if (_aciertos >= 3)
        {
            Debug.Log("Sube nivel");
            if (_levelManager.Level < 10)
                _levelManager.Level += 1;

            _aciertos = 0;
            _fallos = 0;
        }
        else if (_fallos >= 3)
        {
            Debug.Log("Baja nivel");
            if (_levelManager.Level > 1)
                _levelManager.Level -= 1;

            _aciertos = 0;
            _fallos = 0;
            
        }
        UpdateText();
        PrepararEjercicio();
    }

    private void UpdateText()
    {
        levelText.text = _levelManager.Level.ToString();
        aciertosText.text = _aciertos.ToString();
        fallosText.text = _fallos.ToString();
    }

    private void ClearAll()
    {
        // Desactiva los objetos
        Clear(posObjsMochila);
        Clear(posObjsDebeLlevar);
        Clear(posObjsNoDebeLlevar);
        Destroy(posPersona.GetChild(0).gameObject);
        demoraFallosText.gameObject.SetActive(false);

        // Desactiva los resaltados
        if (_posFaltan.Any())
        {
            foreach (int pos in _posFaltan)
                resaltadoDebeLlevar[pos].enabled = false;
            _posFaltan.Clear();
        }
        if (_posNoLlevar.Any())
        {
            foreach (int pos in _posNoLlevar)
                resaltadoNoDebeLlevar[pos].enabled = false;
            _posNoLlevar.Clear();
        }
        if (_posMochilaNoLlevar.Any())
        {
            foreach (int pos in _posMochilaNoLlevar)
                resaltadoMochila[pos].enabled = false;
            _posMochilaNoLlevar.Clear();
        }
    }

    private void Clear(Transform[] positions)
    {
        for (int i=0; i<positions.Length; i++)
        {
            if (positions[i].childCount > 1)
                Destroy(positions[i].GetChild(0).gameObject);
            else
                break;
        }
    }

    private void DrawObjectOnPos(GameObject obj, Transform pos)
    {
        GameObject instance = GameObject.Instantiate(obj);
        instance.transform.SetParent(pos, false);
        instance.transform.SetSiblingIndex(0); // Lo coloca detrás de la imagen de resaltado
    }

    /// <summary>
    /// Dibujar los objetos con los índices en <c>indexes</c> en el arreglo de Transform
    /// </summary>
    /// <param name="indexes"> Lista de índices de <c>objetos</c> </param>
    /// <param name="positions"> Arreglo de Transform donde se dibujarán los objetos comenzando desde el Transfom 0
    ///     hasta <c>indexes.Count</c>. Puede ser <c>posObjsMochila</c>, <c>posObjsDebeLlevar</c> o <c>posObjsNoDebeLlevar</c>.
    /// </param>
    private void DrawListObjects(List<int> indexes, Transform[] positions)
    {
        for (int i = 0; i < indexes.Count; i++)
            DrawObjectOnPos(objetos[indexes[i]], positions[i]);
    }

    private void DrawRandomPerson()
    {
        int i = Random.Range(0, personas.Length);
        DrawObjectOnPos(personas[i], posPersona);
    }

    /// <summary>
    /// Regresa un tipo de ejercicio elegido al azar dependiendo de los pesos indicatos. Se debe cumplir que
    /// probA + probRF + probRO = 1.0
    /// </summary>
    private Ejercicio GetRandomEjercicioWithWeights(float probA, float probRF, float probRO)
    {
        float n = Random.Range(0.0f, 1.0f);

        if (n <= probA)
            return Ejercicio.Aceptar;
        else if (n <= probA + probRF)
            return Ejercicio.RechazarFaltan;
        else
            return Ejercicio.RechazarNoLlevar;
    }

    private void PrepararEjercicio ()
    {
        var probs = _levelManager.GetProbs();
        _tipoEjercicioActual = GetRandomEjercicioWithWeights(probs.probA, probs.probRF, probs.probRO);
        List<int> objsEnMochila = new List<int>(); // índices de objetos que se dibujarán en Mochila
        List<int> objsEnDebeLlevar = new List<int>(); // índices de objetos que se dibujarán en Debe Llevar
        List<int> objsEnNoLlevar = new List<int>(); // índices de objetos que se dibujarán en No Debe Llevar
        List<int> objsFaltan = new List<int>(); // índices de objetos en Debe Llevar que faltan en Mochila
        List<int> objsNoLlevar = new List<int>(); // índices de objetos en No Debe Llevar que están en Mochila

        // Llena la Mochila con objetos al azar
        RandomUtils.FillWithRandomInts(objsEnMochila, _levelManager.GetNumObjsMochila(), objetos.Length);

        switch (_tipoEjercicioActual)
        {
        case Ejercicio.Aceptar:
            Debug.Log("Ejercicio Aceptar");
            // Llena Debe Llevar con objetos en la mochila
            RandomUtils.FillWithRandomFrom(objsEnDebeLlevar, _levelManager.GetNumObjsDebeLlevar(), objsEnMochila);
            // Llena No Debe Llevar con objetos que no están en la mochila
            RandomUtils.FillWithRandomExclude(objsEnNoLlevar, _levelManager.GetNumObjsNoLlevar(), objsEnMochila, objetos.Length);
            break;

        case Ejercicio.RechazarFaltan:
            Debug.Log("Ejercicio Faltan Objetos");
            // Llena Debe Llevar con objetos en la mochila y 1 o 2 objetos que no están es la mochila
            var debeLlevar = _levelManager.GetDebeLlevarRF();
            RandomUtils.FillWithRandomFrom(objsEnDebeLlevar, debeLlevar.lleva, objsEnMochila);
            objsFaltan = RandomUtils.FillWithRandomExclude(objsEnDebeLlevar, debeLlevar.faltan, objsEnMochila, objetos.Length);
            // Llena No Debe Llevar con objetos que no están en la Mochila ni en Debe Llevar
            var union = objsEnMochila.Union(objsEnDebeLlevar);
            RandomUtils.FillWithRandomExclude(objsEnNoLlevar, _levelManager.GetNumObjsNoLlevar(), union.ToList<int>(), objetos.Length);
            break;

        case Ejercicio.RechazarNoLlevar:
            Debug.Log("Ejercicio Hay Objetos No Llevar");
            // Llena Debe Llevar con objetos de la mochila
            var noLlevar = _levelManager.GetNoLlevarRO();
            RandomUtils.FillWithRandomFrom(objsEnDebeLlevar, _levelManager.GetNumObjsDebeLlevar(), objsEnMochila);
            // Llena No Debe Llevar con objetos que no están en la mochila y 1 o 2 objetos que están en la mochila
            RandomUtils.FillWithRandomExclude(objsEnNoLlevar, noLlevar.noLleva, objsEnMochila, objetos.Length);
            var diff = objsEnMochila.Except(objsEnDebeLlevar);
            objsNoLlevar = RandomUtils.FillWithRandomFrom(objsEnNoLlevar, noLlevar.lleva, diff.ToList<int>());
            break;
        }

        RandomUtils.Shuffle(objsEnDebeLlevar);
        RandomUtils.Shuffle(objsEnNoLlevar);

        foreach (int obj in objsFaltan)
        {
            _posFaltan.Add(objsEnDebeLlevar.IndexOf(obj));
        }
        foreach (int obj in objsNoLlevar)
        {
            _posNoLlevar.Add(objsEnNoLlevar.IndexOf(obj));
            _posMochilaNoLlevar.Add(objsEnMochila.IndexOf(obj));
        }

        DrawListObjects(objsEnMochila, posObjsMochila);
        DrawListObjects(objsEnDebeLlevar, posObjsDebeLlevar);
        DrawListObjects(objsEnNoLlevar, posObjsNoDebeLlevar);
        DrawRandomPerson();

        // Iniciamos la corutina
        coroutineFallarPorTiempo = FallarPorTiempo();
        StartCoroutine(coroutineFallarPorTiempo);
    }

    IEnumerator FallarPorTiempo()
    {
        float waitSeconds = segundosDeEsperaInicial + (segundosAdicionalesCadaNivel * (_levelManager.Level - 1));
        yield return new WaitForSeconds(waitSeconds);

        Debug.Log("Fallo por demorar mucho tiempo");
        _fallos++;
        _aciertos = 0;
        GuardaPartida(false, true);
        incorrectoMenu.SetActive(true);
        demoraFallosText.gameObject.SetActive(true);
        switch (_tipoEjercicioActual)
        {
            case Ejercicio.Aceptar:
                ejercicioFallidoText.text = "Tenías que dejarlo pasar";
                break;
            case Ejercicio.RechazarFaltan:
                foreach (int pos in _posFaltan)
                    resaltadoDebeLlevar[pos].enabled = true;
                ejercicioFallidoText.text = "Faltan objetos en la mochila";
                break;
            case Ejercicio.RechazarNoLlevar:
                foreach (int pos in _posNoLlevar)
                    resaltadoNoDebeLlevar[pos].enabled = true;
                foreach (int pos in _posMochilaNoLlevar)
                    resaltadoMochila[pos].enabled = true;
                ejercicioFallidoText.text = "Hay objetos que no debe llevar en la mochila";
                break;
        }
        UpdateText();
    }

    public void GuardaPartida(bool victoria, bool agoto_tiempo)
    {
        Partida p = new Partida();
        p.nivel = level;
        p.juego = "Mochilas por favor";
        p.fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        p.victoria = victoria;
        p.agoto_tiempo = agoto_tiempo;
        StartCoroutine(AduanaCITAN.SubePartidaA_CITAN(p));
    }

    public void ActualizaNeurocoins(int coins)
    {
        StartCoroutine(AduanaCITAN.ActualizaNeurocoins_CITAN(coins));
    }
}