﻿using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class CommunicationScript : MonoBehaviour
{

    Dictionary<int,int[]> table = new Dictionary<int,int[]>();
    
    Thread widefindThread,fibaroThread;

    UdpClient fibaro, widefind;

    int fibaroPort = 42069;
    int widefindPort = 42069;

    bool widefindFlag = true;


    Model model;


    // Start is called before the first frame update
    void Start()
    {
       
        init();

    }

    // Update is called once per frame
    void Update()
    {


    }

    void OnDestroy(){

        widefindFlag = false;
        print("Killing recv thread");
        print("Killing sendThread");
        //widefindThread.Abort();
        fibaroThread.Abort();
    }

     private void init()
    {
        model = new Model();
       
        /*widefindThread = new Thread(new ThreadStart(widefindComm));
        widefindThread.IsBackground = true;
        widefindThread.Start();*/
 

        fibaroThread = new Thread(new ThreadStart(fibaroComm));
        fibaroThread.IsBackground = true;
        fibaroThread.Start();

       
 
    }

    public Model getModel(){

        return this.model;
    }

    private void fibaroComm()
    {
        
        fibaro = new UdpClient(fibaroPort);
        fibaro.Connect("130.240.114.14", fibaroPort);
        //fibaro.Connect("130.240.114.51", fibaroPort);

        string a = "fibaro;";

        int[] k = {299,358,352,360,366,322,326,310,354,382,372,374,376,271,316};
        int i = 0;
        
       
        IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
        
        byte[] c;
        sendSubWidefind();
        while(true){
            i = i%(k.Length);
            //print("Sending : " + a);
            a = "fibaro;" + k[i].ToString();
            i++;
            
            
            c = Encoding.ASCII.GetBytes(a);
            Thread.Sleep(100);
           
            print("SENDING : " + a);
            fibaro.Send(c, c.Length);
            


            byte[] data = fibaro.Receive(ref anyIP);
                
            string text = Encoding.UTF8.GetString(data);
            print("RECEIVING : " + text);

            model.updateTable(text);
                

        }
        /*while(true){
             byte[] data = fibaro.Receive(ref anyIP);
                
            string text = Encoding.UTF8.GetString(data);
            print("RECEIVING : " + text);

            model.updateTable(text);
        }*/





    }


    private void sendSubWidefind(){


        bool tryAgain = true;
        while(tryAgain){
            if(widefindFlag){
                try{
                    
                    string a = "widefind;5";
                    byte [] c = Encoding.ASCII.GetBytes(a);

                    fibaro.Send(c, c.Length);
                    tryAgain = false;
                }
                catch {
                    print("Couldnt send widefind subscribe");
                }
            }
       }

        Task.Delay(5000).ContinueWith(t=> sendSubWidefind());
    }
    private void widefindComm(){

        widefind = new UdpClient(widefindPort);

        widefind.Connect("130.240.74.55",widefindPort);

        IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);

        
        //subscribe to the widefind updates
        


        while(true){

            

            byte[] data = widefind.Receive(ref anyIP);
            

                
            string text = Encoding.UTF8.GetString(data);
            print("Widefind : " + text);
            model.updateTable(text);
            

        }


    }


}
