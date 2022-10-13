using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TCPServerResponseFile
{
    static class ControlExtensions
    {
        public static void UIThread(this Control @this, Action code)
        {
            if (@this.InvokeRequired)
            {
                @this.BeginInvoke(code);
            }
            else
            {
                code.Invoke();
            }
        }

        static System.Globalization.CultureInfo invcul = System.Globalization.CultureInfo.InvariantCulture;
        static System.Globalization.CultureInfo trcul = new System.Globalization.CultureInfo("tr-TR");
        public static string  ToEngineeringNotation(this decimal d, string  format, string  unit)
        {
            return ToEngineeringNotation((double)d, format, unit);
        }

        public static string  ToEngineeringNotation(this double d, string  format, string  unit)
        {
            double exponent = Math.Log10(Math.Abs(d));
            if (Math.Abs(d) >= 1)
            {
                switch ((int)Math.Floor(exponent))
                {
                    case 0:
                    case 1:
                    case 2:
                        return d.ToString(format, trcul) + " " + unit;
                    case 3:
                    case 4:
                    case 5:
                        return (d / 1e3).ToString(format, trcul) + " k" + unit;
                    case 6:
                    case 7:
                    case 8:
                        return (d / 1e6).ToString(format, trcul) + " M" + unit;
                    case 9:
                    case 10:
                    case 11:
                        return (d / 1e9).ToString(format, trcul) + " G" + unit;
                    case 12:
                    case 13:
                    case 14:
                        return (d / 1e12).ToString(format, trcul) + " T" + unit;
                    case 15:
                    case 16:
                    case 17:
                        return (d / 1e15).ToString(format, trcul) + " P" + unit;
                    case 18:
                    case 19:
                    case 20:
                        return (d / 1e18).ToString(format, trcul) + " E" + unit;
                    case 21:
                    case 22:
                    case 23:
                        return (d / 1e21).ToString(format, trcul) + " Z" + unit;
                    default:
                        return (d / 1e24).ToString(format, trcul) + " Y" + unit;
                }
            }
            else if (Math.Abs(d) > 0)
            {
                switch ((int)Math.Floor(exponent))
                {
                    case -1:
                    case -2:
                    case -3:
                        return (d * 1e3).ToString(format, trcul) + " m" + unit;
                    case -4:
                    case -5:
                    case -6:
                        return (d * 1e6).ToString(format, trcul) + " μ" + unit;
                    case -7:
                    case -8:
                    case -9:
                        return (d * 1e9).ToString(format, trcul) + " n" + unit;
                    case -10:
                    case -11:
                    case -12:
                        return (d * 1e12).ToString(format, trcul) + " p" + unit;
                    case -13:
                    case -14:
                    case -15:
                        return (d * 1e15).ToString(format, trcul) + " f" + unit;
                    case -16:
                    case -17:
                    case -18:
                        return (d * 1e15).ToString(format, trcul) + " a" + unit;
                    case -19:
                    case -20:
                    case -21:
                        return (d * 1e15).ToString(format, trcul) + " z" + unit;
                    default:
                        return (d * 1e15).ToString(format, trcul) + " y" + unit;
                }
            }
            else
            {
                return "0" + unit; ;
            }
        }

        static string  getMagUnit(int mag)
        {
            if (mag == -6)
                return "μ";
            if (mag == -3)
                return "m";
            if (mag == 3)
                return "k";
            if (mag == 6)
                return "M";
            return "";
        }
        static string  getMagTimeUnit(int mag)
        {
            if (mag == -6)
                return "p";
            if (mag == -3)
                return "n";
            if (mag == 3)
                return "m";
            if (mag == 6)
                return "s";
            return "μ";
        }

    }
}


 