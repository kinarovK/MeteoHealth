using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using SQLite_Database_service.Models;

namespace MeteoHealth.Services
{
    internal class ChartMaker : IChartMaker
    {
        private bool isSync;
        private readonly List<PlotModel> plotModels = new List<PlotModel>();
        public PlotModel CreateHealthChar(List<HealthStateModel> healthData, string yAxisTitle, string dataFieldName)
        { 

            var plotModel = new PlotModel { };
            var lineSeries = CreateHealthLineSeries(healthData);
            var dateAxis = ColfigureDateTimeAxisForHealthState(healthData);
            var healthAxis = ConfigureHealthYAxis(yAxisTitle);
            plotModel.Axes.Add(dateAxis);
            plotModel.Axes.Add(healthAxis);
            plotModel.Series.Add(lineSeries);
            plotModels.Add(plotModel);
          

            return plotModel;
        }

        private LineSeries CreateHealthLineSeries(List<HealthStateModel> healthData)
        {
            var lineSeries = new LineSeries
            {
                
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
            return lineSeries;

        }

        private DateTimeAxis ColfigureDateTimeAxisForHealthState(List<HealthStateModel> healthData)
        {
            DateTimeAxis dateAxis;
            if (healthData.Count < 5)
            {
                dateAxis = CreateSimpleDateTimeAxisForHealthState();
            }
            else
            {
                var minimumDate = (healthData.Count > 5 ? healthData[healthData.Count - 5] : healthData[0]).Date;
                dateAxis = CreatePanableDateTimeAxisForHealthState(minimumDate, healthData.First().Date, healthData.Last().Date);
            }
            return dateAxis;
        }

        private DateTimeAxis CreateSimpleDateTimeAxisForHealthState()
        {

            return new DateTimeAxis
            {
                Position = AxisPosition.Bottom,
                StringFormat = "yyyy-MM-dd",
                
                IntervalType = DateTimeIntervalType.Days,
                MajorStep = 1,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                Angle = 90,
                IsPanEnabled = false,
                IsZoomEnabled = false
            };

        }
        private DateTimeAxis CreateSimpleDateTimeAxisForWeather(string healthDataMinimumDate, string healthDataMaximumDate)
        {
            return new DateTimeAxis
            {
                IsZoomEnabled = false,
                IsPanEnabled = false,
                AbsoluteMinimum = DateTimeAxis.ToDouble(DateTime.Parse(healthDataMinimumDate)),
                AbsoluteMaximum = DateTimeAxis.ToDouble(DateTime.Parse(healthDataMaximumDate)),
            };
        }

        private DateTimeAxis CreatePanableDateTimeAxisForHealthState(string minimumDate, string absolutMinimumDate, string maximumDate)
        {
            return new DateTimeAxis
            {
                Position = AxisPosition.Bottom,
                StringFormat = "yyyy-MM-dd",
                IntervalType = DateTimeIntervalType.Days,
                MajorStep = 1,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                Angle = 90,
                IsPanEnabled = true,
                Minimum = DateTimeAxis.ToDouble(DateTime.Parse(minimumDate)),
                Maximum = DateTimeAxis.ToDouble(DateTime.Parse(maximumDate)),
                AbsoluteMinimum = DateTimeAxis.ToDouble(DateTime.Parse(absolutMinimumDate)),
                AbsoluteMaximum = DateTimeAxis.ToDouble(DateTime.Parse(maximumDate))
            };
        }
        private DateTimeAxis CreatePanableDateTimeAxisForWeather(string minimumDate, string maximumDate, string absoluteMinimumDate, string absoluteMaximumDate)
        {
            return new DateTimeAxis
            {
              
                MajorStep = 1,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                Angle = 90,
              
                IsPanEnabled = true,
                Minimum = (DateTimeAxis.ToDouble(DateTime.Parse(minimumDate))),
                Maximum = DateTimeAxis.ToDouble(DateTime.Parse(maximumDate)),
                AbsoluteMaximum = DateTimeAxis.ToDouble(DateTime.Parse(absoluteMaximumDate)),
                AbsoluteMinimum = DateTimeAxis.ToDouble(DateTime.Parse(absoluteMinimumDate)),
            };
        }
        private LinearAxis ConfigureHealthYAxis(string yAxisTitle)
        {
            return new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = "Health state",
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                IsPanEnabled = true,
                IsZoomEnabled = false

            };
        }
        public PlotModel CreateWeatherChart(List<WeatherModel> weatherData, List<HealthStateModel> healthData, string yAxisTitle, string dataFieldName)
        {
            var plotModel = new PlotModel { Title = dataFieldName };
            var lineSeries = CreateLineSeries();
            DateTime.TryParse(healthData.LastOrDefault()?.Date, out DateTime parsedLastHealthdata);
            AddWeatherDataPoints(lineSeries, weatherData, dataFieldName);
            var newCol = weatherData.Where(x => DateTime.Parse(x.DateTime) <= parsedLastHealthdata).ToList();//delete
            var dateTimeAxis = ConfigureDateTimeAxis(weatherData, healthData, parsedLastHealthdata);
            plotModel.Axes.Add(dateTimeAxis);
            var yAxis = ConfigureYAxis(yAxisTitle);
            plotModel.Axes.Add(yAxis);
            plotModel.Series.Add(lineSeries);
            plotModels.Add(plotModel);
            return plotModel;
        }

