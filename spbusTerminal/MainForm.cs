using System;
using System.IO.Ports;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace spbusTerminal
{

    public partial class MainForm : Form
    {
        // Объявляем переменную bus принадлежащую классу Spbus, доступную во всем коде
        Spbus bus = new Spbus();
        Messages work_msg = new Messages();
        byte[] ByteBuffer = new byte[4096];
        int buffseek = 0;
        public MainForm()
        {
            InitializeComponent();
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            // Заполнение компонентов настройки COM порта и выбор параметров по умолчанию.
            FormControl StartProperties = new FormControl();
            StartProperties.BaudRatesInit(BaudRate_comboBox);
            StartProperties.ParityInit(Parity_comboBox);
            StartProperties.StopBitsInit(StopBits_comboBox);
            StartProperties.DataBitsInit(DataBits_comboBox);
            StartProperties.FNCInit(FNC_comboBox);
            if(!StartProperties.PortInit(Port_comboBox)) OpenPort_button.Enabled = false;

            HEX_radioButton.Checked = true;
            /*
             * Send_textBox.Text = "HT0HT65530FFHT5HT2HT20HT8HT1HT0FF"; //Первый вариант форматирования сообщений
             * Собираем строку запроса
             * Поле DataSet состоит из двух указателей. Первый имеет следующую структуру:
             * HT Ссылочный номер канала HT Ссылочный номер параметра FF
             * Временной срез определяется вторым указателем. Его структура такова:
             * HT День HT Месяц HT Год HT Час HT Минуты HT Секунды FF
             * Формат запроса = пробел - HT (09h), ! - FF (0Ch)
             *  HT_Channel_HT_Parameter_FFHT_Day_HT_Mounth_HT_Year_HT_Hour_HT_Minutes_HT_Seconds_FF
             *  где Channel -канал, Parameter - параметр, Day - день, Mounth - месяц, Year - год, Hour - часы, Minutes - минуты, Seconds - секунды
             */
            Send_textBox.Text = " 0 65530! 5 2 20 8 0 0!";
        }

        private void OpenPort_button_Click(object sender, EventArgs e)
        {
            if (!_serialPort.IsOpen)
            {
                // Если порт закрыт, прописываем в него параметры с формы и открываем.
                //int[] BaudRate = { 300, 600, 1200, 2400, 4800, 9600, 19200, 38400, 57600, 115200 };
                int[] port_databits = { 5, 6, 7, 8 };
                Parity[] port_parity = { Parity.Even, Parity.Mark, Parity.None, Parity.Odd, Parity.Space };
                StopBits[] port_stopbits = { StopBits.None, StopBits.One, StopBits.OnePointFive, StopBits.Two };
                _serialPort.PortName = Port_comboBox.Text;
                //_serialPort.BaudRate = BaudRate[BaudRate_comboBox.SelectedIndex];
                _serialPort.Parity = port_parity[Parity_comboBox.SelectedIndex];
                _serialPort.StopBits = port_stopbits[StopBits_comboBox.SelectedIndex];
                _serialPort.DataBits = port_databits[DataBits_comboBox.SelectedIndex];
                _serialPort.BaudRate = Int32.Parse(BaudRate_comboBox.Text);

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

            //Инициализируем переменные адресов данные из полей на форме,
            //К адресу SAD побитово добавляем 128, или 80h
            //А поле FNC переводим из шестнадцатеричной системы в десятичную
            byte[] Head = work_msg.MakeHead((byte)DAD_numericUpDown.Value, (byte)SAD_numericUpDown.Value, byte.Parse(FNC_comboBox.Text, System.Globalization.NumberStyles.HexNumber), "");
            byte SAD = (byte)((byte)SAD_numericUpDown.Value | 0x80);
            byte DAD = (byte)DAD_numericUpDown.Value;
            byte FNC = byte.Parse(FNC_comboBox.Text, System.Globalization.NumberStyles.HexNumber);

            //Инициализируем строку сообщения из соответствующего текстового поля на форме
            //и заменяем в ней пробелы на \t (09h), а знаки ! на \f (0Ch)
            string SendMsg = Send_textBox.Text.Replace(" ", "\t").Replace("!", "\f");
            
            //Вызываем метод CreateMessage для создания цельного сообщения из кусочков
            //и записываем его в байтовый массив msg
            byte[] msg = bus.CreateMessage(DAD, SAD, FNC, SendMsg);

            //Вызываем методы очищения стека и  добавляем туда сообщение
            bus.StackClear();
            bus.AddToStack(msg);
            //Если выбран режим отображения в шестнадцатеричном виде то выводим содержимое стека в виде
            //шестнадцатеричных чисел, иначе в текстовом виде
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
            int ByteToRead = _serialPort.BytesToRead;
            byte[] buff = new byte[ByteToRead];

            _serialPort.Read(buff, 0, ByteToRead);

            Array.Copy(buff, 0, ByteBuffer, buffseek, ByteToRead);
            buffseek += ByteToRead;

            // Проверяем наличие начала, конца сообщения и его контрольных чисел
            // если успешно, то выполняем вложенный код
            if (work_msg.isFullMessage(ByteBuffer, buffseek))
            {
                // Находим индексы вхождения кодов soh, isi, stx и etx в буфере приёма
                int soh = work_msg.GetSOH(ByteBuffer);
                //int isi = work_msg.GetISI(ByteBuffer);
                int stx = work_msg.GetSTX(ByteBuffer);
                int etx = work_msg.GetETX(ByteBuffer);

                // Объявляем массив для целого сообщения и переносим в него сообщение из буфера приёма
                byte[] finalbuff = new byte[etx+4];
                Array.Copy(ByteBuffer, soh, finalbuff, 0, etx + 4);

                // Объявляем и заполняем массивы заголовка, тела и контрольного числа
                byte[] Head = work_msg.GetHead(finalbuff, soh, stx);
                byte[] Body = work_msg.GetBody(finalbuff, stx, etx - stx + 2);
                byte[] CRC = work_msg.GetBody(finalbuff, etx + 2, 2);

                string result = "";
                for (int i = 0; i < finalbuff.Length; i++)
                {
                    result += finalbuff[i].ToString("X2") + " ";
                }
                Console_textBox.Invoke(new Action(() => { Console_textBox.AppendText("Message Received: => {" + result + " }"); }));
                
                // Меняем местами значения в массиве CRC и сравниваем его с подсчитанным функцией CrCode2 контрольным числом
                // Если коды верны, то выполняем вложенный код
                Array.Reverse(CRC);
                if (BitConverter.ToInt16(CRC, 0) == bus.CrCode2(finalbuff))
                {
                    Console_textBox.Invoke(new Action(() => { Console_textBox.AppendText(" CRC: OK" + "\r\n"); }));
                } else Console_textBox.Invoke(new Action(() => { Console_textBox.AppendText(" CRC: FAILED" + "\r\n"); }));

                string[] result_str = work_msg.GetStrings(Body);
                for (int i = 0; i < result_str.Length; i++)
                    Console_textBox.Invoke(new Action(() => { Console_textBox.AppendText(result_str[i] + "\r\n"); }));
                //MessageBox.Show(bodyStr);

                Array.Clear(ByteBuffer, 0, ByteBuffer.Length);
                buffseek = 0;
                work_msg.DropSendFlag();
            }
        }

        private void Test_button_Click(object sender, EventArgs e)
        {
            Console_textBox.Clear();
        }

        private void auto_button_Click(object sender, EventArgs e)
        {
            auto_button.Enabled = false;
            //AutoThread kuku = new AutoThread("AutoThread", work_msg);
            Thread AutoThr = new Thread(Lol);
            AutoThr.Start();
        }
        private void Lol()
        {
            Console_textBox.Invoke(new Action(() => { Console_textBox.AppendText("Опрос Начат" + "\r\n"); }));
            Thread.Sleep(2000);
            for (int i = 8; i < 12; i++)
            {
                work_msg.SetSendFlag();
                if (work_msg.isSendFlag())
                {
                    Console_textBox.Invoke(new Action(() => { Console_textBox.AppendText("Запрос данных..." + "\r\n"); }));
                    
                    byte SAD = (byte)((byte)SAD_numericUpDown.Value | 0x80);
                    byte DAD = (byte)DAD_numericUpDown.Value;
                    byte FNC = 0x18;
                    string SendMsg = "\t0\t65530\f\t5\t2\t20\t"+i+"\t0\t0\f";
                    byte[] msg = bus.CreateMessage(DAD, SAD, FNC, SendMsg);
                    bus.StackClear();
                    bus.AddToStack(msg);
                    if (HEX_radioButton.Checked) Console_textBox.Invoke(new Action(() => {
                        Console_textBox.AppendText(bus.StackToHEX() + "\r\r\n"); }));
                    else if (Text_radioButton.Checked) Console_textBox.AppendText(bus.StackToString() + "\r\r\n");
                    bus.StackClear();
                    if (_serialPort.IsOpen) _serialPort.Write(msg, 0, msg.Length);
                    else
                    {
                        Console_textBox.Invoke(new Action(() => {Console_textBox.AppendText("Ошибка: порт не открыт!");}));
                        break;
                        //MessageBox.Show("Порт не открыт", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }
                while (work_msg.isSendFlag()) 
                {
                    Thread.Sleep(1000);
                }
                
            }
            Console_textBox.Invoke(new Action(() => { Console_textBox.AppendText("Опрос завершён" + "\r\n"); }));
            auto_button.Invoke(new Action(() => { auto_button.Enabled = true; }));
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
class Messages
{
    bool msgSendFlag = false;
    public byte[] GetHead(byte[] sourceArray, int sourceIndex, int lenght)
    {
        // Объявляем массивы заголовка
        byte[] Head = new byte[lenght];

        // Заполняем массывы заголовка
        Array.Copy(sourceArray, sourceIndex, Head, 0, lenght);
        return Head;
    }
    public byte[] GetBody(byte[] sourceArray, int sourceIndex, int lenght)
    {
        // Объявляем массивы заголовка
        byte[] Body = new byte[lenght];

        // Заполняем массывы заголовка
        Array.Copy(sourceArray, sourceIndex, Body, 0, lenght);
        return Body;
    }
    public byte[] GetCRC(byte[] sourceArray, int sourceIndex, int lenght)
    {
        // Объявляем массивы заголовка
        byte[] CRC = new byte[lenght];

        // Заполняем массывы заголовка
        Array.Copy(sourceArray, sourceIndex, CRC, 0, lenght);
        return CRC;
    }
    public byte[] MakeHead(byte destAdd, byte sourceAdd, byte FNC, string DataHead)
    {
        
        byte[] Head = Encoding.UTF8.GetBytes(DataHead);
        return Head;
    }
    public int GetSOH(byte[] sourceArray)
    {
        return FindWordBytes(sourceArray, 0x1001);
    }
    public int GetISI(byte[] sourceArray)
    {
        return FindWordBytes(sourceArray, 0x101F);
    }
    public int GetSTX(byte[] sourceArray)
    {
        return FindWordBytes(sourceArray, 0x1002);
    }
    public int GetETX(byte[] sourceArray)
    {
        return FindWordBytes(sourceArray, 0x1003);
    }
    public string[] GetStrings(byte[] sourceArray)
    {
        // Получаем строку из буфера байтов в кодировке 866
        string bodyStr = Encoding.GetEncoding(866).GetString(sourceArray);
        // Удаляем символы DLE STX и первый TAB в начале сообщения и DLE ETX в конце сообщения
        bodyStr = bodyStr.Remove(0, 2);
        bodyStr = bodyStr.Remove(bodyStr.Length - 3, 3);
        string[] result_str = bodyStr.Split('\f');
        // Удаляем /t  в начале каждой строки
        for (int i = 0; i < result_str.Length; i++)
            result_str[i] = result_str[i].Remove(0, 1);
        return result_str;
    }
    public bool isFullMessage(byte[] sourceArray, int length)
    {
        bool beg_msg = false, end_msg = false, crc_msg = false;
        for (int i = 0; i < length; i++)
        {
            if (sourceArray[i] == 0x10 && sourceArray[i + 1] == 0x01) beg_msg = true;
            if (sourceArray[i] == 0x10 && sourceArray[i + 1] == 0x03) end_msg = true;
            if (sourceArray[i] == 0x10 && sourceArray[i + 1] == 0x03 
                && sourceArray[i + 2] > 0 && sourceArray[i + 3] > 0) crc_msg = true;
        }
        return (beg_msg && end_msg && crc_msg);
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
    public void SetSendFlag()
    {
        msgSendFlag = true;
    }
    public void DropSendFlag()
    {
        msgSendFlag = false;
    }
    public bool isSendFlag()
    {
        return msgSendFlag;
    }
}
class BaseOfDates
{
    string[,] Base = new string[1,1];
    
    public void AddTitle()
    {

    }
}