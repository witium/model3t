using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using OSGeo.GDAL;
using OSGeo.OSR;

namespace PKU.Model3T.Compute
{
    public class Utils
    {
        public double [] FetchDataFromFile(string filename)
        {
            try
            {
                OSGeo.GDAL.Gdal.AllRegister();
                OSGeo.GDAL.Dataset dataset = Gdal.Open(filename, Access.GA_ReadOnly);
                if (dataset == null)
                {
                    Console.WriteLine("Read file was failed!");
                }
                int width = dataset.RasterXSize;
                int height = dataset.RasterYSize;
                Band band1 = dataset.GetRasterBand(1);
                double[] data = new double[width * height];
                band1.ReadRaster(0, 0, width, height, data, width, height, 0, 0);

                return data;
            }
            catch (Exception ex)
            {
                return null;
                //Console.WriteLine("Read data was failed!");
                //Console.WriteLine("Error: " + ex.Message);
            }
        }

        public double GetMaxValue(double[] arr)
        {
            double max = arr[0];
            for (int i = 0; i < arr.Length; i++)
            {
                max = Math.Max(max, arr[i]);
            }

            return max;
        }

        public bool MakeTifFile(double[] et, string source, string dest)
        {
            bool result = false;
            OSGeo.GDAL.Gdal.AllRegister();
            OSGeo.GDAL.Dataset in_dataset = Gdal.Open(source, Access.GA_ReadOnly);
            int width = in_dataset.RasterXSize;
            int height = in_dataset.RasterYSize;

            Driver driver = Gdal.GetDriverByName("GTiff");
            Dataset out_dataset = driver.Create(dest, width, height, 1, DataType.GDT_Float64, null);
            string projectionRef = in_dataset.GetProjectionRef();
            double[] geoTransform = new double[6];
            out_dataset.SetProjection(projectionRef);
            out_dataset.SetGeoTransform(geoTransform);
            out_dataset.GetRasterBand(1).WriteRaster(0, 0, width, height, et, width, height, 0, 0);
            out_dataset.GetRasterBand(1).FlushCache();
            out_dataset.FlushCache();
            out_dataset.Dispose();
            result = true;

            return result;
        }
    }
}
