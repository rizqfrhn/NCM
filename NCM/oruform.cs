using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Diagnostics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Data.Common;
using System.Net.NetworkInformation;
using System.Net;
using System.Text.RegularExpressions;
using System.Buffers.Text;
using static System.ComponentModel.Design.ObjectSelectorEditor;
using System.Security.Cryptography;
using System.Threading.Channels;

namespace NCM
{
    public partial class oruform : Form
    {
        private System.Windows.Forms.Timer scanscheduler;
        string getdataoru = ConfigurationManager.AppSettings["getdataoru"];
        // string getdataoru = "C:\\Users\\TRAKINDO\\source\\repos\\NCM\\py\\getdataoru.py";

        public oruform()
        {
            // Initialize the timer and other components before starting async task
            InitTimer();
            InitializeComponent();

            // Ensure async scan task starts after initialization
            Task.Run(() => StartScanAsyncORU());
        }

        private void oruform_Shown(object sender, EventArgs e)
        {
            LoadData();

            // Add CellDoubleClick event handler
            dgv_oru.CellDoubleClick += dgv_oru_CellDoubleClick;
        }

        public void InitTimer()
        {
            scanscheduler = new System.Windows.Forms.Timer();
            scanscheduler.Tick += new EventHandler(scanscheduler_Tick);
            scanscheduler.Interval = 600000; // in miliseconds
            scanscheduler.Start();
        }

        private void scanscheduler_Tick(object sender, EventArgs e)
        {
            // Start the scan in a background task
            Task.Run(() => StartScanAsyncORU());

            // Notify user that scan has started
            Console.WriteLine($"Scan started in the background.");
        }

