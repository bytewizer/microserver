namespace Bytewizer.TinyCLR.Telnet
{
    public enum TelnetOptions : byte
    {                //Decimal code	 Name	                RFC
        E = 1,       //echo	                                857
        R = 2,       //Reconnection	            
        SGA = 3,     //suppress go ahead	                858
        AMSN = 4,    //Approx Message Size Negotiation
        S = 5,       //status	                            859
        TM = 6,      //timing mark	                        860
        RCT = 7,     //Remote Controlled Trans and Echo	            
        OLW = 8,     //Output Line Width	            
        OPS = 9,     //Output Page Size 	            
        OCRD = 10,   //Output Carriage-Return Disposition	            
        OHTS = 11,   //Output Horizontal Tab Stops 	            
        OHTD = 12,   //Output Horizontal Tab Disposition  	            
        OFD = 13,    //Output Formfeed Disposition  	            
        OVT = 14,    //Output Vertical Tabstops   	            
        OVTD = 15,   //Output Vertical Tab Disposition   	            
        OLD = 16,    //Output Linefeed Disposition  	            
        EA = 17,     //Extended ASCII  	            
        LO = 18,     //Logout  	            
        BM = 19,     //Byte Macro  	            
        DET = 20,    //Data Entry Terminal 	            
        SD = 21,     //SUPDUP 	            
        SDO = 22,    //SUPDUP Output 	            
        SL = 23,     //Send Location 	            
        TT = 24,     //terminal type	                    1091
        EOR = 25,    //End of Record
        TUI = 26,    //TACACS User Identification
        OM = 27,     //Output Marking
        TLN = 28,    //Terminal Location Number
        TR = 29,     //Telnet 3270 Regime 
        PAD = 30,    //X.3 PAD
        WS = 31,     //window size	                        1073
        TS = 32,     //terminal speed	                    1079
        RFC = 33,    //remote flow control	                1372
        LM = 34,     //linemode	                            1184
        XDL = 35,    //X Display Location
        EV = 36,     //environment variables	            1408
        AO = 37,     //Authentication Option
        EO = 38,     //Encryption Option
        EOL = 255,   //Extended-Options-List
    }
}
