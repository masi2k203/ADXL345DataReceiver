using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ADXL345DataReceiver.Models
{
    class AxisValue : INotifyPropertyChanged
    {
        #region フィールド
        // 3軸加速度値
        public ReactiveProperty<double> XAxis { get; set; }
            = new ReactiveProperty<double>();
        public ReactiveProperty<double> YAxis { get; set; }
            = new ReactiveProperty<double>();
        public ReactiveProperty<double> ZAxis { get; set; }
            = new ReactiveProperty<double>();

        // キャンバス座標
        public ReactiveProperty<double> Canvas_XAxis { get; set; }
            = new ReactiveProperty<double>();
        public ReactiveProperty<double> Canvas_YAxis { get; set; }
            = new ReactiveProperty<double>();

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
    }
}
