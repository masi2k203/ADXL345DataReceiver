using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;
using DebugOutput = System.Diagnostics.Debug;

namespace ADXL345DataReceiver.Models
{
    /// <summary>
    /// シリアル通信用オブジェクト
    /// </summary>
    class SerialObject
    {
        #region フィールドの定義
        
        /// <summary>
        /// シリアルポート
        /// </summary>
        public SerialPort Port { get; private set; }

        /// <summary>
        /// ポート一覧
        /// </summary>
        public string[] PortList { get; private set; }

        /// <summary>
        /// 初期化フラグ
        /// </summary>
        public bool IsInitialized { get; set; }
        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SerialObject()
        {
            // インスタンス化時にポートを取得
            this.PortList = SerialPort.GetPortNames();
        }

        #region メソッド
        /// <summary>
        /// シリアルポートの初期化を行うメソッド
        /// </summary>
        /// <param name="portname">ポート名</param>
        /// <param name="baudrate">ボーレート</param>
        /// <param name="parity">パリティビット</param>
        /// <param name="stopbits">ストップビット</param>
        /// <param name="databit">データビット</param>
        public void InitializeSerialPort(string portname, int baudrate, Parity parity, StopBits stopbits, int databit)
            => this.Port = new SerialPort(portname, baudrate, parity, databit, stopbits);
        
        /// <summary>
        /// デバイスにシリアル通信で接続するメソッド
        /// </summary>
        public void StartConnection()
        {
            try
            {
                // ポートが非nullかつ未開放
                if (this.Port != null && !this.Port.IsOpen)
                {
                    if (this.IsInitialized == true)
                    {
                        // シリアルポート開放
                        this.Port.Open();
                        // イベント購読
                        this.Port.DataReceived += SerialDataReceived;
                    }
                }
                else
                {
                    DebugOutput.WriteLine("既に開かれているか、インスタンスがnullです。");
                }
            }
            catch (Exception ex)
            {
                DebugOutput.WriteLine("Exception \n" + ex.ToString());
            }
        }

        /// <summary>
        /// デバイスとのシリアル通信を終了するメソッド
        /// </summary>
        public void FinishConnection()
        {
            try
            {
                // 非nullかつ開放状態
                if ( this.Port != null && this.Port.IsOpen)
                {
                    // 購読解除
                    this.Port.DataReceived -= SerialDataReceived;
                    // 切断処理
                    this.Port.Close();
                    this.Port.Dispose();
                }
            }
            catch (Exception ex)
            {
                DebugOutput.WriteLine("Exception \n" + ex.ToString());
            }
        }

        /// <summary>
        /// データ受信時に呼び出されるメソッド
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void SerialDataReceived(object sender, SerialDataReceivedEventArgs e)
            => DebugOutput.WriteLine(this.Port.ReadLine());

        #endregion
    }
}
