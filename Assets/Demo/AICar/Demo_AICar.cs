using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HeartOfTheMachine;

public class Demo_AICar : MonoBehaviour
{
    public AICar prefab;
    public Transform spawnPos;
    public int spawnNum = 50;
    public int rayNum = 20;
    public int rayDistance = 10;
    public NeuralNetwork bestNetwork;
    public float maxDistance;
    public float minSpeed = 0.1f;
    public float timeScale;
    public float variantCoef = 0.1f;
    private float _baseVariantCoef;
    public float speedFactor = 5;
    public float angularSpeedFactor = 50;
    private bool _newRecord;

    // Start is called before the first frame update
    void Start()
    {
        _baseVariantCoef = variantCoef;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            bestNetwork.Save(Application.dataPath + "/Demo", "AICar");
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            bestNetwork = NeuralNetwork.Load(Application.dataPath + "/Demo/AICar");
        }
    
        if (timeScale != Time.timeScale)
        {
            Time.timeScale = timeScale;
        }
        if (AICar.AllPlayers.Count == 0)
        {
            Spawn();
        }
    }

    private void Spawn()
    {
        if (_newRecord) variantCoef = _baseVariantCoef;
        else variantCoef += 0.01f;
        for (int i = 0; i < spawnNum; ++i)
        {
            var p = Instantiate(prefab);
            p.transform.position = spawnPos.position;
            p.transform.rotation = spawnPos.rotation;
            var network = bestNetwork;
            if (bestNetwork == null)
            {
                network = CreateNewNetwork();
            }
            else
            {
                network = bestNetwork.Variant(-variantCoef, variantCoef);
            }
            p.Init(network, this);
        }

        _newRecord = false;
    }

    private NeuralNetwork CreateNewNetwork()
    {
        //先创建一个神经网络
        var network = new NeuralNetwork();
        //创建一个全连接层，输出形状为(1, 10, 1)，激活函数为Sigmoid
        //var d1 = new DenseLayer(new Shape(1, 10, 1), ActivationType.Sigmoid);
        //network.AddLayer(d1);
        //创建一个全连接层，输出形状为(1, 2, 1)，注意这里是最后一层，输入形状要和你需要的输出对应，激活函数为Sigmoid
        var d2 = new DenseLayer(new Shape(1, 2, 1), ActivationType.Sigmoid);
        network.AddLayer(d2);
        
        //初始化参数
        var initArgs = new NetworkInitializeArgs();
        //输入形状 你的输入数据形状，这里是小车的demo，输入为射线的数量
        initArgs.inputShape = new Shape(1, rayNum, 1);
        //权重的初始化范围 weight = Random.Range(-0.1f, 0.1f)
        initArgs.initWeightRange = (-0.1f, 0.1f);
        //偏执项的初始化范围 bias = Random.Range(-0.1f, 0.1f)
        initArgs.initBiasRange = (-0.1f, 0.1f);
        //初始化
        network.Initialize(initArgs);
        return network;
    }

    public void SetNewRecord(NeuralNetwork network, float distance)
    {
        _newRecord = true;
        bestNetwork = network;
        maxDistance = distance;
    }
}
