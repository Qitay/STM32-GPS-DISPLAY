using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GPSdisplay
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }

    class NMEAData
    {
        public string longitude { get; set; }
        public string latitude { get; set; }
        public string mapslink { get; set; }

        public string fix { get; set; }
        public string numberofsatelites { get; set; }
        public string trackedsatelites { get; set; }
        public string altitude { get; set; }
        public string hdop { get; set; }
        public string pdop { get; set; }
        public string vdop { get; set; } 
        public string speedKPH { get; set; }

        private const string url = "http://www.google.pl/maps/search/";

        
        public void parse(string NMEA)
        {
            if (!IsValid(NMEA)) return; //Checksum

            string [] split = NMEA.Split(new Char [] {','}); //Split data
            
            bool test = false;
            while (test == false)
            {
                test = ParseGPRMC(split);
                test = ParseGPGGA(split);
                test = ParseGPGLL(split);
                test = ParseGPGSA(split);
                test = ParseGPGSV(split);
                test = ParseGPRMC(split);
                break; 
            }
        }

        public bool ParseGPRMC(string[] split) 
        {
            if (split[0] == "$GPRMC" && (split[3] != "" & split[4] != "" & split[5] != "" & split[6] != ""))
            {
                string Latitude = split[3].Substring(0, 2) + "°";
                Latitude = Latitude + split[3].Substring(2) + "\'";
                Latitude = Latitude + split[4];
                latitude = Latitude;

                string Longitude = split[5].Substring(0, 3) + "°";
                Longitude = Longitude + split[5].Substring(3) + "\'";
                Longitude = Longitude + split[6];
                longitude = Longitude;

                mapslink = url + Latitude + "+" + Longitude;
                // Create URL
                return true;
            }
            else return false;
        }
        public bool ParseGPGGA(string[] split) 
        {
            if (split[0] == "$GPGGA" && (split[2] != "" & split[3] != "" & split[4] != "" & split[5] != ""))
            {
                string Latitude = split[2].Substring(0, 2) + "°";
                Latitude = Latitude + split[2].Substring(2) + "\'";
                Latitude = Latitude + split[3];
                latitude = Latitude;

                string Longitude = split[4].Substring(0, 3) + "°";
                Longitude = Longitude + split[4].Substring(3) + "\'";
                Longitude = Longitude + split[5];
                longitude = Longitude;

                mapslink = url + Latitude + "+" + Longitude;
                // Create URL

                if (split[6] != "")
                {
                    fix = split[6];
                }
                if (split[7] != "")
                {
                    trackedsatelites = split[7];
                }
                if (split[8] != "")
                {
                    hdop = split[8];
                    /*double dop = double.Parse(split[8],
                    System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.NumberFormatInfo.InvariantInfo);
                    if (dop <= 1) hdop = "idealny";
                    if (dop > 1 && dop < 4) hdop = "znakomity";
                    if (dop > 3 && dop < 7) hdop = "dobry";
                    if (dop > 6 && dop < 9) hdop = "umiarkowany";
                    if (dop > 8 && dop < 21) hdop = "słaby";
                    if (dop > 20) hdop = "zły";*/
                }

                if (split[9] != "")
                {
                    altitude = split[9];
                }
                return true;
            }
            else return false;
        }
        public bool ParseGPGLL(string[] split) 
        {
            if (split[0] == "$GPGLL" && (split[1] != "" & split[2] != "" & split[3] != "" & split[4] != ""))
            {
                string Latitude = split[1].Substring(0, 2) + "°";
                Latitude = Latitude + split[1].Substring(2) + "\'";
                Latitude = Latitude + split[2];
                latitude = Latitude;

                string Longitude = split[3].Substring(0, 3) + "°";
                Longitude = Longitude + split[3].Substring(3) + "\'";
                Longitude = Longitude + split[4];
                longitude = Longitude;

                mapslink = url + Latitude + "+" + Longitude;
                // Create URL
                return true;
            }
            else return false;
        }
        public bool ParseGPGSA(string[] split) 
        {
            if (split[0] == "$GPGSA" && (split[15] != "" & split[16] != "" & split[17] != ""))
            {
                pdop = split[15];
                hdop = split[16];
                vdop = split[17].Substring(0, 3);

              return true;
            }
            else return false;
        }
        public bool ParseGPGSV(string[] split)
        {
            if (split[0] == "$GPGSV" && (split[3] != ""))
            {
                numberofsatelites = split[3];
                return true;
            }
            else return false;
        }
        public bool ParseGPVTG(string[] split)
        {
            if (split[0] == "$GPVTG" && (split[7] != ""))
            {
                speedKPH = split[7];
                return true;
            }
            else return false;
        }


        public bool IsValid(string x)
        {
            return x.Substring(x.IndexOf("*")+1, 2) == GetChecksum(x);
        }
        public string GetChecksum(string x)
        {
            // Loop through all chars to get a checksum
            int Checksum = 0;
            foreach (char Character in x)
            {
                if (Character == '$') {}
                else if (Character == '*') {break;}
                else
                {
                    if (Checksum == 0)
                    { 
                        Checksum = Convert.ToByte(Character); 
                    }
                    else
                    {
                        // XOR the checksum with character's value
                        Checksum = Checksum ^ Convert.ToByte(Character);
                    }
                }
            }
            // Return the checksum formatted as a two-character hexadecimal
            return Checksum.ToString("X2");
        }
    }
}
