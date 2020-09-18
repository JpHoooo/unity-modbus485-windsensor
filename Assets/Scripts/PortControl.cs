using UnityEngine;
using UnityEngine.UI;
using System.IO.Ports;
using System;
using System.Collections.Generic;
using System.Threading;

/// <summary>
/// 串口通信类，挂载至任意对象下即可使用，建议（Main Camera）
/// </summary>
public class PortControl : MonoBehaviour
{
    public Text speedText;
    public static string speed;

    #region 定义串口属性
    //定义基本信息
    public string portName = "USB-SERIAL CH340 (COM3)";//串口名
    public int baudRate = 9600;//波特率
    public Parity parity = Parity.None;//效验位
    public int dataBits = 8;//数据位
    public StopBits stopBits = StopBits.One;//停止位
    protected SerialPort sp = null;
    #endregion

    protected Thread dataReceiveThread;
    protected Thread dataProcessorThread;
    protected Thread sendDataThread;

    public List<byte> receive = new List<byte>();   //接收到的所有消息
    protected List<byte> message = new List<byte>();  //接收的一条消息
    protected static string[] text;                          //处理后的消息
    protected bool sendState = false;                 //接收状态
    protected bool readTextState = false;             //读取状态
    public bool ReadTextState
    {
        get
        {
            return readTextState;
        }
    }
    public bool SendState
    {
        get
        {
            return sendState;
        }
    }

    private void Awake()
    {
        dataReceiveThread = new Thread(new ThreadStart(DataReceiveFunction));
        dataProcessorThread = new Thread(new ThreadStart(DataProcessor));
        sendDataThread = new Thread(new ThreadStart(TestMyData));
    }
    private void Start()
    {
        OpenPort();
        
    }
    void Update()
    {
        speedText.text = speed;
    }



