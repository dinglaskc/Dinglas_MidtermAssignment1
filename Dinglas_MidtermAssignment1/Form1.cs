using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dinglas_MidtermAssignment1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void label4_Click(object sender, EventArgs e) { }

        private void btnCompute_Click(object sender, EventArgs e)
        {
            try
            {
                // ── PRELIM ──────────────────────────────────────────
                double preCP = GetAverage("txtPreAsgn", "txtPreSw", "txtPreRec");
                double preLab = GetAverage("txtPreLab");
                double preQuiz = GetAverage("txtPreQiz");
                double preLE = GetAverage("txtPreLE");
                double preWE = GetGrade(txtPreWES, txtPreWET);

                SetText("txtPreCPAvg", preCP);
                SetText("txtPreLabAvg", preLab);
                SetText("txtPreQizAvg", preQuiz);
                SetText("txtPreLEAvg", preLE);
                SetText("txtPreWEAvg", preWE);

                double prelimTotal = (preCP * 0.10) + (preLab * 0.10) + (preQuiz * 0.20)
                                   + (preLE * 0.20) + (preWE * 0.40);
                txtSummaryPrelim.Text = prelimTotal.ToString("0.00");

                // ── MIDTERM ─────────────────────────────────────────
                double midCP = GetAverage("txtMidAsgn", "txtMidSw", "txtMidRec");
                double midLab = GetAverage("txtMidLab");
                double midQuiz = GetAverage("txtMidQiz");
                double midLE = GetAverage("txtMidLE");
                double midWE = GetGrade(txtMidWES, txtMidWET);

                SetText("txtMidCPAvg", midCP);
                SetText("txtMidLabAvg", midLab);
                SetText("txtMidQizAvg", midQuiz);
                SetText("txtMidLEAvg", midLE);
                SetText("txtMidWEAvg", midWE);

                double midtermTotal = (midCP * 0.10) + (midLab * 0.10) + (midQuiz * 0.20)
                                    + (midLE * 0.20) + (midWE * 0.40);
                txtSummaryMidterm.Text = midtermTotal.ToString("0.00");

                // ── FINALS ──────────────────────────────────────────
                double finCP = GetAverage("txtFinAsgn", "txtFinSw", "txtFinRec");
                double finLab = GetAverage("txtFinLab");
                double finQuiz = GetAverage("txtFinQiz");
                double finProj = GetAverage("txtFinPres", "txtFinManu");
                double finWE = GetGrade(txtFinWES, txtFinWET);

                SetText("txtFinCPAvg", finCP);
                SetText("txtFinLabAvg", finLab);
                SetText("txtFinQizAvg", finQuiz);
                SetText("txtFinProjAvg", finProj);
                SetText("txtFinWEAvg", finWE);

                double finalsTotal = (finCP * 0.05) + (finLab * 0.10) + (finQuiz * 0.20)
                                   + (finProj * 0.25) + (finWE * 0.40);
                txtSummaryFinals.Text = finalsTotal.ToString("0.00");

                // ── FINAL GRADE ─────────────────────────────────────
                double finalGrade = (prelimTotal * 0.33) + (midtermTotal * 0.33)
                                  + (finalsTotal * 0.33);
                txtSummaryFG.Text = finalGrade.ToString("0.00");
            }
            catch (FormatException)
            {
                MessageBox.Show("Please enter valid numbers in all filled fields.", "Input Error");
            }
            catch (DivideByZeroException)
            {
                MessageBox.Show("Total score cannot be zero.", "Math Error");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Validation Error");
            }
        }

        // Finds a TextBox by name and writes the computed value into it
        private void SetText(string name, double value)
        {
            var box = this.Controls.Find(name, true).FirstOrDefault() as TextBox;
            if (box != null)
                box.Text = value.ToString("0.00");
        }

        // Converts a score/total pair into a grade using: (score / total) * 60 + 40
        private double GetGrade(TextBox scoreBox, TextBox totalBox)
        {
            if (string.IsNullOrWhiteSpace(scoreBox.Text) ||
                string.IsNullOrWhiteSpace(totalBox.Text))
                throw new FormatException();

            double score = double.Parse(scoreBox.Text);
            double total = double.Parse(totalBox.Text);

            if (score < 0 || total < 0)
                throw new Exception("Values must not be negative.");
            if (score > total)
                throw new Exception("Score cannot be greater than Total.");
            if (total == 0)
                throw new DivideByZeroException();

            return (score / total) * 60 + 40;
        }

        // Scans all score/total pairs under the given prefixes and returns their average grade
        private double GetAverage(params string[] prefixes)
        {
            double sum = 0;
            int itemsFound = 0;

            foreach (string prefix in prefixes)
            {
                for (int i = 1; i <= 10; i++)
                {
                    var sBox = this.Controls.Find(prefix + i + "S", true).FirstOrDefault() as TextBox;
                    var tBox = this.Controls.Find(prefix + i + "T", true).FirstOrDefault() as TextBox;

                    // Skip if controls don't exist or score is blank
                    if (sBox == null || tBox == null) continue;
                    if (string.IsNullOrWhiteSpace(sBox.Text)) continue;

                    // If score is filled but total is blank or zero, that's a user error
                    if (string.IsNullOrWhiteSpace(tBox.Text))
                        throw new Exception($"Missing total for {prefix}{i}. Please fill in the total.");

                    sum += GetGrade(sBox, tBox);
                    itemsFound++;
                }
            }

            return itemsFound > 0 ? sum / itemsFound : 0;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            Action<Control.ControlCollection> clearAll = null;
            clearAll = (controls) =>
            {
                foreach (Control c in controls)
                {
                    if (c is TextBox tb) tb.Clear();
                    else clearAll(c.Controls);
                }
            };
            clearAll(this.Controls);
        }
    }
}