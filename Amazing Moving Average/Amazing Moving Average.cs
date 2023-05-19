using System;
using cAlgo.API;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo
{

    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.FullAccess)]
    public class AmazingMovingAverage : Indicator
    {

        #region Identity

        public const string NAME = "Amazing Moving Average";

        public const string VERSION = "1.0.1";

        #endregion

        #region Params

        [Parameter(NAME + " " + VERSION, Group = "Identity", DefaultValue = "https://www.google.com/search?q=ctrader+guru+amazing+moving+average")]
        public string ProductInfo { get; set; }
        
        [Parameter("Source", Group = "Params")]
        public DataSeries Source { get; set; }

        [Parameter("Period", Group = "Params", DefaultValue = 100, MinValue = 1)]
        public int Period { get; set; }

        [Parameter("MA Type", Group = "Params", DefaultValue = MovingAverageType.Triangular)]
        public MovingAverageType Type { get; set; }

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