        static async Task GetDataORU(string[] args)
        {

            ProcessStartInfo start = new ProcessStartInfo()
            {
                FileName = ConfigurationManager.AppSettings["python"],
                // Console.Write(args.Length);
                // arg[0] = Path to your python script (example : "C:\\add_them.py")
                // arg[1] = first arguement taken from  C#'s main method's args variable (here i'm passing a number : 5)
                // arg[2] = second arguement taken from  C#'s main method's args variable ( here i'm passing a number : 6)
                // pass these to your Arguements property of your ProcessStartInfo instance

                Arguments = string.Format("{0} {1} {2} {3}", args[0], args[1], args[2], args[3]),
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

        private async Task StartScanAsyncORU()
        {
            // Scan the IP range from 10.10.10.0 to 10.10.10.225
            string baseIP = "10.10.10.";

            for (int i = 10; i < 256; i++)
            {
                string loaderip = baseIP + i;

                // Check if the IP is reachable
                bool isReachable = IsORUReachable(loaderip);
                string status = isReachable ? "Online" : "Offline";

                // Get the current status from the database to compare
                string currentStatus = GetCurrentStatusFromDatabase(loaderip);

                // If the status changed from Online to Offline, update the database
                if (currentStatus == "Online" && !isReachable)
                {
                    // Update status to "Offline" in the database
                    UpdateStatusToOffline(loaderip);
                    Console.WriteLine($"Loader {loaderip} went Offline.");
                }

                // Check if the IP is reachable
                if (IsORUReachable(loaderip))
                {
                    // Get the MAC address for the reachable IP
                    string macORU = GetMacAddress(loaderip);
                    Console.WriteLine($"{macORU}");
                    if (!string.IsNullOrEmpty(macORU))
                    {
                        string[] parts = loaderip.Split('.');

                        // Get the last octet
                        string lastOctet = parts[3];

                        // Check if the last octet has two digits (between 10 and 99)
                        if (lastOctet.Length == 2)
                        {
                            // Add "6" in front of the last octet
                            lastOctet = "6" + lastOctet;
                        }
                        else if (lastOctet.Length == 3 && Convert.ToInt32(lastOctet) > 150)
                        {
                            // Get the last part and replace the first digit with '8'
                            lastOctet = "8" + lastOctet.Substring(1);
                        }
                        else
                        {
                            lastOctet = "8" + lastOctet;
                        }

                        // Update the last octet in the parts array
                        string nolhd = lastOctet;

                        InsertIpORU(loaderip, nolhd, macORU);
                        await GetDataORU([getdataoru, loaderip, nolhd, macORU]);
                        Console.WriteLine($"Inserted IP: {loaderip}, MAC Address: {macORU}");
                        Invoke((Action)(() =>
                        {
                            LoadData();
                        }));
                    }
                    else
                    {
                        Console.WriteLine($"MAC address for {loaderip} could not be retrieved.");
                    }
                }
                else
                {
                    Console.WriteLine($"IP {loaderip} is not reachable.");
                }
            }

            // Notify user that scan is completed
            Invoke((Action)(() =>
            {
                LoadData();
                Console.WriteLine($"Scan completed.");
            }));
        }
        private static string GetCurrentStatusFromDatabase(string ip)
        {
            using (SQLiteConnection conn = new SQLiteConnection(ConfigurationManager.ConnectionStrings["Key"].ConnectionString))
            {
                conn.Open();
                string query = "SELECT status FROM tb_oru WHERE ip_oru = @oru";
                SQLiteCommand command = new SQLiteCommand(query, conn);
                command.Parameters.AddWithValue("@oru", ip);

                // Execute the query and get the status
                var result = command.ExecuteScalar();
                return result?.ToString() ?? "Offline";  // Default to "Offline" if no result
            }
        }

        private static void UpdateStatusToOffline(string ip)
        {
            using (SQLiteConnection conn = new SQLiteConnection(ConfigurationManager.ConnectionStrings["Key"].ConnectionString))
            {
                conn.Open();
                string updateQuery = "UPDATE tb_oru SET status = 'Offline' WHERE ip_oru = @oru";
                SQLiteCommand command = new SQLiteCommand(updateQuery, conn);
                command.Parameters.AddWithValue("@oru", ip);

                command.ExecuteNonQuery();
            }
        }

        // Get the MAC address of the IP address using ARP
        private static string GetMacAddress(string ipAddress)
        {
            try
            {
                // Run the ARP command to retrieve the MAC address for the given IP
                Process process = new Process();
                process.StartInfo.FileName = "arp";
                process.StartInfo.Arguments = "-a " + ipAddress;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.UseShellExecute = false;
                process.Start();

                string output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
                Console.WriteLine($"{output}");
                // Use regex to extract the MAC address from the ARP output
                Regex macRegex = new Regex(@"(?:[0-9a-fA-F]{2}-){5}[0-9a-fA-F]{2}");
                Match match = macRegex.Match(output);
                return match.Value;
            }
            catch (Exception)
            {
                return string.Empty; // If there's an error, return an empty string
            }
        }

        public void LoadData()
        {
            using (SQLiteConnection conn = new SQLiteConnection(ConfigurationManager.ConnectionStrings["Key"].ConnectionString))
            {
                try
                {
                    conn.Open();  // Ensure connection is open
                    SQLiteDataAdapter cmd = new SQLiteDataAdapter("SELECT no_loader, ip_oru, mac, channel, tb_channelroam.Id_channelroam as idchannelroam, tb_channelroam.desc as channelroam, essid, tb_bridging.Id_bridging as idbridging, tb_bridging.desc as bridging, delay, leave_threshold, scan_threshold, min_signal, status FROM tb_oru left JOIN tb_channelroam ON tb_oru.channelroam = tb_channelroam.id_channelroam left JOIN tb_bridging ON tb_oru.bridging = tb_bridging.id_bridging ORDER BY no_loader", conn);
                    DataTable dt = new DataTable();
                    dt.Clear();
                    cmd.Fill(dt);

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        dgv_oru.DataSource = dt;

                        // Set the header text for each column by index or name
                        dgv_oru.Columns["no_loader"].HeaderText = "Loader Number";
                        dgv_oru.Columns["ip_oru"].HeaderText = "IP Address";
                        dgv_oru.Columns["mac"].HeaderText = "MAC Address";
                        dgv_oru.Columns["channel"].HeaderText = "Channel";
                        dgv_oru.Columns["essid"].HeaderText = "ESSID";
                        dgv_oru.Columns["bridging"].HeaderText = "Bridging";
                        dgv_oru.Columns["delay"].HeaderText = "Delay";
                        dgv_oru.Columns["leave_threshold"].HeaderText = "Leave Threshold";
                        dgv_oru.Columns["scan_threshold"].HeaderText = "Scan Threshold";
                        dgv_oru.Columns["min_signal"].HeaderText = "Min Signal";
                        dgv_oru.Columns["status"].HeaderText = "Status ORU";

                        dgv_oru.Columns["channelroam"].Visible = false;
                        dgv_oru.Columns["idchannelroam"].Visible = false;
                        dgv_oru.Columns["idbridging"].Visible = false;
                        dgv_oru.Refresh();  // Refresh the DataGridView

                        // Make the DataGridView non-editable
                        dgv_oru.ReadOnly = true;
                    }
                    else
                    {
                        MessageBox.Show("No Data Found, Scanning ORU On Progress!");
                    }
                    conn.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Database error: " + ex.Message);
                }
            }
        }

        // Check if IP exists in the database
        private static bool IsORUInDatabase(string ip)
        {
            using (SQLiteConnection conn = new SQLiteConnection(ConfigurationManager.ConnectionStrings["Key"].ConnectionString))
            {
                conn.Open();
                string checkIpQuery = "SELECT COUNT(*) FROM tb_oru WHERE ip_oru = @oru;";
                SQLiteCommand command = new SQLiteCommand(checkIpQuery, conn);
                command.Parameters.AddWithValue("@oru", ip);
                long count = (long)command.ExecuteScalar();
                return count > 0;
            }
        }

        // Insert the IP into the database if it does not exist
        private static void InsertIpORU(string ip, string nolhd, string macORU)
        {
            using (SQLiteConnection conn = new SQLiteConnection(ConfigurationManager.ConnectionStrings["Key"].ConnectionString))
            {
                conn.Open();
                // If the IP is reachable, check if it's in the database
                if (!IsORUInDatabase(ip))
                {
                    string insertIpQuery = "INSERT INTO tb_oru (id_oru, ip_oru, no_loader, mac, status) VALUES (ifnull((select max(id_oru) from tb_oru) + 1,1), @oru, @nolhd, @macORU, 'Online');";
                    SQLiteCommand command = new SQLiteCommand(insertIpQuery, conn);
                    command.Parameters.AddWithValue("@oru", ip);
                    command.Parameters.AddWithValue("@nolhd", nolhd);
                    command.Parameters.AddWithValue("@macORU", macORU);
                    command.ExecuteNonQuery();
                }
                else
                {
                    Console.WriteLine($"Loader already exist: {nolhd}");
                }
            }
        }

        // Ping the IP address to check if it is reachable
        private static bool IsORUReachable(string ip)
        {
            try
            {
                using (Ping ping = new Ping())
                {
                    PingReply reply = ping.Send(ip, 1); // Timeout of 1000 ms (1 second)
                    return reply.Status == IPStatus.Success;
                }
            }
            catch (PingException)
            {
                return false; // If there is a PingException, we consider the IP unreachable
            }
        }

        private void dgv_oru_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // Ensure the user clicked on a valid row
            if (e.RowIndex >= 0)
            {
                if (dgv_oru.Rows[e.RowIndex].Cells["channel"].Value != DBNull.Value && dgv_oru.Rows[e.RowIndex].Cells["channelroam"].Value != DBNull.Value &&
                    dgv_oru.Rows[e.RowIndex].Cells["essid"].Value != DBNull.Value && dgv_oru.Rows[e.RowIndex].Cells["bridging"].Value != DBNull.Value &&
                    dgv_oru.Rows[e.RowIndex].Cells["delay"].Value != DBNull.Value && dgv_oru.Rows[e.RowIndex].Cells["leave_threshold"].Value != DBNull.Value)
                {
                    // Extract data from the selected row
                    string noLoader = dgv_oru.Rows[e.RowIndex].Cells["no_loader"].Value.ToString();
                    string iporu = dgv_oru.Rows[e.RowIndex].Cells["ip_oru"].Value.ToString();
                    string mac = dgv_oru.Rows[e.RowIndex].Cells["mac"].Value.ToString();
                    string channel = dgv_oru.Rows[e.RowIndex].Cells["channel"].Value.ToString();
                    string channelroam = dgv_oru.Rows[e.RowIndex].Cells["idchannelroam"].Value.ToString();
                    string essid = dgv_oru.Rows[e.RowIndex].Cells["essid"].Value.ToString();
                    string bridging = dgv_oru.Rows[e.RowIndex].Cells["idbridging"].Value.ToString();
                    int delay = Convert.ToInt32(dgv_oru.Rows[e.RowIndex].Cells["delay"].Value) == null ? 0 : Convert.ToInt32(dgv_oru.Rows[e.RowIndex].Cells["delay"].Value);
                    int leave = Convert.ToInt32(dgv_oru.Rows[e.RowIndex].Cells["leave_threshold"].Value) == null ? 0 : Convert.ToInt32(dgv_oru.Rows[e.RowIndex].Cells["leave_threshold"].Value);
                    int scan = Convert.ToInt32(dgv_oru.Rows[e.RowIndex].Cells["scan_threshold"].Value) == null ? 0 : Convert.ToInt32(dgv_oru.Rows[e.RowIndex].Cells["scan_threshold"].Value);
                    int signal = Convert.ToInt32(dgv_oru.Rows[e.RowIndex].Cells["min_signal"].Value) == null ? 0 : Convert.ToInt32(dgv_oru.Rows[e.RowIndex].Cells["min_signal"].Value);
                    string statusoru = dgv_oru.Rows[e.RowIndex].Cells["status"].Value.ToString();

                    if(statusoru == "Online") {
                        // Open the new form and pass the data
                        EditOru(iporu, noLoader, mac, channel, channelroam, essid, bridging, delay, leave, scan, signal);
                    } else {
                        MessageBox.Show("ORU Offline, Wait Until It is Re-Scanned", "Caution", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("ORU Data is Incomplete, Wait Until It is Re-Scanned And The ORU Data is Complete!", "Caution", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void EditOru(string iporu, string noLoader, string mac, string channel, string channelroam, string essid,
            string bridging, int delay, int leave, int scan, int signal)
        {
            // Create and show the new form (not as a dialog)
            editoruform EditForm = new editoruform(iporu, noLoader, mac, channel, channelroam, essid, bridging, delay, leave, scan, signal);
            EditForm.Show(); // Open the form independently
        }
    }
}
