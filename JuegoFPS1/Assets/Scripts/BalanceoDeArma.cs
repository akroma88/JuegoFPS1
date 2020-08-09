using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalanceoDeArma : MonoBehaviour
{
    public float cantidad;
    public float cantidadMaxima;
    public float tiempo;
    private
        Vector3 posIni;
    public bool seBalancea;
    // Start is called before the first frame update
    void Start()
    {
        posIni = transform.localPosition;   
    }

    // Update is called once per frame
    void Update()
    {
        seBalancea = true;
        float movX = Input.GetAxis("Mouse X") * cantidad;
        float movY = Input.GetAxis("Mouse Y") * cantidad;

        movX = Mathf.Clamp(movX, -cantidadMaxima, cantidadMaxima);
        movY = Mathf.Clamp(movX, -cantidadMaxima, cantidadMaxima);

        Vector3 posFinalMov = new Vector3(movX, movY, 0);

        if (Input.GetMouseButton(1)) {
            seBalancea = false;
        }

        if (seBalancea) {
            transform.localPosition = Vector3.Lerp(transform.localPosition, posFinalMov + posIni, tiempo * Time.deltaTime);
        }
    }
}
