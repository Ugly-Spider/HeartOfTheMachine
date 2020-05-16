# HeartOfTheMachine
机器学习脚本：目前只有全连接层的神经网络，可以使用演化算法和BP算法训练AI。

## 使用方式
### 1.创建
```c#
        //先创建一个神经网络
        var network = new NeuralNetwork();
        //创建一个全连接层，输出形状为(1, 10, 1)，激活函数为Sigmoid
        var d1 = new DenseLayer(new Shape(1, 10, 1), ActivationType.Sigmoid);
        network.AddLayer(d1);
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
```

### 2.训练

一种方式是通过演化算法，在原有网络的基础上进行一次变异，变异系数越大，新网络的表现与原来的差异越大，只要设置合理的规则让更好的网络更容易'存活'下来即可，涉及的主要代码是`var newNetwork = network.Variant(-0.1f, 0.1f);`。

另一种方式是通过BP算法实现，同时提供数据和正确答案，然后根据偏差修正权重。
```c#
        var trainArgs = new NeuralNetworkTrainArgs();
        trainArgs.trainingData = ReadTrainingData();//设置数据
        trainArgs.trainingLabels = ReadTrainingLabels();//设置标签
        trainArgs.learningRate = 0.01f;//设置学习速率，越大学习的速度越快，但出现不收敛的可能性也越大
        trainArgs.trainEpoches = 100;//设置训练的回合数
        network.Train(trainArgs);//开始训练
```

### 3.获取输出
```c#
        var predict = network.Forward(input);
```

## 小车Demo
该demo属于非监督性学习，通过设置合理的规则，促使神经网络不断进化。
- 设置好赛道，根据`小车周围的的碰撞体距离`作为输入，输入一个`二维向量`，分别作为`速度`和`角速度`，来控制小车的前进方式。
- 每次小车撞毁时判断是否刷新了`最远距离`的记录，如果是，则记录当前小车的神经网络为`最佳网络`，后续生成的小车在`最佳网络`的基础上进行`变异`。
- 为了加速训练，多次未刷新记录会提高`变异系数`，刷新后重置。

第一回合：
第一个回合系数完全是随机的，小车行驶毫无规律
![epoch0](https://github.com/Ugly-Spider/HeartOfTheMachine/blob/master/Gifs/AICar_Epoch_0.gif)

第二回合：
第二回合生成的小车是在第一回合中`最佳小车`的基础上进行了一些变化，有些变得更“强”，有些则变得更“弱”，“弱”的会被淘汰掉，而“强”的会继续进化。
![epoch1](https://github.com/Ugly-Spider/HeartOfTheMachine/blob/master/Gifs/AICar_Epoch_1.gif)

第三回合：似乎没什么变化。
![epoch2](https://github.com/Ugly-Spider/HeartOfTheMachine/blob/master/Gifs/AICar_Epoch_2.gif)

后面连续N回合：这个弯道卡了好久，看起来每次小车都撞毁在同一地方，但其实是有变化的，每次都能比前一次走得更远一点。
![epoch3](https://github.com/Ugly-Spider/HeartOfTheMachine/blob/master/Gifs/AICar_Epoch_3.gif)

第N回合：能跑完前面的弯道了。
![epoch4](https://github.com/Ugly-Spider/HeartOfTheMachine/blob/master/Gifs/AICar_Epoch_4.gif)

最后：
终于能完整的跑完一圈了!
![epoch5](https://github.com/Ugly-Spider/HeartOfTheMachine/blob/master/Gifs/AICar_Epoch_5.gif)


## Mnist手写数字识别Demo
该demo属于监督性学习，需要同时提供数据和正确答案，训练时，神经网络会根据数据计算出一个估计的结果，然后用这个结果和正确答案做对比，再根据偏差的情况来更新权重。

Mnist的数据为28*28像素的黑白颜色值，我们把它作为神经网络的输入，输入则是1个10维向量，每一项代表是该数字的可能性，如[0, 0.2, 0.1, 0.1, 0, 0.8, 0.2, 0.1, 0.2, 0.1]表示输入为0的可能性为0，1的可能性为0.2, 2的可能性为0.1...，因为索引5对应的概率最大，该向量就代表数字5。
为了提高训练速度，我这里仅使用了前100个数据（原数据共60000个，全部训练完成至少需要几个小时），而且测试准确率时也采用了和训练一样的数据，目的是验证代码是否正确。

经过100个回合的训练，正确率已经能达到100%了，证明代码ok。

![accuracy](https://github.com/Ugly-Spider/HeartOfTheMachine/blob/master/Gifs/DigitsRecognition.gif)
