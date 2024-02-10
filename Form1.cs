using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Linq;
using System.Text;

namespace Assignment3Again
{
    public partial class Form1 : Form
    {
        private List<Athlete> athletes;
        private Athlete currentAthlete;
        private DateTime startTime;
        private TimeSpan endTime;


        public Form1()
        {
            InitializeComponent();
            ResetTime();//gives me the initial stopwatch 00:00:00.000
            button4.Enabled = false;
            athletes = new List<Athlete>();
            currentAthlete = null;
            timer1.Interval = 10; //10 milli resolution
            SetDoubleBuffered(tableLayoutPanel1);
            SetDoubleBuffered(tableLayoutPanel2);
            SetDoubleBuffered(tableLayoutPanel3);
            SetDoubleBuffered(tableLayoutPanel4);
            SetDoubleBuffered(tableLayoutPanel5);

        }

        public static void SetDoubleBuffered(System.Windows.Forms.Control c)
        {
            if (System.Windows.Forms.SystemInformation.TerminalServerSession)
                return;
            System.Reflection.PropertyInfo aProp = typeof(System.Windows.Forms.Control).GetProperty("DoubleBuffered", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            aProp.SetValue(c, true, null);
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;
                return cp;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Add to list button
            string firstName = firstNBox.Text;
            string lastName = lastNBox.Text;
            if (!string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName))
            {
                Athlete newRunner = new Athlete(firstName, lastName);
                athletes.Add(newRunner);
                listBox1.Items.Add(newRunner);
                firstNBox.Clear();
                lastNBox.Clear();
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Stop button
            timer1.Stop();
            button4.Enabled = false;
            if (currentAthlete != null)
            {
                endTime = DateTime.Now - startTime;
                label5.Text = endTime.ToString(@"hh\:mm\:ss\.fff");
                
                currentAthlete.AddLapTime(endTime.Hours, endTime.Minutes, endTime.Seconds, endTime.Milliseconds);
                UpdateCurrentAthleteTextBox();
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Start button
            button4.Enabled = true;
            startTime = DateTime.Now;
            timer1.Start();
        }

        private void button4_Click(object sender, EventArgs e)
        {   //lap
            if (currentAthlete != null)
            {
                TimeSpan lapTime = DateTime.Now - startTime;
                currentAthlete.AddLapTime(lapTime.Hours, lapTime.Minutes, lapTime.Seconds, lapTime.Milliseconds);
                UpdateCurrentAthleteTextBox();


            }
        }

        private void button5_Click(object sender, EventArgs e)
        { // Reset button
            ResetTime();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {//index athlete selection
            if (listBox1.SelectedIndex >= 0)
            {
                ResetTime();
                currentAthlete = (Athlete)listBox1.SelectedItem;
                UpdateCurrentAthleteTextBox();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (startTime != null)
            {
                TimeSpan elapsedTime = DateTime.Now - startTime;
                string formattedTime = $"{elapsedTime.Hours:D2}:{elapsedTime.Minutes:D2}:{elapsedTime.Seconds:D2}.{elapsedTime.Milliseconds:D3}";
                label5.Text = formattedTime;
            }
        }


        private void UpdateCurrentAthleteTextBox()
        {
            if (currentAthlete != null)
            {
                textBox3.Text = $"{currentAthlete.FirstName} {currentAthlete.LastName}\r\n";

                for (int i = 0; i < currentAthlete.Laps.Count; i++)
                {
                    var lapTime = currentAthlete.Laps[i];
                    string formattedLapTime = $"{lapTime.Hours:D2}:{lapTime.Minutes:D2}:{lapTime.Seconds:D2}.{lapTime.Milliseconds:D3}";
                    Console.WriteLine($"Debug - Lap {i + 1}: {formattedLapTime}");
                    textBox3.AppendText($"Lap {i + 1}: {formattedLapTime}\r\n");
                }
            }
        }

        private void ResetTime()
        {
            timer1.Stop();
            label5.Text = "00:00:00.000";
            startTime = DateTime.MinValue;
        }
        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //call clear data method. 
            ClearAllData();
            MessageBox.Show("Information has been reset!");
        }
        private void ClearAllData()
        {
            // Clear the list of athletes
            athletes.Clear();
            // Clear the list box
            listBox1.Items.Clear();
            // Clear the current athlete
            currentAthlete = null;
            // Clear the text boxes
            firstNBox.Clear();
            lastNBox.Clear();
            textBox3.Clear();
            // Reset the stopwatch label
            ResetTime();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {//save function menu tool
         // save function menu tool
            if (listBox1.Items.Count == 0)
            {
                MessageBox.Show("Nothing for you to save yet, atleast add a runner.", "No Runners", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Manipulate the filename based on the current date and time
            string manipulatedFileName = ManipulateFileName();

            // Show the save file dialog with the manipulated filename
            saveFileDialog1.FileName = manipulatedFileName; // Set the initial filename in the save file dialog
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // Write data to the manipulated filename
                    using (StreamWriter writer = new StreamWriter(saveFileDialog1.FileName))
                    {
                        foreach (Athlete runner in athletes)
                        {
                            writer.WriteLine($"{runner.FirstName} {runner.LastName}:");
                            foreach (Time2sw lapTime in runner.Laps)
                            {
                                string formattedLapTime = $"{lapTime.Hours:D2}h {lapTime.Minutes:D2}m {lapTime.Seconds:D2}s {lapTime.Milliseconds:D3}ms";
                                writer.WriteLine($"   Lap: {formattedLapTime}");
                            }
                        }
                    }
                    MessageBox.Show("File saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error saving file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }
        private string ManipulateFileName()
        {
            // Generate a filename based on the current date and time
            string currentDate = DateTime.Now.ToString("yyyyMMdd_HH_mm_ss");
            string fileName = $"RunRecord_{currentDate}.txt";

            return fileName;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            // new window button
            if (listBox1.Items.Count == 0)
            {
                MessageBox.Show("Add someone to the report first.", "Empty List", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            StringBuilder reportText = new StringBuilder();
            foreach (Athlete runner in athletes)
            {
                reportText.AppendLine($"{runner.FirstName} {runner.LastName}:");
                foreach (Time2sw lapTime in runner.Laps)
                {
                    string formattedLapTime = $"{lapTime.Hours:D2}h {lapTime.Minutes:D2}m {lapTime.Seconds:D2}s {lapTime.Milliseconds:D3}ms";
                    reportText.AppendLine($"   Lap: {formattedLapTime}");
                }
                reportText.AppendLine(); // Add a blank line between each athlete
            }

            Form reportForm = new Form();
            reportForm.Text = "Runner Times Report";
            TextBox reportTextBox = new TextBox
            {
                Multiline = true,
                Dock = DockStyle.Fill,
                Text = reportText.ToString()
            };
            reportForm.Controls.Add(reportTextBox);
            reportForm.ShowDialog();
        }


        private void button7_Click(object sender, EventArgs e)
        {
            // best time button
            if (athletes.Count == 0)
            {
                MessageBox.Show("Add someone to the report first.", "Empty List", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var sortedAthletes = athletes
                .OrderBy(a => a.Laps.Sum(lap => lap.Hours * 3600000 + lap.Minutes * 60000 + lap.Seconds * 1000 + lap.Milliseconds));

            foreach (var athlete in sortedAthletes)
            {
                Console.WriteLine($"{athlete.FirstName} {athlete.LastName}: Total Time = {GetTotalMilliseconds(athlete.Laps)} milliseconds");
            }

            Athlete bestTotalTimeAthlete = sortedAthletes.FirstOrDefault();

            if (bestTotalTimeAthlete != null)
            {
                int selectedIndex = listBox1.Items.IndexOf(bestTotalTimeAthlete);
                if (selectedIndex != -1)
                {
                    listBox1.SelectedIndex = selectedIndex;
                }
                else
                {
                    MessageBox.Show("Selected athlete not found in the list.");
                }
            }
            else
            {
                MessageBox.Show("No athletes with lap times found.");
            }
        }
        private double GetTotalMilliseconds(List<Time2sw> laps)
        {
            double totalMilliseconds = laps.Sum(lap =>
            {
                Console.WriteLine($"Lap: {lap.Hours}h {lap.Minutes}m {lap.Seconds}s {lap.Milliseconds}ms");
                return lap.Hours * 3600000 + lap.Minutes * 60000 + lap.Seconds * 1000 + lap.Milliseconds;
            });

            return totalMilliseconds;
        }
    }
}

