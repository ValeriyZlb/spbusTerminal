using System;
using System.IO.Ports;
using System.Text;
using System.Windows.Forms;

namespace spbusTerminal
{
    public partial class MainForm : Form
    {
        // Объявляем переменную bus принадлежащую классу Spbus, доступную во всем коде
        Spbus bus = new Spbus();
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            //Раскидываем управление по умолчанию на форме и
            //Строки ниже содержат списки возможных настроек COM порта, в combobox-ах
            string[] BaudRates = { "300", "600", "1200", "2400", "4800", "9600", "19200", "38400", "57600", "115200" };
            string[] PortParity = { "Even", "Odd", "None", "Mark", "Space" };
            string[] PortStopBits = { "0", "1", "1.5", "2" };
            string[] PortDataBits = { "5", "6", "7", "8" };
            string[] FNCs = { "18", "19", "21" };
            //Заполнение combobox порта списком доступных для подключения портов, полученый методом GetPortNames 
            Port_comboBox.Items.AddRange(SerialPort.GetPortNames());
            //Заполняем часть combobox-ов настроек порта (данными из массивов строк объявленых и инициализированных выше)
            BaudRate_comboBox.Items.AddRange(BaudRates);
            Parity_comboBox.Items.AddRange(PortParity);
            StopBits_comboBox.Items.AddRange(PortStopBits);
            DataBits_comboBox.Items.AddRange(PortDataBits);
            FNC_comboBox.Items.AddRange(FNCs);
            //Заполняем остальную часть combobox-ов настроек порта цифровыми значениями по умолчанию
            BaudRate_comboBox.SelectedIndex = 4;
            Parity_comboBox.SelectedIndex = 2;
            StopBits_comboBox.SelectedIndex = 1;
            DataBits_comboBox.SelectedIndex = 3;
            //Устанавливаем по умолчанию второй в списке COM-порт (для локальных тестов),
            //только если он есть (второй порт), иначе ставим первый из списа, или блокируем кнопку если порты отсутствуют вовсе
            if (Port_comboBox.Items.Count > 1) Port_comboBox.SelectedIndex = 1;
            else if (Port_comboBox.Items.Count > 0) Port_comboBox.SelectedIndex = 0;
            else OpenPort_button.Enabled = false;
            //Установка настроек вида сообщений в консоли: в  шестнадцатиричном, или текстовом виде
            FNC_comboBox.SelectedIndex = 0;
            Text_radioButton.Checked = true;
            /**
             * Send_textBox.Text = "HT0HT65530FFHT5HT2HT20HT8HT1HT0FF"; //Первый вариант форматирования сообщений
             * Собираем строку запроса
             * Поле DataSet состоит из двух указателей. Первый имеет следующую структуру:
             * HT Ссылочный номер канала HT Ссылочный номер параметра FF
             * Временной срез определяется вторым указателем. Его структура такова:
             * HT День HT Месяц HT Год HT Час HT Минуты HT Секунды FF
             * Формат запроса = пробел - HT (09h), ! - FF (0Ch)
             *  HT_Channel_HT_Parameter_FFHT_Day_HT_Mounth_HT_Year_HT_Hour_HT_Minutes_HT_Seconds_FF
             *  где Channel -канал, Parameter - параметр, Day - день, Mounth - месяц, Year - год, Hour - часы, Minutes - минуты, Seconds - секунды
            **/
            Send_textBox.Text = " 0 65530! 5 2 20 8 1 0!";
        }

        private void OpenPort_button_Click(object sender, EventArgs e)
        {
            if (!_serialPort.IsOpen)
            {
                /**
                 * Действия совершаеые при нажатии кнопки Open (открытие порта)
                **/
                //Если порт закрыт, прописываем в него параметры с формы и открываем
                int[] BaudRate = { 300, 600, 1200, 2400, 4800, 9600, 19200, 38400, 57600, 115200 };
                int[] port_databits = { 5, 6, 7, 8 };
                Parity[] port_parity = { Parity.Even, Parity.Mark, Parity.None, Parity.Odd, Parity.Space };
                StopBits[] port_stopbits = { StopBits.None, StopBits.One, StopBits.OnePointFive, StopBits.Two };
                _serialPort.PortName = Port_comboBox.Text;
                _serialPort.BaudRate = BaudRate[BaudRate_comboBox.SelectedIndex];
                _serialPort.Parity = port_parity[Parity_comboBox.SelectedIndex];
                _serialPort.StopBits = port_stopbits[StopBits_comboBox.SelectedIndex];
                _serialPort.DataBits = port_databits[DataBits_comboBox.SelectedIndex];
                //Открываем порт
                _serialPort.Open();
                //Если порт благополучно открывается делаем кнопку открытия недоступной,
                //а кнопку закрытия напротив доступной, а в консоль выводим сообщение
                //с датой и временем открытия порта
                if (_serialPort.IsOpen)
                {
                    OpenPort_button.Enabled = false;
                    ClosePort_button.Enabled = true;
                    Console_textBox.AppendText("Port opened at " + DateTime.Now + "\r\n");
                }
            }
        }

