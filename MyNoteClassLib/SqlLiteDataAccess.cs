using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MyNoteDataClassLib
{
    public class SqlLiteDataAccess
    {
        /// <summary>
        /// to be set from the app using this DLL prior to use.
        /// </summary>
        public static string SQLiteDBLocation { get; set; }

        #region GroupPhrase

        /// <summary>
        /// Get a groupphrase object from unique title or ID.
        /// </summary>
        /// <param name="strTitle"></param>
        /// <returns>A GroupPhrase object</returns>
        public static GroupPhrase GetGroupPhrase(string GroupPhraseTitle = null)
        {
            if (GroupPhraseTitle != null)
            {
                string sql = "SELECT * FROM GroupPhrases WHERE GroupPhraseTitle = @GroupPhraseTitle;";
                using (IDbConnection cnn = new SQLiteConnection("Data Source=" + SQLiteDBLocation))
                {

                    //to get an ilist cnn.Query<GroupPhrase>("SELECT * FROM GroupPhrases");
                    GroupPhrase output = cnn.QuerySingleOrDefault<GroupPhrase>(sql, new { GroupPhraseTitle });
                    return output;
                }
            }
            return null;
        }

        public static List<GroupPhrase> GetGroupPhrasesByType(int NoteDesignSubsectionID)
        {
            using (IDbConnection cnn = new SQLiteConnection("Data Source=" + SqlLiteDataAccess.SQLiteDBLocation))
            {
                string sql = $"Select * from GroupPhrases g where g.GroupPhraseSubSection = {NoteDesignSubsectionID};";
                return cnn.Query<GroupPhrase>(sql).ToList();
            }
        }

        /// <summary>
        /// Get a groupphrase object from unique title or ID.
        /// </summary>
        /// <param name="strTitle"></param>
        /// <returns>A GroupPhrase object</returns>
        public static GroupPhrase GetGroupPhrase(int? GroupPhraseID = null)
        {
            if (GroupPhraseID != null)
            {
                string sql = "SELECT * FROM GroupPhrases WHERE GroupPhraseID = @GroupPhraseID;";
                using (IDbConnection cnn = new SQLiteConnection("Data Source=" + SQLiteDBLocation))
                {

                    //to get an ilist cnn.Query<GroupPhrase>("SELECT * FROM GroupPhrases");
                    GroupPhrase output = cnn.QuerySingleOrDefault<GroupPhrase>(sql, new { GroupPhraseID });
                    return output;
                }
            }
            return null;
        }


        /// <summary>
        /// Update a groupphrase object
        /// </summary>
        /// <param name="strTitle"></param>
        /// <returns>True if successfull</returns>
        public static bool UpdateGroupPhrase(GroupPhrase gp)
        {
            if (gp != null)
            {
                string sql = "UPDATE GroupPhrases SET "+
                    "GroupPhraseTitle = @GroupPhraseTitle, " +
                    "GroupPhraseOSAdult = @GroupPhraseOSAdult, " +
                    "GroupPhraseCSAdult= @GroupPhraseCSAdult," +
                    "GroupPhraseOSChild= @GroupPhraseOSChild," +
                    "GroupPhraseCSChild= @GroupPhraseCSChild," +
                    "GroupPhraseSubsectionID= @GroupPhraseSubsectionID," +
                    "GroupPhraseConnectionType= @GroupPhraseConnectionType," +
                    "GroupPhraseSideType= @GroupPhraseSideType," +
                    "GroupPhraseComment= @GroupPhraseComment" +
                    " WHERE GroupPhraseID = @GroupPhraseID;";

                using (IDbConnection cnn = new SQLiteConnection("Data Source=" + SQLiteDBLocation))
                {
                    var affectedRows = cnn.Execute(sql, gp);
                }
                return true;
            }
            return false;
        }

        public static bool IsGroupPhraseTitleUnique(string strTitle)
        {
            string sql = "Select count(*) from GroupPhrases gp where gp.GroupPhraseTitle = @GroupPhraseTitle;";
            using (IDbConnection cnn = new SQLiteConnection("Data Source=" + SQLiteDBLocation))
            {
                var iCount = cnn.ExecuteScalar<int>(sql, new { GroupPhraseTitle = strTitle });
                if (iCount > 0) return false;
            }
            return true;
        }

        /// <summary>
        /// Add a groupphrase object to database
        /// </summary>
        /// <param name="gp">a groupphrase object</param>
        /// <returns>True if successfull</returns>
        public static bool AddGroupPhrase(GroupPhrase gp)
        {
            if (gp != null)
            {
                string sql = "INSERT INTO GroupPhrases (" +
                    "GroupPhraseTitle," +
                    "GroupPhraseOSAdult," +
                    "GroupPhraseCSAdult," +
                    "GroupPhraseOSChild," +
                    "GroupPhraseCSChild," +
                    "GroupPhraseSubsectionID," +
                    "GroupPhraseConnectionType," +
                    "GroupPhraseSideType," +
                    "GroupPhraseComment" +
                    ") VALUES (" +
                    "@GroupPhraseTitle," +
                    "@GroupPhraseOSAdult," +
                    "@GroupPhraseCSAdult," +
                    "@GroupPhraseOSChild," +
                    "@GroupPhraseCSChild," +
                    "@GroupPhraseSubsectionID," +
                    "@GroupPhraseConnectionType," +
                    "@GroupPhraseSideType," +
                    "@GroupPhraseComment)";

                using (IDbConnection cnn = new SQLiteConnection("Data Source=" + SQLiteDBLocation))
                {
                    try
                    {
                        var affectedRows = cnn.Execute(sql, gp);

                    }
                    catch (Exception e)
                    {
                        if (e.Message.Contains("UNIQUE constraint failed: GroupPhrases.GroupPhraseTitle"))
                        {
                            MessageBox.Show("Cannot add new GroupPhrase because the title is not unique!");
                            return false;
                        }
                        MessageBox.Show($"Cannot update due to error: ${e.Message}");
                        return false;
                    }

                }
                return true;
            }
            return false;
        }


        public static bool DeleteGroupPhrase(string GroupPhraseTitle = null, int? GroupPhraseID = null)
        {
            if (GroupPhraseTitle != null)
            {
                string sql = "DELETE FROM GroupPhrases WHERE GroupPhraseTitle = @GroupPhraseTitle;";
                using (IDbConnection cnn = new SQLiteConnection("Data Source=" + SQLiteDBLocation))
                {

                    //to get an ilist cnn.Query<GroupPhrase>("SELECT * FROM GroupPhrases");
                    var affectedRows = cnn.Execute(sql, new { GroupPhraseTitle });
                    return true; //todo: update this to return correct value
                }
            }
            if (GroupPhraseID != null)
            {
                string sql = "DELETE FROM GroupPhrases WHERE GroupPhraseID = @GroupPhraseID;";
                using (IDbConnection cnn = new SQLiteConnection("Data Source=" + SQLiteDBLocation))
                {

                    //to get an ilist cnn.Query<GroupPhrase>("SELECT * FROM GroupPhrases");
                    var affectedRows = cnn.Execute(sql, new { GroupPhraseID });
                    return true; //todo: update this to return correct value
                }
            }
            return false;
        }

        public static void SaveGroupPhrase(GroupPhrase gp)
        {
            using (IDbConnection cnn = new SQLiteConnection("Data Source=" + SQLiteDBLocation))
            {
                cnn.Execute("Insert into GroupPhrases (GroupPhraseTitle)", gp);
            }
        }



        #endregion

        #region Phrases

        /// <summary>
        /// Get a phrase object from unique title or ID.
        /// </summary>
        /// <param name="strTitle"></param>
        /// <returns>A Phrase object</returns>
        public static Phrase GetPhrase(string PhraseTitle = null, int? PhraseID = null)
        {
            if (PhraseID != null)
            {
                string sql = "SELECT * FROM Phrases WHERE PhraseID = @PhraseID;";
                using (IDbConnection cnn = new SQLiteConnection("Data Source=" + SQLiteDBLocation))
                {

                    //to get an ilist cnn.Query<Phrase>("SELECT * FROM Phrases");
                    Phrase output = cnn.QuerySingleOrDefault<Phrase>(sql, new { PhraseID });
                    return output;
                }
            }
            if (PhraseTitle != null)
            {
                string sql = "SELECT * FROM Phrases WHERE PhraseTitle = @PhraseTitle;";
                using (IDbConnection cnn = new SQLiteConnection("Data Source=" + SQLiteDBLocation))
                {

                    //to get an ilist cnn.Query<Phrase>("SELECT * FROM Phrases");
                    Phrase output = cnn.QuerySingleOrDefault<Phrase>(sql, new { PhraseTitle });
                    return output;
                }
            }
            return null;
        }

        /// <summary>
        /// Update a phrase object
        /// </summary>
        /// <param name="strTitle"></param>
        /// <returns>True if successfull</returns>
        public static bool UpdatePhrase(Phrase p)
        {
            if (p != null)
            {
                string sql = "UPDATE Phrases SET " +
                    "PhraseTitle = @PhraseTitle, " +
                    "PhraseDefaultOption= @PhraseDefaultOption" +
                    " WHERE PhraseID = @PhraseID;";

                using (IDbConnection cnn = new SQLiteConnection("Data Source=" + SQLiteDBLocation))
                {
                    var affectedRows = cnn.Execute(sql, p);
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Add a phrase object to database
        /// </summary>
        /// <param name="gp">a phrase object</param>
        /// <returns>True if successfull</returns>
        public static bool AddPhrase(Phrase p)
        {
            if (p != null)
            {
                string sql = "INSERT INTO Phrases (" +
                    "PhraseTitle," +
                    "PhraseDefaultOption" +
                    ") VALUES (" +
                    "@PhraseTitle," +
                    "@PhraseDefaultOption)";

                using (IDbConnection cnn = new SQLiteConnection("Data Source=" + SQLiteDBLocation))
                {
                    try
                    {
                        var affectedRows = cnn.Execute(sql, p);

                    }
                    catch (Exception e)
                    {
                        if (e.Message.Contains("UNIQUE constraint failed: Phrases.PhraseTitle"))
                        {
                            MessageBox.Show("Cannot add new Phrase because the title is not unique!");
                            return false;
                        }
                        MessageBox.Show($"Cannot update due to error: ${e.Message}");
                        return false;
                    }

                }
                return true;
            }
            return false;
        }


        public static bool DeletePhrase(string PhraseTitle = null, int? PhraseID = null)
        {
            if (PhraseTitle != null)
            {
                string sql = "DELETE FROM Phrases WHERE PhraseTitle = @PhraseTitle;";
                using (IDbConnection cnn = new SQLiteConnection("Data Source=" + SQLiteDBLocation))
                {

                    //to get an ilist cnn.Query<Phrase>("SELECT * FROM Phrases");
                    var affectedRows = cnn.Execute(sql, new { PhraseTitle });
                    return true; //todo: update this to return correct value
                }
            }
            if (PhraseID != null)
            {
                string sql = "DELETE FROM Phrases WHERE PhraseID = @PhraseID;";
                using (IDbConnection cnn = new SQLiteConnection("Data Source=" + SQLiteDBLocation))
                {

                    //to get an ilist cnn.Query<Phrase>("SELECT * FROM Phrases");
                    var affectedRows = cnn.Execute(sql, new { PhraseID });
                    return true; //todo: update this to return correct value
                }
            }
            return false;
        }

        public static void SavePhrase(Phrase p)
        {
            using (IDbConnection cnn = new SQLiteConnection("Data Source=" + SQLiteDBLocation))
            {
                cnn.Execute("Insert into Phrases (PhraseTitle)", p);
            }
        }
        #endregion

        #region PhraseOptions

        /// <summary>
        /// Get a PhraseOption object from unique title or ID.
        /// </summary>
        /// <param name="strTitle"></param>
        /// <returns>A PhraseOption object</returns>
        public static PhraseOption GetPhraseOption(string PhraseOptionTitle)
        {
                string sql = "SELECT * FROM PhraseOptions WHERE PhraseOptionTitle = @PhraseOptionTitle;";
                using (IDbConnection cnn = new SQLiteConnection("Data Source=" + SQLiteDBLocation))
                {

                    //to get an ilist cnn.Query<PhraseOption>("SELECT * FROM PhraseOptions");
                    PhraseOption output = cnn.QuerySingleOrDefault<PhraseOption>(sql, new { PhraseOptionTitle });
                    return output;
                }
        }

        /// <summary>
        /// Get a PhraseOption object from unique title or ID.
        /// </summary>
        /// <param name="strTitle"></param>
        /// <returns>A PhraseOption object</returns>
        public static PhraseOption GetPhraseOption(int PhraseOptionID)
        {
                string sql = "SELECT * FROM PhraseOptions WHERE PhraseOptionID = @PhraseOptionID;";
                using (IDbConnection cnn = new SQLiteConnection("Data Source=" + SQLiteDBLocation))
                {

                    //to get an ilist cnn.Query<PhraseOption>("SELECT * FROM PhraseOptions");
                    PhraseOption output = cnn.QuerySingleOrDefault<PhraseOption>(sql, new { PhraseOptionID });
                    return output;
                }
        }

        /// <summary>
        /// Update a PhraseOption object
        /// </summary>
        /// <param name="strTitle"></param>
        /// <returns>True if successfull</returns>
        public static bool UpdatePhraseOption(PhraseOption p)
        {
            if (p != null)
            {
                string sql = "UPDATE PhraseOptions SET " +
                    "PhraseOptionTitle = @PhraseOptionTitle, " +
                    "PhraseOptionValue= @PhraseOptionValue" +
                    " WHERE PhraseOptionID = @PhraseOptionID;";

                using (IDbConnection cnn = new SQLiteConnection("Data Source=" + SQLiteDBLocation))
                {
                    var affectedRows = cnn.Execute(sql, p);
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Add a PhraseOption object to database
        /// </summary>
        /// <param name="gp">a PhraseOption object</param>
        /// <returns>True if successfull</returns>
        public static bool AddPhraseOption(PhraseOption p)
        {
            if (p != null)
            {
                string sql = "INSERT INTO PhraseOptions (" +
                    "PhraseOptionTitle," +
                    "PhraseOptionValue" +
                    ") VALUES (" +
                    "@PhraseOptionTitle," +
                    "@PhraseOptionValue)";

                using (IDbConnection cnn = new SQLiteConnection("Data Source=" + SQLiteDBLocation))
                {
                    try
                    {
                        var affectedRows = cnn.Execute(sql, p);

                    }
                    catch (Exception e)
                    {
                        if (e.Message.Contains("UNIQUE constraint failed: PhraseOptions.PhraseOptionTitle"))
                        {
                            MessageBox.Show("Cannot add new PhraseOption because the title is not unique!");
                            return false;
                        }
                        MessageBox.Show($"Cannot update due to error: ${e.Message}");
                        return false;
                    }

                }
                return true;
            }
            return false;
        }


        public static bool DeletePhraseOption(string PhraseOptionTitle = null, int? PhraseOptionID = null)
        {
            if (PhraseOptionTitle != null)
            {
                string sql = "DELETE FROM PhraseOptions WHERE PhraseOptionTitle = @PhraseOptionTitle;";
                using (IDbConnection cnn = new SQLiteConnection("Data Source=" + SQLiteDBLocation))
                {

                    //to get an ilist cnn.Query<PhraseOption>("SELECT * FROM PhraseOptions");
                    var affectedRows = cnn.Execute(sql, new { PhraseOptionTitle });
                    return true; //todo: update this to return correct value
                }
            }
            if (PhraseOptionID != null)
            {
                string sql = "DELETE FROM PhraseOptions WHERE PhraseOptionID = @PhraseOptionID;";
                using (IDbConnection cnn = new SQLiteConnection("Data Source=" + SQLiteDBLocation))
                {

                    //to get an ilist cnn.Query<PhraseOption>("SELECT * FROM PhraseOptions");
                    var affectedRows = cnn.Execute(sql, new { PhraseOptionID });
                    return true; //todo: update this to return correct value
                }
            }
            return false;
        }

        public static void SavePhraseOption(PhraseOption p)
        {
            using (IDbConnection cnn = new SQLiteConnection("Data Source=" + SQLiteDBLocation))
            {
                cnn.Execute("Insert into PhraseOptions (PhraseOptionTitle)", p);
            }
        }
        #endregion

        #region NoteDesign
        /// <summary>
        /// Get a PhraseOption object from unique title or ID.
        /// </summary>
        /// <param name="strTitle"></param>
        /// <returns>A PhraseOption object</returns>
        public static List<NoteDesign> GetNoteDesign()
        {
                string sql = "SELECT * FROM DesignTableView;";
                using (IDbConnection cnn = new SQLiteConnection("Data Source=" + SQLiteDBLocation))
                {

                    var output = cnn.Query<NoteDesign>(sql);                    
                    return output.ToList();
                }
        }
        #endregion
    }
}
