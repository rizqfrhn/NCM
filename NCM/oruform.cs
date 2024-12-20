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

namespace NCM
{
    public partial class oruform : Form
    {
        private System.Windows.Forms.Timer scanscheduler;
        string getdataoru = "C:\\Users\\TRAKINDO\\source\\repos\\NCM\\py\\getdataoru.py";
        
        public oruform()
        {
            Task.Run(() => StartScanAsyncORU());
            LoadData();
            InitTimer();
            InitializeComponent();
        }
        public void InitTimer()
        {
            scanscheduler = new System.Windows.Forms.Timer();
            scanscheduler.Tick += new EventHandler(scanscheduler_Tick);
            scanscheduler.Interval = 300000; // in miliseconds
            scanscheduler.Start();
        }

        private void scanscheduler_Tick(object sender, EventArgs e)
        {
            // Start the scan in a background task
            Task.Run(() => StartScanAsyncORU());

            // Notify user that scan has started
            Console.WriteLine($"Scan started in the background.");
        }

        static void GetDataORU(string[] args)
        {

            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = "C:\\Users\\TRAKINDO\\AppData\\Local\\Programs\\Python\\Python311\\python.exe";
            // Console.Write(args.Length);
            // arg[0] = Path to your python script (example : "C:\\add_them.py")
            // arg[1] = first arguement taken from  C#'s main method's args variable (here i'm passing a number : 5)
            // arg[2] = second arguement taken from  C#'s main method's args variable ( here i'm passing a number : 6)
            // pass these to your Arguements property of your ProcessStartInfo instance

            start.Arguments = string.Format("{0} {1} {2} {3}", args[0], args[1], args[2], args[3]);
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;       
            // start.RedirectStandardError = true; // Redirect errors
            // start.CreateNoWindow = true; // Don't create a new window
            
            // try
            // {
            //     using (Process process = Process.Start(start))
            //     {
            //         // Read the output asynchronously
            //         process.OutputDataReceived += (sender, e) =>
            //         {
            //             if (!string.IsNullOrEmpty(e.Data))
            //             {
            //                 Console.WriteLine("Output: " + e.Data);
            //             }
            //         };
            //         process.ErrorDataReceived += (sender, e) =>
            //         {
            //             if (!string.IsNullOrEmpty(e.Data))
            //             {
            //                 Console.WriteLine("Error: " + e.Data);
            //             }
            //         };
            //         process.BeginOutputReadLine(); // Begin reading the output
            //         process.BeginErrorReadLine(); // Begin reading the error output
            //     }
            // }
            // catch (Exception ex)
            // {
            //     Console.WriteLine("Error starting process: " + ex.Message);
            // }
            using (Process process = Process.Start(start))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    string result = reader.ReadToEnd();
                    Console.Write(result);

                }
            }
            Console.Read();
        }

        private async Task StartScanAsyncORU()
        {
            // Scan the IP range from 10.10.10.0 to 10.10.10.225
            string baseIP = "10.10.10.";

            for (int i = 10; i < 256; i++)
            {
                string loaderip = baseIP + i;

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
                        GetDataORU([getdataoru, loaderip, nolhd, macORU]);
                        Console.WriteLine($"Inserted IP: {loaderip}, MAC Address: {macORU}");
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
                conn.Open();
                try
                {
                    SQLiteCommand cmd = new SQLiteCommand("select * from tb_oru order by no_loader", conn);
                    using (SQLiteDataReader read = cmd.ExecuteReader())
                    {
                        while (read.Read())
                        {
                            dgv_oru.Rows.Add(new object[] {
                                read.GetValue(read.GetOrdinal("no_loader")),  // Or column name like this
                                read.GetValue(read.GetOrdinal("mac")),
                                read.GetValue(read.GetOrdinal("channel")),
                                read.GetValue(read.GetOrdinal("essid")),
                                read.GetValue(read.GetOrdinal("bridging")),
                                read.GetValue(read.GetOrdinal("delay")),
                                read.GetValue(read.GetOrdinal("leave_threshold")),
                                read.GetValue(read.GetOrdinal("scan_threshold")),
                                read.GetValue(read.GetOrdinal("min_signal"))
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    // write exception info to log or anything else
                    MessageBox.Show("Error loading data: " + ex.Message);
                }
                conn.Close();
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
                    string insertIpQuery = "INSERT INTO tb_oru (id_oru, ip_oru, no_loader, mac) VALUES (ifnull((select max(id_oru) from tb_oru) + 1,1), @oru, @nolhd, @macORU);";
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
    }
}
