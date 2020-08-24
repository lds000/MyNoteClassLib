using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace MyNoteDataClassLib
{
    public class GroupPhrase
    {
        public GroupPhrase(string strTitle)
        {
            GroupPhraseSideType = 1;
            GroupPhraseConnectionType = 1;
            GroupPhraseTitle = strTitle;
        }

        public GroupPhrase() { }


        public int GroupPhraseID { get; set; }
        public string GroupPhraseTitle { get; set; }
        public string GroupPhraseComment { get; set; }
        public int GroupPhraseSideType { get; set; }
        public int GroupPhraseConnectionType { get; set; }
        public string GroupPhraseOSAdult { get; set; }
        public string GroupPhraseCSAdult { get; set; }
        public string GroupPhraseOSChild { get; set; }
        public string GroupPhraseCSChild { get; set; }
        public int GroupPhraseSubsectionID { get; set; }

        /// <summary>
        /// Load the relationship datamodel
        /// </summary>
        /// <returns>a list of RelGroupPhrasesPhraseModels</returns>
        public List<RelGroupPhrasesPhrase> GetRelGroupPhrasesPhrases()
        {
            string sql = $"SELECT * from RelGroupPhrasesPhrases r where r.RelGPPGroupPhraseID = {GroupPhraseID}";
            using (IDbConnection cnn = new SQLiteConnection("Data Source=" + SqlLiteDataAccess.SQLiteDBLocation))
            {
                var output = cnn.Query<RelGroupPhrasesPhrase>(sql);
                //execute sql load for phrase associated with relationship so it is loaded once and manipulate through model
                foreach (RelGroupPhrasesPhrase r in output)
                {
                    r.ParentGroupPhrase = this;
                    r.ParentPhrase = cnn.QuerySingleOrDefault<Phrase>($"SELECT * FROM Phrases WHERE PhraseID = {r.RelGPPPhraseID};");
                    r.DefaultPhraseOption = cnn.QuerySingleOrDefault<PhraseOption>($"SELECT * FROM PhraseOptions WHERE PhraseOptionID = {r.DefaultPhraseOptionID};");
                    string sql2 = $"SELECT p.PhraseTitle, o.PhraseOptionTitle, o.PhraseOptionValue FROM RelPhraseOption r INNER JOIN Phrases p ON p.PhraseID = r.RelPhraseOptionPID INNER JOIN PhraseOptions o ON o.PhraseOptionID = r.RelPhraseOptionPOID where p.PhraseID = {r.RelGPPPhraseID} order by r.RelPhraseOptionID;";
                    r.PhraseOptions = cnn.Query<PhraseOption>(sql2).ToList();
                }
                return output.ToList();
            }
        }


    }
}
