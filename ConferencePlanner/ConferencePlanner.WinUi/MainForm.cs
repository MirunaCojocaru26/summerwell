﻿using ConferencePlanner.Abstraction.Repository;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ConferencePlanner.Abstraction.Model;
using System.Data.SqlClient;
using Accessibility;
using ConferencePlanner.Repository.Ado.Repository;
using System.Drawing.Text;
using Windows.UI.Xaml.Documents;
using System.Security.Cryptography.X509Certificates;

namespace ConferencePlanner.WinUi
{

    public partial class MainForm : Form
    {
        private readonly IConferenceRepository _ConferenceRepository;
        private readonly IConferenceTypeRepository _ConferenceTypeRepository;
        private readonly IGetSpeakerDetail _GetSpeakerDetail;

        private int totalEntries;
        private int startingPoint;
        private int HosttotalEntries;
        private int HoststartingPoint;
        private List<ConferenceDetailModel> x;
        private List<ConferenceDetailModel> y;
        private string currentUser;

        public MainForm(IGetSpeakerDetail GetSpeakerDetail, IConferenceTypeRepository conferenceTypeRepository,  IConferenceRepository ConferenceRepository, string var_email)
        {
            InitializeComponent();
            _ConferenceTypeRepository = conferenceTypeRepository;
            _ConferenceRepository = ConferenceRepository;
            _GetSpeakerDetail = GetSpeakerDetail;
            x = _ConferenceRepository.GetConferenceDetail();
            currentUser = var_email;
            y = _ConferenceRepository.GetConferenceDetailForHost(currentUser);
            totalEntries = x.Count;
            startingPoint = 0;
            HoststartingPoint = 0;
            HosttotalEntries = y.Count;

            if (x == null || x.Count() == 0)
            {
                return;
            }
            populateConferenceGridViewByDate(0, 5, dateTimePicker2.Value, dateTimePicker1.Value);
            changeColor();

            if (y == null || y.Count() == 0)
            {
                return;
            }
            populateHostGridViewByDate(0, 5, dateTimePicker4.Value, dateTimePicker3.Value);
            
        }

