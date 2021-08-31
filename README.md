# 🔥 Use unity3d to connect wind speed sensor through Modbus485 protocol

Awaiting solution：

- [ ] The response speed of the wind speed sensor is very slow

Maybe a hardware problem...

## ⭐ First step：Hardware assembly

![windSensor](https://github.com/JpHoooo/unity-modbus485-windsensor/blob/master/Preview/windSensor.png)  

![usbTo485](https://github.com/JpHoooo/unity-modbus485-windsensor/blob/master/Preview/usbTo485.png)  

## ⭐ Second step：Install 340h driver

[Download link](http://www.wch.cn/downloads/CH341SER_EXE.html)

After installing the driver to stop vomiting and connecting the USB to the host computer, if the following icon appears in the device manager, it means the configuration is correct

![port](https://github.com/JpHoooo/unity-modbus485-windsensor/blob/master/Preview/port.png) 

## ⭐ Third step：Open the unity project & Modify the port in the code

See the red box in the picture below, please make changes according to the COM port on the picture in the second step 

![editor](https://github.com/JpHoooo/unity-modbus485-windsensor/blob/master/Preview/editor.png)  

## ⭐ Step 4: Run the project

The light of the scene will become stronger or weaker according to the wind speed value. The higher the wind speed, the greater the light brightness, and vice versa.

![stop2move](https://github.com/JpHoooo/unity-modbus485-windsensor/blob/master/Preview/stop2move.gif)

![move2stop](https://github.com/JpHoooo/unity-modbus485-windsensor/blob/master/Preview/move2stop.gif)
