using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;

namespace Actividad_09
{
    public partial class Form1 : Form
    {

        delegate void SetTextDelegate(string value);

        public SerialPort ArduinoPort
        {
            get;
        }

        public Form1()
        {
            InitializeComponent();
            ArduinoPort = new System.IO.Ports.SerialPort();
            ArduinoPort.PortName = "COM6";
            ArduinoPort.BaudRate = 9600;
            ArduinoPort.DataBits = 8;
            ArduinoPort.ReadTimeout = 1000;
            ArduinoPort.WriteTimeout = 1000;
            ArduinoPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);

                            
            this.btnConectar.Click += btnConectar_Click;
        }

        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            string dato = ArduinoPort.ReadLine();
            Escribirtxt(dato);
        }

        private void Escribirtxt(string dato)
        {
            if (InvokeRequired)
                try
                {
                    Invoke(new SetTextDelegate(Escribirtxt), dato);
                }
                catch
                {


                }
            else
                lbTempeteratura.Text = dato;
        }


        private void Form1_Load(object sender, EventArgs e)
        {
                
        }

        private void btnConectar_Click(object sender, EventArgs e)
        {
            btnConectar.Enabled = false;
            try
            {
                if (!ArduinoPort.IsOpen)
                    ArduinoPort.Open();
                if (int.TryParse(tbLimite.Text, out int temperaturaLimit))
                {
                    string limitString = temperaturaLimit.ToString();
                    ArduinoPort.WriteLine(limitString);
                }
                else
                {
                    MessageBox.Show("Ingresa número valor valido");
                }
                lbEstado.Text = "Conexión OK";
                lbEstado.ForeColor = System.Drawing.Color.Green;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDesconectar_Click(object sender, EventArgs e)
        {
            btnConectar.Enabled = true; 
            btnDesconectar.Enabled = false;
            if (ArduinoPort.IsOpen)
                ArduinoPort.Close();
            lbEstado.Text = "Desconectado";
            lbEstado.ForeColor = System.Drawing.Color.Red;
            lbTempeteratura.Text = "00.0";
        }

        private void tbLimite_TextChanged(object sender, EventArgs e)
        {

        }

        private void lbTempeteratura_Click(object sender, EventArgs e)
        {

        }
    }
}
