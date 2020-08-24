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

namespace ConferencePlanner.WinUi
{
    public partial class AddEvent : Form
    { 
        private readonly IConferenceRepository _ConferenceRepository;

        private int totalEntries;
        private int currentOffset;
        private int startingPoint;
        private List<ConferenceDetailModel> x;
        private string currentUser;


        public AddEvent(IConferenceRepository ConferenceRepository, 
            string var_email,
            string ConferenceName, 
            string Type, 
            string Category,
            string Address, 
            string MainSpeaker, 
            DateTime StartDate, 
            DateTime EndDate)
        {
            
            InitializeComponent();

            currentUser = var_email;

            AddName.Text = ConferenceName;
            AddType.Text = Type;
            AddCategory.Text = Category;
            AddAddress.Text = Address;
            AddMainSpeaker.Text = MainSpeaker;
            AddStartDate.Value = StartDate;
            AddEndDate.Value = EndDate;
            if (ConferenceName != null) { btnEdit.Visible = true; }
            else btnAdd.Visible = true;
            


            _ConferenceRepository = ConferenceRepository;
            x = _ConferenceRepository.GetConferenceDetail();

            
        }

        private void Form3_Activated(object sender, System.EventArgs e)
        {
            AddName.Text = "string";
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}