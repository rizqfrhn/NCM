using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace NCM
{
    public partial class editoruform : Form
    {
        string IPORU, noLHD, Mac, Channel, ChannelRoam, ESSID, Bridging, Delay, Leave, Scan, Signal;
        public editoruform(string iporu, string noLoader, string mac, string channel, string channelroam, string essid,
            string bridging, int delay, int leave, int scan, int signal)
        {
            InitializeComponent();

            // Set the values to display on the form (e.g., in labels or textboxes)
            IPORU = iporu;
            tbnoloader.Text = noLHD = noLoader;
            tbmac.Text = Mac = mac;
            Channel = channel;
            ChannelRoam = channelroam;
            tbessid.Text = ESSID = essid;
            Bridging = bridging;
            tbdelay.Text = Delay = delay.ToString();
            tbleave.Text = Leave = leave.ToString();
            tbscan.Text = Scan = scan.ToString();
            tbsignal.Text = Signal = signal.ToString();
            
            cbbridging.Items.Clear();
            cbbridging.DataSource = null;
            cbchannel.Items.Clear();
            cbchannel.DataSource = null;

            LoadCB();
        }

        public void LoadCB()
        {
            using (SQLiteConnection conn = new SQLiteConnection(ConfigurationManager.ConnectionStrings["Key"].ConnectionString))
            {
                conn.Open();
                try
                {
                    DataTable dtroam = new DataTable();
                    SQLiteDataAdapter daroam = new SQLiteDataAdapter("select id_channelroam, desc from tb_channelroam", conn);
                    dtroam = new DataTable();
                    dtroam.Clear();
                    daroam.Fill(dtroam);

                    //DataRow selectroam = dtroam.NewRow();
                    //selectroam[1] = "Select Channel";
                    //dtroam.Rows.InsertAt(selectroam, 0);

                    cbchannel.DisplayMember = "desc";
                    cbchannel.ValueMember = "desc";
                    cbchannel.DataSource = dtroam;

                    cbchannel.Refresh();
                    cbchannel.SelectedValue = ChannelRoam;

                    DataTable dtbridging = new DataTable();
                    SQLiteDataAdapter dabridging = new SQLiteDataAdapter("select id_bridging, desc from tb_bridging", conn);
                    dtbridging = new DataTable();
                    dtbridging.Clear();
                    dabridging.Fill(dtbridging);

                    //DataRow select = dtbridging.NewRow();
                    //select[1] = "Select Bridging Mode";
                    //dtbridging.Rows.InsertAt(select, 0);

                    cbbridging.DisplayMember = "desc";
                    cbbridging.ValueMember = "desc";
                    cbbridging.DataSource = dtbridging;

                    cbbridging.Refresh();
                    cbbridging.SelectedValue = Bridging;
                }
                catch (Exception ex)
                {
                    // write exception info to log or anything else
                    MessageBox.Show("Error loading data: " + ex.Message);
                }
                conn.Close();
            }
        }

        private void btnsave_Click(object sender, EventArgs e)
        {
            // Check if all TextBoxes are filled
            if (tbessid.Text == "" || tbdelay.Text == "" || tbleave.Text == "" || tbscan.Text == "" || tbsignal.Text == "")
            {
                // Notify the user that all TextBoxes need to be filled
                MessageBox.Show("Please fill in all the fields before saving.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                if (ChannelRoam == cbchannel.Text && ESSID == tbessid.Text &&
                    Bridging == cbbridging.Text && tbdelay.Text == Delay && tbleave.Text == Leave &&
                    tbscan.Text == Scan && tbsignal.Text == Signal) 
                {
                    MessageBox.Show("No Data Updated!", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    Channel = cbchannel.Text == "1 (2.412 GHz)" ? "1" : "11";
                    ChannelRoam = cbchannel.Text;
                    Bridging = cbbridging.Text;
                    // Ensure async scan task starts after initialization
                    Task.Run(() => AsyncSetDataORU());
                }
            }
        }

        private async Task AsyncSetDataORU()
        {
            await SetDataORU([ConfigurationManager.AppSettings["setdataoru"], IPORU, noLHD, Mac, Channel, ChannelRoam, tbessid.Text
                , Bridging, tbdelay.Text, tbleave.Text, tbscan.Text, tbsignal.Text]);
        }

        static async Task SetDataORU(string[] args)
        {

            ProcessStartInfo start = new ProcessStartInfo()
            {
                FileName = ConfigurationManager.AppSettings["python"],
                Arguments = string.Format("{0} {1} {2} {3}", args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11]),
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
    }
}
