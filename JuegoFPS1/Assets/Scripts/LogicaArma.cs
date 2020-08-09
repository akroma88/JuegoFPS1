using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ModoDeDisparo
{
    SemiAuto,
    FullAuto
}

public class LogicaArma : MonoBehaviour
{
    protected Animator animator;
    protected AudioSource audioSource;
    public bool tiempoNoDisparo = false;
    public bool puedeDisparar = false;
    public bool recargando = false;

    [Header("Referencia de Objectos")]
    public ParticleSystem fuegoDeArma;
    public Camera camaraPrincipal;
    public Transform puntoDisparo;
    public GameObject efectoDanoPrefab;

    [Header("Referencia de Sonido")]
    public AudioClip sonidoDisparo;
    public AudioClip sonidoSinBalas;
    public AudioClip sonidoCartuchoEntra;
    public AudioClip sonidoCartuchoSale;
    public AudioClip sonidoVacio;
    public AudioClip sonidoDesenfudndar;

    [Header("Atriburos del arma")]
    public ModoDeDisparo modoDeDisparo = ModoDeDisparo.FullAuto;
    public float dano = 20f;
    public float ritoDeDisparo = 0.3f;
    public int balasRestantes;
    public int balasEnCartucho;
    public int tamanoDeCartucho = 12;
    public int maximoDeBalas = 100;
    public bool estaADS = false;
    public Vector3 disCadera;
    public Vector3 ADS;
    public float tiempoApuntar;
    public float zoom;
    public float normal;



    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();

        balasEnCartucho = tamanoDeCartucho;
        balasRestantes = maximoDeBalas;

        Invoke("habilitarArma", 0.5f);

    }

    // Update is called once per frame
    void Update()
    {
        // comportamientos armas
        if (modoDeDisparo == ModoDeDisparo.FullAuto && Input.GetButton("Fire1"))
        {
            revisarDisparo();
        }
        else if (modoDeDisparo == ModoDeDisparo.SemiAuto && Input.GetButtonDown("Fire1"))
        {
            revisarDisparo();
        }

        if (Input.GetButtonDown("Reload"))
        {
            revisarRecargar();
        }


        
        if (Input.GetMouseButton(1))
        {
            transform.localPosition = Vector3.Slerp(transform.localPosition, ADS, tiempoApuntar * Time.deltaTime);
            estaADS = true;
            camaraPrincipal.fieldOfView = Mathf.Lerp(camaraPrincipal.fieldOfView, zoom, tiempoApuntar * Time.deltaTime);
        }

        if (Input.GetMouseButtonUp(1))
        {
            estaADS = false;
        }

        if (estaADS == false)
        {
            transform.localPosition = Vector3.Slerp(transform.localPosition, disCadera, tiempoApuntar * Time.deltaTime);
            camaraPrincipal.fieldOfView = Mathf.Lerp(camaraPrincipal.fieldOfView, normal, tiempoApuntar * Time.deltaTime);
        }
        
    }

    void habilitarArma()
    {
        puedeDisparar = true;
    }

    void revisarDisparo()
    {
        if (!puedeDisparar) return;
        if (tiempoNoDisparo) return;
        if (recargando) return;
        if (balasEnCartucho > 0)
        {
            disparar();
        }
        else
        {
            sinBalas();
        }
    }

    void disparar()
    {
        audioSource.PlayOneShot(sonidoDisparo);
        tiempoNoDisparo = true;
        fuegoDeArma.Stop();
        fuegoDeArma.Play();
        reproducirAnimacionDisparo();
        balasEnCartucho--;
        StartCoroutine(ReiniciarTiempoNoDisparo());

        //disparoDirecto();
    }

    public void crearEfectoDano(Vector3 pos, Quaternion rot)
    {
        GameObject efectoDano = Instantiate(efectoDanoPrefab, pos, rot);
        Destroy(efectoDano, 1f);
    }
    /*
    void disparoDirecto()
    {
        RaycastHit hit;
        if (Physics.Raycast(puntoDisparo.position, puntoDisparo.forward, out hit))
        {
            if (hit.transform.CompareTag("Enemigo"))
            {
                Vida vida = hit.transform.GetComponent<Vida>();
                if (vida == null)
                {
                    throw new System.Exception("No se encontro el componente de Vida del Enemigo");
                }
                else
                {
                    vida.recibirDano(dano);
                    crearEfectoDano(hit.point, hit.transform.rotation);
                }
            }
        }
    }
    */

    public virtual void reproducirAnimacionDisparo()
    {
        if (gameObject.name == "Policy9mm")
        {
            if (balasEnCartucho > 1)
            {
                animator.CrossFadeInFixedTime("Fire", 0.1f);
            }
            else
            {
                animator.CrossFadeInFixedTime("FireLast", 0.1f);
            }
        }
        else
        {
            animator.CrossFadeInFixedTime("Fire", 0.1f);
        }
    }

    void sinBalas()
    {
        audioSource.PlayOneShot(sonidoSinBalas);
        tiempoNoDisparo = true;
        StartCoroutine(ReiniciarTiempoNoDisparo());
    }

    IEnumerator ReiniciarTiempoNoDisparo()
    {
        yield return new WaitForSeconds(ritoDeDisparo);
        tiempoNoDisparo = false;
    }

    void revisarRecargar()
    {
        if (balasRestantes > 0 && balasEnCartucho < tamanoDeCartucho)
        {
            recargar();
        }
    }

    void recargar()
    {
        if (recargando) return;
        recargando = true;
        animator.CrossFadeInFixedTime("Reload", 0.1f);
    }

    void recargarMuniciones()
    {
        int balasParaRecarfar = tamanoDeCartucho - balasEnCartucho;
        int restarBalas = (balasRestantes >= balasParaRecarfar) ? balasParaRecarfar : balasRestantes;
        balasRestantes -= restarBalas;
        balasEnCartucho += balasParaRecarfar;
    }

    public void desenfundarOn()
    {
        audioSource.PlayOneShot(sonidoDesenfudndar);
    }

    public void cartuchoEntraOn()
    {
        audioSource.PlayOneShot(sonidoCartuchoEntra);
        recargarMuniciones();
    }

    public void cartuchoSaleOn()
    {
        audioSource.PlayOneShot(sonidoCartuchoSale);
    }

    public void vacioOn()
    {
        audioSource.PlayOneShot(sonidoVacio);
        Invoke("reiniciarRecargar", 0.1f);
    }

    void reiniciarRecargar()
    {
        recargando = false;
    }
}
