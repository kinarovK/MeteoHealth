using OxyPlot;
using SQLite_Database_service.Models;
using System.Collections.Generic;

namespace MeteoHealth.Services
{
    public interface IChartMaker
    {
        PlotModel CreateHealthChar(List<HealthStateModel> healthData, string yAxisTitle, string dataFieldName);
        PlotModel CreateWeatherChart(List<WeatherModel> weatherData, List<HealthStateModel> healthData,string yAxisTitle, string dataFieldName);
        void InitializeCharts(PlotModel sourcePlotModel, PlotModel targetPlotModel);
    }
}