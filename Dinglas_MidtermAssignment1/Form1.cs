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

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void btnCompute_Click(object sender, EventArgs e)
        {
            try
            {
                // prelim
                double preCP = GetAverage(6, "txtPreAsgn", "txtPreSw", "txtPreRec");
                double preLab = GetAverage(4, "txtPreLab");
                double preQuiz = GetAverage(3, "txtPreQiz");
                double preLE = GetAverage(2, "txtPreLE");
                double preWE = GetGrade(txtPreCPAvg, txtPreWET);

                double prelimTotal = (preCP * 0.1) + (preLab * 0.1) + (preQuiz * 0.2) + (preLE * 0.2) + (preWE * 0.4);
                txtSummaryPrelim.Text = prelimTotal.ToString("0.00");

                // midterm
                double midCP = GetAverage(6, "txtMidAsgn", "txtMidSw", "txtMidRec");
                double midLab = GetAverage(4, "txtMidLab");
                double midQuiz = GetAverage(3, "txtMidQiz");
                double midLE = GetAverage(2, "txtMidLE");
                double midWE = GetGrade(txtMidWES, txtMidWET);

                double midtermTotal = (midCP * 0.1) + (midLab * 0.1) + (midQuiz * 0.2) + (midLE * 0.2) + (midWE * 0.4);
                txtSummaryMidterm.Text = midtermTotal.ToString("0.00");

                // finals
                double finCP = GetAverage(6, "txtFinAsgn", "txtFinSw", "txtFinRec");
                double finLab = GetAverage(4, "txtFinLab");
                double finQuiz = GetAverage(3, "txtFinQiz");
                double finProj = GetAverage(2, "txtFinPres", "txtFinManu"); 
                double finWE = GetGrade(txtFinWES, txtFinWET);

                double finalsTotal = (finCP * 0.05) + (finLab * 0.1) + (finQuiz * 0.2) + (finProj * 0.25) + (finWE * 0.4);
                txtSummaryFinals.Text = finalsTotal.ToString("0.00");

                // final grade 
                double finalGrade = (prelimTotal * 0.33) + (midtermTotal * 0.33) + (finalsTotal * 0.33);
                txtSummaryFG.Text = finalGrade.ToString("0.00");
            }
            catch (FormatException)
            {
                MessageBox.Show("Please enter valid numbers.", "Input Error");
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

        private double GetGrade(TextBox scoreBox, TextBox totalBox)
        {
            double score = double.Parse(scoreBox.Text);
            double total = double.Parse(totalBox.Text);

            // if statements for validation
            if (score > total)
                throw new Exception("Score cannot be greater than Total.");
            if (score < 0 || total < 0)
                throw new Exception("Values must not be negative.");
            if (total == 0)
                throw new DivideByZeroException();

            // foormula: (Score / Total) * 60 + 40
            return (score / total) * 60 + 40;
        }

        private double GetAverage(int maxCount, params string[] prefixes)
        {
            double sum = 0;
            int itemsFound = 0;

            foreach (string prefix in prefixes)
            {
                for (int i = 1; i <= 10; i++) // scans for txtName1S, txtName2S, etc.
                {
                    var sBox = this.Controls.Find(prefix + i + "S", true).FirstOrDefault() as TextBox;
                    var tBox = this.Controls.Find(prefix + i + "T", true).FirstOrDefault() as TextBox;

                    if (sBox != null && tBox != null && !string.IsNullOrWhiteSpace(sBox.Text))
                    {
                        sum += GetGrade(sBox, tBox);
                        itemsFound++;
                    }
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
                    if (c is TextBox) ((TextBox)c).Clear();
                    else clearAll(c.Controls);
                }
            };
            clearAll(this.Controls);
        }
    }
}