        private void ClosePort_button_Click(object sender, EventArgs e)
        {
            //Если CJOM-порт открыт то закрываем его и выводим в консоль сообщение об этом,
            //а кнопку закрытия делаем неактивной, а открытия активной
            if (_serialPort.IsOpen)
            {
                _serialPort.Close();
                ClosePort_button.Enabled = false;
                OpenPort_button.Enabled = true;
                Console_textBox.AppendText("Port closed at " + DateTime.Now + "\r\n");
            }
        }

        private void Send_button_Click(object sender, EventArgs e)
        {
            /**
             * Собираем строку запроса
             * Поле DataSet состоит из двух указателей. Первый имеет следующую структуру:
             * HT Ссылочный номер канала HT Ссылочный номер параметра FF
             * Временной срез определяется вторым указателем. Его структура такова:
             * HT День HT Месяц HT Год HT Час HT Минуты HT Секунды FF
             * Формат запроса = пробел - HT (09h), ! - FF (0Ch)
             *  HT_Channel_HT_Parameter_FFHT_Day_HT_Mounth_HT_Year_HT_Hour_HT_Minutes_HT_Seconds_FF
             *  где Channel -канал, Parameter - параметр, Day - день, Mounth - месяц, Year - год, Hour - часы, Minutes - минуты, Seconds - секунды
            **/

            //Инициалиируем переменные адресов данные из полей на форме,
            //К адресы SAD побитово добавляем 128, или 80h
            //А поле FNC переводим из шестнадцатиричной системы в десятичную
            byte SAD = (byte)((byte)SAD_numericUpDown.Value | 0x80);
            byte DAD = (byte)DAD_numericUpDown.Value;
            byte FNC = byte.Parse(FNC_comboBox.Text, System.Globalization.NumberStyles.HexNumber);

            //Инициализируеем строку сообщения из соответствующего текстового поля на форме
            //и заменяем в ней пробелы на \t (09h), а знаки ! на \f (0Ch)
            string SendMsg = Send_textBox.Text;
            SendMsg = SendMsg.Replace(" ", "\t");
            SendMsg = SendMsg.Replace("!", "\f");
            
            //Вызываем метод CreateMessage для создания цельного сообщения из кусочков
            //и записываем его в байтовый массив msg
            byte[] msg = bus.CreateMessage(DAD, SAD, FNC, SendMsg);

            //Вызываем методы очищения стека и  добавляем туда сообщение
            bus.StackClear();
            bus.AddToStack(msg);
            //Если выбран режим отображения в шестнадцатиричном виде то выводим содержимое стека в виде
            //шестнадцаричных чисел, иначе в текстовом виде
            if(HEX_radioButton.Checked) Console_textBox.AppendText(bus.StackToHEX() + "\r\r\n");
            else if (Text_radioButton.Checked) Console_textBox.AppendText(bus.StackToString() + "\r\r\n");
            //Вызываем метод очищения стека
            bus.StackClear();
            //Если COM-порт открыт, то пишем содержимое массива msg в порт,
            //если порт не доступен, выводим сообщение об ошибке
            if (_serialPort.IsOpen) _serialPort.Write(msg, 0, msg.Length);
            else MessageBox.Show("Порт не открыт", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }

        private void _serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            /**
             * Здесь принимаем поступающие на порт байты
            **/
            //Инициализируем переменную количеством принятых байтов и
            //объявляем массив buff с такой размерностью
            int BytesRead = _serialPort.BytesToRead;
            byte[] buff = new byte[BytesRead];
            //Считываем в массив buff с самого его начала байты, в количестве определенном в BytesRead
            _serialPort.Read(buff, 0, BytesRead);
            //Добавляем принятые байты в стек
            bus.AddToStack(buff);
            //Проверяем на знаки конца сообщения, посредством метода проверяющего стек на конец сообщения
            //а затем если конец сообщения найден, вызываем метод выводящий содержимое стека в шестнадцатиричном виде,
            //или текстовом в консоль, в зависимости от выбранного режима
            if (bus.Stack_ifMsgEnd())
            {
                if (HEX_radioButton.Checked) Console_textBox.Invoke(new Action(() => {Console_textBox.AppendText(Port_comboBox.Text + " => " + bus.StackToHEX());}));
                else if (Text_radioButton.Checked) Console_textBox.Invoke(new Action(() => { Console_textBox.AppendText(Port_comboBox.Text + " => " + bus.StackToString()); }));
            }
        }

