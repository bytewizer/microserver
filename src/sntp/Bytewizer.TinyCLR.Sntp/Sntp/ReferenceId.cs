﻿namespace Bytewizer.TinyCLR.Sntp
{
    /// <summary>
    /// Represents the identifier of the reference source.
    /// </summary>
    public enum ReferenceId : uint
    {
        /// <summary>
        /// Uncalibrated local clock used as a primary reference for a subnet without external means of synchronization.
        /// </summary>
        LOCL = ('L' << 24) + ('O' << 16) + ('C' << 8) + 'L',

        /// <summary>
        /// Atomic clock or other pulse-per-second source individually calibrated to national standards.
        /// </summary>
        PPS = ('P' << 24) + ('P' << 16) + ('S' << 8),

        /// <summary>
        /// NIST dialup modem service.
        /// </summary>
        ACTS = ('A' << 24) + ('C' << 16) + ('T' << 8) + 'S',

        /// <summary>
        /// U.S. Naval Observatory modem service.
        /// </summary>
        USNO = ('U' << 24) + ('S' << 16) + ('N' << 8) + 'O',

        /// <summary>
        /// PTB (Germany) modem service.
        /// </summary>
        PTB = ('P' << 24) + ('T' << 16) + ('B' << 8),

        /// <summary>
        /// Allouis (France) Radio 164 kHz.
        /// </summary>
        TDF = ('T' << 24) + ('D' << 16) + ('F' << 8),

        /// <summary>
        /// Mainflingen (Germany) Radio 77.5 kHz.
        /// </summary>
        DCF = ('D' << 24) + ('C' << 16) + ('F' << 8),

        /// <summary>
        /// Rugby (UK) Radio 60 kHz.
        /// </summary>
        MSF = ('M' << 24) + ('S' << 16) + ('F' << 8),

        /// <summary>
        /// Ft. Collins (US) Radio 2.5, 5, 10, 15, 20 MHz.
        /// </summary>
        WWV = ('W' << 24) + ('W' << 16) + ('V' << 8),

        /// <summary>
        /// Boulder (US) Radio 60 kHz.
        /// </summary>
        WWVB = ('W' << 24) + ('W' << 16) + ('V' << 8) + 'B',

        /// <summary>
        /// Kaui Hawaii (US) Radio 2.5, 5, 10, 15 MHz.
        /// </summary>
        WWVH = ('W' << 24) + ('W' << 16) + ('V' << 8) + 'H',

        /// <summary>
        /// Ottawa (Canada) Radio 3330, 7335, 14670 kHz.
        /// </summary>
        CHU = ('C' << 24) + ('H' << 16) + ('U' << 8),

        /// <summary>
        /// LORAN-C radionavigation system.
        /// </summary>
        LORC = ('L' << 24) + ('O' << 16) + ('R' << 8) + 'C',

        /// <summary>
        /// OMEGA radionavigation system.
        /// </summary>
        OMEG = ('O' << 24) + ('M' << 16) + ('E' << 8) + 'G',

        /// <summary>
        /// Global Positioning Service.
        /// </summary>
        GPS = ('G' << 24) + ('P' << 16) + ('S' << 8),

        /// <summary>
        /// Geostationary Orbit Environment Satellite.
        /// </summary>
        GOES = ('G' << 24) + ('E' << 16) + ('O' << 8) + 'S',
    }
}