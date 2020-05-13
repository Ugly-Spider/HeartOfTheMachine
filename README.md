# HeartOfTheMachine
机器学习脚本：目前只有全连接层的神经网络，可以使用演化算法和BP算法训练AI。

## 小车Demo
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
