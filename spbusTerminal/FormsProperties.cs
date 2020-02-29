namespace spbusTerminal
{
    using System.IO.Ports;
    using System.Windows.Forms;
    class FormControl
    {
        public void BaudRatesInit(ComboBox br)
        {
            string[] BaudRates = { "300", "600", "1200", "2400", "4800", "9600", "19200", "38400", "57600", "115200" };
            br.Items.AddRange(BaudRates);
            br.SelectedIndex = 4;
        }
        public void ParityInit(ComboBox pr)
        {
            string[] PortParity = { "Even", "Odd", "None", "Mark", "Space" };
            pr.Items.AddRange(PortParity);
            pr.SelectedIndex = 2;
        }
        public void StopBitsInit(ComboBox sb)
        {
            string[] PortStopBits = { "0", "1", "1.5", "2" };
            sb.Items.AddRange(PortStopBits);
            sb.SelectedIndex = 1;
        }
        public void DataBitsInit(ComboBox db)
        {
            string[] PortDataBits = { "5", "6", "7", "8" };
            db.Items.AddRange(PortDataBits);
            db.SelectedIndex = 3;
        }
        public void FNCInit(ComboBox fnc)
        {
            string[] FNCs = { "19", "18" };
            fnc.Items.AddRange(FNCs);
            fnc.SelectedIndex = 0;
        }
        public bool PortInit(ComboBox prt)
        {
            prt.Items.AddRange(SerialPort.GetPortNames());

            if (prt.Items.Count > 1)
            {
                prt.SelectedIndex = 1;
                return true;
            } else if (prt.Items.Count > 0)
            {
                prt.SelectedIndex = 0;
                return true;
            }
            else return false;
        }
    }
}
