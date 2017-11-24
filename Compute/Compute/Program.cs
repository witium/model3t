using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OSGeo.GDAL;
using OSGeo.OSR;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PKU.Model3T.Compute
{
    class Program
    {
        static void Main(string[] args)
        {
            var arguments = CommandLineArgumentParser.Parse(args);
            if (arguments.Has("dirname"))
            {
                string dirname = arguments.Get("-dirname").Next;
                Do(dirname);
            }
            else
            {
                
            }
        }

        public static bool Do(string dirname)
        {
            bool result = false;
            //
            double[] NDVI = null;
            double[] F = null;
            double[] G = null;
            double[] GSD = null;
            double[] RNCP = null;
            //
            double[] RNSD = null;
            double[] RN = null;
            double[] RNC = null;
            double[] RNS = null;
            double[] TA = null;
            double[] TC = null;
            double[] RSWD = null;
            //
            double[] LST = null;
            double TCP = 0.0;
            double[] TS = null;
            double TSD = 0.0;
            double[] LAI = null;
            double[] A = null;
            double[] LSE = null;
            // 
            double[] FPAR = null;
            //
            double[] ETS = null;
            double[] ETC = null;
            double[] ET = null;
            double[] JUD = null;

            // 读取配置文件
            string configStr = File.ReadAllText("config.json");
            JObject config = JsonConvert.DeserializeObject<JObject>(configStr);
            
            var helper = new Utils();

            // 1、读取Ta
            TA = helper.FetchDataFromFile(Convert.ToString(config["TA"]));

            // 初始化需要求解的变量
            int length = TA.Length;
            F = new double[length];
            LAI = new double[length];
            FPAR = new double[length];
            TS = new double[length];
            TC = new double[length];
            G = new double[length];
            GSD = new double[length];
            RNCP = new double[length];
            RNSD = new double[length];
            RNC = new double[length];
            RNS = new double[length];
            A = new double[length];
            LSE = new double[length];
            LST = new double[length];
            RN = new double[length];
            JUD = new double[length];
            ETC = new double[length];
            ETS = new double[length];
            ET = new double[length];

            // 2、读取NDVI，求f
            NDVI = helper.FetchDataFromFile(Convert.ToString(config["NDVI"]));

            // 求MIN_NDVI和MAX_NDVI
            double minNDVI = 0;
            double maxNDVI = 0;
            List<double> temp_ndvi = new List<double>();
            // 在程序自动求NDVI阈值的时候，首先去除NDVI中小于0的NDVI，然后再取前后1%，作为min_NDVI和max_NDVI
            for (int i = 0; i < length; i++)
            {
                if (NDVI[i] >= 0)
                {
                    temp_ndvi.Add(NDVI[i]);
                }
            }
            temp_ndvi.Sort();

            int minNDVINum = 0;
            int maxNDVINum = 0;

            minNDVI = Convert.ToDouble(config["MINNDVI"]);
            maxNDVI = Convert.ToDouble(config["MAXNDVI"]);

            if (Convert.ToInt32(config["MINNDVIAUTO"]) == 1)
            {
                int minPercent = Convert.ToInt32(config["MINNDVIPERCENT"]);
            }

            if (Convert.ToInt32(config["MAXNDVIAUTO"]) == 1)
            {
                int maxPercent = Convert.ToInt32(config["MAXNDVIPERCENT"]);
            }


            return result;
        }
    }

}
