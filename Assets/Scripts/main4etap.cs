//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//using System;
//using System.Runtime.InteropServices;
//using System.IO;
//using System.Text;

//using System.Net;
//using System.Net.Sockets;
//using System.Threading;

//using System.Runtime.Serialization;
//using System.Runtime.Serialization.Formatters.Binary;

//using newLib;

//public class main4etap : MonoBehaviour {

//    public GameObject addWindow;
//    public GameObject eventSyst;
//    public GameObject inputField;
//    public GameObject checkWindow;
//    public GameObject videoWindow;

//    // Use this for initialization
//    void Start () {
		
//	}
	
//	// Update is called once per frame
//	void Update () {
//        if(Input.GetKeyDown(KeyCode.Space))
//        {
//            //int a = 0, b = 0;
//            //try
//            //{
//            //    a = Convert.ToInt32(test.Substring(0, 2));
//            //    b = Convert.ToInt32(test.Substring(2, 2));              
//            //}
//            //catch(Exception e)
//            //{
//            //    Debug.Log("error: " + e);
//            //}
//            //finally
//            //{
//            //    Debug.Log(a + ":" + b);
//            //}
//        }
		
//	}

//    string Coding(int a, int b)
//    {
//        return a.ToString() + b.ToString();
//    }

//    VREtap Encoding(string code)
//    {
//        int a = 0, b = 0;
//        try
//        {
//            a = Convert.ToInt32(code.Substring(1, 1));
//            b = Convert.ToInt32(code.Substring(3, 1));
//        }
//        catch (Exception e)
//        {
//            Debug.Log("error: " + e);
//        }
//        finally
//        {
//            Debug.Log(a + ":" + b);
//        }
//        return new VREtap(a, b);
//    }

//    public void btnSend()
//    {
//        string code = inputField.GetComponent<InputField>().text;
        
//        bool flag = true;
//        try
//        {
//            int intCode = Convert.ToInt32(code);
//        }
//        catch(Exception e)
//        {
//            flag = false;
//        }
//        finally
//        {
//            if(flag && code.Length == 4)
//            {
//                addWindow.SetActive(true);
//                addWindow.GetComponent<addWindow4etap>().alarm.text = inputField.GetComponent<InputField>().text;
//            }
//            else
//            {
//                Debug.Log("Ошибка ввода кода");
//                checkWindow.SetActive(true);
//            }
//        }       
//    }

//    public void onBtnVideo()
//    {
//        videoWindow.SetActive(true);
//    }

//    public void main()
//    {
        
//        VREtap obj = Encoding(inputField.GetComponent<InputField>().text);
//        Filename f = new Filename(eventSyst.GetComponent<commonData>().filename);
//        Serialization(obj, eventSyst.GetComponent<commonData>().filename + "part4.dat");
//        byte[] bytes = Transport.ObjectToByteArray(obj);
//        byte[] dataType = System.Text.Encoding.ASCII.GetBytes("part4");
//        byte[] filename = Transport.ObjectToByteArray(f);
//        Connect(bytes,filename,dataType);
//    }

//    public void Connect(byte[] msg, byte[] filename, byte[] dataType)
//    {
//        try
//        {
//            String server = eventSyst.GetComponent<commonData>().ipadress;
//            Int32 port = eventSyst.GetComponent<commonData>().port;
//            TcpClient client = new TcpClient(server, port);
//            NetworkStream stream = client.GetStream();

//            stream.Write(dataType, 0, dataType.Length);

//            byte[] dataOut = new byte[32];
//            stream.Read(dataOut, 0, dataOut.Length);
//            int ok = BitConverter.ToInt32(dataOut, 0);
//            if (ok != 2)
//            {
//                return;
//            }

//            stream.Write(msg, 0, msg.Length);

//            dataOut = new byte[32];
//            stream.Read(dataOut, 0, dataOut.Length);
//            ok = BitConverter.ToInt32(dataOut, 0);
//            if (ok != 2)
//            {
//                return;
//            }

//            stream.Write(filename, 0, filename.Length);

//            client.Close();
//        }
//        catch (ArgumentNullException e)
//        {
//            Debug.Log("ArgumentNullException: " + e);
//            //eventSyst.GetComponent<commonData>().error = true;
//        }
//        catch (SocketException e)
//        {
//            Debug.Log("SocketException: " + e);
//            //eventSyst.GetComponent<commonData>().error = true;
//        }
//        finally
//        {
//        }
//    }
//    /////////////////////////////////////////////////////////////////////////

//    /////////////////////////////////////////////////////////////////////////
//    //public static byte[] ObjectToByteArray(System.Object obj)
//    //{
//    //    BinaryFormatter bf = new BinaryFormatter();
//    //    using (var ms = new MemoryStream())
//    //    {
//    //        bf.Serialize(ms, obj);
//    //        return ms.ToArray();
//    //    }
//    //}
//    //public static System.Object ByteArrayToObject(byte[] arrBytes)
//    //{
//    //    using (var memStream = new MemoryStream())
//    //    {
//    //        var binForm = new BinaryFormatter();
//    //        memStream.Write(arrBytes, 0, arrBytes.Length);
//    //        memStream.Seek(0, SeekOrigin.Begin);
//    //        System.Object obj = binForm.Deserialize(memStream);
//    //        return obj;
//    //    }
//    //}
//    /////////////////////////////////////////////////////////////////////////



//    /////////////////////////////////////////////////////////////////////////
//    public void Serialization(System.Object obj, string fileName)
//    {
//        FileStream fs = new FileStream(fileName, FileMode.Create);
//        BinaryFormatter formatter = new BinaryFormatter();
//        formatter.Serialize(fs, obj);
//        fs.Close();
//    }
//    public System.Object Deserialize(string fileName)
//    {
//        FileStream fs = new FileStream(fileName, FileMode.Open);
//        BinaryFormatter formatter = new BinaryFormatter();
//        return /*(VREtap)*/(Test)formatter.Deserialize(fs);
//    }
//    /////////////////////////////////////////////////////////////////////////
//}
