using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoExtractor
{
    public class Coord
    {
        public double x { get; set; }
        public double y { get; set; }
        public double z { get; set; }
    }

    public class Hd
    {
        public int startpos { get; set; }
        public double endpos { get; set; }
        public string SAMPLES { get; set; }
        public string FREQUENCY { get; set; }
        public string FREQUENCY_STEPS { get; set; }
        public string SIGNAL_POSITION { get; set; }
        public string RAW_SIGNAL_POSITION { get; set; }
        public string DISTANCE_FLAG { get; set; }
        public string TIME_FLAG { get; set; }
        public string PROGRAM_FLAG { get; set; }
        public string EXTERNAL_FLAG { get; set; }
        public string TIME_INTERVAL { get; set; }
        public string OPERATOR { get; set; }
        public string CUSTOMER { get; set; }
        public string SITE { get; set; }
        public string ANTENNAS { get; set; }
        public string ANTENNA_ORIENTATION { get; set; }
        public string COMMENT { get; set; }
        public string STACKS { get; set; }
        public string STACK_EXPONENT { get; set; }
        public string STACKING_TIME { get; set; }
        public string LAST_TRACE { get; set; }
        public string SYSTEM_CALIBRATION { get; set; }
        public string SHORT_FLAG { get; set; }
        public string INTERMEDIATE_FLAG { get; set; }
        public string LONG_FLAG { get; set; }
        public string PREPROCESSING { get; set; }
        public string HIGH { get; set; }
        public string LOW { get; set; }
        public string FIXED_INCREMENT { get; set; }
        public string FIXED_MOVES_UP { get; set; }
        public string FIXED_MOVES_DOWN { get; set; }
        public string FIXED_POSITION { get; set; }
        public string WHEEL_CALIBRATION { get; set; }
        public string POSITIVE_DIRECTION { get; set; }
        public string OMGPSHEIGHT { get; set; }
    }

    public class GPR_DataModel
    {
        public List<List<string>> data { get; set; }
        public List<string> depth { get; set; }
        public List<Coord> coords { get; set; }
        public List<string> traces { get; set; }
        public List<string> pos { get; set; }
        public List<string> time0 { get; set; }
        public List<object> time { get; set; }
        public List<string> fid { get; set; }
        public List<object> rec { get; set; }
        public List<object> trans { get; set; }
        public List<object> coordref { get; set; }
        public List<string> dz { get; set; }
        public List<string> dx { get; set; }
        public List<string> antsep { get; set; }
        public List<string> name { get; set; }
        public List<string> description { get; set; }
        public List<string> filepath { get; set; }
        public List<string> depthunit { get; set; }
        public List<string> posunit { get; set; }
        public List<string> surveymode { get; set; }
        public List<string> date { get; set; }
        public List<string> crs { get; set; }
        public List<Vel> vel { get; set; }
        public List<object> delineations { get; set; }
        public List<Hd> hd { get; set; }
        public List<string> img_base64 { get; set; }
    }

    public class Vel
    {
        public double v { get; set; }
    }

}
