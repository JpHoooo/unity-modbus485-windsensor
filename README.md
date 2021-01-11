# unity3d通过Modbus485协议连接风速传感器
***
# 第一步：硬件安装  
![windSensor](https://jp-github.oss-cn-shenzhen.aliyuncs.com/unity-modbus485-windsensor/windSensor.png)  
![usbTo485](https://jp-github.oss-cn-shenzhen.aliyuncs.com/unity-modbus485-windsensor/usbTo485.png)  
***
# 第二步：安装CH340驱动
CH340/CH341USB转串口WINDOWS驱动程序   
支持32/64位 Windows 10/8.1/8/7/VISTA/XP，SERVER 2016/2012/2008/2003，2000/ME/98  
__转跳到[下载链接](http://www.wch.cn/downloads/CH341SER_EXE.html)__
安装好驱动，把USB连接到电脑主机后，在设备管理器出现如下图示则表示配置正确  
![port](https://jp-github.oss-cn-shenzhen.aliyuncs.com/unity-modbus485-windsensor/port.png) 
***
# 第三步：打开工程文件，修改代码里的端口  
看到以下图片的红框，请根据第二步图片上的COM口进行更改  
![editor](https://jp-github.oss-cn-shenzhen.aliyuncs.com/unity-modbus485-windsensor/editor.png)  
***
# 第四步：运行工程
场景的灯光会根据风速值得大小变强变弱，风速越大，灯光亮度越大，反之亦然  
![stop2move](https://jp-github.oss-cn-shenzhen.aliyuncs.com/unity-modbus485-windsensor/stop2move.gif)
![move2stop](https://jp-github.oss-cn-shenzhen.aliyuncs.com/unity-modbus485-windsensor/move2stop.gif)
***
## 以上是简单操作流程，具体实操难度较为大，欢迎交流咨询
## 联系方式：86-18819301997（wechat/tel）
