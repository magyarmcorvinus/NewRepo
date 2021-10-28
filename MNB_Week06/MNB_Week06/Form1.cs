using MNB_Week06.Entities;
using MNB_Week06.MNBServiceReference;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Xml;

namespace MNB_Week06
{
    public partial class Form1 : Form
    {
        BindingList<RateData> Rates = new BindingList<RateData>();

        public Form1()
        {
            InitializeComponent();
            
            dataGridView1.DataSource = Rates;
            chartRateData.DataSource = Rates;
            chartRateData.Series[0].ChartType = SeriesChartType.Line;
            chartRateData.Series[0].XValueMember = "Date";
            chartRateData.Series[0].YValueMembers = "Value";
            chartRateData.Legends[0].Enabled = false;
            chartRateData.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            chartRateData.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
            chartRateData.ChartAreas[0].AxisY.IsStartedFromZero = false;

            LoadXML();
        }

        private void LoadXML()
        {
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(CallWebService());
            foreach (XmlElement item in xml.DocumentElement)
            {
                RateData r = new RateData();
                r.Currency = ((XmlElement)item.ChildNodes[0]).GetAttribute("curr");
                r.Date = DateTime.Parse(item.GetAttribute("date"));
                r.Value = decimal.Parse(((XmlElement)item.ChildNodes[0]).InnerText);
                int unit = int.Parse(((XmlElement)item.ChildNodes[0]).GetAttribute("unit"));
                if (unit != 0) r.Value = r.Value / unit;
                Rates.Add(r);
            }
        }

        string CallWebService()
        {
            MNBArfolyamServiceSoapClient mnbService = new MNBArfolyamServiceSoapClient();
            GetExchangeRatesRequestBody req = new GetExchangeRatesRequestBody();
            req.currencyNames = "EUR";
            req.startDate = "2021-01-01";
            req.startDate = "2021-06-30";
            var resp = mnbService.GetExchangeRates(req);
            return resp.GetExchangeRatesResult;
        }
    }
}
