# 🔥 Use unity3d to connect wind speed sensor through Modbus485 protocol

## ⭐ First step：Hardware assembly

![windSensor](https://jp-github.oss-cn-shenzhen.aliyuncs.com/unity-modbus485-windsensor/windSensor.png)  

![usbTo485](https://jp-github.oss-cn-shenzhen.aliyuncs.com/unity-modbus485-windsensor/usbTo485.png)  

## ⭐ Second step：Install 340h driver

[Download link](http://www.wch.cn/downloads/CH341SER_EXE.html)__

After installing the driver to stop vomiting and connecting the USB to the host computer, if the following icon appears in the device manager, it means the configuration is correct

![port](https://jp-github.oss-cn-shenzhen.aliyuncs.com/unity-modbus485-windsensor/port.png) 

## ⭐ Third step：Open the unity project & Modify the port in the code

See the red box in the picture below, please make changes according to the COM port on the picture in the second step 

![editor](https://jp-github.oss-cn-shenzhen.aliyuncs.com/unity-modbus485-windsensor/editor.png)  

## ⭐ Step 4: Run the project

The light of the scene will become stronger or weaker according to the wind speed value. The higher the wind speed, the greater the light brightness, and vice versa.

![stop2move](https://jp-github.oss-cn-shenzhen.aliyuncs.com/unity-modbus485-windsensor/stop2move.gif)

![move2stop](https://jp-github.oss-cn-shenzhen.aliyuncs.com/unity-modbus485-windsensor/move2stop.gif)