        private void Test_button_Click(object sender, EventArgs e)
        {
            Console_textBox.Clear();
        }
    }
}
/**
 * Класс Spbus
 * ******************************************************************
**/
class Spbus
{
    //Объявляем массив для стека и переменную количества байт содержащиеся в стеке
    byte[] MsgStack = new byte[1000];
    public int StackBytes_count = 0;
    public short CrCode(byte[] msg, out byte crc1, out byte crc2)
    {
        int j, crc = 0, count = 0;
        int len = msg.Length;
        while (len-- > 0)
        {
            crc = crc ^ msg[count++] << 8;
            for (j = 0; j < 8; j++)
            {
                if ((crc & 0x8000) == 0x8000) crc = (crc << 1) ^ 0x1021;
                else crc <<= 1;
            }
        }
        crc1 = (byte)(crc >> 8);
        crc2 = (byte)crc;
        return (short)(crc & 0xFFFF);
    }
    public byte[] CompMessage(char[] msg, short crc)
    {
        byte crc1 = (byte)(crc >> 8);
        byte crc2 = (byte)crc;
        int len = msg.Length;
        byte[] temp = new byte[len + 4];
        for (int i = 0; i < len; i++)
        {
            temp[i + 2] = (byte)msg[i];
        }
        temp[0] = 16;
        temp[1] = 1;
        temp[len + 2] = crc1;
        temp[len + 3] = crc2;
        return temp;
    }
    public byte[] CreateMessage(byte DAD, byte SAD, byte FNC, string SendMsg)
    {
        short crc;
        byte crc1, crc2;
        Spbus bus = new Spbus();

        byte[] msgA = { 16, 1, DAD, SAD, 16, 31, FNC, 16, 2 };
        byte[] msgB = Encoding.ASCII.GetBytes(SendMsg);
        byte[] msgC = { 16, 3 };
        byte[] msgD = new byte[msgA.Length + msgB.Length + msgC.Length - 2];
        byte[] msgE = new byte[msgD.Length + 4];

        Array.Copy(msgA, 2, msgD, 0, msgA.Length - 2);
        Array.Copy(msgB, 0, msgD, msgA.Length - 2, msgB.Length);
        Array.Copy(msgC, 0, msgD, msgA.Length + msgB.Length - 2, msgC.Length);
        Array.Copy(msgD, 0, msgE, 2, msgD.Length);
        crc = bus.CrCode(msgD, out crc1, out crc2);
        msgE[0] = 16;
        msgE[1] = 01;
        msgE[msgE.Length - 2] = crc1;
        msgE[msgE.Length - 1] = crc2;

        return msgE;
    }
    public void AddToStack(byte[] buff)
    {
        Array.Copy(buff, 0, MsgStack, StackBytes_count, buff.Length);
        StackBytes_count += buff.Length;
    }
    public bool Stack_ifMsgEnd()
    {
        for (int i = 0; i < StackBytes_count-1; i++)
        if(MsgStack[i] == 16 && MsgStack[i+1] == 3 && MsgStack[i+3] > 0) return true;
        return false;
    }
    public string StackToHEX()
    {
        string result = "";
        for (int i = 0; i < StackBytes_count; i++)
        {
            result += MsgStack[i].ToString("X2") + " ";
        }
        return result;
    }
    public string StackToString()
    {
        string result = "";
        for (int i = 0; i < StackBytes_count; i++)
        {
            result += (char)MsgStack[i];
        }
        return result;
    }
    public string StackToString2()
    {
        byte[] bytes= new byte[StackBytes_count];
        Array.Copy(MsgStack, 0, bytes, 0, StackBytes_count);
        return Encoding.GetEncoding(866).GetString(bytes); ;
    }
    public void StackClear()
    {
        Array.Clear(MsgStack, 0, MsgStack.Length);
        StackBytes_count = 0;
    }
}