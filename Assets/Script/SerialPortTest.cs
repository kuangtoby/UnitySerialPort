using UnityEngine;
using System.Collections;
using System;
using System.Threading;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Text.RegularExpressions;
using System.Text;

public class SerialPortTest : MonoBehaviour
{


	private SerialPort spSend;
	private SerialPort spReceive;
	private Queue<string> queueDataPool;
	private Thread threadReceive;
	private Thread tPortDeal;
	private string strOutPool = string.Empty;
	string finalstring = string.Empty;
	string tempstring = string.Empty;
	// Use this for initialization 
	void Start ()
	{
		queueDataPool = new Queue<string> ();
		spSend = new SerialPort ("/dev/tty.usbserial-FT9LY4FI", 9600, Parity.None, 8, StopBits.One);
		if (!spSend.IsOpen) {
			spSend.Open ();
		}
		spReceive = spSend;
		//		spReceive  = new SerialPort("COM4", 9600, Parity.None, 8, StopBits.One);
		//		if (!spReceive.IsOpen) 
		//		{ 
		//			spReceive.Open(); 
		//		} 


		threadReceive = new Thread (ReceiveData);
		threadReceive.Start ();


		//		tPortDeal = new Thread(ReceiveData); 
		//		tPortDeal.Start(); 
	}


	// Update is called once per frame 
	void Update ()
	{
		//		if (!tPortDeal.IsAlive) 
		//		{ 
		//			tPortDeal = new Thread(ReceiveData); 
		//			tPortDeal.Start(); 
		//		} 
		//		if (!tPort.IsAlive) 
		//		{ 
		//			tPort = new Thread(DealData); 
		//			tPort.Start(); 
		//		} 


		//		if (spReceive.IsOpen) {
		//			ReceiveData ();
		//		}


		if (Input.GetKeyDown ("s")) {
			SendSerialPortData ("1234567890ggfgfdgsdfgsdfgsdfgafsdafwefawee90ggfgfdgsdfgsdfgs90ggfgfdgsdfgsdfgs90ggfgfdgsdfgsdfgs90ggfgfdgsdfgsdfgs");
		}
	}

	private void ReceiveData ()
	{

		while (true) {
			try {
				//								Debug.Log ("111");


				if (spReceive != null && spReceive.IsOpen) {

					//										Debug.Log ("222");
					string sbReadline2str = string.Empty;
					int bufferSize = spReceive.ReadBufferSize;
					//										Debug.Log ("bufferSize:" + bufferSize);
					Byte[] buf = new Byte[bufferSize];
					int count = spReceive.Read (buf, 0, bufferSize);
					Debug.Log ("ctn:" + count);
					if (buf.Length == 0) {
						return;
					}

					//										Debug.Log ("333");
					if (count > 0) {
//												StringBuilder sb = new StringBuilder ();
//												for (int i = 0; i < buf.Length; i++) { 
//														//					string c = buf.ToString("X2");
//														string c = System.Text.Encoding.ASCII.GetString (buf,0,count);
//														sb.Append (c);
//
//														//					sbReadline2str += buf.ToString("X2"); 
//														//					queueDataPool.Enqueue(sbReadline2str); 
//
//												} 
//
//												Debug.Log ("out string:" + sb.ToString ());

						string c = System.Text.Encoding.ASCII.GetString (buf, 0, count);
						Debug.Log ("show out:" + c.ToString ());
					}
				}
				//Debug.Log("over");
			} catch (Exception ex) {
				Debug.Log ("error");
				Debug.Log (ex);
			}
			Thread.Sleep (100);
		}
	}
	//	private void DealData() 
	//	{ 
	//		while (queueDataPool.Count != 0) 
	//		{ 
	//			for (int i = 0; i < queueDataPool.Count; i++) 
	//			{ 
	//				strOutPool+= queueDataPool.Dequeue(); 
	//				if(strOutPool.Length==16) 
	//				{ 
	//					Debug.Log(strOutPool); 
	//					strOutPool=string.Empty; 
	//				} 
	//			} 
	//			
	//		} 
	//	} 

	private void SendSerialPortData (string data)
	{
		if (spSend.IsOpen) {
			Debug.Log ("send data:" + data);
			spSend.Write(data);
			spSend.WriteLine (data);
		}
	}

	void OnApplicationQuit ()
	{
		spSend.Close ();
		spReceive.Close ();
		threadReceive.Abort ();
		//		tPort.Abort ();
	}
}