    #region 创建串口，并打开串口
    public virtual void OpenPort()
    {
        //创建串口
        sp = new SerialPort(portName, baudRate, parity, dataBits, stopBits);
        sp.ReadTimeout = 400;
        try
        {
          
            sp.Open();
            sendDataThread.Start();
            dataReceiveThread.Start();
            dataProcessorThread.Start();
            sendState = true;
            Debug.Log("串口开启！！！！");
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }
    #endregion

    #region 程序退出时关闭串口
    private void OnApplicationQuit()
    {
        ClosePort();
    }
    public void ClosePort()
    {
        try
        {
            sp.Close();
            sendDataThread.Abort();
            dataReceiveThread.Abort();
            dataProcessorThread.Abort();
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }
    #endregion

    /// <summary>
    /// 数据处理
    /// </summary>
    /// <returns></returns>
    public virtual void DataProcessor()
    {
        while (true)
        {
            if (receive.Count > 0)
            {
                if (receive[0] == id && readTextState == false)
                {
                    if (receive.Count >= 3)
                    {
                        int index = 0;
                        message.Add(receive[index++]);//message[0] 地址码
                        message.Add(receive[index++]);//message[0] 功能码
                        message.Add(receive[index++]);//message[0] 有效字节数
                       
                        switch (message[1])
                        {
                        
                            case 0x03:
                                int length = Convert.ToInt32(Convert.ToString(message[2], 10));
                                if (receive.Count >= length + 5)
                                {
                                    while (index < length + 3)
                                    {
                                        message.Add(receive[index++]);
                                    }
                                }
                                else
                                {
                                    goto case 0x03;
                                }
                                break;                       
                        }
                        message.Add(receive[index++]);//
                        message.Add(receive[index++]);//当前风速值
                        receive.RemoveRange(0, message.Count);
                        //Debug.Log("message.Count：" + message.Count);
                        byte[] data = new byte[message.Count - 2];
                        message.CopyTo(0, data, 0, data.Length);
                        uint crc16 = Crc16_Modbus(data, (uint)data.Length);
                        byte crcH = Convert.ToByte(crc16 & 0xFF);
                        byte crcL = Convert.ToByte(crc16 / 0x100);
                        //crc验证，如果通过，代表收到的数据无误，使用text接收，不通过就自动重发
                        if ((message[message.Count - 2].Equals(crcH) && message[message.Count - 1].Equals(crcL)) || (message[message.Count - 1].Equals(crcH) && message[message.Count - 2].Equals(crcL)))
                        {
                            text = new string[message.Count];
                            lock (text)
                            { 
                               
                                for (int i = 0; i < message.Count; i++)
                                {
                                    text[i] = Convert.ToString(message[i], 10);
                                   
                                }
                                if (text.Length>=4)
                                speed =text[4];
                                readTextState = true;
                                sendState = true;
                            }
                        }
                        else
                        {
                            WriteData(sendData);
                        }
                        message.Clear();
                    }
                }
                else
                {
                    receive.RemoveAt(0);            //*移除多余的数据*
                }
            }
            Thread.Sleep(20);
        }
    }
    /// <summary>
    /// 为外界提供获取数据的方法
    /// </summary>
    /// <returns>获取是否成功</returns>


    #region 接收数据
    private void DataReceiveFunction()
    {
        byte[] buffer = new byte[1];
        int bytes = 0;
        while (true)
        {
            lock (receive)
            {
                if (sp != null && sp.IsOpen)
                {
                    try
                    {
                        int tmp = sp.ReadBufferSize;
                        for (int i = 0; i < tmp; i++)
                        {
                            bytes = sp.Read(buffer, 0, 1);//接收字节
                            if (bytes == 0)
                            {
                                continue;
                            }
                            else
                            {
                                receive.Add(buffer[0]);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        if (ex.GetType() != typeof(ThreadAbortException))
                        {
                        }
                    }
                }
            }
            Thread.Sleep(20);//线程休眠200ms

        }
    }
    #endregion

    #region 发送数据
    private byte[] sendData;                                    //上一次发送的数据，暂存，用于通信失败后自动重发
    protected virtual void WriteData(byte[] dataStr)
    {
        if (sp.IsOpen)
        {
            sendState = false;
            readTextState = false;
            text = null;
            sp.Write(dataStr, 0, dataStr.Length);
            //Debug.Log("WriteData");
        }
    }
    /// <summary>
    /// 以Modbus协议发送数据
    /// </summary>
    /// <param name="id">从机地址</param>
    /// <param name="fc">功能码</param>
    /// <param name="addrH">寄存器地址高位或起始地址高位</param>
    /// <param name="addrL">寄存器地址低位或起始地址低位</param>
    /// <param name="dinumH">目标地址高位或线圈数高位或地址数高位</param>
    /// <param name="dinumL">目标地址低位或线圈数低位或地址数低位</param>
    /// <param name="value">传输的数据，可空，仅更新数据时用，首位表示数据长度</param>
    public void SendData(byte id, byte fc, byte addrH, byte addrL, byte dinumH, byte dinumL, params byte[] value)
    {
        sendData = new byte[0];
        uint crc16;
        switch (fc)
        {
            case 0x03:
                sendData = new byte[8];
                sendData[0] = id; //从机地址
                //Debug.Log("SendData第1位：" + sendData[0]);
                sendData[1] = fc; //功能码
                //Debug.Log("SendData第2位：" + sendData[1]);
                sendData[2] = addrH; //寄存器地址高字节
                sendData[3] = addrL; //寄存器地址低字节
                sendData[4] = dinumH; //读取的寄存器数高字节
                sendData[5] = dinumL;  //寄存器数低字节
                crc16 = Crc16_Modbus(sendData, 6);          
                sendData[6] = Convert.ToByte(crc16 & 0xFF);
                //Debug.Log("SendData第7位："+sendData[6]);
                sendData[7] = Convert.ToByte(crc16 / 0x100);
                //Debug.Log("SendData第8位：" + sendData[7]);
                break;       
        }
        this.id = sendData[0];
        WriteData(sendData);
    }
    #endregion

    public void TestMyData()
    {
        while (true)
        {
            if (sp != null && sp.IsOpen)
            {
                SendData(0x01, 0x03, 0x00, 0x00, 0x00, 0x01);
            }
            Thread.Sleep(200);//线程休眠200ms
        }
       
    }
    /// <summary>
    /// crc验证
    /// </summary>
    /// <returns></returns>
    private uint Crc16_Modbus(byte[] modebusdata, uint length) //length为modbusdata的长度
    {
        uint i, j;
        uint crc16 = 0xFFFF;

        for (i = 0; i < length; i++)
        {
            crc16 ^= modebusdata[i];  //CRC = BYTE xor CRC（^=取反）
            for (j = 0; j < 8; j++)
            {
                if ((crc16 & 0x01) == 1)  //如果CRC最后一位为1，右移一位后carry=1，则将CRC右移一位后，再与POLY16=0xA001进行xor运算
                {
                    crc16 = (crc16 >> 1) ^ 0xA001;
                }
                else
                {
                    crc16 = crc16 >> 1;   //如果CRC最后一位为0，则只将CRC右移一位
                }
            }
        }

        return crc16;
    }

    #region 发送协议封装
    public byte id = 0x01;      //从机地址
    public byte Id
    {
        get
        {
            return id;
        }
        set
        {
            id = value;
        }
    }
}
#endregion