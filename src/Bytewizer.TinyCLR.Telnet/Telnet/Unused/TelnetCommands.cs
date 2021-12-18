namespace Bytewizer.TinyCLR.Telnet
{
    //Telnet commands.  From RFC 854.
    public enum TelnetCommands : byte
    {
        SE = 240,    //End of subnegotiation parameters
        NOP = 241,   //No Operation
        DM = 242,    //Data Mark
        BRK = 243,   //Break
        IP = 244,    //Suspend
        AO = 245,    //Abort Output
        AYT = 246,   //Are You There
        EC = 247,    //Erase Character
        EL = 248,    //Erase Line
        GA = 249,    //Go Ahead
        SB = 250,    //Subnegotiation
        WILL = 251,  //Will
        WONT = 252,  //Won't
        DO = 253,    //Do
        DONT = 254,  //Don't
        IAC = 255    //Interpret As Command
    }
}
