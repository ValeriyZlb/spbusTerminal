using System;
using System.IO;
using System.IO.Ports;
using System.Text;
using System.Windows.Forms;

namespace spbusTerminal
{
    public partial class MainForm : Form
    {
        // Объявляем переменную bus принадлежащую классу Spbus, доступную во всем коде
        Spbus bus = new Spbus();
        byte[] ByteBuffer = new byte[4096];
        int buffseek = 0;
        bool buff_beginmsg = false, buff_endmsg = false, buff_crcmsg = false;
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
            string[] FNCs = { "19", "18" };
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
            string SendMsg = Send_textBox.Text.Replace(" ", "\t").Replace("!", "\f");
            
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
            byte DLE = 0x10, SOH = 0x1, STX = 0x02, ETX = 0x03;
            int ByteToRead = _serialPort.BytesToRead;
            byte[] buff = new byte[ByteToRead];

            _serialPort.Read(buff, 0, ByteToRead);

            Array.Copy(buff, 0, ByteBuffer, buffseek, ByteToRead);
            buffseek += ByteToRead;

            // Сканируем буфер на наличи кодов начала, конца сообщения и его контрольных чисел, с установкой флагов
            for (int i = 0; i < buffseek; i++)
            {
                if (ByteBuffer[i] == DLE && ByteBuffer[i + 1] == SOH) buff_beginmsg = true;
                if (ByteBuffer[i] == DLE && ByteBuffer[i + 1] == ETX) buff_endmsg = true;
                if (ByteBuffer[i] == DLE && ByteBuffer[i + 1] == ETX && ByteBuffer[i+2] > 0 && ByteBuffer[i+3] > 0) buff_crcmsg = true;
            }
            // Проверяем наличие начала, конца сообщения и его контрольных чисел
            // если успешно, то выполняем вложеный код
            if (buff_beginmsg && buff_endmsg && buff_crcmsg)
            {
                // Находим индексы вхождения кодов soh, isi, stx и etx в буфере приёма
                int soh = bus.FindWordBytes(ByteBuffer, 0x1001);
                int isi = bus.FindWordBytes(ByteBuffer, 0x101F);
                int stx = bus.FindWordBytes(ByteBuffer, 0x1002);
                int etx = bus.FindWordBytes(ByteBuffer, 0x1003);

                // Объявляем массив для целого сообщения и переносим в него сообщение из буфера приёма
                byte[] finalbuff = new byte[etx+4];
                Array.Copy(ByteBuffer, soh, finalbuff, 0, etx + 4);

                // Объявляем массивы заголовка, тела и контрольного числа
                byte[] Head = new byte[stx];
                byte[] Body = new byte[etx-stx+2];
                byte[] CRC = new byte[2];

                // Заполняем массывы заголовка, тела и контрольного числа
                Array.Copy(finalbuff, soh, Head, 0, stx);
                Array.Copy(finalbuff, stx, Body, 0, etx-stx+2);
                Array.Copy(finalbuff, etx + 2, CRC, 0, 2);

                string result = "";
                for (int i = 0; i < finalbuff.Length; i++)
                {
                    result += finalbuff[i].ToString("X2") + " ";
                }
                Console_textBox.Invoke(new Action(() => { Console_textBox.AppendText("Message Received: => {" + result + " }"); }));
                
                // Меняем местами значения в массиве CRC и сравниваем его сподсчитаным функцией CrCode2 контрольным числом
                // Если коды верны, то выполняем вложенный код
                Array.Reverse(CRC);
                if (BitConverter.ToInt16(CRC, 0) == bus.CrCode2(finalbuff))
                {
                    Console_textBox.Invoke(new Action(() => { Console_textBox.AppendText(" CRC: OK"); }));
                } else Console_textBox.Invoke(new Action(() => { Console_textBox.AppendText(" CRC: FAILED"); }));

                // Обработка-----------------------------------
                string newStr = Encoding.GetEncoding(866).GetString(finalbuff);
                string bodyStr = Encoding.GetEncoding(866).GetString(Body);
                // Удаляем символы DLE STX и первый TAB в начале сообщения и DLE ETX в конце сообщения
                bodyStr = bodyStr.Remove(0, 3);
                bodyStr = bodyStr.Remove(bodyStr.Length-2, 2);
                MessageBox.Show(bodyStr);


            }
            /**
            //Здесь принимаем поступающие на порт байты
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
                string Resmsg = bus.StackToString();
                if (bus.MsgBuilder(out int DAD, out int SAD, out int FNC, out string[] mess) == 32)
                {
                    string[] para = mess[0].Split('\t');
                    string[] data1 = mess[1].Split('\t');
                    string[] data2 = mess[2].Split('\t');
                    string dat1 = data1[1] + "." + data1[2] + "." + data1[3] + " " + data1[4] + ":" + data1[5] + ":" + data1[6];
                    string dat2 = data2[1] + "." + data2[2] + "." + data2[3] + " " + data2[4] + ":" + data2[5] + ":" + data2[6];
                    string.Join("", data1);
                    DateTime dt1, dt2;
                    int ch, pr;
                    int len = mess.Length - 5;
                    double[] dates = new double[len];
                    ch = int.Parse(para[1]);
                    pr = int.Parse(para[2]);
                    dt1 = Convert.ToDateTime(dat1.ToString());
                    dt2 = Convert.ToDateTime(dat2.ToString());
                    for (int i = 0; i < len; i++)
                    {
                        mess[i + 4] = mess[i + 4].Remove(0, 1);
                        dates[i] = double.Parse(mess[i + 4], System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
                    }
                    //Добавляем данные на форму Данных
                    string[] row = { "Hello", "World" };
                    Dates_dataGridView.Rows.Add(dt1.Day+"."+dt1.Month+"."+dt1.Year, dt2.TimeOfDay, para[0],para[1]);
                }
                else
                {
                    string[] para = mess[0].Split('\t');
                    string[] datas = new string[6];
                    Dates_dataGridView.ColumnCount = mess.Length+2;
                    Dates_dataGridView.Columns[0].Name = "Дата";
                    Dates_dataGridView.Columns[1].Name = "Время";
                    Dates_dataGridView.Columns[2].Name = "Канал";
                    Dates_dataGridView.Columns[3].Name = "Параметр";
                    for (int i = 0; i < mess.Length-2; i++)
                    {
                        datas = mess[i+1].Split('\t');
                        Dates_dataGridView.Columns[i + 4].Name = datas[0] + datas[1];

                    }
                }
            }
            **/
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
    public short CrCode2(byte[] msg)
    {
        int j, crc = 0, count = 2;
        int len = msg.Length-4;
        while (len-- > 0)
        {
            crc = crc ^ msg[count++] << 8;
            for (j = 0; j < 8; j++)
            {
                if ((crc & 0x8000) == 0x8000) crc = (crc << 1) ^ 0x1021;
                else crc <<= 1;
            }
        }
        byte crc1 = (byte)(crc >> 8);
        byte crc2 = (byte)crc;
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
        for (int i = 0; i < 0; i++) ;
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
    public void StackClear()
    {
        Array.Clear(MsgStack, 0, MsgStack.Length);
        StackBytes_count = 0;
    }
    public int StackCheck(byte[]searchpat)
    {
        byte[] msg = MsgStack;
        //Проверяем чтобы образец не был больше массива для поиска
        if (searchpat.Length > msg.Length) return -1;
        for (int i = 0; i < msg.Length - searchpat.Length; i++)
        {
            bool found = true;
            for (int j = 0; j < searchpat.Length; j++)
            {
                if (msg[i + j] != searchpat[j])
                {
                    found = false;
                    break;
                }
            }
            if (found) return i;
        }
        return -1;
    }
    public int MsgBuilder(out int DAD, out int SAD, out int FNC, out string[] mess)
    {
        byte[] pat1 = { 16, 1 };
        byte[] pat2 = { 16, 31 };
        byte[] pat3 = { 16, 2 };
        DAD = 0;
        SAD = 0;
        FNC = 0;
        int msgbeg;
        string msg = "";
        mess = new string[] { "" };
        DAD = MsgStack[this.StackCheck(pat1)+2];
        SAD = MsgStack[this.StackCheck(pat1) + 3];
        FNC = MsgStack[this.StackCheck(pat2) + 2];
        msgbeg = MsgStack[this.StackCheck(pat3) + 2];
        for (int i = msgbeg; i < StackBytes_count; i++)
            msg += (char)MsgStack[i];
        mess = msg.Split('\f');
        for (int i = 0; i < mess.Length; i++)
        {
            byte[] bytes = Encoding.GetEncoding(855).GetBytes(mess[i]);
            mess[i] = Encoding.GetEncoding(1251).GetString(bytes);
        }

        return FNC;
    }
    public int FindWordBytes(byte[] a, short x)
    {
        byte[] b = new byte[2];
        b[1] = (byte)(x & 0x00FF);
        b[0] = (byte)(x >> 8);

        if (b.Length > a.Length) return -1;
        for (int i = 0; i < a.Length - b.Length; i++)
        {
            bool found = true;
            for (int j = 0; j < b.Length; j++)
            {
                if (a[i + j] != b[j])
                {
                    found = false;
                    break;
                }
            }
            if (found) return i;
        }
        return -1;
    }
}
class BaseOfDates
{
    string[,] Base = new string[1,1];
    
    public void AddTitle()
    {

    }
}