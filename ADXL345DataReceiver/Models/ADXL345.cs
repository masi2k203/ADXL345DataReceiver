using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;
using System.Text.RegularExpressions;

namespace ADXL345DataReceiver.Models
{
    class ADXL345 : SerialObject
    {
        #region フィールド
        // 3軸加速度値
        public AxisValue axisValue;
        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ADXL345()
        {
            axisValue = new AxisValue();
        }

        /// <summary>
        /// データ受信メソッドのオーバーライド
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void SerialDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            // 受信データフォーマット
            try
            {
                // 受信データを正規表現にかける
                MatchCollection mc = GetMatches(base.Port.ReadLine());

                // データを分離
                int i = 0;
                double[] values = new double[3];
                foreach (var item in mc)
                {
                    values[i] = double.Parse(item.ToString());
                    i++;
                }

                // 加速度値取得
                axisValue.XAxis.Value = values[0];
                axisValue.YAxis.Value = values[1];

                // 座標取得
                axisValue.Canvas_XAxis.Value = 220 + axisValue.YAxis.Value * 5;
                axisValue.Canvas_YAxis.Value = 155 + axisValue.XAxis.Value * -5;

            }
            catch (OperationCanceledException)
            {
                System.Diagnostics.Debug.WriteLine("オペレーションがキャンセルされました。(予期した動作)");
            }
        }

        /// <summary>
        /// 正規表現に適合したものを取得
        /// </summary>
        /// <param name="inputstr"></param>
        /// <returns></returns>
        private MatchCollection GetMatches(string inputstr)
        {
            // パターン生成
            Regex regex = new Regex(@"[+-]?([0-9]+(\.[0-9]*)?|\.[0-9]+)([eE][+-]?[0-9]+)?", RegexOptions.IgnoreCase);
            // MatchCollectionを返却
            return regex.Matches(inputstr);
        }
    }
}
