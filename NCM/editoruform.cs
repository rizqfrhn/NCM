using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace NCM
{
    public partial class editoruform : Form
    {
        string IPORU, noLHD, Mac, Channel, ESSID, Bridging, Delay, Leave, Scan, Signal;
        public editoruform(string noLoader, string iporu, string mac, string channel, string essid,
            string bridging, int delay, int leave, int scan, int signal)
        {
            InitializeComponent();

            // Set the values to display on the form (e.g., in labels or textboxes)
            IPORU = iporu;
            tbnoloader.Text = noLHD = noLoader;
            tbmac.Text = Mac = mac;
            tbchannel.Text = Channel = channel;
            tbessid.Text = ESSID = essid;
            tbbridging.Text = Bridging = bridging;
            tbdelay.Text = Delay = delay.ToString();
            tbleave.Text = Leave = leave.ToString();
            tbscan.Text = Scan = scan.ToString();
            tbsignal.Text = Signal = signal.ToString();
        }

        private void btnsave_Click(object sender, EventArgs e)
        {
            // Check if all TextBoxes are filled
            if (IsAnyTextBoxEmpty())
            {
                // Notify the user that all TextBoxes need to be filled
                MessageBox.Show("Please fill in all the fields before saving.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                // Ensure async scan task starts after initialization
                Task.Run(() => AsyncSetDataORU());
            }
        }

        private async Task AsyncSetDataORU()
        {
            await SetDataORU([ConfigurationManager.AppSettings["setdataoru"], IPORU, noLHD, Mac, tbchannel.Text, tbessid.Text
                , tbbridging.Text, tbdelay.Text, tbleave.Text, tbscan.Text, tbsignal.Text]);
        }

        static async Task SetDataORU(string[] args)
        {

            ProcessStartInfo start = new ProcessStartInfo()
            {
                FileName = ConfigurationManager.AppSettings["python"],
                Arguments = string.Format("{0} {1} {2} {3}", args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10]),
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true, // Redirect errors
                CreateNoWindow = true // Don't create a new window
            };

            try
            {
                using (Process process = new Process())
                {
                    process.StartInfo = start;
                    process.Start();

                    // Asynchronously read output and error streams to avoid blocking
                    Task outputTask = Task.Run(() =>
                    {
                        using (StreamReader reader = process.StandardOutput)
                        {
                            string line;
                            while ((line = reader.ReadLine()) != null)
                            {
                                Console.WriteLine("Python Output: " + line);
                            }
                        }
                    });

                    Task errorTask = Task.Run(() =>
                    {
                        using (StreamReader reader = process.StandardError)
                        {
                            string line;
                            while ((line = reader.ReadLine()) != null)
                            {
                                Console.WriteLine("Python Error: " + line);
                            }
                        }
                    });

                    // Wait for the process to complete asynchronously
                    await Task.WhenAll(outputTask, errorTask);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error starting process: " + ex.Message);
            }
        }

        private void btnclose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Method to check if any TextBox is empty
        private bool IsAnyTextBoxEmpty()
        {
            // Check each TextBox to see if it's empty or contains only whitespace
            return string.IsNullOrWhiteSpace(tbchannel.Text) ||
                   string.IsNullOrWhiteSpace(tbessid.Text) ||
                   string.IsNullOrWhiteSpace(tbbridging.Text) ||
                   string.IsNullOrWhiteSpace(tbdelay.Text) ||
                   string.IsNullOrWhiteSpace(tbleave.Text) ||
                   string.IsNullOrWhiteSpace(tbscan.Text) ||
                   string.IsNullOrWhiteSpace(tbsignal.Text);
        }
    }
}