        private LineSeries CreateLineSeries()
        {
            return new LineSeries
            {
                MarkerType = MarkerType.Circle,
                MarkerSize = 3,
                MarkerStroke = OxyColors.Red,
                LineStyle = LineStyle.Solid
            };
        }

        private void AddWeatherDataPoints(LineSeries lineSeries, List<WeatherModel> weatherData, string yAxisTitle)
        {
            foreach (var item in weatherData)
            {
                if (DateTime.TryParse(item.DateTime, out DateTime parsedDateTime))
                {
                    switch (yAxisTitle)
                    {
                        case "Temperature":
                            lineSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(parsedDateTime), item.Temperature));
                            break;
                        case "Humidity":
                            lineSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(parsedDateTime), item.Humidity));
                            break;
                        case "Pressure":
                            lineSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(parsedDateTime), item.Pressure));
                            break;
                        case "Wind":
                            lineSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(parsedDateTime), item.WindSpeed));
                            break;
                        case "Precipitation Probability":
                            lineSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(parsedDateTime), item.PrecipitationProbability));
                            break;
                        case "Precipitation Volume":
                            lineSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(parsedDateTime), item.PrecipitationVolume));
                            break;
                        default:
                            throw new Exception($"Unknown yAxisTitle: {yAxisTitle}");
                    }
                }
            }
        }
        private void SyncTwoCharts(DateTimeAxis sourceAxis, PlotModel targetPlotModel)
        {
            var targetAxis = targetPlotModel.Axes.OfType<DateTimeAxis>().FirstOrDefault();
            if (targetAxis != null && targetAxis != sourceAxis)
            {
                // Detach the existing event handler
                targetAxis.AxisChanged -= (s, args) => OnAxisChanged(sourceAxis, args, targetPlotModel);

                // Sync the axis
                targetAxis.Zoom(sourceAxis.ActualMinimum, sourceAxis.ActualMaximum);

                // Re-attach the event handler
                targetAxis.AxisChanged += (s, args) => OnAxisChanged(targetAxis, args, targetPlotModel);

                // Refresh the target plot
                targetPlotModel.InvalidatePlot(false);
            }
        }
        private DateTimeAxis ConfigureDateTimeAxis(List<WeatherModel> weatherData, List<HealthStateModel> healthData, DateTime parsedLastHealthdata)
        {
            var newCol = weatherData.Where(x => DateTime.Parse(x.DateTime) <= parsedLastHealthdata).ToList();

            DateTimeAxis dateTimeAxis;
            if (newCol.Count <= 33)
            {
                dateTimeAxis = CreateSimpleDateTimeAxisForWeather(healthData.First().Date, healthData.Last().Date);
            }
            else
            {
                var minimumDateTemp = (weatherData.Count > 33 ? weatherData[newCol.Count - 33] : weatherData[0]).DateTime;
                var weatherl = weatherData[weatherData.Count - 1].DateTime;
                var weatherlc = weatherData.Last().DateTime;
                dateTimeAxis = CreatePanableDateTimeAxisForWeather(minimumDateTemp, weatherData.Last().DateTime, healthData.First().Date, healthData.Last().Date);
            }

            return dateTimeAxis;
        }
        private LinearAxis ConfigureYAxis(string yAxisTitle)
        {
            return new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = yAxisTitle,
               
                AxisDistance = 1,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                IsPanEnabled = false,
                IsZoomEnabled = false
            };
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
