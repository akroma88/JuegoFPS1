using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControladorArmas : MonoBehaviour
{
    public LogicaArma[] armas;
    public int indiceArmaActual = 0;
    // Start is called before the first frame update
    void Start()
    {
        /*armas[0].gameObject.SetActive(true);
        for (int i = 1; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        if (armas.Length > 0)
        {
            revisarCambioDeArmas();
        }
    }

    void cambiarArmaActual()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

        armas[indiceArmaActual].gameObject.SetActive(true);
    }

    void revisarCambioDeArmas()
    {
        float ruedaMouse = Input.GetAxis("Mouse ScrollWheel");
        if (ruedaMouse > 0f)
        {
            seleccionarArmaAnterior();
            armas[indiceArmaActual].recargando = false;
            armas[indiceArmaActual].tiempoNoDisparo = false;
            armas[indiceArmaActual].estaADS = false;
        }
        else if (ruedaMouse < 0f)
        {
            seleccionarArmaSiguiente();
            armas[indiceArmaActual].recargando = false;
            armas[indiceArmaActual].tiempoNoDisparo = false;
            armas[indiceArmaActual].estaADS = false;
        }
    }

    void seleccionarArmaAnterior()
    {
        if (indiceArmaActual == 0)
        {
            indiceArmaActual = armas.Length - 1;
        }
        else
        {
            indiceArmaActual--;
        }
        cambiarArmaActual();
    }

    void seleccionarArmaSiguiente()
    {
        if (indiceArmaActual >= (armas.Length - 1))
        {
            indiceArmaActual = 0;
        }
        else
        {
            indiceArmaActual++;
        }
        cambiarArmaActual();
    }
}