        private void populateConferenceGridViewByDate(int startingPoint, int endingPoint, DateTime StartDate, DateTime EndDate)
        {
            for (int i = startingPoint; i < endingPoint; i++)
            {
                if (x[i].StartDate > StartDate && x[i].StartDate < EndDate)
                {
                    dataGridView1.Rows.Add(x[i].ConferenceName, x[i].StartDate, x[i].DictionaryConferenceTypeName,
                                       x[i].DictionaryConferenceCategoryName,
                                       x[i].DictionaryCityName,
                                       x[i].SpeakerName,
                                       null, null, null, x[i].ConferenceId);
                }
            }


        }
        private void populateHostGridViewByDate(int startingPoint, int endingPoint,DateTime StartDate, DateTime EndDate)
            {   
                for (int i = startingPoint; i < endingPoint; i++)
                {

                    if (y[i].StartDate > StartDate && y[i].StartDate < EndDate)
                    {
                        dataGridView2.Rows.Add(y[i].ConferenceName, y[i].StartDate, y[i].DictionaryConferenceTypeName,
                                  y[i].DictionaryConferenceCategoryName,
                                  y[i].DictionaryCityName,
                                  y[i].SpeakerName,
                                  null, null, null, y[i].ConferenceId);
                    }
                }


            }
    
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;
            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn)
            {
                bool isAttend = false;
                bool isJoin = false;
                bool inWithdraw = false;
                int colindex = senderGrid.CurrentCell.ColumnIndex;
                if(colindex.ToString().Equals("6") && isAttend== false)
                {
                    ConferenceAudienceModel _conferenceAudienceModel = new ConferenceAudienceModel();
                    isAttend = true;
                    pressButtonGreen(sender, e.RowIndex, e.ColumnIndex);
                    _conferenceAudienceModel.ConferenceId = (int)dataGridView1.Rows[e.RowIndex].Cells["ConferenceId"].Value;
                    _conferenceAudienceModel.Participant = currentUser;
                    _conferenceAudienceModel.ConferenceStatusId = 3;
                    try
                    {
                        _ConferenceRepository.AddParticipant(_conferenceAudienceModel);
                    }
                    catch(SqlException ex)
                    {
                        _ConferenceRepository.UpdateParticipant(_conferenceAudienceModel);
                    }
                    InitTimer(sender, e.RowIndex, e.ColumnIndex);

                }
                if (colindex.ToString().Equals("7") && isJoin == false)
                {
                    isJoin = true;
                    DateTime startDate = (DateTime)dataGridView1.Rows[e.RowIndex].Cells["StartDate"].Value;
                    DateTime currentTime = DateTime.Now;
                    if (currentTime.AddMinutes(5) >= startDate)
                    {
                        ConferenceAudienceModel _conferenceAudienceModel = new ConferenceAudienceModel();
                        _conferenceAudienceModel.ConferenceId = (int)dataGridView1.Rows[e.RowIndex].Cells["ConferenceId"].Value;
                        _conferenceAudienceModel.Participant = currentUser;
                        _conferenceAudienceModel.ConferenceStatusId = 1;
                        int rows_affected = _ConferenceRepository.UpdateParticipant(_conferenceAudienceModel);
                        if (rows_affected > 0)
                        {
                            JoinConference jc = new JoinConference();
                            jc.Show(this);
                            pressButtonGreen(sender, e.RowIndex, e.ColumnIndex);
                        }
                        else
                        {
                            MessageBox.Show("You have to attend before you can join!");
                        }

                    }
                    else
                    {
                        MessageBox.Show("You can't join the conference yet!");
                    }
                }
                if (colindex.ToString().Equals("8") && inWithdraw == false)
                {
                    ConferenceAudienceModel _conferenceAudienceModel = new ConferenceAudienceModel();
                    _conferenceAudienceModel.ConferenceId = (int)dataGridView1.Rows[e.RowIndex].Cells["ConferenceId"].Value;
                    _conferenceAudienceModel.Participant = currentUser;
                    _conferenceAudienceModel.ConferenceStatusId = 2;
                    int rows_affected = _ConferenceRepository.UpdateParticipant(_conferenceAudienceModel);
                    if (rows_affected <= 0)
                        MessageBox.Show("You have to attend before you can withdraw!");
                    inWithdraw = true;
                    pressButtonGreen(sender, e.RowIndex, e.ColumnIndex);
                    isAttend = true;
                    pressButtonGreen(sender, e.RowIndex, e.ColumnIndex-2);
                }
            }

        }
        
        private void pressButtonGreen(object sender,int row, int col)
        {
            var senderGrid = (DataGridView)sender;
            DataGridViewButtonCell bc = ((DataGridViewButtonCell)senderGrid.Rows[row].Cells[col]);
            bc.FlatStyle = FlatStyle.Flat;
            bc.Style.BackColor = Color.Red;
            bc.Style.ForeColor = Color.DarkRed;
        }

        private void makeButtonGreen(object datagrid, int row, int col)
        {
            var senderGrid = (DataGridView)datagrid;
            DataGridViewButtonCell bc = ((DataGridViewButtonCell)senderGrid.Rows[row].Cells[col]);
            bc.FlatStyle = FlatStyle.Flat;
            bc.Style.BackColor = Color.Green;
            bc.Style.ForeColor = Color.DarkGreen;
        }

        private Timer timer1;
        public void InitTimer(object datagrid, int row, int col)
        {
            timer1 = new Timer();
            timer1.Tick += (sender,e) => timer1_Tick(sender, e, datagrid, row, col);
            timer1.Interval = 10000; // 10 seconds / 10000 MillSecs
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e, object datagrid, int row, int col)
        {
            var senderGrid = (DataGridView)datagrid;
            try {
                if (!(senderGrid.Rows[row] == null | senderGrid.Rows[row].Cells[1].Value.ToString().Equals("")))
                {
                    DateTime startDate = DateTime.ParseExact(senderGrid.Rows[row].Cells[1].Value.ToString(), "dd.MM.yyyy HH:mm:ss", null);
                    DateTime now = DateTime.Now;
                    //MessageBox.Show(now.ToString());
                    if (now.AddMinutes(5) >= startDate)
                    {
 
                        makeButtonGreen(datagrid, row, col + 1);
                    }
                    if (startDate.AddMinutes(5) <= now)
                    {
                        makeButtonGreen(datagrid, row, col + 2);
                        Timer timer = (Timer)sender;
                        timer.Stop();
                    }
                }
            }
            catch (System.ArgumentOutOfRangeException ex)
            {
                Timer timer = (Timer)sender;
                timer.Stop();
                System.Environment.Exit(1);
            }
        }

        private void changeColor()
        {
            // 
            // Button color
            // 
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                DataGridViewButtonCell bc = ((DataGridViewButtonCell)dataGridView1.Rows[i].Cells[6]);
                bc.FlatStyle = FlatStyle.Flat;
                bc.Style.BackColor = System.Drawing.Color.Green;
                bc.Style.ForeColor = System.Drawing.Color.DarkGreen;
            }
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                DataGridViewButtonCell bc = ((DataGridViewButtonCell)dataGridView1.Rows[i].Cells[7]);
                bc.FlatStyle = FlatStyle.Flat;
                bc.Style.BackColor = System.Drawing.Color.Red;
                bc.Style.ForeColor = System.Drawing.Color.DarkRed;

                DataGridViewButtonCell bc1 = ((DataGridViewButtonCell)dataGridView1.Rows[i].Cells[8]);
                bc1.FlatStyle = FlatStyle.Flat;
                bc1.Style.BackColor = System.Drawing.Color.Red;
                bc1.Style.ForeColor = System.Drawing.Color.DarkRed;
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;
            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn)
            {
                int colindex = senderGrid.CurrentCell.ColumnIndex;
                if (colindex.ToString().Equals("7"))
                { 
                    AddEvent form3 = new AddEvent(_GetSpeakerDetail, _ConferenceTypeRepository, _ConferenceRepository, currentUser,
                        (string)dataGridView2.Rows[e.RowIndex].Cells["HostConferenceName"].Value,
                        (string)dataGridView2.Rows[e.RowIndex].Cells["HostType"].Value,
                        (string)dataGridView2.Rows[e.RowIndex].Cells["HostCategory"].Value, 
                        (string)dataGridView2.Rows[e.RowIndex].Cells["HostAddress"].Value, 
                        (string)dataGridView2.Rows[e.RowIndex].Cells["HostMainSpeaker"].Value,
                        (DateTime)dataGridView2.Rows[e.RowIndex].Cells["HostStartDate"].Value,
                        (DateTime)dataGridView2.Rows[e.RowIndex].Cells["HostEndDate"].Value);
                    form3.Tag = this;
                    form3.Show(this);
                }
            }

        }


        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView2_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            {

                if (dataGridView2.CurrentCell == null)
                {

                    MessageBox.Show("namdate??");
                    //textBox1.Text = dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[2].Value.ToString();
                    // textBox2.Text = dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[1].Value.ToString();
                }
                try
                { 


                    MainSpeakerDetails mf = new MainSpeakerDetails(_ConferenceRepository, dataGridView2.CurrentRow.Cells["MainSpeaker"].Value.ToString());
                    mf.textBox1.Text = this.dataGridView2.CurrentRow.Cells[0].Value.ToString();
                    mf.textBox2.Text = this.dataGridView2.CurrentRow.Cells[1].Value.ToString();
                    mf.textBox3.Text = this.dataGridView2.CurrentRow.Cells[2].Value.ToString();
                    mf.textBox4.Text = this.dataGridView2.CurrentRow.Cells[3].Value.ToString();
                    mf.textBox5.Text = this.dataGridView2.CurrentRow.Cells[4].Value.ToString();
                    mf.textBox6.Text = this.dataGridView2.CurrentRow.Cells[5].Value.ToString();

                  
                 
                    mf.ShowDialog();
                }
                catch (NullReferenceException)
                {
                    MessageBox.Show("You cannot process an empty cell");
                }
            }


        }


        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

            

            if (dataGridView1.CurrentCell == null)
            {

                MessageBox.Show("namdate??");
                //textBox1.Text = dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[2].Value.ToString();
               // textBox2.Text = dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[1].Value.ToString();
            }
            try
            {

                string rating = "";
                string nationality = "";
                string picture = "";
                SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString);
                conn.Open();

                SqlCommand command = new SqlCommand("Select Rating, Nationality, SpeakerImage from Speaker where SpeakerName=@name", conn);
                command.Parameters.AddWithValue("@name", this.dataGridView1.CurrentRow.Cells[5].Value.ToString());
                
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        rating = String.Format("{0}", reader["Rating"]);
                        nationality = String.Format("{0}", reader["Nationality"]);
                        picture = String.Format("{0}", reader["SpeakerImage"]);

                    }
                }

                conn.Close();


                MainSpeakerDetails mf = new MainSpeakerDetails(_ConferenceRepository,dataGridView1.CurrentRow.Cells["MainSpeaker"].Value.ToString());
                mf.textBox1.Text = this.dataGridView1.CurrentRow.Cells[0].Value.ToString();
                mf.textBox2.Text = this.dataGridView1.CurrentRow.Cells[1].Value.ToString();
                mf.textBox3.Text = this.dataGridView1.CurrentRow.Cells[2].Value.ToString();
                mf.textBox4.Text = this.dataGridView1.CurrentRow.Cells[3].Value.ToString();
                mf.textBox5.Text = this.dataGridView1.CurrentRow.Cells[4].Value.ToString();
                mf.textBox6.Text = this.dataGridView1.CurrentRow.Cells[5].Value.ToString();
                mf.textBox7.Text = rating;
                mf.textBox8.Text = nationality;
                mf.ShowDialog();
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("You cannot process an empty cell");
            }
        }

        private void btnAddEvent_Click(object sender, EventArgs e)
        {
            
                    DateTime localDate = DateTime.Now;
                    AddEvent form3 = new AddEvent(_GetSpeakerDetail, _ConferenceTypeRepository, _ConferenceRepository, currentUser, null, null, null, null, null, localDate, localDate);
                    form3.Tag = this;
                    form3.Show(this);
                
            
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
        private void btnHostSearch_Click(object sender, EventArgs e)
        {
            dataGridView2.Rows.Clear();
            populateHostGridViewByDate(0, 5, dateTimePicker4.Value, dateTimePicker3.Value);

        }

        private void previousPage(object sender, EventArgs e)
        {


            if (startingPoint >= 5)
            {
                startingPoint -= 5;
                dataGridView1.Rows.Clear();
                populateConferenceGridViewByDate(startingPoint, startingPoint + 5, dateTimePicker2.Value, dateTimePicker1.Value);
                changeColor();

            }

            else if (startingPoint > 0)
            {
                startingPoint = 0;
                dataGridView1.Rows.Clear();
                populateConferenceGridViewByDate(startingPoint, startingPoint + 5, dateTimePicker2.Value, dateTimePicker1.Value);
                changeColor();

            }

            else
            {
                return;
            }
        }

        


        private void nextPage(object sender, EventArgs e)
        {

            if(startingPoint <= totalEntries - 5)
            {
                startingPoint += 5;
                dataGridView1.Rows.Clear();
                if (startingPoint + 5 < totalEntries) {
                    populateConferenceGridViewByDate(startingPoint, startingPoint + 5, dateTimePicker2.Value, dateTimePicker1.Value);
                    changeColor();
                }
                
                else {
                    populateConferenceGridViewByDate(startingPoint, totalEntries, dateTimePicker2.Value, dateTimePicker1.Value);
                    changeColor();
                }
            }
            else if(startingPoint < totalEntries)
            {
                dataGridView1.Rows.Clear();
                populateConferenceGridViewByDate(startingPoint, totalEntries, dateTimePicker2.Value, dateTimePicker1.Value);
                startingPoint = totalEntries;
            }
            else
            {
                return;
            }
            

            


        }
        private void btnNextHost_Click(object sender, EventArgs e)
        {
            if (HoststartingPoint <= HosttotalEntries - 5)
            {
                HoststartingPoint += 5;
                dataGridView2.Rows.Clear();
                if (HoststartingPoint + 5 < HosttotalEntries)
                {
                    populateHostGridViewByDate(HoststartingPoint, HoststartingPoint + 5, dateTimePicker4.Value, dateTimePicker3.Value);
                }

                else
                {
                    populateHostGridViewByDate(HoststartingPoint, HosttotalEntries, dateTimePicker4.Value, dateTimePicker3.Value);
                }
            }
            else if (HoststartingPoint < HosttotalEntries)
            {
                dataGridView2.Rows.Clear();
                populateHostGridViewByDate(HoststartingPoint, HosttotalEntries, dateTimePicker4.Value, dateTimePicker3.Value);
                HoststartingPoint = HosttotalEntries;
            }
            else
            {
                return;
            }
        }

        private void btnBackHost_Click(object sender, EventArgs e)
        {
            if (HoststartingPoint >= 5)
            {
                HoststartingPoint -= 5;
                dataGridView2.Rows.Clear();
                populateHostGridViewByDate(HoststartingPoint, HoststartingPoint + 5, dateTimePicker4.Value, dateTimePicker3.Value);

            }

            else if (HoststartingPoint > 0)
            {
                HoststartingPoint = 0;
                dataGridView2.Rows.Clear();
                populateHostGridViewByDate(HoststartingPoint, HoststartingPoint + 5, dateTimePicker4.Value, dateTimePicker3.Value);

            }

            else
            {
                return;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            populateConferenceGridViewByDate(0, 5, dateTimePicker2.Value, dateTimePicker1.Value);
            changeColor();
        }
        

    }



}
