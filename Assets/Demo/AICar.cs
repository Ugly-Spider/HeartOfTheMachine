using System;
using System.Collections;
using System.Collections.Generic;
using HeartOfTheMachine;
using UnityEngine;

public class AICar : MonoBehaviour
{
    public static List<AICar> AllPlayers = new List<AICar>();

    public NeuralNetwork network;
    public float speed;
    public float angularSpeed;
    private Demo_AICar _demoAiCar;
    public bool drawRay;

    private float _distance;

    void Awake()
    {
        AllPlayers.Add(this);
    }

    void OnDestroy()
    {
        if (_distance > _demoAiCar.maxDistance)
        {
            _demoAiCar.SetNewRecord(network, _distance);
        }
        AllPlayers.Remove(this);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    public void Init(NeuralNetwork network, Demo_AICar demoAiCar)
    {
        this.network = network;
        _demoAiCar = demoAiCar;
    }

    private Tensor GetInputData()
    {
        var array = new float[_demoAiCar.rayNum];
        for (int i = 0; i < _demoAiCar.rayNum; ++i)
        {
            var angle = i * (360 / _demoAiCar.rayNum);
            var quater = Quaternion.Euler(0, angle, 0);
            var direction = quater * transform.forward;
            var ray = Physics.Raycast(transform.position, direction, out var hit, _demoAiCar.rayDistance, ~(1 << 9));
            if(drawRay) Debug.DrawLine(transform.position, transform.position + direction * _demoAiCar.rayDistance);
            if (hit.collider != null)
            {
                array[i] = Vector3.Distance(transform.position, hit.point) / _demoAiCar.rayDistance;
            }
            else
            {
                array[i] = 0;
            }
        }
        var input = new Tensor(array);
        return input;
    }

    // Update is called once per frame
    void Update()
    {
        var input = GetInputData();
        var predict = network.Forward(input);
        predict.TryAsArray(out var result);
        speed = Mathf.Clamp(result[0] * 5 - 2, _demoAiCar.minSpeed, 1);
        angularSpeed = result[1] - 0.5f;

        transform.position += transform.forward * speed * Time.deltaTime * _demoAiCar.speedFactor;
        transform.Rotate(Vector3.up, angularSpeed * Time.deltaTime * _demoAiCar.angularSpeedFactor);

        _distance += speed * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision other)
    {
        Destroy(gameObject);
    }
}
