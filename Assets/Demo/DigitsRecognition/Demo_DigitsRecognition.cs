using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using HeartOfTheMachine;
using UnityEngine;
using UnityEngine.UI;

public class Demo_DigitsRecognition : MonoBehaviour
{
    private const string TRAINING_DATA_FILE_PATH = "Demo/DigitsRecognition/train-images.idx3-ubyte";
    private const string TRAINING_LABELS_FILE_PATH = "Demo/DigitsRecognition/train-labels.idx1-ubyte";
    public int maxTrainingNum = 100;
    
    
    // Start is called before the first frame update
    void Start()
    {
        var network = new NeuralNetwork();
        var d1 = new DenseLayer(new Shape(1, 10, 1), ActivationType.Sigmoid);
        network.AddLayer(d1);
        
        var initArgs = new NetworkInitializeArgs();
        initArgs.inputShape = new Shape(1, 28 * 28, 1);
        initArgs.initWeightRange = (-0.1f, 0.1f);
        initArgs.initBiasRange = (-0.1f, 0.1f);
        network.Initialize(initArgs);
        
        var trainArgs = new NeuralNetworkTrainArgs();
        trainArgs.trainingData = ReadTrainingData();//设置数据
        trainArgs.trainingLabels = ReadTrainingLabels();//设置标签
        trainArgs.learningRate = 0.01f;//设置学习速率，越大学习的速度越快，但出现不收敛的可能性也越大
        trainArgs.onOnceEpoch = (i) =>
        {
            var accuracy = GetAccuracy(network, trainArgs.trainingData, trainArgs.trainingLabels);
            Debug.Log($"第{i}个训练回合, 准确率:{accuracy}");
        };
        trainArgs.trainEpoches = 100;//设置训练的回合数
        network.Train(trainArgs);//开始训练
        
        TestNetwork(network, trainArgs.trainingData, trainArgs.trainingLabels);
    }

    public RawImage image;
    public Text text;
    private Texture2D _tex;

    private async void TestNetwork(NeuralNetwork network, Tensor[] data, Tensor[] labels)
    {
        for (int i = 0; i < data.Length; ++i)
        {
            var predict = network.Forward(data[i]);
            var predictNum = MaxArgs(predict);
            var realNum = MaxArgs(labels[i]);

            text.text = $"预测值为:{predictNum}";
            ShowDigit(data[i]);

            await Task.Delay(TimeSpan.FromSeconds(2));
        }
    }

    private void ShowDigit(Tensor data)
    {
        if(_tex == null) _tex = new Texture2D(28, 28);
        for (int i = 0; i < 28; ++i)
        {
            for (int j = 0; j < 28; ++j)
            {
                float f = 1 - data[0, i * 28 + j, 0];
                _tex.SetPixel(j, 27 - i, new Color(f, f, f,1));
            }
        }
        _tex.Apply();
        image.texture = _tex;
    }

    private float GetAccuracy(NeuralNetwork network, Tensor[] data, Tensor[] labels)
    {
        int correct = 0;
        for (int i = 0; i < data.Length; ++i)
        {
            var predict = network.Forward(data[i]);
            var predictNum = MaxArgs(predict);
            var realNum = MaxArgs(labels[i]);
            if (predictNum == realNum)
            {
                ++correct;
            }
        }
        return (float)correct / data.Length;
    }

    private int MaxArgs(Tensor t)
    {
        var max = t[0,0,0];
        var index = 0;
        for (int i = 1; i < t.Shape.len1; ++i)
        {
            var v = t[0, i, 0];
            if (v > max)
            {
                max = v;
                index = i;
            }
        }

        return index;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private int Revert(int x)
    {
        var p1 = x & 0xff;
        var p2 = (x >> 8) & 0xff;
        var p3 = (x >> 16) & 0xff;
        var p4 = (x >> 24) & 0xff;

        return p4 + (p3 << 8) + (p2 << 16) + (p1 << 24);
    }

    Tensor[] ReadTrainingLabels()
    {
        var fs = new FileStream(Application.dataPath + "/" + TRAINING_LABELS_FILE_PATH, FileMode.Open);
        var br = new BinaryReader(fs);
        var magicNum = Revert(br.ReadInt32());
        var imgNum = Mathf.Min(maxTrainingNum, Revert(br.ReadInt32()));
        
        var trainingLabels = new Tensor[imgNum];

        Debug.LogError(magicNum +"  " + imgNum);
        for (int i = 0; i < imgNum; ++i)
        {
            int label = br.ReadByte();
            float[] array = new float[10];
            array[label] = 1;
            trainingLabels[i] = new Tensor(array);
        }
        br.Close();
        return trainingLabels;
    }
    
    Tensor[] ReadTrainingData()
    {
        var fs = new FileStream(Application.dataPath + "/" + TRAINING_DATA_FILE_PATH, FileMode.Open);
        var br = new BinaryReader(fs);
        var magicNum = Revert(br.ReadInt32());
        var imgNum = Mathf.Min(maxTrainingNum, Revert(br.ReadInt32()));
        var row = Revert(br.ReadInt32());
        var col = Revert(br.ReadInt32());
        
        var trainingData = new Tensor[imgNum];

        Debug.LogError(magicNum +"  " + imgNum + "  " + row + "  " + col);
        for (int i = 0; i < imgNum; ++i)
        {
            float[] array = new float[row * col];
            for (int j = 0; j < row * col; ++j)
            {
                var alpha = br.ReadByte();
                array[j] = (float)alpha / 255;
            }
            trainingData[i] = new Tensor(array);
        }
        br.Close();
        return trainingData;
    }
}
