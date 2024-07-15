using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLite_Database_service;
using OxyPlot.Xamarin.Forms;

namespace MeteoHealth.Services
{
    internal class ChartMaker
    {
        private bool isSync;
        private List<PlotModel> plotModels = new List<PlotModel>();
        internal PlotModel CreateHealthChar(List<HealthStateModel> healthData, PlotView plotview, string yAxisTitle, string dataFieldName)
        {
            var plotModel = new PlotModel { };
            var lineSeries = new LineSeries
            {
                //Title = "Humidity",
                MarkerType = MarkerType.Circle,
                MarkerSize = 4,
                MarkerStroke = OxyColors.Yellow,
                LineStyle = LineStyle.Solid,
                Smooth = true,
                Color = OxyColors.Red,

            };
            foreach (var item in healthData)
            {
                if (DateTime.TryParse(item.Date, out DateTime parsedDateTime))
                {
                    lineSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(parsedDateTime), item.HealthLevel));
                }
            }
            var minimumDate = (healthData.Count > 5 ? healthData[healthData.Count - 5] : healthData[0]).Date;
            var dateAxis = new DateTimeAxis
            {
                Position = AxisPosition.Bottom,
                StringFormat = "yyyy-MM-dd",
                //Title = "Date"
                IntervalType = DateTimeIntervalType.Days,
                MajorStep = 1,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                Angle = 90,
                IsPanEnabled = true,
                //IsZoomEnabled = true,

                Minimum = (DateTimeAxis.ToDouble(DateTime.Parse(minimumDate))),
                Maximum = DateTimeAxis.ToDouble(DateTime.Parse(healthData[healthData.Count - 1].Date)),
                AbsoluteMaximum = DateTimeAxis.ToDouble(DateTime.Parse(healthData.Last().Date)),
                AbsoluteMinimum = DateTimeAxis.ToDouble(DateTime.Parse(healthData.First().Date)),
            };
            //dateAxis.AxisChanged += OnPressureHealthAxisChanged;
            //dateAxis.AxisChanged += (s, e) => OnAxisChanged(s, e, plotview);
            var healthForPressureAxis = new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = "Health state",
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                IsPanEnabled = true,
                //IsZoomEnabled = true

            };
            plotModel.Axes.Add(dateAxis);
            plotModel.Axes.Add(healthForPressureAxis);
            plotModel.Series.Add(lineSeries);

            plotview.Model = plotModel;
            plotview.Model.PlotMargins = new OxyThickness(41, 10, 10, 80);
            plotview.Model.Padding = new OxyThickness(0);



            plotModels.Add(plotModel);
            ///dateAxis.AxisChanged += (s, e) => OnAxisChanged(dateAxis, e);
            return plotModel;
        }



        internal PlotModel CreateWeatherChart(List<WeatherModel> weatherData, List<HealthStateModel> healthData, PlotView plotview, string yAxisTitle, string dataFieldName)
        {
            var plotModel = new PlotModel { Title = dataFieldName };

            var lineSeries = new LineSeries
            {
                MarkerType = MarkerType.Circle,
                MarkerSize = 4,
                MarkerStroke = OxyColors.Red,
                LineStyle = LineStyle.Solid
            };
            var parsedHealthData = DateTime.TryParse(healthData.LastOrDefault().Date, out DateTime parsedLastHealthdata);

            // Populate temperature data points
            foreach (var item in weatherData)
            {
                if (DateTime.TryParse(item.DateTime, out DateTime parsedDateTime))
                {
                    switch(dataFieldName)
                    {
                        case "Temperature":
                            lineSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(parsedDateTime),
                                item.Temperature));
                          
                            break;
                        case "Humidity":
                            lineSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(parsedDateTime),
                                item.Humidity));
                 
                            break;
                        case "Pressure":
                            lineSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(parsedDateTime), 
                                item.Pressure));
                            break;
                        case "Wind":
                            lineSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(parsedDateTime), 
                                item.WindSpeed));
                            break;
                        case "PrecipitationProbability":
                            lineSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(parsedDateTime),
                                item.PrecipitationProbability));
                            break;
                        case "PrecipitationVolume":
                            lineSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(parsedDateTime),
                                item.PrecipitationVolume));
                            break;
                        default:
                            //Some nofind category exception
                            throw new Exception();
                    }
                }
            }
            var newCol = weatherData.Where(x => DateTime.Parse(x.DateTime) <= parsedLastHealthdata).ToList();
            var minimumDateTemp = (weatherData.Count > 33 ? weatherData[newCol.Count - 33] : weatherData[0]).DateTime;
            // Create the DateTime axis for the X-axis
            var dateTimeAxis = new DateTimeAxis
            {
                Position = AxisPosition.Bottom,
                StringFormat = "yyyy-MM-dd", // Adjust the format as needed

                IntervalType = DateTimeIntervalType.Days,
                MajorStep = 1,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,

                Angle = 90,
                IsPanEnabled = true, // Enable panning
                //IsZoomEnabled = true, // Enable zooming
                // Set the initial window to the last 10 records
                Minimum = DateTimeAxis.ToDouble(DateTime.Parse(minimumDateTemp)),
                Maximum = DateTimeAxis.ToDouble(DateTime.Parse(weatherData[weatherData.Count - 1].DateTime)),
                AbsoluteMaximum = DateTimeAxis.ToDouble(DateTime.Parse(healthData.Last().Date)),
                AbsoluteMinimum = DateTimeAxis.ToDouble(DateTime.Parse(healthData.First().Date)),
                LabelFormatter = x => string.Empty
            };
            //ateTimeAxis.AxisChanged += OnPressureAxisChanged;

            plotModel.Axes.Add(dateTimeAxis);
            //dateTimeAxis.AxisChanged += (s, e) => OnAxisChanged(s, e, plotview);
            // Create the Linear axis for the Y-axis (temperature)
            var pressureAxis = new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = yAxisTitle,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                IsPanEnabled = true, // Enable panning
                //IsZoomEnabled = true // Enable zooming
            };

            plotModel.Axes.Add(pressureAxis);

            // Add the series to the plot model
            plotModel.Series.Add(lineSeries);

            // Set the model to plot view
            plotview.Model = plotModel;
            plotview.Model.PlotMargins = new OxyThickness(40, 0, 10, 0);
            plotview.Model.Padding = new OxyThickness(0);

            plotModels.Add(plotModel);
            ///dateTimeAxis.AxisChanged += (s, e) => OnAxisChanged(dateTimeAxis, e);
            return plotModel;
        }

        //private void OnAxisChanged(DateTimeAxis sourceAxis, AxisChangedEventArgs e)
        //{
        //    if (isSync) return;
        //    isSync = true;

        //    foreach (var targetPlotModel in plotModels)
        //    {
        //        var targetAxis = targetPlotModel.Axes.OfType<DateTimeAxis>().FirstOrDefault();
        //        if (targetAxis != null && targetAxis != sourceAxis)
        //        {
        //            // Detach existing event handler
        //            targetAxis.AxisChanged -= (s, args) => OnAxisChanged(targetAxis, args);

        //            // Sync the axis
        //            targetAxis.Zoom(sourceAxis.ActualMinimum, sourceAxis.ActualMaximum);

        //            // Attach the event handler back
        //            targetAxis.AxisChanged += (s, args) => OnAxisChanged(targetAxis, args);

        //            // Refresh the target plot
        //            targetPlotModel.InvalidatePlot(false);
        //        }
        //    }

        //    isSync = false;
        //}
        private void SyncTwoCharts(DateTimeAxis sourceAxis, PlotModel targetPlotModel)
        {
            var targetAxis = targetPlotModel.Axes.OfType<DateTimeAxis>().FirstOrDefault();
            if (targetAxis != null && targetAxis != sourceAxis)
            {
                // Detach the existing event handler
                targetAxis.AxisChanged -= (s, args) => OnAxisChanged(sourceAxis,args, targetPlotModel);

                // Sync the axis
                targetAxis.Zoom(sourceAxis.ActualMinimum, sourceAxis.ActualMaximum);

                // Re-attach the event handler
                targetAxis.AxisChanged += (s, args) => OnAxisChanged(targetAxis, args, targetPlotModel);

                // Refresh the target plot
                targetPlotModel.InvalidatePlot(false);
            }
        }
        private void OnAxisChanged(object sender, AxisChangedEventArgs e, PlotModel targetPlotModel)
        {
            if (isSync) return;
            isSync = true;

            var sourceAxis = sender as DateTimeAxis;
            if (sourceAxis != null)
            {
                SyncTwoCharts(sourceAxis, targetPlotModel);
            }

            isSync = false;
        }

        public void InitializeCharts(PlotModel sourcePlotModel, PlotModel targetPlotModel)
        {
            var sourceDateAxis = sourcePlotModel.Axes.OfType<DateTimeAxis>().FirstOrDefault();
            var targetDateAxis = targetPlotModel.Axes.OfType<DateTimeAxis>().FirstOrDefault();

            if (sourceDateAxis != null)
            {
                sourceDateAxis.AxisChanged += (s, e) => OnAxisChanged(s, e, targetPlotModel);
            }

            if (targetDateAxis != null)
            {
                targetDateAxis.AxisChanged += (s, e) => OnAxisChanged(s, e, sourcePlotModel);
            }
        }


    }
}
