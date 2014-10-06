// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Sun.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Calculation of sunrise/sunset
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using System;

    /// <summary>
    /// Calculation of sunrise/sunset
    /// </summary>
    /// <remarks>http://williams.best.vwh.net/sunrise_sunset_algorithm.htm
    /// based on code by Huysentruit Wouter, Fastload-Media.be</remarks>
    public static class Sun
    {
        private static double Deg2Rad(double angle)
        {
            return Math.PI * angle / 180.0;
        }

        private static double Rad2Deg(double angle)
        {
            return 180.0 * angle / Math.PI;
        }

        private static double FixValue(double value, double min, double max)
        {
            while (value < min)
            {
                value += max - min;
            }

            while (value >= max)
            {
                value -= max - min;
            }

            return value;
        }

        public static DateTime Calculate(DateTime date, double latitude, double longitude, bool sunrise, Func<DateTime, DateTime> utcToLocalTime, double zenith = 90.5)
        {
            // 1. first calculate the day of the year
            int n = date.DayOfYear;

            // 2. convert the longitude to hour value and calculate an approximate time
            double lngHour = longitude / 15.0;

            double t;

            if (sunrise)
            {
                t = n + ((6.0 - lngHour) / 24.0);
            }
            else
            {
                t = n + ((18.0 - lngHour) / 24.0);
            }

            // 3. calculate the Sun's mean anomaly
            double m = (0.9856 * t) - 3.289;

            // 4. calculate the Sun's true longitude
            double l = m + (1.916 * Math.Sin(Deg2Rad(m))) + (0.020 * Math.Sin(Deg2Rad(2 * m))) + 282.634;
            l = FixValue(l, 0, 360);

            // 5a. calculate the Sun's right ascension
            double ra = Rad2Deg(Math.Atan(0.91764 * Math.Tan(Deg2Rad(l))));
            ra = FixValue(ra, 0, 360);

            // 5b. right ascension value needs to be in the same quadrant as L
            double lquadrant = Math.Floor(l / 90.0) * 90.0;
            double raquadrant = Math.Floor(ra / 90.0) * 90.0;
            ra = ra + (lquadrant - raquadrant);

            // 5c. right ascension value needs to be converted into hours
            ra = ra / 15.0;

            // 6. calculate the Sun's declination
            double sinDec = 0.39782 * Math.Sin(Deg2Rad(l));
            double cosDec = Math.Cos(Math.Asin(sinDec));

            // 7a. calculate the Sun's local hour angle
            double cosH = (Math.Cos(Deg2Rad(zenith)) - (sinDec * Math.Sin(Deg2Rad(latitude)))) /
                          (cosDec * Math.Cos(Deg2Rad(latitude)));

            // 7b. finish calculating H and convert into hours
            double h;

            if (sunrise)
            {
                h = 360.0 - Rad2Deg(Math.Acos(cosH));
            }
            else
            {
                h = Rad2Deg(Math.Acos(cosH));
            }

            h = h / 15.0;

            // 8. calculate local mean time of rising/setting
            double localMeanTime = h + ra - (0.06571 * t) - 6.622;

            // 9. adjust back to UTC
            double utc = localMeanTime - lngHour;

            // 10. convert UT value to local time zone of latitude/longitude
            date = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, DateTimeKind.Utc);
            var utctime = date.AddHours(utc);
            var localTime = utcToLocalTime(utctime);

            utc = (localTime - date).TotalHours;
            utc = FixValue(utc, 0, 24);
            return date.AddHours(utc);
        }
    }

    /*
    Sunrise/Sunset Algorithm

    Source:
        Almanac for Computers, 1990
        published by Nautical Almanac Office
        United States Naval Observatory
        Washington, DC 20392

    Inputs:
        day, month, year:      date of sunrise/sunset
        latitude, longitude:   location for sunrise/sunset
        zenith:                Sun's zenith for sunrise/sunset
          offical      = 90 degrees 50'
          civil        = 96 degrees
          nautical     = 102 degrees
          astronomical = 108 degrees

        NOTE: longitude is positive for East and negative for West
            NOTE: the algorithm assumes the use of a calculator with the
            trig functions in "degree" (rather than "radian") mode. Most
            programming languages assume radian arguments, requiring back
            and forth convertions. The factor is 180/pi. So, for instance,
            the equation RA = atan(0.91764 * tan(L)) would be coded as RA
            = (180/pi)*atan(0.91764 * tan((pi/180)*L)) to give a degree
            answer with a degree input for L.

    1. first calculate the day of the year

        N1 = floor(275 * month / 9)
        N2 = floor((month + 9) / 12)
        N3 = (1 + floor((year - 4 * floor(year / 4) + 2) / 3))
        N = N1 - (N2 * N3) + day - 30

    2. convert the longitude to hour value and calculate an approximate time

        lngHour = longitude / 15

        if rising time is desired:
          t = N + ((6 - lngHour) / 24)
        if setting time is desired:
          t = N + ((18 - lngHour) / 24)

    3. calculate the Sun's mean anomaly

        M = (0.9856 * t) - 3.289

    4. calculate the Sun's true longitude

        L = M + (1.916 * sin(M)) + (0.020 * sin(2 * M)) + 282.634
        NOTE: L potentially needs to be adjusted into the range [0,360) by adding/subtracting 360

    5a. calculate the Sun's right ascension

        RA = atan(0.91764 * tan(L))
        NOTE: RA potentially needs to be adjusted into the range [0,360) by adding/subtracting 360

    5b. right ascension value needs to be in the same quadrant as L

        Lquadrant  = (floor( L/90)) * 90
        RAquadrant = (floor(RA/90)) * 90
        RA = RA + (Lquadrant - RAquadrant)

    5c. right ascension value needs to be converted into hours

        RA = RA / 15

    6. calculate the Sun's declination

        sinDec = 0.39782 * sin(L)
        cosDec = cos(asin(sinDec))

    7a. calculate the Sun's local hour angle

        cosH = (cos(zenith) - (sinDec * sin(latitude))) / (cosDec * cos(latitude))

        if (cosH >  1)
          the sun never rises on this location (on the specified date)
        if (cosH < -1)
          the sun never sets on this location (on the specified date)

    7b. finish calculating H and convert into hours

        if if rising time is desired:
          H = 360 - acos(cosH)
        if setting time is desired:
          H = acos(cosH)

        H = H / 15

    8. calculate local mean time of rising/setting

        T = H + RA - (0.06571 * t) - 6.622

    9. adjust back to UTC

        UT = T - lngHour
        NOTE: UT potentially needs to be adjusted into the range [0,24) by adding/subtracting 24

    10. convert UT value to local time zone of latitude/longitude

        localT = UT + localOffset

         */
}