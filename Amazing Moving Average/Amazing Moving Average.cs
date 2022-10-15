using System;
using cAlgo.API;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo
{

    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class AmazingMovingAverage : Indicator
    {

        #region Identity

        public const string NAME = "Amazing Moving Average";

        public const string VERSION = "1.0.0";

        #endregion

        #region Params

        [Parameter(NAME + " " + VERSION, Group = "Identity", DefaultValue = "https://www.google.com/search?q=ctrader+guru+amazing+moving+average")]
        public string ProductInfo { get; set; }
        [Parameter("Source")]
        public DataSeries Source { get; set; }

        [Parameter("Period", DefaultValue = 100, MinValue = 1)]
        public int Period { get; set; }

        [Parameter("MA Type", DefaultValue = MovingAverageType.Triangular)]
        public MovingAverageType Type { get; set; }

        [Parameter("Deviation", DefaultValue = 0, MinValue = 0, Step = 0.1)]
        public double Deviation { get; set; }   

        [Output("AMA", LineColor = "FF595959", PlotType = PlotType.DiscontinuousLine, Thickness = 1)]
        public IndicatorDataSeries AMAresult{ get; set; }

        [Output("AMA+", LineColor = "FF00BF00", PlotType = PlotType.DiscontinuousLine, Thickness = 1)]
        public IndicatorDataSeries AMAplus { get; set; }

        [Output("AMA-", LineColor = "FFFE0000", PlotType = PlotType.DiscontinuousLine, Thickness = 1)]
        public IndicatorDataSeries AMAminus { get; set; }

        #endregion

        #region Property

        private MovingAverage AMA;

        #endregion

        #region Indicator Events
        
        protected override void Initialize()
        {

            AMA = Indicators.MovingAverage(Source, Period, Type);

        }

        public override void Calculate(int index)
        {

            if (index <= Period) return;

            AMAresult[index] = AMA.Result.Last(0);
            AMAplus[index] = double.NaN;
            AMAminus[index] = double.NaN;

            double diff = Math.Round(Math.Abs(AMA.Result.Last(0) - AMA.Result.Last(1)), Symbol.Digits);

            if (diff < PipsToDigits(Deviation)) return;

            if (AMA.Result.Last(0) > AMA.Result.Last(1))
            {

                AMAplus[index] = AMA.Result.Last(0);

            }
            else if (AMA.Result.Last(0) < AMA.Result.Last(1))
            {

                AMAminus[index] = AMA.Result.Last(0);

            }

        }

        #endregion

        #region Methods
        private double PipsToDigits(double Pips)
        {

            return Math.Round(Pips * Symbol.PipSize, Symbol.Digits);

        }

        #endregion

    }

}