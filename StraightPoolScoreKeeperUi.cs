﻿using System;
using System.Linq;
using System.Windows.Forms;

namespace StraightPoolScoreKeeper
{
    public partial class FrmStraightPoolScoreKeeper : Form
    {
        private StatisticsModel statisticsModel = new StatisticsModel();

        public FrmStraightPoolScoreKeeper()
        {
            InitializeComponent();

            txtCurrentBest.Text = statisticsModel.GetCurrentBest().ToString();
            txtRecord.Text = statisticsModel.GetRecord().ToString();
            txtAverage.Text = statisticsModel.GetAverage().ToString();
        }

        private void TxtCurrentScoreEnter(object sender, EventArgs e)
        {
            txtCurrentScore.Focus();
            txtCurrentScore.SelectAll();
        }

        private void TxtCurrentScoreKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                SaveCurrentScore(Convert.ToInt32(txtCurrentScore.Text));
                txtCurrentScore.SelectAll();
            }
            else if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
            }
        }

        private void TxtCurrentScoreTextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCurrentScore.Text))
            {
                txtCurrentScore.Text = "0";
                txtCurrentScore.SelectAll();
            }
        }

        private void TxtCurrentBestKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                statisticsModel.SaveCurrentBest(Convert.ToInt32(txtCurrentBest.Text));
            }
            else if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
            }
        }

        private void BtnResetClick(object sender, EventArgs e)
        {
            SaveCurrentScore(Convert.ToInt32(txtCurrentScore.Text));
            CalculateStatistics(false);

            SaveCurrentScore(0);
        }

        private void MiDeleteClick(object sender, EventArgs e)
        {
            if (dgCurrentScores.SelectedRows.Count == 0)
                return;

            statisticsModel.DeleteCurrentScore(dgCurrentScores.SelectedRows[0].Index);
            CalculateStatistics(true);
        }

        private void SaveCurrentScore(int score)
        {
            statisticsModel.SaveCurrentScore(score);

            txtCurrentBest.Text = statisticsModel.GetCurrentBest().ToString();
            txtRecord.Text = statisticsModel.GetRecord().ToString();

            txtCurrentScore.Text = score.ToString();
            txtCurrentScore.Focus();
            txtCurrentScore.SelectAll();
        }

        private void CalculateStatistics(bool deleting)
        {
            statisticsModel.CalculateStatistics(deleting);

            txtCurrentBest.Text = statisticsModel.GetCurrentBest().ToString();
            txtTotalAttempts.Text = statisticsModel.GetTotalAttempts().ToString();
            txtTotalRacks.Text = string.Format("{0} of {1} ({2:P0})",
                statisticsModel.GetTotalRacks().ToString(),
                statisticsModel.GetTotalPossibleRacks(),
                statisticsModel.GetRacksCompletedPercentage());
            txtTotalBalls.Text = string.Format("{0} of {1} ({2:P0})",
                statisticsModel.GetTotalBalls().ToString(),
                statisticsModel.GetTotalPossibleBalls(),
                statisticsModel.GetBallPocketingPercentage());
            txtAverage.Text = statisticsModel.GetAverage().ToString();

            dgCurrentScores.DataSource = statisticsModel.GetCurrentScores().ToList();
            if (dgCurrentScores.Rows.Count > 0)
            {
                dgCurrentScores.FirstDisplayedScrollingRowIndex = dgCurrentScores.Rows.Count - 1;
                dgCurrentScores.Rows[dgCurrentScores.Rows.Count - 1].Selected = true;
            }
            
            dgRackStatistics.DataSource = statisticsModel.GetRackStatistcs().OrderBy(stat => stat.RackNumber).ToList();
        }
    }
}
