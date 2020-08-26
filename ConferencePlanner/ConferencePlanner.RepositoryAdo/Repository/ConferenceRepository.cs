﻿using ConferencePlanner.Abstraction.Model;
using ConferencePlanner.Abstraction.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace ConferencePlanner.Repository.Ado.Repository
{
    public class ConferenceRepository : IConferenceRepository
    {
        private readonly SqlConnection _sqlConnection;

        public ConferenceRepository(SqlConnection sqlConnection)
        {
            _sqlConnection = sqlConnection;
        }

        public List<ConferenceModel> GetConference()
        {
            SqlCommand sqlCommand = _sqlConnection.CreateCommand();
            sqlCommand.CommandText = "SELECT c.ConferenceId, c.ConferenceTypeId, c.LocationId, " +
                                        "c.ConferenceCategoryId, c.HostEmail, c.StartDate, c.EndDate, c.ConferenceName " +
                                        "FROM Conference c";
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            List<ConferenceModel> conferences = new List<ConferenceModel>();
            if (sqlDataReader.HasRows)
            {
                while (sqlDataReader.Read())
                {
                    DateTime timeNow = DateTime.Now;
                    DateTime StartDate = sqlDataReader.GetDateTime("StartDate");
                    var ConferenceName = "";
                    if (!sqlDataReader.IsDBNull("ConferenceName"))
                    {
                        ConferenceName = sqlDataReader.GetString("ConferenceName");
                    }
                    if (timeNow < StartDate)
                        conferences.Add(new ConferenceModel()
                        {
                            ConferenceId = sqlDataReader.GetInt32("ConferenceId"),
                            ConferenceTypeId = sqlDataReader.GetInt32("ConferenceTypeId"),
                            LocationId = sqlDataReader.GetInt32("LocationId"),
                            ConferenceCategoryId = sqlDataReader.GetInt32("ConferenceCategoryId"),
                            HostEmail = sqlDataReader.GetString("HostEmail"),
                            StartDate = StartDate,
                            EndDate = sqlDataReader.GetDateTime("StartDate"),
                            ConferenceName = ConferenceName
                        });

                }
            }
            return conferences;
        }
        public List<ConferenceDetailModel> GetConferenceDetail()
        {
            SqlCommand sqlCommand = _sqlConnection.CreateCommand();
            sqlCommand.CommandText = "select c.ConferenceId, c.ConferenceName, c.StartDate,c.EndDate, d.DictionaryConferenceTypeName," +
                " ci.DictionaryCityName, dcc.DictionaryConferenceCategoryName, sp.SpeakerName, c.HostEmail " +
                "from Conference c " +
                "INNER JOIN DictionaryConferenceType d on ConferenceTypeId = d.DictionaryConferenceTypeId " +
                "INNER JOIN Location l on c.LocationId = l.LocationId " +
                "INNER JOIN DictionaryCity ci on l.CityId = ci.DictionaryCityId " +
                "INNER JOIN DictionaryConferenceCategory dcc on c.ConferenceCategoryId = dcc.DictionaryConferenceCategoryId " +
                "INNER JOIN  SpeakerxConference spc on c.ConferenceId = spc.ConferenceId " +
                "INNER JOIN Speaker sp on sp.SpeakerId = spc.SpeakerId " +
                "WHERE StartDate > GETDATE()" +
                "ORDER BY StartDate";
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            List<ConferenceDetailModel> conferenceDetails = new List<ConferenceDetailModel>();
            if (sqlDataReader.HasRows)
            {
                while (sqlDataReader.Read())
                {
                    var conferenceName = "";
                    var conferenceTypeName = "";
                    var conferenceCityName = "";
                    var conferenceCategoryName = "";
                    var conferenceSpeakerName = "";
                    var conferenceHostEmail = "";
                    if (!sqlDataReader.IsDBNull("ConferenceName"))
                    {
                        conferenceName = sqlDataReader.GetString("ConferenceName");
                    }
                    if (!sqlDataReader.IsDBNull("DictionaryConferenceTypeName"))
                    {
                        conferenceTypeName = sqlDataReader.GetString("DictionaryConferenceTypeName");
                    }
                    if (!sqlDataReader.IsDBNull("DictionaryCityName"))
                    {
                        conferenceCityName = sqlDataReader.GetString("DictionaryCityName");
                    }
                    if (!sqlDataReader.IsDBNull("DictionaryConferenceCategoryName"))
                    {
                        conferenceCategoryName = sqlDataReader.GetString("DictionaryConferenceCategoryName");
                    }
                    if (!sqlDataReader.IsDBNull("SpeakerName"))
                    {
                        conferenceSpeakerName = sqlDataReader.GetString("SpeakerName");
                    }
                    if (!sqlDataReader.IsDBNull("HostEmail"))
                    {
                        conferenceHostEmail = sqlDataReader.GetString("HostEmail");
                    }
                    conferenceDetails.Add(new ConferenceDetailModel()
                    {
                        ConferenceName = conferenceName,
                        StartDate = sqlDataReader.GetDateTime("StartDate"),
                        EndDate = sqlDataReader.GetDateTime("EndDate"),
                        DictionaryConferenceTypeName = conferenceTypeName,
                        DictionaryCityName = conferenceCityName,
                        DictionaryConferenceCategoryName = conferenceCategoryName,
                        SpeakerName = conferenceSpeakerName,
                        HostEmail = conferenceHostEmail,
                        ConferenceId = sqlDataReader.GetInt32("ConferenceId")
                    });
                }
            }
            return conferenceDetails;
        }
        public List<ConferenceDetailModel> GetConferenceDetail(DateTime StartDate, DateTime EndDate)
        {
            SqlCommand sqlCommand = _sqlConnection.CreateCommand();
            sqlCommand.CommandText = "select c.ConferenceId, c.ConferenceName, c.StartDate,c.EndDate, d.DictionaryConferenceTypeName," +
                " ci.DictionaryCityName, dcc.DictionaryConferenceCategoryName, sp.SpeakerName, c.HostEmail " +
                "from Conference c " +
                "INNER JOIN DictionaryConferenceType d on ConferenceTypeId = d.DictionaryConferenceTypeId " +
                "INNER JOIN Location l on c.LocationId = l.LocationId " +
                "INNER JOIN DictionaryCity ci on l.CityId = ci.DictionaryCityId " +
                "INNER JOIN DictionaryConferenceCategory dcc on c.ConferenceCategoryId = dcc.DictionaryConferenceCategoryId " +
                "INNER JOIN  SpeakerxConference spc on c.ConferenceId = spc.ConferenceId " +
                "INNER JOIN Speaker sp on sp.SpeakerId = spc.SpeakerId " +
                "WHERE StartDate > GETDATE()" +
                "ORDER BY StartDate";
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            List<ConferenceDetailModel> conferenceDetails = new List<ConferenceDetailModel>();
            if (sqlDataReader.HasRows)
            {
                while (sqlDataReader.Read())
                {
                    DateTime conferenceStartDate = new DateTime();
                    conferenceStartDate = sqlDataReader.GetDateTime("StartDate");
                    if (conferenceStartDate > StartDate && conferenceStartDate < EndDate)
                    {
                        var conferenceName = "";
                        var conferenceTypeName = "";
                        var conferenceCityName = "";
                        var conferenceCategoryName = "";
                        var conferenceSpeakerName = "";
                        var conferenceHostEmail = "";
                        if (!sqlDataReader.IsDBNull("ConferenceName"))
                        {
                            conferenceName = sqlDataReader.GetString("ConferenceName");
                        }
                        if (!sqlDataReader.IsDBNull("DictionaryConferenceTypeName"))
                        {
                            conferenceTypeName = sqlDataReader.GetString("DictionaryConferenceTypeName");
                        }
                        if (!sqlDataReader.IsDBNull("DictionaryCityName"))
                        {
                            conferenceCityName = sqlDataReader.GetString("DictionaryCityName");
                        }
                        if (!sqlDataReader.IsDBNull("DictionaryConferenceCategoryName"))
                        {
                            conferenceCategoryName = sqlDataReader.GetString("DictionaryConferenceCategoryName");
                        }
                        if (!sqlDataReader.IsDBNull("SpeakerName"))
                        {
                            conferenceSpeakerName = sqlDataReader.GetString("SpeakerName");
                        }
                        if (!sqlDataReader.IsDBNull("HostEmail"))
                        {
                            conferenceHostEmail = sqlDataReader.GetString("HostEmail");
                        }
                        conferenceDetails.Add(new ConferenceDetailModel()
                        {
                            ConferenceName = conferenceName,
                            StartDate = sqlDataReader.GetDateTime("StartDate"),
                            EndDate = sqlDataReader.GetDateTime("EndDate"),
                            DictionaryConferenceTypeName = conferenceTypeName,
                            DictionaryCityName = conferenceCityName,
                            DictionaryConferenceCategoryName = conferenceCategoryName,
                            SpeakerName = conferenceSpeakerName,
                            HostEmail = conferenceHostEmail,
                            ConferenceId = sqlDataReader.GetInt32("ConferenceId")
                        });
                    }
                }
            }
            return conferenceDetails;
        }
        public List<ConferenceDetailModel> GetConferenceDetailForHost(string hostName)
        {
            SqlCommand sqlCommand = _sqlConnection.CreateCommand();
            sqlCommand.CommandText = "select c.ConferenceId, c.ConferenceName, c.StartDate,c.EndDate, d.DictionaryConferenceTypeName," +
                " ci.DictionaryCityName, dcc.DictionaryConferenceCategoryName, sp.SpeakerName, c.HostEmail " +
                "from Conference c " +
                "INNER JOIN DictionaryConferenceType d on ConferenceTypeId = d.DictionaryConferenceTypeId " +
                "INNER JOIN Location l on c.LocationId = l.LocationId " +
                "INNER JOIN DictionaryCity ci on l.CityId = ci.DictionaryCityId " +
                "INNER JOIN DictionaryConferenceCategory dcc on c.ConferenceCategoryId = dcc.DictionaryConferenceCategoryId " +
                "INNER JOIN  SpeakerxConference spc on c.ConferenceId = spc.ConferenceId " +
                "INNER JOIN Speaker sp on sp.SpeakerId = spc.SpeakerId " +
                "WHERE StartDate > GETDATE()" +
                "ORDER BY StartDate";
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            List<ConferenceDetailModel> conferenceDetails = new List<ConferenceDetailModel>();
            if (sqlDataReader.HasRows)
            {
                while (sqlDataReader.Read())
                {
                    var conferenceHostEmail = "";
                    if (!sqlDataReader.IsDBNull("HostEmail"))
                    {
                        conferenceHostEmail = sqlDataReader.GetString("HostEmail");
                    }
                    if (conferenceHostEmail == hostName)
                    {
                        var conferenceName = "";
                        var conferenceTypeName = "";
                        var conferenceCityName = "";
                        var conferenceCategoryName = "";
                        var conferenceSpeakerName = "";

                        if (!sqlDataReader.IsDBNull("ConferenceName"))
                        {
                            conferenceName = sqlDataReader.GetString("ConferenceName");
                        }
                        if (!sqlDataReader.IsDBNull("DictionaryConferenceTypeName"))
                        {
                            conferenceTypeName = sqlDataReader.GetString("DictionaryConferenceTypeName");
                        }
                        if (!sqlDataReader.IsDBNull("DictionaryCityName"))
                        {
                            conferenceCityName = sqlDataReader.GetString("DictionaryCityName");
                        }
                        if (!sqlDataReader.IsDBNull("DictionaryConferenceCategoryName"))
                        {
                            conferenceCategoryName = sqlDataReader.GetString("DictionaryConferenceCategoryName");
                        }
                        if (!sqlDataReader.IsDBNull("SpeakerName"))
                        {
                            conferenceSpeakerName = sqlDataReader.GetString("SpeakerName");
                        }
                        conferenceDetails.Add(new ConferenceDetailModel()
                        {
                            ConferenceName = conferenceName,
                            StartDate = sqlDataReader.GetDateTime("StartDate"),
                            EndDate = sqlDataReader.GetDateTime("EndDate"),
                            DictionaryConferenceTypeName = conferenceTypeName,
                            DictionaryCityName = conferenceCityName,
                            DictionaryConferenceCategoryName = conferenceCategoryName,
                            SpeakerName = conferenceSpeakerName,
                            HostEmail = conferenceHostEmail,
                            ConferenceId = sqlDataReader.GetInt32("ConferenceId")
                        });
                    }
                }
            }
            return conferenceDetails;
        }
        public List<ConferenceDetailModel> GetConferenceDetailForHost(string hostName,DateTime StartDate, DateTime EndDate)
        {
            SqlCommand sqlCommand = _sqlConnection.CreateCommand();
            sqlCommand.CommandText = "select c.ConferenceId, c.ConferenceName, c.StartDate,c.EndDate, d.DictionaryConferenceTypeName," +
                " ci.DictionaryCityName, dcc.DictionaryConferenceCategoryName, sp.SpeakerName, c.HostEmail " +
                "from Conference c " +
                "INNER JOIN DictionaryConferenceType d on ConferenceTypeId = d.DictionaryConferenceTypeId " +
                "INNER JOIN Location l on c.LocationId = l.LocationId " +
                "INNER JOIN DictionaryCity ci on l.CityId = ci.DictionaryCityId " +
                "INNER JOIN DictionaryConferenceCategory dcc on c.ConferenceCategoryId = dcc.DictionaryConferenceCategoryId " +
                "INNER JOIN  SpeakerxConference spc on c.ConferenceId = spc.ConferenceId " +
                "INNER JOIN Speaker sp on sp.SpeakerId = spc.SpeakerId " +
                "WHERE StartDate > GETDATE()" +
                "ORDER BY StartDate";
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            List<ConferenceDetailModel> conferenceDetails = new List<ConferenceDetailModel>();
            if (sqlDataReader.HasRows)
            {
                while (sqlDataReader.Read())
                {
                    var conferenceHostEmail = "";
                    DateTime conferenceStartDate = new DateTime();
                    if (!sqlDataReader.IsDBNull("HostEmail"))
                    {
                        conferenceHostEmail = sqlDataReader.GetString("HostEmail");
                        conferenceStartDate = sqlDataReader.GetDateTime("StartDate");
                    }
                    if (conferenceHostEmail == hostName && conferenceStartDate > StartDate && conferenceStartDate < EndDate)
                    {
                        var conferenceName = "";
                        var conferenceTypeName = "";
                        var conferenceCityName = "";
                        var conferenceCategoryName = "";
                        var conferenceSpeakerName = "";

                        if (!sqlDataReader.IsDBNull("ConferenceName"))
                        {
                            conferenceName = sqlDataReader.GetString("ConferenceName");
                        }
                        if (!sqlDataReader.IsDBNull("DictionaryConferenceTypeName"))
                        {
                            conferenceTypeName = sqlDataReader.GetString("DictionaryConferenceTypeName");
                        }
                        if (!sqlDataReader.IsDBNull("DictionaryCityName"))
                        {
                            conferenceCityName = sqlDataReader.GetString("DictionaryCityName");
                        }
                        if (!sqlDataReader.IsDBNull("DictionaryConferenceCategoryName"))
                        {
                            conferenceCategoryName = sqlDataReader.GetString("DictionaryConferenceCategoryName");
                        }
                        if (!sqlDataReader.IsDBNull("SpeakerName"))
                        {
                            conferenceSpeakerName = sqlDataReader.GetString("SpeakerName");
                        }
                        conferenceDetails.Add(new ConferenceDetailModel()
                        {
                            ConferenceName = conferenceName,
                            StartDate = sqlDataReader.GetDateTime("StartDate"),
                            EndDate = sqlDataReader.GetDateTime("EndDate"),
                            DictionaryConferenceTypeName = conferenceTypeName,
                            DictionaryCityName = conferenceCityName,
                            DictionaryConferenceCategoryName = conferenceCategoryName,
                            SpeakerName = conferenceSpeakerName,
                            HostEmail = conferenceHostEmail,
                            ConferenceId = sqlDataReader.GetInt32("ConferenceId")
                        });
                    }
                }
            }
            return conferenceDetails;
        }
        public List<ConferenceAudienceModel> GetConferenceAudience(string email)
        {
            SqlCommand sqlCommand = _sqlConnection.CreateCommand();
            sqlCommand.CommandText = "SELECT c.ConferenceAudienceId, c.ConferenceId, c.Participant, " +
                                    "c.ConferenceStatusId, c.UniqueParticipantCode " +
                                    "FROM ConferenceAudience c " +
                                    "WHERE c.Participant = @Participant";
            sqlCommand.Parameters.Add("@Participant", SqlDbType.VarChar, 100).Value = email;
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            List<ConferenceAudienceModel> conferenceAudience = new List<ConferenceAudienceModel>();
            if (sqlDataReader.HasRows)
            {
                while (sqlDataReader.Read())
                {
                    conferenceAudience.Add(new ConferenceAudienceModel()
                    {
                        ConferenceAudienceId = sqlDataReader.GetInt32("ConferenceAudienceId"),
                        ConferenceId = sqlDataReader.GetInt32("ConferenceId"),
                        Participant = sqlDataReader.GetString("Participant"),
                        ConferenceStatusId = sqlDataReader.GetInt32("ConferenceStatusId"),
                        UniqueParticipantCode = sqlDataReader.GetString("UniqueParticipantCode")

                    });
                }
                
            }
            return conferenceAudience;
        }
        /*
        public List<ConferenceDetailModel> GetAttendedConferecesFirst(List<ConferenceAudienceModel> _attendedConferences,
             string currentUser, DateTime StartDate, DateTime EndDate)
        {
            SqlCommand sqlCommand = _sqlConnection.CreateCommand();
            sqlCommand.CommandText = "select c.ConferenceId, c.ConferenceName, c.StartDate,c.EndDate, d.DictionaryConferenceTypeName," +
                " ci.DictionaryCityName, dcc.DictionaryConferenceCategoryName, sp.SpeakerName, c.HostEmail " +
                "from Conference c " +
                "INNER JOIN DictionaryConferenceType d on ConferenceTypeId = d.DictionaryConferenceTypeId " +
                "INNER JOIN Location l on c.LocationId = l.LocationId " +
                "INNER JOIN DictionaryCity ci on l.CityId = ci.DictionaryCityId " +
                "INNER JOIN DictionaryConferenceCategory dcc on c.ConferenceCategoryId = dcc.DictionaryConferenceCategoryId " +
                "INNER JOIN  SpeakerxConference spc on c.ConferenceId = spc.ConferenceId " +
                "INNER JOIN Speaker sp on sp.SpeakerId = spc.SpeakerId " +
                "WHERE StartDate > GETDATE()" +
                "ORDER BY StartDate";

            List<ConferenceDetailAttendFirstModel> conferenceDetails = new List<ConferenceDetailAttendFirstModel>();
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            if (sqlDataReader.HasRows)
            {
                while (sqlDataReader.Read())
                {
                    DateTime conferenceStartDate = new DateTime();
                    conferenceStartDate = sqlDataReader.GetDateTime("StartDate");
                    if (conferenceStartDate > StartDate && conferenceStartDate < EndDate)
                    {
                        var conferenceName = "";
                        var conferenceTypeName = "";
                        var conferenceCityName = "";
                        var conferenceCategoryName = "";
                        var conferenceSpeakerName = "";
                        var conferenceHostEmail = "";
                        int conferenceId;
                        if (!sqlDataReader.IsDBNull("ConferenceName"))
                        {
                            conferenceName = sqlDataReader.GetString("ConferenceName");
                        }
                        if (!sqlDataReader.IsDBNull("DictionaryConferenceTypeName"))
                        {
                            conferenceTypeName = sqlDataReader.GetString("DictionaryConferenceTypeName");
                        }
                        if (!sqlDataReader.IsDBNull("DictionaryCityName"))
                        {
                            conferenceCityName = sqlDataReader.GetString("DictionaryCityName");
                        }
                        if (!sqlDataReader.IsDBNull("DictionaryConferenceCategoryName"))
                        {
                            conferenceCategoryName = sqlDataReader.GetString("DictionaryConferenceCategoryName");
                        }
                        if (!sqlDataReader.IsDBNull("SpeakerName"))
                        {
                            conferenceSpeakerName = sqlDataReader.GetString("SpeakerName");
                        }
                        if (!sqlDataReader.IsDBNull("HostEmail"))
                        {
                            conferenceHostEmail = sqlDataReader.GetString("HostEmail");
                        }
                        conferenceId = sqlDataReader.GetInt32("ConferenceId");
                        conferenceDetails.Add(new ConferenceDetailAttendFirstModel()
                        {
                            ConferenceName = conferenceName,
                            StartDate = sqlDataReader.GetDateTime("StartDate"),
                            EndDate = sqlDataReader.GetDateTime("EndDate"),
                            DictionaryConferenceTypeName = conferenceTypeName,
                            DictionaryCityName = conferenceCityName,
                            DictionaryConferenceCategoryName = conferenceCategoryName,
                            SpeakerName = conferenceSpeakerName,
                            HostEmail = conferenceHostEmail,
                            ConferenceId = conferenceId,
                            ConferenceStatusId = _attendedConferences.Exists(currentConference => 
                                                currentConference.ConferenceId == conferenceId ? currentConference.ConferenceStatusId : 0) 
                        
                        });
                    }

                }
            }
            return conferenceDetails;
        }
        */
        public void AddParticipant(ConferenceAudienceModel _conferenceAudienceModel)
        {

            SqlCommand command = _sqlConnection.CreateCommand();
            command.CommandText = "INSERT INTO ConferenceAudience (ConferenceId,Participant,ConferenceStatusId,UniqueParticipantCode) " +
                                    "VALUES (@ConferenceId, @Participant, @ConferenceStatusId, @UniqueParticipantCode)";
            command.Parameters.Add("@ConferenceId", SqlDbType.Int).Value = _conferenceAudienceModel.ConferenceId;
            command.Parameters.Add("@Participant", SqlDbType.VarChar, 100).Value = _conferenceAudienceModel.Participant;
            command.Parameters.Add("@ConferenceStatusId", SqlDbType.Int).Value = _conferenceAudienceModel.ConferenceStatusId;
            command.Parameters.Add("@UniqueParticipantCode", SqlDbType.VarChar, 255).Value = _conferenceAudienceModel.UniqueParticipantCode;
            command.ExecuteNonQuery();
        }
        public int UpdateParticipant(ConferenceAudienceModel _conferenceAudienceModel)
        {
            SqlCommand command = _sqlConnection.CreateCommand();
            command.CommandText = "UPDATE ConferenceAudience " +
                                    "SET ConferenceStatusId = @ConferenceStatusId " +
                                    "WHERE Participant = @Participant and ConferenceId = @ConferenceId";
            command.Parameters.Add("@ConferenceStatusId ", SqlDbType.Int).Value = _conferenceAudienceModel.ConferenceStatusId;
            command.Parameters.Add("@Participant", SqlDbType.VarChar, 100).Value = _conferenceAudienceModel.Participant;
            command.Parameters.Add("@ConferenceId", SqlDbType.Int).Value = _conferenceAudienceModel.ConferenceId;

            return(command.ExecuteNonQuery());
        }
        public int UpdateParticipantToJoin(ConferenceAudienceModel _conferenceAudienceModel)
        {
            SqlCommand command = _sqlConnection.CreateCommand();
            command.CommandText = "UPDATE ConferenceAudience " +
                                    "SET ConferenceStatusId = @ConferenceStatusId " +
                                    "WHERE Participant = @Participant and ConferenceId = @ConferenceId " +
                                    "and ConferenceStatusId = 3";
            command.Parameters.Add("@ConferenceStatusId ", SqlDbType.Int).Value = _conferenceAudienceModel.ConferenceStatusId;
            command.Parameters.Add("@Participant", SqlDbType.VarChar, 100).Value = _conferenceAudienceModel.Participant;
            command.Parameters.Add("@ConferenceId", SqlDbType.Int).Value = _conferenceAudienceModel.ConferenceId;

            return (command.ExecuteNonQuery());
        }

        public string GetUniqueParticipantCode()
        {
            Guid g = Guid.NewGuid();
            string GuidString = Convert.ToBase64String(g.ToByteArray());
            GuidString = GuidString.Replace("=", "");
            GuidString = GuidString.Replace("+", "");
            return GuidString;
        }

        public void AddCountry(string Code, string Name)
        {
            SqlCommand command = _sqlConnection.CreateCommand();
            command.CommandText = "INSERT INTO DictionaryCountry (DictionaryCountryName,DictionaryCountryCode) " +
                                    "VALUES (@CountryName, @CountryCode)";
            command.Parameters.Add("@CountryName", SqlDbType.VarChar, 100).Value = Name;
            command.Parameters.Add("@CountryCode", SqlDbType.VarChar, 100).Value = Code;
            command.ExecuteNonQuery();

        }
        public void AddCounty(string Code, string Name, string country)
        {
            SqlCommand command = _sqlConnection.CreateCommand();
            command.CommandText = "INSERT INTO DictionaryCounty (DictionaryCountyName,DictionaryCountyCode,DictionaryCountryId) " +
                                    "VALUES (@CountyName, @CountyCode, @CountryId)";
            command.Parameters.Add("@CountyName", SqlDbType.VarChar, 100).Value = Name;
            command.Parameters.Add("@CountyCode", SqlDbType.VarChar, 100).Value = Code;
            command.Parameters.Add("@CountryId", SqlDbType.Int, 100).Value = Int32.Parse(country);
            command.ExecuteNonQuery();
        }
        public void AddCity(string Code, string Name, string county)
        {
            SqlCommand command = _sqlConnection.CreateCommand();
            command.CommandText = "INSERT INTO DictionaryCity (DictionaryCityName,DictionaryCityCode,DictionaryCountyId) " +
                                    "VALUES (@CityName, @CityCode, @CountyId)";
            command.Parameters.Add("@CityName", SqlDbType.VarChar, 100).Value = Name;
            command.Parameters.Add("@CityCode", SqlDbType.VarChar, 100).Value = Code;
            command.Parameters.Add("@CountyId", SqlDbType.Int, 100).Value = Int32.Parse(county);
            command.ExecuteNonQuery();
        }
        public void AddSpeaker(string Email,string Name)
        {
            SqlCommand command = _sqlConnection.CreateCommand();
            command.CommandText = "INSERT INTO Speaker (SpeakerName,SpeakerEmail) " +
                                    "VALUES (@SpeakerName,@SpeakerEmail)";
            command.Parameters.Add("@SpeakerName", SqlDbType.VarChar, 100).Value = Name;
            command.Parameters.Add("@SpeakerEmail", SqlDbType.VarChar, 100).Value = Email;
            command.ExecuteNonQuery();
        }
        public void AddType(string Name)
        {
            SqlCommand command = _sqlConnection.CreateCommand();
            command.CommandText = "INSERT INTO DictionaryConferenceType (DictionaryConferenceTypeName,IsRemote) " +
                                    "VALUES (@ConferenceName,@IsRemote)";
            command.Parameters.Add("@ConferenceName", SqlDbType.VarChar, 100).Value = Name;
            command.Parameters.Add("@IsRemote", SqlDbType.Int, 100).Value = 1;
            command.ExecuteNonQuery();
        }
        public void AddCategory(string Name)
        {
            int index = 0;
            SqlCommand command = _sqlConnection.CreateCommand();
            command.CommandText = "SELECT MAX(DictionaryConferenceCategoryId) abc FROM DictionaryConferenceCategory ";
            command.ExecuteNonQuery();
            SqlDataReader sqldatareader = command.ExecuteReader();
            if (sqldatareader.HasRows)
            {
                sqldatareader.Read();
                index = sqldatareader.GetInt32("abc");
            }
            sqldatareader.Close();
            command.CommandText = "INSERT INTO DictionaryConferenceCategory (DictionaryConferenceCategoryName,DictionaryConferenceCategoryId) " +
                                    "VALUES (@ConferenceCategoryName,@ConferenceCategoryId)";
            command.Parameters.Add("@ConferenceCategoryName", SqlDbType.VarChar, 100).Value = Name;
            command.Parameters.Add("@ConferenceCategoryId", SqlDbType.Int, 100).Value = index+1;
            command.ExecuteNonQuery();
        }

        public List<ConferenceDetailAttendFirstModel> GetAttendedConferecesFirst(List<ConferenceAudienceModel> _attendedConferences, string currentUser, DateTime StartDate, DateTime EndDate)
        {
            throw new NotImplementedException();
        }
    }
}
