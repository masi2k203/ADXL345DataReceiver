using ADXL345DataReceiver.Models;
using Reactive.Bindings;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace ADXL345DataReceiver.ViewModels
{
    class MainDataViewModel : INotifyPropertyChanged
    {
        // メモリリーク防止
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainDataViewModel()
        {
            // ADXL345オブジェクト初期化
            _ADXL345 = new ADXL345();
            // 仮初期化
            _ADXL345.InitializeSerialPort(
                "COM4",
                9600,
                System.IO.Ports.Parity.None,
                System.IO.Ports.StopBits.One,
                8
            );
            _ADXL345.IsInitialized = true;
            // 値設定
            Canvas_X.Value = 220;
            Canvas_Y.Value = 155;
            Canvas_X = _ADXL345.axisValue.Canvas_XAxis;
            Canvas_Y = _ADXL345.axisValue.Canvas_YAxis;
            Acc_X = _ADXL345.axisValue.XAxis;
            Acc_Y = _ADXL345.axisValue.YAxis;
            // 購読
            Acc_X.Subscribe(x => ActingInputByAccelerationValue('X', x));
            Acc_Y.Subscribe(y => ActingInputByAccelerationValue('Y', y));
            ConnectADXL345.Subscribe(_ => _ADXL345.StartConnection());
            DisconnectADXL345.Subscribe(_ => _ADXL345.FinishConnection());
        }

        #region フィールド
        // ADXL345
        public ADXL345 _ADXL345;

        // 加速度値
        public ReactiveProperty<double> Acc_X { get; set; }
            = new ReactiveProperty<double>();
        public ReactiveProperty<double> Acc_Y { get; set; }
            = new ReactiveProperty<double>();

        // Canvas座標
        public ReactiveProperty<double> Canvas_X { get; set; }
            = new ReactiveProperty<double>();
        public ReactiveProperty<double> Canvas_Y { get; set; }
            = new ReactiveProperty<double>();

        // 代行文字列
        public ReactiveProperty<string> UpSwingText { get; set; }
            = new ReactiveProperty<string>();
        public ReactiveProperty<string> DownSwingText { get; set; }
            = new ReactiveProperty<string>();
        public ReactiveProperty<string> RightSwingText { get; set; }
            = new ReactiveProperty<string>();
        public ReactiveProperty<string> LeftSwingText { get; set; }
            = new ReactiveProperty<string>();
        #endregion

        #region コマンド
        /// <summary>
        /// ADXL345に接続するコマンド
        /// </summary>
        public ReactiveCommand ConnectADXL345 { get; set; }
            = new ReactiveCommand();

        /// <summary>
        /// ADXL345から切断するコマンド
        /// </summary>
        public ReactiveCommand DisconnectADXL345 { get; set; }
            = new ReactiveCommand();
        #endregion

        #region メソッド
        /// <summary>
        /// 加速度値による入力代行メソッド
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="value"></param>
        private void ActingInputByAccelerationValue(char axis, double value)
        {
            // 整数値化および10倍
            int inputValue = (int)value * 10;

            if (axis == 'X')
            {
                if (inputValue >= 60)
                {
                    SendKeys.SendWait(UpSwingText.Value);
                }
                else if (inputValue <= -60)
                {
                    SendKeys.SendWait(DownSwingText.Value);
                }
            }
            else if (axis == 'Y')
            {
                if (inputValue >= 60)
                {
                    SendKeys.SendWait(RightSwingText.Value);
                }
                else if (inputValue <= -60)
                {
                    SendKeys.SendWait(LeftSwingText.Value);
                }
            }
        }
        #endregion
    }
}
